function registerNamespace(ns) {
    var nsParts = ns.split(".");
    var root = this;

    for (var i = 0; i < nsParts.length; i++) {
        if (typeof root[nsParts[i]] == "undefined")
            root[nsParts[i]] = {};

        root = root[nsParts[i]];
    }
};

registerNamespace('System');

System.Int32 = function(x){
    this.value = x;
};

System.Int32.prototype.valueOf = function(){
    return parseInt(this.value);
};

System.Int32.prototype.op_Division = function(x){
    return new System.Int32((this.valueOf() / x));
};

System.Int32.prototype.op_Addition = function(x){
    return new System.Int32((this.valueOf() + x));
};

System.Int32.prototype.op_Subtraction = function(x){
    return new System.Int32((this.valueOf() - x));
};

System.Int32.prototype.op_Multiply = function(x){
    return new System.Int32((this.valueOf() * x));
};

System.Int32.prototype.op_Modulus = function(x){
    return new System.Int32((this.valueOf() % x));
};

System.Int32.prototype.toString = function(){
    return this.valueOf().toString();
};



registerNamespace('Microsoft.FSharp.Core');

registerNamespace('Microsoft.FSharp.Collections');



Microsoft.FSharp.Core.Operators = {
    op_Addition: function (x) {
        return function (y) {
            if (x.op_Addition) {
                return x.op_Addition(y);
            }

            
            return x.valueOf() + y.valueOf();
        }
    },

    op_Subtraction: function (x) {
        return function (y) {
            if (x.op_Subtraction) {
                return x.op_Subtraction(y);
            }
            return x.valueOf() - y.valueOf();
        }
    },

    op_Multiply: function (x) {
        return function (y) {
            if (x.op_Multiply) {
                return x.op_Multiply(y);
            }

            return x.valueOf() * y.valueOf();
        }
    },

    op_Division: function (x) {
        return function (y) {
            if (x.op_Division) {
                return x.op_Division(y);
            }

            return x.valueOf() / y.valueOf();
        }
    },

    op_Modulus: function (x) {
        return function (y) {
            if (x.op_Modulus) {
                return x.op_Modulus(y);
            }

            return x.valueOf() % y.valueOf();
        }
    },

    op_LessThanOrEqual: function (x) {
        return function (y) {
            if (x.op_LessThanOrEqual) {
                return x.op_LessThanOrEqual(y);
            }

            return x.valueOf() <= y.valueOf();
        }
    },

    op_LessThan: function (x) {
        return function (y) {
            if (x.op_LessThan) {
                return x.op_LessThan(y);
            }

            return x.valueOf() < y.valueOf();
        }
    },

    op_GreaterThan: function (x) {
        return function (y) {
            if (x.op_GreaterThan) {
                return x.op_GreaterThan(y);
            }

            return x.valueOf() > y.valueOf();
        }
    },

    op_GreaterThanOrEqual: function (x) {
        return function (y) {
            if (x.op_GreaterThanOrEqual) {
                return x.op_GreaterThanOrEqual(y);
            }
            return x.valueOf() >= y.valueOf();
        }
    },

    op_Inequality: function (x) {
        return function (y) {
            return Microsoft.FSharp.Core.Operators.op_Equality(x)(y) == false;
        }
    },

    op_Equality: function (one) {
        return function (two) {
            if (one == null && two == null)
                return true;

            if (one == null && two != null)
                return false;

            if (one != null && one.Equality) {
                return one.Equality(two);
            };

            return one.valueOf() === two.valueOf();
        };
    },

    ToDouble: function (x) {
        if(x instanceof System.Int32){
            return x.value;
        }
        return x.valueOf();
    },

    op_Append: function (item1) {
        return function (item2) {
            var list = item1;
            var list2 = Microsoft.FSharp.Collections.ListModule.Reverse(item2);
            while (list2.read()) {
                var temp = list2.get();
                list = new Microsoft.FSharp.Collections.FSharpList.Cons(temp, list);
            }

            return list;
        }
    },

    FailWith: function (msg) {
        throw msg
    },

    FailurePattern: function (msg) {
        if (msg != null) {
            return new Microsoft.FSharp.Core.FSharpOption.Some(msg);
        }
        else {
            return new Microsoft.FSharp.Core.FSharpOption.None();
        }
    },

    op_PipeRight: function (item) {
        return function (func) {
            return func(item);
        }
    },

    Ignore: function (value) {
        return null;
    },

    Fst: function (tup) { return tup.Item1; },
    Snd: function (tup) { return tup.Item2; },

    op_Range: function (start) {
        return function (end) {
            return new Range(start, 1, end);
        }
    },

    op_RangeStep: function (start) {
        return function (step) {
            return function (end) {
                return new Range(start, step, end);
            }
        }
    },

    CreateSequence: function (source) {
        return new Sequence(source);
    },

    ToInt: function (x) {
        if (x instanceof System.Enum)
            return x.Integer;

        return x;
    },

    ToString: function (x) {
        if (x instanceof System.Enum)
            return x.Text;

        return x;
    },

    Reference: function (x) {
        this.Value = x;
        this.get_Value = function () {
            return this.Value;
        }
    },

    Ref: function (x) {
        return new this.Reference(x);
    },

    op_ColonEquals: function (item) {
        return function (x) {
            item.Value = x;
        }
    },

    op_Dereference: function (x) {
        return x.Value;
    },

    Abs: function (x) {
        return Math.abs(x);
    },

    Max: function (x) {
        var self = this
        return function (y) {
            if (self.op_GreaterThan(x)(y))
                return x;

            return y;
        }
    },

    Min: function (x) {
        var self = this
        return function (y) {
            if (self.op_LessThan(x)(y))
                return x;

            return y;
        }
    }
};

function Raise(exception) {
    throw exception;
};


function MatchFailureException(file, line, character) {
    return { file: file, line: line, character: character };
};

function Tuple() {
    this.args = arguments;
    for (var i = 1; i <= arguments.length + 1; i++) {
        this['Item' + i] = arguments[i - 1];
    }
};

Tuple.prototype.toString = function () {
    var result = "("
    for (var i = 0; i < this.args.length; i++) {
        result = result + this.args[i].toString();
        if (i != (this.args.length - 1)) {
            result = result + ", ";
        }
    }
    return result + ")";
};

Microsoft.FSharp.Core.FSharpOption = {
    None: function () {
        this.IsNone = true;
        this.IsSome = false;
        this.Value = null;

        this.get_Value = function () {
            return this.Value;
        };
    },

    Some: function (val) {
        this.IsNone = false;
        this.IsSome = true;
        this.Value = val;

        this.get_Value = function () {
            return this.Value;
        };
    },

    get_IsSome: function (x) {
        return x instanceof Microsoft.FSharp.Core.FSharpOption.Some;
    },

    get_IsNone: function (x) {
        return x instanceof Microsoft.FSharp.Core.FSharpOption.None;
    }
};


function Range(start, step, end) {
    this.start = start;
    this.end = end;
    this.step = step;
    this.current = start - step;
};

Range.prototype.get = function () {

    if (this.current != (this.end + this.step))
        return this.current;
    else
        return null;
};
Range.prototype.read = function () {
    this.current = this.current + this.step
    if (this.current != (this.end + this.step))
        return true;
    else
        return false;
};
Range.prototype.reset = function () {
    this.current = this.start - this.step;
};


Microsoft.FSharp.Collections.SeqModule = {
    Iterate: function (func) {
        return function (source) {
            while (source.read()) {
                func(source.get());
            }
        }
    },

    Delay: function (func) {
        return func();
    },

    Map: function (func) {
        return function (source) {
            return new Sequence(source, func);
        }
    },

    Filter: function (func) {
        return function (source) {
            var array = [];

            while (source.read()) {
                var item = source.get();
                if (func(item))
                    array.push(item);
            }

            return array;
        }
    },

    ToArray: function (source) {
        var arr = [];
        while (source.read())
            arr.push(source.get());
        return arr;
    },

    ToList: function (sequence) {
        var list = new Microsoft.FSharp.Collections.FSharpList.Empty();
        while (sequence.read()) {
            var temp = sequence.get();
            list = new Microsoft.FSharp.Collections.FSharpList.Cons(temp, list);
        }

        return Microsoft.FSharp.Collections.ListModule.Reverse(list);
    },

    Collect: function (func) {
        return function (source) {
            return new Concat(Microsoft.FSharp.Collections.SeqModule.Map(func)(source));
        }
    },

    Exists: function (func) {
        return function (seq) {
            while (seq.read()) {
                var item = seq.get();

                var temp = func(item);
                if (temp) {
                    seq.reset();

                    return true;
                }
            }

            return false;
        }
    },

    Fold: function (func) {
        return function (acc) {
            return function (source) {
                while (source.read()) {
                    var next = source.get();
                    acc = func(acc)(next);
                }

                return acc;
            }
        }
    },

    Head: function (source) {
        if (source.read()) {
            var temp = source.get();
            source.reset();
            return temp;
        }
        return null;
    },

    Singleton: function (item) {
        var array = [item];
        return new Sequence(array);
    },

    Append: function (source) {
        return function (source2) {
            return new Concat([source, source2]);
        }
    },

    Empty: function () {
        return new Sequence([]);
    },

    Skip: function (num) {
        var self = this;
        return function (source) {
            var i = 0;
            var temp = self.SkipWhile(function (input) {
                var result = false;
                if (i < num) {
                    result = true;
                }

                i++;

                return result;
            })(source)
            var oldReset = temp.reset;
            temp.reset = function () {
                i = 0;
                oldReset();
            }

            return temp;
        }
    },

    Average: function (source) {
        return Microsoft.FSharp.Collections.SeqModule.AverageBy(function (x) {
            return x;
        })(source);
    },

    AverageBy: function (func) {
        return function (source) {
            var i = 0;
            var total = 0;
            while (source.read()) {
                var temp = func(source.get());
                total = total + temp;
                i++;
            }

            if (i > 0) {
                return total / i;
            }

            return 0;
        }
    },

    Cache: function (source) {
        var arr = [];
        while (source.read()) {
            arr.push(source.get());
        }

        return new Sequence(arr);
    },

    Get: function (index) {
        return function (source) {
            var i = 0;
            while (source.read()) {
                if (i == index) {
                    var temp = source.get();
                    source.reset();
                    return temp;
                }

                i++;
            }


            return null;
        }
    },

    Length: function (source) {
        var i = 0;
        while (source.read()) {
            i++;
        }

        return i;
    },

    Cast: function (source) {
        return source;
    },

    Choose: function (func) {
        return function (source) {
            return new Filter(function (x) {
                var temp = func(x);
                if (Microsoft.FSharp.Core.FSharpOption.get_IsSome(temp)) {
                    return true;
                }

                return false;
            }, source)
        }
    },

    CompareWith: function (func) {
        return function (source1) {
            return function (source2) {
                while (source1.read() && source2.read()) {
                    var item1 = source1.get()
                    var item2 = source2.get()
                    var result = func(item1)(item2)
                    if (result != 0)
                        return result;
                }

                return -1;
            }

        }
    },

    Find: function (func) {
        return function (source) {
            var result = Microsoft.FSharp.Collections.SeqModule.TryFind(func)(source)
            if (Microsoft.FSharp.Core.FSharpOption.get_IsSome(result)) {
                return result.get_Value();
            }

            throw "Key Not Found";
        }
    },

    TryFind: function (func) {
        return function (source) {
            while (source.read()) {
                var temp = source.get();
                var result = func(temp);
                if (result) {
                    source.reset();
                    return new Microsoft.FSharp.Core.FSharpOption.Some(temp);
                }
            }

            return new Microsoft.FSharp.Core.FSharpOption.None();
        }
    },

    CountBy: function (func) {
        return function (source) {
            var arr = [];

            while (source.read()) {
                var item = source.get();
                var key = func(item);

                var result = Microsoft.FSharp.Collections.SeqModule.TryFind(function (x) {
                    if (Microsoft.FSharp.Core.Operators.op_Equality(key)(x.Item1)) {
                        return true;
                    }
                    return false;
                })(arr);

                if (Microsoft.FSharp.Core.FSharpOption.get_IsSome(result)) {
                    var tuple = result.get_Value();
                    tuple.Item2 = Microsoft.FSharp.Core.Operators.op_Addition(tuple.Item2)(1);
                } else {
                    var tuple = new Tuple(key, 1);
                    arr.push(tuple);
                }

            }

            return new Sequence(arr);
        }
    },

    Distinct: function (source) {
        var arr = [];

        while (source.read()) {
            var item = source.get();
            var exists = Microsoft.FSharp.Collections.SeqModule.Exists(function (x) {
                return Microsoft.FSharp.Core.Operators.op_Equality(item)(x);
            })(arr);

            if (exists == false) {
                arr.push(item);
            }
        }

        return new Sequence(arr);
    },

    OfList: function (source) {
        return new Sequence(source);
    },

    DistinctBy: function (func) {
        return function (source) {
            var keys = [];
            var values = [];

            while (source.read()) {
                var item = source.get();
                var key = func(item);

                var exists = Microsoft.FSharp.Collections.SeqModule.Exists(function (x) {
                    return Microsoft.FSharp.Core.Operators.op_Equality(key)(x);
                })(keys);

                if (exists == false) {
                    keys.push(key);
                    values.push(item);
                }
            }

            return new Sequence(values);
        }
    },

    Exists2: function (func) {
        return function (source1) {
            return function (source2) {
                while (source1.read() && source2.read()) {
                    var item1 = source1.get();
                    var item2 = source2.get();

                    var exists = func(item1)(item2);
                    if (exists) {
                        source1.reset();
                        source2.reset();
                        return true;
                    }
                }

                return false;
            }
        }
    },

    TryFindIndex: function (func) {
        return function (source) {
            var i = 0;
            while (source.read()) {
                var item = source.get();
                var pred = func(item);
                if (pred) {
                    source.reset();
                    return new Microsoft.FSharp.Core.FSharpOption.Some(i);
                }
                i++
            }

            return new Microsoft.FSharp.Core.FSharpOption.None();
        }
    },

    FindIndex: function (func) {
        return function (source) {
            var i = 0;
            while (source.read()) {
                var item = source.get();
                var pred = func(item);
                if (pred) {
                    source.reset();
                    return i;
                }
                i++;
            }

            throw new "Key Not Found";
        }
    },

    ForAll: function (func) {
        return function (source) {
            while (source.read()) {
                var item = source.get();
                var pred = func(item);
                if (!pred) {
                    source.reset();
                    return false;
                }
            }

            return true;
        }
    },

    ForAll2: function (func) {
        return function (source1) {
            return function (source2) {
                while (source1.read() && source2.read()) {
                    var item1 = source1.get();
                    var item2 = source2.get();
                    var pred = func(item1)(item2);
                    if (!pred) {
                        source1.reset();
                        source2.reset();
                        return false;
                    }
                }

                return true;
            }
        }
    },

    GroupBy: function (func) {
        return function (source) {
            var arr = [];

            while (source.read()) {
                var item = source.get();
                var key = func(item);

                var result = Microsoft.FSharp.Collections.SeqModule.TryFind(function (x) {
                    if (Microsoft.FSharp.Core.Operators.op_Equality(key)(x.Item1)) {
                        return true;
                    }
                    return false;
                })(arr);

                if (Microsoft.FSharp.Core.FSharpOption.get_IsSome(result)) {
                    var tuple = result.get_Value();
                    tuple.Item2.push(item);
                } else {
                    var tuple = new Tuple(key, [item]);
                    arr.push(tuple);
                }
            }

            return new Sequence(arr);
        }
    },

    InitializeInfinite: function (func) {
        return {
            index: -1,
            read: function () { return true },
            get: function () {
                this.index++;
                return func(this.index);
            },
            reset: function () { return this.index = -1; }
        }
    },

    Take: function (num) {
        return function (source) {
            return {
                index: 0,
                read: function () {
                    if (this.index < num) {
                        this.index++;
                        return source.read();
                    }

                    return false;
                },
                get: function () { return source.get() },
                reset: function () {
                    this.index = 0;
                    source.reset();
                }
            }
        }
    },

    Initialize: function (num) {
        var self = this;
        return function (func) {
            var infinite = self.InitializeInfinite(func);
            return self.Take(num)(infinite);
        }
    },

    IsEmpty: function (source) {
        var isEmpty = source.read() == false;
        source.reset();
        return isEmpty
    },

    Iterate2: function (func) {
        return function (source1) {
            return function (source2) {
                while (source1.read() && source2.read()) {
                    var item1 = source1.get();
                    var item2 = source2.get();
                    func(item1)(item2);
                }
            }
        }
    },

    IterateIndexed: function (func) {
        return function (source) {
            var i = 0;
            while (source.read()) {
                var item = source.get();
                func(i)(item);
                i++;
            }
        }
    },

    Map2: function (func) {
        return function (source1) {
            return function (source2) {
                return {
                    read: function () { return source1.read() && source2.read(); },
                    get: function () { return func(source1.get())(source2.get()); },
                    reset: function () {
                        source1.reset();
                        source2.reset();
                    }
                }
            }
        }
    },

    Map3: function (func) {
        return function (source1) {
            return function (source2) {
                return function(source3){
                    return {
                        read: function () { return source1.read() && source2.read() && source3.read() },
                        get: function () { return func(source1.get())(source2.get())(source3.get()) },
                        reset: function () {
                            source1.reset();
                            source2.reset();
                            source3.reset();
                        }
                    }
                }
            }
        }
    },

    MapIndexed: function (func) {
        return function (source) {
            return {
                index: -1,
                read: function () {
                    this.index++;
                    return source.read();
                },
                get: function () { return func(this.index)(source.get()) },
                reset: function () { source.reset() }
            }
        }
    },

    MinMaxBy: function (comparator) {
        return function (func) {
            return function (source) {
                var canRead = false;
                var value = null;
                while (source.read()) {
                    canRead = true;
                    var item = source.get()
                    if (value == null) {
                        value = item;
                    } else {
                        var key = func(item);
                        var result = comparator(key)(value);
                        if (result) {
                            value = item;
                        }
                    }
                }

                if (canRead)
                    return value;

                throw new "Sequence cannot be null or empty";
            }
        }
    },

    MaxBy: function (func) {
        var self = this;
        return function (source) {
            return self.MinMaxBy(function (x) { return Microsoft.FSharp.Core.Operators.op_GreaterThan(x) })(func)(source);
        }
    },

    Max: function (source) {
        return this.MaxBy(function (x) { return x; })(source);
    },

    MinBy: function (func) {
        var self = this;
        return function (source) {
            return self.MinMaxBy(function (x) { return Microsoft.FSharp.Core.Operators.op_LessThan(x) })(func)(source);
        }
    },

    Min: function (source) {
        return this.MinBy(function (x) { return x; })(source);
    },

    OfArray: function (source) {
        return new Sequence(source);
    },

    OfList: function (source) {
        return new Sequence(source);
    },

    Pairwise: function (source) {
        return {
            last: null,
            read: function () {
                if (this.last == null) {
                    if (source.read()) {
                        this.last = source.get();
                    } else {
                        return false;
                    }
                }

                return source.read();
            },
            get: function () {
                var temp = source.get();
                var result = new Tuple(this.last, temp);
                this.last = temp;
                return result;
            },
            reset: function () {
                this.last = null;
                source.reset();
            }
        }
    },

    TryPick: function (func) {
        var self = this;
        return function (source) {
            var mapped = self.Map(func)(source);
            return self.TryFind(function (x) {
                return Microsoft.FSharp.Core.FSharpOption.get_IsSome(x);
            })(mapped).get_Value();
        }
    },

    Pick: function (func) {
        var self = this;
        return function (source) {
            return self.TryPick(func)(source).get_Value();
        }
    },

    ReadOnly: function (source) {
        return new Sequence(source);
    },

    Reduce: function (func) {
        return function (source) {
            var acc = null;
            while (source.read()) {
                if (acc == null) {
                    acc = source.get();
                    continue;
                }

                acc = func(acc)(source.get());
            }

            return acc;
        }
    },

    Scan: function (func) {
        return function (state) {
            return function (source) {
                return {
                    isFirst: true,
                    last: state,
                    read: function () {
                        if (this.isFirst) {

                            return true;
                        }
                        return source.read();
                    },
                    get: function () {
                        if (this.isFirst) {
                            this.isFirst = false;
                            return this.last;
                        }

                        this.last = func(this.last)(source.get());
                        return this.last;
                    },
                    reset: function () {
                        this.last = state;
                        source.reset();
                    }
                }
            }
        }
    },

    SkipWhile: function (func) {
        return function (source) {
            return {
                read: function () {
                    if (source.read() == false)
                        return false;

                    var item = source.get();
                    var shouldSkip = func(item);
                    if (shouldSkip) {
                        return this.read();
                    }

                    return true;
                },
                get: function () {
                    return source.get()
                },
                reset: function () {
                    source.reset();
                }
            }
        }
    },

    Truncate: function (num) {
        var self = this;
        return function (source) {
            return self.Take(num)(source);
        }
    },

    Unfold: function (func) {
        var self = this;
        return function (state) {
            return {
                currentState: state,
                nextState: null,
                read: function () {
                    if (this.nextState != null) {
                        this.currentState = this.nextState;
                    }

                    var item = func(this.currentState);
                    var isSome = Microsoft.FSharp.Core.FSharpOption.get_IsSome(item);

                    if (isSome) {
                        var value = item.get_Value().Item2;
                        this.nextState = value;

                        return true;
                    }

                    return false;
                },
                get: function () {
                    return this.currentState;
                },
                reset: function () {
                    this.currentState = state;
                }
            }
        }
    },

    Windowed: function (num) {
        var self = this
        return function (source) {
            return {
                currentNum: 0,
                currentArray: null,
                read: function () {
                    source.reset();
                    var skipped = self.Skip(this.currentNum)(source);
                    var taken = self.Take(num)(skipped);
                    this.currentArray = self.ToArray(taken);
                    this.currentNum++;

                    var length = this.currentArray.length;
                    if (length < num) {
                        return false;
                    }

                    return true;
                },
                get: function () {
                    return this.currentArray;
                },
                reset: function () {
                    this.currentArray = null;
                    this.currentNum = 0;
                    source.reset();
                }
            }
        }
    },

    Zip: function (source1) {
        return function (source2) {
            return {
                read: function () {
                    return source1.read() && source2.read();
                },
                get: function () {
                    return new Tuple(source1.get(), source2.get());
                },
                reset: function () {
                    source1.reset();
                    source2.reset();
                }
            }
        }
    },

    Zip3: function (source1) {
        return function (source2) {
            return function (source3) {
                return {
                    read: function () {
                        return source1.read() && source2.read() && source3.read();
                    },
                    get: function () {
                        return new Tuple(source1.get(), source2.get(), source3.get());
                    },
                    reset: function () {
                        source1.reset();
                        source2.reset();
                        source3.reset();
                    }
                }
            }
        }
    }
};


function Sequence(source, func) {
    var innerFunc = null;
    if (!func) {
        innerFunc = function (x) { return x; }
    } else {
        innerFunc = func;
    }

    this.func = innerFunc;
    this.source = source;
};

Sequence.prototype.get = function () {
    return this.func(this.source.get());
};

Sequence.prototype.read = function () {
    return this.source.read();
};
Sequence.prototype.reset = function () {
    this.source.reset();
};

Sequence.prototype.ToArray = function () {
    return Microsoft.FSharp.Collections.SeqModule.ToArray(this);
};

function Filter(func, source) {
    this.source = source;
    this.func = func;
};

Filter.prototype.get = function () {
    return this.source.get();
};

Filter.prototype.read = function () {
    if (!this.source.read())
        return false;

    if (this.func(this.source.get()))
        return true;

    return this.read();
};



function Concat(sources) {
    this.sources = sources;
    this.currentSource = null;
};

Concat.prototype.read = function () {
    if (this.currentSource == null) {
        if (this.sources.read()) {
            this.currentSource = this.sources.get();
        } else {
            return false;
        }
    }

    if (this.currentSource == null)
        return false;

    var canReadCurrent = this.currentSource.read();
    if (canReadCurrent) {
        return true;
    }

    this.currentSource = null;
    return this.read();
};

Concat.prototype.reset = function () {
    this.currentSource = null;
    this.sources.reset();
};

Concat.prototype.get = function () {
    return this.currentSource.get();
};

Array.prototype.read = function () {
    if (this.position == null) {
        if (this.length == 0)
            return false;

        this.position = -1;

    }
    var temp = this.position < (this.length - 1);
    if (temp) {
        this.position++;
        return true;
    }
    else {
        this.position = null;
        return false;
    }
};

Array.prototype.reset = function () {
    this.position = null;
};

Array.prototype.get = function () {
    return this[this.position];
};

Array.prototype.get_Length = function () {
    return this.length;
};


registerNamespace('Microsoft.FSharp.Collections');

Microsoft.FSharp.Collections.ArrayModule = {
    seq: Microsoft.FSharp.Collections.SeqModule,

    OfSeq: Microsoft.FSharp.Collections.SeqModule.ToArray,

    Append: function (source1) {
        var self = this;
        return function (source2) {
            var final = self.seq.Append(source1)(source2);
            return self.seq.ToArray(final);
        }
    },

    Average: Microsoft.FSharp.Collections.SeqModule.Average,

    AverageBy: Microsoft.FSharp.Collections.SeqModule.AverageBy,

    CopyTo: function (source1) {
        var self = this;
        return function (startIndex1) {
            return function (source2) {
                return function (startIndex2) {
                    return function (num) {
                        var outer1 = startIndex1;
                        var outer2 = startIndex2;
                        for (var a = 0; a < num; a++) {
                            source2[outer2] = source1[outer1];

                            outer1++;
                            outer2++;
                        }
                    }
                }
            }
        }
    },

    Choose: function (func) {
        var self = this;
        return function (source) {
            var temp = self.seq.Choose(func)(source);
            return self.OfSeq(temp);
        }
    },

    Collect: function (func) {
        var self = this;
        return function (source) {
            return self.OfSeq(self.seq.Collect(func)(source));
        }
    },

    Concat: function (source) {
        var arr = [];
        while (source.read()) {
            var innerSource = source.get();
            while (innerSource.read()) {
                arr.push(innerSource.get());
            }
        }

        return arr;
    },

    Copy: function (source) {
        return this.OfSeq(source);
    },

    Create: function (num) {
        return function (value) {
            var arr = [];
            for (var i = 0; i < num; i++) {
                arr.push(value);
            }

            return arr;
        }
    },

    Empty: function () { return []; },

    Exists: Microsoft.FSharp.Collections.SeqModule.Exists,

    Exists2: Microsoft.FSharp.Collections.SeqModule.Exists2,

    Fill: function (target) {
        return function (firstIndex) {
            return function (num) {
                return function (value) {

                    var index = firstIndex;
                    for (var i = 0; i < num; i++) {
                        target[index] = value;
                        index++;
                    }
                }
            }
        }
    },

    Filter: function (func) {
        var self = this;
        return function (source) {
            var temp = self.seq.Filter(func)(source);
            return self.OfSeq(temp);
        }
    },

    Find: Microsoft.FSharp.Collections.SeqModule.Find,

    FindIndex: Microsoft.FSharp.Collections.SeqModule.FindIndex,

    Fold: Microsoft.FSharp.Collections.SeqModule.Fold,

    Fold2: function (func) {
        var self = this;
        return function (state) {
            return function (source1) {
                return function (source2) {
                    if (source1.length != source2.length) {
                        throw new "ArgumentException";
                    }
                    var val = state;
                    for (var i = 0; i < source1.length; i++) {
                        val = func(val)(source1[i])(source2[i]);
                    }

                    return val;
                }
            }
        }
    },

    FoldBack: function (func) {
        var self = this;
        return function (source) {
            return function (acc) {

                var val = acc;
                for (var i = (source.length - 1); i >= 0; i--) {
                    val = func(source[i])(val);
                }

                return val;
            }
        }
    },

    FoldBack2: function (func) {
        return function (source1) {
            return function (source2) {
                return function (state) {
                    if (source1.length != source2.length) {
                        throw new "ArgumentException";
                    }

                    var val = state;
                    for (var i = (source1.length - 1); i >= 0; i--) {
                        val = func(source1[i])(source2[i])(val);
                    }

                    return val;
                }
            }
        }
    },

    ForAll: Microsoft.FSharp.Collections.SeqModule.ForAll,

    ForAll2: Microsoft.FSharp.Collections.SeqModule.ForAll2,

    Get: function (source) {
        return function (index) {
            return source[index];
        }
    },

    Initialize: function (num) {
        var self = this;
        return function (func) {
            var temp = self.seq.Initialize(num)(func);
            return self.OfSeq(temp);
        }
    },

    IsEmpty: Microsoft.FSharp.Collections.SeqModule.IsEmpty,

    Iterate: Microsoft.FSharp.Collections.SeqModule.Iterate,

    Iterate2: Microsoft.FSharp.Collections.SeqModule.Iterate2,

    IterateIndexed: Microsoft.FSharp.Collections.SeqModule.IterateIndexed,

    IterateIndexed2: function (func) {
        return function (source1) {
            return function (source2) {

                var i = 0;
                while (source1.read() && source2.read()) {
                    var item1 = source1.get();
                    var item2 = source2.get();
                    func(i)(item1)(item2);

                    i++;
                }
            }
        }
    },

    Length: function (source) {
        return source.length;
    },

    Map: function (func) {
        var self = this;
        return function (source) {
            return self.OfSeq(self.seq.Map(func)(source));
        }
    },

    Map2: function (func) {
        var self = this;
        return function (source) {
            return function (source2) {
                return self.OfSeq(self.seq.Map2(func)(source)(source2));
            }
        }
    },

    MapIndexed: function (func) {
        var self = this;
        return function (source) {
            return self.OfSeq(self.seq.MapIndexed(func)(source));
        }
    },

    MapIndexed2: function (func) {
        return function (source1) {
            return function (source2) {
                var arr = [];
                var i = 0;
                while (source1.read() && source2.read()) {
                    var item1 = source1.get();
                    var item2 = source2.get();
                    var result = func(i)(item1)(item2);
                    arr.push(result);
                    i++;
                }
                return arr;
            }
        }
    },

    Max: Microsoft.FSharp.Collections.SeqModule.Max,

    MaxBy: Microsoft.FSharp.Collections.SeqModule.MaxBy,

    Min: Microsoft.FSharp.Collections.SeqModule.Min,

    MinBy: Microsoft.FSharp.Collections.SeqModule.MinBy,

    OfList: Microsoft.FSharp.Collections.SeqModule.ToList,

    Partition: function (func) {
        return function (source) {
            var arr1 = [];
            var arr2 = [];

            while (source.read()) {
                var item = source.get();
                if (func(item)) {
                    arr1.push(item);
                } else {
                    arr2.push(item);
                }
            }

            return new Tuple(arr1, arr2);
        }
    },

    Permute: function (func) {
        return function (source) {
            var arr = [];

            var i = 0;
            while (source.read()) {
                var item = source.get();
                var index = func(i);
                arr[index] = item;
                i++;
            }

            return arr;
        }
    },

    Pick: Microsoft.FSharp.Collections.SeqModule.Pick,

    Reduce: Microsoft.FSharp.Collections.SeqModule.Reduce,

    ReduceBack: function (func) {
        return function (source) {
            var acc = null;
            for (var i = (source.length - 1); i >= 0; i--) {
                if (acc == null) {
                    acc = source[i];
                    continue;
                }

                acc = func(acc)(source[i]);
            }

            return acc;
        }
    },

    Reverse: function (source) {
        var arr = [];
        for (var i = (source.length - 1); i >= 0; i--) {
            arr.push(source[i]);
        }
        return arr;
    },

    Scan: function (func) {
        var self = this;
        return function (state) {
            return function (source) {
                var temp = self.seq.Scan(func)(state)(source);
                return self.OfSeq(temp);
            }
        }
    },

    ScanBack: function (func) {
        return function (source) {
            return function (state) {
                var arr = [];

                var val = state;
                for (var i = (source.length - 1); i >= 0; i--) {
                    var item = source[i];
                    val = func(item)(val);
                    arr[i] = val;
                }

                arr.push(state);

                return arr;
            }
        }
    },

    Set: function (source) {
        return function (index) {
            return function (item) {
                source[index] = item;
            }
        }
    },

    Sort: function (source) {
        var temp = this.OfSeq(source);
        this.SortInPlace(temp);
        return temp;

    },

    SortBy: function (func) {
        var self = this;
        return function (source) {
            var temp = self.OfSeq(source);
            self.SortInPlaceBy(func)(temp);
            return temp;
        }
    },

    SortInPlace: function (source) {
        source.sort(function (a, b) { return Microsoft.FSharp.Core.Operators.op_Subtraction(a)(b) })
    },

    SortInPlaceBy: function (func) {
        var self = this;
        return function (source) {
            source.sort(function (a, b) {
                var item1 = func(a);
                var item2 = func(b);
                return Microsoft.FSharp.Core.Operators.op_Subtraction(item1)(item2);
            })
        }
    },

    SortInPlaceWith: function (func) {
        var self = this;
        return function (source) {
            source.sort(function (a, b) {
                return func(a)(b);
            });
        }
    },

    SortWith: function (func) {
        var self = this;
        return function (source) {
            var temp = self.OfSeq(source);
            self.SortInPlaceWith(func)(temp);
            return temp;
        }
    },

    GetSubArray: function (source) {
        var self = this;
        return function (start) {
            return function (num) {
                return self.OfSeq(self.seq.Take(num)(self.seq.Skip(start)(source)));
            }
        }
    },

    Sum: function (source) {
        return this.SumBy(function (x) { return x })(source);
    },

    SumBy: function (func) {
        var self = this;
        return function (source) {
            var val = 0;
            while (source.read()) {
                var item = source.get();
                val = val + func(item);
            }
            return val;
        }
    },

    ToList: Microsoft.FSharp.Collections.SeqModule.ToList,

    ToSeq: function (source) {
        return new Sequence(source);
    },

    TryFind: Microsoft.FSharp.Collections.SeqModule.TryFind,

    TryFindIndex: Microsoft.FSharp.Collections.SeqModule.TryFindIndex,

    TryPick: Microsoft.FSharp.Collections.SeqModule.TryPick,

    Unzip: function (source) {
        var arr1 = [];
        var arr2 = [];
        while (source.read()) {
            var item = source.get();
            arr1.push(item.Item1);
            arr2.push(item.Item2);
        }

        return new Tuple(arr1, arr2);
    },

    Unzip3: function(source){
        var arr1 = [];
        var arr2 = [];
        var arr3 = [];
        while (source.read()) {
            var item = source.get();
            arr1.push(item.Item1);
            arr2.push(item.Item2);
            arr3.push(item.Item3);
        }

        return new Tuple(arr1, arr2, arr3);
    },

    ZeroCreate: function (num) {
        var arr = [];
        for (var i = 0; i < num; i++) {
            arr.push(0);
        }

        return arr;
    },

    Zip: Microsoft.FSharp.Collections.SeqModule.Zip,

    Zip3: Microsoft.FSharp.Collections.SeqModule.Zip3,

};

Microsoft.FSharp.Collections.FSharpList = {
    Empty: function () {
        this.Length = 0;
        this.Head = null;
        this.IsEmpty = true;
        this.Tail = null;
        this.get_Item = function (x) {
            return null;
        };

        this.get_Length = function () {
            return new System.Int32(this.Length);
        };

        this.get_Head = function () {
            return this.Head;
        };

        this.get_IsEmpty = function () {
            return this.IsEmpty;
        };

        this.get_Tail = function () {
            return this.Tail;
        };

        this.read = function () {
            return false;
        };

        this.reset = function () {
            this.ReadState = null;
        }

        this.get = function () {
            return null;
        };
    },

    Cons: function (arg, list) {
        this.ReadState = null;
        this.Length = list.Length + 1;
        this.Head = arg;
        this.IsEmpty = false;
        this.Tail = list;

        this.get_Item = function (x) {
            if (x == 0)
                return this.Head;
            else
                return this.Tail.get_Item(x - 1);
        };

        this.get_Length = function () {
            return new System.Int32(this.Length);
        };

        this.get_Head = function () {
            return this.Head;
        };

        this.get_IsEmpty = function () {
            return this.IsEmpty;
        };

        this.get_Tail = function () {
            return this.Tail;
        };


        this.read = function () {
            if (this.ReadState == null)
                this.ReadState = this;
            else
                this.ReadState = this.ReadState.Tail

            
            if (this.ReadState.Length == 0) {
                this.reset()
                return false;
            }


            return true;
        };

        this.reset = function () {
            this.ReadState = null;
        }

        this.get = function () {
            return this.ReadState.Head
        };

    }
};

Microsoft.FSharp.Collections.ListModule = {
    seq: Microsoft.FSharp.Collections.SeqModule,
    arr: Microsoft.FSharp.Collections.ArrayModule,

    Append: function(source1){
        var self = this;
        return function(source2){
            var temp = self.seq.Append(source1)(source2);
            return self.OfSeq(temp);
        }
    },

    Average: Microsoft.FSharp.Collections.SeqModule.Average,

    AverageBy: Microsoft.FSharp.Collections.SeqModule.AverageBy,

    Choose: function(func){
        var self = this;
        return function(source){
            var temp = self.seq.Choose(func)(source);
            return self.OfSeq(temp);
        }
    },

    Collect: function(func){
        var self = this;
        return function(source){
            var temp = self.seq.Collect(func)(source);
            return self.OfSeq(temp);
        }
    },

    Concat: function(source){
        return this.OfSeq(new Concat(source));
    },

    Empty: function(){
        return new Microsoft.FSharp.Collections.FSharpList.Empty(); 
    },

    Exists: Microsoft.FSharp.Collections.SeqModule.Exists,

    Exists2: Microsoft.FSharp.Collections.SeqModule.Exists2,

    Filter : function(func){
        var self = this;
        return function(source){
            var temp = self.seq.Filter(func)(source);
            return self.OfSeq(temp);
        }
    },

    Find: Microsoft.FSharp.Collections.SeqModule.Find,

    FindIndex: Microsoft.FSharp.Collections.SeqModule.FindIndex,

    Fold: Microsoft.FSharp.Collections.SeqModule.Fold,

    Fold2: function (func) {
        var self = this;
        return function (state) {
            return function (source1) {
                return function (source2) {
                    if (source1.Length != source2.Length) {
                        throw new "ArgumentException";
                    }
                    var val = state;
                    while(source1.read() && source2.read()){
                        val = func(val)(source1.get())(source2.get());
                    }

                    return val;
                }
            }
        }
    },

    FoldBack: function (func) {
        var self = this;
        return function (source) {
            return function (acc) {

                var val = acc;
                while(source.read()){
                    val = func(source.get())(val);
                }

                return val;
            }
        }
    },

    FoldBack2: function (func) {
        return function (source1) {
            return function (source2) {
                return function (state) {
                    if (source1.Length != source2.Length) {
                        throw new "ArgumentException";
                    }

                    var val = state;
                    while(source1.read() && source2.read()){
                        val = func(source1.get())(source2.get())(val);
                    }

                    return val;
                }
            }
        }
    },

    ForAll: Microsoft.FSharp.Collections.SeqModule.ForAll,

    ForAll2: Microsoft.FSharp.Collections.SeqModule.ForAll2,

    Head: function(source){
        return source.Head;
    },

    Initialize: function (num) {
        var self = this;
        return function (func) {
            var temp = self.seq.Initialize(num)(func);
            return self.OfSeq(temp);
        }
    },

    IsEmpty: Microsoft.FSharp.Collections.SeqModule.IsEmpty,

    Iterate: Microsoft.FSharp.Collections.SeqModule.Iterate,

    Iterate2: Microsoft.FSharp.Collections.SeqModule.Iterate2,

    IterateIndexed: Microsoft.FSharp.Collections.SeqModule.IterateIndexed,

    IterateIndexed2: Microsoft.FSharp.Collections.ArrayModule.IterateIndexed2,

    Length: function(source){
        return source.Length;
    },

    Map: function(func){
        var self = this;
        return function(source){
            var temp = self.seq.Map(func)(source);
            return self.OfSeq(temp);
        }
    },

    Map2: function(func){
        var self = this;
        return function(source1){
            return function(source2){
                var temp = self.seq.Map2(func)(source1)(source2);
                return self.OfSeq(temp);

            }
        }
    },

    Map3: function(func){
        var self = this;
        return function(source1){
            return function(source2){
                return function(source3){
                    var temp = self.seq.Map3(func)(source1)(source2)(source3);
                    return self.OfSeq(temp);
                }
            }
        }
    },

    MapIndexed: function(func){
        var self = this;
        return function(source){
            var temp = self.seq.MapIndexed(func)(source);
            return self.OfSeq(temp);
        }
    },

    MapIndexed2: function(func){
        var self = this;
        return function(source1){
            return function(source2){
                var temp = self.arr.MapIndexed2(func)(source1)(source2);
                return self.OfSeq(temp);
            }
        }
    },

    Max: Microsoft.FSharp.Collections.SeqModule.Max,

    MaxBy: Microsoft.FSharp.Collections.SeqModule.MaxBy,

    Min: Microsoft.FSharp.Collections.SeqModule.Min,

    MinBy: Microsoft.FSharp.Collections.SeqModule.MinBy,

    Get: function(source){
        return function(index){
            return source.get_Item(index);
        }
    },

    OfArray: Microsoft.FSharp.Collections.SeqModule.ToList,

    OfSeq: Microsoft.FSharp.Collections.SeqModule.ToList,

    Partition: function(func){
        var self = this;
        return function(source){
            var temp = self.arr.Partition(func)(source);
            var arr1 = temp.Item1;
            var arr2 = temp.Item2;
            return new Tuple(self.OfSeq(arr1), self.OfSeq(arr2));
        }
    },

    Permute: function(func){
        var self = this;
        return function(source){
            var temp = self.arr.Permute(func)(source);
            return self.OfSeq(temp);
        }
    },

    Pick: Microsoft.FSharp.Collections.SeqModule.Pick,

    Reduce: Microsoft.FSharp.Collections.SeqModule.Reduce,

    ReduceBack: function (func) {
        return function (source) {
            var acc = null;
            for (var i = (source.Length - 1); i >= 0; i--) {
                if (acc == null) {
                    acc = source.get_Item(i);
                    continue;
                }

                acc = func(acc)(source.get_Item(i));
            }

            return acc;
        }
    },

    Replicate: function(num){
        return function(value){
            var list = new Microsoft.FSharp.Collections.FSharpList.Empty();
            for(var i = 0; i< num; i++){
                list = new Microsoft.FSharp.Collections.FSharpList.Cons(value, list);
            }

            return list;
        }
    },

    Reverse: function (sequence) {

        var list = new Microsoft.FSharp.Collections.FSharpList.Empty();

        while(sequence.read()){
            list = new Microsoft.FSharp.Collections.FSharpList.Cons(sequence.get(), list);
        }

        return list;
    },

    Scan: function(func){
        var self = this;
        return function(state){
            return function(source){
                var temp = self.seq.Scan(func)(state)(source);
                return self.OfSeq(temp);
            }
        }
    },
    
    ScanBack: function (func) {
        var self = this;
        return function (source) {
            return function (state) {
                var arr = self.arr.OfSeq(source);
                var temp = self.arr.ScanBack(func)(arr)(state);
                return self.OfSeq(temp);
            }
        }
    },

    Sort: function(source){
        var arr = this.arr.OfSeq(source);
        var temp = this.arr.Sort(arr);
        return this.OfSeq(temp);
    },

    SortBy: function(func){
        var self = this;
        return function(source){
            var arr = self.arr.OfSeq(source);
            var temp = self.arr.SortBy(func)(arr);
            return self.OfSeq(temp);
        }
    },

    SortWith: function(func){
        var self = this;
        return function(source){
            var arr = self.arr.OfSeq(source);
            var temp = self.arr.SortWith(func)(arr);
            return self.OfSeq(temp);
        }
    },

    Sum: Microsoft.FSharp.Collections.ArrayModule.Sum,

    SumBy: Microsoft.FSharp.Collections.ArrayModule.SumBy,

    Tail: function(source){
        return source.Tail;
    },

    ToArray: Microsoft.FSharp.Collections.ArrayModule.OfSeq,

    ToSeq: Microsoft.FSharp.Collections.SeqModule.OfList,

    TryFind: Microsoft.FSharp.Collections.SeqModule.TryFind,

    TryFindIndex: Microsoft.FSharp.Collections.SeqModule.TryFindIndex,

    TryPick: Microsoft.FSharp.Collections.SeqModule.TryPick,

    Unzip: function(source){
        var temp = this.arr.Unzip(source);
        var arr1 = temp.Item1;
        var arr2 = temp.Item2;

        return new Tuple(this.OfSeq(arr1), this.OfSeq(arr2));
    },

    Unzip3: function(source){
        var temp = this.arr.Unzip3(source);
        var arr1 = temp.Item1;
        var arr2 = temp.Item2;
        var arr3 = temp.Item3;

        return new Tuple(this.OfSeq(arr1), this.OfSeq(arr2), this.OfSeq(arr3));
    },

    Zip: function(source1){
        var self = this;
        return function(source2){
            var temp = self.seq.Zip(source1)(source2);

            return self.OfSeq(temp);
        }
    },

    Zip3: function(source1){
        var self = this;
        return function(source2){
            return function(source3){
                var temp = self.seq.Zip3(source1)(source2)(source3);
                return self.OfSeq(temp);
            }
        }
    }
};



registerNamespace('Microsoft.FSharp.Core.LanguagePrimitives')
Microsoft.FSharp.Core.LanguagePrimitives.IntrinsicFunctions = {
    UnboxGeneric : function (x) { return x; }
};


Microsoft.FSharp.Collections.FSharpMap = {
    Empty : function () {
        this.Count = 0;
        this.Head = null;
        this.IsEmpty = true;
        this.Tail = null;
        this.get_Item = function (x) {
            return null;
        };

        this.get_Count = function () {
            return new System.Int32(this.Count);
        };

        this.get_Head = function () {
            return this.Head;
        };

        this.get_IsEmpty = function () {
            return this.IsEmpty;
        };

        this.get_Tail = function () {
            return this.Tail;
        };

        this.read = function () {
            return false;
        };

        this.reset = function(){
            this.ReadState = null;
        };

        this.ContainsKey = function (key) {
            return false;
        };

        this.Add = function (key, value) {
            return Microsoft.FSharp.Collections.MapModule.Add(key)(value)(this);
        };

        this.Remove = function (key) {
            return Microsoft.FSharp.Collections.MapModule.Remove(key)(this);
        };

    },

    Cons : function (arg, list) {
        this.ReadState = null;
        this.Count = list.Count + 1;
        this.Head = arg;
        this.IsEmpty = false;
        this.Tail = list;

        this.get_Item = function (x) {
            
            while(this.read()){
                var item = this.get();
                if(Microsoft.FSharp.Core.Operators.op_Equality(item.key)(x)){
                    this.reset();
                    return item.value;
                }
            }

            return null;
        };

        this.get_Count = function () {
            return new System.Int32(this.Count);
        };

        this.get_Head = function () {
            return this.Head;
        };

        this.get_IsEmpty = function () {
            return this.IsEmpty;
        };

        this.get_Tail = function () {
            return this.Tail;
        };

        this.Add = function (key, value) {
            return Microsoft.FSharp.Collections.MapModule.Add(key)(value)(this);
        };

        this.Remove = function (key) {
            return Microsoft.FSharp.Collections.MapModule.Remove(key)(this);
        };

       this.read = function () {
            if (this.ReadState == null)
                this.ReadState = -1;

            this.ReadState++;
            if (this.ReadState >= this.Count) {
                this.ReadState = null;
                return false;
            }


            return true;
        };

        this.getFunc = function(x){
            if (x == 0)
                return this.Head;
            else
                return this.Tail.getFunc(x - 1);
        }

        this.get = function () {
            return this.getFunc(this.ReadState);
        };

        this.reset = function(){
            this.ReadState = null;
        };

        this.ContainsKey = function (key) {
            return Microsoft.FSharp.Collections.MapModule.ContainsKey(key)(this);
        }
    }

};

Microsoft.FSharp.Collections.MapModule = {
    list : Microsoft.FSharp.Collections.ListModule,
    seq : Microsoft.FSharp.Collections.SeqModule,
    
    Add : function (key) {
        return function (value) {
            return function (source) {
                var item = { key: key, value: value };
                return new Microsoft.FSharp.Collections.FSharpMap.Cons(item, source);
            }
        }
    },

    ContainsKey : function (key) {
        return function (source) {
            while (source.read()) {
                var item = source.get();
                if (Microsoft.FSharp.Core.Operators.op_Equality(item.key)(key)) {
                    source.reset();
                    return true;
                }
            }

            return false;
        }
    },

    Empty : function () {
        return new Microsoft.FSharp.Collections.FSharpMap.Empty();
    },

    Exists: function(func){
        return function(source){
            while(source.read()){
                var item = source.get();
                var result = func(item.key)(item.value);
                if(result)
                {
                    source.reset();
                    return true;
                }
            }

            return false;
        }
    },

    Filter: function(func){
        var self = this;
        return function(source){
            var map = self.Empty();
            while(source.read()){
                var item = source.get();
                if(func(item.key)(item.value)){
                    map = self.Add(item.key)(item.value)(map);
                }
            }
            return map;
        }
    },

    Find : function (key) {
        return function (source) {
            while (source.read()) {
                var item = source.get();
                if (Microsoft.FSharp.Core.Operators.op_Equality(item.key)(key)) {
                    source.reset();
                    return item.value;
                }
            }

            return null;
        }

    },

    FindKey: function(func){
        var self = this;
        return function(source){
            while(source.read()){
                var item = source.get();
                var result = func(item.key)(item.value);
                if(result){
                    source.reset();
                    return item.key;
                }
            }

            throw new "KeyNotFoundException";
        }
    },

    Fold: function(func){
        var self = this
        return function(state){
            return function(source){
                var acc = state;
                while(source.read()){
                    var item = source.get();
                    acc = func(acc)(item.key)(item.value);
                }

                return acc;
            }
        }
    },

    FoldBack: function(func){
        var self = this;
        return function(source){
            return function(state){
                var val = state;
                while(source.read()){
                    var item = source.get();
                    val = func(item.key)(item.value)(val);
                }

                return val;
            }
        }
    },

    ForAll: function(func){
        var self = this;
        return function(source){
            while(source.read()){
                var item = source.get();
                var result = func(item.key)(item.value);
                if(!result){
                    source.reset();
                    return false;
                }
            }
            return true;
        }
    },

    IsEmpty: function(source){
        return source.IsEmpty;
    },

    Iterate: function(func){
        return function(source){
            while(source.read()){
                var item = source.get();
                func(item.key)(item.value);
            }
        }
    },

    Map: function(func){
        var self = this;
        return function(source){
            var map = self.Empty();
            while(source.read()){
                var item = source.get();
                var result = func(item.key)(item.value);
                map = self.Add(item.key)(result)(map);
            }

            return self.Reverse(map);
        }
    },

    Reverse: function(source){
        var map = this.Empty();
        while(source.read()){
            var item = source.get();
            map = this.Add(item.key)(item.value)(map);
        }
        return map;
    },

    OfArray: function(source){
        return this.OfSeq(source);
    },

    OfList: function(source){
        var map = this.Empty()
        var reversed = this.list.Reverse(source);
        while(reversed.read()){
            var item = reversed.get();
            map = this.Add(item.Item1)(item.Item2)(map);
        }

        return map;
    },

    OfSeq: function(source){
        var map = this.Empty();
        while(source.read()){
            var item = source.get();
            map = this.Add(item.Item1)(item.Item2)(map);
        }

        return map;
    },

    Partition: function(func){
        var self = this;
        return function(source){
            var map1 = self.Empty();
            var map2 = self.Empty();

            while (source.read()) {
                var item = source.get();
                if (func(item.key)(item.value)) {
                    map1 = self.Add(item.key)(item.value)(map1);
                } else {
                    map2 = self.Add(item.key)(item.value)(map2);
                }
            }

            return new Tuple(self.Reverse(map1), self.Reverse(map2));
        }
    },

    TryPick: function (func) {
        var self = this;
        return function (source) {
            var mapped = self.Map(func)(source);
            return self.TryFind(function (x) {
                return Microsoft.FSharp.Core.FSharpOption.get_IsSome(x);
            })(mapped).get_Value();
        }
    },

    Pick: function (func) {
        var self = this;
        return function (source) {
            while(source.read()){
                var item = source.get();
                var result = func(item.key)(item.value);
                if(Microsoft.FSharp.Core.FSharpOption.get_IsSome(result))
                {
                    source.reset();
                    return result.get_Value();
                }
            }

            return null;
        }
    },

    Remove: function(key){
        var self = this;
        return function(source){
            var result = new Microsoft.FSharp.Collections.FSharpMap.Empty();
            while (source.read()) {
                var item = source.get();
                if(Microsoft.FSharp.Core.Operators.op_Inequality(item.key)(key)){
                    result = new Microsoft.FSharp.Collections.FSharpMap.Cons(item, result);
                }
            }

            return self.Reverse(result);
        }
    },

    ToArray: function(source){
        var arr = [];
        while(source.read()){
            var item = source.get();
            var tup = new Tuple(item.key, item.value);
            arr.push(tup);
        }

        return arr;
    },

    ToList: function(source){
        var result = this.list.Empty();
        while(source.read()){
            var item = source.get();
            var tup = new Tuple(item.key, item.value);
            result = new Microsoft.FSharp.Collections.FSharpList.Cons(tup, result);
        }

        return this.list.Reverse(result);
    },

    ToSeq: function(source){
        return new Sequence(this.ToArray(source));
    },

    TryFind : function (key) {
        return function (source) {
            while (source.read()) {
                var item = source.get();
                if (Microsoft.FSharp.Core.Operators.op_Equality(item.key)(key)) {
                    source.reset();
                    return new Microsoft.FSharp.Core.FSharpOption.Some(item.value);
                }
            }

            return new Microsoft.FSharp.Core.FSharpOption.None();
        }
    },

    TryFindKey: function(func){
        var self = this;
        return function(source){
            while(source.read()){
                var item = source.get();
                var result = func(item.key)(item.value);
                if(result){
                    source.reset();
                    return new Microsoft.FSharp.Core.FSharpOption.Some(item.key);
                }
            }

            return new Microsoft.FSharp.Core.FSharpOption.None();
        }
    },

    TryPick: function(func){
        var self = this;
        return function(source){
            while(source.read()){
                var item = source.get();
                var result = func(item.key)(item.value);
                if(Microsoft.FSharp.Core.FSharpOption.get_IsSome(result)){
                    source.reset();
                    return result;
                }
            }

            return new Microsoft.FSharp.Core.FSharpOption.None();
        }
    }

    
};

Pad = function (number, length) {
    var str = '' + number;
    while (str.length < length) {
        str = '0' + str;
    }

    return str;
};

System.Enum = function () { };
System.Enum.prototype.toString = function () {
    return this.Text;
};

System.Enum.prototype.Equality = function (x) {
    return x.Integer == this.Integer;
};



System.DateTime = function () {
    this.Year = 0001;
    this.Month = 1;
    this.Day = 1;
    this.Hour = 12;
    this.Minute = 0;
    this.Second = 0;

    if (arguments.length == 3) {
        this.Year = arguments[0];
        this.Month = arguments[1];
        this.Day = arguments[2];
    }

    if (arguments.length == 6) {
        this.Year = arguments[0];
        this.Month = arguments[1];
        this.Day = arguments[2];
        this.Hour = arguments[3];
        this.Minute = arguments[4];
        this.Second = arguments[5];
    }


    this.toString = function () {

        var amPm = this.Hour > 12 ? "pm" : "am";
        return this.Month + "/" + this.Day + "/" + Pad(this.Year, 4) + " " + this.Hour + ":" + Pad(this.Minute, 2) + ":" + Pad(this.Second, 2) + " " + amPm;
    };

    this.ToShortDateString = function () {
        var amPm = this.Hour > 12 ? "pm" : "am";
        return this.Month + "/" + this.Day + "/" + Pad(this.Year, 4);
    };

    this.AddYears = function (x) {
        return new System.DateTime(this.Year + x, this.Month, this.Day, this.Hour, this.Minute, this.Second);
    };

    this.AddMonths = function (x) {
        var d = new Date(this.Year, ((this.Month - 1) + x), this.Day);

        return new System.DateTime(d.getFullYear(), d.getMonth() + 1, d.getDate(), this.Hour, this.Minute, this.Second);
    };

    this.AddDays = function (x) {
        var d = new Date(this.Year, (this.Month - 1), this.Day + x);

        return new System.DateTime(d.getFullYear(), d.getMonth() + 1, d.getDate(), this.Hour, this.Minute, this.Second);
    };


    this.Equality = function (x) {
        var result = true;
        result = result && this.Year === x.Year;
        result = result && this.Month === x.Month;
        result = result && this.Day === x.Day;
        result = result && this.Hour === x.Hour;
        result = result && this.Minute === x.Minute;
        result = result && this.Second === x.Second;

        return result;
    };

    var getJavascriptDate = function (x) {
        var date = new Date(x.Year, (x.Month - 1), x.Day, x.Hour, x.Minute, x.Second);

        return date;
    }

    this.op_GreaterThan = function (x) {
        var date1 = getJavascriptDate(this);
        var date2 = getJavascriptDate(x);

        return date1 > date2;
    }

    this.op_LessThan = function (x) {
        var date1 = getJavascriptDate(this);
        var date2 = getJavascriptDate(x);

        return date1 < date2;
    }

    this.op_GreaterThanOrEqual = function (x) {
        var date1 = getJavascriptDate(this);
        var date2 = getJavascriptDate(x);

        return date1 >= date2;
    }

    this.op_LessThanOrEqual = function (x) {
        var date1 = getJavascriptDate(this);
        var date2 = getJavascriptDate(x);

        return date1 <= date2;
    }


};

System.DateTime.get_Now = function () {
    var d = new Date();
    return new System.DateTime(d.getFullYear(), d.getMonth() + 1, d.getDate(), d.getHours(), d.getMinutes(), d.getSeconds());
};

System.DateTime.Parse = function (x) {
    var d = new Date(x);
    var hours = d.getHours() > 0 ? d.getHours() : 12;

    return new System.DateTime(d.getFullYear(), d.getMonth() + 1, d.getDate(), hours, d.getMinutes(), d.getSeconds());
};

System.DateTime.MinValue = new System.DateTime();

String.prototype.Contains = function (x) {
    return this.indexOf(x) != -1;
};
String.prototype.Replace = function(search, replace) {
    return this.replace(search, replace);
}
String.prototype.get_Length = function(){
    return new System.Int32(this.length);
};


System.String = {
    Join: function (seperator, source) {
        var result = Microsoft.FSharp.Collections.SeqModule.Fold(function (acc) {
            return function(next){
                return acc + next + seperator;
            }
        })("")(source);

        return result.slice(0, result.length - (seperator.length));
    }
};

registerNamespace("System.Text");

System.Text.RegularExpressions = {
    Regex : function(regex){
        var innerRegex = new RegExp(regex);

        this.IsMatch = function(toMatch){
            return innerRegex.test(toMatch);
        }
    }
};