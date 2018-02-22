using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IF2017.Admin.Configs
{
    public class IFConfigReader
    {
        private static Microsoft.Extensions.Configuration.IConfigurationRoot conf { get; set; }
        public static void SetConf(Microsoft.Extensions.Configuration.IConfigurationRoot c)
        {
            conf = c;
        }

        public static bool IsProg
        {
            get
            {
                try
                {
                    return bool.Parse(conf["config:isprog"]);
                }
                catch
                {
                    return true;
                }
            }
        }
        public static string SignKey
        {
            get
            {
                return conf["key:signkey"];
            }
        }

        public static string IPFindFile
        {
            get
            {
                return conf["file:ipexts"];
            }
        }
        public static string DC2016Passport
        {
            get
            {
                return conf["dc2016:passport"];
            }
        }

        public static string RSAPrivateKey
        {
            get
            {
                return conf["rsa:private_key"];
            }
        }

        public static string RSAPublicKey
        {
            get
            {
                return conf["rsa:public_key"];
            }
        }

        public static string GetValue(string key)
        {
            return conf[key];
        }
    }
}
