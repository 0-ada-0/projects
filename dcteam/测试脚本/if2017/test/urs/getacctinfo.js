//获取帐号信息
//account支持三种帐号，1、普通邮箱帐号，2、2980字母邮箱帐号（需要@2980.com），3、2980手机号帐号（不用@2980.com）
var params = 
{
  gate: "sw",//产品代号，必选参数
  account:'13268247551' //'940235644@qq.com' "romic005@2980.com" 13268247551  字母帐号,必选参数 
  vcode:'a2w3',//图形验证码，必选参数
  vregval'70f80d9368f54a92ab7cb832966aab17'//获取图形验证码返回的键
};
//上面的结构是请求参数和参数值，不需要值的参数则填''
//测试人员只需要在上面的结构填写测试数据即可

var paramskey=new Array('gate','account');//该数组是接口所有请求参数
var path = "/urs/getacctinfo";
var hostconf = require('../host');
var url = hostconf.gethost()+path+'?';

paramskey.sort();
var signstr=path;
for (i=0;i<paramskey.length;i++)
{
	var key = paramskey[i];
	if(params[key]!=null)
    signstr += paramskey[i]+params[key];
}
signstr += '#erv$ed%@3l4';
signstr = signstr.toLowerCase()
console.log(signstr);

var sign = hostconf.getsign(signstr);
url+='sign='+sign;
console.log(url);

//new Date(Date.now());
var moment = require("moment");
var timestr = moment().format("YYYY-MM-DD HH:mm:ss")

var rq = require('request-promise');
return rq(url,{qs:params,rejectUnauthorized:false  })
.then(
    function(data)
	{
	 var formsdata='';
	 for (i=0;i<paramskey.length;i++)
	 {
	   var key = paramskey[i];
	   if(params[key]!=null)
       formsdata += paramskey[i]+'='+params[key]+'&';
     }
	 
	 var fs = require("fs");   
     var iconv = require('iconv-lite'); 	 
     var arrdata = iconv.encode(timestr+'-------------------------------------------------------------------------------------------'+
	 '\r\n'+'request:'+url+'   '+formsdata+'\r\n'+'response:'+data+'\r\n','UTF8');  
     // appendFile，如果文件不存在，会自动创建新文件  
     // 如果用writeFile，那么会删除旧文件，直接写新文件  
     fs.appendFile('..'+path+'.txt', arrdata, function(err){  
        if(err)  
            console.log("fail " + err);  
        else  
            console.log("写入文件ok");  
     }); 
	  return data;
    }
)
.then(console.log,console.error)


