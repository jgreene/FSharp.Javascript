#light
module FSharp.Javascript.Parser

open JavascriptParser
open Antlr.Runtime;
open Antlr.Runtime.Tree;
open FSharp.Javascript.Ast
open System.Linq.Expressions

//let getAst input =
//    let generator = new IronJS.Compiler.AstGenerator()
//    let ast = generator.Build(input) |> ResizeArray.toList |> List.rev
//
//    convertFromIronJS ast

let getChildSafe (node:ITree) index =
    let child = node.GetChild(index)
    if child = null then failwith "Expected child"

    if child.IsNil = false && child.Type = 0 then failwith ("Unexpected" + child.Text)

    child

let rewriteIfContainsNew (node:ITree) =
    let rec loop (tempNode:ITree) =
        if tempNode = null then None else
        match tempNode.Type with
        | ES3Parser.NEW ->
            let child = getChildSafe tempNode 0
            let idNode = new CommonTree(new CommonToken(child.Type, child.Text))

            tempNode.Parent.ReplaceChildren(0,0,idNode)
            Some(tempNode)
        | ES3Parser.CALL -> None
        | ES3Parser.PAREXPR -> None
        | _ -> loop (tempNode.GetChild(0))

    loop node




let getAst input =
    
    let lexer = new ES3Lexer(new ANTLRStringStream(input))
    let parser = new ES3Parser(new CommonTokenStream(lexer))

    let program = parser.program()
    let root = program.Tree :?> ITree
    


    let rec traverse (node:ITree) =
        if node = null then Ignore else
        match node.Type with
        | ES3Parser.WITH -> 
            let left = traverse (getChildSafe node 0)
            let right = traverse (getChildSafe node 1)

            With(left,right)
        | ES3Parser.BLOCK ->
            buildBlock node
        | ES3Parser.RegularExpressionLiteral ->
            let regex = node.Text
            let lastIndex = regex.LastIndexOf('/')
            let reg = regex.Substring(1, lastIndex - 1)
            let modifiers = regex.Substring(lastIndex + 1)
            Regex(reg, modifiers)
        | ES3Parser.PAREXPR -> 
            traverse (getChildSafe node 0)
        | ES3Parser.EXPR ->
            traverse (getChildSafe node 0)
        | ES3Parser.CEXPR ->
            let nodes = (if node.ChildCount > 0 then [for i in 0..(node.ChildCount - 1) -> (traverse (node.GetChild(i)))] else [])  |> List.rev
            AssignmentBlock(nodes, false)
        | ES3Parser.OBJECT ->
            let namedProps = [for i in 0..(node.ChildCount - 1) -> 
                                                            let child = getChildSafe node i
                                                            AutoProperty((getChildSafe child 0).Text.Trim('\'', '"'), traverse (getChildSafe child 1))] |> List.rev

            New(Identifier("Object", false), [], Some(namedProps))
        | ES3Parser.NEW -> 
            New(traverse (getChildSafe node 0), [], None)
        | ES3Parser.INSTANCEOF ->
            InstanceOf(traverse (getChildSafe node 0), traverse (getChildSafe node 1))
        | ES3Parser.ARRAY ->
            let arrayElements = [for i in 0..(node.ChildCount - 1) -> 
                                                                let child = getChildSafe node i
                                                                traverse (getChildSafe child 0)] |> List.rev
            NewArray(Identifier("Array", false), arrayElements)
        | ES3Parser.FUNCTION -> 
            if node.ChildCount > 2 then
                let name = Some((getChildSafe node 0).Text)
                buildLambda (getChildSafe node 1) (getChildSafe node 2) name
            else
                buildLambda (getChildSafe node 0) (getChildSafe node 1) None
        | ES3Parser.RETURN ->
            if node.ChildCount = 0 then
                Return(Null)
            else
                Return(traverse (getChildSafe node 0))
        | ES3Parser.CALL ->
            let newNode = rewriteIfContainsNew (getChildSafe node 0)
            if newNode.IsSome then
                let childNode = getChildSafe node 1
                New(traverse (getChildSafe newNode.Value 0), (if childNode.ChildCount > 0 then [for i in 0..(childNode.ChildCount - 1) -> traverse(childNode.GetChild(i))] else []) |> List.rev, None)
            else
                let childNode = getChildSafe node 1
                Call(traverse (getChildSafe node 0), (if childNode.ChildCount > 0 then [for i in 0..(childNode.ChildCount - 1) -> traverse( childNode.GetChild(i))] else [])  |> List.rev)
        | ES3Parser.IF | ES3Parser.QUE -> buildIf node
        | ES3Parser.SWITCH -> 
            let def = ref Null
            let cases = [for i in 1..(node.ChildCount - 1) ->
                                                    let child = getChildSafe node i
                                                    if child.Type = ES3Parser.DEFAULT then
                                                        def := traverse (getChildSafe child 0)
                                                        None
                                                    else

                                                    let caseBlock = (if child.ChildCount > 0 then [for j in 1..(child.ChildCount - 1) -> traverse (getChildSafe child j)] else []) |> List.rev
                                                    if caseBlock.Length = 1 then
                                                        Some(traverse (getChildSafe child 0), caseBlock.[0])
                                                    else
                                                        Some(traverse (getChildSafe child 0), Block(caseBlock))] |> List.filter(fun x -> x.IsSome) |> List.map(fun x -> x.Value) |> List.rev

            Switch(traverse (getChildSafe node 0), def.Value, cases, null)
        | ES3Parser.THIS | ES3Parser.Identifier -> Identifier(node.Text, false)
        | ES3Parser.TRY ->
            if node.ChildCount > 2 then
                let catch = (getChildSafe node 1)
                let finallyNode = (getChildSafe node 2)
                Try(traverse (getChildSafe node 0), (if catch = null then None else Some(traverse catch)), (if finallyNode = null then None else Some(traverse finallyNode)))
            else
                let secondChild = getChildSafe node 1
                if secondChild.Type = ES3Parser.FINALLY then
                    let finallyNode = getChildSafe node 1
                    Try(traverse (getChildSafe node 0), None, (if finallyNode = null then None else Some(traverse finallyNode)))
                else 
                    let catch = (getChildSafe node 1)
                    Try(traverse (getChildSafe node 0), (if catch = null then None else Some(traverse catch)), None)
        | ES3Parser.CATCH ->
            Catch(traverse (getChildSafe node 0), traverse (getChildSafe node 1))
        | ES3Parser.FINALLY ->
            buildBlock (getChildSafe node 0)
        | ES3Parser.THROW -> 
            Throw(traverse (getChildSafe node 0))
        | ES3Parser.BYFIELD ->
            let tempNode = (getChildSafe node 0)
            let newNode = rewriteIfContainsNew tempNode
            if newNode.IsSome then
                New(traverse newNode.Value, [], None)
            else
                MemberAccess((getChildSafe node 1).Text, traverse (getChildSafe node 0))
        | ES3Parser.BYINDEX -> 
            let newNode = rewriteIfContainsNew(getChildSafe node 0)
            if newNode.IsSome then
                New(traverse newNode.Value, [], None)
            else
                IndexAccess(traverse (getChildSafe node 0), traverse (getChildSafe node 1))

        | ES3Parser.IN ->
            In(traverse (getChildSafe node 1), traverse (getChildSafe node 0))
        | ES3Parser.WHILE ->
            While(traverse (getChildSafe node 0), traverse (getChildSafe node 1), true)
        | ES3Parser.FOR | ES3Parser.FORSTEP ->
            let body = traverse (getChildSafe node 1)
            let typ = getChildSafe node 0

            if typ.Type = ES3Parser.FORSTEP then
                let init = getChildSafe typ 0
                let test = getChildSafe typ 1
                let incr = getChildSafe typ 2

                let initNode = if init.ChildCount > 0 then traverse init else Null
                let testNode = if test.ChildCount > 0 then traverse test else Boolean(true)
                let incrNode = if incr.ChildCount > 0 then traverse incr else Null

                ForStepNode(initNode, testNode, incrNode, body)
            else
                ForInNode(traverse (getChildSafe typ 0), traverse (getChildSafe typ 1), body)
            
        | ES3Parser.DO ->
            let body = traverse (getChildSafe node 0)
            let test = traverse (getChildSafe node 1)
            While(test, body, false)
        | ES3Parser.BREAK ->
            if node.ChildCount = 0 then
                BreakNode(null)
            else
                BreakNode((getChildSafe node 0).Text)
        | ES3Parser.CONTINUE ->
            if node.ChildCount = 0 then
                Continue(null)
            else
                Continue((getChildSafe node 0).Text)
//        | ES3Parser.LABELLED ->
//            let label = getChildSafe node 0
//            let target = traverse (getChildSafe node 1)
//            
//            
        | ES3Parser.DecimalLiteral ->
            if node.Text.Contains(".") || node.Text.Contains("E") then 
                Number(None, Some(System.Double.Parse(node.Text, System.Globalization.CultureInfo.InvariantCulture)))
            else
                Number(Some(System.Int32.Parse(node.Text, System.Globalization.CultureInfo.InvariantCulture)), None)
        | ES3Parser.StringLiteral ->
            String(node.Text.Substring(1, node.Text.Length - 2), node.Text.[0])
        | ES3Parser.NULL -> Null
        | ES3Parser.TRUE | ES3Parser.FALSE -> Boolean(node.Type = 5)
        | ES3Parser.VAR -> 
            let nodes = [for i in 0..(node.ChildCount - 1) ->
                                                        let assignNode = traverse (getChildSafe node i)
                                                        match assignNode with
                                                        | Assign(l,r) -> Assign((match l with
                                                                                    | Identifier(x,y) -> Identifier(x, false)
                                                                                    | t -> t), r)
                                                        | Identifier(n, isLocal) -> Identifier(n, true)
                                                        | _ -> assignNode] |> List.rev

            if nodes.Length = 1 then
                nodes.[0]
            else
                AssignmentBlock(nodes, true)
        | ES3Parser.ASSIGN ->
            let left = getChildSafe node 0
            let right = getChildSafe node 1
            Assign(traverse left, traverse right)
        | ES3Parser.ADD -> buildBinaryOp node ExpressionType.Add
        | ES3Parser.SUB -> buildBinaryOp node ExpressionType.Subtract
        | ES3Parser.MUL -> buildBinaryOp node ExpressionType.Multiply
        | ES3Parser.DIV -> buildBinaryOp node ExpressionType.Divide
        | ES3Parser.MOD -> buildBinaryOp node ExpressionType.Modulo
        | ES3Parser.ADDASS -> buildBinaryOpAssign node ExpressionType.Add
        | ES3Parser.SUBASS -> buildBinaryOpAssign node ExpressionType.Subtract
        | ES3Parser.MULASS -> buildBinaryOpAssign node ExpressionType.Multiply
        | ES3Parser.DIVASS -> buildBinaryOpAssign node ExpressionType.Divide
        | ES3Parser.MODASS -> buildBinaryOpAssign node ExpressionType.Modulo
        | ES3Parser.EQ -> buildBinaryOp node ExpressionType.Equal
        | ES3Parser.NEQ -> buildBinaryOp node ExpressionType.NotEqual
        | ES3Parser.SAME -> buildStrictCompare node ExpressionType.Equal
        | ES3Parser.NSAME -> buildStrictCompare node ExpressionType.NotEqual
        | ES3Parser.LT -> buildBinaryOp node ExpressionType.LessThan
        | ES3Parser.GT -> buildBinaryOp node ExpressionType.GreaterThan
        | ES3Parser.GTE -> buildBinaryOp node ExpressionType.GreaterThanOrEqual
        | ES3Parser.LTE -> buildBinaryOp node ExpressionType.LessThanOrEqual
        | ES3Parser.SHR -> buildBinaryOp node ExpressionType.RightShift
        | ES3Parser.SHL -> buildBinaryOp node ExpressionType.LeftShift
        | ES3Parser.SHU -> UnsignedRightShift(traverse (getChildSafe node 0), traverse (getChildSafe node 1))
        | ES3Parser.SHRASS -> buildBinaryOpAssign node ExpressionType.RightShift
        | ES3Parser.SHLASS -> buildBinaryOpAssign node ExpressionType.LeftShift
        | ES3Parser.SHUASS -> 
            Assign(traverse (getChildSafe node 0), UnsignedRightShift(traverse (getChildSafe node 0), traverse (getChildSafe node 1)))
        | ES3Parser.AND -> buildBinaryOp node ExpressionType.And
        | ES3Parser.OR -> buildBinaryOp node ExpressionType.Or
        | ES3Parser.XOR -> buildBinaryOp node ExpressionType.ExclusiveOr
        | ES3Parser.ANDASS -> buildBinaryOpAssign node ExpressionType.And
        | ES3Parser.ORASS -> buildBinaryOpAssign node ExpressionType.Or
        | ES3Parser.XORASS -> buildBinaryOpAssign node ExpressionType.ExclusiveOr
        | ES3Parser.LAND -> buildLogicalOp node ExpressionType.AndAlso
        | ES3Parser.LOR -> buildLogicalOp node ExpressionType.OrElse
        | ES3Parser.PINC -> buildIncDecOp node ExpressionType.PostIncrementAssign
        | ES3Parser.PDEC -> buildIncDecOp node ExpressionType.PostDecrementAssign
        | ES3Parser.INC -> buildIncDecOp node ExpressionType.PreIncrementAssign
        | ES3Parser.DEC -> buildIncDecOp node ExpressionType.PreDecrementAssign
        | ES3Parser.INV -> buildUnaryOp node ExpressionType.OnesComplement
        | ES3Parser.NOT -> buildUnaryOp node ExpressionType.Not
        | ES3Parser.NEG -> buildUnaryOp node ExpressionType.Negate
        | ES3Parser.POS -> buildUnaryOp node ExpressionType.UnaryPlus
        | ES3Parser.TYPEOF -> TypeOf(traverse (getChildSafe node 0))
        | ES3Parser.VOID -> Void(traverse (getChildSafe node 0))
        | ES3Parser.DELETE -> Delete(traverse (getChildSafe node 0))
        | _ -> failwith "javascript parser failed"
    and buildUnaryOp node op =
        UnaryOp(traverse (getChildSafe node 0), op)
    and buildIncDecOp node op =
        match op with
        | ExpressionType.PreIncrementAssign | ExpressionType.PreDecrementAssign ->
            Assign(traverse (getChildSafe node 0), BinaryOp(traverse (getChildSafe node 0), Number(Some(1), None), if op = ExpressionType.PreIncrementAssign then ExpressionType.Add else ExpressionType.Subtract))
        | ExpressionType.PostIncrementAssign -> PostfixOperator(traverse (getChildSafe node 0), ExpressionType.PostIncrementAssign)
        | ExpressionType.PostDecrementAssign -> PostfixOperator(traverse (getChildSafe node 0), ExpressionType.PostDecrementAssign)
        | _ -> failwith "invalid inc dec op"
    and buildLogicalOp node op =
        Logical(traverse (getChildSafe node 0), traverse (getChildSafe node 1), op)
    and buildBinaryOp node op =
        BinaryOp(traverse (getChildSafe node 0), traverse (getChildSafe node 1), op)
    and buildBinaryOpAssign node op =
        Assign(traverse (getChildSafe node 0), BinaryOp(traverse (getChildSafe node 0), traverse (getChildSafe node 1), op))
    and buildStrictCompare node op =
        StrictCompare(traverse (getChildSafe node 0), traverse (getChildSafe node 1), op)
    and buildIf ifNode =
        let elseNode = ifNode.GetChild(2)
        If(traverse (getChildSafe ifNode 0), traverse (getChildSafe ifNode 1), (if elseNode = null then None else Some(traverse elseNode)), ifNode.Type = ES3Parser.QUE)
    and buildLambda argsNode blockNode name =
        let args = (if argsNode.ChildCount > 0 then [for i in 0..(argsNode.ChildCount - 1) -> Identifier((getChildSafe argsNode i).Text, false)] else [])
        let body = buildBlock blockNode
        Function(body, args, name)
    and buildBlock blockNode =
        let nodes = (if blockNode.ChildCount > 0 then [for i in 0..(blockNode.ChildCount - 1) -> (traverse (blockNode.GetChild(i)))] else []) |> List.rev
        if nodes.Length = 1 then
            nodes.[0]
        else
            Block(nodes)
            


    if root.IsNil then
        [for i in 0..(root.ChildCount - 1) do yield (traverse (root.GetChild(i)))] |> List.rev
    else
        [traverse root]