using DC2016.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DC2016.BLL
{
    public partial class Share
    {
        public static ShareInfo hasShareInfo(string guid,int number, int userid,string gate,int server)
        {
            return Select.WhereGuid(guid).WhereUserid(userid).WhereNumber(number).WhereGate(gate).WhereServer(server).ToOne();
        }
        public static ShareInfo hasShareInfo( int number, int userid, string gate, int server)
        {
            return Select.WhereUserid(userid).WhereNumber(number).WhereGate(gate).WhereServer(server).ToOne();
        }
        public static ShareInfo hasShareInfo(string guid, string gate)
        {
            return Select.WhereGuid(guid).WhereGate(gate).OrderBy("time desc").ToOne();
        }
        public static ShareInfo hasShareInfoWhithSahreId( string shareid)
        {
            return Select.WhereShareid(shareid).OrderBy("time desc").ToOne();
        }
        public static int getCount()
        {
            return GetItems().Count;
        }
    }
}
