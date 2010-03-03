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
        test <@ emit (1 + 1) @>

    [<Test>]
    member this.``Subtraction``() =
        test <@ emit(1 - 1) @>

    [<Test>]
    member this.``Multiplication``() =
        test <@ emit(2 * 2) @>

    [<Test>]
    member this.``Function declaration with call``() =
        test <@ let f x = x + 1
                emit(f 2) @>

    [<Test>]
    member this.``Multi line function with call``() =
        test <@ let f x =
                    let y = 1
                    x + y
                emit(f 2) @>

    [<Test>]
    member this.``Function with multiple arguments``() =
        test <@ let f x y = x + y
                emit(f 1 2) @>

    [<Test>]
    member this.``Anonymous function``() =
        test <@ let f (x:unit-> int) = x()
                emit(f (fun () -> 1 + 1)) @>

    [<Test>]
    member this.``Anonymous function with arg``() =
        test <@ let f (x:int->int) = x(1)
                emit(f (fun x -> x + 1)) @>


    [<Test>]
    member this.``Array creation and access``() =
        test <@ let list = [|"one";"two";"three"|]
                emit(list.[0]) @>

    [<Test>]
    member this.``Record declaration and property access``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                emit(t.Prop1) @>

    [<Test>]
    member this.``Record with custom method call``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                emit(t.GetProp1()) @>

    [<Test>]
    member this.``Record with custom method call with argument``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                emit(t.GetProp2(" neato")) @>

    [<Test>]
    member this.``Module function that takes record as argument``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                emit(getProp1(t)) @>

    [<Test>]
    member this.``Module function with one argument``() =
        test <@ emit(testFunction 1) @>

    [<Test>]
    member this.``Class inheritance``() =
        test <@ let c = new class2("bob", 1)
                emit(c.GetName()) @>

    [<Test>]
    member this.``ExtensionMethod``() =
        test <@ let a = 1
                emit(a.GetValue()) @>

    [<Test>]
    member this.``ToString``() =
        test <@  let a = 1
                 emit(a.ToString()) @>

    [<Test>]
    member this.``ToString on constant``() =
        test <@ emit((1).ToString()) @>

    [<Test>]
    member this.``ToString on string``() =
        test <@ emit("1".ToString()) @>

    [<Test>]
    member this.``Copy record and change property``() =
        test <@ let t = { Prop1 = 1; Prop2 = "test" }
                let r = { t with Prop2 = "test2" }
                emit(r.Prop2) @>

    [<Test>]
    member this.``Copy record and change two properties``() =
        test <@ let t = { Prop1 = 1; Prop2 = "test" }
                let r = { t with Prop2 = "test2"; Prop1 = 2 }
                emit(r.Prop1) @>

    [<Test>]
    member this.``Pattern Matching``() =
        test <@ let a = true
                let b = match a with
                        | true -> "true"
                        | false -> "false"
                emit(b) @>

    [<Test>]
    member this.``Pattern matching multiple``() =
        test <@ let a = "something"
                let b = match a with
                        | "one" -> "one"
                        | "two" -> "two"
                        | "something" -> "something"
                emit b @>
    
    [<Test>]
    member this.``Pattern matching multiple with invalid match``() =
        test <@ let a = "something"
                try
                    let b = match a with
                            | "one" -> "one"
                            | "two" -> "two"
                            | "some" -> "some"
                            | _ -> failwith "no match"
                    emit b
                with 
                | Failure ex -> emit ex
                | _ -> emit "no match" @>

    [<Test>]
    member this.``Pattern matching on union``() =
        test <@ let t = Second "second"
                let b = match t with
                        | Second x when x = "second" -> "matched properly"
                        | Second x -> "second"
                        | Third x -> "third"
                        | First x -> "first"
                        | Fourth(x,y) -> "Fourth"
                emit(b) @>

    [<Test>]
    member this.``IfThenElse Statement with body``() =
        test <@ let t = true
                let result = if t then
                                let a = 1
                                a
                             else
                                let b = 1
                                b

                emit result @>

    [<Test>]
    member this.``Function that returns a tuple``() =
        test <@ let func (a:int) = (a,1)
                let b = func 2
                emit(fst b) @>

    [<Test>]
    member this.``Call snd on tuple``() =
        test <@ let b = (1,2)
                emit(snd b) @>

    [<Test>]
    member this.``Function that takes a tuple as argument``() =
        test <@ let tup = (1,2)
                let func (x,y) = x + y
                let result = func tup
                emit(result) @>

    [<Test>]
    member this.``TryCatch``() =
        test <@ let result = try
                                failwith "failure"
                             with
                             | _ -> true
                emit result @>

    [<Test>]
    member this.``ActivePattern``() =
        test <@ let a = Some("item")
                let result = match a with
                                | TestPattern x -> true
                                | _ -> false
                emit result @>

    [<Test>]
    member this.``Some/None``() =
        test <@ let a = Some(true)
                if a.IsSome then emit true else emit false @>

    [<Test>]
    member this.``Match on Some/None``() =
        test <@ let a:int option = Some(2)
                let result = match a with
                                | Some(x) -> x
                                | None -> 0
                emit result @>

    [<Test>]
    member this.``Sequence generator with range, map, and toArray``() =
        test <@ let result = Seq.toArray(seq {for i in 0..10 do yield i.ToString() + "stuff"})
                emit(result.[0]) @>

    [<Test>]
    member this.``Recursive factorial``() =
        test <@ let rec factorial n =
                    if n=0 then 1 else n * factorial(n - 1)
                
                let result = factorial 2
                emit result @>

    [<Test>]
    member this.``List cons``() =
        test <@ let list = 1::[]
                emit(list.[0]) @>

    [<Test>]
    member this.``List initialization``() =
        test <@ let list = [1;2;3]
                emit(list.[2]) @>

    [<Test>]
    member this.``List match``() =
        test <@ let li = [1;2]
                let result = match li with
                                | h::t::[] -> true
                                | _ -> false
                emit result @>

    [<Test>]
    member this.``List append``() =
        test <@ let l1 = [1;2]
                let l2 = [3;4]
                let result = l2@l1
                emit(result.Length) @>

    [<Test>]
    member this.``List traversal``() =
        test <@ let list = [1;2;3;4;5]
                let result = [for i in list -> i + 1]
                emit(result.Head) @>

    [<Test>]
    member this.``List reverse``() =
        test <@ let list = [1;2;3;4;5]
                let result = list |> List.rev
                emit(result.Head) @>

    [<Test>]
    member this.``List access works properly``() =
        test <@ let list = [1;2;3;4;5]
                let result = list.[0] + list.[1]
                emit(result) @>

    [<Test>]
    member this.``List traverseal with yield!``() =
        test <@ let list = [1;2;3;4;5]
                let result = [for i in list do yield! [i;2]]
                emit(result.Head) @>


    [<Test>]
    member this.``Tuple access``() =
        test <@ let a = (1,2)
                emit(snd a) @>

    [<Test>]
    member this.``Tuple assign``() =
        test <@ let (a,b) = (1,2)
                emit(a + b) @>

    [<Test>]
    member this.``Sequence with filter``() =
        test <@ let result = seq { for i in 0..10 do yield if i > 5 then Some(i.ToString() + "stuff") else None } 
                                    |> Seq.filter(fun x -> x.IsSome) 
                                    |> Seq.map(fun x -> x.Value)
                                    |> Seq.toArray

                let t = result.[0].ToString() + result.[1].ToString()
                emit(t) @>

    [<Test>]
    member this.``Range with filter``() =
        test <@ let result = seq { 0..10 } |> Seq.filter(fun x -> x > 5) |> Seq.toArray
                emit(result.[0]) @>

    [<Test>]
    member this.``Sequential calls order properly``() =
        test <@ let print (x:int) = ()
                let fact() = let result = 2
                             print result
                             let result2 = 4
                             result2
                emit(fact()) @>

    [<Test>]
    member this.``Type check works properly``() =
        test <@ let item = 0.0
                let checkItem (x:System.Object) =
                                match x with
                                | :? string -> true
                                | :? int -> true
                                | :? float -> true
                                | _ -> false
                emit (checkItem item) @>

    [<Test>]
    member this.``Call function with tuple argument``() =
        test <@ let func (x:int,y:int) = x + y
                let myTup = (1,2)
                let result = func myTup
                emit result @>

    [<Test>]
    member this.``NullCheckPattern``() =
        test <@ let value = "a string"
                let result = match value with
                             | NullCheckPattern(x) -> "Was not null"
                             | _ -> "Was null"
                emit result @>

    [<Test>]
    member this.``Type check on base class works properly``() =
        test <@ let value = new class2("my name", 1) :> System.Object
                let result = match value with
                                | :? class1 -> true
                                | _ -> false
                                
                emit result  @>

    [<Test>]
    member this.``Equality check on records``() =
        let v1 = { x = 1; y = 1 }
        let v2 = { x = 1; y = 1 }
        let r = v1 = v2

        test <@ let value1 = { x = 1; y = 1 }
                let value2 = { x = 1; y = 1 }
                let result = value1 = value2
                emit result @>

    [<Test>]
    member this.``Match on record``() =
        test <@ let value1 = { Prop1 = 1; Prop2 = "neat" }
                let result = match value1 with
                                | { Prop1 = 1; Prop2 = "neat" } -> true
                                | _ -> false
                emit result @>