using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using DC2016.Admin.Common;
using dywebsdk.Web;

namespace DC2016.Admin.Middleware
{
    /// <summary>
    /// 请求日志中间件
    /// </summary>
    public class RequestLoggerMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;

            var body = context.Response.Body;
            var buffer = new MemoryStream();
            context.Response.Body = buffer;
            string resultContent = string.Empty;
            try
            {
                await _next.Invoke(context);

                buffer.Position = 0;
                await buffer.CopyToAsync(body);
                buffer.Position = 0;
                using (StreamReader reader = new StreamReader(buffer))
                {
                    resultContent = reader.ReadToEnd();
                }
            }
            finally
            {
                context.Response.Body = body;
            }
            try
            {
                ILogger logger = context.Items["__requestLogger"] as ILogger;
                if (logger == null)
                {
                    logger = Log.Logger;
                }
                //return this.HttpContext.Connection.RemoteIpAddress.ToString();
                string ip = WebHelper.GetClientRealIP(request.HttpContext);
                string url = $"{request.Path}{request.QueryString}";
                string form = "<空>";

                if (request.HasFormContentType)
                {
                    form = string.Join("&", request.Form.Select(e => $"{e.Key}={e.Value}"));
                }
                else
                {
                    //MemoryStream ms = new MemoryStream();
                    //request.Body.CopyTo(ms);
                    //ms.Position = 0;
                    //using (StreamReader reader = new StreamReader(ms))
                    //{
                    //    string content = reader.ReadToEnd();
                    //    if (!string.IsNullOrEmpty(content))
                    //    {
                    //        form = content;
                    //    }
                    //}
                }
                logger.LogTrace("{0} {1} {2} {3} {4} {5}", request.Protocol, request.Method, ip, url, form, resultContent);
            }
            catch { }
        }
    }
}
