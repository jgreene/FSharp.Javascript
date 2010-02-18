module ComputationModule

let test (quote:Microsoft.FSharp.Quotations.Expr) = 
    let typ = System.Type.GetType("ComputationModule, IronJS.Printer.Tests")
    QuotationsTestHelper.testWithType typ quote

//type CompBuilder() =
//    [<ReflectedDefinition>]
//    member x.Bind(t,f) = f t
//    [<ReflectedDefinition>]
//    member x.Return(y) = y
//
//[<ReflectedDefinition>]
//let comp = new CompBuilder()

type MaybeBuilder() =
    [<ReflectedDefinition>]
    member this.Bind(x, f) =
        match x with
        | Some(t) when t >= 0 && t <= 100 -> f(t)
        | _ -> None
    [<ReflectedDefinition>]
    member this.Delay(f) = f()
    [<ReflectedDefinition>]
    member this.Return(x) = Some x

[<ReflectedDefinition>]
let maybe = new MaybeBuilder()