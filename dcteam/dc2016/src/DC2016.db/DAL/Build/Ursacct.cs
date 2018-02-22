using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DC2016.Model;
using static DC2016.DAL.SqlHelper;

namespace DC2016.DAL {

	public partial class Ursacct : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`ursacct`";
			internal static readonly string Field = "a.`AcctEMail`, a.`AcctNumber`";
			internal static readonly string Sort = "a.`AcctEMail`";
			public static readonly string Delete = "DELETE FROM `ursacct` WHERE ";
			public static readonly string Insert = "INSERT INTO `ursacct`(`AcctEMail`, `AcctNumber`) VALUES(?AcctEMail, ?AcctNumber)";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(UrsacctInfo item) {
			return new MySqlParameter[] {
				GetParameter("?AcctEMail", MySqlDbType.VarChar, 50, item.AcctEMail), 
				GetParameter("?AcctNumber", MySqlDbType.Int32, 11, item.AcctNumber)};
		}
		public UrsacctInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as UrsacctInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new UrsacctInfo {
				AcctEMail = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				AcctNumber = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index)};
		}
		public SelectBuild<UrsacctInfo> Select {
			get { return SelectBuild<UrsacctInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(string AcctEMail) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`AcctEMail` = ?AcctEMail"), 
				GetParameter("?AcctEMail", MySqlDbType.VarChar, 50, AcctEMail));
		}

		public int Update(UrsacctInfo item) {
			return new SqlUpdateBuild(null, item.AcctEMail)
				.SetAcctNumber(item.AcctNumber).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected UrsacctInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(UrsacctInfo item, string AcctEMail) {
				_item = item;
				_where = SqlHelper.Addslashes("`AcctEMail` = {0}", AcctEMail);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 DC2016.DAL.Ursacct.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("DC2016.DAL.Ursacct.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetAcctNumber(int? value) {
				if (_item != null) _item.AcctNumber = value;
				return this.Set("`AcctNumber`", string.Concat("?AcctNumber_", _parameters.Count), 
					GetParameter(string.Concat("?AcctNumber_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetAcctNumberIncrement(int value) {
				if (_item != null) _item.AcctNumber += value;
				return this.Set("`AcctNumber`", string.Concat("`AcctNumber` + ?AcctNumber_", _parameters.Count), 
					GetParameter(string.Concat("?AcctNumber_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
		}
		#endregion

		public UrsacctInfo Insert(UrsacctInfo item) {
			SqlHelper.ExecuteNonQuery(TSQL.Insert, GetParameters(item));
			return item;
		}

		public UrsacctInfo GetItem(string AcctEMail) {
			return this.Select.Where("a.`AcctEMail` = {0}", AcctEMail).ToOne();
		}
	}
}