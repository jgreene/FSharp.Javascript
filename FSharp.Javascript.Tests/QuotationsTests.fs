#light
namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestModule
open QuotationsTestHelper
open FSharp.Javascript.Library

[<TestFixture>]
type QuotationsTests() =

    [<Test>]
    member this.``Dynamic lookup operator``() =
        test <@ let t = { Prop1 = 1; Prop2 = "blah" }
                let result = t?Prop1
                emit result @>

    [<Test>]
    member this.``Dynamic set operator``() =
        test <@ let t = new class2("blah", 2)
                t?Age <- 5
                emit (t?Age) @>
    
    [<Test>]
    member this.``Addition``() =
        test <@ emit(1 + 2) @>

    [<Test>]
    member this.``Subtraction``() =
        test <@ emit(2 - 1) @>

    [<Test>]
    member this.``Multiplication``() =
        test <@ emit(2 * 3) @>

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
                        | _ -> "nothing"
                emit b @>
    
    [<Test>]
    member this.``Pattern matching multiple with invalid match``() =
        test <@ let a = "something"
                let result = 
                    try
                        let b = match a with
                                | "one" -> "one"
                                | "two" -> "two"
                                | "some" -> "some"
                                | _ -> failwith "no match"
                        emit b
                    with 
                    | Failure ex ->  ex
                    | _ ->  "failed match"

                emit result @>

    [<Test>]
    member this.``Pattern matching on union``() =
        test <@ let t = Second "second"
                let b = match t with
                        | Second x when x = "second" -> "matched properly"
                        | Second x -> "second"
                        | Third x -> "third"
                        | First x -> "first"
                        | Fourth(x,y) -> "Fourth"
                emit (b) @>

    [<Test>]
    member this.``Pattern matching on simpleUnion``() =
        test <@ let t = One
                match t with
                | One -> emit true
                | Two -> emit false @>

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
                emit (result.[0]) @>

    [<Test>]
    member this.``Recursive factorial``() =
        test <@ let rec factorial n =
                    if n=0 then 1 else n * factorial(n - 1)
                
                let result = factorial 2
                emit result @>

    [<Test>]
    member this.``List cons``() =
        test <@ let list = 1::[]
                emit (list.[0]) @>

    [<Test>]
    member this.``List initialization``() =
        test <@ let list = [1;2;3]
                emit (list.[2]) @>

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
                emit (result.Length) @>

    [<Test>]
    member this.``List traversal``() =
        test <@ let list = [1;2;3;4;5]
                let result = [for i in list -> i + 1]
                emit (result.Head) @>

    [<Test>]
    member this.``List reverse``() =
        test <@ let list = [1;2;3;4;5]
                let result = list |> List.rev
                emit (result.Head) @>

    [<Test>]
    member this.``List access works properly``() =
        test <@ let list = [1;2;3;4;5]
                let result = list.[0] + list.[1]
                emit (result) @>

    [<Test>]
    member this.``List traverseal with yield!``() =
        test <@ let list = [1;2;3;4;5]
                let result = [for i in list do yield! [i;2]]
                emit (result.Head) @>


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
                emit(checkItem item) @>

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
                emit(result) @>

    [<Test>]
    member this.``Type check on base class``() =
        test <@ let value = new class2("my name", 1) :> System.Object
                let result = match value with
                                | :? class1 -> true
                                | _ -> false
                                
                emit result  @>

    [<Test>]
    member this.``Type check on same class``() =
        test <@ let value = new class2("my name", 1) :> System.Object
                let result = match value with
                                | :? class2 -> true
                                | _ -> false
                                
                emit result  @>

    [<Test>]
    member this.``Type check on union``() =
        test <@ let value = First(1) :> System.Object
                let result = match value with
                                | :? union -> true
                                | _ -> false
                emit result  @>

    [<Test>]
    member this.``Equality check on records``() =
        let v1 = { x = 1; y = 1 }
        let v2 = { x = 1; y = 1 }
        let r = v1 = v2

        test <@ let value1 = { x = 1; y = 1 }
                let value2 = { x = 1; y = 1 }
                let result = value1 >< value2
                emit result @>

    [<Test>]
    member this.``Equality check on unions``() =
        test <@ let value1 = First(1)
                let value2 = First(1)
                let result = value1 >< value2
                emit result @>

    [<Test>]
    member this.``Match on record``() =
        test <@ let value1 = { Prop1 = 1; Prop2 = "neat" }
                let result = match value1 with
                                | { Prop1 = 1; Prop2 = "neat" } -> true
                                | _ -> false
                emit result @>


    [<Test>]
    member this.``Method in module with multiple arguments is not called with tuple``() =
        test <@ let result = multipleArgs 1 2
                emit result @>

    [<Test>]
    member this.``Method in module with tupled arguments is called with tuple``() =
        test <@ let result = tupledArgs (1,2) (3,4)
                emit result @>

    [<Test>]
    member this.``Method in module with tupled arguments and non tupled arguments is called with tuple``() =
        test <@ let result = mixedTupledArgs 1 (3,4)
                emit result @>

    [<Test>]
    member this.``Method on record with multiple arguments is not called with tuple``() =
        test <@ let record = { Prop1 = 1; Prop2 = "neat" }
                let result = record.MultipleArgsOnRecord(1,2)
                emit result @>

    [<Test>]
    member this.``Can match on enum``() =
        test <@ let color = Color.Red
                let result = match color with
                            | Color.Red -> "red"
                            | Color.Blue -> "blue"
                            | Color.Green -> "green"
                            | _ -> failwith "invalid color"
                emit result @>

    [<Test>]
    member this.``Enum ToString works properly``() =
        test <@ let color = Color.Red
                let result = color.ToString()
                emit result @>

    [<Test>]
    member this.``Enum cast to int works properly``() =
        test <@ let color = Color.Red
                let result = (int)color;
                emit result @>

    [<Test>]
    member this.``Enum cast to string works properly``() =
        test <@ let color = Color.Red
                let result = (string)color;
                emit result @>

    [<Test>]
    member this.``Casting works`` () =
        test <@ let item = new class2("blah", 1) :> class1
                let item2 = item :?> class2
                let result = item2.Name
                emit result @>

    [<Test>]
    member this.``Can assign to mutable property on class``() =
        test <@ let item = new class2("blah", 1)
                item.Age <- 18

                emit (item.Age) @>

    [<Test>]
    member this.``Can update ref``() =
        test <@ let item = ref 1
                item := 2
                emit !item @> 

    [<Ignore("F# powerpack cannot convert this to an Expression Tree")>]
    [<Test>]
    member this.``Can update mutable value``() =
        test <@ let mutable item = 1
                item <- 2
                emit item @>

    [<Test>]
    member this.``Can use while loop``() =
        test <@ let item = ref 1
                while !item < 10 do
                    item := !item + 1
                emit !item @>

    [<Ignore("F# quotation system cannot get the AST of a constructor")>]
    [<Test>]
    member this.``Classes constructor fires``() =
        test <@ let item = new class2("blah", 1)
                emit (item.Age) @>

    [<Ignore("F# powerpack cannot convert this to an Expression Tree")>]
    [<Test>]
    member this.``Integer Range Loop``() =
        test <@ let item = ref 1
                for i in {0..10} do
                    item := i

                emit !item @>

    [<Test>]
    member this.``Can iterate sequence``() =
        test <@ let item = ref 1
                let sequence = [1;2;3;4;5;6;7;8;9;10]
                sequence |> Seq.iter (fun x -> do
                                        item := !item + x)

                let result = !item
                emit result @>

    [<Test>]
    member this.``Can use map1``() =
        test <@ let myMap = Map.empty |> (Map.add "first" 1)
                let result = myMap |> Map.find ("first")
                emit result @>

    [<Test>]
    member this.``Can use map2``() =
        test <@ let myMap = Map.empty |> (Map.add "first" 1) |> (Map.add "second" 2) |> (Map.add "third" 3)
                let result = myMap |> Map.find ("third")
                emit result @>

    [<Test>]
    member this.``Map is immutable``() =
        test <@ let myMap = Map.empty |> Map.add "first" 1
                let myMap2 = myMap |> Map.add "second" 2
                emit (myMap.Count = 1 && myMap2.Count = 2)
                //emit myMap.Count
             @>

    [<Test>]
    member this.``Map ContainsKey works``() =
        test <@ let myMap = Map.empty |> (Map.add "first" 1) |> (Map.add "second" 2) |> (Map.add "third" 3)
                emit (myMap.ContainsKey "second") @>

    [<Test>]
    member this.``Map Remove works``() =
        test <@ let myMap = Map.empty |> (Map.add "first" 1) |> (Map.add "second" 2) |> (Map.add "third" 3)
                let myMap2 = myMap.Remove "second"
                emit (myMap2.ContainsKey "second") @>

    [<Test>]
    member this.``Map instance method Add works``() =
        test <@ let myMap = Map.empty.Add("first", 1).Add("second", 2)
                emit (myMap.ContainsKey "second") @>

    [<Test>]
    member this.``List exists works``() =
        test <@ let list = [1;2;3;4;5]
                let result = list |> List.exists (fun x -> x = 4)
                emit result @>

    [<Test>]
    member this.``List fold works``() =
        test <@ let list = ["1"; "2"; "3"; "fifty"]
                let result = list |> List.fold (fun acc next -> next + "<br/>" + acc) ""
                emit result @>

    [<Test>]
    member this.``Map with list value and fold works``() =
        test <@ let errors = Map.empty |> (Map.add "first" ["1"; "2"; "3"; "fifty"])
                let errs = errors |> Map.find "first"
                let errorMessage = errs |> List.fold (fun acc next -> next + "<br/>" + acc) ""
                emit errorMessage @>

    [<Test>]
    member this.``Map tryFind works``() =
        test <@ let errors = Map.empty |> (Map.add "first" ["1"; "2"; "3"; "fifty"])
                let result = errors |> Map.tryFind "first"
                match result with
                | Some x -> emit true
                | None -> emit false @>

    [<Test>]
    member this.``Can iterate map twice``() =
        test <@ let errors = Map.empty |> (Map.add "first" 1)
                let errors2 = Map.add "second" 2 errors
                let result = (errors2 |> Map.containsKey "second") && (errors2 |> Map.containsKey "first")
                emit result
                 @>

    [<Test>]
    member this.``Can iterate list twice``() =
        test <@ let list = ["1"; "2"; "3"; "4"]
                let first = list |> List.fold (fun acc next -> next + "<br/>" + acc) ""
                let second = list |> List.fold (fun acc next -> next + "," + acc) ""
                emit (first + second) @>

    [<Test>]
    member this.``Can iterate array twice``() =
        test <@ let array = [|"1"; "2"; "3"; "4"|]
                let first = array |> Array.fold (fun acc next -> next + "<br/>" + acc) ""
                let second = array |> Array.fold (fun acc next -> next + "," + acc) ""
                emit (first + second) @>

    [<Test>]
    member this.``Property get uses method call get_``() =
        test <@ let item = new class2("blah", 1)
                let result = item.Name
                emit (result) @>

    [<Test>]
    member this.``Property set uses method call set_``() =
        test <@ let item = new class2("blah", 1)
                item.Age <- 18
                emit (item.Age) @>

    [<Test>]
    member this.``Array map with tuple`` () =
        test <@ let arr = [| (1,true);(2,false);(3,true)|]
                let result = arr 
                            |> Array.map (fun (x,y) -> if y then Some x else None)
                            |> Array.filter (fun x -> x.IsSome)

                emit result.[0].Value @>

    [<Test>]
    member this.``List reverse on single element list does not empty list``() =
        test <@ let list = [1]
                let result = list |> List.fold (fun acc next ->  acc + next.ToString()) ""
                emit (result)
                @>

    [<Test>]
    member this.``Pattern match does not change scope of value``() =
        test <@ let test = "Test"
                let value = "1"
                match test with
                | "blah" -> 
                    let value = "2"
                    emit value
                | "Test" ->
                    emit value
                | _ -> emit value @>

    [<Test>]
    member this.``Record with array of tuples``() =
        test <@ let r = { url = ""; arguments = [|("1", "2"); ("3", "$")|]; }
                emit r.url
            @>

    [<Test>]
    member this.``Multiple parameter lambda declares arguments in propert order``() =
        test <@ let f a b = a + b
                let x = f 1
                let result = x 2
                emit result @>
                    

    [<Test>]
    member this.``String.Join works``() =
        test <@ let a = [1;2;3;4;5]
                let result = System.String.Join(",", a)
                emit result @>

    [<Test>]
    member this.``Reassignment in inner scope does not break`` () =
        test <@ let a = 1
                let myMethod x y = x + y
                let result = if true then
                                let a = a + 1
                                if a >= 1 then
                                    let a = a + 2
                                    if true then
                                        let a = a + 3
                                        if true then
                                            let a = a + 4
                                            if true then
                                                let a = myMethod a 5
                                                a
                                            else
                                                a
                                        else
                                        a
                                    else
                                    a
                                else
                                    a
                              else
                                a
                emit result

                @>

    [<Test>]
    member this.``Reassignment in same scope works`` () =
        test <@ let a = 1
                let a = 2
                emit a @>

    [<Test>]
    member this.``Division on uneven int results in int``() =
        test <@ let result = 3 / 2
                emit result @>

    [<Test>]
    member this.``Anonymous lambda with multiple arguments fires correctly`` () =
        test <@ (fun x y -> 
                    let result = (x + y)
                    emit result)(1)(2) @>
