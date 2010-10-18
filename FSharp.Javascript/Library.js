registerNamespace('FSharp.Javascript.Library.DateTime.TryParse2')

FSharp.Javascript.Library.DateTime.TryParse2.Static = function (x) {
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