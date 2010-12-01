namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open QuotationsTestHelper

[<TestFixture>]
type MapModuleTests() =
    let test quote = QuotationsTestHelper.testWithType [] quote

    [<Test>]
    member this.empty() =
        test <@ let map = Map.empty: Map<int,int>
                emit map.Count @>

    [<Test>]
    member this.index() =
        test <@ let map = Map.empty |> Map.add 1 2
                emit map.[1] @>

    [<Test>]
    member this.add() =
        test <@ let map = Map.empty |> Map.add 1 2
                let result = map |> Map.fold (fun acc key value -> acc + "{" + key.ToString() + " : " + value.ToString() + "}") ""
                emit result @>

    [<Test>]
    member this.filter() =
        test <@ let map = [(1,2);(3,4);(5,6)] |> Map.ofList
                let filtered = map |> Map.filter(fun key value -> key >= 3)
                let result = map |> Map.fold (fun acc key value -> acc + "{" + key.ToString() + " : " + value.ToString() + "}") ""
                emit result @>

    [<Test>]
    member this.ofList() =
        test <@ let map = [(1,2);(3,4);(5,6)] |> Map.ofList
                let result = map |> Map.fold (fun acc key value -> acc + "{" + key.ToString() + " : " + value.ToString() + "}") ""
                emit result @>

    [<Test>]
    member this.findKey() =
        test <@ let map = [(1,2);(3,4);(5,6)] |> Map.ofList
                let result = map |> Map.findKey(fun key value -> value > 5)
                emit result @>

    [<Test>]
    member this.foldBack() =
        test <@ let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
                let result =  Map.foldBack (fun key value state -> state + key) map1 0
                emit result @>

    [<Test>]
    member this.forall() =
        test <@ let map1 = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
                let allPositive = Map.forall (fun key value -> key > 0)
                let result = allPositive map1
                emit result @>

    [<Test>]
    member this.isEmpty() =
        test <@ emit (Map.empty |> Map.isEmpty) @>

    [<Test>]
    member this.iter() =
        test <@ let map = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
                let result = ref ""
                map |> Map.iter (fun key value -> result := (!result) + (key.ToString() + ":" + value.ToString()))

                emit !result @>

    [<Test>]
    member this.map() =
        test <@ let map = Map.ofList [ (1, "one"); (2, "two"); (3, "three") ]
                let newMap = map |> Map.map(fun key value -> value.ToUpper())
                let result = newMap |> Map.fold (fun acc key value -> acc + "{" + key.ToString() + " : " + value.ToString() + "}") ""
                emit result @>

    [<Test>]
    member this.partition() =
        test <@ let map1 = [ for i in 1..10 -> (i, i*i)] |> Map.ofList
                let (mapEven, mapOdd) = Map.partition (fun key value -> key % 2 = 0) map1
                let mapEvenResult = mapEven |> Map.fold (fun acc key value -> acc + "{" + key.ToString() + " : " + value.ToString() + "}") ""
                let mapOddResult = mapOdd |> Map.fold (fun acc key value -> acc + "{" + key.ToString() + " : " + value.ToString() + "}") ""
                let result = mapEvenResult + mapOddResult
                emit result @>
    
    [<Test>]
    member this.pick() =
        test <@ let map1 = [ for i in 1 .. 100 -> (i, 100 - i) ] |> Map.ofList
                let result = Map.pick (fun key value -> if key = value then Some(key) else None) map1
                emit result @>

    [<Test>]
    member this.remove() =
        test <@ let map1 = [(1,2);(3,4);(5,6);(7,8)] |> Map.ofList
                let mapResult = map1 |> Map.remove 3
                let result = mapResult |> Map.fold (fun acc key value -> acc + "{" + key.ToString() + " : " + value.ToString() + "}") ""
                emit result @>

    [<Test>]
    member this.toArray() =
        test <@ let map1 = [(1,2);(3,4);(5,6);(7,8)] |> Map.ofList
                let arr = map1 |> Map.toArray
                let result = arr |> Array.fold(fun acc next -> acc + " " + next.ToString()) ""
                emit result @>

    [<Test>]
    member this.toList() =
        test <@ let map1 = [(1,2);(3,4);(5,6);(7,8)] |> Map.ofList
                let list = map1 |> Map.toList
                let result = list |> List.fold(fun acc next -> acc + " " + next.ToString()) ""
                emit result @>

    [<Test>]
    member this.toSeq() =
        test <@ let map1 = [(1,2);(3,4);(5,6);(7,8)] |> Map.ofList
                let seq1 = map1 |> Map.toSeq
                let result = seq1 |> Seq.fold(fun acc next -> acc + " " + next.ToString()) ""
                emit result @>

    [<Test>]
    member this.tryFind() =
        test <@ let map1 = [ for i in 1 .. 100 -> (i, i*i) ] |> Map.ofList
                let result = Map.tryFind 50 map1
                if result.IsSome then
                    emit result.Value
                else emit 0 @>

    [<Test>]
    member this.tryFindKey() =
        test <@ let map1 = [ for i in 1 .. 100 -> (i, i*i) ] |> Map.ofList
                let result = Map.tryFindKey (fun key value -> key = value) map1
                match result with
                | Some key -> emit true
                | None -> emit false @>

    [<Test>]
    member this.tryPick() =
        test <@ let map1 = [ for i in 1 .. 100 -> (i, 100 - i) ] |> Map.ofList
                let result = Map.tryPick (fun key value -> if key = value then Some(key) else None) map1
                match result with
                | Some x -> emit true
                | None -> emit false @>







    