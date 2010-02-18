#light
namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open ComputationModule
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
                emit result.Value @>