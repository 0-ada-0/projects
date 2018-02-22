using DC2016.Admin.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.P2980
{
    public class ReturnMsg2980 : ReturnMsg
    {
        public JObject Data { get; private set; }

        public ReturnMsg2980(string plistData)
        {
            if (!string.IsNullOrEmpty(plistData))
            {
                this.Data = Lib.ParsePListToJson(plistData);
                this.Code = this.Data["Code"].Value<int>();
                this.Succeed = this.Data["Succeed"].Value<bool>();
                this.Message = this.Data["Message"].Value<string>();
            }
        }
    }
}
