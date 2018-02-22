using dywebsdk.Extension;
using dywebsdk.Models;
using dywebsdk.Web;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using IF2017.Admin.Controllers.Common;
using IF2017.Admin.Controllers.DK.Models;
using IF2017.Admin.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace IF2017.Admin.Controllers
{
    [Route("dk")]
    [HideInDocs]
    public partial class DKController : BaseController<DKController>
    {
        public static string DKDomain = "dk.duoyi.com";
        public DKController(ILogger<DKController> logger) : base(logger) { }

        [HttpGet("dk_proxy")]
        [HttpPost("dk_proxy")]
        public IActionResult dk_proxy()
        {
            WebParams ciac = new WebParams(Request);
            string runtest = ciac["runtest"];
            string account = ciac["hd_account"];
            string gate = ciac["hd_product"];
            string gatetarget = ciac["gatetarget"];
            int way = ciac["hd_way"].ToInt();
            int port = ciac["hd_port"].ToInt();
            string money = ciac["hd_money"];
            int amount = ciac["hd_amount"].ToInt();
            string ctype = ciac["hd_buy_type"];
            int server = ciac["server"].ToInt();
            int userid = ciac["userid"].ToInt();
            int reason = ciac["reason"].ToInt();
            string gateid = ciac["hd_gateid"];
            object[] args = new object[] { account, gate, way, port, money, ctype, server, userid, amount, reason, gatetarget };
            string s = string.Format("account={0}&gate={1}&way={2}&port={3}&money={4}&ctype={5}&server={6}&userid={7}&amount={8}&reason={9}&gatetarget={10}", args);
            string df_token = ciac["df_token"];
            if (df_token.Length == 32)
            {
                string rolename = ciac["df_rname"];
                string ursnumber = ciac["df_ursnumber"];
                int payuser = ciac["df_payuser"].ToInt();
                int showid = ciac["showid"].ToInt();
                object[] objArray2 = new object[] { s, df_token, ursnumber, rolename, payuser, showid };
                s = string.Format("{0}&df_token={1}&df_ursnumber={2}&df_rname={3}&df_payuser={4}&showid={5}", objArray2);
            }
            string urlbase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
            string url = "http://pay.duoyi.com/";
            switch (way)
            {
                case (int)EWAY.神州付:
                    url = string.Format("http://{1}/sales/ecard/szf/szfs2.aspx?args={0}", urlbase64, DKDomain);
                    break;

                case (int)EWAY.支付宝:
                    url = string.Format("http://{1}/sales/ecard/alipay/alipays2.aspx?args={0}", urlbase64, DKDomain);
                    break;

                case (int)EWAY.微信:
                    url = string.Format("http://{1}/sales/ecard/wxpayms/wxpays2.aspx?args={0}", urlbase64, DKDomain);
                    break;
            }
            if ((way == (int)EWAY.微信) && (runtest == "true"))
            {
                url = string.Format("http://{1}/sales/ecard/wxpayms/wxpays2.aspx?args={0}", urlbase64, "runtest.dk.duoyi.com");
            }
            if (way == (int)EWAY.银联)
            {
                url = string.Format("http://{2}/sales/ecard/cnpay/cnp_s2.aspx?gateid={1}&args={0}", urlbase64, gateid, DKDomain);
            }
            if (way == (int)EWAY.JCard)
            {
                url = string.Format("http://{1}/sales/ecard/jcard/jcard_s2.aspx?args={0}", urlbase64, DKDomain);
            }
            return FuncResult(new APIReturn(0, url));
        }

        [HttpGet("getwxorder")]
        [HttpPost("getwxorder")]
        public IActionResult get_wx_order()
        {
            APIReturn apiReturn = null;
            WebParams ciac = new WebParams(Request);
            ciac.Add("action", "wxpayms_getordid");
            CallResult cr = WebHttpClient.InvokeHttp("if2017", "dk", "wxpayms_getordid", HttpMethod.Get, ciac);

            if ((cr.Code != 0) || string.IsNullOrEmpty(cr.Message))
            {
                return this.FuncResult(new APIReturn(10000, cr.Message));
            }
            string[] msgs = cr.Message.Split(',');
            if ((int.Parse(msgs[0]) != 0) || (msgs.Length != 9))
            {
                return this.FuncResult(new APIReturn(10000, string.Format($"{cr.Code}:{cr.Message}")));
            }
            if (ciac["image"] == "image")
            {
                MemoryStream ms = new MemoryStream();
                GetQRCode(msgs[8], ms);
                ArraySegment<byte> data = new ArraySegment<byte>();
                ms.TryGetBuffer(out data);
                string str15 = Convert.ToBase64String(data.Array);
                msgs[8] = str15;
            }
            Hashtable ht = new Hashtable();
            ht["orderid"] = msgs[1];
            ht["code"] = msgs[8];
            apiReturn = new APIReturn(0, "", ht);
            return this.FuncResult(apiReturn);
        }
        /// <summary>
        /// 获取订单号状态
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpGet("getwxorderstate")]
        [HttpPost("getwxorderstate")]
        public IActionResult get_wx_order_state([FromQuery]string orderid)
        {
            APIReturn apiReturn = null;
            if (string.IsNullOrEmpty(orderid))
            {
                apiReturn = new APIReturn(10000, $"参数不正确orderid={orderid}");
                return this.FuncResult(apiReturn);
            }
            List<object> listobj = new List<object>();
            CallResult cr = WebHttpClient.InvokeHttp("if2017", "dk", "wxpayms_getordid", HttpMethod.Get, new object[] { "orderid", orderid, "action", "wxpaystate" });
            if (cr.Code == 0 || !string.IsNullOrEmpty(cr.Message))
            {
                char[] separator = new char[] { '|' };
                string[] strArray = cr.Message.Split(separator);
                if ((int.Parse(strArray[0]) == 0) && (cr.Message.Length > 2))
                {
                    apiReturn = new APIReturn(0, cr.Message.Substring(2));
                    return this.FuncResult(apiReturn);
                }
            }
            apiReturn = new APIReturn(196021, $"获取订单状态失败code={cr.Code},message={cr.Message}");
            return this.FuncResult(apiReturn);
        }

        private static bool GetQRCode(string strContent, MemoryStream ms)
        {
            QrCode code;
            string str = strContent;
            QuietZoneModules modules = QuietZoneModules.Four;
            int num = 12;
            if (new QrEncoder(0).TryEncode(str, out code))
            {
                new GraphicsRenderer(new FixedModuleSize(num, modules)).WriteToStream(code.Matrix, ImageFormat.Png, ms);
                return true;
            }
            return false;
        }
        /// <summary>
        /// ios凭据验证
        /// </summary>
        /// <param name="issandbox">是否沙箱，沙箱可以理解为测试环境。程序内做了自动检测</param>
        /// <param name="ordid">订单号</param>
        /// <param name="receipt">回执文本</param>
        /// <returns></returns>
        [HttpPost("test/check_apple_status")]
        public IActionResult test_CheckAppleStatus([FromQuery]bool issandbox, [FromQuery]string ordid, [FromQuery]string receipt)
        {
            return Func_CheckAppleStatus(issandbox,ordid,receipt);
        }
        /// <summary>
        /// ios凭据验证
        /// </summary>
        /// <param name="issandbox">是否沙箱，沙箱可以理解为测试环境。程序内做了自动检测</param>
        /// <param name="ordid">订单号</param>
        /// <param name="receipt">回执文本</param>
        /// <returns></returns>
        [HttpPost("check_apple_status")]
        public IActionResult Func_CheckAppleStatus(bool issandbox, string ordid, string receipt)
        {
            //receipt=System.IO.File.ReadAllText("C:\\Users\\duoyi\\Desktop\\TextFile.txt");
            if (string.IsNullOrEmpty(receipt))
                return this.FuncResult(new APIReturn(10000, $"参数错误(receipt)"));

            string clientIP = this.IP;
            CallResult result = WebHttpClient.CheckIPWhiteList("if2017", "apple", "buy_itunes_verifyreceipt", clientIP);
            int success = result.Code;
            if (success != 0)
            {
                return this.FuncResult(new APIReturn(-97, "IP没有访问权限"));
            }

            string itunes= "buy_itunes_verifyreceipt";
            string receiptbase64 = Convert.ToBase64String(Encoding.ASCII.GetBytes(receipt));
            if (Regex.IsMatch(receipt, "\"environment\"\\s*=\\s*\"Sandbox\""))
                itunes = "sandbox_itunes_verifyreceipt";
            try
            {
                Dictionary<string, string> dicDatas = new Dictionary<string, string>();
                dicDatas.Add("receipt-data", receiptbase64);
                CallResult callresult = WebHttpClient.PostJSON("if2017", "apple", itunes, dicDatas);
                return this.FuncResult(new APIReturn(0, callresult.Message));
            }
            catch (Exception ex)
            {
                throw new Exception("请求ios凭据验证失败", ex);
            }

        }
    }
}
