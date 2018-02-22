using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog.Extensions.Logging;

namespace IF2017.Admin.Common
{
    public class LogFactory
    {
        public static ILogger Logger { get; }

        static LogFactory()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole().AddDebug().AddNLog();

            Logger = loggerFactory.CreateLogger<Program>();
        }
    }
}
