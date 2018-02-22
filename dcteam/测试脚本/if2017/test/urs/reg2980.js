//注册2980帐号
var params = 
{
  mobile: "13268247566",//手机号码（以1开头的11位数字字符串），必选参数
  password: "e10adc3949ba59abbe56e057f20f883e",//密码（必须是经过md5加密的32位字符串），必选参数
  account: "liuxiaoqinagab",//字母帐号（不需要@2980.com，6-16个字母数字字符），可选参数
  name: "",//姓名，可选参数
  idnumber: "",//身份证号，可选参数
  smskey: "",//获取短信验证码时等到的key，必选参数
  smscode: ""//短信验证码，必选参数
};
//上面的结构是请求参数和参数值，不需要值的参数则填''
//测试人员只需要在上面的结构填写测试数据即可

var paramskey=new Array('mobile','password','account','name','idnumber','smskey','smscode');//该数组是接口所有请求参数
var path = "/urs/reg2980";
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
return rq(url,{form:params,method:'post',rejectUnauthorized:false  })
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


