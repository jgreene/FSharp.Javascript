registerNamespace('FSharp.Javascript.Library.DateTime.TryParse2')


FSharp.Javascript.Library = {
    DateTime: {
        TryParse2: {
            Static: function (x) {
                try {
                    if (x == "") {
                        return new Microsoft.FSharp.Core.FSharpOption.None();
                    }
                    var d = new Date(x)
                    if (d == null) {
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
                try {
                    var value = parseInt(x)
                    return new Microsoft.FSharp.Core.FSharpOption.Some(value);
                } catch (err) {
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
            }
        }
    },

    Int16: {
        TryParse2: {
            Static: function (x) {
                try {
                    var value = parseInt(x)
                    return new Microsoft.FSharp.Core.FSharpOption.Some(value);
                } catch (err) {
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
            }
        }
    },

    Int64: {
        TryParse2: {
            Static: function (x) {
                try {
                    var value = parseInt(x)
                    return new Microsoft.FSharp.Core.FSharpOption.Some(value);
                } catch (err) {
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
            }
        }
    },

    Decimal: {
        TryParse2: {
            Static: function (x) {
                try {
                    var value = parseFloat(x)
                    return new Microsoft.FSharp.Core.FSharpOption.Some(value);
                } catch (err) {
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
            }
        }
    },

    Double: {
        TryParse2: {
            Static: function (x) {
                try {
                    var value = parseFloat(x)
                    return new Microsoft.FSharp.Core.FSharpOption.Some(value);
                } catch (err) {
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
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
                    else if (value2){
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
    }
}