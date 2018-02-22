using DC2016.Admin.Enums;
using dywebsdk.Models;
using dywebsdk.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DC2016.Admin.DC
{
    public class DCInvoker
    {
        public static DCResult HttpInvoke(DCProdTypes prodType, DCMethodTypes methodType, string apiName, params object[] datas)
        {
            return HttpInvoke(prodType.ToString(), methodType, apiName, datas);
        }

        public static DCResult HttpInvoke(string product, DCMethodTypes methodType, string apiName, params object[] datas)
        {
            try
            {
                string projectName = $"dc_{product.ToLower()}_{methodType.ToString().ToLower()}";
                CallResult callResult = WebHttpClient.InvokeHttp("dc2016", projectName, apiName, HttpMethod.Get, datas);
                return new DCResult(callResult);
            }
            catch (Exception ex)
            {
                throw new Exception($"调用DC发生异常({product},{methodType},{apiName})", ex);
            }
        }
    }
}
