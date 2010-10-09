#light
namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestModule
open QuotationsTestHelper

[<TestFixture>]
type QuotationsTests() =
    
    [<Test>]
    member this.``Addition``() =
        test <@ (1 + 1) @>

    [<Test>]
    member this.``Subtraction``() =
        test <@ (1 - 1) @>

    [<Test>]
    member this.``Multiplication``() =
        test <@ (2 * 2) @>

    [<Test>]
    member this.``Function declaration with call``() =
        test <@ let f x = x + 1
                (f 2) @>

    [<Test>]
    member this.``Multi line function with call``() =
        test <@ let f x =
                    let y = 1
                    x + y
                (f 2) @>

    [<Test>]
    member this.``Function with multiple arguments``() =
        test <@ let f x y = x + y
                (f 1 2) @>

    [<Test>]
    member this.``Anonymous function``() =
        test <@ let f (x:unit-> int) = x()
                (f (fun () -> 1 + 1)) @>

    [<Test>]
    member this.``Anonymous function with arg``() =
        test <@ let f (x:int->int) = x(1)
                (f (fun x -> x + 1)) @>


    [<Test>]
    member this.``Array creation and access``() =
        test <@ let list = [|"one";"two";"three"|]
                (list.[0]) @>

    [<Test>]
    member this.``Record declaration and property access``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                (t.Prop1) @>

    [<Test>]
    member this.``Record with custom method call``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                (t.GetProp1()) @>

    [<Test>]
    member this.``Record with custom method call with argument``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                (t.GetProp2(" neato")) @>

    [<Test>]
    member this.``Module function that takes record as argument``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                (getProp1(t)) @>

    [<Test>]
    member this.``Module function with one argument``() =
        test <@ (testFunction 1) @>

    [<Test>]
    member this.``Class inheritance``() =
        test <@ let c = new class2("bob", 1)
                (c.GetName()) @>

    [<Test>]
    member this.``ExtensionMethod``() =
        test <@ let a = 1
                (a.GetValue()) @>

    [<Test>]
    member this.``ToString``() =
        test <@  let a = 1
                 (a.ToString()) @>

    [<Test>]
    member this.``ToString on constant``() =
        test <@ ((1).ToString()) @>

    [<Test>]
    member this.``ToString on string``() =
        test <@ ("1".ToString()) @>

    [<Test>]
    member this.``Copy record and change property``() =
        test <@ let t = { Prop1 = 1; Prop2 = "test" }
                let r = { t with Prop2 = "test2" }
                (r.Prop2) @>

    [<Test>]
    member this.``Copy record and change two properties``() =
        test <@ let t = { Prop1 = 1; Prop2 = "test" }
                let r = { t with Prop2 = "test2"; Prop1 = 2 }
                (r.Prop1) @>

    [<Test>]
    member this.``Pattern Matching``() =
        test <@ let a = true
                let b = match a with
                        | true -> "true"
                        | false -> "false"
                (b) @>

    [<Test>]
    member this.``Pattern matching multiple``() =
        test <@ let a = "something"
                let b = match a with
                        | "one" -> "one"
                        | "two" -> "two"
                        | "something" -> "something"
                        | _ -> "nothing"
                b @>
    
    [<Test>]
    member this.``Pattern matching multiple with invalid match``() =
        test <@ let a = "something"
                try
                    let b = match a with
                            | "one" -> "one"
                            | "two" -> "two"
                            | "some" -> "some"
                            | _ -> failwith "no match"
                    b
                with 
                | Failure ex ->  ex
                | _ ->  "no match" @>

    [<Test>]
    member this.``Pattern matching on union``() =
        test <@ let t = Second "second"
                let b = match t with
                        | Second x when x = "second" -> "matched properly"
                        | Second x -> "second"
                        | Third x -> "third"
                        | First x -> "first"
                        | Fourth(x,y) -> "Fourth"
                (b) @>

    [<Test>]
    member this.``IfThenElse Statement with body``() =
        test <@ let t = true
                let result = if t then
                                let a = 1
                                a
                             else
                                let b = 1
                                b

                result @>

    [<Test>]
    member this.``Function that returns a tuple``() =
        test <@ let func (a:int) = (a,1)
                let b = func 2
                (fst b) @>

    [<Test>]
    member this.``Call snd on tuple``() =
        test <@ let b = (1,2)
                (snd b) @>

    [<Test>]
    member this.``Function that takes a tuple as argument``() =
        test <@ let tup = (1,2)
                let func (x,y) = x + y
                let result = func tup
                (result) @>

    [<Test>]
    member this.``TryCatch``() =
        test <@ let result = try
                                failwith "failure"
                             with
                             | _ -> true
                result @>

    [<Test>]
    member this.``ActivePattern``() =
        test <@ let a = Some("item")
                let result = match a with
                                | TestPattern x -> true
                                | _ -> false
                result @>

    [<Test>]
    member this.``Some/None``() =
        test <@ let a = Some(true)
                if a.IsSome then  true else  false @>

    [<Test>]
    member this.``Match on Some/None``() =
        test <@ let a:int option = Some(2)
                let result = match a with
                                | Some(x) -> x
                                | None -> 0
                result @>

    [<Test>]
    member this.``Sequence generator with range, map, and toArray``() =
        test <@ let result = Seq.toArray(seq {for i in 0..10 do yield i.ToString() + "stuff"})
                (result.[0]) @>

    [<Test>]
    member this.``Recursive factorial``() =
        test <@ let rec factorial n =
                    if n=0 then 1 else n * factorial(n - 1)
                
                let result = factorial 2
                result @>

    [<Test>]
    member this.``List cons``() =
        test <@ let list = 1::[]
                (list.[0]) @>

    [<Test>]
    member this.``List initialization``() =
        test <@ let list = [1;2;3]
                (list.[2]) @>

    [<Test>]
    member this.``List match``() =
        test <@ let li = [1;2]
                let result = match li with
                                | h::t::[] -> true
                                | _ -> false
                result @>

    [<Test>]
    member this.``List append``() =
        test <@ let l1 = [1;2]
                let l2 = [3;4]
                let result = l2@l1
                (result.Length) @>

    [<Test>]
    member this.``List traversal``() =
        test <@ let list = [1;2;3;4;5]
                let result = [for i in list -> i + 1]
                (result.Head) @>

    [<Test>]
    member this.``List reverse``() =
        test <@ let list = [1;2;3;4;5]
                let result = list |> List.rev
                (result.Head) @>

    [<Test>]
    member this.``List access works properly``() =
        test <@ let list = [1;2;3;4;5]
                let result = list.[0] + list.[1]
                (result) @>

    [<Test>]
    member this.``List traverseal with yield!``() =
        test <@ let list = [1;2;3;4;5]
                let result = [for i in list do yield! [i;2]]
                (result.Head) @>


    [<Test>]
    member this.``Tuple access``() =
        test <@ let a = (1,2)
                (snd a) @>

    [<Test>]
    member this.``Tuple assign``() =
        test <@ let (a,b) = (1,2)
                (a + b) @>

    [<Test>]
    member this.``Sequence with filter``() =
        test <@ let result = seq { for i in 0..10 do yield if i > 5 then Some(i.ToString() + "stuff") else None } 
                                    |> Seq.filter(fun x -> x.IsSome) 
                                    |> Seq.map(fun x -> x.Value)
                                    |> Seq.toArray

                let t = result.[0].ToString() + result.[1].ToString()
                (t) @>

    [<Test>]
    member this.``Range with filter``() =
        test <@ let result = seq { 0..10 } |> Seq.filter(fun x -> x > 5) |> Seq.toArray
                (result.[0]) @>

    [<Test>]
    member this.``Sequential calls order properly``() =
        test <@ let print (x:int) = ()
                let fact() = let result = 2
                             print result
                             let result2 = 4
                             result2
                (fact()) @>

    [<Test>]
    member this.``Type check works properly``() =
        test <@ let item = 0.0
                let checkItem (x:System.Object) =
                                match x with
                                | :? string -> true
                                | :? int -> true
                                | :? float -> true
                                | _ -> false
                (checkItem item) @>

    [<Test>]
    member this.``Call function with tuple argument``() =
        test <@ let func (x:int,y:int) = x + y
                let myTup = (1,2)
                let result = func myTup
                result @>

    [<Test>]
    member this.``NullCheckPattern``() =
        test <@ let value = "a string"
                let result = match value with
                             | NullCheckPattern(x) -> "Was not null"
                             | _ -> "Was null"
                result @>

    [<Test>]
    member this.``Type check on base class``() =
        test <@ let value = new class2("my name", 1) :> System.Object
                let result = match value with
                                | :? class1 -> true
                                | _ -> false
                                
                result  @>

    [<Test>]
    member this.``Type check on same class``() =
        test <@ let value = new class2("my name", 1) :> System.Object
                let result = match value with
                                | :? class2 -> true
                                | _ -> false
                                
                result  @>

    [<Test>]
    member this.``Type check on union``() =
        test <@ let value = First(1) :> System.Object
                let result = match value with
                                | :? union -> true
                                | _ -> false
                result  @>

    [<Test>]
    member this.``Equality check on records``() =
        let v1 = { x = 1; y = 1 }
        let v2 = { x = 1; y = 1 }
        let r = v1 = v2

        test <@ let value1 = { x = 1; y = 1 }
                let value2 = { x = 1; y = 1 }
                let result = value1 >< value2
                result @>

    [<Test>]
    member this.``Equality check on unions``() =
        test <@ let value1 = First(1)
                let value2 = First(1)
                let result = value1 >< value2
                result @>

    [<Test>]
    member this.``Match on record``() =
        test <@ let value1 = { Prop1 = 1; Prop2 = "neat" }
                let result = match value1 with
                                | { Prop1 = 1; Prop2 = "neat" } -> true
                                | _ -> false
                result @>