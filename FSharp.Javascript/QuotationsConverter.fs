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
    | Block(h::t) -> Block(Return(h)::t)
    | _ -> Return(body)

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

let getMemberAccess (name:string, t:System.Type) =
    let rec loop (typ:System.Type) =
        if typ.DeclaringType = null then
           Identifier(cleanName typ.Name, false)
        else
           MemberAccess(cleanName typ.Name, loop typ.DeclaringType)

    if t = null then
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
                                    | :? System.Enum -> Identifier(y.Name + "." + x.ToString(), false)
                                    //| _ -> Identifier(y.Name, false)
                                    | _ -> failwith "invalid value match"
        | Patterns.Call(exprs, m, args) ->
            match m.Name with
            | n when isBinaryOp n ->
                let left = traverse args.[0]
                let right = traverse args.[1]
                let op = getOperator m.Name
                BinaryOp(left, right, op.Value)
            | n when n = "GetArray" -> 
                let left = traverse args.[0]
                let right = traverse args.[1]
                IndexAccess(left, right)
            | n when n = "op_Range" ->
                let left = traverse args.[0]
                let right = traverse args.[1]
                New(Identifier("Range", false), right::left::[], None)
            | n when n.StartsWith "get_" ->
                let arguments = [for a in args do yield traverse a] |> List.head

                MemberAccess(m.Name.Replace("get_", ""), arguments)

            | _ -> 
                let isLambda ex =
                    match ex with
                    | Lambda(v,e) -> true
                    | _ -> false

                //this rewrites the arguments to make it more javascript friendly
                let args = [for a in args do yield match a with
                                                        | Patterns.Let(x,y,z) when isLambda y && isLambda z -> 
                                                            let result = match z with
                                                                            | Lambda(v, e) ->
                                                                                match e with
                                                                                | Microsoft.FSharp.Quotations.Patterns.Call(p, i,h::t::[]) -> 
                                                                                    if p.IsSome then
                                                                                        Expr.Lambda(v, Expr.Call(p.Value, i, y::t::[]))
                                                                                    else
                                                                                        Expr.Lambda(v, Expr.Call(i, y::t::[]))
                                                                                | _ -> e
                                                                            | _ -> z
                                                            result
                                                         | Patterns.Let(x,y,z) -> match z with
                                                                                    | Patterns.Call(p,i,ar) -> 
                                                                                        if p.IsSome then
                                                                                            Expr.Call(y, i, ar)
                                                                                        else
                                                                                            Expr.Call(i, ar)
                                                                                    | _ -> a
                                                         | _ -> a]

                                                            
                let arguments = [for a in args do yield traverse a] |> List.rev

                let isOperator = arguments.Length = 2 && m.Name.StartsWith("op_")

                let tuple = if arguments.Length > 1 && m.DeclaringType.Name.EndsWith("Builder") && isOperator = false then Some(New(Identifier("Tuple", false), arguments, None)) else None

                //let parameters = m.GetParameters() |> Array.toList
                
                
                let name = getFunction m.Name
                let realName = if name.IsSome then cleanName name.Value else cleanName m.Name
                
                if exprs.IsSome then
                    let left = traverse exprs.Value
                    if tuple.IsSome then
                        Call(MemberAccess(realName, left), [tuple.Value])
                    else
                        Call(MemberAccess(realName, left), arguments)
                else
                    let node = getMemberAccess (m.Name, m.DeclaringType)

                    let call = if tuple.IsSome then 
                                    Call(node, [tuple.Value])
                               elif isOperator then
                                    Call(Call(node, [arguments.[0]]), [arguments.[1]])
                               else
                                    Call(node, arguments)
                    call
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
            New(getMemberAccess (t.Name, t.DeclaringType), ar, None)

        | Patterns.NewObject(i, args) ->
            let argNames = i.GetParameters()
            let ar = [for a in args do yield traverse a] |> List.rev
            New(getMemberAccess (i.DeclaringType.Name, i.DeclaringType.DeclaringType), ar, None)
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
                MemberAccess(i.Name, left)
            else
                getMemberAccess (i.Name, i.DeclaringType)
        | Patterns.FieldGet(l,i) ->
            let left = traverse l.Value
            MemberAccess(i.Name, left)
        | Patterns.FieldGet(l,i) ->
            let left = traverse l.Value
            MemberAccess(i.Name, left)
        | Patterns.TryWith(a,b,c,d,e) -> 
            let tryBody = traverse a
            let withBody = traverse e
            let catch = Catch(Identifier(b.Name, false), rewriteBodyWithReturn withBody)
            Call(Function(Try(rewriteBodyWithReturn tryBody, Some(catch), None), [], None), [])
        | Patterns.NewUnionCase(i, h::[]) -> 
            New(getMemberAccess (i.Name, i.DeclaringType), [traverse h], None)
        | Patterns.NewUnionCase(i, l) -> 
            New(getMemberAccess (i.Name, i.DeclaringType), [for a in l do yield traverse a], None)
        | Patterns.UnionCaseTest(expr, info) ->
            let left = traverse expr
            InstanceOf(left, getMemberAccess (info.Name, info.DeclaringType))
        | Patterns.Sequential(l,r) ->
            let left = traverse l
            let right = traverse r
            Block([right;left])

        | Patterns.Coerce(n,t) ->
            let node = traverse n
            node

        //potentially only index access
        | Patterns.PropertyGet(l,i,r) ->
            let left = if l.IsSome then Some(traverse l.Value) else None
            let args = [for a in r -> traverse a]
            
            Call(MemberAccess(i.Name, left.Value), args)
        | Patterns.TypeTest(expr, t) ->
            let left = traverse expr

            match t.Name with
            | "Int32" | "Double" -> BinaryOp(TypeOf(left), String("number", '"'), ExpressionType.Equal)
            | "String" -> BinaryOp(TypeOf(left), String("string", '"'), ExpressionType.Equal)
            | _ -> InstanceOf(left, getMemberAccess (t.Name, t.DeclaringType))
            //BinaryOp(MemberAccess("constructor", left), getMemberAccess (t.Name, t.DeclaringType), ExpressionType.Equal)
        | Patterns.DefaultValue(x) ->
            match x with
            | _ when x.FullName = "null" -> Null
            | _ when x.FullName = "String" -> Null
            | _ when x.FullName = "Int32" -> Number(Some(0), None)
            | _ -> Number(Some(0), None)
        | ShapeVar v -> Identifier(cleanName v.Name, false)
            
        | _ -> failwith "quotation conversion failure"

    let result = traverse quote
    
    [rewriteBody result]