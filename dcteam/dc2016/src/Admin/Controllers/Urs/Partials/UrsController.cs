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
using System.Security.Cryptography;
using DC2016.BLL.Enums;
using DC2016.Admin.DC;
using DC2016.Admin.Enums;
using DC2016.Admin.Configs;
using dywebsdk.Extension;
using DC2016.Admin.P2980;
using dywebsdk.Cryptography;

namespace DC2016.Admin.Controllers.Urs
{
    public partial class UrsController
    {
        static readonly byte[] key_64 = new byte[] { 1, 20, 6, 125, 136, 251, 14, 30, 1, 20, 6, 125, 136, 251, 14, 30 };
        static readonly byte[] iv_64 = new byte[] { 9, 200, 3, 156, 155, 200, 201, 63, 9, 200, 3, 156, 155, 200, 201, 63 };
        static SymmetricAlgorithm deskey = null;    //已弃用

        private string regcode;
        private int getnumberbyemail(string email)
        {
            object[] objs = { "email", email, "check2980", "1" };
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "acct_getnumberbyemail", objs);
            DicDCValue rmdc = dcr.GetDicDCValue();

            if (rmdc.Code == 0)
            {
                return rmdc["number"].ToInt();
            }
            return 0;
        }
        private string get2980ano(int number)
        {
            string[] objs = { "action", "1401", "gate", "urs","number",number.ToString() };
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "shell_websys_r", objs);
            DicDCValue rmdc = dcr.GetDicDCValue();

            if (rmdc.Code == 0)
            {
                return rmdc["ano"];
            }
            return null;
        }
        private IActionResult func_2980_bindassociationmobile(string email, string mobile)
        {


            Dictionary<string, object> argus = new Dictionary<string, object>();
            argus.Add("act", "dy_2980setrelationphone");
            argus.Add("portkey", DC2Conf.Passport2980);
            argus.Add("phone", mobile);
            int number = getnumberbyemail(email);
            string ano = get2980ano(number);
            argus.Add("ano", ano);

            ReturnMsg2980 ret = P2980Invoker.InvokeHttp("funswregister", argus);
            if (ret.Code == 250)
            {
                return this.FuncResult(new APIReturn(0, "设置关联手机成功"));
            }
            else if (ret.Code == 203)
            {
                return this.FuncResult(new APIReturn(10700, ret.Message + $",code={ret.Code}"));
            }
            else if (ret.Code == 202)
            {
                return this.FuncResult(new APIReturn(10701, ret.Message + $",code={ret.Code}"));
            }
            else if (ret.Code == 205)
            {
                return this.FuncResult(new APIReturn(10701, "手机注册帐号不允许关联手机" + $",code={ret.Code}"));
            }
            return this.FuncResult(new APIReturn(ret.Code, ret.Message + $",code={ret.Code}"));
        }
        private IActionResult func_exec_reg(RegModel regModel, bool isfriend, int ip, string acct, string pass, bool passismyencrypt, string tname, string idcard, string mobile, int qq, string gatesrc, string pstype, string extinfo, string regip)
        {
            //配置控制,是否需要检查注册时的验证码，一段时间之后应去掉此项判断，否则非公司正常产品的gate就绕过此内容
            if (DC2Conf.RegvcodeProducts.Contains(gatesrc) && DC2Conf.RegvcodeEnable)
            {
                //2016-1-20 对于163邮箱，没有传验证码的必须传验证码
                if (acct.ToLower().EndsWith("163.com"))
                {
                    if (string.IsNullOrEmpty(regModel.vcode) && string.IsNullOrEmpty(regModel.vregval))
                    {
                        return createVCode(regip);
                    }
                    else
                    {
                        var retValue = decRegVal(regModel.vregval, regModel.vcode, regip);
                        if (retValue != null)
                        {
                            return retValue;
                        }
                    }
                }
                else
                {
                    var retValue = checkRegRates(regModel.vcode, regModel.vregval, regip);
                    if (retValue != null)
                    {
                        return retValue;
                    }
                }
            }

            UrsacctInfo userInfo = Ursacct.GetItem(acct);
            if (userInfo != null)
            {
                Hashtable htbData = new Hashtable();
                htbData.Add("number", userInfo.AcctNumber.Value);
                return this.FuncResult(new APIReturn(10303, "帐号已存在", htbData));
            }

            DCClass dcc = new DCClass();
            DicDCValue dcValue = dcc.Acct_Register(regModel, isfriend, ip, acct, pass, passismyencrypt, tname, idcard, mobile, qq, gatesrc, pstype);
            if (dcValue.Code == 0)
            {
                int number = dcValue["number"].ToInt();
                Ursuser.CreateUser(number, acct, qq, mobile, idcard, extinfo);
                Hashtable cht_rcdata = new Hashtable();
                cht_rcdata["number"] = number;
                //添加缓存
                if (ip != 0 && ip != -1)
                {
                    string ipkey = string.Format("check-reg-ip-{0}", ip);
                    object obj = UrsHelper.Cache_GetObj(ipkey);
                    List<DateTime> li_ipreglist = new List<DateTime>();
                    if (obj != null && obj is List<DateTime>)
                        li_ipreglist = (List<DateTime>)obj;
                    li_ipreglist.Add(DateTime.Now);
                    UrsHelper.Cache_SetObj(ipkey, li_ipreglist);
                }
                addRegSuc(regip);
                cht_rcdata["email"] = regModel.email;
                cht_rcdata["mailhost"] = "http://mail"+regModel.email.Substring(regModel.email.IndexOf('@'), regModel.email.Length -  regModel.email.IndexOf('@'));
                return this.FuncResult(new APIReturn(0, "注册成功", cht_rcdata));
            }
            else if (dcValue.Code == 65)
            {
                return this.FuncResult(new APIReturn(10303, "帐号已存在"));
            }
            else
            {
                return this.FuncResult(new APIReturn(10306, $"注册失败({dcValue.Code})"));
            }
        }

        void addRegSuc(string regip)
        {
            RegCompact prev;

            string key = "ip_regrates_" + regip;

            string c = UrsHelper.Cache_GetObj(key) as string;
            if (false == string.IsNullOrEmpty(c))
            {
                prev = RegCompact.Parse(c);
                prev.update(DateTime.Now);
            }
            else
            {
                prev = new RegCompact(DateTime.Now);
            }
            UrsHelper.Cache_SetObj(key, prev.ToString(), 60, 0);

            //一旦通过验证码完成注册，则将此码和ip一起作为key，添加缓存，下次提交验证码时判断，
            //是否有此session，有则怀疑重复提交相同验证码,10分钟都不允许同ip提交相同验证码，会误伤
            if (!string.IsNullOrWhiteSpace(this.regcode) && this.regcode.Length == 4)
            {
                UrsHelper.Cache_SetObj(string.Format("cache_reg_code_{0}_ip_{1}", this.regcode, regip), 1, 10, 0);
            }
        }

        //检查注册频率
        private IActionResult checkRegRates(string vcode, string vregval, string regip)
        {
            string key = "ip_regrates_" + regip;
            string c = UrsHelper.Cache_GetObj(key) as string;
            if (false == string.IsNullOrEmpty(c))
            {
                RegCompact prev = RegCompact.Parse(c);
                if (prev.isOver())
                {
                    //如果传递了vcode和vregval 说明提交了验证码
                    if (vcode.Length == 4 && vregval.Length > 0)
                    {
                        //先判断vcode是否被重用了
                        if (UrsHelper.Cache_GetObj(string.Format("cache_reg_code_{0}_ip_{1}", vcode, regip)) != null)
                        {
                            return this.FuncResult(new APIReturn(10102, "请刷新验证码"));
                        }
                        this.regcode = vcode;
                        return decRegVal(vregval, vcode, regip);
                    }
                    else
                    {
                        return createVCode(regip);
                    }
                }
            }
            return null;
        }

        IActionResult decRegVal(string encdata, string inputcode, string regip)
        {
            //应该先要判断session里有这个值
            byte[] buffer = Convert.FromBase64String(encdata.Replace(" ", "+"));
            string s = System.Text.UTF8Encoding.UTF8.GetString(buffer);
            string decstr = UrsHelper.Decrypt(s, deskey, key_64, iv_64);


            string[] arr = decstr.Split(',');
            long ses = RegCompact.LastSeconds(DateTime.Now.Ticks, long.Parse(arr[2]));
            //10分钟600秒，过期之后重复生成有点浪费
            //考虑原有在if2中已有频度检查，这里继续生成一个新的
            if (ses > 600)
            {
                return createVCode(regip);
            }
            if (arr[1].ToLower() != inputcode.ToLower())
            {
                //验证码不正确的时候，是否需要返回新的呢
                return this.FuncResult(new APIReturn(10101, "验证码不正确"));
            }
            return null;
        }

        /// <summary>
        /// 生成验证码，并和加密变量vregval一起返回，如下三种情况被调用
        /// 验证码失效；超过一小时限定次数，直接获取验证码
        /// </summary>
        private IActionResult createVCode(string regip)
        {
            string code = string.Empty;
            byte[] buffer = createVCode(out code);
            if (buffer != null && code.Length == 4)
            {
                string imgbytes = Convert.ToBase64String(buffer);
                Hashtable cls = new Hashtable();
                cls["imgbytes"] = imgbytes;
                cls["vregval"] = encRegVal(regip, code); 
                if(DC2Conf.IsProg)
                    cls["code"] = code;
                return this.FuncResult(new APIReturn(10103, "需要提供验证码", cls));
            }
            else
            {
                return this.FuncResult(new APIReturn(10100, "生成验证码失败"));
            }
        }

        private string encRegVal(string ip, string code)
        {
            string org = string.Format("{0},{1},{2}", ip, code, DateTime.Now.Ticks);
            return Convert.ToBase64String(UrsHelper.Encrypt(org, deskey, key_64, iv_64));
        }

        private bool Func_SetAV2(D2unactiveInfo gua, out string retMsg)
        {
            retMsg = string.Empty;
            if (gua.UavState == (int)EUAS.已激活2)
            {
                string gatesrc = gua.UavGateSrc;
                if (gatesrc.Length > 1)
                {
                    retMsg = gatesrc;
                }
                return true;
            }
            D2unactive.Func_SetState(gua.UavGUID, (int)EUAS.已激活1);
            //object[] objs = { "active_setav.php", "number", gua.UavNumber };
            //ReturnMessageDC2 rmdc = new DCClass(Operator, this.UAV_Gate, "W").Func_GetRet2(objs);
            //ReturnMessageDC rmdc = DCClass.DC_GetRet(Operator, this.UAV_Gate, "W", objs);

            object[] objs = { "number", gua.UavNumber };
            //DCRequest dcr = new DCRequest(gua.UavGate, DCMethodTypes.W);
            //ReturnMsgDC rmdc = dcr.ExecRequest("active_setav.php", objs);

            DCResult dcr = DCInvoker.HttpInvoke(gua.UavGate, DCMethodTypes.W, "active_setav", objs.ToArray());
            DicDCValue rmdc = dcr.GetDicDCValue();

            if (rmdc.Code == 0)
            {
                string gatesrc = rmdc["gatesrc"];
                bool rc = gatesrc.Length > 0 ? D2unactive.Func_SetState(gua.UavGUID, (int)EUAS.已激活2, gatesrc) : D2unactive.Func_SetState(gua.UavGUID, (int)EUAS.已激活2);
                if (rc)
                {
                    string gatesrc2 = rmdc["gatesrc"];
                    if (gatesrc2.Length > 1)
                    {
                        retMsg = gatesrc2;
                    }
                }
                return rc;
            }
            else
            {
                return false;
            }
        }

        private string func_formatacct(string account)
        {
            string[] accountext = account.Split("@".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (accountext.Length != 2) return account;
            string acct = accountext[0];
            string prestr = "", suffixstr = "";
            prestr = acct.Length > 2 ? acct.Substring(0, 2) : acct;
            suffixstr = acct.Length > 4 ? acct.Substring(acct.Length - 2, 2) : "";
            return string.Format("{0}*****{1}@{2}", prestr, suffixstr, accountext[1]);
        }


    }
}
