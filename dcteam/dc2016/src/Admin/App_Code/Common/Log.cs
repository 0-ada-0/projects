using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Extensions.Logging;

namespace DC2016.Admin.Common
{
    public class Log
    {
        public static ILogger Logger { get; }

        static Log()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole().AddDebug().AddNLog();

            Logger = loggerFactory.CreateLogger<Program>();
        }
    }
}
