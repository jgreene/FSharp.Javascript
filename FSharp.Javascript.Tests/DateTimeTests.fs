namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestModule
open QuotationsTestHelper
open System

open FSharp.Javascript.Library

[<TestFixture>]
type DateTimeTests() =

    [<Test>]
    member this.``Get MinTime``() =
        test <@ let date = DateTime.MinValue
                emit (date.ToString()) @>

    [<Test>]
    member this.``Can create specific date time``() =
        test <@ let date = new DateTime(2010, 6,12)
                emit (date.ToString()) @>

    [<Ignore("This test will never pass correctly as the time between when the F# executes and the javascript does effects the resull")>]
    [<Test>]
    member this.``Can get current DateTime``() =
        test <@ let date = DateTime.Now
                emit (date.ToString()) @>

    [<Test>]
    member this.``Can add years``() =
        test <@ let date = new DateTime(2010, 6,12)
                let date = date.AddYears(-18)
                emit (date.ToString()) @>

    [<Test>]
    member this.``Can add months``() =
        test <@ let date = new DateTime(2010, 6,12)
                let date = date.AddMonths(-18)
                emit (date.ToString()) @>

    [<Test>]
    member this.``Can add days``() =
        test <@ let date = new DateTime(2010, 6,12)
                let date = date.AddDays(float -35)
                emit (date.ToString()) @>

    [<Test>]
    member this.``ToShortDateString works``() =
        test <@ let date = new DateTime(2010, 6,12)
                emit (date.ToShortDateString()) @>

    [<Test>]
    member this.``Can Parse Date``() =
        test <@ let input = "06/12/1980"
                let date = DateTime.Parse(input)
                emit (date.ToString()) @>

    [<Test>]
    member this.``Date greater than works``() =
        test <@ let date1 = new DateTime(2010, 6, 12)
                let date2 = new DateTime(2010, 7, 12)
                let result = date1 > date2
                emit result
                @>

    [<Test>]
    member this.``Date less than works``() =
        test <@ let date1 = new DateTime(2010, 6, 12)
                let date2 = new DateTime(2010, 7, 12)
                let result = date1 < date2
                emit result
                @>

    [<Test>]
    member this.``Date greater than equal works``() =
        test <@ let date1 = new DateTime(2010, 6, 12, 4, 40, 33)
                let date2 = new DateTime(2010, 7, 12, 2, 20, 10)
                let result = date1 >= date2
                emit result
                @>

    [<Test>]
    member this.``Date less than equal works``() =
        test <@ let date1 = new DateTime(2010, 6, 12, 4, 40, 33)
                let date2 = new DateTime(2010, 7, 12, 2, 20, 10)
                let result = date1 <= date2
                emit result
                @>

    [<Test>]
    member this.``Date equals works``() =
        test <@ let date1 = new DateTime(2010, 6, 12)
                let date2 = new DateTime(2010, 7, 12)
                let result = date1 = date2
                emit result
                @>

    [<Test>]
    member this.``Date not equals works``() =
        test <@ let date1 = new DateTime(2010, 6, 12)
                let date2 = new DateTime(2010, 7, 12)
                let result = date1 <> date2
                emit result
                @>

    [<Test>]
    member this.``TryParse2 works``() =
        test <@ let input = "06/12/1980"
                let date = DateTime.TryParse2(input)
                emit date.IsSome
                @>

    [<Test>]
    member this.``Date equals Now - 18 years``() =
        test <@ let date = DateTime.Parse("06/12/2005")
                if date >= DateTime.Now.AddYears(-18) then
                    emit "You must be eighteen years or older."
                else
                    emit "You are older than 18 years" @>