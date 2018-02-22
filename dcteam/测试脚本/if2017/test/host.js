module. exports={
gethost:function()
  {
	var url = 'https://if7d.duoyi.com';//正式服地址
	//var url = 'http://10.32.64.36:20801';//外网测试服地址
	//var url = 'http://127.0.0.1:40801';//本机调试地址
	return url;
  },
  getsign:function(signstr)//计算签名
  {
	var crypto = require('crypto'); 
    var sha256 = crypto.createHash('sha256');//定义加密方式:md5不可逆,此处的md5可以换成任意hash加密的方法名称；
    sha256.update(signstr);
    var sign = sha256.digest('hex');  //加密后的值d
	return sign;
  }
};