module TestModule

let test (quote:Microsoft.FSharp.Quotations.Expr) = 
    let typ = System.Type.GetType("TestModule, FSharp.Javascript.Tests")
    QuotationsTestHelper.testWithType typ quote


type record = { x:int; y:int; }

type first = {
    Prop1 : int;
    Prop2 : string;
}
with
    [<ReflectedDefinition>]
    member this.GetProp1() = this.Prop1
    [<ReflectedDefinition>]
    member this.GetProp2(x) = this.Prop2 + x

    [<ReflectedDefinition>]
    member this.MultipleArgsOnRecord(a,b) = a - b

type class1(name) =
    member this.Name = name
    [<ReflectedDefinition>]
    member this.GetName() = this.Name


type class2(name, id) =
    inherit class1(name)

    [<ReflectedDefinition>]
    let mutable age = 0

    member this.Id = id
    member this.Age
        with get() = age
        and set(value) = age <- value

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
let (|NullCheckPattern|_|) x =
    match x with
    | null -> None
    | _ -> Some(x)

[<ReflectedDefinition>]
let unitResult (x:int) = ()

[<ReflectedDefinition>]
let (><) a b = a = b

[<ReflectedDefinition>]
let multipleArgs a b = a - b

[<ReflectedDefinition>]
let tupledArgs (a,b) (c,d) = a + b + c + d

[<ReflectedDefinition>]
let mixedTupledArgs a (c,d) = a + c + d

type Color =
| Red = 0
| Green = 1
| Blue = 2