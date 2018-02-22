using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.DAL {

	public partial class BaishiReward : IDAL {
		#region transact-sql define
		public string Table { get { return TSQL.Table; } }
		public string Field { get { return TSQL.Field; } }
		public string Sort { get { return TSQL.Sort; } }
		internal class TSQL {
			internal static readonly string Table = "`baishiReward`";
			internal static readonly string Field = "a.`id`, a.`activeid`, a.`gate`, a.`rewarditem`, a.`state`, a.`tel`, a.`time`";
			internal static readonly string Sort = "a.`id`";
			public static readonly string Delete = "DELETE FROM `baishiReward` WHERE ";
			public static readonly string Insert = "INSERT INTO `baishiReward`(`activeid`, `gate`, `rewarditem`, `state`, `tel`, `time`) VALUES(?activeid, ?gate, ?rewarditem, ?state, ?tel, ?time); SELECT LAST_INSERT_ID();";
		}
		#endregion

		#region common call
		protected static MySqlParameter GetParameter(string name, MySqlDbType type, int size, object value) {
			MySqlParameter parm = new MySqlParameter(name, type, size);
			parm.Value = value;
			return parm;
		}
		protected static MySqlParameter[] GetParameters(BaishiRewardInfo item) {
			return new MySqlParameter[] {
				GetParameter("?id", MySqlDbType.UInt32, 10, item.Id), 
				GetParameter("?activeid", MySqlDbType.VarChar, 32, item.Activeid), 
				GetParameter("?gate", MySqlDbType.VarChar, 8, item.Gate), 
				GetParameter("?rewarditem", MySqlDbType.Int32, 11, item.Rewarditem), 
				GetParameter("?state", MySqlDbType.Int32, 11, item.State), 
				GetParameter("?tel", MySqlDbType.VarChar, 18, item.Tel), 
				GetParameter("?time", MySqlDbType.DateTime, -1, item.Time)};
		}
		public BaishiRewardInfo GetItem(IDataReader dr) {
			int index = -1;
			return GetItem(dr, ref index) as BaishiRewardInfo;
		}
		public object GetItem(IDataReader dr, ref int index) {
			return new BaishiRewardInfo {
				Id = dr.IsDBNull(++index) ? null : (uint?)dr.GetInt32(index), 
				Activeid = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Gate = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Rewarditem = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				State = dr.IsDBNull(++index) ? null : (int?)dr.GetInt32(index), 
				Tel = dr.IsDBNull(++index) ? null : (string)dr.GetString(index), 
				Time = dr.IsDBNull(++index) ? null : (DateTime?)dr.GetDateTime(index)};
		}
		public SelectBuild<BaishiRewardInfo> Select {
			get { return SelectBuild<BaishiRewardInfo>.From(this, SqlHelper.Instance); }
		}
		#endregion

		public int Delete(uint? Id) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`id` = ?id"), 
				GetParameter("?id", MySqlDbType.UInt32, 10, Id));
		}
		public int DeleteByGateAndTel(string Gate, string Tel) {
			return SqlHelper.ExecuteNonQuery(string.Concat(TSQL.Delete, "`gate` = ?gate AND `tel` = ?tel"), 
				GetParameter("?gate", MySqlDbType.VarChar, 8, Gate), 
				GetParameter("?tel", MySqlDbType.VarChar, 18, Tel));
		}

		public int Update(BaishiRewardInfo item) {
			return new SqlUpdateBuild(null, item.Id)
				.SetActiveid(item.Activeid)
				.SetGate(item.Gate)
				.SetRewarditem(item.Rewarditem)
				.SetState(item.State)
				.SetTel(item.Tel)
				.SetTime(item.Time).ExecuteNonQuery();
		}
		#region class SqlUpdateBuild
		public partial class SqlUpdateBuild {
			protected BaishiRewardInfo _item;
			protected string _fields;
			protected string _where;
			protected List<MySqlParameter> _parameters = new List<MySqlParameter>();
			public SqlUpdateBuild(BaishiRewardInfo item, uint? Id) {
				_item = item;
				_where = SqlHelper.Addslashes("`id` = {0}", Id);
			}
			public SqlUpdateBuild() { }
			public override string ToString() {
				if (string.IsNullOrEmpty(_fields)) return string.Empty;
				if (string.IsNullOrEmpty(_where)) throw new Exception("防止 DC2016.DAL.BaishiReward.SqlUpdateBuild 误修改，请必须设置 where 条件。");
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
				if (value.IndexOf('\'') != -1) throw new Exception("DC2016.DAL.BaishiReward.SqlUpdateBuild 可能存在注入漏洞，不允许传递 ' 给参数 value，若使用正常字符串，请使用参数化传递。");
				_fields = string.Concat(_fields, ", ", field, " = ", value);
				if (parms != null && parms.Length > 0) _parameters.AddRange(parms);
				return this;
			}
			public SqlUpdateBuild SetActiveid(string value) {
				if (_item != null) _item.Activeid = value;
				return this.Set("`activeid`", string.Concat("?activeid_", _parameters.Count), 
					GetParameter(string.Concat("?activeid_", _parameters.Count), MySqlDbType.VarChar, 32, value));
			}
			public SqlUpdateBuild SetGate(string value) {
				if (_item != null) _item.Gate = value;
				return this.Set("`gate`", string.Concat("?gate_", _parameters.Count), 
					GetParameter(string.Concat("?gate_", _parameters.Count), MySqlDbType.VarChar, 8, value));
			}
			public SqlUpdateBuild SetRewarditem(int? value) {
				if (_item != null) _item.Rewarditem = value;
				return this.Set("`rewarditem`", string.Concat("?rewarditem_", _parameters.Count), 
					GetParameter(string.Concat("?rewarditem_", _parameters.Count), MySqlDbType.Int32, 11, value));
			}
			public SqlUpdateBuild SetRewarditemIncrement(int value) {
				if (_item != null) _item.Rewarditem += value;
				return this.Set("`rewarditem`", string.Concat("`rewarditem` + ?rewarditem_", _parameters.Count), 
					GetParameter(string.Concat("?rewarditem_", _parameters.Count), MySqlDbType.Int32, 11, value));
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
			public SqlUpdateBuild SetTel(string value) {
				if (_item != null) _item.Tel = value;
				return this.Set("`tel`", string.Concat("?tel_", _parameters.Count), 
					GetParameter(string.Concat("?tel_", _parameters.Count), MySqlDbType.VarChar, 18, value));
			}
			public SqlUpdateBuild SetTime(DateTime? value) {
				if (_item != null) _item.Time = value;
				return this.Set("`time`", string.Concat("?time_", _parameters.Count), 
					GetParameter(string.Concat("?time_", _parameters.Count), MySqlDbType.DateTime, -1, value));
			}
		}
		#endregion

		public BaishiRewardInfo Insert(BaishiRewardInfo item) {
			uint loc1;
			if (uint.TryParse(string.Concat(SqlHelper.ExecuteScalar(TSQL.Insert, GetParameters(item))), out loc1)) item.Id = loc1;
			return item;
		}

		public BaishiRewardInfo GetItem(uint? Id) {
			return this.Select.Where("a.`id` = {0}", Id).ToOne();
		}
		public BaishiRewardInfo GetItemByGateAndTel(string Gate, string Tel) {
			return this.Select.Where("a.`gate` = {0} AND a.`tel` = {1}", Gate, Tel).ToOne();
		}
	}
}