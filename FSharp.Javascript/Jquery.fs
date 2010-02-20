module FSharp.Javascript.Jquery

open FSharp.Javascript.Dom

type ajaxOptions = {
    url:string
    data:System.Object;
    dataType:string;
    success: System.Object -> unit;
}

let ajaxSettings = { url = ""; data = None; dataType = "HTML"; success = (fun x -> ()) }

type Jquery() =
    inherit System.Collections.Generic.List<HtmlElement>()
    member this.attr(x:string) = new Jquery()
    member this.addClass(x:string) = new Jquery()
    member this.find(x:string) = new Jquery()
    member this.parent() = new Jquery()
    member this.parents(x:string) = new Jquery()
    member this.ready(x) = new Jquery()
    member this.html(x) = new Jquery()
    member this.ajax(x:ajaxOptions) = new Jquery()
    member this.load(x:string) = new Jquery()




let jquery x = new Jquery()