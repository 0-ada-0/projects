module. exports={
gethost:function()
  {
	var url = 'https://if7d.duoyi.com';//��ʽ����ַ
	//var url = 'http://10.32.64.36:20801';//�������Է���ַ
	//var url = 'http://127.0.0.1:40801';//�������Ե�ַ
	return url;
  },
  getsign:function(signstr)//����ǩ��
  {
	var crypto = require('crypto'); 
    var sha256 = crypto.createHash('sha256');//������ܷ�ʽ:md5������,�˴���md5���Ի�������hash���ܵķ������ƣ�
    sha256.update(signstr);
    var sign = sha256.digest('hex');  //���ܺ��ֵd
	return sign;
  }
};