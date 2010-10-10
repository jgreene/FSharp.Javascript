#light
module QuotationsTestHelper

open System.Text
open FSharp.Javascript.QuotationsConverter
open FSharp.Javascript.ModuleCompiler
open FSharp.Javascript.Printer
open FSharp.Javascript.Ast
open NUnit.Framework

open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.ExprShape
open Microsoft.FSharp.Quotations.Patterns
open Microsoft.FSharp.Quotations.DerivedPatterns
open Microsoft.FSharp.Linq
open Microsoft.FSharp.Linq.QuotationEvaluation

open System.Diagnostics
open System.IO

open System.Linq

let print input =
    System.Console.WriteLine((sprintf "%A" input))

//[<ReflectedDefinition>]
let emit x = x

let run (source:string) =
    let filePath = "C:\\Users\\nephesh\\test.js"
    File.WriteAllText(filePath, source)

    let info = new ProcessStartInfo("C:\\cygwin\\bin\\bash", "--login -i -c \"node test.js\"")
    info.CreateNoWindow <- false
    info.UseShellExecute <- true
    info.WindowStyle <- ProcessWindowStyle.Hidden

    use proc = Process.Start(info)

    proc.WaitForExit()

    let testResultPath = "C:\\Users\\nephesh\\testResult.js"
    let result = File.ReadAllText(testResultPath)
    File.Delete(testResultPath)
    result

//type mydel = delegate of obj -> StringBuilder
//
//let run (source:string) =
//    let emitter = new StringBuilder()
//    let func x = emitter.Append(x.ToString())
//    let testDel = new mydel(func)
//    let engine = new JintEngine()
//    engine.SetFunction("emit", testDel).Run(source) |> ignore
//    emitter.ToString()

let testWithType (ty:System.Type) quote =
    let testModule = true
    print quote
    print "--------------------------------------------------------------"
    let ast = convertToAst quote
    print ast
    print "--------------------------------------------------------------"
    let j1 = (getJavascript ast)
    let moduleAst = getAstFromType ty 
    let j2 = getJavascript moduleAst
    let library = System.IO.File.ReadAllText("fsharp.js") + System.Environment.NewLine + System.IO.File.ReadAllText("tests.js")
    let javascript = (library + System.Environment.NewLine + j2 + System.Environment.NewLine + System.Environment.NewLine + j1)
    print j2
    print "--------------------------------------------------------------"
    print j1
    //print javascript
    let quoteResult = quote.EvalUntyped();
    let quoteResultString = if quoteResult = null then "null" else quoteResult.ToString().ToLower()
    print ("F# Result: " + quoteResultString)
    let javascriptResult = run javascript

    let javascriptResult' =  if javascriptResult = null then "null" else javascriptResult.ToString().ToLower()

    let result = (quoteResultString = javascriptResult')
    
    print System.Environment.NewLine
    print ("javascript result: " + javascriptResult')
    Assert.IsTrue(result)