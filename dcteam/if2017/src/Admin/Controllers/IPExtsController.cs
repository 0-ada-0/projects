using IF2017.Admin.Configs;
using IF2017.Admin.Controllers.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;

namespace IF2017.Admin.Controllers
{
    /// <summary>
    /// 获取IP位置（测试）
    /// </summary>
    /// <remark name="ips">要查询的ips，如：ips=124.20.2.2,124.2.26.124</remark>
    /// <returns></returns>
    [Route("common")]
    public partial class IPExtsController : BaseController<IPExtsController>
    {
        /// <summary>
        /// 获取IP位置（测试）
        /// </summary>
        /// <param name="ips">要查询的ips，如：ips=124.20.2.2,124.2.26.124</param>
        /// <returns></returns>
        [HttpGet(@"test/getipposition")]
        public IActionResult test_getIpPosition([FromQuery]string ips)
        {
            return getIpPosition(ips);
        }
        public IPExtsController(ILogger<IPExtsController> logger) : base(logger) { }
        public static bool EnableFileWatch = false;

        private static int offset;
        private static uint[] index = new uint[65536];
        private static byte[] dataBuffer;
        private static byte[] indexBuffer;
        private static long lastModifyTime = 0L;
        private static string ipFile;
        private static readonly object @lock = new object();
        public static bool isloadfile = false;
        public static string[] info = { "country", "province", "city", "town", "operator", "long", "lat" };
        /// <summary>
        /// 获取IP位置
        /// </summary>
        /// <param name="ips">要查询的ips，如：ips=124.20.2.2,124.2.26.124</param>
        /// <returns></returns>
        [HttpGet(@"getipposition")]
        public IActionResult getIpPosition([FromQuery]string ips)
        {
            Hashtable ht = new Hashtable();
            if (string.IsNullOrEmpty(ips))
            {
                return this.FuncResult(new APIReturn(10000, $"参数错误(ips)"));
            }
            Load(IFConfigReader.IPFindFile);
            string[] iparrary = ips.Split(',');
            string[] findstr;
            foreach (string ip in iparrary)
            {
                Hashtable ipht = new Hashtable();
                if (isTrueIP(ip))
                {
                    findstr = IPExtsController.Find(ip);
                    for (int i = 0; i < info.Length; i++)
                    {
                        ipht[info[i]] = findstr[i];
                    }
                    ht[ip] = ipht;
                }
                else
                    return this.FuncResult(new APIReturn(10000, $"参数错误({ip})"));

            }
            return this.FuncResult(new APIReturn(0, "查询成功", ht));
        }
        private bool isTrueIP(string ip)
        {
            string[] ipstr = ip.Split('.');
            if (ipstr.Length != 4 || isIntNotBetween(ipstr[0], 1, 255) || isIntNotBetween(ipstr[1], 0, 255) || isIntNotBetween(ipstr[2], 0, 255)
                || isIntNotBetween(ipstr[3], 0, 255))
                return false;
            return true;
        }
        private bool isIntNotBetween(string n, int lower, int bigger)
        {
            int number;
            if (!int.TryParse(n, out number))
                return true;
            if (number < lower || number > bigger)
                return true;
            return false;
        }

        public static void Load(string filename)
        {
            if (!isloadfile)
            {
                ipFile = new FileInfo(filename).FullName;
                Load();
                isloadfile = true;
            }

        }

        public static string[] Find(string ip)
        {
            lock (@lock)
            {
                var ips = ip.Split('.');
                var ip_prefix_value = Int32.Parse(ips[0]) * 256 + Int32.Parse(ips[1]);
                long ip2long_value = BytesToLong(Byte.Parse(ips[0]), Byte.Parse(ips[1]), Byte.Parse(ips[2]),
                    Byte.Parse(ips[3]));
                var start = index[ip_prefix_value];
                var max_comp_len = offset - 262144 - 4;
                long index_offset = -1;
                var index_length = -1;
                byte b = 0;
                for (start = start * 9 + 262144; start < max_comp_len; start += 9)
                {
                    if (
                        BytesToLong(indexBuffer[start + 0], indexBuffer[start + 1], indexBuffer[start + 2],
                            indexBuffer[start + 3]) >= ip2long_value)
                    {
                        index_offset = BytesToLong(b, indexBuffer[start + 6], indexBuffer[start + 5],
                            indexBuffer[start + 4]);
                        index_length = (0xFF & indexBuffer[start + 7] << 8) + indexBuffer[start + 8];
                        break;
                    }
                }
                var areaBytes = new byte[index_length];
                Array.Copy(dataBuffer, offset + (int)index_offset - 262144, areaBytes, 0, index_length);
                return Encoding.UTF8.GetString(areaBytes).Split('\t');
            }
        }
        private static void Load()
        {
            lock (@lock)
            {
                var file = new FileInfo(ipFile);
                lastModifyTime = file.LastWriteTime.Ticks;
                try
                {
                    dataBuffer = new byte[file.Length];
                    using (var fin = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                    {
                        fin.Read(dataBuffer, 0, dataBuffer.Length);
                    }

                    var indexLength = BytesToLong(dataBuffer[0], dataBuffer[1], dataBuffer[2], dataBuffer[3]);
                    indexBuffer = new byte[indexLength];
                    Array.Copy(dataBuffer, 4, indexBuffer, 0, (int)indexLength);
                    offset = (int)indexLength;

                    for (var i = 0; i < 256; i++)
                    {
                        for (var j = 0; j < 256; j++)
                        {
                            index[i * 256 + j] = BytesToLong(
                                indexBuffer[(i * 256 + j) * 4 + 3],
                                indexBuffer[(i * 256 + j) * 4 + 2],
                                indexBuffer[(i * 256 + j) * 4 + 1],
                                indexBuffer[(i * 256 + j) * 4]);
                        }
                    }
                }
                catch (Exception e) { }
            }
        }

        private static uint BytesToLong(byte a, byte b, byte c, byte d)
        {
            return ((uint)a << 24) | ((uint)b << 16) | ((uint)c << 8) | d;
        }
    }

}