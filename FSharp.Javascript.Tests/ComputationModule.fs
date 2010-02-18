module ComputationModule

let test (quote:Microsoft.FSharp.Quotations.Expr) = 
    let typ = System.Type.GetType("ComputationModule, FSharp.Javascript.Tests")
    QuotationsTestHelper.testWithType typ quote

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