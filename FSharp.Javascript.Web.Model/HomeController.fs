namespace FSharp.Javascript.Web.Controllers

open System.Web.Mvc
open FSharp.Javascript.Web.Model

type HomeController() =
    inherit Controller()

    
    member this.Index() =
        let view = new ModuleCompilerView()
        base.View(view)

    [<ValidateInput(false)>]
    member this.Submit(view : ModuleCompilerView) =
        let result = FSharp.Javascript.Compiler.compile view.FSharp (this.Server.MapPath("~/TempAssemblies/"))
        let errors = fst result
        let javascript = snd result
        if javascript.IsSome then
            let getModuleName (script:string) =
                let script = script.Replace("var", "").Trim()
                let e = script.IndexOf(" ")
                let moduleName = script.Substring(0, e)
                moduleName

            view.ModuleName <- getModuleName javascript.Value
            view.Javascript <- javascript.Value
            base.View("Index", view)
        else
            [for e in errors do yield (this.ModelState.AddModelError("FSharp", e.ErrorText) |> ignore)] |> ignore

            base.View("Index", view)