using DC2016.Admin.Controllers.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Controllers.Activity.Mode
{
    public class ShareGuidInfo
    {
        private JToken jtoken;
        public string guid;
        private string deviceinfo;
        private static string[] keys = {  "appCodeName","colorDepth","doNotTrack","indexedDB",
            "ip","language","localStorage","openDatabase","platform","sessionStorage","TimezoneOffset","widthandheight"};

        public ShareGuidInfo(string json)
        {
            jtoken = JObject.Parse(json);
            for (int i = 0; i < keys.Length; i++)
            {
                deviceinfo += jtoken.Value<string>(keys[i]);
            }
        }
        public string getGuid()
        {
            return UrsHelper.MD5(deviceinfo);
        }
        public string getIP()
        {
            return jtoken.Value<string>("ip");
        }

    }
}
