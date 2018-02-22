using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DC2016.Model;
using static DC2016.DAL.SqlHelper;

namespace DC2016.DAL {

	public partial class Ursuser : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`ursuser`";
			internal static readonly string Field = "a.`UrsNumber`, a.`UrsBirthDay`, a.`UrsIDCard`, a.`UrsMobile`, a.`UrsQQ`, a.`UrsTime`";
			internal static readonly string Sort = "a.`UrsNumber`";
			public static readonly string Delete = "DELETE FROM `ursuser` WHERE ";
			public static readonly string Insert = "INSERT INTO `ursuser`(`UrsNumber`, `UrsBirthDay`, `UrsIDCard`, `UrsMobile`, `UrsQQ`, `UrsTime`) VALUES(?UrsNumber, ?UrsBirthDay, ?UrsIDCard, ?UrsMobile, ?UrsQQ, ?UrsTime)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(UrsuserInfo item) {
			return new MySqlParameter[] {
				GetParameter("?UrsNumber", MySqlDbType.Int32, 11, item.UrsNumber), 
				GetParameter("?UrsBirthDay", MySqlDbType.Int32, 11, item.UrsBirthDay), 
				GetParameter("?UrsIDCard", MySqlDbType.VarChar, 32, item.UrsIDCard), 
				GetParameter("?UrsMobile", MySqlDbType.VarChar, 32, item.UrsMobile), 
				GetParameter("?UrsQQ", MySqlDbType.Int32, 11, item.UrsQQ), 
				GetParameter("?UrsTime", MySqlDbType.DateTime, -1, item.UrsTime)};
		}
		public UrsuserInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as UrsuserInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new UrsuserInfo {
				UrsNumber = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				UrsBirthDay = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				UrsIDCard = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				UrsMobile = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				UrsQQ = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				UrsTime = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index)};
		}
		public SelectBuild<UrsuserInfo> Select {
			get { return SelectBuild<UrsuserInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(int? UrsNumber) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`UrsNumber` = ?UrsNumber"), 
				GetParameter("?UrsNumber", MySqlDbType.Int32, 11, UrsNumber));
		}

		public int Update(UrsuserInfo item) {
			return new SqlUpdateBuild(null, item.UrsNumber)
				.SetUrsBirthDay(item.UrsBirthDay)
				.SetUrsIDCard(item.UrsIDCard)
				.SetUrsMobile(item.UrsMobile)
				.SetUrsQQ(item.UrsQQ)
				.SetUrsTime(item.UrsTime).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected UrsuserInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(UrsuserInfo item, int? UrsNumber) {
				_item = item;
				_where = SqlHelper.Addslashes("`UrsNumber` = {0}", UrsNumber);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 DC2016.DAL.Ursuser.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("DC2016.DAL.Ursuser.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetUrsBirthDay(int? value) {
				if (_item != null) _item.UrsBirthDay = value;
				return this.Set("`UrsBirthDay`", string.Concat("?UrsBirthDay_", _parameters.Count), 
					GetParameter(string.Concat("?UrsBirthDay_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUrsBirthDayIncrement(int value) {
				if (_item != null) _item.UrsBirthDay += value;
				return this.Set("`UrsBirthDay`", string.Concat("`UrsBirthDay` + ?UrsBirthDay_", _parameters.Count), 
					GetParameter(string.Concat("?UrsBirthDay_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUrsIDCard(string value) {
				if (_item != null) _item.UrsIDCard = value;
				return this.Set("`UrsIDCard`", string.Concat("?UrsIDCard_", _parameters.Count), 
					GetParameter(string.Concat("?UrsIDCard_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetUrsMobile(string value) {
				if (_item != null) _item.UrsMobile = value;
				return this.Set("`UrsMobile`", string.Concat("?UrsMobile_", _parameters.Count), 
					GetParameter(string.Concat("?UrsMobile_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetUrsQQ(int? value) {
				if (_item != null) _item.UrsQQ = value;
				return this.Set("`UrsQQ`", string.Concat("?UrsQQ_", _parameters.Count), 
					GetParameter(string.Concat("?UrsQQ_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUrsQQIncrement(int value) {
				if (_item != null) _item.UrsQQ += value;
				return this.Set("`UrsQQ`", string.Concat("`UrsQQ` + ?UrsQQ_", _parameters.Count), 
					GetParameter(string.Concat("?UrsQQ_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUrsTime(DateTime? value) {
				if (_item != null) _item.UrsTime = value;
				return this.Set("`UrsTime`", string.Concat("?UrsTime_", _parameters.Count), 
					GetParameter(string.Concat("?UrsTime_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
		}
		#endregion

		public UrsuserInfo Insert(UrsuserInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public UrsuserInfo GetItem(int? UrsNumber) {
			return this.Select.Where("a.`UrsNumber` = {0}", UrsNumber).ToOne();
		}
	}
}