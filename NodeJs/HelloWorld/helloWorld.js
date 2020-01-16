const http = require('http');
const dt = require('./firstModule')
const url = require('url');


http.createServer(function(req, res) {
    res.writeHead(200, {'Contetnt-Type': 'text/html' });
    res.write("The date and time are currently: " + dt.myDateTime());
    // res.write(req.url);

    var q = url.parse(req.url, true).query;
    var txt = q.year + " " + q.month;
    res.end(txt);
}).listen(8080);

