﻿module FSharp.Javascript.Jquery

//current version 1.6.1

open FSharp.Javascript.Dom

type browser() =
    [<DefaultValue>]
    val mutable webkit : bool
    [<DefaultValue>]
    val mutable msie : bool
    [<DefaultValue>]
    val mutable opera : bool
    [<DefaultValue>]
    val mutable mozilla : bool
    [<DefaultValue>]
    val mutable version : string

type event() =
    [<DefaultValue>]
    val mutable currentTarget : obj
    [<DefaultValue>]
    val mutable data : obj
    [<DefaultValue>]
    val mutable pageX : float
    [<DefaultValue>]
    val mutable pageY : float
    [<DefaultValue>]
    val mutable relatedTarget : HtmlElement
    [<DefaultValue>]
    val mutable result : obj
    [<DefaultValue>]
    val mutable target : HtmlElement
    [<DefaultValue>]
    val mutable timeStamp : obj
    [<DefaultValue>]
    val mutable ``type`` : obj
    [<DefaultValue>]
    val mutable which : obj
    [<DefaultValue>]
    val mutable keyCode : float
    [<DefaultValue>]
    val mutable ``namespace`` : string


    member this.isDefaultPrevented() = true
    member this.isImmediatePropagationStopped() = true
    member this.isPropagationStopped() = true
    member this.preventDefault() = ()
    member this.stopImmediatePropagation() = ()
    member this.stopPropagation() = ()

type position() =
    [<DefaultValue>]
    val mutable top : float
    [<DefaultValue>]
    val mutable left : float

type support() =
    [<DefaultValue>]
    val mutable boxModel : bool
    [<DefaultValue>]
    val mutable cssFloat : bool
    [<DefaultValue>]
    val mutable hrefNormalized : bool
    [<DefaultValue>]
    val mutable htmlSerialize : bool
    [<DefaultValue>]
    val mutable leadingWhitespace : bool
    [<DefaultValue>]
    val mutable noCloneEvent : bool
    [<DefaultValue>]
    val mutable objectAll : bool
    [<DefaultValue>]
    val mutable opacity : bool
    [<DefaultValue>]
    val mutable scriptEval : bool
    [<DefaultValue>]
    val mutable style : bool
    [<DefaultValue>]
    val mutable tbody : bool

type XMLHttpRequest() =
    member this.abort() = ()
    member this.getAllResponseHeaders() = ""
    member this.getResponseHeader(x) = ""
    member this.``open``(x,url) = ()
    member this.overrideMimeType(x) = ()
    member this.send() = ()
    member this.send(x) = ()
    member this.sendAsBinary(x) = ()
    member this.setRequestHeader(x, y) = ()

    [<DefaultValue>]
    val mutable multipart : bool
    [<DefaultValue>]
    val mutable readyState : float
    [<DefaultValue>]
    val mutable response : System.Object
    [<DefaultValue>]
    val mutable responseText : string
    [<DefaultValue>]
    val mutable responseType : string
    [<DefaultValue>]
    val mutable status : float
    [<DefaultValue>]
    val mutable responseXML : string
    [<DefaultValue>]
    val mutable statusText : string
    [<DefaultValue>]
    val mutable withCredentials : bool

    

type promise() =
    member this.``then``(x,y) = deferred()
    member this.``done``(x) = deferred()
    member this.``done``(x,y) = deferred()
    member this.fail(x) = deferred()
    member this.fail(x,y) = deferred()
    member this.always(x) = deferred()
    member this.pipe() = promise()
    member this.pipe(x) = promise()
    member this.pipe(x,y) = promise()

and deferred() =
    member this.always(x) = deferred()
    member this.``done``(x) = deferred()
    member this.``done``(x,y) = deferred()
    member this.fail(x) = deferred()
    member this.fail(x,y) = deferred()
    member this.isRejected() = true
    member this.isResolved() = false
    member this.pipe() = promise()
    member this.pipe(x) = promise()
    member this.pipe(x,y) = promise()
    member this.promise() = promise()
    member this.promise(x) = promise()
    member this.promise(x,y) = promise()
    member this.reject() = deferred()
    member this.reject(x) = deferred()
    member this.rejectWith(x) = deferred()
    member this.rejectWith(x,y) = deferred()
    member this.resolve() = deferred()
    member this.resolve(x) = deferred()
    member this.resolveWith(x) = deferred()
    member this.resolveWith(x,y) = deferred()
    member this.``then``(x,y) = deferred()
and jqXHR() =
    inherit XMLHttpRequest()
    member this.``then``(x,y) = deferred()
    member this.``done``(x) = deferred()
    member this.``done``(x,y) = deferred()
    member this.fail(x) = deferred()
    member this.fail(x,y) = deferred()
    member this.always(x) = deferred()
    member this.pipe() = promise()
    member this.pipe(x) = promise()
    member this.pipe(x,y) = promise()

type jquery(x) =
    inherit System.Collections.Generic.List<HtmlElement>()
    member this.add(x) = jquery(null)
    member this.addClass(x) = jquery(null)
    member this.after(x) = jquery(null)
    member this.ajaxComplete(x) = jquery(null)
    member this.ajaxError(x) = jquery(null)
    member this.ajaxSend(x) = jquery(null)
    member this.ajaxStart(x) = jquery(null)
    member this.ajaxStop(x) = jquery(null)
    member this.ajaxSuccess(x) = jquery(null)
    member this.andSelf() = jquery(null)
    member this.animate(x, y) = jquery(null)
    member this.animate(x,y,z) = jquery(null)
    member this.animate(x,y,z,q) = jquery(null)
    member this.append(x) = jquery(null)
    member this.appendTo(x) = jquery(null)
    member this.attr(x) = ""
    member this.before(x) = jquery(null)
    member this.before(x,y) = jquery(null)
    member this.bind(eventType) = jquery(null)
    member this.bind(x,y) = jquery(null)
    member this.bind(x,y,z) = jquery(null)
    member this.blur() = jquery(null)
    member this.blur(x) = jquery(null)
    member this.blur(x,y) = jquery(null)
    member this.change() = jquery(null)
    member this.change(x) = jquery(null)
    member this.change(x,y) = jquery(null)
    member this.children() = jquery(null)
    member this.children(selector) = jquery(null)
    member this.clearQueue() = jquery(null)
    member this.clearQueue(queueName) = jquery(null)
    member this.click() = jquery(null)
    member this.click(handler) = jquery(null)
    member this.click(x,y) = jquery(null)
    member this.clone() = jquery(null)
    member this.clone(x) = jquery(null)
    member this.clone(x,y) = jquery(null)
    member this.closest(x) = jquery(null)
    member this.closest(x,y) = jquery(null)
    member this.contents() = jquery(null)
    [<DefaultValue>]
    val mutable context : HtmlElement
    member this.css(x) = jquery(null)
    member this.css(x,y) = jquery(null)
    member this.data(key) = new System.Object()
    member this.data(key,value) = ()
    member this.dblclick() = jquery(null)
    member this.dblclick(x) = jquery(null)
    member this.dblclick(x,y) = jquery(null)
    member this.delay(x) = jquery(null)
    member this.delay(x,y) = jquery(null)
    member this.``delegate``(selector,events) = jquery(null)
    member this.``delegate``(selector,eventType,handler) = jquery(null)
    member this.``delegate``(selector,eventType,eventData,handler) = jquery(null)
    member this.dequeue() = jquery(null)
    member this.dequeue(x) = jquery(null)
    member this.detach() = jquery(null)
    member this.detach(selector) = jquery(null)
    member this.die() = jquery(null)
    member this.die(eventType) = jquery(null)
    member this.die(eventType,handler) = jquery(null)
    member this.each(callback) = jquery(null)
    member this.enableSelectioN() = jquery(null)
    member this.empty() = jquery(null)
    member this.``end``() = jquery(null)
    member this.eq(x) = jquery(null)
    member this.error(x) = jquery(null)
    member this.error(x,y) = jquery(null)
    member this.fadeIn() = jquery(null)
    member this.fadeIn(x) = jquery(null)
    member this.fadeIn(x,y) = jquery(null)
    member this.fadeIn(x,y,z) = jquery(null)
    member this.fadeOut() = jquery(null)
    member this.fadeOut(x) = jquery(null)
    member this.fadeOut(x,y) = jquery(null)
    member this.fadeOut(x,y,z) = jquery(null)
    member this.fadeTo(duration,opacity) = jquery(null)
    member this.fadeTo(duration,opacity,callback) = jquery(null)
    member this.fadeTo(duration,opacity,callback,y) = jquery(null)
    member this.fadeToggle() = jquery(null)
    member this.fadeToggle(x) = jquery(null)
    member this.fadeToggle(x,y) = jquery(null)
    member this.fadeToggle(x,y,z) = jquery(null)
    member this.filter(x) = jquery(null)
    member this.find(x) = jquery(null)
    member this.first() = jquery(null)
    member this.focus() = jquery(null)
    member this.focus(handler) = jquery(null)
    member this.focus(x,y) = jquery(null)
    member this.focusin(x) = jquery(null)
    member this.focusin(x,y) = jquery(null)
    member this.focusout(x) = jquery(null)
    member this.focusout(x,y) = jquery(null)
    member this.get() = new HtmlElement()
    member this.get(x) = new HtmlElement()
    member this.has(x) = jquery(null)
    member this.hasClass(x) = jquery(null)
    member this.height() = 0.0
    member this.height(x) = jquery(null)
    member this.hide() = jquery(null)
    member this.hide(x) = jquery(null)
    member this.hide(x,y) = jquery(null)
    member this.hide(x,y,z) = jquery(null)
    member this.hover(x) = jquery(null)
    member this.hover(x,y) = jquery(null)
    member this.html() = ""
    member this.html(x) = jquery(null)
    member this.index() = 0.0
    member this.index(x) = 0.0
    member this.innerHeight() = 1.0
    member this.innerWidth() = 1.0
    member this.insertAfter(x) = jquery(null)
    member this.insertBefore(x) = jquery(null)
    member this.is(x) = true
    member this.keydown() = jquery(null)
    member this.keydown(x : event -> unit) = jquery(null)
    member this.keydown(x : event -> bool) = jquery(null)
    member this.keydown(z,x : event -> unit) = jquery(null)
    member this.keydown(z, x : event -> bool) = jquery(null)
    member this.keypress() = jquery(null)
    member this.keypress(x : event -> unit) = jquery(null)
    member this.keypress(x : event -> bool) = jquery(null)
    member this.keypress(z,x : event -> unit) = jquery(null)
    member this.keypress(z,x : event -> bool) = jquery(null)
    member this.keyup() = jquery(null)
    member this.keyup(x:event -> unit) = jquery(null)
    member this.keyup(x:event -> bool) = jquery(null)
    member this.keyup(z,x:event -> unit) = jquery(null)
    member this.keyup(z,x:event -> bool) = jquery(null)
    member this.last() = jquery(null)
    [<DefaultValue>]
    val mutable length : float
    member this.live(eventType, handler) = jquery(null)
    member this.live(eventType,eventData,handler) = jquery(null)
    member this.load(a) = jquery(null)
    member this.load(a,b) = jquery(null)
    member this.load(a,b,c) = jquery(null)
    member this.map(x) = jquery(null)
    member this.mousedown() = jquery(null)
    member this.mousedown(x) = jquery(null)
    member this.mousedown(x,y) = jquery(null)
    member this.mouseenter() = jquery(null)
    member this.mouseenter(x) = jquery(null)
    member this.mouseenter(x,y) = jquery(null)
    member this.mouseleave() = jquery(null)
    member this.mouseleave(x: event -> unit) = jquery(null)
    member this.mouseleave(y, x: event -> unit) = jquery(null)
    member this.mousemove() = jquery(null)
    member this.mousemove(x: event -> unit) = jquery(null)
    member this.mousemove(y, x: event -> unit) = jquery(null)
    member this.mouseout() = jquery(null)
    member this.mouseout(x: event -> unit) = jquery(null)
    member this.mouseout(y, x: event -> unit) = jquery(null)
    member this.mouseover() = jquery(null)
    member this.mouseover(x: event -> unit) = jquery(null)
    member this.mouseover(y, x: event -> unit) = jquery(null)
    member this.mouseup() = jquery(null)
    member this.mouseup(x: event -> unit) = jquery(null)
    member this.mouseup(y, x: event -> unit) = jquery(null)
    member this.next() = jquery(null)
    member this.next(x) = jquery(null)
    member this.nextAll() = jquery(null)
    member this.nextAll(x) = jquery(null)
    member this.nextUntil() = jquery(null)
    member this.nextUntil(x) = jquery(null)
    member this.not(x) = jquery(null)
    member this.offset() = new position()
    member this.offset(x) = new position()
    member this.offsetParent() = new position()
    member this.one(x) = jquery(null)
    member this.one(x,y) = jquery(null)
    member this.one(x,y,z) = jquery(null)
    member this.outerHeight() = 0.0
    member this.outerHeight(x) = 0.0
    member this.outerWidth() = 0.0
    member this.outerWidth(x) = 0.0
    member this.parent() = jquery(null)
    member this.parent(x) = jquery(null)
    member this.parents() = jquery(null)
    member this.parents(x) = jquery(null)
    member this.parentsUntil() = jquery(null)
    member this.parentsUntil(x) = jquery(null)
    member this.position() = position()
    member this.prepend(x) = jquery(null)
    member this.prepend(x,y) = jquery(null)
    member this.prependTo(x) = jquery(null)
    member this.prev() = jquery(null)
    member this.prev(x) = jquery(null)
    member this.prevAll() = jquery(null)
    member this.prevAll(x) = jquery(null)
    member this.prevUntil() = jquery(null)
    member this.prevUntil(x) = jquery(null)
    member this.promise() = promise()
    member this.promise(x) = promise()
    member this.promise(x,y) = promise()
    member this.prop(x) = ""
    member this.prop(x,y) = ""
    //member this.pushStack(x) = jquery(null)
    member this.pushStack(x,y) = jquery(null)
    //member this.pushStack(x,y,z) = jquery(null)
    member this.queue() = [||]
    member this.queue(x) = [||]
    member this.queue(x,y) = [||]
    member this.ready(x) = jquery(null)
    member this.remove() = jquery(null)
    member this.remove(x) = jquery(null)
    member this.removeAttr(x) = jquery(null)
    member this.removeClass(x) = jquery(null)
    member this.removeData(x) = jquery(null)
    member this.removeProp(x) = jquery(null)
    member this.removeProp(x,y) = jquery(null)
    member this.replaceAll(x) = jquery(null)
    member this.replaceWith(x) = jquery(null)
    member this.resize() = jquery(null)
    member this.resize(x) = jquery(null)
    member this.resize(x,y) = jquery(null)
    member this.scroll() = jquery(null)
    member this.scroll(x) = jquery(null)
    member this.scroll(x,y) = jquery(null)
    member this.scrollLeft() = 0.0
    member this.scrollLeft(x) = 0.0
    member this.scrollTop() = 0.0
    member this.scrollTop(x) = 0.0
    member this.select() = jquery(null)
    member this.select(x) = jquery(null)
    member this.select(x,y) = jquery(null)
    [<DefaultValue>]
    val mutable selector : string
    member this.serialize() = ""
    member this.serializeArray() = [||]
    member this.show() = jquery(null)
    member this.show(x) = jquery(null)
    member this.show(x,y) = jquery(null)
    member this.show(x,y,z) = jquery(null)
    member this.siblings() = jquery(null)
    member this.siblings(x) = jquery(null)
    member this.size() = 0.0
    member this.slice(start) = jquery(null)
    member this.slice(start,e) = jquery(null)
    member this.slideDown() = jquery(null)
    member this.slideDown(x) = jquery(null)
    member this.slideDown(x,y) = jquery(null)
    member this.slideDown(x,y,z) = jquery(null)
    member this.slideToggle() = jquery(null)
    member this.slideToggle(x) = jquery(null)
    member this.slideToggle(x,y) = jquery(null)
    member this.slideToggle(x,y,z) = jquery(null)
    member this.slideUp() = jquery(null)
    member this.slideUp(x) = jquery(null)
    member this.slideUp(x,y) = jquery(null)
    member this.slideUp(x,y,z) = jquery(null)
    member this.stop() = jquery(null)
    member this.stop(x) = jquery(null)
    member this.stop(x,y) = jquery(null)
    member this.submit() = jquery(null)
    member this.submit(x) = jquery(null)
    member this.submit(x,y) = jquery(null)
    member this.text() = ""
    member this.toArray() = [||]
    member this.toggle() = jquery(null)
    member this.toggle(x) = jquery(null)
    member this.toggle(x,y) = jquery(null)
    member this.toggle(x,y,z) = jquery(null)
    member this.toggleClass(x) = jquery(null)
    member this.toggleClass(x,y) = jquery(null)
    member this.trigger(x) = jquery(null)
    member this.trigger(x,y) = jquery(null)
    member this.triggerHandler(x) = new System.Object()
    member this.triggerHandler(x,y) = new System.Object()
    member this.unbind(x) = jquery(null)
    member this.unbind(x,y) = jquery(null)
    member this.undelegate() = jquery(null)
    member this.undelegate(x,y) = jquery(null)
    member this.undelegate(x,y,z) = jquery(null)
    member this.unload(x) = jquery(null)
    member this.unload(x,y) = jquery(null)
    member this.unwrap() = jquery(null)
    member this.``val``() = ""
    member this.``val``(x) = ""
    member this.value() = ""
    member this.value(x) = ""
    member this.width() = 0
    member this.width(x) = 0
    member this.wrap(x) = jquery(null)
    member this.wrapAll(x) = jquery(null)
    member this.wrapInner(x) = jquery(null)


    static member ajax(x) = jqXHR()
    static member ajax(x, y) = jqXHR()
    static member ajaxPrefilter(x, y) = ()
    static member ajaxSetup(x) = ()

    //[<DefaultValue>]
    static let mutable browser = browser()

    static let mutable cssHooks = new System.Object()

    static member contains(container, contained) = true
    //static member data(key) = System.Object()
    static member data(element, key) = new System.Object()
    static member data(element, key, value) = ()
    static member dequeue(element) = ()
    static member dequeue(element, queueName) = ()
    static member each(collection, callback) = ()
    static member error(message) = ()
    static member extend(x) = ()
    static member extend(x,y) = ()
    static member extend(x,y,z) = ()
    static member extend(x,y,z,q) = ()
//    static member get(a) = HtmlElement()
//    static member get(a,b) = HtmlElement()
//    static member get(a,b,c) = HtmlElement()
//    static member get(a,b,c,d) = HtmlElement()
    static member getJSON(a) = jqXHR()
    static member getJSON(a,b) = jqXHR()
    static member getJSON(a,b,c) = jqXHR()
    static member getJSON(a,b,c,d) = jqXHR()
    static member getScript(a) = XMLHttpRequest()
    static member getScript(a,b) = XMLHttpRequest()
    static member globalEval(x) = ()
    static member grep(x) = [||]
    static member grep(x,y) = [||]
    static member grep(x,y,z) = [||]
    static member hasData(x) = true
    static member holdReady(x) = true
    static member inArray(x,y) = 0.0
    static member isArray(x) = true
    static member isEmptyObject(x) = true
    static member isFunction(x) = true
    static member isPlainObject(x) = true
    static member isWindow(x) = true
    static member isXMLDoc(x) = true
    static member makeArray(x) = [||]
    static member map(x,y) = [||]
    static member merge(x,y) = [||]
    static member noConflict() = System.Object()
    static member noConflict(x) = System.Object()
    static member noop() = (fun () -> ())
    static member now() = 0.0
    static member param(x) = ""
    static member param(x,y) = ""
    static member parseJSON(x) = new System.Object()
    static member parseXML(x) = new System.Object()
    static member post(a) = jqXHR()
    static member post(a,b) = jqXHR()
    static member post(a,b,c) = jqXHR()
    static member post(a,b,c,d) = jqXHR()
    static member proxy(x,y) = x
    static member pushStack(x) = jquery(null)
    static member pushStack(x,y,z) = jquery(null)


    static member queue(x) = [||] : obj array
    static member queue(a,b) = [||] : obj array
    static member queue(c,y,z) = [||] : obj array
    //uncommenting this gives a very strange error.
    //static member removeData(element) = jquery(null)
    static member removeData(element, name) = jquery(null)

    static member sub() = jquery(null)
    
    static member support = new support()
    static member trim(x:string) = ""
    static member unique(x) = [||]
    static member ``when``(def) = promise()

module UI =
        type jquery with
            member this.draggable() = jquery(null)
            member this.draggable(x) = jquery(null)
            member this.droppable() = jquery(null)
            member this.droppable(x) = jquery(null)
            member this.resizable() = jquery(null)
            member this.resizable(x) = jquery(null)
            member this.selectable() = jquery(null)
            member this.selectable(x) = jquery(null)
            member this.sortable() = jquery(null)
            member this.sortable(x) = jquery(null)
            member this.accordion() = jquery(null)
            member this.accordion(x) = jquery(null)
            member this.accordion(x, y) = jquery(null)
            member this.accordion(x, y, z) = jquery(null)
            member this.autocomplete() = jquery(null)
            member this.autocomplete(x) = jquery(null)
            member this.autocomplete(x, y) = jquery(null)
            member this.autocomplete(x, y, z) = jquery(null)
            member this.button() = jquery(null)
            member this.button(x) = jquery(null)
            member this.button(x, y) = jquery(null)
            member this.button(x, y, z) = jquery(null)
            member this.datepicker() = jquery(null)
            member this.datepicker(x) = jquery(null)
            member this.datepicker(x, y) = jquery(null)
            member this.datepicker(x, y, z) = jquery(null)
            member this.datepicker(x, y, z, a) = jquery(null)
            member this.datepicker(x, y, z, a, b) = jquery(null)
            member this.dialog() = jquery(null)
            member this.dialog(x) = jquery(null)
            member this.dialog(x, y) = jquery(null)
            member this.dialog(x, y, z) = jquery(null)
            member this.progressbar() = jquery(null)
            member this.progressbar(x) = jquery(null)
            member this.progressbar(x, y) = jquery(null)
            member this.progressbar(x, y, z) = jquery(null)
            member this.slider() = jquery(null)
            member this.slider(x) = jquery(null)
            member this.slider(x,y) = jquery(null)
            member this.slider(x,y,z) = jquery(null)
            member this.tabs() = jquery(null)
            member this.tabs(x) = jquery(null)
            member this.tabs(x, y) = jquery(null)
            member this.tabs(x, y, z) = jquery(null)
            member this.tabs(x, y, z, a) = jquery(null)
            member this.toggleClass(x:string) = jquery(null)
            member this.toggleClass(x:string, y:float) = jquery(null)
            member this.addClass(x:string) = jquery(null)
            member this.addClass(x:string, y:float) = jquery(null)
            member this.removeClass() = jquery(null)
            member this.removeClass(x:string) = jquery(null)
            member this.removeClass(x:string, y:float) = jquery(null)
            member this.switchClass(x:string, y:string) = jquery(null)
            member this.switchClass(x:string, z:float) = jquery(null)
            member this.effect(x:string) = jquery(null)
            member this.effect(x:string, y) = jquery(null)
            member this.effect(x:string, y, z) = jquery(null)
            member this.effect(x:string, y, z, a) = jquery(null)
            member this.toggle(x:string) = jquery(null)
            member this.toggle(x:string, y) = jquery(null)
            member this.toggle(x:string, y, z) = jquery(null)
            member this.toggle(x:string, y, z, a) = jquery(null)
            member this.hide(x:string) = jquery(null)
            member this.hide(x:string, y) = jquery(null)
            member this.hide(x:string, y, z) = jquery(null)
            member this.hide(x:string, y, z, a) = jquery(null)
            member this.show(x:string) = jquery(null)
            member this.show(x:string, y) = jquery(null)
            member this.show(x:string, y, z) = jquery(null)
            member this.show(x:string, y, z, a) = jquery(null)
            member this.position(x) = jquery(null)