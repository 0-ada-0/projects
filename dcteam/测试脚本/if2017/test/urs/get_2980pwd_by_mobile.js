//2980帐号通过手机号码重置密码
var params = 
{
	account:'insiststrive',//2980邮箱帐号,需要@2980.com后缀,手机帐号也需要在手机后面加@2980.com，必选参数
	password:'e10adc3949ba59abbe56e057f20f883e',//旧密码的MD5，可选参数
	newpwd:'e10adc3949ba59abbe56e057f20f883e',//新密码的MD5，必选参数
	ip:'127.0.0.1',//来源ip，是IP字符串，必选参数
	tel:'13268247551',//手机号码，必选参数
	key:'',//获取短信验证码接口返回的key。必选参数（测试服可以填任意值）
	smscode:'',//短信验证码。必选参数（测试服可以填任意值）
	forceset:''	//0的时候会校验旧密码，1的时候不会校验旧密码。可选参数（默认是0，需要校验旧密码）
};
//上面的结构是请求参数和参数值，不需要值的参数则填''
//测试人员只需要在上面的结构填写测试数据即可

var paramskey=new Array('account','password','newpwd','ip','tel','key','smscode','forceset');//该数组是接口所有请求参数
var path = "/urs/get_2980pwd_by_mobile";
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


