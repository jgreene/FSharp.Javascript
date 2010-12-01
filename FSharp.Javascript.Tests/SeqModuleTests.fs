namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open QuotationsTestHelper

[<TestFixture>]
type SeqModuleTests() =
    let test quote = QuotationsTestHelper.testWithType [] quote

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

    [<Test>]
    member this.``Sequence pairwise`` () =
        test <@ let seq1 = Seq.pairwise (seq { for i in 1 .. 10 -> i * i })
                let result = seq1 |> Seq.fold(fun acc (x,y) -> acc + "(" + x.ToString() + ", " + y.ToString() + ") ") ""
                emit result @>

    [<Test>]
    member this.``Sequence pick`` () =
        test <@ let seq1 = seq { 1..10 }
                let result = seq1 |> Seq.pick (fun x -> if x = 4 then Some x else None)
                emit result @>

    [<Test>]
    member this.``Sequence tryPick`` () =
        test <@ let seq1 = seq { 1..10 }
                let result = seq1 |> Seq.tryPick (fun x -> if x = 11 then Some x else None)
                emit result.IsSome @>

    [<Test>]
    member this.``Sequence reduce`` () =
        test <@ let seq = seq { 1..10 }
                let result = seq |> Seq.reduce(fun acc x -> acc + x)
                emit result @>

    [<Test>]
    member this.``Sequence scan`` () =
        test <@ let initialBalance = 1122.73
                let transactions = [ -100.00; +450.34; -62.34; -127.00; -13.50; -12.92 ]
                let balances = Seq.scan (fun balance transactionAmount -> balance + transactionAmount) initialBalance transactions
                let result = balances |> Seq.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``Sequence skip``() =
        test <@ let seq1 = seq { 1..40 }
                let seq2 = seq1 |> Seq.skip 10
                let result = seq2 |> Seq.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``Sequence skipWhile``() =
        test <@ let seq1 = seq { 1..40 }
                let seq2 = seq1 |> Seq.skipWhile (fun x -> x < 20)
                let result = seq2 |> Seq.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``Sequence truncate`` () =
        test <@ let seq1 = seq { 1..40 }
                let seq2 = seq1 |> Seq.truncate 20
                let result = seq2 |> Seq.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``Sequence tryFindIndex`` () =
        test <@ let seq1 = seq { 1..40 }
                let result = seq1 |> Seq.tryFindIndex(fun x -> x = 10)
                emit result.Value @>

    [<Test>]
    member this.``Sequence unfold`` () =
        test <@ let seq1 = Seq.unfold (fun state -> if (state > 20) then None else Some(state, state + 1)) 0
                let result = seq1 |> Seq.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``Sequence windowed`` () =
        test <@ let seqNumbers = [ 1.0; 1.5; 2.0; 1.5; 1.0; 1.5 ] :> seq<float>
                let seqWindows = Seq.windowed 3 seqNumbers

                let result = seqWindows |> Seq.fold(fun acc next -> acc + "[" + (next |> Seq.fold(fun innerAcc n -> innerAcc + n.ToString() + ",") "") + "] ") ""
                emit result @>

    [<Test>]
    member this.``Sequence zip`` () =
        test <@ let seq1 = seq { 1..10 }
                let seq2 = seq { 10.. -1 .. 1 }
                let zip = Seq.zip seq1 seq2
                let result = zip |> Seq.fold (fun acc (l,r) -> acc + "(" + l.ToString() + ", " + r.ToString() + ") ") ""
                emit result @>

    [<Test>]
    member this.``Sequence zip3`` () =
        test <@ let seq1 = seq { 1..10 }
                let seq2 = seq { 10.. -1 .. 1 }
                let seq3 = seq { 100 .. -1 .. 1 }
                let zip = Seq.zip3 seq1 seq2 seq3
                let result = zip |> Seq.fold (fun acc (l,m,r) -> acc + "(" + l.ToString() + ", " + m.ToString() + ", " + r.ToString() + ") ") ""
                emit result @>