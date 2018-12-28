var fs = require("fs");

module.exports = function (callback, file, extension) {
    var imgName = 'img' + new Date().getTime() + extension;
    console.log('Image Name: ' + imgName);
    var imgPath = './wwwroot/uploadedImages/';
    fs.renameSync(imgPath + file, imgPath + imgName);
    var mapPath = './mapper.json';
    var mapJson = require(mapPath);
    mapJson["img"].push(imgName);
    fs.writeFileSync(mapPath, JSON.stringify(mapJson, null, 4), 'utf8');
}