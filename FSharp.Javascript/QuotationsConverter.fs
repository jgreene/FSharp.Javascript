module FSharp.Javascript.QuotationsConverter

open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.ExprShape
open Microsoft.FSharp.Quotations.Patterns
open Microsoft.FSharp.Quotations.DerivedPatterns

open System.Linq.Expressions
open System.Reflection
open Microsoft.FSharp.Reflection

open FSharp.Javascript.Ast



let getOperator op =
    match op with
    | "op_Addition"    -> Some ExpressionType.Add
    | "op_Modulus"     -> Some ExpressionType.Modulo
    | "op_Division"    -> Some ExpressionType.Divide
    | "op_Subtraction" -> Some ExpressionType.Subtract
    | "op_Multiply"    -> Some ExpressionType.Multiply
    | "op_Concatenate" -> Some ExpressionType.Add
//    | "op_Equals"      -> Some ExpressionType.Equal
//    | "op_Equality"    -> Some ExpressionType.Equal
    | "op_Inequality" -> Some ExpressionType.NotEqual
    //| "op_ColonEquals" -> Some ExpressionType.
    | "op_LessThan"    -> Some ExpressionType.LessThan
    | "op_GreaterThan" -> Some ExpressionType.GreaterThan
    | "op_LessThanOrEqual" -> Some ExpressionType.LessThanOrEqual
    | "op_GreaterThanOrEqual" -> Some ExpressionType.GreaterThanOrEqual
    | "op_LessGreater" -> Some ExpressionType.NotEqual
    | _ -> None

let getFunction func =
    match func with
    | "ToString" -> Some "toString"
    | "ToLower" -> Some "toLowerCase"
    | "ToUpper" -> Some "toUpperCase"
    | _ -> None

let isBinaryOp op =
    (getOperator op).IsSome

let rewriteBody body =
    match body with
    | Block(children) -> 
        let rec mashBlocks c =
            [for b in c do yield! match b with
                                    | Block(l) -> mashBlocks l
                                    | _ -> [b]]

        let resultBlock = Block(mashBlocks children)
        resultBlock
    | _ -> body

let rewriteBodyWithReturn body =
        let result = rewriteBody body
        match result with
        | Block(h::t) -> 
            //this extra return is here to fix variable scoping in javascript.
            Return(Call(Function(Block(Return(h)::t), [], None), []))
        | _ -> Return(body)

//let rewriteBodyWithReturn body =
//        let result = rewriteBody body
//        match result with
//        | Block(h::t) -> 
//            //this extra return is here to fix variable scoping in javascript.
//            Block([Return(Call(Function(Block(Return(h)::t), [], None), []))])
//        | _ -> Block([Return(body)])

let rewriteBlockToSingleStatement node =
    match node with
    | Block(l) -> 
        let values = [for t in l do yield match t with
                                            | Assign(i,v) -> match i with
                                                                | Identifier(n,il) -> Some(n,v)
                                                                | _ -> None
                                            | _ -> None] |> List.filter(fun x -> x.IsSome) |> List.map(fun x -> x.Value) |> Map.ofList

        let result = [for t in l do yield match t with
                                          | New(x,y,z) ->
                                                let args = [for i in y 
                                                                do yield match i with
                                                                            | Identifier(n,il) when values |> Map.containsKey(n) -> (values |> Map.find n)
                                                                            | _ -> i]

                                                New(x,args,z)
                                          | BinaryOp(left,right, op) ->
                                                let lResult = match left with
                                                              | Identifier(n,il) when values |> Map.containsKey(n) -> (values |> Map.find n)
                                                              | _ -> left

                                                BinaryOp(lResult, right, op)
                                          | _ -> t]

        result |> List.head
    | _ -> node

let cleanName (n:string) = if n = null then "" else n.Replace("|", "").Replace("`1", "").Replace("`2", "").Replace("@", "")

let getMemberAccess (name:string, t:System.Type, nameSpace:string) =
    let rec loop (typ:System.Type) =
        if typ.DeclaringType = null then
            let name = if nameSpace = "" || nameSpace = null then typ.Name else nameSpace + "." + typ.Name
            Identifier(cleanName (name), false)
        else
            MemberAccess(cleanName typ.Name, loop typ.DeclaringType)

    if t = null then
        let name = if nameSpace = "" || nameSpace = null then name else nameSpace + "." + name
        Identifier(cleanName name, false)
    else
        MemberAccess(cleanName name, loop t)
        

let convertToAst quote =
    let rec traverse node =
        match node with
        | Patterns.Value(x,y) -> match x with
                                    | :? int -> Number(Some(x :?> int), None)
                                    | :? float -> Number(None, Some(x :?> float))
                                    | :? string -> String(x :?> string, '"')
                                    | :? bool -> Boolean(x :?> bool)
                                    | null -> Null
                                    | :? System.Enum -> New(Identifier(y.DeclaringType.Name + "." + y.Name + "." + x.ToString(), false), [], None)
                                    //| _ -> Identifier(y.Name, false)
                                    | _ -> failwith "invalid value match"
        | Patterns.Call(exprs, m, args) ->
            //this rewrites the arguments to make it more javascript friendly
            let getArgs args' =
                let rec loop arg lets =
                    match arg with
                    | Patterns.Let(x,y,z) ->
                        loop z (lets |> Map.add x y)
                    | Patterns.Call(a,m,cargs) ->
                        let newArgs = [for carg in cargs -> 
                                        match carg with
                                        | Var x when (lets.ContainsKey x) ->
                                            let newArg = lets |> Map.find (x)
                                            newArg
                                        | _ -> carg]

                        if a.IsSome then
                            let a' = match a.Value with
                                    | Var x when (lets.ContainsKey x) ->
                                        lets |> Map.find (x)
                                    | _ -> a.Value
                            Expr.Call(a', m, newArgs)
                        else
                            Expr.Call(m, newArgs)
                    | Patterns.Lambda(_, Patterns.Let(_,_,_)) ->
                        arg
                    | Patterns.Lambda(v,e) ->
                        Expr.Lambda(v, (loop e lets))
                    | _ -> arg

                [for a in args' -> (loop a Map.empty)]

            let args = (getArgs args)

            match m.Name with
            | n when n = "GetArray" -> 
                let left = traverse args.[0]
                let right = traverse args.[1]
                IndexAccess(left, right)

            | _ -> 

                

                let definition = Microsoft.FSharp.Quotations.Expr.TryGetReflectedDefinition(m)

                let name = getFunction m.Name
                let realName =  if name.IsSome then name.Value else cleanName m.Name

                let createTuple args' = New(Identifier("Tuple", false), args', None)

                let getArguments (def:Expr option, args':Expr list) : node list =
                    match def with
                    | Some expr ->                             
                        let rec loop exp position acc =
                            match exp with
                            | Patterns.Lambda(v,x) when v.Type.Name.Contains("Tuple") -> 
                                let rec getTuplePositions exp' pos =
                                    match exp' with
                                    | Patterns.Let(a, Patterns.TupleGet(b,c), d) -> getTuplePositions d (pos + 1)
                                    | _ -> pos

                                let lastTuplePosition = (getTuplePositions x position)

                                let arguments = [for pos in { position .. (lastTuplePosition) - 1 } -> traverse (args'.[pos])] |> List.rev
                                let tuple = createTuple arguments
                                loop x lastTuplePosition (tuple::acc)
                            | Patterns.Lambda(v,x) when v.Type.Name = "Unit" || position > (args'.Length - 1) ->
                                let args = args'
                                loop x position acc

                            | Patterns.Lambda(v,x) -> 
                                let args = args'
                                let arg = args.[position]
                                if arg.Type = v.Type then
                                    let result = traverse arg
                                    loop x (position + 1) (result::acc)
                                else
                                    loop x position acc

                            | Patterns.Let(a, Patterns.TupleGet(b,c), d) ->
                                loop d position acc
                            
                            | _ -> acc

                        loop expr 0 []
                        
                    | None -> [for a in args' -> traverse a] 

                let arguments = getArguments (definition, args)

                let getCallNode node =
                    if arguments.Length = 0 then
                        Call(node, arguments)
                    else
                        let temp = ref node

                        for a in arguments |> List.rev do
                            temp := Call(temp.Value, [a])

                        temp.Value

                match exprs with
                | Some expr ->
                    let left = traverse exprs.Value
                    getCallNode (MemberAccess(realName, left))
                | None -> 
                    let node = getMemberAccess (m.Name, m.DeclaringType, m.DeclaringType.Namespace)
                    
                    getCallNode node

        | Patterns.IfThenElse(s,b,e) ->

            
            
            let els = rewriteBodyWithReturn (traverse e)

            match s with
            | Let(x,y,z) -> 
                let body = match b with
                            | Let(p,o,i) when p.Name = x.Name && y = o ->
                                rewriteBodyWithReturn(traverse i)
                            | _ -> rewriteBodyWithReturn (traverse b)
                let result = traverse y
                let statement = traverse z
                let after = Call(Function(Block([If(statement,body,Some(els), false)]), [], None), [])
                Block([after;Assign(Identifier(x.Name, true), result)])
            | _ -> 
                let body = rewriteBodyWithReturn (traverse b)
                let statement = traverse s
                Call(Function(Block([If(statement,body,Some(els), false)]), [], None), [])
                
            
        | Patterns.Let(v, r, a) ->
            match r with
            | Let(x,y,z) -> 
                let right = traverse y
                let after = traverse a
                let right2 = traverse z
                let afterResult = match after with
                                    | Block(r::l::[]) -> r::l::[]
                                    | _ -> [after]

                let le2 = Assign(Identifier(x.Name, true), right)
                let le = Assign(Identifier(v.Name, true), right2)
                Block(afterResult@(le::le2::[]))
            | _ -> 
                let right = traverse r
                let after = traverse a
                let afterResult = match after with
                                    | Block(r::l::[]) -> r::l::[]
                                    | _ -> [after]

                let le = Assign(Identifier(v.Name, true), right)
                Block(afterResult@[le])
        | Patterns.LetRecursive(lets, e) -> 
            let functions = [for (v,l) in lets -> Assign(Identifier(cleanName v.Name, true), traverse l)]
            let after = traverse e
            Block(after::functions)

        | Patterns.Lambda(v,x) ->
            let arg = Identifier(cleanName v.Name, false)
            let body = rewriteBodyWithReturn (traverse x)
            Function(body, [arg], None)
        | Patterns.Application(l,r) ->
            let left = traverse l
            let right = traverse r
            Call(left, [right])
        | Patterns.NewArray(t,l) ->
            NewArray(Null, [for a in l do yield traverse a] |> List.rev)

        | Patterns.NewRecord(t,args) ->
            let argNames = t.GetProperties() |> Array.toList

            let ar = [for i in [0..(args.Length - 1)] do yield (traverse args.[i])] |> List.rev
            New(getMemberAccess (t.Name, t.DeclaringType, t.Namespace), ar, None)

        | Patterns.NewObject(i, args) ->
            let argNames = i.GetParameters()
            let ar = [for a in args do yield traverse a] |> List.rev
            New(getMemberAccess (i.DeclaringType.Name, i.DeclaringType.DeclaringType, i.DeclaringType.Namespace), ar, None)
        | Patterns.NewTuple(tup) ->
            let args = [for t in tup do yield traverse t] |> List.rev
            New(Identifier("Tuple", false), args, None)
        | Patterns.TupleGet(n,index) ->
            let identifier = traverse n
            MemberAccess("Item" + (index + 1).ToString(), identifier)
        | Patterns.Coerce(v,o) ->
            traverse v

        | Patterns.PropertyGet(l, i, []) ->
            if l.IsSome then
                    let left = traverse l.Value
                    Call(MemberAccess("get_" + i.Name, left), [])
            else
                Call(getMemberAccess ("get_" + i.Name, i.DeclaringType, i.DeclaringType.Namespace), [])
        | Patterns.PropertyGet(l,i,r) ->
            let left = if l.IsSome then Some(traverse l.Value) else None
            let args = [for a in r -> traverse a]
            
            Call(MemberAccess("get_" + i.Name, left.Value), args)
        | Patterns.FieldGet(l,i) ->
            if l.IsSome then
                let left = traverse l.Value
                MemberAccess(i.Name, left)
            else
                getMemberAccess (i.Name, i.DeclaringType, i.DeclaringType.Namespace)
        | Patterns.TryWith(a,b,c,d,e) -> 
            let tryBody = traverse a
            let withBody = traverse e
            let catch = Catch(Identifier(b.Name, false), rewriteBodyWithReturn withBody)
            Call(Function(Try(rewriteBodyWithReturn tryBody, Some(catch), None), [], None), [])
        | Patterns.NewUnionCase(i, h::[]) -> 
            New(getMemberAccess (i.Name, i.DeclaringType, i.DeclaringType.Namespace), [traverse h], None)
        | Patterns.NewUnionCase(i, l) -> 
            New(getMemberAccess (i.Name, i.DeclaringType, i.DeclaringType.Namespace), [for a in l do yield traverse a], None)
        | Patterns.UnionCaseTest(expr, info) ->
            let left = traverse expr
            InstanceOf(left, getMemberAccess (info.Name, info.DeclaringType, info.DeclaringType.Namespace))
        | Patterns.Sequential(l,r) ->
            let left = traverse l
            let right = traverse r
            Block([right;left])

        //potentially only index access
        
        | Patterns.TypeTest(expr, t) ->
            let left = traverse expr

            match t.Name with
            | "Int32" | "Double" -> BinaryOp(TypeOf(left), String("number", '"'), ExpressionType.Equal)
            | "String" -> BinaryOp(TypeOf(left), String("string", '"'), ExpressionType.Equal)
            | _ -> InstanceOf(left, getMemberAccess (t.Name, t.DeclaringType, t.Namespace))
            //BinaryOp(MemberAccess("constructor", left), getMemberAccess (t.Name, t.DeclaringType), ExpressionType.Equal)
        | Patterns.DefaultValue(x) ->
            match x with
            | _ when x.FullName = "null" -> Null
            | _ when x.FullName = "String" -> Null
            | _ when x.FullName = "Int32" -> Number(Some(0), None)
            | _ -> Number(Some(0), None)

        | Patterns.PropertySet(a,pi, exps,c) ->
            let a' = traverse a.Value
                
            let memberAccess = MemberAccess("set_" + pi.Name, a')

            Call(memberAccess, [traverse c])
        | Patterns.VarSet(a, v) ->
            Assign(Identifier(a.Name, false), traverse v)
        | Patterns.WhileLoop(a,b) ->
            While(traverse a, traverse b, true)
        | Patterns.FieldSet(a,pi,c) ->
            let a' = traverse a.Value
                
            let memberAccess = MemberAccess(pi.Name, a')

            Assign(memberAccess, traverse c)
        //variable name, start, end, body
        | Patterns.ForIntegerRangeLoop(v, a, b, c) ->
            let oper = BinaryOp(Identifier(v.Name, false), traverse b, ExpressionType.LessThanOrEqual)
            ForStepNode(Assign(Identifier(v.Name, true), traverse a), oper, PostfixOperator(Identifier(v.Name, false), ExpressionType.PostIncrementAssign), traverse c)
           
        | Patterns.Quote(x) ->
            traverse x     
        | ShapeVar v -> Identifier(cleanName v.Name, false)
            
        
            
        | _ -> failwith "quotation conversion failure"

    let result = traverse quote
    
    [rewriteBody result]