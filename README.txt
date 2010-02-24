You can play with the converter at http://fscript.justsimplecode.com

To get up and running with FSharp.Javascript

Reference the FSharp.Javascript.dll
For simple quotation conversion use the below code:


open FSharp.Javascript.Converter
open FSharp.Javascript.Dom

let javascript = convert <@ let i = 0
                            alert(i) @>

To convert a module to javascript:

open FSharp.Javascript.Converter

let javascript = convertModule (System.Type.GetType("MyModule, MyAssembly"))

The module compiler does not support converting standalone initialization calls, e.g.:

module MyModule

[<ReflectedDefinition>]
let func() = 1

//this will not show up in the converted javascript
func()

This is because the F# quotations system does not support this feature.

Also remember that all function definitions that you want to convert must be decorated with the [<ReflectedDefinition>] attribute.


You can also use jquery with

open FSharp.Javascript.Jquery