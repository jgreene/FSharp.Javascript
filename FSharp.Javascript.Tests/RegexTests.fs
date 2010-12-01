namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestModule
open QuotationsTestHelper
open System
open System.Text.RegularExpressions

open FSharp.Javascript.Library

[<TestFixture>]
type RegexTests() =

    [<Test>]
    member this.``Can match zipcode`` () =
        test <@ let regex = System.Text.RegularExpressions.Regex("^(?!0{5})(\d{5})(?!-?0{4})(-?\d{4})?$")
                let valueToMatch = "12345-6789"
                let result = regex.IsMatch(valueToMatch)
                emit result

                @>

    [<Test>]
    member this.``Will not match invalid zipcode`` () =
        test <@ let regex = System.Text.RegularExpressions.Regex("^(?!0{5})(\d{5})(?!-?0{4})(-?\d{4})?$")
                let valueToMatch = "123a5-6789"
                let result = regex.IsMatch(valueToMatch)
                emit result

                @>

