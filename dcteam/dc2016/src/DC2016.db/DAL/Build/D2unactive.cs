using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DC2016.Model;
using static DC2016.DAL.SqlHelper;

namespace DC2016.DAL {

	public partial class D2unactive : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`d2unactive`";
			internal static readonly string Field = "a.`UavGUID`, a.`UavEMail`, a.`UavFlag`, a.`UavGate`, a.`UavGateSrc`, a.`UavNumber`, a.`UavState`, a.`UavTime1`, a.`UavTime2`";
			internal static readonly string Sort = "a.`UavGUID`";
			public static readonly string Delete = "DELETE FROM `d2unactive` WHERE ";
			public static readonly string Insert = "INSERT INTO `d2unactive`(`UavGUID`, `UavEMail`, `UavFlag`, `UavGate`, `UavGateSrc`, `UavNumber`, `UavState`, `UavTime1`, `UavTime2`) VALUES(?UavGUID, ?UavEMail, ?UavFlag, ?UavGate, ?UavGateSrc, ?UavNumber, ?UavState, ?UavTime1, ?UavTime2)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(D2unactiveInfo item) {
			return new MySqlParameter[] {
				GetParameter("?UavGUID", MySqlDbType.VarChar, 32, item.UavGUID), 
				GetParameter("?UavEMail", MySqlDbType.VarChar, 64, item.UavEMail), 
				GetParameter("?UavFlag", MySqlDbType.Int32, 11, item.UavFlag), 
				GetParameter("?UavGate", MySqlDbType.VarChar, 32, item.UavGate), 
				GetParameter("?UavGateSrc", MySqlDbType.VarChar, 8, item.UavGateSrc), 
				GetParameter("?UavNumber", MySqlDbType.Int32, 11, item.UavNumber), 
				GetParameter("?UavState", MySqlDbType.Int32, 11, item.UavState), 
				GetParameter("?UavTime1", MySqlDbType.DateTime, -1, item.UavTime1), 
				GetParameter("?UavTime2", MySqlDbType.DateTime, -1, item.UavTime2)};
		}
		public D2unactiveInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as D2unactiveInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new D2unactiveInfo {
				UavGUID = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				UavEMail = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				UavFlag = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				UavGate = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				UavGateSrc = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				UavNumber = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				UavState = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				UavTime1 = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				UavTime2 = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index)};
		}
		public SelectBuild<D2unactiveInfo> Select {
			get { return SelectBuild<D2unactiveInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(string UavGUID) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`UavGUID` = ?UavGUID"), 
				GetParameter("?UavGUID", MySqlDbType.VarChar, 32, UavGUID));
		}

		public int Update(D2unactiveInfo item) {
			return new SqlUpdateBuild(null, item.UavGUID)
				.SetUavEMail(item.UavEMail)
				.SetUavFlag(item.UavFlag)
				.SetUavGate(item.UavGate)
				.SetUavGateSrc(item.UavGateSrc)
				.SetUavNumber(item.UavNumber)
				.SetUavState(item.UavState)
				.SetUavTime1(item.UavTime1)
				.SetUavTime2(item.UavTime2).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected D2unactiveInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(D2unactiveInfo item, string UavGUID) {
				_item = item;
				_where = SqlHelper.Addslashes("`UavGUID` = {0}", UavGUID);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 DC2016.DAL.D2unactive.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("DC2016.DAL.D2unactive.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetUavEMail(string value) {
				if (_item != null) _item.UavEMail = value;
				return this.Set("`UavEMail`", string.Concat("?UavEMail_", _parameters.Count), 
					GetParameter(string.Concat("?UavEMail_", _parameters.Count), MySqlDbType.VarChar, 64, value));
			}
			public SqlUpdateBuild SetUavFlag(int? value) {
				if (_item != null) _item.UavFlag = value;
				return this.Set("`UavFlag`", string.Concat("?UavFlag_", _parameters.Count), 
					GetParameter(string.Concat("?UavFlag_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUavFlagIncrement(int value) {
				if (_item != null) _item.UavFlag += value;
				return this.Set("`UavFlag`", string.Concat("`UavFlag` + ?UavFlag_", _parameters.Count), 
					GetParameter(string.Concat("?UavFlag_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUavGate(string value) {
				if (_item != null) _item.UavGate = value;
				return this.Set("`UavGate`", string.Concat("?UavGate_", _parameters.Count), 
					GetParameter(string.Concat("?UavGate_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetUavGateSrc(string value) {
				if (_item != null) _item.UavGateSrc = value;
				return this.Set("`UavGateSrc`", string.Concat("?UavGateSrc_", _parameters.Count), 
					GetParameter(string.Concat("?UavGateSrc_", _parameters.Count), MySqlDbType.VarChar, 8, value));
			}
			public SqlUpdateBuild SetUavNumber(int? value) {
				if (_item != null) _item.UavNumber = value;
				return this.Set("`UavNumber`", string.Concat("?UavNumber_", _parameters.Count), 
					GetParameter(string.Concat("?UavNumber_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUavNumberIncrement(int value) {
				if (_item != null) _item.UavNumber += value;
				return this.Set("`UavNumber`", string.Concat("`UavNumber` + ?UavNumber_", _parameters.Count), 
					GetParameter(string.Concat("?UavNumber_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUavState(int? value) {
				if (_item != null) _item.UavState = value;
				return this.Set("`UavState`", string.Concat("?UavState_", _parameters.Count), 
					GetParameter(string.Concat("?UavState_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUavStateIncrement(int value) {
				if (_item != null) _item.UavState += value;
				return this.Set("`UavState`", string.Concat("`UavState` + ?UavState_", _parameters.Count), 
					GetParameter(string.Concat("?UavState_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUavTime1(DateTime? value) {
				if (_item != null) _item.UavTime1 = value;
				return this.Set("`UavTime1`", string.Concat("?UavTime1_", _parameters.Count), 
					GetParameter(string.Concat("?UavTime1_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetUavTime2(DateTime? value) {
				if (_item != null) _item.UavTime2 = value;
				return this.Set("`UavTime2`", string.Concat("?UavTime2_", _parameters.Count), 
					GetParameter(string.Concat("?UavTime2_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
		}
		#endregion

		public D2unactiveInfo Insert(D2unactiveInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public D2unactiveInfo GetItem(string UavGUID) {
			return this.Select.Where("a.`UavGUID` = {0}", UavGUID).ToOne();
		}
	}
}