using dywebsdk.Web;
using IF2017.Admin.Configs;
using IF2017.Admin.Controllers.Common;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IF2017.Admin.Common
{
    public class DC2Invoker
    {
        public static APIReturn InvokeHttp(string apiName, HttpMethod method, IDictionary postData)
        {
            try
            {
                if (postData == null)
                {
                    postData = new Dictionary<string, object>();
                }
                if (postData is Dictionary<string, object>)
                {
                    var dicParams = (Dictionary<string, object>)postData;
                    if (!dicParams.ContainsKey("passport"))
                    {
                        dicParams.Add("passport", IFConfigReader.DC2016Passport);
                    }
                }
                var callResult = WebHttpClient.InvokeHttp("if2017", "dc2016", apiName, method, postData);
                return JsonConvert.DeserializeObject<APIReturn>(callResult.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("请求DC2016发生异常", ex);
            }
        }
    }
}
