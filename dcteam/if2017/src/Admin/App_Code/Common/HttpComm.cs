using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using dywebsdk.Web;
using IF2017.Admin.Models;

namespace IF2017.Admin.Common
{
    public class HttpComm
    {
        private static readonly int DefaultTimeout = 5000;
        private static Encoding DefaultEncoding = Encoding.UTF8;

        public HttpResult ExecGetRequest(string url)
        {
            //Dictionary<string, string> dicRequestParams = new Dictionary<string, string>();
            //dicRequestParams.Add("url", url);

            //return this.ExecGetRequest(dicRequestParams);

            // By Maples7
            try
            {
                string ret = WebHelper.GetFrom(url);
                return new HttpResult(ret);
            }
            catch (Exception ex)
            {
                return new HttpResult(ex);
            }
        }

        public HttpResult ExecGetRequest(Dictionary<string, string> dicReqParams)
        {
            int num = 3;
            Exception exception = null;
            for (int i = 0; i < num; i++)
            {
                try
                {
                    string uriString = dicReqParams["url"];
                    string cookieHeader = dicReqParams.ContainsKey("cookies") ? dicReqParams["cookies"] : string.Empty;
                    int timeout = dicReqParams.ContainsKey("timeout") ? int.Parse(dicReqParams["timeout"]) : DefaultTimeout;
                    string strEnc = dicReqParams.ContainsKey("encoding") ? dicReqParams["encoding"] : string.Empty;
                    Encoding encoding = string.IsNullOrEmpty(strEnc) ? DefaultEncoding : Encoding.GetEncoding(strEnc);
                    CookieCollection cookies = new CookieCollection();
                    CookieContainer container = new CookieContainer();
                    if (cookieHeader.Length > 0)
                    {
                        Uri uri = new Uri(uriString);
                        container.SetCookies(uri, cookieHeader);
                        cookies = container.GetCookies(uri);
                    }
                    string responseString = HttpHelper.GetResponseString(HttpHelper.CreateGetHttpResponse(uriString, timeout, null, cookies), encoding);
                    return new HttpResult(responseString);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }
            return new HttpResult(exception);
        }

        public HttpResult ExecWebRequest(Dictionary<string, string> dicReqParams)
        {
            int num = 3;
            Exception exception = null;
            for (int i = 0; i < num; i++)
            {
                try
                {
                    string uriString = dicReqParams["url"];
                    string cookieHeader = dicReqParams.ContainsKey("cookies") ? dicReqParams["cookies"] : string.Empty;
                    int timeout = dicReqParams.ContainsKey("timeout") ? int.Parse(dicReqParams["timeout"]) : DefaultTimeout;
                    string strEnc = dicReqParams.ContainsKey("encoding") ? dicReqParams["encoding"] : string.Empty;
                    Encoding encoding = string.IsNullOrEmpty(strEnc) ? DefaultEncoding : Encoding.GetEncoding(strEnc);
                    CookieCollection cookies = new CookieCollection();
                    CookieContainer container = new CookieContainer();
                    if (cookieHeader.Length > 0)
                    {
                        Uri uri = new Uri(uriString);
                        container.SetCookies(uri, cookieHeader);
                        cookies = container.GetCookies(uri);
                    }
                    string responseString = HttpHelper.GetResponseString(HttpHelper.CreateGetHttpResponse(uriString, timeout, null, cookies), encoding);
                    return new HttpResult(responseString);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }
            return new HttpResult(exception);
        }
    }
}
