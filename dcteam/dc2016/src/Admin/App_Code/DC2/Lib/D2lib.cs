using DC2016.Admin.Common;
using DC2016.Admin.Configs;
using DC2016.Admin.Controllers.Common;
using DC2016.Admin.DC;
using DC2016.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using dywebsdk.Mails;
using DC2016.Admin.P2980;

namespace DC2016.Admin.DC2.Lib
{
    public class D2lib
    {

        public static string Lib_BuildCdKey(long keynum, int legth)
        {
            //去掉了数字0，字母O、i ，只有33个字符，按33进制使用
            string[] keyarray = { "V", "T", "L", "P", "D", "4", "6", "C", "J", "H", "S", "Z", "Y", "7", "F", "K", "3", "E", "X", "N", "9", "2", "G", "R", "B", "5", "W", "8", "A", "M", "1", "I", "O" };
            long long_limit = 1406408618241;//生成的8位字符上限，超过后就是9位了
            long calenum = (long)keynum * 10000;
            calenum += new Random().Next(1, 10000);
            calenum = calenum % long_limit;//超8位上限后重新轮回,因为数值够大，理伦上到轮回时，之前的应该都已经过期了
            int len = keyarray.Length;
            List<int> li_index = new List<int>();
            while (calenum > 0)
            {
                int index = (int)(calenum % len);
                li_index.Add(index);
                calenum /= len;
            }
            li_index.Reverse();
            while (li_index.Count < legth)
                li_index.Insert(0, 0);//不足8位数时补0

            StringBuilder sb = new StringBuilder();
            foreach (int index in li_index)
                sb.Append(keyarray[index]);
            return sb.ToString();
        }


        #region 邮件相关

        public static bool SendMail_Henhaoji(object obj, string FromName, string ToName, string ToMail, string Subject, string Message, bool Html)
        {
            string[] acctexts = { "e1d0cfb10e", "c6d21323c3", "e3c49491d4", "1a15cb4ff3", "9107e5e628" };
            //2013-01-09 test
            //2013-01-16 10%,暂只用于找回密码，激活未用
            string fromName = FromName;
            if (string.IsNullOrEmpty(fromName)) fromName = "多益网络";
            string fromMail = DC2Conf.IsHerojoys ? "acctactive@herojoys.com" : "acctactive@duoyi.com";
            bool isModeForceHenhaoji = false;
            if (ToMail.ToLower().EndsWith("@163.com"))
                isModeForceHenhaoji = false;
            bool rand5 = false;
            bool randlet = true;
            if (ToMail.ToLower().EndsWith("@qq.com") || ToMail == "netcsy@163.com")
            {
                //isModeForceHenhaoji = true;
                //rand5 = true;
            }
            if (rand5 && fromMail == "acctactive@duoyi.com")
            {
                fromMail = string.Format("acctactive_{0}@duoyi.com", acctexts[new Random().Next(0, acctexts.Length)]);
            }
            if (isModeForceHenhaoji == false)
            {
                int port = 0;
                if (randlet && obj is D2unactiveInfo)
                {
                    D2unactiveInfo ua = (D2unactiveInfo)obj;
                    int seed = ua.UavNumber.Value % 100;
                    if (ua.UavGUID == "f539815b6cfb47a2b7add3baa5cc441a")//test guid
                        seed = new Random().Next(0, 7);
                    if (seed >= 1 && seed <= 6)
                        port = seed;
                }
                return Func_SendMail_SendServer(port, fromName, fromMail, ToName, ToMail, Subject, Message, Html);
            }

            /* 暂不支持
            NetworkCredential myNetworkCredential = new NetworkCredential("mxsystem", "hhjxt1234");
            NetworkCredential myNetworkCredential = new NetworkCredential("acctactive", "e1d0cfb10e");
            SmtpClient mail = new SmtpClient("mail.henhaoji.com");
            mail.UseDefaultCredentials = false;
            mail.Credentials = myNetworkCredential;
            MailMessage mmsg = new MailMessage();
            mmsg.From = new MailAddress("acctactive@henhaoji.com", fromName);
            if (ToMail.ToLower().StartsWith("2241") || new Random().Next(0, 100) <= (DateTime.Now - DateTime.Parse("2012-10-03")).TotalDays * 5 + 20)//某测试帐号或1/10概率，使用@duoyi.com发送
                mmsg.From = new MailAddress("acctactive@duoyi.com", fromName);
            //if (ToMail.ToLower().StartsWith("2241"))
            //    mmsg.From = new MailAddress("acctactive@duoyi.com", "多益网络");
            mmsg.Subject = Subject;
            mmsg.Body = Message;
            mmsg.BodyEncoding = Encoding.UTF8;
            //mmsg.BodyEncoding = Encoding.GetEncoding("gb2312");
            mmsg.To.Add(new MailAddress(ToMail, ToName));
            mmsg.IsBodyHtml = Html;
            mail.Send(mmsg);
            */

            return true;
        }

        public static bool 使用备用发件服务器 = false;

        public static bool Func_SendMail_SendServer(int port, string fromName, string fromMail, string ToName, string ToMail, string Subject, string Message, bool Html)
        {
            return sendemailWithcentralCommominterface(fromName, fromMail, ToMail, ToName, Subject, Message, Html);
        }

        public static bool sendemailWithcentralCommominterface(string fromname, string fromMail, string tomail, string toname, string subject, string message, bool html)
        {
            try
            {
                object[] parms = { "passport", "8890a290fec9437283238fff4e653613", "fromname", fromname, "frommail", fromMail, "tomail", tomail, "toname", toname, "subject", subject, "message", message, "html", html };
                //string sign = ParamSignif2016("#eRV$ed%@3l4", parms);

                //CIAC ciac = new CIAC("fromName", fromname, "fromMail", fromMail, "toMail", tomail, "toName", toname, "subject", subject, "message", message, "html", html, "sign", sign);
                //APIReturn ar = API.InvokeMethod("projliteif2016", "if2016centralcommoninterface", "APIF_if2016_SendMail_FromHenhaoji", ciac);

                //ReturnMsg retMsg = EMail.SendEMail(tomail, toname, fromMail, fromname, subject, message, html, sign);

                var EmailClient = MailHenhaojiClient.SendMail("dc2016", fromname, fromMail, toname, tomail, subject, message, html);

                //return retMsg.Code == 0;
                return EmailClient.Succeed;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private  static string ParamSignif2016(string sceretKey, params object[] param)
        {
            Dictionary<string, string> nvs = new Dictionary<string, string>();
            if (param.Length % 2 != 0) return "";
            for (int i = 0; i < param.Length; i += 2)
            {
                string key = param[i].ToString();
                string value = param[i + 1].ToString();
                nvs[key] = value;
            }
            string[] keys = new string[nvs.Keys.Count];

            nvs.Keys.CopyTo(keys, 0);
            Array.Sort(keys);
            StringBuilder sb = new StringBuilder();
            // sb.Append(sceretKey);
            foreach (string key in keys)
                sb.Append(key).Append(nvs[key]);
            sb.Append(sceretKey);

            string signature = UrsHelper.MD5(sb.ToString().ToLower());
            return signature;
        }

        #endregion

        // 检查普通验证码是否正常
        public static ReturnMsg Func_CheckSmsCode(int number, string telnum, string smscode)
        {
            string baseUrl = DC2Conf.MPFUrlHeader;
            string isProg = DC2Conf.IsProg.ToString();
            string url = $"{baseUrl}&&isprog={isProg}&action=checkcode&number={number}&telnum={telnum}&code={smscode}";
            HttpComm comm = new HttpComm();
            HttpResult httpResult = comm.ExecWebRequest(url);

            //string urltext = CLSTemplate.Transform2(urlformat, cht_data);
            //CLSInterfaceReturn cir = new CLSInterfaceReturn().initWithContentsOfURL(urltext);
            //return Operator.RetFor(cir.Succeed, cir.Message, cir.Code);

            if (httpResult.IsSucceed)
            {
                return new ReturnMsg2980(httpResult.Content);
            }
            else
            {
                throw httpResult.HttpException;
            }
        }
    }
}
