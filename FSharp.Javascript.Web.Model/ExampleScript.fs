module ExampleScript

open FSharp.Javascript.Dom
open FSharp.Javascript.Jquery

[<ReflectedDefinition>]
let print x = 
    let output = jquery("#output")
    let currentHtml = output.html()
    output.html(currentHtml + x.ToString()) |> ignore

[<ReflectedDefinition>]
let (|NullCheckPattern|_|) x =
    match x with
    | null -> None
    | _ -> Some(x)

[<ReflectedDefinition>]
let activePatternMatch() = let value = "a string"
                           match value with
                           | NullCheckPattern(x) -> print "Was not null"
                           | _ -> print "Was null"

[<ReflectedDefinition>]
let init() = jquery(document)
              .ready(fun x -> 
               activePatternMatch())

[<ReflectedDefinition>]
let rec factorial n =
    if n=0 then 1 else n * factorial(n - 1)

[<ReflectedDefinition>]
let fact() = 
    let result = factorial 2
    print result
             


type myOptions = {
    success: System.Object -> unit;
    dataType: string;
    url:string;
}

[<ReflectedDefinition>]
let ajax() = jquery(document)
              .ready(fun x -> 
               jquery.ajax({ success = (fun x -> print x |> ignore); dataType = "HTML"; url = "/home/index" }))



[<ReflectedDefinition>]
let click() = jquery(document)
                .ready(fun x -> 
                 jquery("#output")
                  .html("<a id='tempElement' href='#'>click here</a>")
                   .bind("click", 
                    fun y -> print "clicked"))

open FSharp.Javascript.Converter

let javascript = convertModule (System.Type.GetType("MyModule, MyAssembly"))


