using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace IF2017.Admin.Configs
{
    /// <summary>
    /// 获取项目配置文件路径
    /// </summary>
    public class ConfigPathBuilder
    {
        private const string projectName = "if2017";
        private const string LinuxWebConfigPath = @"/var/webconfig/";
        private const string WindowsWebConfigPath = @"C:\webconfig\";

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
            string path = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? WindowsWebConfigPath : LinuxWebConfigPath;
            return Path.Combine(path, projectName, "101", "lv1", "conf");
        }

        /// <summary>
        ///  获取邮件模板目录路径
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        static public string GetViewPath(string viewName)
        {
            return Path.Combine(GetWebConfigPath(), "101", "lv1", "EMailTPL", $"{viewName}.cshtml");
        }
    }
}
