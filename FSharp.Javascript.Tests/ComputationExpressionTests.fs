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