using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DC2016.Model {

	public partial class ShareinfoInfo {
		#region fields
		private int? _Id;
		private string _Extends;
		private string _Gate;
		private string _Guid;
		private string _Ip;
		private int? _Number;
		private int? _Server;
		private string _Shareid;
		private int? _State;
		private DateTime? _Time;
		private int? _Used_number;
		private int? _Used_userid;
		private int? _Userid;
		#endregion

		public ShareinfoInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Shareinfo(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Id == null ? "null" : _Id.ToString(), "|",
				_Extends == null ? "null" : _Extends.Replace("|", StringifySplit), "|",
				_Gate == null ? "null" : _Gate.Replace("|", StringifySplit), "|",
				_Guid == null ? "null" : _Guid.Replace("|", StringifySplit), "|",
				_Ip == null ? "null" : _Ip.Replace("|", StringifySplit), "|",
				_Number == null ? "null" : _Number.ToString(), "|",
				_Server == null ? "null" : _Server.ToString(), "|",
				_Shareid == null ? "null" : _Shareid.Replace("|", StringifySplit), "|",
				_State == null ? "null" : _State.ToString(), "|",
				_Time == null ? "null" : _Time.Value.Ticks.ToString(), "|",
				_Used_number == null ? "null" : _Used_number.ToString(), "|",
				_Used_userid == null ? "null" : _Used_userid.ToString(), "|",
				_Userid == null ? "null" : _Userid.ToString());
		}
		public ShareinfoInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 13, StringSplitOptions.None);
			if (ret.Length != 13) throw new Exception("格式不正确，ShareinfoInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Id = int.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Extends = ret[1].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[2]) != 0) _Gate = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Guid = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _Ip = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _Number = int.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) _Server = int.Parse(ret[6]);
			if (string.Compare("null", ret[7]) != 0) _Shareid = ret[7].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[8]) != 0) _State = int.Parse(ret[8]);
			if (string.Compare("null", ret[9]) != 0) _Time = new DateTime(long.Parse(ret[9]));
			if (string.Compare("null", ret[10]) != 0) _Used_number = int.Parse(ret[10]);
			if (string.Compare("null", ret[11]) != 0) _Used_userid = int.Parse(ret[11]);
			if (string.Compare("null", ret[12]) != 0) _Userid = int.Parse(ret[12]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Id") ? string.Empty : string.Format(", Id : {0}", Id == null ? "null" : Id.ToString()), 
				__jsonIgnore.ContainsKey("Extends") ? string.Empty : string.Format(", Extends : {0}", Extends == null ? "null" : string.Format("'{0}'", Extends.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Gate") ? string.Empty : string.Format(", Gate : {0}", Gate == null ? "null" : string.Format("'{0}'", Gate.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Guid") ? string.Empty : string.Format(", Guid : {0}", Guid == null ? "null" : string.Format("'{0}'", Guid.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Ip") ? string.Empty : string.Format(", Ip : {0}", Ip == null ? "null" : string.Format("'{0}'", Ip.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Number") ? string.Empty : string.Format(", Number : {0}", Number == null ? "null" : Number.ToString()), 
				__jsonIgnore.ContainsKey("Server") ? string.Empty : string.Format(", Server : {0}", Server == null ? "null" : Server.ToString()), 
				__jsonIgnore.ContainsKey("Shareid") ? string.Empty : string.Format(", Shareid : {0}", Shareid == null ? "null" : string.Format("'{0}'", Shareid.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : State.ToString()), 
				__jsonIgnore.ContainsKey("Time") ? string.Empty : string.Format(", Time : {0}", Time == null ? "null" : Time.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Used_number") ? string.Empty : string.Format(", Used_number : {0}", Used_number == null ? "null" : Used_number.ToString()), 
				__jsonIgnore.ContainsKey("Used_userid") ? string.Empty : string.Format(", Used_userid : {0}", Used_userid == null ? "null" : Used_userid.ToString()), 
				__jsonIgnore.ContainsKey("Userid") ? string.Empty : string.Format(", Userid : {0}", Userid == null ? "null" : Userid.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Id")) ht["Id"] = Id;
			if (!__jsonIgnore.ContainsKey("Extends")) ht["Extends"] = Extends;
			if (!__jsonIgnore.ContainsKey("Gate")) ht["Gate"] = Gate;
			if (!__jsonIgnore.ContainsKey("Guid")) ht["Guid"] = Guid;
			if (!__jsonIgnore.ContainsKey("Ip")) ht["Ip"] = Ip;
			if (!__jsonIgnore.ContainsKey("Number")) ht["Number"] = Number;
			if (!__jsonIgnore.ContainsKey("Server")) ht["Server"] = Server;
			if (!__jsonIgnore.ContainsKey("Shareid")) ht["Shareid"] = Shareid;
			if (!__jsonIgnore.ContainsKey("State")) ht["State"] = State;
			if (!__jsonIgnore.ContainsKey("Time")) ht["Time"] = Time;
			if (!__jsonIgnore.ContainsKey("Used_number")) ht["Used_number"] = Used_number;
			if (!__jsonIgnore.ContainsKey("Used_userid")) ht["Used_userid"] = Used_userid;
			if (!__jsonIgnore.ContainsKey("Userid")) ht["Userid"] = Userid;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(ShareinfoInfo).GetField("JsonIgnore");
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
			ShareinfoInfo item = obj as ShareinfoInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(ShareinfoInfo op1, ShareinfoInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(ShareinfoInfo op1, ShareinfoInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public int? Id {
			get { return _Id; }
			set { _Id = value; }
		}
		public string Extends {
			get { return _Extends; }
			set { _Extends = value; }
		}
		public string Gate {
			get { return _Gate; }
			set { _Gate = value; }
		}
		public string Guid {
			get { return _Guid; }
			set { _Guid = value; }
		}
		public string Ip {
			get { return _Ip; }
			set { _Ip = value; }
		}
		public int? Number {
			get { return _Number; }
			set { _Number = value; }
		}
		public int? Server {
			get { return _Server; }
			set { _Server = value; }
		}
		public string Shareid {
			get { return _Shareid; }
			set { _Shareid = value; }
		}
		public int? State {
			get { return _State; }
			set { _State = value; }
		}
		public DateTime? Time {
			get { return _Time; }
			set { _Time = value; }
		}
		public int? Used_number {
			get { return _Used_number; }
			set { _Used_number = value; }
		}
		public int? Used_userid {
			get { return _Used_userid; }
			set { _Used_userid = value; }
		}
		public int? Userid {
			get { return _Userid; }
			set { _Userid = value; }
		}
		#endregion

		public DC2016.DAL.Shareinfo.SqlUpdateBuild UpdateDiy {
			get { return DC2016.BLL.Shareinfo.UpdateDiy(this, _Id); }
		}
	}
}

