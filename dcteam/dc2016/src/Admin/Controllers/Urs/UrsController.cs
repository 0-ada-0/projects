using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using DC2016.BLL;
using DC2016.Model;
using Microsoft.Extensions.Options;
using DC2016.Admin.Controllers.Common;
using DC2016.Admin.Controllers.Urs.Models;
using DC2016.Admin.Extensions;
using System.Text;
using DC2016.Admin.DC2;
using DC2016.Admin.DC2.Lib;
using System.Text.RegularExpressions;
using DC2016.BLL.Enums;
using DC2016.Admin.Configs;
using DC2016.Admin.Controllers.Base;
using DC2016.Admin.Enums;
using DC2016.Admin.DC;
using DC2016.Admin.Common;
using dywebsdk.Extension;
using dywebsdk.Web;
using dywebsdk.Common;
using dywebsdk.Models;
using System.Net.Http;
using DC2016.Admin.P2980;
using DC2016.Admin.Filter;
using Newtonsoft.Json.Linq;

namespace DC2016.Admin.Controllers.Urs
{
    [Route("dc2/urs")]
    public partial class UrsController : UrsBaseController<UrsController>
    {
        private ViewRenderService _viewRender;

        public UrsController(ILogger<UrsController> logger, ViewRenderService viewRender) : base(logger)
        {
            this._viewRender = viewRender;
        }

        #region 普通邮箱帐号

        /// <summary>
        /// 注册普通帐号
        /// </summary>
        /// <param name="regModel">请求参数</param>
        /// <returns>执行结果</returns>
        [HttpPost(@"reg")]
        public IActionResult func_reg(RegModel regModel)
        {
            string acct = regModel.email;
            string pass = regModel.pass ?? string.Empty;
            string encrypt = regModel.encrypt ?? string.Empty;
            bool passisencrypt = regModel.encrypt == "True" || regModel.encrypt == "true";
            string tname = regModel.tname ?? string.Empty;
            string mobile = regModel.mobile ?? string.Empty;
            string idcard = regModel.idcard ?? string.Empty;
            string gatesrc = regModel.gatesrc ?? "urs";
            string pstype = regModel.pstype ?? "game";
            int qq = regModel.qq;
            string ipstr = regModel.ip ?? string.Empty;
            string regip = ipstr;
            bool hasip = !string.IsNullOrWhiteSpace(ipstr) && ipstr.Length > 7;
            int ip = ip2long(ipstr);

            string ipkey = string.Format("check-reg-ip-{0}", ip);
            object obj = UrsHelper.Cache_GetObj(ipkey);
            List<DateTime> li_ipreglist = new List<DateTime>();
            if (obj != null && obj is List<DateTime>)
            {
                li_ipreglist = (List<DateTime>)obj;
                int regcount = 0;
                for (int i = li_ipreglist.Count - 1; i >= 0; i--)
                {
                    DateTime d = li_ipreglist[i];
                    if (Math.Abs((DateTime.Now - d).TotalSeconds) > 900)
                        li_ipreglist.RemoveAt(i);
                    else
                        regcount++;
                }
                if (regcount >= 101)//放得较宽，以防如3G网之类的可能有问题
                {
                    this.FuncResult(new APIReturn(10320, "注册频繁"));
                }
            }

            string vcode = regModel.vcode;
            if (regModel.ismodenopass)
            {
                pass = "0ab7dea79b1de3d0816f8b5cbbb36367";
                passisencrypt = true;
            }
            string passmd5 = passisencrypt ? pass : UrsHelper.MD5(pass);
            if (passmd5 == "0ab7dea79b1de3d0816f8b5cbbb36367")
            {
                //Operator.CHT_ExtData["ismodenopass"] = true;
                regModel.ismodenopass = true;
            }

            WebParams webParans = new WebParams(this.Request);
            var dicValues = webParans.Where(e => e.Key.StartsWith("extarg_")).ToDictionary(e => e.Key, e => e.Value);
            Hashtable cht_args = new Hashtable(dicValues);

            if (qq <= 0) qq = 0;
            string friendname = UrsHelper.Lib_Friend_GetName(acct);
            if (friendname.Length > 0)
            {
                this.FuncResult(new APIReturn(10350, string.Format("{0}帐号不能在这里进行注册操作", friendname)));
            }

            string[] acctspt = acct.Split('@');
            string domain = acctspt[1].ToLower();
            if (domain.EndsWith("2980.com") && domain != "2980.com"
                || domain == "2925.com"
                || acct.ToLower().IndexOf("@henhaoji.com") != -1
                || acct.ToLower().IndexOf("@duoyi.com") != -1)
            {
                this.FuncResult(new APIReturn(10355, "该类邮箱不支持注册，请选用其他邮箱"));
            }
            if (friendname != string.Empty)
            {
                this.FuncResult(new APIReturn(10326, string.Format("{0}帐号无需注册，可直接登录", friendname)));
            }
            if (!isemail(acct))
            {
                this.FuncResult(new APIReturn(10302, "帐号格式错误"));
            }
            if (!isvpass(pass, passisencrypt))
            {
                this.FuncResult(new APIReturn(10305, "密码格式错误"));
            }
            if (!string.IsNullOrWhiteSpace(idcard) && idcard.Length > 0 && !isidcard(idcard))
            {
                this.FuncResult(new APIReturn(10711, "身份证格式错误"));
            }
            if (!string.IsNullOrWhiteSpace(tname) && tname.Length > 0 && !isvname(tname))
            {
                this.FuncResult(new APIReturn(10720, "姓名格式错误"));
            }
            if (!string.IsNullOrWhiteSpace(mobile) && mobile.Length >= 1 && !IsMobile(mobile))
            {
                this.FuncResult(new APIReturn(10701, "手机号格式错误"));
            }
            if (regModel.qq.ToString().Length >= 5 && (qq <= 0))
            {
                this.FuncResult(new APIReturn(10730, "QQ号格式错误"));
            }
            if (passisencrypt)
            {
                if(pass.Length!=32)
                    this.FuncResult(new APIReturn(10000,$"参数错误pass={pass}"));
                pass = UrsHelper.MD5(pass.Substring(7, 16)).Substring(3, 16);
            }
            return func_exec_reg(regModel, false, ip, acct, pass, passisencrypt, tname, idcard, mobile, qq, gatesrc, pstype, cht_args.ToJson(), regip);
        }

        /// <summary>
        /// 通过手机号找回密码（普通邮箱帐号）
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="mobile">手机号码</param>
        /// <param name="newpass">新密码</param>
        /// <param name="country">国家</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_pwd_by_mobile")]
        public IActionResult func_getPwdByMobile(string account, string mobile, string newpass, string country)
        {
            if (string.IsNullOrWhiteSpace(mobile) || mobile.Length < 5)
            {
                return this.FuncResult(new APIReturn(10205, "不合法的手机号长度"));
            }

            int number = (Math.Abs(mobile.GetHashCode()) > 20000000 ? Math.Abs(mobile.GetHashCode()) : 200000000 + Math.Abs(mobile.GetHashCode()));
            string ipstr = this.IP;
            int ip = 0;
            if (!string.IsNullOrEmpty(ipstr))
            {
                ip = this.ip2long(ipstr);
            }

            if (string.IsNullOrWhiteSpace(account) || !isemail(account))
            {
                return this.FuncResult(new APIReturn(10302, "不合法的邮箱帐号格式"));
            }
            if (string.IsNullOrWhiteSpace(newpass) || newpass.Length != 32)
            {
                return this.FuncResult(new APIReturn(10305, "不合法的密码长度"));
            }

            string[] accountext = account.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string maildomain = accountext[1];
            if (maildomain.ToLower() == "2980.com")
            {
                return this.FuncResult(new APIReturn(10350, "2980帐号请到2980邮箱重设密码"));
            }

            newpass = UrsHelper.mymd5(newpass);

            List<object> listobj = new List<object>();
            listobj.AddRange(new object[] { "ip", ip, "number", account, "mobile", mobile, "newpass", newpass, "country", country ?? string.Empty });
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.W, "acct_setpass_mobile", listobj.ToArray());
            DicDCValue retMsg = dcr.GetDicDCValue();

            if (retMsg.Code == 0 || retMsg.Code == 54)
            {
                Hashtable ht = new Hashtable();
                ht.Add("email",account);
                //重置成功
                return this.FuncResult(new APIReturn(0, "", ht));
            }
            else if (retMsg.Code == 68)
            {
                return this.FuncResult(new APIReturn(10351, "手机号与通行证已关联但未生效"));
            }
            else if (retMsg.Code == 69)
            {
                return this.FuncResult(new APIReturn(10351, "手机号与帐号不匹配"));
            }
            else if (retMsg.Code == 151)
            {
                return this.FuncResult(new APIReturn(10350, "2980帐号请到2980邮箱重设密码"));
            }
            else
            {
                return this.FuncResult(new APIReturn(19804, $"重设密码失败({retMsg.Code}"));
            }
        }

        /// <summary>
        /// 通过邮箱找回密码（普通邮箱帐号）
        /// </summary>
        /// <param name="gate">游戏代号</param>
        /// <param name="number">帐号ID</param>
        /// <param name="email">邮箱</param>
        /// <param name="pstype">类型</param>
        /// <param name="lang">语言</param>
        /// <param name="ip">来源IP</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_pwd_by_mail")]
        public IActionResult func_getPwdByMail(string gate, int number, string email, string pstype, string lang, int ip)
        {
            if (!DC2Conf.IsProg && !this.IsVIP())
            {
                return this.FuncResult(new APIReturn(-97, "IP没有访问权限"));
            }

            if (gate.IsNullOrEmpty())
            {
                return this.FuncResult(new APIReturn(10000, "参数错误(gate)"));
            }
            if (email.IsNullOrEmpty() || !Utilities.IsValidEmail(email))
            {
                return this.FuncResult(new APIReturn(10302, "帐号格式错误"));
            }

            string sendEmail = email;
            if (sendEmail.EndsWith("@2980.com"))
            {
                return this.FuncResult(new APIReturn(10350, "2980帐号请到2980邮箱操作"));
            }
            if (number < 10000)
            {
                number = getnumberbyemail(email);
            }

            string[] mailfixs = new string[] { "yahoo.com.cn", "yahoo.cn" };
            if (Array.IndexOf<string>(mailfixs, sendEmail.Split('@')[1]) != -1)
            {
                DCClass dcc_urs = new DCClass();
                if (number < 20000000)//将升级到多益通帐号的number转成多益通number
                {
                    number = dcc_urs.URS_GetURSNumberByNumber(number);
                }
                dcc_urs = new DCClass();
                string bindMail = dcc_urs.URS_GetBindMail(number);
                if (!string.IsNullOrEmpty(bindMail) && bindMail.Length > 5)
                {
                    sendEmail = bindMail;
                }
            }

            bool tooMuch = D2getpass.CheckLast(email, 0, 1);
            if (tooMuch)
            {
                return this.FuncResult(new APIReturn(10323, "找回密码太过频繁"));
            }

            EGTP egtpType = EGTP.邮箱找回密码;
            if (pstype == "web")//默认游戏
            {
                egtpType = EGTP.邮箱找回密码_网站;
            }
            D2getpassInfo passInfo = new D2getpassInfo()
            {
                GtpsGUID = Guid.NewGuid().ToString("N"),
                GtpsIP = ip,
                GtpsState = (int)EGST.未发送,
                GtpsType = (int)egtpType,
                GtpsGate = gate,
                GtpsNumber = number,
                GtpsEMail = sendEmail,
                GtpsTime1 = DateTime.Now
            };
            D2getpass.Insert(passInfo);

            bool rc = D2Game.Func_SendMail(passInfo, lang, _viewRender);
            if (rc)
            {
                Hashtable ht = new Hashtable();
                ht.Add("email", email);
                //重置成功
                return this.FuncResult(new APIReturn(0, "", ht));
            }
            else
            {
                return this.FuncResult(new APIReturn(10502, "找回密码邮件发送失败"));
            }
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

            //object[] objs = new object[] { "act", "dy_2980registeruser", "portkey", DC2Conf.Passport2980, "phone", model.mobile, "password", model.password, "accort", model.account, "name", model.name };
            Dictionary<string, object> argus = new Dictionary<string, object>();
            argus.Add("act", "dy_2980registeruser");
            argus.Add("portkey", DC2Conf.Passport2980);
            argus.Add("phone", model.mobile);
            argus.Add("password", model.password);
            argus.Add("accort", model.account ?? string.Empty);
            argus.Add("name", model.name ?? string.Empty);

            ReturnMsg2980 ret = P2980Invoker.InvokeHttp("funswregister", argus);

            #region 返回码处理

            int code;
            string msg;

            if (ret.Code == 250)
            {
                //注册成功
                return this.FuncResult(new APIReturn(0));
            }
            else if (ret.Code == 609)
            {
                //参数不能为空（格式不对）
                code = 10000;
                msg = "部分参数缺失";
            }
            else if (ret.Code == 608)
            {
                //手机号或帐号不存在（格式不对）
                code = 10300;
                msg = "帐号不存在";
            }
            else if (ret.Code == 625)
            {
                //手机号或帐号存在
                code = 10303;
                msg = "帐号已存在";
            }
            else if (ret.Code == 624)
            {
                //请按要求填写帐号
                code = 10302;
                msg = "帐号格式错误";
            }
            else if (ret.Code == 626)
            {
                //对象为空，用户注册失败
                code = 10306;
                msg = "注册失败（对象为空）";
            }
            else if (ret.Code == 627)
            {
                //注册失败
                code = 10306;
                msg = "注册失败";
            }
            else if (ret.Code == 631)
            {
                //通行证出错
                code = -98;
                msg = "通行证出错";
            }
            else if (ret.Code == 632)
            {
                //通行证连接超时
                code = -95;
                msg = "网络异常（超时）";
            }
            else if (ret.Code == 633)
            {
                //帐号已存在多益通行证
                code = 10303;
                msg = "帐号已存在";
            }
            else if (ret.Code == 634)
            {
                //手机已存在多益通行证
                code = 10303;
                msg = "帐号已存在（手机）";
            }
            else if (ret.Code == 635)
            {
                //身份证号格式不对
                code = 10711;
                msg = "身份证号格式错误";
            }
            else if (ret.Code == 636)
            {
                //密码长度不对，6~16个字符（字母、数字）
                code = 10305;
                msg = "密码格式错误";
            }
            else if (ret.Code == 637)
            {
                //密码格式不对
                code = 10305;
                msg = "密码格式错误";
            }
            else if (ret.Code == 500)
            {
                //服务器出错
                code = -98;
                msg = "未知系统错误";
            }
            else if (ret.Code == 614)
            {
                //你不具备访问权限
                code = -97;
                msg = "IP不具备访问权限";
            }
            else if (ret.Code == 283)
            {
                //无任何执行动作
                code = 19900;
                msg = "未知错误";
            }
            else
            {
                code = 19801;
                msg = $"注册失败({ret.Code},{ret.Message})";
            }

            #endregion

            return this.FuncResult(new APIReturn(code, msg));
        }

        /// <summary>
        /// 通过手机号找回密码（2980邮箱帐号）
        /// </summary>
        /// <param name="account">帐号</param>
        /// <param name="password">旧密码MD5</param>
        /// <param name="newpwd">新密码MD5</param>
        /// <param name="ip">来源IP</param>
        /// <param name="tel">手机号码</param>
        /// <param name="forceset">是否校验旧密码（不能与旧密码相同）</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_2980pwd_by_mobile")]
        public IActionResult func_get2980PwdByMobile(string account, string password, string newpwd, string ip, string tel, string forceset)
        {
            if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(newpwd) || (((newpwd.Length != 32) || string.IsNullOrEmpty(tel)) || (account.IndexOf("@") == -1)))
            {
                return this.FuncResult(new APIReturn(19901, "参数错误"));
            }
            if (string.IsNullOrEmpty(ip))
            {
                return this.FuncResult(new APIReturn(19901, "参数错误(ip)"));
            }

            object[] objs = { "gatesrc", "urs", "email", account };
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "acct_getnumberbyemail", objs.ToArray());
            DicDCValue rmdc = dcr.GetDicDCValue();

            int number = 0;
            if (rmdc.Code == 0)
            {
                number = rmdc["number"].ToInt();
            }
            else if(rmdc.Code==23)
            {
                return this.FuncResult(new APIReturn(10300, $"帐号不存在"));
            }
            else
                return this.FuncResult(new APIReturn(10324, $"获取帐号信息失败({rmdc.Code})"));

            objs = new object[] { "action", 1401, "gate", "urs", "number", number };
            dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "shell_websys_r", objs.ToArray());
            rmdc = dcr.GetDicDCValue();

            string ano = string.Empty;
            string vmobile = string.Empty;
            if (rmdc.Code == 0)
            {
                ano = rmdc["ano"];
                vmobile = rmdc["vmobile"];
            }
            else
            {
                return this.FuncResult(new APIReturn(10324, $"获取帐号信息失败({rmdc.Code},{rmdc["Message"]})"));
            }

            objs = new object[] { "ip", ip2long(ip), "newpassmd5", newpwd, "passmd5", password, "ano", ano, "forceset", forceset };
            dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.W, "2980_acct_setpass", objs.ToArray());
            rmdc = dcr.GetDicDCValue();

            if (rmdc.Code == 0)
            {
                //成功
                return this.FuncResult(new APIReturn(0));
            }
            else if (rmdc.Code == 54)
            {
                //新旧密码相同
                return this.FuncResult(new APIReturn(10327, "不能修改为原有密码"));
            }
            else if (rmdc.Code == 20)
            {
                //参数错误
                return this.FuncResult(new APIReturn(10000, "参数错误(20)"));
            }
            else
            {
                return this.FuncResult(new APIReturn(19804, $"找回密码失败({rmdc.Code})"));
            }
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

            SignMaker signMaker = new SignMaker();
            Dictionary<string, Object> dicParams = new Dictionary<string, object>();
            dicParams.Add("act", "dy_2980gamesendqqmail");
            dicParams.Add("mail", account);
            dicParams.Add("portkey", DC2Conf.Passport2980);
            string sign = signMaker.MakeSign(dicParams);

            Dictionary<string, object> argus = new Dictionary<string, object>();
            argus.Add("act", "dy_2980gamesendqqmail");
            argus.Add("portkey", DC2Conf.Passport2980);
            argus.Add("mail", account);
            argus.Add("sig", sign);

            ReturnMsg2980 ret = P2980Invoker.InvokeHttp("funswregister", argus);

            #region 返回码处理

            int code;
            string msg;
            if (ret.Code == 250)
            {
                code = 0;
                msg = string.Empty;
            }
            else if (ret.Code == 201)
            {
                //用户名格式不正确
                code = 10302;
                msg = "帐号格式错误";
            }
            else if (ret.Code == 202)
            {
                //用户不存在
                code = 10300;
                msg = "帐号不存在";
            }
            else if (ret.Code == 203)
            {
                //该邮箱未绑定QQ
                code = 10520;
                msg = "邮箱未绑定qq";
            }
            else if (ret.Code == 204)
            {
                //QQ找回密码发送失败
                code = 10502;
                msg = "找回密码邮件发送失败";
            }
            else if (ret.Code == 500)
            {
                //服务器内部错误
                code = -98;
                msg = "未知系统错误";
            }
            else
            {
                code = 19801;
                msg = $"找回密码失败({ret.Code},{ret.Message})";
            }

            #endregion

            return this.FuncResult(new APIReturn(code, msg));
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
            string uavGUID = aid;
            Hashtable htb = new Hashtable();
            if (uavGUID.IsNullOrEmpty() || uavGUID.Length != 32)
            {
                htb.Add("gate", "sw");
                return this.FuncResult(new APIReturn(10302, "帐号格式错误", htb));
            }
            D2unactiveInfo gua = D2unactive.GetItem(uavGUID);
            if (gua == null)
            {
                htb.Add("gate", "sw");
                return this.FuncResult(new APIReturn(10300, "帐号不存在", htb));
            }
            TimeSpan ts1 = DateTime.Now - gua.UavTime1.Value;
            if (ts1.TotalHours >= 48)
            {
                htb.Add("gate", gua.UavGate);
                return this.FuncResult(new APIReturn(10354, "超过48小时未激活", htb));
            }

            string gateSrc;
            bool rc = this.Func_SetAV2(gua, out gateSrc);
            string gatesrcurl = string.Empty;
            try
            {
                if (rc && !string.IsNullOrEmpty(gateSrc))
                {
                    string gatesrc = gateSrc;
                    gatesrcurl = DC2Conf.GetGateSrcUrl(gateSrc);
                    if (gatesrc == "025")
                    {
                        gatesrcurl += (gatesrcurl.IndexOf("?") == -1 ? "?" : "&") + string.Format("{0}={1}&tid={2}", "email", gua.UavEMail, gua.UavGUID);
                    }
                    gatesrcurl = gatesrcurl.Replace("=", "%3d");
                    gatesrcurl = gatesrcurl.Replace("&", "%26");
                }
            }
            catch (Exception e)
            {
                gatesrcurl = null;
            }
            htb.Add("gate", gua.UavGate);
            htb.Add("gatesrcurl", gatesrcurl);
            if (rc)
            {
                return this.FuncResult(new APIReturn(0, string.Empty, htb));
            }
            else
            {
                return this.FuncResult(new APIReturn(0, "激活帐号失败", htb));
            }
        }

        /// <summary>
        /// 从DC获取未激活的帐号（定时器调用）
        /// </summary>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_unactive_list")]
        public IActionResult func_getUnActiveList()
        {
            string gate = "urs";
            Hashtable htbData = new Hashtable();
            if (!(this.IP == "127.0.0.1" || this.IP == "::1"))
            {
                htbData.Add("IP", this.IP);
                return this.FuncResult(new APIReturn(-97, "没有权限", htbData));
            }
            if (string.IsNullOrEmpty(gate))
            {
                return this.FuncResult(new APIReturn(10000, "参数错误(gate)", htbData));
            }
            checkpara_string(gate, 1, 8);

            StringBuilder sbout = new StringBuilder();
            int rc = Job.UnActive_GetList(gate, sbout);
            htbData.Add("rc", rc);

            return this.FuncResult(new APIReturn(0, sbout.ToString(), htbData));
        }

        /// <summary>
        /// 发送帐号激活邮件（定时器调用）
        /// </summary>
        /// <returns>执行结果</returns>
        [HttpGet(@"send_active_email")]
        public IActionResult func_sendActiveEmail()
        {
            string gate = "urs";

            Hashtable htbData = new Hashtable();
            if (!(this.IP == "127.0.0.1" || this.IP == "::1"))
            {
                htbData.Add("IP", this.IP);
                return this.FuncResult(new APIReturn(-97, "没有权限", htbData));
            }
            checkpara_string(gate, 1, 8);

            int countFailed = 0;
            int countSucceed = Job.UnActive_SendMail(gate, _viewRender, ref countFailed);

            htbData.Add("countSucceed", countSucceed);
            htbData.Add("countFailed", countFailed);

            return this.FuncResult(new APIReturn(0, string.Empty, htbData));
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
            if(string.IsNullOrEmpty(gate)||string.IsNullOrEmpty(email))
                return this.FuncResult(new APIReturn(10000, $"参数不正确gate={gate},email={email}"));
            if (email.Length < 5 || gate.Length > 4)
            {
                return this.FuncResult(new APIReturn(10000, $"参数不正确gate={gate},email={email}"));
            }

            List<D2unactiveInfo> scc = D2unactive.GetByEmail(gate, email);
            if (scc.Count() == 0)
            {
                //没有可供重发的信息
                return this.FuncResult(new APIReturn(10353, "帐号已激活或72小时内未注册"));
            }

            D2unactiveInfo gua = (D2unactiveInfo)scc[0];
            TimeSpan ts = DateTime.Now - gua.UavTime1.Value;
            if (ts.TotalHours >= 48)
            {
                return this.FuncResult(new APIReturn(10354, "超过48小时未激活"));
            }
            bool rc = Job.Func_SendMail(gua, _viewRender);
            if (rc)
            {
                return this.FuncResult(new APIReturn(0));
            }
            else
            {
                //发送激活邮件失败
                return this.FuncResult(new APIReturn(10501, "重发激活邮件失败"));
            }
        }

        #endregion

        #region 找回帐号

        /// <summary>
        /// 通过MAC地址找回帐号
        /// </summary>
        /// <param name="gate">游戏代号</param>
        /// <param name="mac">MAC地址</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_acct_by_mac")]
        public IActionResult func_getAcctByMac(string gate, string mac)
        {
            if (string.IsNullOrEmpty(gate) || string.IsNullOrEmpty(mac))
            {
                return this.FuncResult(new APIReturn(10000, "参数错误( gate or mac)"));
            }
            int counts = -1;
            if (counts < 0)
            {
                counts = 10;
            }

            int action = gate == "mx" ? 1 : 28;

            DCResult dcr = null;
            DicDCValue rmdc = null;
            object[] objs = { "action", action, "mac", mac, "gate", gate, "counts", counts, "retd2", "1" };
            if (gate == "sw")
            {
                //dcr = new DCRequest(DCProdTypes.SW, DCMethodTypes.W);
                //rmdc = dcr.ExecRequest("shell_cs.php", objs);
                dcr = DCInvoker.HttpInvoke(DCProdTypes.SW, DCMethodTypes.W, "shell_cs", objs.ToArray());
                rmdc = dcr.GetDicDCValue();
            }
            else if (gate == "mx")
            {
                //dcr = new DCRequest(DCProdTypes.MX, DCMethodTypes.R);
                //rmdc = dcr.ExecRequest("acct_getbymac.php", objs);
                dcr = DCInvoker.HttpInvoke(DCProdTypes.MX, DCMethodTypes.R, "acct_getbymac", objs.ToArray());
                rmdc = dcr.GetDicDCValue();
            }
            else
            {
                //dcr = new DCRequest(DCProdTypes.URS, DCMethodTypes.R);
                //rmdc = dcr.ExecRequest("shell_websys_user_r.php", objs);
                dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "shell_websys_user_r", objs.ToArray());
                rmdc = dcr.GetDicDCValue();
            }
            if (rmdc.Code == 0 && rmdc.Content.Length > 2)
            {
                Hashtable ht = new Hashtable();
                string[] data = rmdc.Content.Substring(2).Split('&');
                if(data.Length!=3&&data[0]=="0")
                    return this.FuncResult(new APIReturn(10300, " 帐号不存在"));
                for (int i=0;i<data.Length;i++)
                {
                    if (i == 0)
                    {
                        string[] count = data[i].Split('=');
                        ht[count[0]] = count[1];
                    }
                    if (i == 1)
                    {
                        string[] acct = data[i].Split('=');
                        ht[acct[0]] = acct[1].Split(',');
                    }
                    if (i == 2)
                    {
                        string[] logtimes = data[i].Split('=');
                        ht[logtimes[0]] = logtimes[1].Split(',');
                    }
                }
                return this.FuncResult(new APIReturn(0, "",ht));
            }
            return this.FuncResult(new APIReturn(19804, $"找回帐号失败({rmdc.Code})"));
        }

        /// <summary>
        /// 通过手机号找回帐号
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"get_acct_by_mobile")]
        public IActionResult func_getAcctByMobile(string mobile)
        {
            //int number = (Math.Abs(mobile.GetHashCode()) > 20000000 ? Math.Abs(mobile.GetHashCode()) : 200000000 + Math.Abs(mobile.GetHashCode()));
            if (mobile.IsNullOrEmpty() || !this.IsMobile(mobile))
            {
                return this.FuncResult(new APIReturn(10205, "手机号格式错误"));
            }

            object[] objs = { "action", "1406", "mobile", mobile };
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "shell_websys_r", objs.ToArray());
            DicDCValue rmdc = dcr.GetDicDCValue();

            List<string> li_rc = new List<string>();
            Hashtable htb = new Hashtable();
            if (rmdc.Code == 0)
            {
                int count = int.Parse(rmdc["count"]);
                if (count > 0)
                {
                    string[] accounts = rmdc["accounts"].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (string account in accounts)
                    {
                        string acct = func_formatacct(account);
                        li_rc.Add(acct);
                    }
                }
                htb.Add("count", count);
                htb.Add("accounts", string.Join(",", li_rc.ToArray()));

                return this.FuncResult(new APIReturn(0, string.Empty, htb));
            }
            return this.FuncResult(new APIReturn(19804, $"找回帐号失败({rmdc.Code})"));
        }

        #endregion

        #region 帐号判断相关

        /// <summary>
        /// 检查战盟帐号是否存在
        /// </summary>
        /// <param name="account">战盟帐号</param>
        /// <param name="checkactive">是否验证帐号已激活</param>
        /// <returns>执行结果</returns>
        [HttpGet(@"isexist_acct")]
        public IActionResult func_IsExistAcct(string account, int checkactive)
        {
            if (account.IsNullOrEmpty() || !this.IsMobile(account) && !Utilities.IsValidEmail(account) )
            {
                return this.FuncResult(new APIReturn(10000, "参数错误(account)"));
            }

            #region 2980帐号

            Hashtable htb = new Hashtable();
            if (this.IsMobile(account) || account.EndsWith("@2980.com"))
            {
                string checkType;
                string phoneUserName;
                if (this.IsMobile(account))
                {
                    checkType = "phone";
                    phoneUserName = account;
                }
                else
                {
                    checkType = "name";
                    phoneUserName = account.Substring(0, account.LastIndexOf('@'));
                }

                Dictionary<string, object> argus = new Dictionary<string, object>();
                argus.Add("act", "dy_gs_checknamephone");
                argus.Add("portkey", DC2Conf.Passport2980);
                argus.Add("checktype", checkType);
                argus.Add("phoneUserName", phoneUserName);
                ReturnMsg2980 ret = P2980Invoker.InvokeHttp("funswregister", argus);

                #region 返回码处理

                int code;
                string msg;
                if (ret.Code == 250)
                {
                    code = 0;
                    msg = "帐号不存在";
                }
                else if (ret.Code == 625)
                {
                    if (checkactive == 1)
                    {
                        htb.Add("isActive", true);
                    }
                    return this.FuncResult(new APIReturn(10303, "帐号已存在", htb));
                }
                else if (ret.Code == 609)
                {
                    //参数不能为空、重要参数长度不足
                    code = 10000;
                    msg = "部分参数缺失";
                }
                else if (ret.Code == 611)
                {
                    //检测超出规定次数
                    code = 19902;
                    msg = "操作太频繁";
                }
                else if (ret.Code == 500)
                {
                    //服务器错误
                    code = -98;
                    msg = "未知系统错误";
                }
                else if (ret.Code == 614)
                {
                    //你不具备访问权限
                    code = -97;
                    msg = "IP没有权限";
                }
                else if (ret.Code == 583)
                {
                    //无任何执行动作
                    code = 10000;
                    msg = "部分参数缺失";
                }
                else
                {
                    code = 19801;
                    msg = $"参数或其它错误({ret.Code},{ret.Message})";
                }

                #endregion

                return this.FuncResult(new APIReturn(code, msg));
            }

            #endregion

            bool isActive = false;
            UrsacctInfo acctInfo = Ursacct.GetItem(account);
            if (acctInfo != null)
            {
                UrsuserInfo userInfo = Ursuser.GetItem(acctInfo.AcctNumber);
                if (userInfo.UrsTime.HasValue && userInfo.UrsTime.HasValue && userInfo.UrsTime.Value.Year > 2000 && (DateTime.Now - userInfo.UrsTime.Value).TotalDays > 3)//兼容之前没写日期的
                {
                    isActive = true;
                }
                if (checkactive == 1)
                {
                    D2unactiveInfo activeInfo = D2unactive.GetItemByNumber(acctInfo.AcctNumber.Value);
                    isActive = activeInfo != null && activeInfo.UavState == (int)EUAS.已激活2;
                    htb.Add("isActive", isActive);
                }
                return this.FuncResult(new APIReturn(10303, "帐号已存在", htb));
            }

            //请求DC验证
            //DCRequest dcr = new DCRequest(DCProdTypes.URS, DCMethodTypes.R);
            List<object> listobj = new List<object>();
            listobj.AddRange(new object[] { "email", account });
            //ReturnMsgDC rc = dcr.ExecRequest("acct_getinfoforupgrade.php", listobj.ToArray());
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "acct_getinfoforupgrade", listobj.ToArray());
            DicDCValue rc = dcr.GetDicDCValue();
            if (rc.Code == 23)
            {
                //帐号不存在
                return this.FuncResult(new APIReturn(0, "帐号不存在"));
            }
            else if (rc.Code == 99)
            {
                //帐号未激活
                isActive = false;
            }
            else
            {
                //帐号已存在
                isActive = true;
                int number = rc["number"].ToInt();
            }
            if (checkactive == 1)
            {
                htb.Add("isActive", isActive);
            }
            return this.FuncResult(new APIReturn(10303, "帐号已存在", htb));
        }
        /// <summary>
        /// 验证绑定手机号码是不是绑定手机
        /// </summary>
        /// <param name="account">邮箱帐号</param>
        /// <param name="mobile">手机号</param>
        /// <returns></returns>
        [HttpGet(@"valid_mobile")]
        public IActionResult func_valid_mobile(string account, string mobile)
        {
            if (account.IsNullOrEmpty() || !this.IsMobile(account) && !Utilities.IsValidEmail(account))
            {
                return this.FuncResult(new APIReturn(10000, "参数错误(account)"));
            }
            if(string.IsNullOrEmpty(mobile)||mobile.Length<7)
                return this.FuncResult(new APIReturn(10000, "参数错误(mobile)"));
            string tel=string.Empty;
            int number = DCClass.Func_URS_GetNumberByEmail(account);
            if(number == -1000)
                return this.FuncResult(new APIReturn(10300, "帐号不存在"));
            if (number != -1|| number !=0)
                tel  = DCClass.dc_getmobilebynumber(number);
            if(string.IsNullOrEmpty(tel))
                return this.FuncResult(new APIReturn(10356, "该帐号未绑定手机号"));
            if(tel!=mobile)
                return this.FuncResult(new APIReturn(10357, "帐号与手机号不匹配"));
            return this.FuncResult(new APIReturn(0, "ok"));
        }


        #endregion

        #region 绑定，关联手机相关

        [HttpGet(@"bindassociationmobile")]
        public IActionResult func_bindassociationmobile([FromQuery]string email, [FromQuery]string mobile)
        {
            DCResult dcr = null;
            DicDCValue rmdc = null;
            object[] obj = new object[] { "account", email, "mobile", mobile };
            dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.W, "acct_bindmobile", obj);
            rmdc = dcr.GetDicDCValue();;
            if (rmdc.Code == 0)
            {
                string itype = "";
                rmdc.TryGetValue("itype", out itype);
                if (itype == "1")
                    return this.FuncResult(new APIReturn(0, "绑定手机成功"));
                else
                {
                    //int n = dcc.Func_URS_GetNumberByEmail(account);
                    //string createtime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    //inc.FuncClassD2.Func_SendBindMobileMsg(Operator, account, n, mobile, createtime);
                    return this.FuncResult(new APIReturn(0, "设置关联手机成功"));
                }
            }
            else if (rmdc.Code == 105)
                return this.FuncResult(new APIReturn(10702, "该手机已关联此通行证帐号"));
            else if (rmdc.Code == 115)
                return this.FuncResult(new APIReturn(10704, "该手机已经绑定多个通行证帐号"));
            else if (rmdc.Code == 151)
                return func_2980_bindassociationmobile(email, mobile);
            else if (rmdc.Code==20)
                return this.FuncResult(new APIReturn(10300, "帐号不存在"));
            else
                return this.FuncResult(new APIReturn(10703, string.Format("手机号与通行证关联失败({0})", rmdc.Code)));
        }

        #endregion
        #region 获取帐号信息
        [HttpGet(@"getacctinfo")]
        public IActionResult func_getacctinfo(string ip, string gate, string account)
        {

            if (account.Length < 5) return this.FuncResult(new APIReturn(10000, "account error"));

            bool ismobile = IsMobile(account);
            if (account.Contains("@") == false && ismobile)
                account += "@2980.com";

            bool is2980 = account.ToLower().EndsWith("@2980.com");
            if (is2980)
            {
                string act = "dy_2980gamegetphoneqq";
                string mail = account.ToLower().Replace("@2980.com", "");


                SignMaker signMaker = new SignMaker();
                Dictionary<string, Object> dicParams = new Dictionary<string, object>();
                dicParams.Add("act", "dy_2980gamegetphoneqq");
                dicParams.Add("mail", mail);
                dicParams.Add("portkey", DC2Conf.Passport2980);
                string sign = signMaker.MakeSign(dicParams);

                Dictionary<string, object> argus = new Dictionary<string, object>();
                argus.Add("act", "dy_2980gamegetphoneqq");
                argus.Add("mail", mail);
                argus.Add("portkey", DC2Conf.Passport2980);
                argus.Add("sig", sign);

                ReturnMsg2980 ret = P2980Invoker.InvokeHttp("funswregister", argus);
                if (ret.Code == 250)
                {
                    string m = ret.Data["phone"].Value<string>();
                    string mobilestr = "";
                    if (IsMobile(m))
                        mobilestr = string.Format("{0}*****{1}", m.Substring(0, 3), m.Substring(m.Length - 3, 3));
                    Hashtable ht = new Hashtable();
                    ht["qq"] = ret.Data["qq"].Value<string>();
                    ht["phone"] = mobilestr;


                    return this.FuncResult(new APIReturn(0, "", ht));
                }
                else if (ret.Code == 201)
                {
                    return this.FuncResult(new APIReturn(10302, "帐号格式错误"));
                }
                else if (ret.Code == 202)
                {
                    return this.FuncResult(new APIReturn(10300, "帐号不存在"));
                }
                else if (ret.Code == 203)
                {
                    return this.FuncResult(new APIReturn(10307, "非手机注册帐号"));
                }
                else if (ret.Code == 301)
                {
                    return this.FuncResult(new APIReturn(10740, "无效的签名"));
                }
                else
                {
                    return this.FuncResult(new APIReturn(-98, $"未知系统错误2980{ret.Code}"));
                }

            }

            object[] objs = new object[] { "action", 1411, "ip", ip ?? string.Empty, "account", account, "gatesrc", gate };
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "shell_websys_r", objs.ToArray());
            DicDCValue rmdc = dcr.GetDicDCValue();

            string eamil = string.Empty;
            string mobile = string.Empty;
            string qq = string.Empty;
            if (rmdc.Code == 0)
            {
                string m = rmdc["mobile"];
                string mobilestr = "";
                if (IsMobile(m))
                    mobilestr = string.Format("{0}*****{1}", m.Substring(0, 3), m.Substring(m.Length - 3, 3));
                Hashtable ht = new Hashtable();
                ht["email"] = rmdc["email"];
                ht["mobile"] = mobilestr;
                ht["qq"] = rmdc["qq"];
                return this.FuncResult(new APIReturn(0, "", ht));
            }
            else if (rmdc.Code == 23)
            {
                return this.FuncResult(new APIReturn(10300, "账号不存在"));
            }
            else
            {
                return this.FuncResult(new APIReturn(10324, $"获取帐号信息失败({rmdc.Code})"));
            }
        }
        #endregion
        #region 验证码相关
        [HttpGet(@"getimgcode")]
        public IActionResult func_getimgcode(string regip)
        {
            string code = string.Empty;
            byte[] buffer = createVCode(out code);
            if (buffer != null && code.Length == 4)
            {
                string imgbytes = Convert.ToBase64String(buffer);
                Hashtable cls = new Hashtable();
                cls["imgbytes"] = imgbytes;
                cls["vregval"] = encRegVal(regip, code);
                if (DC2Conf.IsProg)
                    cls["code"] = code;
                return this.FuncResult(new APIReturn(0, "需要提供验证码", cls));
            }
            else
            {
                return this.FuncResult(new APIReturn(10100, "生成验证码失败"));
            }
        }
        [HttpGet(@"checkimgcode")]
        public IActionResult func_checkimgcode(string regip,string key,string code)
        {
            return decRegVal(key,code,regip);
        }
        #endregion

    }
}
