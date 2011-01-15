registerNamespace('FSharp.Javascript')


FSharp.Javascript.Library = {
    IsNumber: function (n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    },

    DateTime: {
        TryParse2: {
            Static: function (x) {
                try {
                    var d = new Date(x)
                    if (d == null || isNaN(d.getYear())) {
                        return new Microsoft.FSharp.Core.FSharpOption.None();
                    }


                    var dateTime = new System.DateTime(d.getFullYear(), d.getMonth() + 1, d.getDate(), d.getHours(), d.getMinutes(), d.getSeconds())
                    return new Microsoft.FSharp.Core.FSharpOption.Some(dateTime);
                }
                catch (err) {
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
            }
        }
    },

    Int32: {
        TryParse2: {
            Static: function (x) {

                if (FSharp.Javascript.Library.IsNumber(x) == false)
                    return new Microsoft.FSharp.Core.FSharpOption.None();

                var value = new System.Int32(x)
                return new Microsoft.FSharp.Core.FSharpOption.Some(value);
            }
        }
    },

    Int16: {
        TryParse2: {
            Static: function (x) {
                if (FSharp.Javascript.Library.IsNumber(x) == false)
                    return new Microsoft.FSharp.Core.FSharpOption.None();

                var value = new System.Int32(x)
                return new Microsoft.FSharp.Core.FSharpOption.Some(value);
            }
        }
    },

    Int64: {
        TryParse2: {
            Static: function (x) {
                if (FSharp.Javascript.Library.IsNumber(x) == false)
                    return new Microsoft.FSharp.Core.FSharpOption.None();

                var value = new System.Int32(x)
                return new Microsoft.FSharp.Core.FSharpOption.Some(value);
            }
        }
    },

    Decimal: {
        TryParse2: {
            Static: function (x) {
                if (FSharp.Javascript.Library.IsNumber(x) == false)
                    return new Microsoft.FSharp.Core.FSharpOption.None();

                var value = parseFloat(x)
                return new Microsoft.FSharp.Core.FSharpOption.Some(value);
            }
        }
    },

    Double: {
        TryParse2: {
            Static: function (x) {
                if (FSharp.Javascript.Library.IsNumber(x) == false)
                    return new Microsoft.FSharp.Core.FSharpOption.None();

                var value = parseFloat(x)
                return new Microsoft.FSharp.Core.FSharpOption.Some(value);
            }
        }
    },

    Single: {
        TryParse2: {
            Static: function (x) {
                if (FSharp.Javascript.Library.IsNumber(x) == false)
                    return new Microsoft.FSharp.Core.FSharpOption.None();

                var value = parseFloat(x)
                return new Microsoft.FSharp.Core.FSharpOption.Some(value);
            }
        }
    },

    Boolean: {
        TryParse2: {
            Static: function (x) {
                try {
                    var value = x == "true"
                    var value2 = x == "false"
                    if (value) {
                        return new Microsoft.FSharp.Core.FSharpOption.Some(true);
                    }
                    else if (value2) {
                        return new Microsoft.FSharp.Core.FSharpOption.Some(false);
                    }
                    else {
                        return new Microsoft.FSharp.Core.FSharpOption.None();
                    }
                } catch (err) {
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
            }
        }
    },

    JObject : function(){

    },

    op_Dynamic: function (obj) {
        return function (name) {
            return obj[name]
        }
    },

    op_DynamicAssignment: function (obj) {
        return function (name) {
            return function (value) {
                obj[name] = value;
            }
        }
    }
}