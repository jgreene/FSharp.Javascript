var QuotationsTestHelper = {}
QuotationsTestHelper.emit = function (x) {
    var fs = require('fs')

    var fileName = __filename + "Result.js"
    try{

        fs.writeFileSync(fileName, x.toString());
    }
    catch (err){
        fs.writeFileSync(fileName, err.toString());
    }
}