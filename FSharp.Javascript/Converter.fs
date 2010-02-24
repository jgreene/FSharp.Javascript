module FSharp.Javascript.Converter

let convert (quote:Microsoft.FSharp.Quotations.Expr) =
    let ast = QuotationsConverter.convertToAst quote
    Printer.getJavascript ast

let convertModule (typ:System.Type) =
    let ast = ModuleCompiler.getAstFromType typ
    Printer.getJavascript ast