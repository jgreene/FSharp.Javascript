
var Operators = {}

Operators.op_Equality = function (one) {
    return function (two) {
        if (one == null && two == null)
            return true

        if (one == null && two != null)
            return false

        if (one != null && one.Equality) {
            return one.Equality(two)
        }

        return one === two;
    }
}


Operators.op_Append = function (item1) {
    return function (item2) {
        var list = item1
        var list2 = ListModule.Reverse(item2)
        while (list2.read()) {
            var temp = list2.get();
            list = new FSharpList.Cons(list, temp);
        }

        return list;
    }
}

function Raise(exception) {
    throw exception;
}

Operators.FailWith = function (msg) {
    throw msg
}

Operators.FailurePattern = function (msg) {
    if (msg != null) {
        return new FSharpOption.Some(msg)
    }
    else {
        return new FSharpOption.None()
    }
}

Operators.op_PipeRight = function (func) {
    return function(item){
        return func(item)
    }
}

Operators.Ignore = function (value) {
    return null
}

Operators.Fst = function (tup) {
    return tup.Item1;
}


function MatchFailureException(file, line, character) {
    return { file: file, line: line, character: character };
}

function Tuple() {
    for (var i = 1; i <= arguments.length + 1; i++) {
        this['Item' + i] = arguments[i - 1];
    }
}

Operators.Fst = function (tup) { return tup.Item1; }
Operators.Snd = function (tup) { return tup.Item2; }

var FSharpOption = {}
FSharpOption.Some = function (val) {
    this.IsNone = false;
    this.IsSome = true;
    this.Value = val;
}
FSharpOption.None = function () {
    this.IsNone = true;
    this.IsSome = false;
    this.Value = null;
}

function Range(start, end) {
    this.start = start
    this.end = end
    this.current = start - 1
}

Range.prototype.get = function () {

    if (this.current <= this.end)
        return this.current
    else
        return null
}
Range.prototype.read = function () {
    this.current++
    if (this.current <= this.end)
        return true
    else
        return false
}

Operators.op_Range = function (end) {
    return function (start) {
        return new Range(start, end)
    }
}

function Map(func, source) {
    this.func = func
    this.source = source
}

Map.prototype.get = function () {
    return this.func(this.source.get())
}

Map.prototype.read = function () {
    return this.source.read()
}

function Filter(func, source) {
    this.source = source
    this.func = func
}

Filter.prototype.get = function () {
    return this.source.get()
}

Filter.prototype.read = function () {
    if (!this.source.read())
        return false

    if (this.func(this.source.get()))
        return true

    return this.read()
}

var SeqModule = {}
SeqModule.Delay = function (func) {
    return func();
}
SeqModule.Map = function (a) {
    return function (b) {
        return new Map(b, a)
    }
    
}
SeqModule.Filter = function (a) {
    return function (b) {
        return new Filter(b, a)
    }
}


function Sequence(source) {
    this.source = source
}

Sequence.prototype.get = function () {
    return this.source.get()
}

Sequence.prototype.read = function () {
    return this.source.read()
}

Operators.CreateSequence = function (source) {
    return new Sequence(source)
}

SeqModule.ToArray = function (source) {
    var arr = []
    while (source.read())
        arr.push(source.get())
    return arr
}

Sequence.prototype.ToArray = function () {
    return SeqModule.ToArray(this)
}

SeqModule.Collect = function (a) {
    return function (b) {
        return new Concat(new Map(b, a))
    }
    
}

function Concat(sources) {
    this.sources = sources
    this.currentSource = null
}

Concat.prototype.read = function () {
    if (this.currentSource == null) {
        this.sources.read();
        this.currentSource = this.sources.get()
    }

    var current = this.currentSource.read()
    if (current)
        return true;

    var hasMoreSources = this.sources.read();
    if (!hasMoreSources)
        return false


    this.currentSource = this.sources.get()

    return this.currentSource.read()
}

Concat.prototype.get = function () {
    return this.currentSource.get()
}

Array.prototype.read = function () {
    if (this.position == null)
        this.position = -1
    var temp = this.position < this.length
    this.position++
    return temp
}
Array.prototype.get = function () {
    return this[this.position]
}

var FSharpList = {}

FSharpList.Empty = function () {
    this.Length = 0
    this.Head = null
    this.IsEmpty = true
    this.Tail = null
    this.Item = function (x) {
        return null
    }
}

FSharpList.Empty.prototype.read = function () {
    return false
}



FSharpList.Cons = function (list, arg) {
    this.ReadState = null;
    this.Length = list.Length + 1;
    this.Head = arg;
    this.IsEmpty = false;
    this.Tail = list;
    this.Item = function (x) {
        if (x == 0)
            return this.Head;
        else
            return this.Tail.Item(x - 1);
    }
}

FSharpList.Cons.prototype.read = function () {
    if (this.ReadState == null)
        this.ReadState = this.Length

    this.ReadState--;
    if (this.ReadState < 0) {
        this.ReadState = null
        return false;
    }


    return true;
}

FSharpList.Cons.prototype.get = function () {
    return this.Item(this.ReadState)
}

var ListModule = {}
ListModule.Reverse = function (sequence) {
    var array = [];
    while (sequence.read()) {
        var temp = sequence.get();
        array.push(temp);
    }

    var list = new FSharpList.Empty();
    for (var i = array.length - 1; i >= 0; i--) {
        list = new FSharpList.Cons(list, array[i])
    }

    return list;
}

ListModule.Exists = function (list) {
    return function (func) {
        while (list.read()) {
            var item = list.get()

            var result = func(item)
            if (result == true)
                return true;
        }

        return false;
    }
}

SeqModule.ToList = function (sequence) {
    var list = new FSharpList.Empty();
    while (sequence.read()) {
        var temp = sequence.get();
        list = new FSharpList.Cons(list, temp);
    }

    return list;
}


LanguagePrimitives = {}
LanguagePrimitives.IntrinsicFunctions = {}

LanguagePrimitives.IntrinsicFunctions.UnboxGeneric = function (x) { return x; }

Enum = function () { }
Enum.prototype.toString = function () {
    return this.Text;
}

Enum.prototype.Equality = function (x) {
    return x.Integer == this.Integer;
}

Operators.ToInt = function (x) {
    if (x instanceof Enum)
        return x.Integer;

    return x;
}

Operators.ToString = function (x) {
    if (x instanceof Enum)
        return x.Text;

    return x;
}

Operators.Reference = function (x) {
    this.Value = x;
}

Operators.Ref = function (x) {
    return new Operators.Reference(x);
}

Operators.op_ColonEquals = function (x) {
    return function (item) {
        item.Value = x;
    }
}

Operators.op_Dereference = function (x) {
    return x.Value;
}

SeqModule.Iterate = function (source) {
    return function (func) {
        while (source.read()) {
            func(source.get())
        }
    }
}


FSharpMap = {}

FSharpMap.Empty = function () {
    this.Count = 0
    this.Head = null
    this.IsEmpty = true
    this.Tail = null
    this.Item = function (x) {
        return null
    }
}

FSharpMap.Empty.prototype.read = function () {
    return false
}

FSharpMap.Cons = function (list, arg) {
    this.ReadState = null;
    this.Count = list.Count + 1;
    this.Head = arg;
    this.IsEmpty = false;
    this.Tail = list;
    this.Item = function (x) {
        if (x == 0)
            return this.Head;
        else
            return this.Tail.Item(x - 1);
    }
}

FSharpMap.Cons.prototype.read = function () {
    if (this.ReadState == null)
        this.ReadState = this.Count

    this.ReadState--;
    if (this.ReadState < 0) {
        this.ReadState = null
        return false;
    }


    return true;
}


FSharpMap.Cons.prototype.get = function () {
    return this.Item(this.ReadState)
}

FSharpMap.Cons.prototype.ContainsKey = function (key) {
    while (this.read()) {
        var item = this.get()
        if (item == null)
            return false;
        if (item.key == key)
            return true;
    }

    return false;
}

FSharpMap.Cons.prototype.Remove = function (key) {
    var result = new FSharpMap.Empty()
    while (this.read()) {
        var item = this.get()
        if (item == null)
            return result;

        if (item.key != key) {
            result = new FSharpMap.Cons(result, item)
        }
    }

    return result;
}

MapModule = {}

MapModule.Empty = function () {
    return new FSharpMap.Empty();
}

MapModule.Add = function (source) {
    return function (value) {
        return function (key) {
            var item = { key: key, value: value }
            return new FSharpMap.Cons(source, item)
        }
    }
}

MapModule.Find = function (source) {
    return function (key) {
        while (source.read()) {
            var item = source.get()
            if (item.key == key) {
                return item.value;
            }
        }

        return null;
    }
}

