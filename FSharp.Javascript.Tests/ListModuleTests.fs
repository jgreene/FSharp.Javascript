namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open QuotationsTestHelper

[<TestFixture>]
type ListModuleTests() =
    let test quote = QuotationsTestHelper.testWithType [] quote

    [<Test>]
    member this.append() =
        test <@ let list1 = [1..5]
                let list2 = [5..10]
                let final = List.append list1 list2
                let result = final |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.choose() =
        test <@ let numbers = [1..20]
                let evens = List.choose(fun x -> 
                            match x with
                            | x when x % 2=0 -> match x with
                                                | x -> Some(x)
                            | _ -> None ) numbers

                let result = evens |> List.fold (fun acc next -> acc + next.ToString()) ""
                emit result @>

    [<Test>]
    member this.collect() =
        test <@ let list1 = [10; 20; 30]
                let collectList = List.collect (fun x -> [for i in 1..3 -> x * i]) list1
                let result = collectList |> List.fold (fun acc next -> acc + next.ToString()) ""
                emit result @>

    [<Test>]
    member this.concat() =
        test <@ let list1to10 = List.append [1; 2; 3] [4; 5; 6; 7; 8; 9; 10]
                let listResult = List.concat [ [1; 2; 3]; [4; 5; 6]; [7; 8; 9] ]
                let result = listResult |> List.fold (fun acc next -> acc + next.ToString()) ""
                emit result @>

    [<Test>]
    member this.filter() =
        test <@ let data = [("Cats",4);
                            ("Dogs",5);
                            ("Mice",3);
                            ("Elephants",2)]
                let res = data |> List.filter (fun (nm,x) -> nm.Length <= 4)
                let result = res |> List.fold(fun acc x -> acc + x.ToString()) ""
                emit result @>

    [<Test>]
    member this.fold2() =
        test <@ let result = List.fold2 (fun acc elem1 elem2 -> acc + max elem1 elem2) 0 [ 1; 2; 3 ] [ 3; 2; 1 ]
                emit result @>

    [<Test>]
    member this.foldBack() =
        test <@ let result = List.foldBack (fun acc elem -> acc - elem) [ 1; 2; 3 ] 0
                emit result @>

    [<Test>]
    member this.foldBack2() =
        test <@ let transactionTypes = [ "Deposit"; "Deposit"; "Withdrawal" ]
                let transactionAmounts = [ 100.00; 1000.00; 95.00 ]
                let initialBalance = 200.00
                let endingBalance = List.foldBack2 (fun elem1 elem2 acc ->
                                                        match elem1 with
                                                        | "Deposit" -> acc + elem2
                                                        | "Withdrawal" -> acc - elem2
                                                        | _ -> acc)
                                                        transactionTypes
                                                        transactionAmounts
                                                        initialBalance

                emit endingBalance @>

    [<Test>]
    member this.map() =
        test <@ let arr = [ 1; 2; 3 ] |> List.map (fun x -> x + 10)
                let result = arr |> List.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.map2() =
        test <@ let list1 = [ 1; 2; 3 ]
                let list2 = [ 4; 5; 6 ]
                let listOfSums = List.map2 (fun x y -> x + y) list1 list2
                let result = listOfSums |> List.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.map3() =
        test <@ let list1 = [ 1; 2; 3 ]
                let list2 = [ 4; 5; 6 ]
                let list3 = [ 7; 8; 9 ]
                let listOfSums = List.map3 (fun x y z -> x + y + z) list1 list2 list3
                let result = listOfSums |> List.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.mapi() =
        test <@ let array1 = [ 1; 2; 3 ]
                let arrayAddTimesIndex = List.mapi (fun i x -> (x) * i) array1
                let result = arrayAddTimesIndex |> List.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.mapi2() =
        test <@ let array1 = [ 1; 2; 3 ]
                let array2 = [ 4; 5; 6 ]
                let arrayAddTimesIndex = List.mapi2 (fun i x y -> (x + y) * i) array1 array2
                let result = arrayAddTimesIndex |> List.fold(fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.nth() =
        test <@ let list = [1..10]
                let result = List.nth list 3
                emit result @>

    [<Test>]
    member this.partition() =
        test <@ let (l,r) = List.partition (fun elem -> elem > 50 && elem < 60) [ 1 .. 100 ]
                let result1 = l |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result2 = r |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result = result1 + result2
                emit result @>

    [<Test>]
    member this.reduceBack() =
        test <@ let result = List.reduceBack (fun elem acc -> elem - acc) [ 1; 2; 3; 4 ]
                emit result @>

    [<Test>]
    member this.replicate() =
        test <@ let list = List.replicate 10 1
                let result = list |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.scan() =
        test <@ let initialBalance = 1122.73
                let transactions = [ -100.00; +450.34; -62.34; -127.00; -13.50; -12.92 ]
                let balances = List.scan (fun balance transactionAmount -> balance + transactionAmount) initialBalance transactions
                let result = balances |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.scanBack() =
        test <@ let ops1 =
                     [ fun x -> x + 1
                       fun x -> x + 2
                       fun x -> x - 5 ]

                let arr = List.scanBack (fun op x -> op x) ops1 10
                let result = arr |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sort() =
        test <@ let arr = List.sort [1; 4; 8; -2; 5]
                let result = arr |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sortBy() =
        test <@ let arr = List.sortBy (fun elem -> abs elem) [1; 4; 8; -2; 5]
                let result = arr |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.sortWith() =
        test <@ let arr = [1; 4; 8; -2; 5]
                let arr1 = List.sortWith (fun elem1 elem2 -> elem1 - elem2) arr
                let result = arr1 |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.tail() =
        test <@ let list = [1..10]
                let tail = list |> List.tail
                let result = tail |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                emit result @>

    [<Test>]
    member this.unzip() =
        test <@ let array1, array2 = List.unzip [ (1, 2); (3, 4) ]
                let result1 = array1 |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result2 = array2 |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result = result1 + result2
                emit result @>

    [<Test>]
    member this.unzip3() =
        test <@ let array1, array2, array3 = List.unzip3 [ (1, 2, 5); (3, 4, 10) ]
                let result1 = array1 |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result2 = array2 |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result3 = array3 |> List.fold (fun acc next -> acc + next.ToString() + ",") ""
                let result = result1 + result2 + result3
                emit result @>

    [<Test>]
    member this.``zip`` () =
        test <@ let seq1 = [1..10]
                let seq2 = [10.. -1 .. 1]
                let zip = List.zip seq1 seq2
                let result = zip |> List.fold (fun acc (l,r) -> acc + "(" + l.ToString() + ", " + r.ToString() + ") ") ""
                emit result @>

    [<Test>]
    member this.``zip3`` () =
        test <@ let seq1 = [1..10]
                let seq2 = [10.. -1 .. 1]
                let seq3 = [20 .. -1 .. 11]
                let zip = List.zip3 seq1 seq2 seq3
                let result = zip |> List.fold (fun acc (l,m,r) -> acc + "(" + l.ToString() + ", " + m.ToString() + ", " + r.ToString() + ") ") ""
                emit result @>


