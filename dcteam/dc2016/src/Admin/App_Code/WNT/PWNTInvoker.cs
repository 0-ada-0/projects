using dywebsdk.Models;
using dywebsdk.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DC2016.Admin.App_Code.WNT
{
    public class PWNTInvoker
    {
        public static ReturnMsgWnt InvokeHttp(string apiName, string json)
        {
            CallResult callResult;
            try
            {
                callResult = WebHttpClient.PostJSON("dc2016", "wnt", apiName,json);
                return new ReturnMsgWnt(callResult.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("请求wnt发生异常", ex);
            }
        }
        public static ReturnMsgWnt InvokeHttp(string apiName, object json)
        {
            CallResult callResult;
            try
            {
                callResult = WebHttpClient.PostJSON("dc2016", "wnt", apiName, json);
                return new ReturnMsgWnt(callResult.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("请求wnt发生异常", ex);
            }
        }
    }
}
