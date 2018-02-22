using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DC2016.Admin.Common
{
    public class HttpHelper
    {
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        public static HttpWebResponse CreateGetHttpResponse(string url, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //TODO: 类型不支持
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpHelper.CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "GET";
            //request.UserAgent = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent;
            request.ContinueTimeout = timeout;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Uri(url), cookies);
            }
            return (request.GetResponseAsync().Result as HttpWebResponse);
        }

        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int timeout, string userAgent, CookieCollection cookies)
        {
            HttpWebRequest request = null;
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(HttpHelper.CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            //request.UserAgent = string.IsNullOrEmpty(userAgent) ? DefaultUserAgent : userAgent;
            request.ContinueTimeout = timeout;
            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Uri(url), cookies);
            }
            if ((parameters != null) && (parameters.Count != 0))
            {
                StringBuilder builder = new StringBuilder();
                int num = 0;
                foreach (string str in parameters.Keys)
                {
                    if (num > 0)
                    {
                        builder.AppendFormat("&{0}={1}", str, parameters[str]);
                    }
                    else
                    {
                        builder.AppendFormat("{0}={1}", str, parameters[str]);
                        num++;
                    }
                }
                byte[] bytes = Encoding.ASCII.GetBytes(builder.ToString());
                using (Stream stream = request.GetRequestStreamAsync().Result)
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            //request.Headers.GetValues("Content-Type");
            return (request.GetResponseAsync().Result as HttpWebResponse);
        }

        public static string GetResponseString(HttpWebResponse webresponse, Encoding encoding)
        {
            using (Stream stream = webresponse.GetResponseStream())
            {
                return new StreamReader(stream, encoding).ReadToEnd();
            }
        }
    }
}

