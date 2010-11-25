module FSharp.Javascript.Dom

type Window() =
    [<DefaultValue>]
    val mutable closed : bool
    [<DefaultValue>]
    val mutable defaultStatus : string
    [<DefaultValue>]
    val mutable document : Document
    [<DefaultValue>]
    val mutable frameElement : HtmlElement
    [<DefaultValue>]
    val mutable frames : System.Array
    [<DefaultValue>]
    val mutable history : History
    [<DefaultValue>]
    val mutable innerHeight : float
    [<DefaultValue>]
    val mutable innerWidth : float
    [<DefaultValue>]
    val mutable length : float
    [<DefaultValue>]
    val mutable location : Location
    [<DefaultValue>]
    val mutable name : string
    [<DefaultValue>]
    val mutable navigator : Navigator
    [<DefaultValue>]
    val mutable opener : Window
    [<DefaultValue>]
    val mutable outerHeight : float
    [<DefaultValue>]
    val mutable outerWidth : float
    [<DefaultValue>]
    val mutable parent : Window
    [<DefaultValue>]
    val mutable screen : Screen
    [<DefaultValue>]
    val mutable self : Window
    [<DefaultValue>]
    val mutable status : string
    [<DefaultValue>]
    val mutable top : Window
    [<DefaultValue>]
    val mutable window : Window

    member this.alert(x) = ()
    member this.attachEvent(event:string, notify) = ()
    member this.blur() = ()
    member this.captureEvents(x) = ()
    member this.clearInterval(x:float) = ()
    member this.clearTimeout(x:float) = ()
    member this.close() = ()
    member this.confirm(x) = true
    member this.createPopup() = new Window()
    member this.detachEvent(event:string, notify) = ()
    member this.focus() = ()
    member this.moveBy(x:float, y:float) = ()
    member this.moveTo(x:float, y:float) = ()
    member this.navigate(url:string) = ()
    member this.``open``(url:string, windowName:string, features:string) = new Window()
    member this.print() = ()
    member this.prompt(message:string, defaultReply:string) = ""
    member this.resizeBy() = ()
    member this.resizeTo() = ()
    member this.ScriptEngineMajorVersion() = ""
    member this.ScriptEngineMinorVersion() = ""
    member this.scroll() = ()
    member this.scrollBy() = ()
    member this.scrollTo() = ()
    member this.setInterval(func:unit -> unit, timeout:float) = float 0
    member this.setTimeout(func:unit -> unit, timeout:float) = float 0
    member this.showModalDialog(url:string) = new Window()
    member this.showModelessDialog(url:string) = new Window()

and Document() =
    [<DefaultValue>]
    val mutable documentElement : Element
    [<DefaultValue>]
    val mutable anchors : HtmlElement array
    [<DefaultValue>]
    val mutable applets : Node array
    [<DefaultValue>]
    val mutable body : HtmlElement
    [<DefaultValue>]
    val mutable cookie : string
    [<DefaultValue>]
    val mutable domain : string
    [<DefaultValue>]
    val mutable embeds : System.Array
    [<DefaultValue>]
    val mutable forms : HtmlElement array
    [<DefaultValue>]
    val mutable frames : HtmlElement array
    [<DefaultValue>]
    val mutable images : HtmlElement array
    [<DefaultValue>]
    val mutable layers : System.Array
    [<DefaultValue>]
    val mutable links : HtmlElement array
    [<DefaultValue>]
    val mutable location : Location
    [<DefaultValue>]
    val mutable parentWindow : Window
    [<DefaultValue>]
    val mutable plugins : System.Array
    [<DefaultValue>]
    val mutable referrer : string
    [<DefaultValue>]
    val mutable scripts : System.Array
    [<DefaultValue>]
    val mutable styleSheets : System.Array
    [<DefaultValue>]
    val mutable title : string
    [<DefaultValue>]
    val mutable uniqueID : string
    [<DefaultValue>]
    val mutable url : string
    [<DefaultValue>]
    val mutable window : Window

    member this.createElement(tagName:string) = Element()
    member this.createTextNode(data:string) = Text()
    
    member this.write(text:string) = ()
    member this.getElementById(x:string) = HtmlElement()
    member this.getElementsByTagName(tagName:string) = [||] : HtmlElement array
    member this.getElementsByName(elementName:string) = [||] : HtmlElement array


    member this.attachEvent(event:string, x) = true
    member this.captureEvents(x) = ()
    member this.close() = ()
    member this.createStyleSheet(url:string, index:float) = StyleSheet()
    member this.``open``() = ()
    member this.writeln(text:string) = ()

and StyleSheet() =
    [<DefaultValue>]
    val mutable href : string
    [<DefaultValue>]
    val mutable rules : System.Array

    member this.addRule(selector:string, style:string, index:float) = float 0
    member this.removeRule(index:float) = ()

and Node() =
    [<DefaultValue>]
    val mutable attributes : Node array
    [<DefaultValue>]
    val mutable childNodes : Node array
    [<DefaultValue>]
    val mutable firstChild : Node
    [<DefaultValue>]
    val mutable lastChild : Node
    [<DefaultValue>]
    val mutable nextSibling : Node
    [<DefaultValue>]
    val mutable nodeName : Node
    [<DefaultValue>]
    val mutable nodeType : float
    [<DefaultValue>]
    val mutable nodeValue : string
    [<DefaultValue>]
    val mutable ownderDocument : Document
    [<DefaultValue>]
    val mutable parentElement : Node
    [<DefaultValue>]
    val mutable parentNode : Node
    [<DefaultValue>]
    val mutable previousSibling : Node

    member this.appendChild(newChild:Node) = Node()
    member this.cloneNode(b:bool) = Node()
    member this.hasChildNodes() = false
    member this.insertBefore(newChild:Node, refChild:Node) = Node()
    member this.removeChild(oldChild:Node) = Node()
    member this.removeNode(removeChildren:bool) = Node()
    member this.replaceChild(newChild:Node, refChild:Node) = Node()

and Element() =
    inherit Node()

    [<DefaultValue>]
    val mutable tagName : string

    member this.getAttribute(name:string) = ""
    member this.getElementsByTagName(name:string) = [||] : HtmlElement array
    member this.normalize() = ()
    member this.removeAttribute(name:string) = ()
    member this.setAttribute(name:string, value:string) = ()

and HtmlElement() =
    inherit Element()

    [<DefaultValue>]
    val mutable children : HtmlElement array
    [<DefaultValue>]
    val mutable className : string
    [<DefaultValue>]
    val mutable dir : string
    [<DefaultValue>]
    val mutable document : Document
    [<DefaultValue>]
    val mutable id : string
    [<DefaultValue>]
    val mutable innerHTML : string
    [<DefaultValue>]
    val mutable innerText : string
    [<DefaultValue>]
    val mutable lang : string
    [<DefaultValue>]
    val mutable offsetHeight : float
    [<DefaultValue>]
    val mutable offsetLeft : float
    [<DefaultValue>]
    val mutable offsetParent : float
    [<DefaultValue>]
    val mutable offsetTop : float
    [<DefaultValue>]
    val mutable offsetWidth : float
    [<DefaultValue>]
    val mutable title : string
    [<DefaultValue>]
    val mutable uniqueID : string

    member this.addBehavior(url:string) = float 0
    member this.attachEvent(event:string, x) = true
    member this.detachEvent(event:string, x) = ()
    member this.insertAdjavacentHTML(where:string, html:string) = ()
    member this.removeBehavior(x:float) = true


and HtmlImageElement() =
    inherit HtmlElement()

    [<DefaultValue>]
    val mutable alt : string
    [<DefaultValue>]
    val mutable src : string
    [<DefaultValue>]
    val mutable useMap : string
    [<DefaultValue>]
    val mutable isMap : bool
    [<DefaultValue>]
    val mutable width : float
    [<DefaultValue>]
    val mutable height : float
    [<DefaultValue>]
    val mutable naturalWidth : float
    [<DefaultValue>]
    val mutable naturalHeight : float
    [<DefaultValue>]
    val mutable complete : bool

and MediaError() =
    [<DefaultValue>]
    val mutable MEDIA_ERR_ABORTED : float
    [<DefaultValue>]
    val mutable MEDIA_ERR_NETWORK : float
    [<DefaultValue>]
    val mutable MEDIA_ERR_DECODE : float
    [<DefaultValue>]
    val mutable MEDIA_ERR_SRC_NOT_SUPPORTED : float
    [<DefaultValue>]
    val mutable code : float

and TimeRanges() =
    [<DefaultValue>]
    val mutable length : float
    member this.start(index:float) = float 0
    member this.``end``(index:float) = float 0

and DocumentFragment() = class end

and TimedTrackCue() = 
    [<DefaultValue>]
    val mutable track : TimedTrack
    [<DefaultValue>]
    val mutable id : string
    [<DefaultValue>]
    val mutable startTime : float
    [<DefaultValue>]
    val mutable endTime : float
    [<DefaultValue>]
    val mutable pauseOnExit : bool
    [<DefaultValue>]
    val mutable direction : string
    [<DefaultValue>]
    val mutable snapToLines : bool
    [<DefaultValue>]
    val mutable linePosition : float
    [<DefaultValue>]
    val mutable textPosition : float
    [<DefaultValue>]
    val mutable size : float
    [<DefaultValue>]
    val mutable alignment : string
    [<DefaultValue>]
    val mutable voice : string

    member this.getCuseAsSource() = ""
    member this.getCueAsHtml() = DocumentFragment()

    [<DefaultValue>]
    val mutable onenter : unit -> unit
    [<DefaultValue>]
    val mutable onexit : unit -> unit

and TimedTrackCueList() =
    [<DefaultValue>]
    val mutable length : float
    
    member this.getCueById(id:string) = TimedTrackCue()
and TimedTrack() =

    [<DefaultValue>]
    val mutable kind : string
    [<DefaultValue>]
    val mutable label : string
    [<DefaultValue>]
    val mutable language : string
    [<DefaultValue>]
    val mutable NONE : float
    [<DefaultValue>]
    val mutable LOADING : float
    [<DefaultValue>]
    val mutable LOADED : float
    [<DefaultValue>]
    val mutable ERROR : float
    [<DefaultValue>]
    val mutable readyState : float
    [<DefaultValue>]
    val mutable onload : unit -> unit
    [<DefaultValue>]
    val mutable onerror : unit -> unit
    [<DefaultValue>]
    val mutable OFF : float
    [<DefaultValue>]
    val mutable HIDDEN : float
    [<DefaultValue>]
    val mutable SHOWING : float
    [<DefaultValue>]
    val mutable mode : float
    [<DefaultValue>]
    val mutable cues : TimedTrackCueList
    [<DefaultValue>]
    val mutable activeCues : TimedTrackCueList
    [<DefaultValue>]
    val mutable oncuechange : unit -> unit

and MutableTimedTrack() =
    inherit TimedTrack()

    member this.addCue(cue:TimedTrackCue) = ()
    member this.removeCue(cue:TimedTrackCue) = ()

and HtmlMediaElement() =
    inherit HtmlElement()

    [<DefaultValue>]
    val mutable error : MediaError
    [<DefaultValue>]
    val mutable src : string
    [<DefaultValue>]
    val mutable NETWORK_EMPTY : float
    [<DefaultValue>]
    val mutable NETWORK_IDLE : float
    [<DefaultValue>]
    val mutable NETWORK_LOADING : float
    [<DefaultValue>]
    val mutable NETWORK_NO_SOURCE : float
    [<DefaultValue>]
    val mutable networkState : float
    [<DefaultValue>]
    val mutable preload : string
    [<DefaultValue>]
    val mutable buffered : TimeRanges
    [<DefaultValue>]
    val mutable HAVE_NOTHING : float
    [<DefaultValue>]
    val mutable HAVE_METADATA : float
    [<DefaultValue>]
    val mutable HAVE_CURRENT_DATA : float
    [<DefaultValue>]
    val mutable HAVE_FUTURE_DATA : float
    [<DefaultValue>]
    val mutable HAVE_ENOUGH_DATA : float
    [<DefaultValue>]
    val mutable readyState : float
    [<DefaultValue>]
    val mutable seeking : bool
    [<DefaultValue>]
    val mutable currentTime : float
    [<DefaultValue>]
    val mutable initialTime : float
    [<DefaultValue>]
    val mutable duration : float
    [<DefaultValue>]
    val mutable startOffsetTime : System.DateTime
    [<DefaultValue>]
    val mutable paused : bool
    [<DefaultValue>]
    val mutable defaultPlaybackRate : float
    [<DefaultValue>]
    val mutable playbackRate : float
    [<DefaultValue>]
    val mutable played : TimeRanges
    [<DefaultValue>]
    val mutable seekable : TimeRanges
    [<DefaultValue>]
    val mutable ended : bool
    [<DefaultValue>]
    val mutable autoplay : bool
    [<DefaultValue>]
    val mutable loop : bool
    [<DefaultValue>]
    val mutable controls : bool
    [<DefaultValue>]
    val mutable volumne : float
    [<DefaultValue>]
    val mutable muted : bool
    [<DefaultValue>]
    val mutable tracks : TimedTrack array



    member this.load() = ()
    member this.canPlayType(typ:string) = ""
    member this.play() = ()
    member this.pause() = ()
    member this.addTrack(kind:string) = MutableTimedTrack()

and HtmlVideoElement() =
    inherit HtmlMediaElement()

    [<DefaultValue>]
    val mutable width : float
    [<DefaultValue>]
    val mutable height : float
    [<DefaultValue>]
    val mutable videoWidth : float
    [<DefaultValue>]
    val mutable videoHeight : float
    [<DefaultValue>]
    val mutable poster : string

and CharacterData() =
    inherit Node()

    [<DefaultValue>]
    val mutable data : string
    [<DefaultValue>]
    val mutable length : float

    member this.appendData(arg:string) = ()
    member this.deleteData(offset:float, count:float) = ()
    member this.insertData(offset:float, arg:string) = ()
    member this.replaceData(offset:float, count:float, arg:string) = ()
    member this.substringData(offset:float, count:float) = ""

and Text() =
    inherit CharacterData()

    member this.splitText(offset:float) = Text()

and History() =
    [<DefaultValue>]
    val mutable current : string
    [<DefaultValue>]
    val mutable next : string
    [<DefaultValue>]
    val mutable previous : string

    member this.back() = ()
    member this.forward() = ()
    member this.go(x) = ()

and Location() =
    [<DefaultValue>]
    val mutable hash : string
    [<DefaultValue>]
    val mutable host : string
    [<DefaultValue>]
    val mutable hostname : string
    [<DefaultValue>]
    val mutable href : string
    [<DefaultValue>]
    val mutable pathname : string
    [<DefaultValue>]
    val mutable port : string
    [<DefaultValue>]
    val mutable protocol : string
    [<DefaultValue>]
    val mutable search : string

    member this.assign(url:string) = ()
    member this.reload(b:bool) = ()
    member this.replace(url:string) = ()

and Navigator() =
    [<DefaultValue>]
    val mutable appCodeName : string
    [<DefaultValue>]
    val mutable appMinorVersion : string
    [<DefaultValue>]
    val mutable appName : string
    [<DefaultValue>]
    val mutable appVersion : string
    [<DefaultValue>]
    val mutable browserLanguage : string
    [<DefaultValue>]
    val mutable cookieEnabled : bool
    [<DefaultValue>]
    val mutable cpuClass : string
    [<DefaultValue>]
    val mutable isWebKing : bool
    [<DefaultValue>]
    val mutable mimeTypes : System.Array
    [<DefaultValue>]
    val mutable online : bool
    [<DefaultValue>]
    val mutable platform : string
    [<DefaultValue>]
    val mutable plugins : Plugin
    [<DefaultValue>]
    val mutable userAgent : string

    member this.getContext() = new WebKingContext()
    member this.javaEnabled() = false


and Plugin() =
    member this.refresh() = ()
and WebKingContext() =
    member this.clear() = ()
    member this.get(key:string) = new obj()
    member this.getContext(typ:string) = new WebKingContext()
    member this.getContextContaining(name:string) = new WebKingContext()
    member this.getContextName() = ""
    member this.getContextType() = ""
    member this.getKeys() = seq { yield "" }
    member this.getNamedContext(name:string) = ()
    member this.getParentContext() = ()
    member this.put(key:string, value:obj) = ()

and Screen() =
    [<DefaultValue>]
    val mutable availHeight : float
    [<DefaultValue>]
    val mutable availLeft : float
    [<DefaultValue>]
    val mutable availTop : float
    [<DefaultValue>]
    val mutable availWidth : float
    [<DefaultValue>]
    val mutable colorDepth : float
    [<DefaultValue>]
    val mutable height : float
    [<DefaultValue>]
    val mutable pixelDepth : float
    [<DefaultValue>]
    val mutable width : float

and Math() =
    [<DefaultValue>]
    val mutable E : float
    [<DefaultValue>]
    val mutable LN2 : float
    [<DefaultValue>]
    val mutable LN10 : float
    [<DefaultValue>]
    val mutable LOG2E : float
    [<DefaultValue>]
    val mutable LOG10E : float
    [<DefaultValue>]
    val mutable PI : float
    [<DefaultValue>]
    val mutable SQRT1_2 : float
    [<DefaultValue>]
    val mutable SQRT2 : float

    member this.abs(x:float) = float 0
    member this.acos(x:float) = float 0
    member this.asin(x:float) = float 0
    member this.atan(x:float) = float 0
    member this.atan2(x:float) = float 0
    member this.ceil(x:float) = float 0
    member this.cose(x:float) = float 0
    member this.exp(x:float) = float 0
    member this.floor(x:float) = float 0
    member this.log(x:float) = float 0
    member this.pow(x,y) = float 0
    member this.random() = float 0
    member this.round(x:float) = float 0
    member this.sin(x:float) = float 0
    member this.sqrt(x:float) = float 0
    member this.tan(x:float) = float 0

let window = new Window()
let document = window.document

let Math = new Math()

let alert x = window.alert(x)

