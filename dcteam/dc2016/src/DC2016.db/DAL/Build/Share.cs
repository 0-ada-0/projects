using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.DAL {

	public partial class Share : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`share`";
			internal static readonly string Field = "a.`id`, a.`extends`, a.`gate`, a.`guid`, a.`ip`, a.`number`, a.`server`, a.`shareid`, a.`state`, a.`time`, a.`used_number`, a.`used_userid`, a.`userid`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `share` WHERE ";
			public static readonly string Insert = "INSERT INTO `share`(`extends`, `gate`, `guid`, `ip`, `number`, `server`, `shareid`, `state`, `time`, `used_number`, `used_userid`, `userid`) VALUES(?extends, ?gate, ?guid, ?ip, ?number, ?server, ?shareid, ?state, ?time, ?used_number, ?used_userid, ?userid); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(ShareInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.Int32, 11, item.Id), 
				GetParameter("?extends", MySqlDbType.VarChar, 1000, item.Extends), 
				GetParameter("?gate", MySqlDbType.VarChar, 8, item.Gate), 
				GetParameter("?guid", MySqlDbType.VarChar, 32, item.Guid), 
				GetParameter("?ip", MySqlDbType.VarChar, 32, item.Ip), 
				GetParameter("?number", MySqlDbType.Int32, 11, item.Number), 
				GetParameter("?server", MySqlDbType.Int32, 11, item.Server), 
				GetParameter("?shareid", MySqlDbType.VarChar, 16, item.Shareid), 
				GetParameter("?state", MySqlDbType.Int32, 11, item.State), 
				GetParameter("?time", MySqlDbType.DateTime, -1, item.Time), 
				GetParameter("?used_number", MySqlDbType.Int32, 11, item.Used_number), 
				GetParameter("?used_userid", MySqlDbType.Int32, 11, item.Used_userid), 
				GetParameter("?userid", MySqlDbType.Int32, 11, item.Userid)};
		}
		public ShareInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as ShareInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new ShareInfo {
				Id = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Extends = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Gate = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Guid = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Ip = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Number = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Server = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Shareid = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				State = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index), 
				Used_number = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Used_userid = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Userid = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index)};
		}
		public SelectBuild<ShareInfo> Select {
			get { return SelectBuild<ShareInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(int? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.Int32, 11, Id));
		}

		public int Update(ShareInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetExtends(item.Extends)
				.SetGate(item.Gate)
				.SetGuid(item.Guid)
				.SetIp(item.Ip)
				.SetNumber(item.Number)
				.SetServer(item.Server)
				.SetShareid(item.Shareid)
				.SetState(item.State)
				.SetTime(item.Time)
				.SetUsed_number(item.Used_number)
				.SetUsed_userid(item.Used_userid)
				.SetUserid(item.Userid).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected ShareInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(ShareInfo item, int? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 DC2016.DAL.Share.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("DC2016.DAL.Share.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetExtends(string value) {
				if (_item != null) _item.Extends = value;
				return this.Set("`extends`", string.Concat("?extends_", _parameters.Count), 
					GetParameter(string.Concat("?extends_", _parameters.Count), MySqlDbType.VarChar, 1000, value));
			}
			public SqlUpdateBuild SetGate(string value) {
				if (_item != null) _item.Gate = value;
				return this.Set("`gate`", string.Concat("?gate_", _parameters.Count), 
					GetParameter(string.Concat("?gate_", _parameters.Count), MySqlDbType.VarChar, 8, value));
			}
			public SqlUpdateBuild SetGuid(string value) {
				if (_item != null) _item.Guid = value;
				return this.Set("`guid`", string.Concat("?guid_", _parameters.Count), 
					GetParameter(string.Concat("?guid_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetIp(string value) {
				if (_item != null) _item.Ip = value;
				return this.Set("`ip`", string.Concat("?ip_", _parameters.Count), 
					GetParameter(string.Concat("?ip_", _parameters.Count), MySqlDbType.VarChar, 32, value));
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
			public SqlUpdateBuild SetShareid(string value) {
				if (_item != null) _item.Shareid = value;
				return this.Set("`shareid`", string.Concat("?shareid_", _parameters.Count), 
					GetParameter(string.Concat("?shareid_", _parameters.Count), MySqlDbType.VarChar, 16, value));
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
			public SqlUpdateBuild SetTime(DateTime? value) {
				if (_item != null) _item.Time = value;
				return this.Set("`time`", string.Concat("?time_", _parameters.Count), 
					GetParameter(string.Concat("?time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
			public SqlUpdateBuild SetUsed_number(int? value) {
				if (_item != null) _item.Used_number = value;
				return this.Set("`used_number`", string.Concat("?used_number_", _parameters.Count), 
					GetParameter(string.Concat("?used_number_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUsed_numberIncrement(int value) {
				if (_item != null) _item.Used_number += value;
				return this.Set("`used_number`", string.Concat("`used_number` + ?used_number_", _parameters.Count), 
					GetParameter(string.Concat("?used_number_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUsed_userid(int? value) {
				if (_item != null) _item.Used_userid = value;
				return this.Set("`used_userid`", string.Concat("?used_userid_", _parameters.Count), 
					GetParameter(string.Concat("?used_userid_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetUsed_useridIncrement(int value) {
				if (_item != null) _item.Used_userid += value;
				return this.Set("`used_userid`", string.Concat("`used_userid` + ?used_userid_", _parameters.Count), 
					GetParameter(string.Concat("?used_userid_", _parameters.Count), MySqlDbType.Int32, 11, value));
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

		public ShareInfo Insert(ShareInfo item) {
			int loc1;
			if (int.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public ShareInfo GetItem(int? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
	}
}