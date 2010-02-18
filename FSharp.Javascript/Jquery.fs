module IronJS.Printer.Jquery

type Jquery() =
    member this.attr(x:string) = new Jquery()
    member this.addClass(x:string) = new Jquery()
    member this.find(x:string) = new Jquery()
    member this.parent() = new Jquery()
    member this.parents(x:string) = new Jquery()

let jquery (x:string) = new Jquery()