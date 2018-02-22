using System;
using System.Collections.Generic;
using System.Linq;
using DC2016.Model;

namespace DC2016.BLL
{

    public partial class D2getpass
    {
        //检查在最近X天内有无找回过
        public static bool CheckLast(string gtpsEMail, int xdaysago, int xminutesago)
        {
            //DateTime time1 = DateTime.Now.AddDays(0 - xdaysago);
            //time1 = time1.AddMinutes(0 - xminutesago);
            //DateTime time2 = time1;
            //AFSQLGetScalar afsql = new AFSQLGetScalar(Operator, tablename);
            //afsql.SetFields("GTPS_GUID");
            //afsql.AddWhere("GTPS_EMail", GTPS_EMail);
            //afsql.AddWhere("GTPS_Time1", ">=", time2);
            //object obj = afsql.Func_GetScalar();
            //return obj is string && ((string)obj).Length == 32;

            DateTime time1 = DateTime.Now.AddDays(0 - xdaysago);
            time1 = time1.AddMinutes(0 - xminutesago);
            DateTime time2 = time1;
            return Select.WhereGtpsEMail(gtpsEMail).WhereGtpsTime1Range(time2).Count() > 0;
        }

        public static bool SetState(string gtpsGUID, int gtpsState)
        {
            //this.SQL_SOP.ModeOnlyWrite["GTPS_State,GTPS_Time2"] = new object[] { (int)GTPS_State, DateTime.Now };
            //return Operator.RetMsg.Succeed;

            return UpdateDiy(gtpsGUID).SetGtpsState(gtpsState).SetGtpsTime2(DateTime.Now).ExecuteNonQuery() > 0;
        }
    }

    public partial class D2getpassSelectBuild
    {

    }
}