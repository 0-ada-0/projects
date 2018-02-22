using System;

namespace IF2017.Admin.Controllers.Common
{
    public class FuncHelper
    {
        public static bool IsMobile(string mobile)
        {
            return isnum(mobile) && mobile != null && mobile.Length == 11 && mobile[0] == '1';
        }
        public static bool isnum(string strck)
        {
            foreach (char c in strck)
                if (c < '0' || c > '9')
                    return false;
            return true;
        }
        public static string MD5(string toCryString)
        {
            toCryString = toCryString ?? string.Empty;
            using (System.Security.Cryptography.MD5 provider = System.Security.Cryptography.MD5.Create())
            {
                return BitConverter.ToString(provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(toCryString))).Replace("-", "").ToLower();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sourcein"></param>
        /// <returns></returns>
        public static string UTF8MD5(string Sourcein)
        {
            return MD5(Sourcein);
        }
        public static int getAge(string idcard)
        {
            DateTime now = DateTime.Now;
            int year = int.Parse(idcard.Substring(6,4));
            int month = int.Parse(idcard.Substring(10,2));
            int day = int.Parse(idcard.Substring(12,2));
            int age = now.Year - year;
            if (now.Month < month || (now.Month == month && now.Day < day))
                age--;
            return age;
        }
        public static string getUrl(string host,object[]obj)
        {
            string parms = "";
            if (obj.Length % 2 != 0)
                throw new Exception("getUrl pararms error");
            for (int i = 0; i < obj.Length; i += 2)
            {
                if (string.IsNullOrEmpty(parms))
                    parms += "?"+obj[i] + "=" + obj[i + 1];
                else
                    parms += "&"+obj[i] + "=" + obj[i + 1];
            }
            return host + parms;
        }
    }
}
