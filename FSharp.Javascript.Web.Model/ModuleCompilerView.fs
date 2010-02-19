namespace FSharp.Javascript.Web.Model

type ModuleCompilerView() =
    let mutable fsharp = ""
    let mutable javascript = ""
    let mutable moduleName = ""

    member this.FSharp
        with get() = fsharp
        and set(value) = fsharp <- value

    member this.Javascript
        with get() = javascript
        and set(value) = javascript <- value


    member this.ModuleName
        with get() = moduleName
        and set(value) = moduleName <- value
