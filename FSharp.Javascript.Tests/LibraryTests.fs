#light
namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestHelper

[<TestFixture>]
type LibraryTests() =

    [<Test>]
    member this.``Test1``() =
        let script = System.IO.File.ReadAllText("Test1.txt")
        test script

    [<Test>]
    member this.``JQuery 1.4``() =
        let script = System.IO.File.ReadAllText("jquery-1.4.txt")
        testLibrary script

    [<Test>]
    member this.``JQuery 1.2.6``() =
        let script = System.IO.File.ReadAllText("jquery-1.2.6.txt")
        testLibrary script

    [<Test>]
    member this.``Mootools 1.2.4``() =
        let script = System.IO.File.ReadAllText("mootools-1.2.4.txt")
        testLibrary script