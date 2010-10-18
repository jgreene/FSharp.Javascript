registerNamespace('FSharp.Javascript.Library.DateTime.TryParse2')


FSharp.Javascript.Library = {
    DateTime: {
        TryParse2: {
            Static : function (x) {
                try {
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

    Int32 : {
        TryParse2 : {
            Static : function(x){
                try{
                    var value = parseInt(x)
                    return new Microsoft.FSharp.Core.FSharpOption.Some(value);
                }catch(err){
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
            }
        }
    },

    Int16 : {
         TryParse2 : {
            Static : function(x){
                try{
                    var value = parseInt(x)
                    return new Microsoft.FSharp.Core.FSharpOption.Some(value);
                }catch(err){
                    return new Microsoft.FSharp.Core.FSharpOption.None();
                }
            }
        }
    },

    Int64 : {
         TryParse2 : {
            Static : function(x){
                try{
                    var value = parseInt(x)
                    return new Microsoft.FSharp.Core.FSharpOption.Some(value);
                }catch(err){
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
    }
}