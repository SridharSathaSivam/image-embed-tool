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

	var data = [];
	var img = mapJson["img"];
	for (var i = img.length; i > 0; i -= 3) {
		var imgData = { "ImageOne": img[i - 1] };
		var img1 = img[i - 2];
		var img2 = img[i - 3];
		if (img1) { imgData["ImageTwo"] = img1 };
		if (img2) { imgData["ImageThree"] = img2 };
		data.push(imgData)
	}

	var dataSource = `var dataSource = ${JSON.stringify(data, null, 4)}`;
	fs.writeFileSync('./wwwroot/dataSource.js', dataSource, 'utf8');
}