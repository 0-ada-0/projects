//通过手机号码获取帐号
var params = 
{
	mobile:'13268247551',//手机号码（必须参数）
	smscode:'',//短信验证码（可选参数）
	key:'',//获取短信验证码返回的key（可选参数）
	isvalid:'1'//是否验证验证码（可选参数），默认是0，需要检验短信验证码。1则不校验。如果校验短信验证码，smscode和key必填
};
//上面的结构是请求参数和参数值，不需要值的参数则填''
//测试人员只需要在上面的结构填写测试数据即可

var paramskey=new Array('mobile',"smscode",'key','isvalid');//该数组是接口所有请求参数
var path = "/urs/get_acct_by_mobile";
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
console.log(signstr);

var sign = hostconf.getsign(signstr);
url+='sign='+sign;
console.log(url);

//new Date(Date.now());
var moment = require("moment");
var timestr = moment().format("YYYY-MM-DD HH:mm:ss")

var rq = require('request-promise');
return rq(url,{qs:params,rejectUnauthorized:false})
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


