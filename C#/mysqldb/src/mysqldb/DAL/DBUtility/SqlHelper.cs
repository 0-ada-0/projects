using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace mysqldb.DAL.DBUtility
{
    public class SqlHelper
    {
        private static string connectionString = "server=192.168.181.128;user=root;database=ada;port=3306;password=toor;";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            conn.Open(); 
            return conn;
        }

        public static void ReleaseConnection(MySqlConnection conn)
        {
            conn.Close();
        }

        public static List<string> ExecuteReader(string sqlCommand)
        {
            MySqlConnection conn = GetConnection();
            MySqlCommand cmd = new MySqlCommand(sqlCommand,conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            List<string> results = new List<string>();
            while (reader.Read())
            {
                results.Add(reader[0].ToString());
            }
            ReleaseConnection(conn);
            return results;
        }
    }
}
