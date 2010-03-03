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


let getAstFromType (mo:System.Type) =
    let rec loop (t:Type) acc =
        //let types = t.GetNestedTypes(System.Reflection.BindingFlags.Public ||| System.Reflection.BindingFlags.NonPublic)
        
        let childResults = [for ty in t.GetNestedTypes() do yield! loop ty []]
        let quotesAndMethods = [for m in t.GetMethods() ->
                                                            (m, Microsoft.FSharp.Quotations.Expr.TryGetReflectedDefinition(m))]
                                                            |> List.filter(fun (x,y) -> y.IsSome) |> List.map(fun (x,y) -> (x,y.Value))

        if FSharpType.IsModule t then
            let result = [for (m,q) in quotesAndMethods do yield match ((QuotationsConverter.convertToAst q) |> List.head) with
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
                                                                        //let arguments = Assign(MemberAccess("constructor", Identifier("this", false)), 
                                                                        let func = Assign(MemberAccess(name, Identifier(t.Name, false)), Function(b,args,None))
                                                                        Some(func)
                                                                    | Function(b,args,name) -> 
                                                                        Some(Assign(MemberAccess(m.Name, Identifier(t.Name, false)), Function(b,args, None)))
                                                                    | _ -> None] |> List.filter(fun x -> x.IsSome) |> List.map(fun x-> x.Value)

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

            let func = [for (r,parameters) in rd do yield 
                                                        let values = [for p in parameters do yield (Identifier(p.Name, false), Assign(MemberAccess(camelCase(p.Name), Identifier("this", false)), Identifier(p.Name,false)))] in
                                                        
                                                        Block([Assign(MemberAccess("prototype", MemberAccess(r.Name.Replace("New",""), getMemberAccess(t.Name, t.DeclaringType))), 
                                                                        MemberAccess("prototype", getMemberAccess(t.Name, t.DeclaringType)));
                                                                Assign(MemberAccess(r.Name.Replace("New",""), getMemberAccess(t.Name, t.DeclaringType)), 
                                                                Function(Block( [for (par,prop) in values do yield prop]), [for (par,prop) in values do yield par], None))
                                                               ])]

            
            
            let baseType = getBaseType t
            let inherits = if baseType = t then [] else [Assign(MemberAccess("prototype", getMemberAccess (t.Name, t.DeclaringType)), MemberAccess("prototype", getMemberAccess (baseType.Name, t.DeclaringType)))]


            Assign(getMemberAccess (t.Name, t.DeclaringType), Function(Block([]), [], None))::func
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

            (func::acc@inherits@q)@childResults


    (loop mo []) |> List.rev
