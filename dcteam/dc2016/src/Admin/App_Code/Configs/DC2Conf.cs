using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Configs
{
    public static class DC2Conf
    {
        private static IConfigurationRoot confRoot;

        internal static void Initialize(IConfigurationRoot configurationRoot)
        {
            confRoot = configurationRoot;
        }

        public static bool IsProg
        {
            get
            {
                return bool.Parse(confRoot["sys:prog"]);
            }
        }

        public static string RedisConnection
        {
            get
            {
                return confRoot["sys:redisconnection"];
            }
        }

        public static string MySqlConnection
        {
            get
            {
                return confRoot["sys:mysqlconnection"];
            }
        }

        public static string Passport
        {
            get
            {
                return confRoot["sys:passport"];
            }
        }
        public static string getshareUrl(string gate)
        {
          
                return confRoot["share:"+gate];
            
        }
        public static string Passport2980
        {
            get
            {
                return confRoot["2980:passport"];
            }
        }

        public static bool IsHerojoys
        {
            get
            {
                return bool.Parse(confRoot["sys:isHerojoys"]);
            }
        }

        public static string MPFUrlHeader
        {
            get
            {
                return $"http://m.pf.duoyi.com/func/iface.aspx?passport={confRoot["mpf:passport"]}";
            }
        }

        public static List<string> VipIPs
        {
            get
            {
                List<string> lstVipIPs = new List<string>();
                foreach (var item in confRoot.GetSection("ws:vipips").GetChildren())
                {
                    lstVipIPs.Add(item.Value);
                }
                return lstVipIPs;
            }
        }

        public static string AgpUrlFormat
        {
            get
            {
                return confRoot["urlformat:getpass"];
            }
        }
        public static string AvUrlFormat
        {
            get
            {
                return confRoot["urlformat:active"];
            }
        }
        public static string AvNPUrlFormat
        {
            get
            {
                return confRoot["urlformat:activenp"];
            }
        }

        public static string GetPName(string gate)
        {
            return confRoot[$"dy:pname:{gate}"];
        }

        public static string GetGateSrcUrl(string gateSrc)
        {
            return confRoot[$"gate:{gateSrc}"];
        }

        public static string[] FirendAcctExt()
        {
            return confRoot.GetSection("friendlist:ext").GetChildren().Select(e => e.Value).ToArray();
        }

        public static string[] RegvcodeProducts
        {
            get
            {
                return confRoot.GetSection("regvcode:products").GetChildren().Select(e => e.Value).ToArray();
            }
        }

        public static bool RegvcodeEnable
        {
            get
            {
                return bool.Parse(confRoot["regvcode:enable"]);
            }
        }
    }
}
