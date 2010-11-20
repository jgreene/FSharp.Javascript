namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestNamespace.ComputationModule
open QuotationsTestHelper

[<TestFixture>]
type ComputationExpressionTests() =

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

        
                