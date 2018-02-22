using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.DAL {

	public partial class Validinfo : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`validinfo`";
			internal static readonly string Field = "a.`pkid`, a.`addtime`, a.`code`, a.`gate`, a.`mobile`, a.`number`, a.`param`, a.`server`, a.`state`, a.`type`, a.`updatetime`, a.`userid`";
			internal static readonly string Sort = "a.`pkid`";
			public static readonly string Delete = "DELETE FROM `validinfo` WHERE ";
			public static readonly string Insert = "INSERT INTO `validinfo`(`addtime`, `code`, `gate`, `mobile`, `number`, `param`, `server`, `state`, `type`, `updatetime`, `userid`) VALUES(?addtime, ?code, ?gate, ?mobile, ?number, ?param, ?server, ?state, ?type, ?updatetime, ?userid); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ValidinfoInfo item) {
			return new MySqlParameter[] {
				GetParameter("?pkid", MySqlDbType.Int32, 11, item.Pkid), 
				GetParameter("?addtime", MySqlDbType.DateTime, -1, item.Addtime), 
				GetParameter("?code", MySqlDbType.VarChar, 10, item.Code), 
				GetParameter("?gate", MySqlDbType.VarChar, 8, item.Gate), 
				GetParameter("?mobile", MySqlDbType.VarChar, 18, item.Mobile), 
				GetParameter("?number", MySqlDbType.Int32, 11, item.Number), 
				GetParameter("?param", MySqlDbType.VarChar, 200, item.Param), 
				GetParameter("?server", MySqlDbType.Int32, 11, item.Server), 
				GetParameter("?state", MySqlDbType.Int32, 11, item.State), 
				GetParameter("?type", MySqlDbType.Int32, 11, item.Type), 
				GetParameter("?updatetime", MySqlDbType.DateTime, -1, item.Updatetime), 
				GetParameter("?userid", MySqlDbType.Int32, 11, item.Userid)};
		}
		public ValidinfoInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ValidinfoInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new ValidinfoInfo {
				Pkid = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Addtime = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Code = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Gate = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Mobile = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Number = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Param = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Server = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				State = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Type = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Updatetime = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Userid = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index)};
		}
		public SelectBuild<ValidinfoInfo> Select {
			get { return SelectBuild<ValidinfoInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(int? Pkid) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`pkid` = ?pkid"), 
				GetParameter("?pkid", MySqlDbType.Int32, 11, Pkid));
		}

		public int Update(ValidinfoInfo item) {
			return new SqlUpdateBuild(null, item.Pkid)
				.SetAddtime(item.Addtime)
				.SetCode(item.Code)
				.SetGate(item.Gate)
				.SetMobile(item.Mobile)
				.SetNumber(item.Number)
				.SetParam(item.Param)
				.SetServer(item.Server)
				.SetState(item.State)
				.SetType(item.Type)
				.SetUpdatetime(item.Updatetime)
				.SetUserid(item.Userid).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ValidinfoInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ValidinfoInfo item, int? Pkid) {
				_item = item;
				_where = SqlHelper.Addslashes("`pkid` = {0}", Pkid);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 DC2016.DAL.Validinfo.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("DC2016.DAL.Validinfo.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetAddtime(DateTime? value) {
				if (_item != null) _item.Addtime = value;
				return this.Set("`addtime`", string.Concat("?addtime_", _parameters.Count), 
					GetParameter(string.Concat("?addtime_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetCode(string value) {
				if (_item != null) _item.Code = value;
				return this.Set("`code`", string.Concat("?code_", _parameters.Count), 
					GetParameter(string.Concat("?code_", _parameters.Count), MySqlDbType.VarChar, 10, value));
			}
			public SqlUpdateBuild SetGate(string value) {
				if (_item != null) _item.Gate = value;
				return this.Set("`gate`", string.Concat("?gate_", _parameters.Count), 
					GetParameter(string.Concat("?gate_", _parameters.Count), MySqlDbType.VarChar, 8, value));
			}
			public SqlUpdateBuild SetMobile(string value) {
				if (_item != null) _item.Mobile = value;
				return this.Set("`mobile`", string.Concat("?mobile_", _parameters.Count), 
					GetParameter(string.Concat("?mobile_", _parameters.Count), MySqlDbType.VarChar, 18, value));
			}
			public SqlUpdateBuild SetNumber(int? value) {
				if (_item != null) _item.Number = value;
				return this.Set("`number`", string.Concat("?number_", _parameters.Count), 
					GetParameter(string.Concat("?number_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetNumberIncrement(int value) {
				if (_item != null) _item.Number += value;
				return this.Set("`number`", string.Concat("`number` + ?number_", _parameters.Count), 
					GetParameter(string.Concat("?number_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetParam(string value) {
				if (_item != null) _item.Param = value;
				return this.Set("`param`", string.Concat("?param_", _parameters.Count), 
					GetParameter(string.Concat("?param_", _parameters.Count), MySqlDbType.VarChar, 200, value));
			}
			public SqlUpdateBuild SetServer(int? value) {
				if (_item != null) _item.Server = value;
				return this.Set("`server`", string.Concat("?server_", _parameters.Count), 
					GetParameter(string.Concat("?server_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetServerIncrement(int value) {
				if (_item != null) _item.Server += value;
				return this.Set("`server`", string.Concat("`server` + ?server_", _parameters.Count), 
					GetParameter(string.Concat("?server_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetState(int? value) {
				if (_item != null) _item.State = value;
				return this.Set("`state`", string.Concat("?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetStateIncrement(int value) {
				if (_item != null) _item.State += value;
				return this.Set("`state`", string.Concat("`state` + ?state_", _parameters.Count), 
					GetParameter(string.Concat("?state_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetType(int? value) {
				if (_item != null) _item.Type = value;
				return this.Set("`type`", string.Concat("?type_", _parameters.Count), 
					GetParameter(string.Concat("?type_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetTypeIncrement(int value) {
				if (_item != null) _item.Type += value;
				return this.Set("`type`", string.Concat("`type` + ?type_", _parameters.Count), 
					GetParameter(string.Concat("?type_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUpdatetime(DateTime? value) {
				if (_item != null) _item.Updatetime = value;
				return this.Set("`updatetime`", string.Concat("?updatetime_", _parameters.Count), 
					GetParameter(string.Concat("?updatetime_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetUserid(int? value) {
				if (_item != null) _item.Userid = value;
				return this.Set("`userid`", string.Concat("?userid_", _parameters.Count), 
					GetParameter(string.Concat("?userid_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUseridIncrement(int value) {
				if (_item != null) _item.Userid += value;
				return this.Set("`userid`", string.Concat("`userid` + ?userid_", _parameters.Count), 
					GetParameter(string.Concat("?userid_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
		}
		#endregion

		public ValidinfoInfo Insert(ValidinfoInfo item) {
			int loc1;
			if (int.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Pkid = loc1;
			return item;
		}

		public ValidinfoInfo GetItem(int? Pkid) {
			return this.Select.Where("a.`pkid` = {0}", Pkid).ToOne();
		}
	}
}