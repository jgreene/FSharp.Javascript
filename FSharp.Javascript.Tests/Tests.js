var QuotationsTestHelper = {}
QuotationsTestHelper.emit = function (x) {
    var sys = require('sys')
    var fs = require('fs')

    sys.print("got here");

    try{

        fs.writeFileSync('testResult.js', x.toString());
    }
    catch (err){
        fs.writeFileSync('testResult.js', err.toString());
    }
}