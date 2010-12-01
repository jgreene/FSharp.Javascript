module FSharp.Javascript.Canvas

open FSharp.Javascript.Dom

type HtmlCanvasElement() =
    inherit HtmlElement()

    [<DefaultValue>]
    val mutable width : float
    [<DefaultValue>]
    val mutable height : float

    member this.toDataURL() = ""

    member this.getContext(contextId:string) = CanvasRenderingContext2d()

and Context() = class end
    
and CanvasRenderingContext2d() =
    inherit Context()

    [<DefaultValue>]
    val mutable canvas : HtmlCanvasElement
    [<DefaultValue>]
    val mutable globalAlpha : float
    [<DefaultValue>]
    val mutable globalCompositeOperation : string
    [<DefaultValue>]
    val mutable strokeStyle : obj
    [<DefaultValue>]
    val mutable fillStyle : obj
    [<DefaultValue>]
    val mutable lineWidth : float
    [<DefaultValue>]
    val mutable lineCap : string
    [<DefaultValue>]
    val mutable lineJoin : string
    [<DefaultValue>]
    val mutable miterLimit : float
    [<DefaultValue>]
    val mutable shadowOffsetX : float
    [<DefaultValue>]
    val mutable shadowOffsetY : float
    [<DefaultValue>]
    val mutable shadowBlur : float
    [<DefaultValue>]
    val mutable shadowColor : string
    [<DefaultValue>]
    val mutable font : string
    [<DefaultValue>]
    val mutable textAlign : string
    [<DefaultValue>]
    val mutable textBaseline : string

    
    member this.save() = ()
    member this.restore() = ()
    member this.scale(x:float, y:float) = ()
    member this.rotate(angle:float) = ()
    member this.translate(x:float, y:float) = ()
    member this.transform(a:float, b: float, c:float, d:float, e:float, f:float) = ()
    member this.setTransform(a:float, b: float, c:float, d:float, e:float, f:float) = ()

    member this.createLinearGradient(x0:float, y0:float, x1:float, y1:float) = CanvasGradient()
    member this.createRadialGradient(x0:float, y0:float, r0:float, x1:float, y1:float, r1:float) = CanvasGradient()
    member this.createPattern(image:HtmlImageElement, repitition:string) = CanvasPattern()
    member this.createPattern(image:HtmlCanvasElement, repitition:string) = CanvasPattern()
    member this.createPattern(image:HtmlVideoElement, repitition:string) = CanvasPattern()

    member this.clearRect(x:float, y:float, w:float, h:float) = ()
    member this.fillRect(x:float, y:float, w:float, h:float) = ()
    member this.strokeRect(x:float, y:float, w:float, h:float) = ()

    member this.beginPath() = ()
    member this.closePath() = ()
    member this.moveTo(x:float, y:float) = ()
    member this.lineTo(x:float, y:float) = ()
    member this.quadraticCurveTo(cpx:float, cpy:float, x:float, y:float) = ()
    member this.bezierCurveTo(cp1x:float, cp1y:float, cp2x:float, cp2y:float, x:float, y:float) = ()
    member this.arcTo(x1:float, y1:float, x2:float, y2:float, radius:float) = ()
    member this.rect(x:float, y:float, w:float, h:float) = ()
    member this.arc(x:float, y:float, radius:float, startAngle:float, endAngle:float) = ()
    member this.arc(x:float, y:float, radius:float, startAngle:float, endAngle:float, anticlockwise:bool) = ()
    member this.fill() = ()
    member this.stroke() = ()
    member this.clip() = ()
    member this.isPointInPath(x:float, y:float) = ()
    member this.drawFocusRing(element:Element, xCaret:float, yCaret:float) = true
    member this.drawFocusRing(element:Element, xCaret:float, yCaret:float, canDrawCustom:bool) = true
    member this.fillText(text:string, x:float, y:float) = ()
    member this.fillText(text:string, x:float, y:float, maxWidth:float) = ()
    member this.strokeText(text:string, x:float, y:float) = ()
    member this.strokeText(text:string, x:float, y:float, maxWidth:float) = ()
    member this.measureText(text:string) = TextMetrics()
    member this.drawImage(image:HtmlImageElement, dx:float, dy:float) = ()
    member this.drawImage(image:HtmlImageElement, dx:float, dy:float, dw:float, dh:float) = ()
    member this.drawImage(image:HtmlImageElement, sx:float, sy:float, sw:float, sh:float, dx:float, dy:float, dw:float, dh:float) = ()

    member this.drawImage(image:HtmlCanvasElement, dx:float, dy:float) = ()
    member this.drawImage(image:HtmlCanvasElement, dx:float, dy:float, dw:float, dh:float) = ()
    member this.drawImage(image:HtmlCanvasElement, sx:float, sy:float, sw:float, sh:float, dx:float, dy:float, dw:float, dh:float) = ()

    member this.drawImage(image:HtmlVideoElement, dx:float, dy:float) = ()
    member this.drawImage(image:HtmlVideoElement, dx:float, dy:float, dw:float, dh:float) = ()
    member this.drawImage(image:HtmlVideoElement, sx:float, sy:float, sw:float, sh:float, dx:float, dy:float, dw:float, dh:float) = ()

    member this.createImageData(sw:float, sh:float) = ImageData()
    member this.createImageData(imagedata:ImageData) = ImageData()
    member this.getImageData(sx:float, sy:float, sw:float, sh:float) = ImageData()
    member this.putImageData(imagedata:ImageData, dx:float, dy:float, dirtyX:float, dirtyY:float, dirtyWidth:float, dirtyHeight:float) = ()

    
and TextMetrics() =
    [<DefaultValue>]
    val mutable width : float

and CanvasGradient() =
    member this.addColorStop(offset:float, color:string) = ()

and CanvasPattern() = class end

and ImageData() =
    [<DefaultValue>]
    val mutable width : float
    [<DefaultValue>]
    val mutable height : float
    [<DefaultValue>]
    val mutable data : float array

let getCanvasById (id:string) = HtmlCanvasElement()



