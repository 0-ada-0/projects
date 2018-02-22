using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mysqldb.DAL.DBUtility;

namespace console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string sql = "select name from student";
            List<string> results = SqlHelper.ExecuteReader(sql);
            foreach (string result in results)
                Console.WriteLine(result);
        }
    }
}
