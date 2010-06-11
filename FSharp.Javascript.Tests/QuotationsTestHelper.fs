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

open System.Linq

let print input =
    System.Console.WriteLine((sprintf "%A" input))

//[<ReflectedDefinition>]
let emit x = x

let run (source:string) =
    let emitter = new StringBuilder()
    let context = new IronJS.Runtime.Context()
    let astBuilder = new IronJS.Compiler.AstGenerator()
    let etGenerator = new IronJS.Compiler.EtGenerator()

    let scope = IronJS.Runtime.Js.Scope.CreateGlobal(context)
    let func x = emitter.Append(IronJS.Runtime.Utils.JsTypeConverter.ToString(x))
    let emit = Microsoft.FSharp.Core.FSharpFunc<System.Object,StringBuilder>.ToConverter(func)
    scope.Global("emit", emit) |> ignore

    context.SetupGlobals(scope)
    let astNodes = astBuilder.Build(source)
    let compiled = etGenerator.Build(astNodes, context)

    compiled.Invoke(scope)

    emitter.ToString();

//let run (source:string) =
//    let emitter = new StringBuilder()
//    let astGenerator = new IronJS.Compiler.IjsAstGenerator()
//    let astNodes = astGenerator.Build(source);
//    let globalScope = IronJS.Compiler.Ast.GlobalScope.Create(astNodes).Analyze()
//
//    let context = new IronJS.IjsContext()
//    
//    let func x = emitter.Append(if x <> null then x.ToString() else "null")
//    let emit = Microsoft.FSharp.Core.FSharpFunc<System.Object,StringBuilder>.ToConverter(func)
//    context.GlobalScope.Set("emit", emit)
//
//    let compiled = globalScope.Compile(context)
//
//    let result = compiled.Invoke(context.GlobalClosure)
//
//    emitter.ToString();

let testWithType (ty:System.Type) quote =
    let testModule = true
    print quote
    let ast = convertToAst quote
    print ast
    let j1 = (getJavascript ast)
    let moduleAst = getAstFromType ty 
    let j2 = getJavascript moduleAst
    let library = System.IO.File.ReadAllText("fsharp.js") + System.Environment.NewLine + System.IO.File.ReadAllText("tests.js")
    let javascript = (library + System.Environment.NewLine + j2 + System.Environment.NewLine + System.Environment.NewLine + j1)
    print (j2 + j1)
    //print javascript
    let quoteResult = quote.EvalUntyped();
    let quoteResultString = if quoteResult = null then "null" else quoteResult.ToString().ToLower()
    print quoteResultString
    let javascriptResult = run javascript

    let result = (quoteResultString = javascriptResult.ToLower())
    print (quoteResultString + System.Environment.NewLine +  javascriptResult)
    Assert.IsTrue(result)