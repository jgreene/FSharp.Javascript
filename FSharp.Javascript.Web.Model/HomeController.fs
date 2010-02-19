namespace FSharp.Javascript.Web.Controllers

open System.Web.Mvc
open FSharp.Javascript.Web.Model

type HomeController() =
    inherit Controller()

    
    member this.Index() =
        let view = new ModuleCompilerView()
        view.FSharp <- "module ExampleScript

open FSharp.Javascript.Dom
open FSharp.Javascript.Jquery

[<ReflectedDefinition>]
let rec factorial n =
    if n=0 then 1 else n * factorial(n - 1)

[<ReflectedDefinition>]
let init() = jquery(document).ready(fun x -> let result = factorial 2
                                             jquery(\"#output\").html(result)) |> ignore"
        base.View(view)

    [<ValidateInput(false)>]
    member this.Submit(view : ModuleCompilerView) =
        let result = FSharp.Javascript.Compiler.compile view.FSharp (this.Server.MapPath("~/TempAssemblies/"))
        let errors = fst result
        let javascript = snd result
        if javascript.IsSome then
            let getModuleName (script:string) =
                let script = script.Replace("var", "").Trim()
                let moduleName = script.Substring(0, script.IndexOf(" "))
                moduleName

            view.ModuleName <- getModuleName javascript.Value
            view.Javascript <- javascript.Value
            base.View("Index", view)
        else
            [for e in errors do yield (this.ModelState.AddModelError("FSharp", e.ErrorText) |> ignore)] |> ignore

            base.View("Index", view)