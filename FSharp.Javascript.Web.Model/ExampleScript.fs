module ExampleScript

open FSharp.Javascript.Dom
open FSharp.Javascript.Jquery

[<ReflectedDefinition>]
let rec factorial n =
    if n=0 then 1 else n * factorial(n - 1)

[<ReflectedDefinition>]
let init() = jquery(document).ready(fun x -> let result = factorial 2
                                             jquery("#output").html(result) |> ignore
                                             

                                             ) |> ignore
