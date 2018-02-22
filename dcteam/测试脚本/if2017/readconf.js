//var rf=require("fs");  
//var data=rf.readFileSync("C://webconfig//dc2016//101//lv1//conf//dc2.json","utf-8");  
//console.log(data);  
//var str = JSON.stringify(data);
//console.log(str); 
var json ={
	"host":"https//if7.duoyi.com",
	"path":"/common/sendsmscode",
	"params":{
		"tel":"13268247551"
	}
}



var rq = require('request-promise');

var url = 'http://127.0.0.1:40801/common/sendsmscode';
rq(url)
.then(JSON.parse)
.then(function(data){
	var id = data.requestID;
	console.log
	//return rq('http://127.0.0.1:40801/common/sendsmscode',{form{},method:'post'})
	return rq('http://127.0.0.1:40801/common/sendsmscode',{qs:{requestID:id}})
})
.then(console.log,console.error);