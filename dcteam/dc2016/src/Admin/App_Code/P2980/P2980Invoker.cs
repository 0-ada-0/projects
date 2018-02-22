using DC2016.Admin.Common;
using dywebsdk.Web;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DC2016.Admin.P2980
{
    public class P2980Invoker
    {
        public static ReturnMsg2980 InvokeHttp(string apiName, IDictionary postData)
        {
            try
            {
                var callResult = WebHttpClient.InvokeHttp("dc2016", "2980", apiName, HttpMethod.Get, postData);
                Log.Logger.LogDebug("debug", callResult.Source, callResult.Message);
                return new ReturnMsg2980(callResult.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("请求2980发生异常", ex);
            }
        }
    }
}
