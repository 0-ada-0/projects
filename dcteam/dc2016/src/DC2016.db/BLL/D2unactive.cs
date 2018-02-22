using System;
using System.Collections.Generic;
using System.Linq;
using DC2016.Model;
using static DC2016.DAL.D2unactive;
using DC2016.BLL.Enums;

namespace DC2016.BLL
{
    public partial class D2unactive
    {
        public static DateTime GetLast(string product)
        {
            //AFSQLGetScalar afsql = (AFSQLGetScalar)new AFSQLGetScalar(Operator, tablename).Base.SetTop(1).AddWhere("UAV_Gate", product).SetOrderBy("UAV_Time1 desc").SetFields("UAV_Time1");
            //object rcobj = afsql.Func_GetScalar();
            //if (rcobj != null && rcobj is DBNull == false && rcobj is DateTime)
            //    return (DateTime)rcobj;
            //else
            //    return new DateTime(2000, 1, 1);

            D2unactiveInfo item = Select.WhereUavGate(product).Sort("UavTime1 desc").ToOne();
            if (item != null)
            {
                return item.UavTime1.Value;
            }
            return new DateTime(2000, 1, 1);
        }

        public static DateTime GetLast2(string product)
        {
            //AFSQLGetScalar afsql = (AFSQLGetScalar)new AFSQLGetScalar(Operator, tablename).Base.SetTop(1).AddWhere("UAV_Gate", product).SetOrderBy("UAV_Time2 desc").SetFields("UAV_Time2");
            //object rcobj = afsql.Func_GetScalar();
            //if (rcobj != null && rcobj is DBNull == false && rcobj is DateTime)
            //    return (DateTime)rcobj;
            //else
            //    return new DateTime(2000, 1, 1);

            D2unactiveInfo item = Select.WhereUavGate(product).Sort("UavTime2 desc").ToOne();
            if (item != null)
            {
                return item.UavTime2.Value;
            }
            return new DateTime(2000, 1, 1);
        }

        public static D2unactiveInfo initWithUIDForURSUpgrade(string UAV_Gate, int UAV_Number)
        {
            //AFSQLGetSome afsql = new AFSQLGetSome(Operator, tablename);
            //afsql.AddWhere("UAV_Gate", "=", UAV_Gate);
            //afsql.AddWhere("UAV_Number", "=", UAV_Number);
            //afsql.AddWhere("UAV_Time1", ">", DateTime.Now.AddDays(-2));
            //afsql.SetTop(1);
            //afsql.SetOrderBy("UAV_Time1 desc");
            //List<Hashtable> LI_Data = afsql.Func_Exec();
            //if (LI_Data.Count == 0)
            //    return null;
            //else
            //    return new GUnActive(Operator, LI_Data[0]);

            return Select.WhereUavGate(UAV_Gate)
                .WhereUavNumber(UAV_Number)
                .WhereUavTime1Range(DateTime.Now.AddDays(-2))
                .Sort("UavTime1 desc")
                .ToOne();
        }

        public static D2unactiveInfo initWithUID(string UAV_Gate, int UAV_Number)
        {
            //this.SQL_Fill("UAV_Gate", UAV_Gate, "UAV_Number", UAV_Number);
            //return this;

            return Select.WhereUavGate(UAV_Gate)
                .WhereUavNumber(UAV_Number)
                .ToOne();
        }

        public static List<D2unactiveInfo> GetUnSend(string product, int uavState, int topCount)
        {
            //AFSQLGetSome afsql = new AFSQLGetSome(Operator, tablename);
            //afsql.AddWhere("UAV_State", (int)EUAS.未发送);
            //afsql.AddWhere("UAV_Gate", product);
            //afsql.SetTop(topCount);
            //afsql.SetOrderBy("UAV_Time1 asc");
            //return afsql;

            return Select.WhereUavGate(product)
              .WhereUavState(uavState)
              .Sort("UavTime1 asc")
              .Limit(topCount)
              .ToList();
        }

        public static bool Func_SetState(string uavGUID, int uavState)
        {
            //this.SQL_SOP.ModeOnlyWrite["UAV_State,UAV_Time2"] = new object[] { (int)UAV_State, DateTime.Now };
            //return Operator.RetMsg.Succeed;

            return D2unactive.UpdateDiy(uavGUID)
                .SetUavState(uavState)
                .SetUavTime2(DateTime.Now)
                .ExecuteNonQuery() > 0;
        }

        public static bool Func_SetState(string uavGUID, int uavState, string uavGateSrc)
        {
            //this.SQL_SOP.ModeOnlyWrite["UAV_State,UAV_GateSrc,UAV_Time2"] = new object[] { (int)UAV_State, UAV_GateSrc, DateTime.Now };
            //return Operator.RetMsg.Succeed;

            return D2unactive.UpdateDiy(uavGUID)
                .SetUavState(uavState)
                .SetUavGateSrc(uavGateSrc)
                .SetUavTime2(DateTime.Now)
                .ExecuteNonQuery() > 0;
        }

        public static List<D2unactiveInfo> GetByEmail(string product, string email)
        {
            return GetByEmail(product, email, true);
        }
        public static List<D2unactiveInfo> GetByEmail(string product, string email, bool limittime)
        {
            //int topCount = 1;
            //AFSQLGetSome afsql = new AFSQLGetSome(Operator, tablename);
            ////afsql.AddWhere("UAV_State", (int)EUAS.未发送);
            //afsql.AddWhere("UAV_Gate", product);
            //afsql.AddWhere("UAV_EMail", email);
            //if (limittime)
            //    afsql.AddWhere("UAV_Time1", ">", DateTime.Now.AddDays(-3));//72小时内的（超时48小时由调用方另行判断）
            //afsql.SetTop(topCount);
            //afsql.SetOrderBy("UAV_Time1 desc");
            //return afsql;

            int topCount = 1;
            var build = Select.WhereUavGate(product).WhereUavEMail(email);
            if (limittime)
            {
                build.WhereUavTime1gt(DateTime.Now.AddDays(-3));
            }
            return build.Sort("UavTime1 desc")
              .Limit(topCount)
              .ToList();
        }

        public static D2unactiveInfo GetItemByNumber(int number)
        {
            return Select.WhereUavNumber(number).Limit(1).ToList().FirstOrDefault();
        }
    }

    public partial class D2unactiveSelectBuild
    {
        public D2unactiveSelectBuild WhereUavTime1gt(DateTime begin)
        {
            return base.Where("a.`UavTime1` > {0}", begin) as D2unactiveSelectBuild;
        }
    }
}