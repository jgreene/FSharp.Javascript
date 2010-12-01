namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open QuotationsTestHelper

[<TestFixture>]
type ArrayModuleTests() =
    let test quote = QuotationsTestHelper.testWithType [] quote

    [<Test>]
    member this.``append`` () =
        test <@ let arr1 = [|1;2;3;4|]
                let arr2 = [|5;6;7;8|]
                let final = Array.append arr1 arr2
                let result = final |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result
                @>

    [<Test>]
    member this.average () =
        test <@ let arr1 = [|1.0..10.0|]
                let result = arr1 |> Array.average
                emit result @>

    [<Test>]
    member this.``averageBy`` () =
        test <@
                let values = [|1..10|]
                let result = values |> Array.averageBy (fun elem -> float elem)
                emit result

                @>

    [<Test>]
    member this.``zeroCreate``() =
        test <@ let arr = Array.zeroCreate 4
                emit (arr.Length = 4) @>

    [<Test>]
    member this.``blit`` () =
        test <@ let array1 = [| 1 .. 10 |]
                let array2 = Array.zeroCreate 20
                // Copy 4 elements from index 3 of array1 to index 5 of array2.
                Array.blit array1 3 array2 5 4
                let result = array2 |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``choose`` () =
        test <@ let numbers = seq { 1..20 } |> Array.ofSeq
                let evens = Array.choose(fun x -> 
                            match x with
                            | z when z % 2=0 -> Some(z)
                            | _ -> None ) numbers

                let result = evens |> Array.fold (fun acc next -> acc + next.ToString()) ""
                emit result @>

    [<Test>]
    member this.``collect`` () =
        test <@ let arr = (Array.collect (fun elem -> [| 0 .. elem |]) [| 1; 5; 10|])
                let result = arr |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``concat`` () =
        test <@ let multiplicationTable max = seq { for i in 1 .. max -> [| for j in 1 .. max -> (i, j, i*j) |] }
                let arr = (Array.concat (multiplicationTable 3))
                let result = arr |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.``copy`` () =
        test <@ let array1 = [| 1 .. 10 |]
                let array2 = Array.copy array1
                let result = array2 |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.create () =
        test <@ let arr = Array.create 10 1
                let result = arr |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.fill() =
        test <@ let arrayFill1 = [| 1 .. 25 |]
                Array.fill arrayFill1 2 20 0
                let result = arrayFill1 |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.filter() =
        test <@ let names = [|"Bob"; "Ann"; "Stephen"; "Vivek"; "Fred"; "Kim"; "Brian"; "Ling"; "Jane"; "Jonathan"|]
                let longNames = names |> Array.filter (fun x -> x.Length > 4)
                let result = longNames |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.fold2() =
        test <@ let result = Array.fold2 (fun acc elem1 elem2 -> acc + max elem1 elem2) 0 [| 1; 2; 3 |] [| 3; 2; 1 |]
                emit result @>

    [<Test>]
    member this.foldBack() =
        test <@ let result = Array.foldBack (fun acc elem -> acc - elem) [| 1; 2; 3 |] 0
                emit result @>

    [<Test>]
    member this.foldBack2() =
        test <@ let transactionTypes = [| "Deposit"; "Deposit"; "Withdrawal" |]
                let transactionAmounts = [| 100.00; 1000.00; 95.00 |]
                let initialBalance = 200.00
                let endingBalance = Array.foldBack2 (fun elem1 elem2 acc ->
                                                        match elem1 with
                                                        | "Deposit" -> acc + elem2
                                                        | "Withdrawal" -> acc - elem2
                                                        | _ -> acc)
                                                        transactionTypes
                                                        transactionAmounts
                                                        initialBalance

                emit endingBalance @>

    [<Test>]
    member this.init() =
        test <@ let arr = (Array.init 10 (fun index -> index * index))
                let result = arr |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.isEmpty() =
        test <@ let arr = [||] : int array
                let result = arr |> Array.isEmpty
                emit result @>

    [<Test>]
    member this.iteri2() =
        test <@ let array1 = [| 1; 2; 3 |]
                let array2 = [| 4; 5; 6 |]
                let result = ref ""
                Array.iteri2 (fun i elem1 elem2 -> result := (!result) + i.ToString() + ":(" + elem1.ToString() + ", " + elem2.ToString() + ")") array1 array2

                emit (!result) @>

    [<Test>]
    member this.map() =
        test <@ let arr = [| 1; 2; 3 |] |> Array.map (fun x -> x + 10)
                let result = arr |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.map2() =
        test <@ let array1 = [| 1; 2; 3 |]
                let array2 = [| 4; 5; 6 |]
                let arrayOfSums = Array.map2 (fun x y -> x + y) array1 array2
                let result = arrayOfSums |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.mapi2() =
        test <@ let array1 = [| 1; 2; 3 |]
                let array2 = [| 4; 5; 6 |]
                let arrayAddTimesIndex = Array.mapi2 (fun i x y -> (x + y) * i) array1 array2
                let result = arrayAddTimesIndex |> Array.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.partition() =
        test <@ let (l,r) = Array.partition (fun elem -> elem > 50 && elem < 60) [| 1 .. 100 |]
                let result1 = l |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result2 = r |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result = result1 + result2
                emit result @>

    [<Test>]
    member this.permute() =
        test <@ let reverse len orig = len - orig - 1
                let arr = Array.permute (reverse 10) [|1..10|]
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.reduceBack() =
        test <@ let result = Array.reduceBack (fun elem acc -> elem - acc) [| 1; 2; 3; 4 |]
                emit result @>

    [<Test>]
    member this.reverse() =
        test <@ let arr = [|1..10|] |> Array.rev
                let result = arr |>  Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.scan() =
        test <@ let initialBalance = 1122.73
                let transactions = [| -100.00; +450.34; -62.34; -127.00; -13.50; -12.92 |]
                let balances = Array.scan (fun balance transactionAmount -> balance + transactionAmount) initialBalance transactions
                let result = balances |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.scanBack() =
        test <@ let ops1 =
                     [| fun x -> x + 1
                        fun x -> x + 2
                        fun x -> x - 5 |]

                let arr = Array.scanBack (fun op x -> op x) ops1 10
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.set() =
        test <@ let arr = [|1..10|]
                Array.set arr 4 12
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sort() =
        test <@ let arr = Array.sort [|1; 4; 8; -2; 5|]
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sortBy() =
        test <@ let arr = Array.sortBy (fun elem -> abs elem) [|1; 4; 8; -2; 5|]
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sortInPlace() =
        test <@ let arr = [|1; 4; 8; -2; 5|]
                Array.sortInPlace arr
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sortInPlaceBy() =
        test <@ let arr = [|1; 4; 8; -2; 5|]
                Array.sortInPlaceBy (fun elem -> abs elem) arr
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sortInPlaceWith() =
        //5, 12, 6, 3
        test <@ let arr = [|1; 4; 8; -2; 5|]
                Array.sortInPlaceWith (fun elem1 elem2 -> elem1 - elem2) arr
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sortWith() =
        test <@ let arr = [|1; 4; 8; -2; 5|]
                let arr1 = Array.sortWith (fun elem1 elem2 -> elem1 - elem2) arr
                let result = arr1 |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sub() =
        test <@ let arr = Array.sub [|1..10|] 3 5
                let result = arr |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sum() =
        test <@ let result = [| 1 .. 10 |] |> Array.sum
                emit result @>

    [<Test>]
    member this.sumBy() =
        test <@ let result = [| 1 .. 10 |] |> Array.sumBy (fun x -> x * x)
                emit result @>

    [<Test>]
    member this.unzip() =
        test <@ let array1, array2 = Array.unzip [| (1, 2); (3, 4) |]
                let result1 = array1 |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result2 = array2 |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result = result1 + result2
                emit result @>

    [<Test>]
    member this.unzip3() =
        test <@ let array1, array2, array3 = Array.unzip3 [| (1, 2, 5); (3, 4, 10) |]
                let result1 = array1 |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result2 = array2 |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result3 = array3 |> Array.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result = result1 + result2 + result3
                emit result @>

