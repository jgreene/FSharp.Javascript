#light
module FSharp.Javascript.ModuleCompiler

open FSharp.Javascript.Ast
open FSharp.Javascript.QuotationsConverter

open System
open System.Reflection
open Microsoft.FSharp.Reflection



let getDefaultValue (t:PropertyInfo) =
    match t.ReflectedType with
    | x when x = typeof<int> -> Number(Some(0), None)
    | x when x = typeof<float> -> Number(None, Some(float 0))
    | x when x = typeof<string> -> Ast.String("", '"')
    | _ -> Null

let camelCase (input:string) =
    if input.Length = 0 then
        input
    else
        let first = (input.Substring(0, 1)).ToUpper()
        first + (input.Substring(1, input.Length - 1))


let getName (m:System.Reflection.MethodInfo) =
    let rec loop (typ:System.Type) acc =
        if typ.DeclaringType = null then
            acc
        else
            loop typ.DeclaringType (typ.Name.Replace("|", "")::acc)

    (loop m.DeclaringType (m.Name::[])) |> String.concat "."

let getBaseType (startingType:System.Type) =
                let rec innerGet (typ:System.Type) = 
                    if typ.BaseType = null || typ.BaseType.Name = "Object"
                    then typ
                    else
                        innerGet typ.BaseType

                innerGet startingType

let getEqualityFunction (parameters:string list) =

    let initialStatement = Assign(Identifier("result", true), Ast.Boolean(true))
    let getBlock (p:string) = Assign(Identifier("result", false), 
                                        BinaryOp(Identifier("result", false), 
                                            Call(Call(Identifier("Operators.op_Equality", false),
                                                [MemberAccess(camelCase(p), Identifier("this", false))]),
                                                    [MemberAccess(camelCase(p), Identifier("compareTo", false))]), 
                                                        System.Linq.Expressions.ExpressionType.AndAlso))


    let blockStatements = parameters |> List.map getBlock |> List.rev
    
    
    let statements = match blockStatements with
                     | [] -> blockStatements
                     | h::[] -> match h with
                                | Assign(x,r) -> Return(r)::[]
                                | _ -> Return(h)::[]
                     | h::t -> 
                                Return(Identifier("result", false))::h::t



    (Function(Block(statements@[initialStatement]), [Identifier("compareTo", false)], None))

//let createPrototype name:string node:node =
//     Assign(MemberAccess("prototype", MemberAccess(name

let getInheritance (t:Type) =
    let baseType = getBaseType t
    let inherits = if baseType = t then [] else [Assign(MemberAccess("prototype", getMemberAccess (t.Name, t.DeclaringType)), MemberAccess("prototype", getMemberAccess (baseType.Name, t.DeclaringType)))]
    inherits

let getAstFromType (mo:System.Type) =
    let rec loop (t:Type) acc =

//        let assem = Assembly.GetAssembly(t)
//        assem.GetReferencedAssemblies() |> Array.tryFind(fun x -> x.Name = "FSharp.Core")
        
        let childResults = [for ty in t.GetNestedTypes() do yield! loop ty []]
        let quotesAndMethods = [for m in t.GetMethods() ->
                                                            (m, Microsoft.FSharp.Quotations.Expr.TryGetReflectedDefinition(m))]
                                                            |> List.filter(fun (x,y) -> y.IsSome) |> List.map(fun (x,y) -> (x,y.Value))

        if FSharpType.IsModule t then
            let getResult (m:MethodInfo,q:Quotations.Expr) = match QuotationsConverter.convertToAst q |> List.head with
                                                            //extension method support
                                                            | Function(b,args,name) when m.Name.Contains(".") ->
                                                                let split = m.Name.Split('.')
                                                                let func = match b with
                                                                            | Return x -> match x with
                                                                                            | Function(t,y,z) -> Function(t, args, None)
                                                                                            | _ -> x
                                                                            | _ -> Function(b,args,None)
                                                                Some(Block([
                                                                            Assign(MemberAccess(split.[1], MemberAccess(split.[0], Identifier(t.Name, false))), func);
                                                                            Assign(MemberAccess(split.[0], Identifier(t.Name, false)), Function(Block([]), [], None))
                                                                ]))
                                                            //active pattern support
                                                            | Function(b,args,name) when m.Name.Contains("|") ->
                                                                let name = m.Name.Replace("|", "")
                                                                let func = Assign(MemberAccess(name, Identifier(t.Name, false)), Function(b,args,None))
                                                                Some(func)
                                                            | Function(b,args,name) -> 
                                                                Some(Assign(MemberAccess(m.Name, Identifier(t.Name, false)), Function(b,args, None)))
                                                            | _ -> None
            
            let result = quotesAndMethods |> List.map getResult |> List.filter(fun x -> x.IsSome) |> List.map(fun x-> x.Value)

            let props = [for p in t.GetProperties() do yield (p, (Microsoft.FSharp.Quotations.Expr.TryGetReflectedDefinition(p.GetGetMethod())))] 
                                                |> List.filter(fun (p,x) -> x.IsSome) 
                                                |> List.map(fun (p,x) -> (p,(QuotationsConverter.convertToAst x.Value).Head))
                                                |> List.map(fun (p,x) -> Assign(MemberAccess(p.Name, Identifier(t.Name, false)), x))
            
            let d = Assign(Identifier(t.Name, true), New(Null, [], Some([])))
            [d; Block((childResults@Block(result@props)::acc) |> List.rev)]

        elif FSharpType.IsUnion t then
            let cases = FSharpType.GetUnionCases t
            let rdr = [for c in cases do yield FSharpValue.PreComputeUnionConstructorInfo c]
            let rd = [for r in rdr do yield (r,r.GetParameters())]

            let func = [for (r,parameters) in rd do yield! 
                                                        let values = [for p in parameters do yield (Identifier(p.Name, false), Assign(MemberAccess(camelCase(p.Name), Identifier("this", false)), Identifier(p.Name,false)))] in
                                                        let construct = Assign(MemberAccess(r.Name.Replace("New",""), getMemberAccess(t.Name, t.DeclaringType)), 
                                                                                Function(Block( [for (par,prop) in values do yield prop]), [for (par,prop) in values do yield par], None)) in

                                                        let inheritance = Assign(MemberAccess("prototype", MemberAccess(r.Name.Replace("New",""), getMemberAccess(t.Name, t.DeclaringType))), 
                                                                                 MemberAccess("prototype", getMemberAccess(t.Name, t.DeclaringType))) in

                                                        let equals = Assign(MemberAccess("Equality", MemberAccess("prototype", MemberAccess(r.Name.Replace("New",""), getMemberAccess(t.Name, t.DeclaringType)))),
                                                                            getEqualityFunction (parameters |> Array.map (fun p -> p.Name) |> Array.toList) ) in
                                                        [equals;inheritance;construct]
                                                        ]

            
            
            //let inherits = getInheritance t


            Assign(getMemberAccess (t.Name, t.DeclaringType), Function(Block([]), [], None))::[Block(func)]
        
        elif t.BaseType = typeof<Enum> then
            let values = Enum.GetValues(t) :?> int[]
            let enums = values |> Array.map (fun v -> (Enum.GetName(t,v), v)) |> Array.toList

            let func = [for (n,i) in enums do yield!
                                                let construct = Assign(MemberAccess(n, getMemberAccess(t.Name, t.DeclaringType)),
                                                                    Function(Block([Assign(MemberAccess("Text", Identifier("this", false)), Ast.String(n,'"'));
                                                                                    Assign(MemberAccess("Integer", Identifier("this", false)), Number(Some(i), None))]), [], None)) in

                                                let inheritance = Assign(MemberAccess("prototype", MemberAccess(n, getMemberAccess(t.Name, t.DeclaringType))), 
                                                                                             MemberAccess("prototype", getMemberAccess(t.Name, t.DeclaringType))) in
                                                [inheritance;construct]]

            let inherits = Assign(MemberAccess("prototype", getMemberAccess (t.Name, t.DeclaringType)), MemberAccess("prototype", Identifier("Enum", false)))
            
            Assign(getMemberAccess (t.Name, t.DeclaringType), Function(Block([]), [], None))::inherits::[Block(func)]
        else
            let properties = t.GetProperties() |> Array.toList
            let constructors = t.GetConstructors() |> Array.toList
            let construct = if constructors.Length > 0 then Some(constructors.Head) else None
            let parameters = if construct.IsSome then [for p in construct.Value.GetParameters() do yield Identifier(p.Name, false)] else []

            let members = [for p in properties do yield
                                                    Assign(MemberAccess(p.Name, Identifier("this", false)), 
                                                    let d = [for r in parameters do yield match r with
                                                                                            | Identifier(n,l) when n.ToLower() = p.Name.ToLower() -> Some(Identifier(n,l))
                                                                                            | _ -> None] |> List.filter(fun i -> i.IsSome)
                                                    match d with
                                                    | h::[] when h.IsSome -> h.Value
                                                    | _ -> getDefaultValue p

                                                    )]


            let func = Assign(getMemberAccess(t.Name, t.DeclaringType), Function(Block(members),parameters, None))

            let baseType = getBaseType t
            let inherits = if baseType = t then [] else [Assign(MemberAccess("prototype", getMemberAccess (t.Name, t.DeclaringType)), MemberAccess("prototype", getMemberAccess (baseType.Name, t.DeclaringType)))]

            let q = [for (m,q) in quotesAndMethods do yield 
                                                        Assign(MemberAccess(m.Name, MemberAccess("prototype", getMemberAccess (t.Name, t.DeclaringType))), 
                                                        let t = (QuotationsConverter.convertToAst q) |> List.head
                                                        match t with
                                                        | Function(n,args,props) -> match n with
                                                                                    | Return(x) -> x
                                                                                    | _ -> t
                                                        | _ -> t)
                                                        ]

            let equality =
                if FSharpType.IsRecord t then
                    let propertyNames = (properties |> List.map (fun x -> x.Name))
                    let equalityFunction = getEqualityFunction propertyNames
                    let result = Assign(MemberAccess("Equality", MemberAccess("prototype", getMemberAccess(t.Name, t.DeclaringType))), equalityFunction)
                    [result]
                else
                    []
                

            (func::acc@inherits@equality@q)@childResults


    (loop mo []) |> List.rev
