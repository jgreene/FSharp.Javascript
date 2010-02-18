#light
module FSharp.Javascript.Printer

open FSharp.Javascript.Ast
open System.Linq.Expressions

let getOp (op:System.Linq.Expressions.ExpressionType) =
    match op with
    | ExpressionType.Add -> "+"
    | ExpressionType.And -> "&"
    | ExpressionType.AndAlso -> "&&"
    | ExpressionType.Subtract -> "-"
    | ExpressionType.Multiply -> "*"
    | ExpressionType.Divide -> "/"
    | ExpressionType.Modulo -> "%"
    | ExpressionType.Equal -> "=="
    | ExpressionType.NotEqual -> "!="
    | ExpressionType.LessThan -> "<"
    | ExpressionType.GreaterThan -> ">"
    | ExpressionType.GreaterThanOrEqual -> ">="
    | ExpressionType.LessThanOrEqual -> "<="
    | ExpressionType.Or -> "|"
    | ExpressionType.ExclusiveOr -> "^"
    | ExpressionType.LeftShift -> "<<"
    | ExpressionType.RightShift -> ">>"
    | ExpressionType.OrElse -> "||"
    | ExpressionType.PostIncrementAssign -> "++"
    | ExpressionType.PostDecrementAssign -> "--"
    | ExpressionType.PreIncrementAssign -> "++"
    | ExpressionType.PreDecrementAssign -> "--"
    | ExpressionType.Negate -> "-"
    | ExpressionType.Not -> "!"
    | ExpressionType.OnesComplement -> "~"
    | ExpressionType.UnaryPlus -> "+"
    | _ -> failwith "unsuported operator"

let getStrictOp (op:ExpressionType) =
    match op with
    | ExpressionType.Equal -> "==="
    | ExpressionType.NotEqual -> "!=="
    | _ -> failwith "unsupported strict operator"

let getTab indent = ({0..indent} |> Seq.filter (fun x -> x <> 0) |> Seq.map (fun x -> "    ") |> String.concat "")

let trim (x:string) =
    x.Trim([|' '; '\n'; '\t'; '\r'; ','|])


let getJavascript ast =
    let getLine = System.Environment.NewLine
    let rec getBody (nodes:node list) char indent =
        let getIndent = getTab (indent + 1)
        let getDedent = getTab (indent - 1)
        let result = [for n in nodes do yield! getLine::char::(traverse n [] (indent + 1))@[getIndent]]
        result
    and traverse (node:node) acc indent =
        let getIndent = getTab (indent)
        let getDedent = getTab (indent - 1)
        match node with
        | Ignore -> acc
        | Assign(l,r) -> 
            let left = traverse l [] indent
            let right = traverse r [] indent
            right@(" = "::left)@(acc)
        | AutoProperty(n,v) ->
            let value = traverse v [] indent
            value@("\"" + n.ToString() + "\"" + " : "::acc)
        | BinaryOp(l,r,o) ->
            let left = traverse l [] indent
            let right = traverse r [] indent
            let op = getOp o
            "))"::right@("("::" " + op + " "::")"::left)@("(("::acc)
        | Identifier(n,l) -> if l then "var " + n::acc else n::acc
        | Number(v) -> ")"::(v.ToString())::"("::acc
        | Function(b,args, n) ->
            let body = traverse b [] indent
            let arguments = [for a in args do yield! (traverse a [] 0)] |> String.concat ","
            
            if n.IsSome then
                getIndent + "}"::body@(getLine::"){"::arguments::"function " + n.Value + "("::acc)
            else
                getIndent + "}"::body@(getLine::"){"::arguments::"function("::acc)
        | Block(nodes) ->
            
            let body = (getBody nodes ";" indent) |> List.rev |> String.concat ""
            body::acc
        | AssignmentBlock(nodes, l) -> 
            let result = ((getBody nodes "," indent) |> List.rev |> String.concat "") |> trim
            
            //is local
            if l then
                result::"var "::acc
            //is global
            else
                result::acc
        | Boolean(b) ->
            b.ToString().ToLower()::acc
        | BreakNode(l) ->
            "break"::acc
        | ForInNode(l,r,b) ->
            let left = traverse l [] 0
            let right = traverse r [] 0
            let body = traverse b [] indent

            getIndent + "}"::body@("){" + getLine::right)@(" in "::left)@("for("::acc)
        | ForStepNode(s,t,i,b) ->
            let step = ((traverse s [] 0) |> List.rev |> String.concat "").Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(";", "")
            let test = traverse t [] 0
            let inc = traverse i [] 0
            let body = traverse b [] indent

            getIndent + "}"::body@("){" + getLine::inc)@("; "::test)@("; "::step::"for("::acc)
        | MemberAccess(name,node) ->
            let n = traverse node [] 0
            name::"."::n@acc
        | Call(n,args) ->
            let node = match n with
                        | Function(_,_,_) -> ")"::(traverse n [] 0)@("("::[])
                        | _ -> traverse n [] 0
            let arguments = ([for a in args do yield! ","::(traverse a [] 0)] |> List.rev |> String.concat "").Trim([|','|])
            ")"::arguments::"("::node@acc
        | Catch(t,b) ->
            let target = traverse t [] 0
            let body = traverse b [] indent
            getIndent + "}"::body@("){" + getLine::target)@(getLine + getIndent + "catch("::acc)
        | Continue(l) -> "continue"::acc
        | Delete(n) ->
            let node = traverse n [] 0
            node@("delete "::acc)
        | Try(b,c,f) ->
            let body = traverse b [] indent
            let catch = if c.IsSome then traverse c.Value [] indent else []
            let fin = if f.IsSome then getIndent + "}"::(traverse f.Value (getLine + getIndent + "finally{" + getLine::[]) indent) else []
            
            fin@catch@(getIndent + "}"::body)@("try {" + getLine::acc)
        | New(t,args,props) ->
            if props.IsSome then
                let properties = [for p in props.Value do yield! "," + getLine + getTab (indent + 1)::(traverse p [] (indent + 1))] |> List.rev |> String.concat "" |> trim
                getIndent + "}"::getLine::properties::("{" + getLine + getTab (indent + 1)::acc)
            else
                let targ = traverse t [] 0
                let args = ([for a in args do yield! ","::(traverse a [] 0)] |> List.rev |> String.concat "").Trim([|','|])
                ")"::args::("("::targ)@("new "::acc)
        | NewArray(t,args) ->
            let target = traverse t [] 0
            let arguments = ([for a in args do yield! ","::(traverse a [] indent)] |> List.rev |> String.concat "").Trim([|','|])
            "]"::arguments::("["::acc)
        | If(t,b,e,it) ->
            let test = traverse t [] 0
            let trueBranch = traverse b [] indent
            let elseBranch = if e.IsSome then traverse e.Value [] indent else []

            //ternary op
            if it then
                "))"::elseBranch@(") : ("::trueBranch)@(") ? ("::test)@("(("::acc)
            else
                let result = (getIndent + "}"::trueBranch)@("){" + getLine::test)@("if("::acc)
                if e.IsNone then
                    result
                else
                    match e.Value with
                    // else if
                    | If(_,_,_,_) -> elseBranch@("else "::getLine + getIndent::result)
                    | _ -> getIndent + "}"::elseBranch@("else{" + getLine::result)
        | IndexAccess(t,i) ->
            let target = traverse t [] 0
            let index = traverse i [] 0
            match t with
            | Assign(l,r) -> 
                "]"::index@("["::")"::target)@("("::acc)
            | _ ->
                "]"::index@("["::target)@(acc)
        | In(t,p) ->
            let target = traverse t [] 0
            let prop = traverse p [] 0
            target@(" in "::prop)@acc
        | InstanceOf(t,f) ->
            let target = traverse t [] 0
            let func = traverse f [] 0
            func@(" instanceof "::target)@acc
        | Logical(l,r,o) ->
            let left = traverse l [] 0
            let right = traverse r [] 0
            let op = getOp o
            "))"::right@("("::" " + op + " "::")"::left)@("(("::acc)
        | Null -> "null"::acc
        | PostfixOperator(t,o) ->
            let target = traverse t [] 0
            let op = getOp o
            op::target@acc
        | Regex(r,m) -> "/" + r + "/" + m::acc
        | Return(v) -> (traverse v [] 0)@("return "::acc)
        | StrictCompare(l,r,o) ->
            let left = traverse l [] 0
            let right = traverse r [] 0
            let op = getStrictOp o
            "))"::right@("("::[])@(" " + op + " "::")"::left)@("(("::acc)
        | String(s,c) -> c.ToString() + s + c.ToString()::acc
        | Switch(t,d,c,label) ->
            let target = traverse t [] 0
            let def = ";"::(traverse d [] (indent + 1))
            let cases = [for (l,r) in c do yield! (traverse r [] (indent + 1))@(":" + getLine::(traverse l (getTab (indent + 1) + "case "::[]) (indent + 1)))]
            getLine + getIndent + "}"::def@(getTab (indent + 1) + "default:" + getLine + (getTab (indent + 2))::cases)@("){" + getLine::target)@("switch("::acc)
        | Throw(b) -> (traverse b [] 0)@("throw "::acc)
        | TypeOf(n) -> (traverse n [] 0)@("typeof "::acc)
        | UnaryOp(t,o) ->
            let target = traverse t [] 0
            let op = getOp o
            ")"::target@("("::op::acc)
        | UnsignedRightShift(l,r) ->
            let left = traverse l [] 0
            let right = traverse r [] 0
            right@(" >>> "::left)@acc
        | Void(n) ->
            let node = traverse n [] 0
            ")"::node@("void("::acc)
        | While(t,b,l) ->
            let test = traverse t [] 0
            let body = traverse b [] indent
            //while loop
            if l then
                getIndent + "}"::body@("){" + getLine::test)@("while("::acc)
            //do while    
            else
                ")"::test@(getIndent + "while("::getIndent + "}"::body)@("do{" + getLine::acc)
        | With(t,b) ->
            let target = traverse t [] 0
            let body = traverse b [] indent
            getIndent + "}"::body@("){" + getLine::target)@("with("::acc)
        | _ -> failwith "print matching failed"

    let result = [for a in ast do yield! ";" + System.Environment.NewLine::(traverse a [] 0)]
    result |> List.rev |> String.concat ""