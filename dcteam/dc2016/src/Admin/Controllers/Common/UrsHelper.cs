using DC2016.Admin.Configs;
using DC2016.BLL;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using dywebsdk.Common;
using dywebsdk.Cryptography;

namespace DC2016.Admin.Controllers.Common
{
    public class UrsHelper
    {
        public static string MD5(string toCryString)
        {
            toCryString = toCryString ?? string.Empty;
            using (System.Security.Cryptography.MD5 provider = System.Security.Cryptography.MD5.Create())
            {
                return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(toCryString))).Replace("-", "").ToLower();
            }
        }

        public static string mymd5(string toCryString)
        {
            toCryString = toCryString ?? string.Empty;
            return MD5(MD5(toCryString).Substring(7, 16)).Substring(3, 16);
        }

        public static byte[] Encrypt(string plainText, SymmetricAlgorithm key, byte[] KEY_64, byte[] IV_64)
        {
            var AesTool = new AesFactory(KEY_64, IV_64);

            return Encoding.ASCII.GetBytes(AesTool.Encrypt(plainText));
            //return null;
        }

        public static string Decrypt(byte[] CypherText, SymmetricAlgorithm key, byte[] KEY_64, byte[] IV_64)
        {
            AesFactory AesTool = new AesFactory(KEY_64, IV_64);
            return AesTool.Decrypt(CypherText);
            //return string.Empty;
        }
        public static string Decrypt(string CypherText, SymmetricAlgorithm key, byte[] KEY_64, byte[] IV_64)
        {
            var AesTool = new AesFactory(KEY_64, IV_64);

            return AesTool.Decrypt(CypherText);
            //return string.Empty;
        }

        public static string Lib_Friend_GetName(string acct)
        {
            string[] acctspt = acct.Split('@');
            if (acctspt.Length != 2)
            {
                return string.Empty;
            }
            string domain = acctspt[1].ToLower();
            string[] fFirendListExt = DC2Conf.FirendAcctExt();

            int index = Array.IndexOf(fFirendListExt, domain);
            return index == -1 ? string.Empty : fFirendListExt[index];
        }

        //设置缓存
        public static object Cache_GetObj(string key)
        {
            string valueStr = RedisHelper.Get(key);
            return valueStr == null ? null : JsonConvert.DeserializeObject(valueStr);
        }

        //获取缓存
        public static void Cache_SetObj(string key, object value)
        {
            string valueStr = JsonConvert.SerializeObject(value);
            RedisHelper.Set(key, valueStr);
        }
        public static void Cache_SetObj(string key, object value, int min, int sec)
        {
            string valueStr = JsonConvert.SerializeObject(value);
            RedisHelper.Set(key, valueStr, (int)new TimeSpan(0, min, sec).TotalSeconds);
        }

        //获取模板路径
        public static string GetTPLViewPath(string viewName)
        {
            return $"EMailTPL/{viewName}.cshtml";
        }
    }
}
