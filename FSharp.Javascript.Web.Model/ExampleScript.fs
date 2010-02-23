module ExampleScript

open FSharp.Javascript.Dom
open FSharp.Javascript.Jquery

[<ReflectedDefinition>]
let rec factorial n =
    if n=0 then 1 else n * factorial(n - 1)

type myOptions = {
    success: System.Object -> unit;
    dataType: string;
    url:string;
}

[<ReflectedDefinition>]
let print x = jquery("#output").html(x) |> ignore

[<ReflectedDefinition>]
let ajax() = jquery(document).ready(fun x -> jquery.ajax({ success = (fun x -> jquery("#output").html(x) |> ignore); dataType = "HTML"; url = "/home/index" }))

[<ReflectedDefinition>]
let click() = jquery(document).ready(fun x -> jquery("#output").html("<a id='tempElement' href='#'>click here</a>").click(fun y -> jquery("#output").html("clicked")))

[<ReflectedDefinition>]
let fact() = let result = factorial 2
             print result

[<ReflectedDefinition>]
let init() = jquery(document).ready(fun x -> click() )
