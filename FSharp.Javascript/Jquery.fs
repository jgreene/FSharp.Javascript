module FSharp.Javascript.Jquery

open FSharp.Javascript.Dom

type Jquery() =
    inherit System.Collections.Generic.List<HtmlElement>()
    member this.attr(x:string) = new Jquery()
    member this.addClass(x:string) = new Jquery()
    member this.find(x:string) = new Jquery()
    member this.parent() = new Jquery()
    member this.parents(x:string) = new Jquery()
    member this.ready(x) = new Jquery()
    member this.html(x) = new Jquery()
    member this.ajax(x) = new Jquery()
    member this.load(x:string) = new Jquery()




let jquery x = new Jquery()