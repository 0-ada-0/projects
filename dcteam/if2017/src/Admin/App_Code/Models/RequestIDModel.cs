using dywebsdk.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.Models
{
    public class RequestIDModel
    {
        public static string RequestIDKey = "__requestid";

        public static string GetRequestID(HttpContext context)
        {
            if (context.Items.ContainsKey(RequestIDKey))
            {
                return context.Items[RequestIDKey].ToString();
            }
            WebParams webParams = new WebParams(context.Request);
            if (webParams.ContainsKey(RequestIDKey))
            {
                return webParams[RequestIDKey];
            }
            if (context.Request.Headers.ContainsKey(RequestIDKey))
            {
                return context.Request.Headers[RequestIDKey].ToString();
            }

            string requestID = Guid.NewGuid().ToString("N");
            context.Items.Add(RequestIDKey, requestID);
            return requestID;
        }
    }
}
