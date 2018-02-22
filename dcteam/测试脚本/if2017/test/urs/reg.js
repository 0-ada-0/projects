//注册帐号（普通邮箱帐号）
var params = 
{
  "email": "chen19924912@163.com",//邮箱帐号（必须参数）
  "pass": "123456",//密码(如果encrypt字段为true，则为经过md5加密的32位字符，否则则用明文传输，必须参数)
  "encrypt": "false",//密码是否加密，true表示加密（非必须参数，默认是加密）
  "tname": "",//姓名（非必须参数）
  "mobile": "13268247551",//手机号码（必须参数）
  "idcard": "",//身份证号码（非必须参数）
  "gatesrc": "",//游戏代号（非必须参数，不传默认urs）
  "pstype": "game",//非必须参数，不传默认是game
  "qq": "",//QQ帐号，（非必须参数）
  "ip": "127.0.0.1",//客户端IP，（非必须参数，建议必传）
  "vcode": "999X",//图片验证码（非必须参数，需要验证码时提供）
  "vregval": "VTRGS1FhRksya1c3UmVCbVpEb3BwdC8wK1l3Ykx3NlpHUkRGcGJIVTZsRExVb1VoNXJEVkxWL3MxYTlRK0svUA==",//图片验证码的键。（非必须参数，需要验证码时提供）
  "adsid": "",//页面推广id（非必须参数）
  "tgaccount": "",//桌面退关id（非必须参数）
  "language": "",//语言（非必须参数）
  "extargs": "",//附加参数（非必须参数）
};
//上面的结构是请求参数和参数值，不需要值的参数则填''
//测试人员只需要在上面的结构填写测试数据即可

var paramskey=new Array('email','pass','encrypt','tname','mobile','idcard','gatesrc','pstype','qq','ip','vcode','vregval','adsid','tgaccount','language','extargs');//该数组是接口所有请求参数
var path = "/urs/reg";
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


