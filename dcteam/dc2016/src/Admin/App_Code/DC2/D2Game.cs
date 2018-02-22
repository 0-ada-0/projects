using DC2016.Admin.Configs;
using DC2016.Admin.Controllers.Common;
using DC2016.Admin.DC2.Lib;
using DC2016.BLL;
using DC2016.BLL.Enums;
using DC2016.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using dywebsdk.Mails;
using dywebsdk.Models;
using DC2016.Admin.Common;
using Microsoft.Extensions.Logging;

namespace DC2016.Admin.DC2
{
    public class D2Game
    {
        public static bool Func_SendMail(D2getpassInfo passInfo, string lang, ViewRenderService viewRender)
        {
            string pName = DC2Conf.GetPName(passInfo.GtpsGate);

            string title = string.Format("{0}-密码找回", pName);
            string url = string.Format(DC2Conf.AgpUrlFormat, passInfo.GtpsGUID, passInfo.GtpsGate);

            string viewName = "letter_getpass";
            Hashtable cht_data = new Hashtable();
            cht_data["产品名"] = pName;
            cht_data["链接地址"] = url;
            string fromName = "多益网络";
            if (string.IsNullOrWhiteSpace(lang) || lang.ToLower() == "zh-tw")
            {
                title = string.Format("{0}-密碼找回", "英雄通行證");
                viewName = "letter_getpass_tw";
                cht_data["产品名"] = "英雄通行證";
                fromName = "英雄網絡";
                string urltw = url.Replace("hi.duoyi.com", "hi.herojoys.com");
                cht_data["链接地址"] = urltw;
            }
            else if (DC2Conf.IsHerojoys)
            {
                title = string.Format("{0}-密码找回", "英雄通行证");
                cht_data["产品名"] = "英雄通行证";
                fromName = "英雄网络";
                string urltw = url.Replace("hi.duoyi.com", "dy.herojoys.com");
                cht_data["链接地址"] = urltw;
            }
            string viewPath = UrsHelper.GetTPLViewPath(viewName);
            string content = viewRender.Render<Hashtable>(viewPath, cht_data);
            string emailaddress = passInfo.GtpsEMail.ToLower();

            /*
             * TODO: 这里需要确认一下逻辑
            if (emailaddress.EndsWith("@old.2980.com"))
            {
                //old.2980.com的帐号进行特殊处理
                //发到该手机号@old.2980.com帐号原先登录游戏时的那个邮箱
                string mobile = emailaddress.Split('@')[0];
                URSOld2980 uold = new URSOld2980(Operator, mobile);
                if (URSOld2980.IsFill(uold))
                    emailaddress = string.Format("{0}@2980.com", uold.UO_Account);
            }
            */

            bool rc = false;
            try
            {
                rc = D2lib.SendMail_Henhaoji(null, fromName, emailaddress, emailaddress, title, content, true);

                if (rc)
                {
                    D2getpass.SetState(passInfo.GtpsGUID, (int)EGST.已发送);
                }
            }
            catch (Exception es)
            {
                if (es is FormatException)
                {
                    D2getpass.SetState(passInfo.GtpsGUID, (int)EGST.异常);
                }
                Log.Logger.LogCritical(es.ToString());
            }
            return rc;
        }
    }
}
