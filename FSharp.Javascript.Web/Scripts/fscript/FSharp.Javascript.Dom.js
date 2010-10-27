
registerNamespace('FSharp.Javascript')

FSharp.Javascript.Dom = {
    get_document: function () {
        return window.document;
    },

    get_window: function () {
        return window;
    },

    alert : window.alert
}