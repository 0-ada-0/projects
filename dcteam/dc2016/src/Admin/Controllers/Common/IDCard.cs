using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.Admin.Controllers.Common
{
    public class IDCard
    {
        public enum ESEX
        {
            未定义 = -1,
            女 = 0,
            男 = 1
        }
        //这些变量都有初始值，以免外部调用出现意外
        public readonly string idcard = "";
        public readonly ESEX sex = ESEX.未定义;
        public readonly DateTime birthday = DateTime.MinValue;
        public readonly string local = "";
        public int Age
        {
            get
            {
                DateTime now = DateTime.Now;
                int age = now.Year - birthday.Year - 1;
                if (now.Month > birthday.Month || (now.Month == birthday.Month && now.Day >= birthday.Day))
                    age++;
                return age;
            }
        }
        public ESEX Sex
        {
            get
            {
                return sex;// ismale() ? ESEX.男 : ESEX.女;
            }
        }
        public string IDcard
        {
            get { return idcard; }
        }
        public int Birthdayint
        {
            get
            {
                if (this.birthday.Year < 1800)
                    return 0;
                return birthday.Year * 10000 + birthday.Month * 100 + birthday.Day;
            }
        }
        public readonly bool Valid = false;
        public IDCard(string idcard)
        {
            this.idcard = idcard;
            if (this.idcard.Length == 15) this.idcard = idcard = this.CID15To18(idcard);
            if (this.idcard.Length != 18)
                return;
            local = this.idcard.Substring(0, 6);
            sex = ismale() ? ESEX.男 : ESEX.女;
            int year = this.Convert_Int32(idcard.Substring(6, 4));
            int month = this.Convert_Int32(idcard.Substring(10, 2));
            int day = this.Convert_Int32(idcard.Substring(12, 2));
            if (func_checkdate(year, month, day) == false) return;// throw new Exception(string.Format("{0},{1},{2},{3}", idcard, year, month, day));
            birthday = new DateTime(year, month, day);
            Valid = true;
        }
        int Convert_Int32(string int32str)
        {
            try
            {
                return Int32.Parse(int32str);
            }
            catch
            {
                return -1;
            }
        }
        string CID15To18(string cid)
        {

            string rs = cid.Substring(0, 6) + "19" + cid.Substring(6);
            rs += GetCheckCode(cid);
            return rs;
        }
        string GetCheckCode(string cid)
        {
            string[] check = { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            int[] weight = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
            int rs = 0;
            for (int i = 0; i <= cid.Length - 1; i++)
            {
                rs += int.Parse(cid.Substring(i, 1)) * weight[i];
            }
            rs = rs % 11;
            return check[rs];
        }

        int getage(string IDcard, DateTime now)
        {
            int rc = 0;
            if (IDcard.Length == 15)
                rc = 1900 + Convert_Int32(IDcard.Substring(6, 2));
            if (IDcard.Length == 18)
                rc = Convert_Int32(IDcard.Substring(6, 4));
            return now.Year - rc;
        }
        bool ismale()
        {
            int flag = -1;
            if (IDcard.Length == 15)
                flag = Convert_Int32(IDcard.Substring(13, 1));
            if (IDcard.Length == 18)
                flag = Convert_Int32(IDcard.Substring(16, 1));
            if (flag % 2 == 1)
                return true;
            else
                return false;
        }
        bool func_checkdate(int year, int month, int day)
        {
            if (year < 0)
                return false;
            if (month <= 0 || month > 12)
                return false;
            int daymax = getmonthday(year, month);
            if (day <= 0 || day > daymax)
                return false;
            return true;
        }
        int getmonthday(int year, int month)
        {
            return (month > 12 || month <= 0) ? 0 : (month == 2 ? ((year % 4 == 0 && (year % 100 == 0 ? year % 400 == 0 : true)) ? 29 : 28) : (30 + (month <= 7 ? month : month + 1) % 2));
        }
    }
}
