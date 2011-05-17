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

open Microsoft.FSharp.Control

open System.Diagnostics
open System.IO

open System.Linq

open IronJS

let print input =
    System.Console.WriteLine((sprintf "%A" input))

//[<ReflectedDefinition>]
let emit x = x



let getRandomFileName () = System.Guid.NewGuid().ToString() + ".js"

let run (source:string) =
    
    let fileName = getRandomFileName ()
    let filePath = "C:\\cygwin\\home\\nephesh\\" + fileName
    File.WriteAllText(filePath, source)

    let info = new ProcessStartInfo("C:\\cygwin\\bin\\bash", "--login -i -c \"node " + fileName + "\"")
    info.CreateNoWindow <- true
    info.UseShellExecute <- false
    info.WindowStyle <- ProcessWindowStyle.Hidden
    info.RedirectStandardOutput <- true

    use proc = Process.Start(info)
    
    let testResult = proc.StandardOutput.ReadLine()
    proc.WaitForExit()

    File.Delete(filePath)

    testResult

//let run (source:string) =
//    let emitter = new StringBuilder()
//    let func x = emitter.Append(x.ToString()) |> ignore
//    let ctx = IronJS.Hosting.FSharp.createContext()
//    let env = ctx |> IronJS.Hosting.FSharp.env 
//    let exitFunc = new System.Action(func) |> Native.Utils.createFunction env (Some(1))
//    (IronJS.Hosting.FSharp.execute source ctx) |> ignore
//    emitter.ToString()

//type mydel = delegate of obj -> StringBuilder
//
//let run (source:string) =
//    let emitter = new StringBuilder()
//    let func x = emitter.Append(x.ToString())
//    let testDel = new mydel(func)
//    let engine = new Jurassic.ScriptEngine()
//    engine.SetGlobalFunction("test", testDel)
//    engine.Execute(source)
//    emitter.ToString()

//type mydel = delegate of obj -> StringBuilder
//
//let run (source:string) =
//    let emitter = new StringBuilder()
//    let func x = emitter.Append(x.ToString())
//    let testDel = new mydel(func)
//    let engine = new JintEngine()
//    engine.SetFunction("emit", testDel).Run(source) |> ignore
//    emitter.ToString()

let testWithType (typs:System.Type list) quote =
    let testModule = true
    print quote
    print "--------------------------------------------------------------"
    let ast = convertToAst quote
    print ast
    print "--------------------------------------------------------------"
    let j1 = (getJavascript ast)

    let moduleScript = typs |> List.fold (fun acc next -> (FSharp.Javascript.Converter.convertModule next) + System.Environment.NewLine + acc) ""

    let files = ["FSharp.Javascript.js";"tests.js";"FSharp.Javascript.Library.js"]

    let library = files |> List.fold (fun acc next -> System.IO.File.ReadAllText(next) + System.Environment.NewLine + acc) ""

    //let library = System.IO.File.ReadAllText("fsharp.js") + System.Environment.NewLine + System.IO.File.ReadAllText("tests.js")
    let javascript = (library + System.Environment.NewLine + moduleScript + System.Environment.NewLine + System.Environment.NewLine + j1)
    print moduleScript
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
                                