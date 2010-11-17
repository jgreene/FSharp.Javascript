namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestNamespace.ComputationModule
open QuotationsTestHelper

[<TestFixture>]
type ComputationExpressionTests() =

//    [<Test>]
//    member this.``ComputationExpression``() =
//        test <@ let result = comp { 
//                    return 2
//                }
//                emit result @>
//    [<Test>]
//    member this.``Computation Expression with let!``() =
//        test <@ let result = comp {
//                    let! a = 2
//                    return a
//                }
//                emit (result) @>

    [<Test>]
    member this.``Maybe Builder``() =
        test <@ let result = maybe {
                    let! z = Some 30
                    return z
                }
                emit (result.Value) @>

    [<Test>]
    member this.``State Monad``() =
        test <@ let (list,state) = executeState (addInt 1) []
                emit ((list).[0]) @>

    [<Test>]
    member this.``State Monad with two elements``() =
        test <@ let myState = state {
                    let start = [] : int list
                    let! x = state { return 1::start }
                    let! b = state { return 2::x }
                    return b
                }
                let (list,_) = executeState myState []
                emit ((list).[0] + (list).[1]) @>

    [<Test>]
    member this.``Sequence expression test1`` () =
        test <@ let tree = Directory("home", [File "test.jpg"; Directory("documents", [])])
                
                let rec getDirectoryNames x = seq {
                    match x with
                    | Directory(name, children) ->
                        yield name
                        for c in children do yield! getDirectoryNames c
                    | File n -> ()
                }

                let directories = getDirectoryNames tree
                let result = (directories |> Seq.skip 1 |> Seq.head)
                emit result

                @>

    [<Test>]
    member this.``Sequence average`` () =
        test <@
                let values = [1.0..10.0]
                let result = values |> Seq.average
                emit result

                @>

    [<Test>]
    member this.``Sequence average by`` () =
        test <@
                let values = [1..10]
                let result = values |> Seq.averageBy (fun elem -> float elem)
                emit result

                @>

    [<Test>]
    member this.``Sequence enumeration twice`` () =
        test <@ let tree = Directory("home", [File "test.jpg"; Directory("documents", [])])
                
                let rec getDirectoryNames x = seq {
                    match x with
                    | Directory(name, children) ->
                        yield name
                        for c in children do yield! getDirectoryNames c
                    | File n -> ()
                }

                let directories = getDirectoryNames tree
                let result1 = directories |> Seq.fold (fun acc next -> acc + next) ""
                let result2 = directories |> Seq.fold (fun acc next -> acc + next) ""
                let result = result1 + result2
                emit result

                @>

    [<Test>]
    member this.``Sequence cache`` () =
        test <@ let isPrime n =
                    let rec check i =
                        i > n/2 || (n % i <> 0 && check (i + 1))
                    check 2

                let seqPrimes = seq { for n in 2 .. 10000 do if isPrime n then yield n }
                // Cache the sequence to avoid recomputing the sequence elements.
                let cachedSeq = Seq.cache seqPrimes
                let tempSeq = seq {
                    for index in 1..5 do yield ((Seq.nth (Seq.length cachedSeq - index) cachedSeq).ToString() + " is Prime")
                }

                let result = tempSeq |> Seq.fold (fun acc next -> acc + next) ""

                emit result
                @>

    [<Test>]
    member this.``Sequence choose`` () =
        test <@ let numbers = seq { 1..20 }
                let evens = Seq.choose(fun x -> 
                            match x with
                            | z when z % 2=0 -> Some(z)
                            | _ -> None ) numbers

                let result = evens |> Seq.fold (fun acc next -> acc + next.ToString()) ""
                emit result @>

    [<Test>]
    member this.``Sequence compareWith`` () =
        test <@ let sequence1 = seq { 1 .. 10 }
                let sequence2 = seq { 10 .. -1 .. 1 }
                let compareSequences = Seq.compareWith (fun elem1 elem2 ->
                    if elem1 > elem2 then 1
                    elif elem1 < elem2 then -1
                    else 0) 

                let compareResult1 = compareSequences sequence1 sequence2
                match compareResult1 with
                | 1 -> emit "Sequence1 is greater than sequence2."
                | -1 -> emit "Sequence1 is less than sequence2."
                | 0 -> emit "Sequence1 is equal to sequence2."
                | _ -> emit "Invalid comparison result."
                @>

    [<Test>]
    member this.``Sequence countBy`` () =
        test <@ let mySeq1 = seq { 1.. 100 }
                let seqResult = Seq.countBy (fun elem ->
                                                    if (elem % 2 = 0) then 0 else 1) mySeq1

                let result = seqResult |> Seq.fold (fun acc (x,y) -> acc + " (" + x.ToString() + ", " + y.ToString() + ")") ""
                emit result
                @>

    [<Test>]
    member this.``Sequence find`` () =
        test <@ let isDivisibleBy number elem = elem % number = 0
                let result = Seq.find (isDivisibleBy 5) [ 1 .. 100 ]
                emit result
                @>

    [<Test>]
    member this.``Sequence tryFind`` () =
        test <@ let isDivisibleBy number elem = elem % number = 0
                let result = Seq.tryFind (isDivisibleBy 5) [ 1 .. 100 ]
                emit result.IsSome

                @>

    [<Test>]
    member this.``Sequence distinct`` () =
        test <@ let resultSequence = Seq.distinct [1;2;1;3;4;5;23;453;1;2;45;2]

                let result = resultSequence |> Seq.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``Sequence distinctBy`` () =
        test <@ 
                let inputSequence = { -5 .. 10 }
                let absoluteSeq = Seq.distinctBy (fun elem -> abs elem) inputSequence
                let result = absoluteSeq |> Seq.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``Sequence exists2`` () =
        test <@ let seq1to5 = seq { 1 .. 5 }
                let seq5to1 = seq { 5 .. -1 .. 1 }
                emit (Seq.exists2 (fun elem1 elem2 -> elem1 = elem2) seq1to5 seq5to1) 
                @>

    [<Test>]
    member this.``Sequence findIndex``() =
        test <@ let seq1 = seq { 1..5 }
                let result = seq1 |> Seq.findIndex (fun x -> x = 3)
                emit result @>

    [<Test>]
    member this.``Sequence forAll`` () =
        test <@ let seq1 = seq { 1..5 }
                let result = seq1 |> Seq.forall (fun x -> x < 10)
                emit result @>

    [<Test>]
    member this.``Sequence forAll2`` () =
        test <@ let seq1to5 = seq { 1 .. 5 }
                let seq5to1 = seq { 5 .. -1 .. 1 }
                emit (Seq.forall2 (fun elem1 elem2 -> elem1 < 10 && elem2 < 10) seq1to5 seq5to1) 
                @>

    [<Test>]
    member this.``Sequence groupBy`` () =
        test <@ let sequence = seq { 1 .. 100 }
                let sequences3 = Seq.groupBy (fun index ->
                                                if (index % 2 = 0) then 0 else 1) sequence
                let result = sequences3 
                                |> Seq.fold (fun acc (key,values) -> 
                                                acc + "(" + key.ToString() + ", [" + (values |> Seq.fold(fun acc2 next -> acc2 + next.ToString() + "," ) "") + "])") ""
                emit result @>

    [<Test>]
    member this.``Sequence initInfinite and take`` () =
        test <@ let seqInfinite = Seq.initInfinite (fun i -> i + 1)

                let seqTake = seqInfinite |> Seq.take 100
                let result = seqTake |> Seq.fold (fun acc next -> acc + next.ToString() + "," ) ""
                emit result @>

    [<Test>]
    member this.``Sequence init`` () =
        test <@ let init = Seq.init 10 (fun i -> i + 1)
                let result = init |> Seq.fold (fun acc next -> acc + next.ToString() + "," ) ""
                emit result

                @>

    [<Test>]
    member this.``Sequence isEmpty`` () =
        test <@ let seq = seq { 1..10 }
                emit (seq |> Seq.isEmpty) @>

    [<Test>]
    member this.``Sequence iter2`` () =
        test <@ let seq1 = [1; 2; 3]
                let seq2 = [4; 5; 6]
                let result = ref ""
                Seq.iter2 (fun x y -> result := (!result + x.ToString() + ":" + y.ToString() + ",")) seq1 seq2

                emit (!result) @>

    [<Test>]
    member this.``Sequence iteri`` () =
        test <@ let seq1 = seq { 1..5 }
                let result = ref ""
                Seq.iteri (fun i x -> result := (!result + i.ToString() + ":" + x.ToString() + ",")) seq1
                emit (!result) @>

    [<Test>]
    member this.``Sequence map2`` () =
        test <@ let seq1 = seq { 1..5 }
                let seq2 = seq { 5.. -1 .. 1 }
                let resultSeq = Seq.map2 (fun x y -> (x + y)) seq1 seq2
                let result = resultSeq |> Seq.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result

                    @>

    [<Test>]
    member this.``Sequence mapi`` () =
        test <@ let seq1 = seq { 1..5 }
                let resultSeq = Seq.mapi (fun i x -> i + x) seq1
                let result = resultSeq |> Seq.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``Sequence maxBy`` () =
        test <@ let seq1 = seq { 1..5 }
                let result = Seq.maxBy (fun x -> x + 1) seq1
                emit result @>

    [<Test>]
    member this.``Sequence max`` () =
        test <@ let seq1 = seq { 1..5 }
                let result = Seq.max seq1
                emit result @>

    [<Test>]
    member this.``Sequence minBy`` () =
        test <@ let seq1 = seq { 1..5 }
                let result = Seq.minBy (fun x -> x + 1) seq1
                emit result @>

    [<Test>]
    member this.``Sequence min`` () =
        test <@ let seq1 = seq { 1..5 }
                let result = Seq.min seq1
                emit result @>
                