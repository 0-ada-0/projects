using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mysqldb.DAL.DBUtility;

namespace mysqldb
{
    public class Class1
    {
        public Class1()
        {
            string sql = "select name from student";
            List<string> results = SqlHelper.ExecuteReader(sql);
            foreach (string result in results)
                Console.WriteLine(result);
        }
    }
}
