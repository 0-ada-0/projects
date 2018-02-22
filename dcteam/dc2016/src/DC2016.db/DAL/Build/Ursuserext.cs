using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DC2016.Model;
using static DC2016.DAL.SqlHelper;

namespace DC2016.DAL {

	public partial class Ursuserext : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`ursuserext`";
			internal static readonly string Field = "a.`ExtNumber`, a.`ExtCHTInfo`";
			internal static readonly string Sort = "a.`ExtNumber`";
			public static readonly string Delete = "DELETE FROM `ursuserext` WHERE ";
			public static readonly string Insert = "INSERT INTO `ursuserext`(`ExtNumber`, `ExtCHTInfo`) VALUES(?ExtNumber, ?ExtCHTInfo)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(UrsuserextInfo item) {
			return new MySqlParameter[] {
				GetParameter("?ExtNumber", MySqlDbType.Int32, 11, item.ExtNumber), 
				GetParameter("?ExtCHTInfo", MySqlDbType.Text, -1, item.ExtCHTInfo)};
		}
		public UrsuserextInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as UrsuserextInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new UrsuserextInfo {
				ExtNumber = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				ExtCHTInfo = dr.IsDBNull(++index) ? null : (string)dr.GetString(index)};
		}
		public SelectBuild<UrsuserextInfo> Select {
			get { return SelectBuild<UrsuserextInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(int? ExtNumber) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`ExtNumber` = ?ExtNumber"), 
				GetParameter("?ExtNumber", MySqlDbType.Int32, 11, ExtNumber));
		}

		public int Update(UrsuserextInfo item) {
			return new SqlUpdateBuild(null, item.ExtNumber)
				.SetExtCHTInfo(item.ExtCHTInfo).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected UrsuserextInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(UrsuserextInfo item, int? ExtNumber) {
				_item = item;
				_where = SqlHelper.Addslashes("`ExtNumber` = {0}", ExtNumber);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 DC2016.DAL.Ursuserext.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("DC2016.DAL.Ursuserext.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetExtCHTInfo(string value) {
				if (_item != null) _item.ExtCHTInfo = value;
				return this.Set("`ExtCHTInfo`", string.Concat("?ExtCHTInfo_", _parameters.Count), 
					GetParameter(string.Concat("?ExtCHTInfo_", _parameters.Count), MySqlDbType.Text, -1, value));
			}
		}
		#endregion

		public UrsuserextInfo Insert(UrsuserextInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public UrsuserextInfo GetItem(int? ExtNumber) {
			return this.Select.Where("a.`ExtNumber` = {0}", ExtNumber).ToOne();
		}
	}
}