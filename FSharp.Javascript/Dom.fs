module FSharp.Javascript.Dom

type Window() =
    member this.document = new Document()
    member this.alert(x) = ()
    member this.clearInterval(x) = ()
    member this.confirm(x) = true

and Document() =
    member this.window = new Window()
    member this.write(x) = System.Console.WriteLine(x.ToString())
    member this.getElementById(x:string) = ()

and HtmlElement() =
    member this.children : HtmlElement list = []
    member this.innerHTML = ""
    member this.innerText = ""

let window = new Window()
let document = window.document

let alert x = window.alert(x)

