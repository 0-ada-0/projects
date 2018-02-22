using DC2016.Admin.Configs;
using DC2016.Admin.Controllers.Common;
using DC2016.Admin.DC;
using DC2016.Admin.DC2.Lib;
using DC2016.Admin.Enums;
using DC2016.BLL;
using DC2016.BLL.Enums;
using DC2016.Model;
using dywebsdk.Mails;
using dywebsdk.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dywebsdk.Extension;

namespace DC2016.Admin.DC2
{
    public class Job
    {
        static bool isGetListRunning = false;
        static bool isSendMailRunning = false;
        public static int Lib_UnActive_GetList(string product)
        {
            return UnActive_GetList(product, new StringBuilder());
        }

        public static int UnActive_GetList(string product, StringBuilder sbout)
        {
            if (isSendMailRunning) return -1000;

            try
            {
                isGetListRunning = true;
                DateTime time1 = D2unactive.GetLast(product);
                object[] objs = { "state", 1, "time1", time1.ToString("yyyy-MM-dd HH:mm:ss") };
                //DCRequest dcr = new DCRequest(product, DCMethodTypes.R);
                //ReturnMsgListDC rmdc = dcr.ExecRequestList("active_getlist.php", objs);
                ListDCValue dcValue= DCInvoker.HttpInvoke(product, DCMethodTypes.R, "active_getlist", objs).GetListDCValue();

                sbout.Append(string.Format("\r\ngetlist返回原文，{0}", dcValue.Content));
                if (dcValue.Code != 0)
                {
                    return -2;
                }
                if (dcValue.ListDatas.Count <= 1)
                {
                    return -3;
                }
                int count = 0;
                for (int i = 1; i < dcValue.ListDatas.Count; i++)
                {
                    int data_number = dcValue.ListDatas[i][0].ToInt();
                    string data_email = dcValue.ListDatas[i][1];
                    DateTime data_time1 = DateTime.Parse(dcValue.ListDatas[i][2]);
                    string UAV_GateSrc = "";
                    int data_flag = 0;//比如nopass
                    if (dcValue.ListDatas[i].Length >= 4)
                    {
                        UAV_GateSrc = dcValue.ListDatas[i][3];
                    }
                    if (dcValue.ListDatas[i].Length >= 5)
                    {
                        data_flag = dcValue.ListDatas[i][4].ToInt();
                    }
                    D2unactiveInfo ga = product == "dw" ? D2unactive.initWithUIDForURSUpgrade(product, data_number) : D2unactive.initWithUID(product, data_number);

                    if (ga == null)//已存在，不处理
                    {
                        ga = new D2unactiveInfo()
                        {
                            UavGUID = Guid.NewGuid().ToString("N"),
                            UavState = (int)EUAS.未发送,
                            UavGate = product,
                            UavGateSrc = UAV_GateSrc,
                            UavNumber = data_number,
                            UavEMail = data_email,
                            UavTime1 = data_time1,
                            UavFlag = data_flag
                        };
                        D2unactive.Insert(ga);

                        sbout.Append(string.Format("\r\n成功导入，{0}", data_number));
                        count++;
                    }
                    else
                    {
                        sbout.Append(string.Format("\r\n这个帐号已存在，不必导入{0}", data_number));
                    }
                }
                return count;
            }
            catch (Exception es)
            {
                sbout.Append(string.Format("\r\n发生了异常{0}", es.Message));
            }
            finally
            {
                isGetListRunning = false;
            }
            return -1;
        }

        public static int UnActive_SendMail(string product, ViewRenderService viewRender, ref int countFailed)
        {
            if (isSendMailRunning) return -1000;

            try
            {
                isSendMailRunning = true;
                int countSucceed = 0;
                List<D2unactiveInfo> sccga = D2unactive.GetUnSend(product, (int)EUAS.未发送, 100);
                foreach (D2unactiveInfo gua in sccga)
                {
                    bool rc = Func_SendMail(gua, viewRender);
                    if (rc)
                        countSucceed++;
                    else
                        countFailed++;
                }
                return countSucceed;
            }
            finally
            {
                isSendMailRunning = false;
            }
            return -1;
        }
        public static bool IsTW(int uavFlag)
        {
           
                return uavFlag / 100 % 10 == 1;
            
        }

        public static bool Func_SendMail(D2unactiveInfo unactiveInfo, ViewRenderService viewRender)
        {
            string pName = DC2Conf.GetPName(unactiveInfo.UavGate);
            string srcname = DC2Conf.GetGateSrcUrl(unactiveInfo.UavGateSrc);

            string title = string.Format("请确认您的{0}帐号", pName);
            string url = string.Format(DC2Conf.AvUrlFormat, unactiveInfo.UavGUID, unactiveInfo.UavGate);
            if (unactiveInfo.UavFlag == 11 || unactiveInfo.UavFlag == 12)
            {
                url = string.Format(DC2Conf.AvNPUrlFormat, unactiveInfo.UavGUID, unactiveInfo.UavGate);
            }

            Hashtable cht_data = new Hashtable();
            cht_data["来源产品名"] = srcname;
            if (!string.IsNullOrWhiteSpace(srcname) && srcname.IndexOf("多益通") == -1)
                title += string.Format("[{0}]", srcname);
            cht_data["产品名"] = pName;
            cht_data["用户帐号名"] = unactiveInfo.UavEMail;
            cht_data["链接地址"] = url;
            string viewName = "letter_active";
            string fromName = "多益网络";
            if (IsTW(unactiveInfo.UavFlag??0) && unactiveInfo.UavGateSrc == "dw")
            {
                title = string.Format("请确认您的{0}帐号[夢想帝王]", "英雄通行證");
                cht_data["来源产品名"] = "夢想帝王";
                cht_data["产品名"] = "英雄通行證";
                viewName = "letter_active_tw";
                fromName = "英雄網路";
                string urltw = url.Replace("hi.duoyi.com", "hi.herojoys.com");
                cht_data["链接地址"] = urltw;
            }
            if (DC2Conf.IsHerojoys)
            {
                title = title.Replace("多益", "英雄");
                cht_data["产品名"] = "英雄通行证";
                cht_data["来源产品名"] = cht_data["来源产品名"].ToString().Replace("多益", "英雄");
                fromName = "英雄网络";
            }
            if (unactiveInfo.UavGate.ToLower() == "dw" || unactiveInfo.UavGate.ToLower() == "mx")//这些是确认邮件
            {
                viewName = "letter_confirmemail";
                title = string.Format("请确认您在[{0}]使用的邮箱帐号", pName);
            }
            string viewPath = UrsHelper.GetTPLViewPath(viewName);
            string content = viewRender.Render<Hashtable>(viewPath, cht_data);

            //this.EndFor(sw.ToString());
            bool rc = false;
            try
            {
                rc = D2lib.SendMail_Henhaoji(unactiveInfo, fromName, unactiveInfo.UavEMail, unactiveInfo.UavEMail, title, content, true);

                if (rc)
                {
                    D2unactive.Func_SetState(unactiveInfo.UavGUID, (int)EUAS.已发送);
                }
            }
            catch (Exception es)
            {
                if (es is FormatException)
                {
                    rc = true;
                    D2unactive.Func_SetState(unactiveInfo.UavGUID, (int)EUAS.异常);
                }
            }
            return rc;
        }
    }
}
