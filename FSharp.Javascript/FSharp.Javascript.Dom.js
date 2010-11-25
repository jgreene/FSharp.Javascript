
registerNamespace('FSharp.Javascript')

FSharp.Javascript.Dom = {
    get_document: function () {
        return window.document;
    },

    get_window: function () {
        return window;
    },

    alert: function (x) {
        window.alert(x)
    },

    get_Math: function(){
        return Math;
    }
}

FSharp.Javascript.Canvas = {
    getCanvasById : function(id){
        return window.document.getElementById(id)
    }
}