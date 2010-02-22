﻿var Operators = {}

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

Operators.op_PipeRight = function (tup) {
    return tup.Item1(tup.Item2)
}

Operators.Ignore = function (value) {
    return null
}


function MatchFailureException(file, line, character) {
    return { file: file, line: line, character: character };
}

function Tuple() {
    for (var i = 1; i <= arguments.length + 1; i++) {
        this['Item' + i] = arguments[i - 1];
    }
}

Tuple.prototype.toString = function () {
    var a = '('
    var hasItem = false;
    for (var p in this) {
        if (p.substring(0, 4) == "Item") {
            a += this[p]
            a += ','
            hasItem = true
        }

    }

    if (!hasItem) return null

    a = a.slice(0, a.length - 1)

    a += ')'
    return a;
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

function Map(source, func) {
    this.func = func
    this.source = source
}

Map.prototype.get = function () {
    return this.func(this.source.get())
}

Map.prototype.read = function () {
    return this.source.read()
}

function Filter(source, func) {
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
SeqModule.Map = function (tuple) {
    return new Map(tuple.Item1, tuple.Item2)
}
SeqModule.Filter = function (tuple) {
    return new Filter(tuple.Item1, tuple.Item2)
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
    var arr = new Array()
    while (source.read())
        arr.push(source.get())
    return arr
}

Sequence.prototype.ToArray = function () {
    return SeqModule.ToArray(this)
}

SeqModule.Collect = function (tup) {
    return new Concat(new Map(tup.Item1, tup.Item2))
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

SeqModule.ToList = function (sequence) {
    var list = new FSharpList.Empty();
    while (sequence.read()) {
        var temp = sequence.get();
        list = new FSharpList.Cons(list, temp);
    }

    return list;
}

Operators.op_Append = function (tup) {
    var list = tup.Item1
    var list2 = ListModule.Reverse(tup.Item2)
    while (list2.read()) {
        var temp = list2.get();
        list = new FSharpList.Cons(list, temp);
    }

    return list;
}

