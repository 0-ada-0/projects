using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DC2016.Model {

	public partial class BaishiRewardInfo {
		#region fields
		private uint? _Id;
		private string _Activeid;
		private string _Gate;
		private int? _Rewarditem;
		private int? _State;
		private string _Tel;
		private DateTime? _Time;
		#endregion

		public BaishiRewardInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<BaishiReward(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Activeid == null ? "null" : _Activeid.Replace("|", StringifySplit), "|",
				_Gate == null ? "null" : _Gate.Replace("|", StringifySplit), "|",
				_Rewarditem == null ? "null" : _Rewarditem.ToString(), "|",
				_State == null ? "null" : _State.ToString(), "|",
				_Tel == null ? "null" : _Tel.Replace("|", StringifySplit), "|",
				_Time == null ? "null" : _Time.Value.Ticks.ToString());
		}
		public BaishiRewardInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 7, StringSplitOptions.None);
			if (ret.Length != 7) throw new Exception("格式不正确，BaishiRewardInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = uint.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Activeid = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) _Gate = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Rewarditem = int.Parse(ret[3]);
			if (string.Compare("null", ret[4]) != 0) _State = int.Parse(ret[4]);
			if (string.Compare("null", ret[5]) != 0) _Tel = ret[5].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[6]) != 0) _Time = new DateTime(long.Parse(ret[6]));
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Activeid") ? string.Empty : string.Format(", Activeid : {0}", Activeid == null ? "null" : string.Format("'{0}'", Activeid.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Gate") ? string.Empty : string.Format(", Gate : {0}", Gate == null ? "null" : string.Format("'{0}'", Gate.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Rewarditem") ? string.Empty : string.Format(", Rewarditem : {0}", Rewarditem == null ? "null" : Rewarditem.ToString()), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : State.ToString()), 
				__jsonIgnore.ContainsKey("Tel") ? string.Empty : string.Format(", Tel : {0}", Tel == null ? "null" : string.Format("'{0}'", Tel.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Time") ? string.Empty : string.Format(", Time : {0}", Time == null ? "null" : Time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Activeid")) ht["Activeid"] = Activeid;
			if (!__jsonIgnore.ContainsKey("Gate")) ht["Gate"] = Gate;
			if (!__jsonIgnore.ContainsKey("Rewarditem")) ht["Rewarditem"] = Rewarditem;
			if (!__jsonIgnore.ContainsKey("State")) ht["State"] = State;
			if (!__jsonIgnore.ContainsKey("Tel")) ht["Tel"] = Tel;
			if (!__jsonIgnore.ContainsKey("Time")) ht["Time"] = Time;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(BaishiRewardInfo).GetField("JsonIgnore");
						__jsonIgnore = new Dictionary<string, bool>();
						if (field != null) {
							string[] fs = string.Concat(field.GetValue(null)).Split(',');
							foreach (string f in fs) if (!string.IsNullOrEmpty(f)) __jsonIgnore[f] = true;
						}
					}
				}
			}
		}
		public override bool Equals(object obj) {
			BaishiRewardInfo item = obj as BaishiRewardInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(BaishiRewardInfo op1, BaishiRewardInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(BaishiRewardInfo op1, BaishiRewardInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public uint? Id {
			get { return _Id; }
			set { _Id = value; }
		}
		/// <summary>
		/// 活动编号
		/// </summary>
		public string Activeid {
			get { return _Activeid; }
			set { _Activeid = value; }
		}
		public string Gate {
			get { return _Gate; }
			set { _Gate = value; }
		}
		/// <summary>
		/// 奖励编号
		/// </summary>
		public int? Rewarditem {
			get { return _Rewarditem; }
			set { _Rewarditem = value; }
		}
		public int? State {
			get { return _State; }
			set { _State = value; }
		}
		public string Tel {
			get { return _Tel; }
			set { _Tel = value; }
		}
		/// <summary>
		/// 记录时间
		/// </summary>
		public DateTime? Time {
			get { return _Time; }
			set { _Time = value; }
		}
		#endregion

		public DC2016.DAL.BaishiReward.SqlUpdateBuild UpdateDiy {
			get { return DC2016.BLL.BaishiReward.UpdateDiy(this, _Id); }
		}
	}
}

