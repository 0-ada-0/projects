using Admin.Controllers.Models;
using dywebsdk.Common;
using dywebsdk.Extension;
using dywebsdk.Models;
using dywebsdk.Web;
using IF2017.Admin.Common;
using IF2017.Admin.Configs;
using IF2017.Admin.Controllers.Common;
using IF2017.Admin.Filter;
using IF2017.Common;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Net.Http;

namespace IF2017.Admin.Controllers
{
    [EnableCors("zhanmen")]
    [Route("urs")]
    [HideInDocs]
    public partial class UrsController : BaseController<UrsController>
    {
        public UrsController(ILogger<UrsController> logger) : base(logger) { }

        #region 普通邮箱帐号

        /// <summary>
        /// 注册普通帐号
        /// </summary>
        /// <param name="regModel">请求参数</param>
        /// <returns>执行结果</returns>
        [HttpPost(@"reg")]
        public IActionResult func_reg(RegModel regModel)
        {
            //if(string.IsNullOrEmpty(regModel.vcode)||string.IsNullOrEmpty(regModel.vregval))
            //    return this.FuncResult(new APIReturn(10000, $"参数错误，验证码或者验证码key错误"));
            //if (string.IsNullOrEmpty(RedisHelper.Get(regModel.vregval)) ||RedisHelper.Get(regModel.vregval).ToLower() != regModel.vcode.ToLower())
            //    return this.FuncResult(new APIReturn(10101, $"验证码错误"));
            //RedisHelper.Remove(regModel.vregval);
            string acct = regModel.email;
            string pass = regModel.pass;
            bool passisencrypt = regModel.encrypt == "True" || regModel.encrypt == "true";
            string tname = regModel.tname;
            string mobile = regModel.mobile;
            string idcard = regModel.idcard;
            string gatesrc = regModel.gatesrc;
            string pstype = regModel.pstype;
            int qq = regModel.qq;
            string ipstr = regModel.ip ?? this.IP;
            string regip = ipstr;
            bool hasip = !string.IsNullOrWhiteSpace(ipstr) && ipstr.Length > 7;
            int ip = IPToLong(ipstr);

            if (string.IsNullOrWhiteSpace(acct) || !Utilities.IsValidEmail(acct))
            {
                return this.FuncResult(new APIReturn(10302, "帐号格式错误"));
            }

            string[] acctspt = acct.Split('@');
            string domain = acctspt[1].ToLower();
            if (domain.EndsWith("2980.com") && domain != "2980.com" ||
                domain == "2925.com" ||
                acct.ToLower().IndexOf("@henhaoji.com") != -1 ||
                acct.ToLower().IndexOf("@duoyi.com") != -1)
            {
                return this.FuncResult(new APIReturn(10355, "该类邮箱不支持注册，请选用其他邮箱"));
            }
            if (!Utilities.IsValidEmail(acct))
            {
                return this.FuncResult(new APIReturn(10302, "帐号格式错误"));
            }
            if (!string.IsNullOrWhiteSpace(mobile) && mobile.Length <= 6)
            {
                return this.FuncResult(new APIReturn(10701, "手机号格式错误"));
            }
            if (regModel.qq.ToString().Length >= 5 && (qq <= 0))
            {
                return this.FuncResult(new APIReturn(10730, "QQ号格式错误"));
            }
            var dicParams = this.DicParams;
            dicParams.Add("ip", this.IP ?? string.Empty);
            dicParams.Add("gatesrc", regModel.gatesrc ?? string.Empty);
            dicParams.Add("pstype", "game");
            dicParams.Add("encrypt", passisencrypt);
            dicParams.Add("email", acct);
            dicParams.Add("pass", regModel.pass ?? string.Empty);
            dicParams.Add("lang", regModel.language ?? string.Empty);
            dicParams.Add("idcard", regModel.idcard ?? string.Empty);
            dicParams.Add("tname", tname);
            dicParams.Add("mobile", mobile ?? string.Empty);
            dicParams.Add("qq", regModel.qq);

            if (regModel.adsid > 0)
                dicParams.Add("adsid", regModel.adsid);
            if (regModel.tgaccount > 0)
                dicParams.Add("tgaccount", regModel.tgaccount);
            if (!string.IsNullOrWhiteSpace(regModel.vcode) && regModel.vcode.Length == 4)
            {
                dicParams.Add("vcode", regModel.vcode);
                dicParams.Add("vregval", regModel.vregval);
            }
            if (!string.IsNullOrEmpty(regModel.extargs))
            {
                string[] extargs = regModel.extargs.Split(',');
                if (extargs.Length % 2 != 0)
                {
                    return this.FuncResult(new APIReturn(19901, "参数不合法(extargs)"));
                }
                for (int i = 0; i < extargs.Length; i += 2)
                {
                    string fieldname = "extargs_" + extargs[i];
                    string value = extargs[i + 1];
                    dicParams.Add(fieldname, extargs[i + 1]);
                }
            }
            return this.FuncResult(DC2Invoker.InvokeHttp("reg", HttpMethod.Post, dicParams));
        }
        /// <summary>
        /// 注册普通帐号刷新验证码
        /// </summary>
        /// <param name="regip"></param>
        /// <returns></returns>
        [HttpGet(@"refresh_imgcode")]
        public IActionResult func_refresh_imgcode(string regip)
        {
            if (string.IsNullOrEmpty(regip))
            {
                regip = this.IP;
            }

            var dicParams = this.DicParams;
            dicParams.Add("regip", regip);
            return this.FuncResult(DC2Invoker.InvokeHttp("getimgcode", HttpMethod.Get, dicParams));
        }
        /// <summary>
        /// 通过手机号找回密码（普通邮箱帐号）
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="smskey">验证码key</param>
        /// <param name="smscode">短信验证码</param>
        /// <param name="newpass">新密码</param>
        /// <param name="country">国家</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_pwd_by_mobile")]
        public IActionResult func_getPwdByMobile(string account, string mobile, string smskey, string smscode, string newpass, string country)
        {
            if (string.IsNullOrEmpty(account))
            {
                return this.FuncResult(new APIReturn(10001, "缺少账号参数"));
            }
            if (!Utilities.IsValidEmail(account))
            {
                return this.FuncResult(new APIReturn(10302, "不合法的邮箱帐号格式"));
            }
            if (mobile.Length < 5)
            {
                return this.FuncResult(new APIReturn(10205, "不合法的手机号长度"));
            }
            if (string.IsNullOrEmpty(newpass) || newpass.Length != 32)
            {
                return this.FuncResult(new APIReturn(10305, "不合法的密码长度"));
            }

            if (!IFConfigReader.IsProg)
            {
                if (string.IsNullOrEmpty(smskey))
                {
                    return this.FuncResult(new APIReturn(10005, "短信验证KEY不能为空"));
                }
                if (string.IsNullOrEmpty(smscode))
                {
                    return this.FuncResult(new APIReturn(10004, "短信验证码不能为空"));
                }
                var smsCheckRet = SmsController.CheckSMS(smskey, smscode);
                if (smsCheckRet.Code != 0)
                {
                    return this.FuncResult(smsCheckRet);
                }
            }

            string[] accountext = account.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string maildomain = accountext[1];
            if (maildomain.ToLower() == "2980.com")
            {
                return this.FuncResult(new APIReturn(10350, "2980帐号请到2980邮箱重设密码"));
            }

            var dicParams = this.DicParams;
            dicParams.Add("account", account);
            dicParams.Add("mobile", mobile);
            //dicParams.Add("smscode", smscode);
            dicParams.Add("newpass", newpass);
            dicParams.Add("country", country);

            return this.FuncResult(DC2Invoker.InvokeHttp("get_pwd_by_mobile", HttpMethod.Get, dicParams));
        }

        /// <summary>
        /// 通过邮箱找回密码（普通邮箱帐号）
        /// </summary>
        /// <param name="gate">游戏代号</param>
        /// <param name="email">邮箱</param>
        /// <param name="number">帐号ID</param>
        /// <param name="lang">语言</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_pwd_by_mail")]
        public IActionResult func_getPwdByMail(string gate, string email, int number, string lang)
        {
            if (gate.Length < 2 || gate.Length > 4)
            {
                return this.FuncResult(new APIReturn(10000, "参数错误(gate)"));
            }
            if (!Utilities.IsValidEmail(email))
            {
                return this.FuncResult(new APIReturn(10302, "帐号格式错误"));
            }
            string sendEmail = email;
            if (sendEmail.EndsWith("@2980.com"))
            {
                return this.FuncResult(new APIReturn(10350, "2980帐号请到2980邮箱操作"));
            }

            string pstype = "game";
            long ip = this.IPToLong(WebHelper.GetClientRealIP(this.HttpContext));

            var dicParams = this.DicParams;
            dicParams.Add("gate", gate);
            dicParams.Add("number", number);
            dicParams.Add("email", email);
            dicParams.Add("pstype", pstype);
            dicParams.Add("lang", lang);
            dicParams.Add("ip", ip);

            return this.FuncResult(DC2Invoker.InvokeHttp("get_pwd_by_mail", HttpMethod.Get, dicParams));
        }

        #endregion

        #region 2980邮箱帐号

        /// <summary>
        /// 注册2980邮箱帐号
        /// </summary>
        /// <param name="model">请求参数</param>
        /// <returns>执行结果</returns>
        [HttpPost(@"reg2980")]
        public IActionResult func_reg2980(Reg2980Model model)
        {
            if (string.IsNullOrEmpty(model.mobile))
            {
                return this.FuncResult(new APIReturn(10008, "手机号码不能为空"));
            }
            if (string.IsNullOrEmpty(model.password))
            {
                return this.FuncResult(new APIReturn(10002, "密码不能为空"));
            }
            if (model.account == null)
            { }
            else if(model.account.EndsWith("@2980.com")||model.account.Length<6|| model.account.Length>16)
                return this.FuncResult(new APIReturn(10302, "帐号格式错误"));
            //if (string.IsNullOrEmpty(model.account))
            //{
            //    return this.FuncResult(new APIReturn(10001, "帐号不能为空"));
            //}

            if (!IFConfigReader.IsProg)
            {
                if (string.IsNullOrEmpty(model.smskey))
                {
                    return this.FuncResult(new APIReturn(10005, "短信验证KEY不能为空"));
                }
                if (string.IsNullOrEmpty(model.smscode))
                {
                    return this.FuncResult(new APIReturn(10004, "短信验证码不能为空"));
                }
                var smsCheckRet = SmsController.CheckSMS(model.smskey, model.smscode);
                if (smsCheckRet.Code != 0)
                {
                    return this.FuncResult(smsCheckRet);
                }
            }

            var dicParams = this.DicParams;
            dicParams.Add("mobile", model.mobile);
            dicParams.Add("password", model.password);
            dicParams.Add("account", model.account ?? string.Empty);
            dicParams.Add("name", model.name ?? string.Empty);
            dicParams.Add("idnumber", model.idnumber ?? string.Empty);

            return this.FuncResult(DC2Invoker.InvokeHttp("reg2980", HttpMethod.Post, dicParams));
        }

        /// <summary>
        /// 通过手机号找回密码（2980邮箱帐号）
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">旧密码MD5</param>
        /// <param name="newpwd">新密码MD5</param>
        /// <param name="ip">来源IP</param>
        /// <param name="tel">手机号码</param>
        /// <param name="key">验证码KEY</param>
        /// <param name="smscode">验证码</param>
        /// <param name="forceset">是否校验旧密码（不能与旧密码相同）</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_2980pwd_by_mobile")]
        public IActionResult func_get2980PwdByMobile(string account, string password, string newpwd, string ip, string tel, string key, string smscode, int forceset)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(newpwd) || (((newpwd.Length != 32) || string.IsNullOrEmpty(tel)) || (account.IndexOf("@") == -1)))
            {
                return this.FuncResult(new APIReturn(19901, "参数错误"));
            }
            if(!string.IsNullOrEmpty(password) && password.Length != 32)
                return this.FuncResult(new APIReturn(19901, $"参数错误{password}"));
            if (string.IsNullOrEmpty(password))
            {
                password = FuncHelper.UTF8MD5(" ");
            }
            if (forceset != 1)
                forceset = 0;
            if (!IFConfigReader.IsProg)
            {
                if (string.IsNullOrEmpty(key))
                {
                    return this.FuncResult(new APIReturn(10005, "短信验证KEY不能为空"));
                }
                if (string.IsNullOrEmpty(smscode))
                {
                    return this.FuncResult(new APIReturn(10004, "短信验证码不能为空"));
                }
                var smsCheckRet = SmsController.CheckSMS(key, smscode);
                if (smsCheckRet.Code != 0)
                {
                    return this.FuncResult(smsCheckRet);
                }
            }

            var dicParams = this.DicParams;
            dicParams.Add("account", account);
            dicParams.Add("password", password);
            dicParams.Add("newpwd", newpwd);
            dicParams.Add("ip", ip);
            dicParams.Add("tel", tel);
            dicParams.Add("forceset", forceset);

            return this.FuncResult(DC2Invoker.InvokeHttp("get_2980pwd_by_mobile", HttpMethod.Get, dicParams));
        }

        /// <summary>
        /// 通过邮箱找回密码（2980邮箱帐号）
        /// </summary>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_2980pwd_by_mail")]
        public IActionResult func_get2980PwdByMail(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                return this.FuncResult(new APIReturn(10000, "参数错误"));
            }
            account = account.ToLower();
            var dicParams = this.DicParams;
            dicParams.Add("account", account);

            return this.FuncResult(DC2Invoker.InvokeHttp("get_2980pwd_by_mail", HttpMethod.Get, dicParams));
        }

        #endregion

        #region 激活相关

        /// <summary>
        /// 激活帐号
        /// </summary>
        /// <param name="aid">帐号标识</param>
        /// <param name="ip">来源</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"active")]
        public IActionResult func_active(string aid, string ip)
        {
            string UAV_GUID = aid;
            Hashtable htb = new Hashtable();
            if (string.IsNullOrEmpty(aid) || UAV_GUID.Length != 32)
            {
                htb.Add("aid", "aid");
                return this.FuncResult(new APIReturn(10302, "帐号格式错误", htb));
            }

            var dicParams = this.DicParams;
            dicParams.Add("aid", aid);
            dicParams.Add("ip", ip);

            return this.FuncResult(DC2Invoker.InvokeHttp("active", HttpMethod.Get, dicParams));
        }

        /// <summary>
        /// 重发激活邮件
        /// </summary>
        /// <param name="gate">游戏代号</param>
        /// <param name="email">目标邮箱</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"resend_active_mail")]
        public IActionResult func_resendActiveMail(string gate, string email)
        {
            /*
             * 需要和邮件激活流程同步同步上线
             * 
            if (gate.IsNullOrEmpty())
            {
                gate = "urs";
            }
            if (email.Length < 5 || gate.Length > 4)
            {
                return this.FuncResult(new APIReturn(10000, "参数不正确"));
            }
            var dicParams = this.DicParams;
            dicParams.Add("gate", gate);
            dicParams.Add("email", email);

            return this.FuncResult(DC2Invoker.InvokeHttp("resend_active_mail", HttpMethod.Get, dicParams));
            */


            //调用dc2改到dc2016
            //var dicParams = this.DicParams;
            //dicParams.Add("action", "ava_resend");
            //dicParams.Add("passport", "c8912ed30c7f46cc8ebd783ff9cf14b0");
            //dicParams.Add("gate", gate);
            //dicParams.Add("email", email);
            //CallResult callResult = WebHttpClient.InvokeHttp("if2017", "dc2", "resend_active_mail", HttpMethod.Get, dicParams);
            //if (callResult.Code == 0)
            //{
            //    int code = int.Parse(callResult.Message.Split('|')[0]);
            //    if (code == 0)
            //    {
            //        return this.FuncResult(new APIReturn(0));
            //    }
            //    else if (code == 20)
            //    {
            //        return this.FuncResult(new APIReturn(10000, "参数不正确"));
            //    }
            //    else if (code == 23)
            //    {
            //        return this.FuncResult(new APIReturn(10353, "帐号已激活或72小时内未注册"));
            //    }
            //    else if (code == 102)
            //    {
            //        return this.FuncResult(new APIReturn(10354, "超过48小时未激活"));
            //    }
            //    else
            //    {
            //        return this.FuncResult(new APIReturn(10501, $"重发激活邮件失败(dc2,{callResult.Message})"));
            //    }
            //}
            //else
            //{
            //    throw new Exception($"请求DC2发生异常：{callResult.Message}");
            //}
            var dicParams = this.DicParams;
            dicParams.Add("gate", gate);
            dicParams.Add("email", email);
            return this.FuncResult(DC2Invoker.InvokeHttp("resend_active_mail", HttpMethod.Get, dicParams));

        }

        #endregion

        #region 找回帐号

        /// <summary>
        /// 通过Mac地址找回帐号
        /// </summary>
        /// <param name="gate">游戏代号</param>
        /// <param name="mac">max地址</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_acct_by_mac")]
        public IActionResult func_getAcctByMac(string gate, string mac)
        {
            if (string.IsNullOrEmpty(gate) || string.IsNullOrEmpty(mac))
            {
                return this.FuncResult(new APIReturn(10000, "参数错误"));
            }
            mac = mac.Replace('-','\0');
            mac = mac.Replace('_', '\0');
            var dicParams = this.DicParams;
            dicParams.Add("gate", gate);
            dicParams.Add("mac", mac);

            return this.FuncResult(DC2Invoker.InvokeHttp("get_acct_by_mac", HttpMethod.Get, dicParams));
        }

        /// <summary>
        /// 通过手机号找回帐号
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <param name="smscode">短信验证码</param>
        /// <param name="smskey">短信验证码返回的KEY</param>
        /// <param name="isvalid">是否检查短信验证码</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_acct_by_mobile")]
        public IActionResult func_getAcctByMobile(string mobile, string smscode, string smskey, int isvalid)
        {
            bool validatecode = isvalid == 0;//是否检查短信验证码
            //int number = (Math.Abs(mobile.GetHashCode()) > 20000000 ? Math.Abs(mobile.GetHashCode()) : 200000000 + Math.Abs(mobile.GetHashCode()));
            if (mobile.IsNullOrEmpty() || mobile.Length < 7)
            {
                return this.FuncResult(new APIReturn(10205, "手机号格式错误"));
            }

            if (!IFConfigReader.IsProg || validatecode)
            {
                if (string.IsNullOrEmpty(smskey))
                {
                    return this.FuncResult(new APIReturn(10005, "短信验证KEY不能为空"));
                }
                if (string.IsNullOrEmpty(smscode))
                {
                    return this.FuncResult(new APIReturn(10004, "短信验证码不能为空"));
                }
                var smsCheckRet = SmsController.CheckSMS(smskey, smscode);
                if (smsCheckRet.Code != 0)
                {
                    return this.FuncResult(smsCheckRet);
                }
            }

            var dicParams = this.DicParams;
            dicParams.Add("mobile", mobile);
            return this.FuncResult(DC2Invoker.InvokeHttp("get_acct_by_mobile", HttpMethod.Get, dicParams));
        }

        #endregion

        #region 账号判断相关

        /// <summary>
        /// 检查战盟账号是否存在
        /// </summary>
        /// <param name="account">战盟账号</param>
        /// <param name="checkactive">是否验证账号已激活</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"isexist_acct")]
        public IActionResult func_IsExistAcct(string account, int checkactive)
        {
            if (account.IsNullOrEmpty())
            {
                return this.FuncResult(new APIReturn(10000, "参数错误(account)"));
            }
            if (FuncHelper.IsMobile(account))
            { }
            else if(!account.Contains("@") || !account.EndsWith(".com"))
            {
                return this.FuncResult(new APIReturn(10000, "参数错误(account)"));
            }
            var dicParams = this.DicParams;
            dicParams.Add("account", account);
            dicParams.Add("checkactive", checkactive);
            return this.FuncResult(DC2Invoker.InvokeHttp("isexist_acct", HttpMethod.Get, dicParams));
        }
        /// <summary>
        /// 验证手机号码是否是绑定的手机号
        /// </summary>
        /// <param name="account"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        [HttpGet(@"valid_mobile")]
        public IActionResult func_valid_mobile(string account, string mobile)
        {
            if (account.IsNullOrEmpty() || !FuncHelper.IsMobile(account) && !Utilities.IsValidEmail(account))
            {
                return this.FuncResult(new APIReturn(10000, "参数错误(account)"));
            }
            if (string.IsNullOrEmpty(mobile) || mobile.Length < 7)
                return this.FuncResult(new APIReturn(10000, "参数错误(mobile)"));
            var dicParams = this.DicParams;
            dicParams.Add("account", account);
            dicParams.Add("mobile", mobile);
            return this.FuncResult(DC2Invoker.InvokeHttp("valid_mobile", HttpMethod.Get, dicParams));
        }
        #endregion

        #region 绑定，关联手机相关

        /// <summary>
        /// 关联手机
        /// </summary>
        /// <param name="email">邮箱账号（must）</param>
        /// <param name="mobile">手机账号（must）</param>
        /// <param name="smskey">获取短信验证码返回的key（must）</param>
        /// <param name="smscode">短信验证码（must）</param>
        /// <returns></returns>
        [HttpGet(@"bindassociationmobile")]
        public IActionResult func_bindassociationmobile([FromQuery]string email, [FromQuery]string mobile, [FromQuery]string smskey, [FromQuery]string smscode)
        {
            if (email.IsNullOrEmpty() || !email.Contains("@") || !email.EndsWith(".com"))
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误({email})"));
            }
            if (mobile.IsNullOrEmpty() || mobile.Length < 7)
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误({mobile})"));
            }
            if (!IFConfigReader.IsProg)
            {
                if (string.IsNullOrEmpty(smskey))
                {
                    return this.FuncResult(new APIReturn(10005, "短信验证KEY不能为空"));
                }
                if (string.IsNullOrEmpty(smscode))
                {
                    return this.FuncResult(new APIReturn(10004, "短信验证码不能为空"));
                }
                var smsCheckRet = SmsController.CheckSMS(smskey, smscode);
                if (smsCheckRet.Code != 0)
                {
                    return this.FuncResult(smsCheckRet);
                }
            }
            var dicParams = this.DicParams;
            dicParams.Add("email", email);
            dicParams.Add("mobile", mobile);
            return this.FuncResult(DC2Invoker.InvokeHttp("bindassociationmobile", HttpMethod.Get, dicParams));
        }

        #endregion
        #region 获取帐号信息
        [HttpGet(@"getacctinfo")]
        public IActionResult func_getacctinfo(string vcode,string vregval, string gate, string account)
        {
            if (string.IsNullOrEmpty(vcode) || string.IsNullOrEmpty(vregval))
                return this.FuncResult(new APIReturn(10000, $"参数错误，验证码或者验证码key错误"));
            if (string.IsNullOrEmpty(RedisHelper.Get(vregval)) || RedisHelper.Get(vregval).ToLower() != vcode.ToLower())
                return this.FuncResult(new APIReturn(10101, $"验证码错误"));
            RedisHelper.Remove(vregval);
            if (string.IsNullOrEmpty(gate) || string.IsNullOrEmpty(account))
            {
                return this.FuncResult(new APIReturn(10000, "参数错误"));
            }
            if (account.Length < 5) return this.FuncResult(new APIReturn(10000, "account error"));
            string ip = getClientIP();
            var dicParams = this.DicParams;
            dicParams.Add("gate", gate);
            dicParams.Add("account", account);
            dicParams.Add("ip", ip);
            return this.FuncResult(DC2Invoker.InvokeHttp("getacctinfo", HttpMethod.Get, dicParams));
        }
        #endregion
    }
}
