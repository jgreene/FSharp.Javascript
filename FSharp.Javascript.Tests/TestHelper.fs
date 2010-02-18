#light
module TestHelper

open FSharp.Javascript.Printer
open FSharp.Javascript.Parser
open NUnit.Framework


let print input =
    System.Console.WriteLine((sprintf "%A" input))

let test input = 
    print "-------------------- input --------------------"
    print input
    let ast = getAst input
    let javascript = getJavascript ast
    print "-------------------- first --------------------"
    print javascript
    let ast2 = getAst javascript
    let javascript2 = getJavascript ast2
    print "-------------------- second --------------------"
    print javascript2

    let result = (ast = ast2)

    //if result = false then
    print ast
    print ast2
                                      
    Assert.IsTrue(result)

let output value (name:string) =
    use file1 = new System.IO.StreamWriter(name)
    file1.WriteLine((sprintf "%A" value))

let testLibrary input =
    let ast = getAst input
    let javascript = getJavascript ast
    print "-------------------- first --------------------"
    print javascript
    let ast2 = getAst javascript
    let javascript2 = getJavascript ast2
    print "-------------------- second --------------------"
    print javascript2

    let result = (ast = ast2)

    if result = false then
        output ast "ast1.txt"
        output ast2 "ast2.txt"
        output javascript "javascript1.txt"
        output javascript2 "javascript2.txt"
                                          
    Assert.IsTrue(result)



             


