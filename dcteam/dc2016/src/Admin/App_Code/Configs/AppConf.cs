using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Configs
{
    public class AppConf
    {
        private static IConfigurationRoot confRoot;

        internal static void Initialize(IConfigurationRoot configurationRoot)
        {
            confRoot = configurationRoot;
        }
    }
}
