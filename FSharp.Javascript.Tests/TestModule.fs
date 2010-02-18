module TestModule

let test (quote:Microsoft.FSharp.Quotations.Expr) = 
    let typ = System.Type.GetType("TestModule, FSharp.Javascript.Tests")
    QuotationsTestHelper.testWithType typ quote

type first = {
    Prop1 : int;
    Prop2 : string;
}
with
    [<ReflectedDefinition>]
    member this.GetProp1() = this.Prop1
    [<ReflectedDefinition>]
    member this.GetProp2(x) = this.Prop2 + x

type class1(name) =
    member this.Name = name
    [<ReflectedDefinition>]
    member this.GetName() = this.Name

type class2(name, id) =
    inherit class1(name)

    member this.Id = id

[<ReflectedDefinition>]
let getProp1 (x:first) = x.Prop1

[<ReflectedDefinition>]
let testFunction x = x

[<ReflectedDefinition>]
type System.Object with
    [<ReflectedDefinition>]
    member x.GetValue() = x

type union =
| First of int
| Second of string
| Third of bool
| Fourth of int * int

[<ReflectedDefinition>]
let (|TestPattern|_|) input = 
    match input with
    | Some(a) -> Some(a)
    | _ -> None

[<ReflectedDefinition>]
let unitResult (x:int) = ()

