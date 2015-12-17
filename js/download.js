http = require('http');
fs = require('fs');
crypto = require('crypto');

var download = function(url, callback) {
  var request = http.get(url, function(response) {
    let buffers = []

    response.on('data', function(chunk) {
      buffers.push(chunk)
    });

    req.on('end', () => {
      callback(Buffer.concat(buffers));
      next()
    });

  }).on('error', function(err) {
    fs.unlink(dest);
    if (callback) {
      callback(err.message);
    }
  });
};

download("http://frankevers.nl/img/iherou.jpg", null);


// let buffers = []
//
// req.on("data", (chunk) => {
//   buffers.push(chunk)
// })
//
// req.on("end", () => {
//   req.body = Buffer.concat(buffers)
//   next()
// })
