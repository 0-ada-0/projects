using DC2016.Admin.Controllers.Common;
using DC2016.Admin.Controllers.Urs.Models;
using DC2016.Admin.Enums;
using System.Collections.Generic;
using dywebsdk.Web;
using System.Net.Http;
using System.Collections;
using dywebsdk.Models;
using dywebsdk.Extension;
using System.Linq;

namespace DC2016.Admin.DC
{
    public class DCClass
    {
        /// <summary>
        /// 查询email的通行证number(支持普通帐号和2980帐号，和骆明已确认)
        /// </summary>
        /// <returns></returns>
        public static int Func_URS_GetNumberByEmail(string email)
        {
            object[] objs = { "gatesrc", "urs", "email", email };
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "acct_getnumberbyemail", objs.ToArray());
            DicDCValue rmdc = dcr.GetDicDCValue();

            int number = 0;
            if (rmdc.Code == 0)
            {
                number = rmdc["number"].ToInt();
            } else if(rmdc.Code == 23)
                return -1000;//帐号不存在
            else
            {
                return -1;//获取帐号信息失败
            }
            return number;
        }
        public static string dc_getmobilebynumber(int number)
        {
            object[] objs = { "vtype", "m", "number", number };
            
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.R, "acct_getvalidinfo", objs.ToArray());
            DicDCValue rmdc = dcr.GetDicDCValue();

            string  tel = "";
            if (rmdc.Code == 0)
            {
                tel = rmdc["mobile"];
            }
            else
            {
                return null;//获取帐号信息失败
            }
            return tel;
        }
        public int URS_GetURSNumberByNumber(int number)
        {
            //object[] objs = { "urs_getupgradeinfo.php", "number", number };
            //ReturnMessageDC2 rmdc = new DCClass(Operator, this.Product, "R").Func_GetRet2(objs);
            //if (rmdc.Code == 0)
            //{
            //    int data_ursnumber = rmdc.CHT_DC2["ursnumber"].ValueInt;
            //    return data_ursnumber;
            //}

            object[] objs = { "number", number };
            /*
            DCRequest dcr = new DCRequest(DCProdTypes.URS, DCMethodTypes.R);
            ReturnMsgDC rmdc = dcr.ExecRequest("urs_getupgradeinfo.php", objs);
            if (rmdc.Code == 0)
            {
                return rmdc.CHT_DC["ursnumber"].ValueInt;
            }
            return 0;
            */
            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.W, "acct_getbindmail", objs);
            DicDCValue dcValue = dcr.GetDicDCValue();
            if (dcValue.Code == 0)
            {
                return dcValue["ursnumber"].ToInt();
            }
            return 0;
        }

        public string URS_GetBindMail(int number)
        {
            //this.Product = "urs";
            //this.Method = "R";
            //List<object> LI_Objs = new List<object>(new object[] { "acct_getbindmail.php", "number", number, });
            //object[] objs = LI_Objs.ToArray();
            //ReturnMessageDC2 rmdc = this.Func_GetRet2(objs);
            //string email = "";
            //if (rmdc.Code == 0)
            //    email = rmdc.CHT_DC2["email"].ValueString;
            //if (email.Contains("@") == false) email = "";
            //return email;

            object[] objs = { "number", number };
            /*
           DCRequest dcr = new DCRequest(DCProdTypes.URS, DCMethodTypes.R);
           ReturnMsgDC rmdc = dcr.ExecRequest("acct_getbindmail.php", objs);
           if (rmdc.Code == 0)
           {
               return rmdc.CHT_DC["email"].ValueString;
           }
           return string.Empty;
           */

            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.W, "acct_getbindmail", objs);
            DicDCValue dcValue = dcr.GetDicDCValue();
            if (dcValue.Code == 0)
            {
                return dcValue["email"];
            }
            return string.Empty;
        }

        public DicDCValue Acct_Register(RegModel regModel, bool isfriend, int ip, string email, string password, bool passisencrypt, string name, string idcard, string mobile, int qq, string gatesrc, string pstype)
        {
            if (passisencrypt == false)
            {
                password = UrsHelper.mymd5(password);
            }
            List<object> listobj = new List<object>();
            listobj.AddRange(new object[] { "isfriend", isfriend.ToString().ToLower(), "ip", ip, "email", email, "password", password, "mobile", mobile, "qq", qq, "idcard", idcard, "gatesrc", gatesrc, "pstype", pstype, "name", name });
            if (!string.IsNullOrWhiteSpace(regModel.language) && regModel.language.Length > 0)
            {
                listobj.AddRange(new object[] { "language", regModel.language });
            }
            if (regModel.ismodenopass)
            {
                listobj.AddRange(new object[] { "ismodenopass", 1 });
            }
            if (regModel.adsid > 0)
            {
                listobj.AddRange(new object[] { "adsid", regModel.adsid });
            }
            if (regModel.tgaccount > 0)
            {
                listobj.AddRange(new object[] { "tgaccount", regModel.tgaccount });
            }

            DCResult dcr = DCInvoker.HttpInvoke(DCProdTypes.URS, DCMethodTypes.W, "acct_reg", listobj.ToArray());
            return dcr.GetDicDCValue();
        }
    }
}