using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Configs
{
    /// <summary>
    /// 获取项目配置文件路径
    /// </summary>
    public class GetConfigPath
    {
        private static string projectName = "dc2016";

        private static string LinuxLogPath = @"/var/webos/logs/";
        private static string WindowsLogPath = @"C:\webos\logs\";

        private static string LinuxWebConfigPath = @"/var/webconfig/";
        private static string WindowsWebConfigPath = @"C:\webconfig\";

        static private bool OsIsWindows()
        {
            string key = "OS";
            IDictionary dict = Environment.GetEnvironmentVariables();
            return dict.Contains(key) && dict[key].ToString().ToLower().Contains("windows");
        }
        
        /// <summary>
        /// 获取日志配置文件路径
        /// </summary>
        /// <returns></returns>
        static public string GetLogConfigPath()
        {
            return Path.Combine(GetWebConfigPath(), "nlog.config");
        }

        /// <summary>
        /// 获取 WebConfig 配置文件路径
        /// </summary>
        /// <returns></returns>
        static public string GetWebConfigPath()
        {
            return Path.Combine(OsIsWindows() ? WindowsWebConfigPath : LinuxWebConfigPath, projectName, "101", "lv1", "conf");
        }

        /// <summary>
        /// 获取邮件模板目录路径
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        static public string GetViewPath(string viewName)
        {
            return Path.Combine(GetWebConfigPath(), "101", "lv1", "EMailTPL", $"{viewName}.cshtml");
        }
    }
}
