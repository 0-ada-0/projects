using DC2016.Admin.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.App_Code.WNT
{
    public class ReturnMsgWnt : ReturnMsg
    {
        public JObject Data { get; private set; }

        public ReturnMsgWnt(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                this.Data = JsonConvert.DeserializeObject<JObject>(data);
                this.Code = this.Data["code"].Value<int>();
                this.Succeed = this.Data["succeed"].Value<bool>();
               // this.Message = this.Data["msg"].Value<JObject>()["uuid"];
            }
        }
    }
}
