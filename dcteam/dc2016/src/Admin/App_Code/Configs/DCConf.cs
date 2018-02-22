using DC2016.Admin.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Configs
{
    public class DCConf
    {
        private static IConfigurationRoot confRoot;

        internal static void Initialize(IConfigurationRoot configurationRoot)
        {
            confRoot = configurationRoot;
        }

        public static string GetDCUrl(string prodType, string methodType)
        {
            string key = $"dc:{prodType}:{methodType}";

            return confRoot[key];
        }
    }
}
