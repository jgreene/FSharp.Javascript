#light
namespace FSharp.Javascript.Tests

open NUnit.Framework
open FSharp.Javascript.Printer
open TestHelper

[<TestFixture>]
type PrintingTests() =
    
    [<Test>]
    member this.``Single Assignment With PlusOp``() =
        let script = "var a = 1 + 1";
        test script

    [<Test>]
    member this.``Double Assignment With Plus and Minus Ops``() =
        let script = "var a = 1 + 2;
var b = 3 + 4;"
        test script

    [<Test>]
    member this.``Function with multiple statements``() =
        let script = "function Name(){
    var a = 1 + 2;
    var b = 3 + 4;
}"
        test script

    [<Test>]
    member this.``Function``() =
        let script = "var a = function(){ 1 + 1 }"
        test script

    [<Test>]
    member this.``Global Function``() =
        test "a = function() { 1 + 1 }"

    [<Test>]
    member this.``Function with arguments``() =
        let script = "var a = function(x,y) { }"
        test script

    [<Test>]
    member this.``BooleanNode``() =
        test "var a = true;"

    [<Test>]
    member this.``Global assign``() =
        test "a = true;"

    [<Test>]
    member this.``Identifier with operations``() =
        let script = "var a = 1;
        var b = a + 2"
        test script

    [<Test>]
    member this.``ForInNode with body``() =
        let script = "for(var i in array){
    1 + 1
};"
        test script

    [<Test>]
    member this.``ForStepNode with body``() =
        let script = "for(var i = 0; i < array.length; i = i + 1){
    1 + 1
}"
        test script

    [<Test>]
    member this.``ForStepNode with AssignmentBlock``() =
        test "for(var i = 0, l = this.length; i<l; i++){
    1 + 1
}"

    [<Test>]
    member this.``Call test with arguments``() =
        let script = "var a = function(x,y){

}

var b = a(1,2);"
        test script

    [<Test>]
    member this.``Nested functions``() =
        let script = "var a = function(){
    var b = function(){

    }
}"
        test script

    [<Test>]
    member this.``Call test without arguments``() =
        let script = "var a = function(){

}
var b = a()"
        test script

    [<Test>]
    member this.``TryCatchFinally``() =
        let script = "try{
    3 + 3
}
catch(ex){
    2 + 2
}
finally{
    1 + 1
}"
        test script

    [<Test>]
    member this.``TryCatchFinally inside function``() =
        let script = "var a = function(){
    try{
        3 + 3
    }
    catch(ex){
        2 + 2
    }
    finally{
        1 + 1
    }
}"
        test script

    [<Test>]
    member this.``For loop with continue and break``() =
        let script = "for(var i = 0; i < array.length; i = i + 1){
    continue;
    break;
}"
        test script

    [<Test>]
    member this.``Delete property``() =
        let script = "var a = { b : 1 };
delete a.b;"
        test script

    [<Test>]
    member this.``New``() =
        test "var a = new Object()"

    [<Test>]
    member this.``New with args``() =
        test "var a = new Object(1,2,3)"

    [<Test>]
    member this.``New with properties inside function``() =
        test "var a = function(){
        var b = { c : 1, d : 2, e : function() { 1 + 1 } }
}"

    [<Test>]
    member this.``IfElse``() =
        test "if(true){
    1 + 1
}else {
    2 + 2
}"

    [<Test>]
    member this.``If``() =
        test "if(true) { 1 + 1 }"

    [<Test>]
    member this.``IfThen ElseIf Else``() =
        test "if(true) { 1 + 1 } else if(false) { 2 + 2 } else { 3 + 3 }"

    [<Test>]
    member this.``IfThen ElseIf ElseIf Else``() =
        test "if(true) { 1 + 1 } else if(false) { 2 + 2 } else if(true) { 3 + 3 } else { 4 + 4 }"

    [<Test>]
    member this.``TernaryOp``() =
        test "true ? 1 : 2"

    [<Test>]
    member this.``IndexAccess``() =
        test "array[0]"

    [<Test>]
    member this.``Property with IndexAccess``() =
        test "a.b[0]"

    [<Test>]
    member this.``IndexAccess with multi dimensional array``() =
        test "array[0,0]"

    [<Test>]
    member this.``In``() =
        test "x in { x : 1 }"

    [<Test>]
    member this.``InstanceOf``() =
        test "'string' instanceof String"

    [<Test>]
    member this.``Logical Operator``() =
        test "1 < 2"

    [<Test>]
    member this.``Null``() =
        test "null"

    [<Test>]
    member this.``Postfix operators``() =
        test "a++"

    [<Test>]
    member this.``Regex with modifier``() =
        test "/t/g"

    [<Test>]
    member this.``StrictCompare``() =
        test "1 !== 1"

    [<Test>]
    member this.``Switch``() =
        test "switch(true){
    case false:
        1 + 1
        break;
    default:
        2 + 2
}"
    [<Test>]
    member this.``Switch inside function``() =
        test "var a = function(){
    switch(true){
        case true:
            3 + 3
        case false:
            1 + 1
            break;
        default:
            2 + 2
    }
}"
    
    [<Test>]
    member this.``Throw``() =
        test "throw 'fail!'"

    [<Test>]
    member this.``TypeOf``() =
        test "typeof a"

    [<Test>]
    member this.``UnaryOp``() =
        test "-a"

    [<Test>]
    member this.``UnsignedRightShift``() =
        test "1 >>> 2"

    [<Test>]
    member this.``Void``() =
        test "void(0)"

    [<Test>]
    member this.``While loop``() =
        test "while(true){
    1 + 1;
}"
    
    [<Test>]
    member this.``DoWhile loop``() =
        test "do{
    1 + 1
} while(true)"

    [<Test>]
    member this.``With``() =
        test "with(a) {
    b = 1;
}"

    [<Test>]
    member this.``Return``() =
        test "var a = function(){
    return 1;
};

a();"

    [<Test>]
    member this.``Anonymous function called``() =
        test "(function(){ return 1; })();"

    [<Test>]
    member this.``Empty array``() =
        test "var a = []"

    [<Test>]
    member this.``Array with values``() =
        test "var a = [1,2,3,4]"

    [<Test>]
    member this.``Grouping works properly``() =
        test "var a = true && (false || b)"

    [<Test>]
    member this.``Grouping with functioncall works properly``() =
        test "(\" \" + this[i].className + \" \").replace(rclass, \" \")"

    [<Test>]
    member this.``For statement with null``()=
        test "for(null; (i < length); i++){
    1 + 1
}"
    
    [<Test>]
    member this.``array access with PostfixOperator``() =
        test "object[++i]"

    [<Test>]
    member this.``AssignmentBlock with new RegExp``() =
        test "var a = 1, namespace = new RegExp(\"(^|\\.)\" + cleaned.join(\"\\.(?:.*\\.)?\") + \"(\\.|$)\")"

    [<Test>]
    member this.``For loop with global assignment``() =
        test "for ( i = 0, l = match.length; i < l; i++ ) {
    1 + 1
}"

    [<Test>]
    member this.``Global assign multiple``() =
        test "(a = 1, b = 2, c =3)"

    [<Test>]
    member this.``While grouping``() =
        test "while ( (chunker.exec(\"\"), m = chunker.exec(soFar)) !== null ) {
    1 + 1
}"

    [<Test>]
    member this.``PreDecrementOperator with BooleanReversal``() =
        test "!--jQuery.active"

    [<Test>]
    member this.``New Date without constructor``() =
        test "var a = (new Date).getTime()"

    [<Test>]
    member this.``New Date no args``() =
        test "var a = new Date().getTime()"

    [<Test>]
    member this.``New Date with args``() =
        test "var a = new Date(1).getTime()"

    [<Test>]
    member this.``Local assign without value``() =
        test "var sortOrder;"

    [<Test>]
    member this.``Function with name``() =
        test "function Name() { }"

    [<Test>]
    member this.``Function argument with name``() =
        test "div.attachEvent(\"onclick\", function click() {
	div.detachEvent(\"onclick\", click);
});"
    
    [<Test>]
    member this.``Array creation with multiple elements``() =
        test "var a = ['one','two','three']"

    [<Test>]
    member this.``Object method call``() =
        test "var a = { SomeMethod : function(){ return 1 + 1; } }; a.SomeMethod();"

    [<Test>]
    member this.``IndexAccess with parenthesis``() =
        test "(opt.specialEasing = opt.specialEasing || {})[p] = prop[p][1];"