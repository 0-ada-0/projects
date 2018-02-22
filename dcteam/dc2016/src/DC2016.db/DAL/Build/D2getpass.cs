using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DC2016.Model;
using static DC2016.DAL.SqlHelper;

namespace DC2016.DAL {

	public partial class D2getpass : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`d2getpass`";
			internal static readonly string Field = "a.`GtpsGUID`, a.`GtpsEMail`, a.`GtpsGate`, a.`GtpsIP`, a.`GtpsNumber`, a.`GtpsState`, a.`GtpsTime1`, a.`GtpsTime2`, a.`GtpsType`";
			internal static readonly string Sort = "a.`GtpsGUID`";
			public static readonly string Delete = "DELETE FROM `d2getpass` WHERE ";
			public static readonly string Insert = "INSERT INTO `d2getpass`(`GtpsGUID`, `GtpsEMail`, `GtpsGate`, `GtpsIP`, `GtpsNumber`, `GtpsState`, `GtpsTime1`, `GtpsTime2`, `GtpsType`) VALUES(?GtpsGUID, ?GtpsEMail, ?GtpsGate, ?GtpsIP, ?GtpsNumber, ?GtpsState, ?GtpsTime1, ?GtpsTime2, ?GtpsType)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(D2getpassInfo item) {
			return new MySqlParameter[] {
				GetParameter("?GtpsGUID", MySqlDbType.VarChar, 32, item.GtpsGUID), 
				GetParameter("?GtpsEMail", MySqlDbType.VarChar, 64, item.GtpsEMail), 
				GetParameter("?GtpsGate", MySqlDbType.VarChar, 4, item.GtpsGate), 
				GetParameter("?GtpsIP", MySqlDbType.Int32, 11, item.GtpsIP), 
				GetParameter("?GtpsNumber", MySqlDbType.Int32, 11, item.GtpsNumber), 
				GetParameter("?GtpsState", MySqlDbType.Int32, 11, item.GtpsState), 
				GetParameter("?GtpsTime1", MySqlDbType.DateTime, -1, item.GtpsTime1), 
				GetParameter("?GtpsTime2", MySqlDbType.DateTime, -1, item.GtpsTime2), 
				GetParameter("?GtpsType", MySqlDbType.Int32, 11, item.GtpsType)};
		}
		public D2getpassInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as D2getpassInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new D2getpassInfo {
				GtpsGUID = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				GtpsEMail = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				GtpsGate = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				GtpsIP = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				GtpsNumber = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				GtpsState = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				GtpsTime1 = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				GtpsTime2 = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				GtpsType = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index)};
		}
		public SelectBuild<D2getpassInfo> Select {
			get { return SelectBuild<D2getpassInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(string GtpsGUID) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`GtpsGUID` = ?GtpsGUID"), 
				GetParameter("?GtpsGUID", MySqlDbType.VarChar, 32, GtpsGUID));
		}

		public int Update(D2getpassInfo item) {
			return new SqlUpdateBuild(null, item.GtpsGUID)
				.SetGtpsEMail(item.GtpsEMail)
				.SetGtpsGate(item.GtpsGate)
				.SetGtpsIP(item.GtpsIP)
				.SetGtpsNumber(item.GtpsNumber)
				.SetGtpsState(item.GtpsState)
				.SetGtpsTime1(item.GtpsTime1)
				.SetGtpsTime2(item.GtpsTime2)
				.SetGtpsType(item.GtpsType).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected D2getpassInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(D2getpassInfo item, string GtpsGUID) {
				_item = item;
				_where = SqlHelper.Addslashes("`GtpsGUID` = {0}", GtpsGUID);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 DC2016.DAL.D2getpass.SqlUpdateBuild 误修改，请必须设置 where 条件。");
				return string.Concat("UPDATE ", TSQL.Table, " SET ", _fields.Substring(1), " WHERE ", _where);
			}
			public int ExecuteNonQuery() {
				string sql = this.ToString();
				if (string.IsNullOrEmpty(sql)) return 0;
				return SqlHelper.ExecuteNonQuery(sql, _parameters.ToArray());
			}
			public SqlUpdateBuild Where(string filterFormat, params object[] values) {
				if (!string.IsNullOrEmpty(_where)) _where = string.Concat(_where, " AND ");
				_where = string.Concat(_where, "(", SqlHelper.Addslashes(filterFormat, values), ")");
				return this;
			}
			public SqlUpdateBuild Set(string field, string value, params MySqlParameter[] parms) {
				if (value.IndexOf('\'') != -1) throw new Exception("DC2016.DAL.D2getpass.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetGtpsEMail(string value) {
				if (_item != null) _item.GtpsEMail = value;
				return this.Set("`GtpsEMail`", string.Concat("?GtpsEMail_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsEMail_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetGtpsGate(string value) {
				if (_item != null) _item.GtpsGate = value;
				return this.Set("`GtpsGate`", string.Concat("?GtpsGate_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsGate_", _parameters.Count), MySqlDbType.VarChar, 4, value));
			}
			public SqlUpdateBuild SetGtpsIP(int? value) {
				if (_item != null) _item.GtpsIP = value;
				return this.Set("`GtpsIP`", string.Concat("?GtpsIP_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsIP_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetGtpsIPIncrement(int value) {
				if (_item != null) _item.GtpsIP += value;
				return this.Set("`GtpsIP`", string.Concat("`GtpsIP` + ?GtpsIP_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsIP_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetGtpsNumber(int? value) {
				if (_item != null) _item.GtpsNumber = value;
				return this.Set("`GtpsNumber`", string.Concat("?GtpsNumber_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsNumber_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetGtpsNumberIncrement(int value) {
				if (_item != null) _item.GtpsNumber += value;
				return this.Set("`GtpsNumber`", string.Concat("`GtpsNumber` + ?GtpsNumber_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsNumber_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetGtpsState(int? value) {
				if (_item != null) _item.GtpsState = value;
				return this.Set("`GtpsState`", string.Concat("?GtpsState_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsState_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetGtpsStateIncrement(int value) {
				if (_item != null) _item.GtpsState += value;
				return this.Set("`GtpsState`", string.Concat("`GtpsState` + ?GtpsState_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsState_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetGtpsTime1(DateTime? value) {
				if (_item != null) _item.GtpsTime1 = value;
				return this.Set("`GtpsTime1`", string.Concat("?GtpsTime1_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsTime1_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetGtpsTime2(DateTime? value) {
				if (_item != null) _item.GtpsTime2 = value;
				return this.Set("`GtpsTime2`", string.Concat("?GtpsTime2_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsTime2_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetGtpsType(int? value) {
				if (_item != null) _item.GtpsType = value;
				return this.Set("`GtpsType`", string.Concat("?GtpsType_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsType_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetGtpsTypeIncrement(int value) {
				if (_item != null) _item.GtpsType += value;
				return this.Set("`GtpsType`", string.Concat("`GtpsType` + ?GtpsType_", _parameters.Count), 
					GetParameter(string.Concat("?GtpsType_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
		}
		#endregion

		public D2getpassInfo Insert(D2getpassInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public D2getpassInfo GetItem(string GtpsGUID) {
			return this.Select.Where("a.`GtpsGUID` = {0}", GtpsGUID).ToOne();
		}
	}
}