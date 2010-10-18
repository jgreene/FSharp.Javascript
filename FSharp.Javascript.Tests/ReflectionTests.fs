#light
namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestModule
open QuotationsTestHelper

[<TestFixture>]
type ReflectionTests() =

    [<Test>]
    member this.``Can GetType``() =
        test <@ let t = { Prop1 = 1; Prop2 = "Test" }
                let typ = t.GetType()
                let props = typ.GetProperties()
                emit props.Length  @>