var Jquery = {}
Jquery.jquery = window.jQuery

jQuery.fn.value = jQuery.fn.val

$.extend($.fn, {
    fsharpBind: function (func) {
        var self = this;
        return function (eventName) {
            self.bind(eventName, func)
        }
    }
})