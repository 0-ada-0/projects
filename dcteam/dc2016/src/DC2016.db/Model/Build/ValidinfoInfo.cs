using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DC2016.Model {

	public partial class ValidinfoInfo {
		#region fields
		private int? _Pkid;
		private DateTime? _Addtime;
		private string _Code;
		private string _Gate;
		private string _Mobile;
		private int? _Number;
		private string _Param;
		private int? _Server;
		private int? _State;
		private int? _Type;
		private DateTime? _Updatetime;
		private int? _Userid;
		#endregion

		public ValidinfoInfo() { }

		#region 独创的序列化，反序列化
		protected static readonly string StringifySplit = "@<Validinfo(Info]?#>";
		public string Stringify() {
			return string.Concat(
				_Pkid == null ? "null" : _Pkid.ToString(), "|",
				_Addtime == null ? "null" : _Addtime.Value.Ticks.ToString(), "|",
				_Code == null ? "null" : _Code.Replace("|", StringifySplit), "|",
				_Gate == null ? "null" : _Gate.Replace("|", StringifySplit), "|",
				_Mobile == null ? "null" : _Mobile.Replace("|", StringifySplit), "|",
				_Number == null ? "null" : _Number.ToString(), "|",
				_Param == null ? "null" : _Param.Replace("|", StringifySplit), "|",
				_Server == null ? "null" : _Server.ToString(), "|",
				_State == null ? "null" : _State.ToString(), "|",
				_Type == null ? "null" : _Type.ToString(), "|",
				_Updatetime == null ? "null" : _Updatetime.Value.Ticks.ToString(), "|",
				_Userid == null ? "null" : _Userid.ToString());
		}
		public ValidinfoInfo(string stringify) {
			string[] ret = stringify.Split(new char[] { '|' }, 12, StringSplitOptions.None);
			if (ret.Length != 12) throw new Exception("格式不正确，ValidinfoInfo：" + stringify);
			if (string.Compare("null", ret[0]) != 0) _Pkid = int.Parse(ret[0]);
			if (string.Compare("null", ret[1]) != 0) _Addtime = new DateTime(long.Parse(ret[1]));
			if (string.Compare("null", ret[2]) != 0) _Code = ret[2].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[3]) != 0) _Gate = ret[3].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[4]) != 0) _Mobile = ret[4].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[5]) != 0) _Number = int.Parse(ret[5]);
			if (string.Compare("null", ret[6]) != 0) _Param = ret[6].Replace(StringifySplit, "|");
			if (string.Compare("null", ret[7]) != 0) _Server = int.Parse(ret[7]);
			if (string.Compare("null", ret[8]) != 0) _State = int.Parse(ret[8]);
			if (string.Compare("null", ret[9]) != 0) _Type = int.Parse(ret[9]);
			if (string.Compare("null", ret[10]) != 0) _Updatetime = new DateTime(long.Parse(ret[10]));
			if (string.Compare("null", ret[11]) != 0) _Userid = int.Parse(ret[11]);
		}
		#endregion

		#region override
		private static Dictionary<string, bool> __jsonIgnore;
		private static object __jsonIgnore_lock = new object();
		public override string ToString() {
			this.Init__jsonIgnore();
			string json = string.Concat(
				__jsonIgnore.ContainsKey("Pkid") ? string.Empty : string.Format(", Pkid : {0}", Pkid == null ? "null" : Pkid.ToString()), 
				__jsonIgnore.ContainsKey("Addtime") ? string.Empty : string.Format(", Addtime : {0}", Addtime == null ? "null" : Addtime.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Code") ? string.Empty : string.Format(", Code : {0}", Code == null ? "null" : string.Format("'{0}'", Code.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Gate") ? string.Empty : string.Format(", Gate : {0}", Gate == null ? "null" : string.Format("'{0}'", Gate.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Mobile") ? string.Empty : string.Format(", Mobile : {0}", Mobile == null ? "null" : string.Format("'{0}'", Mobile.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Number") ? string.Empty : string.Format(", Number : {0}", Number == null ? "null" : Number.ToString()), 
				__jsonIgnore.ContainsKey("Param") ? string.Empty : string.Format(", Param : {0}", Param == null ? "null" : string.Format("'{0}'", Param.Replace("\\", "\\\\").Replace("\r\n", "\\r\\n").Replace("'", "\\'"))), 
				__jsonIgnore.ContainsKey("Server") ? string.Empty : string.Format(", Server : {0}", Server == null ? "null" : Server.ToString()), 
				__jsonIgnore.ContainsKey("State") ? string.Empty : string.Format(", State : {0}", State == null ? "null" : State.ToString()), 
				__jsonIgnore.ContainsKey("Type") ? string.Empty : string.Format(", Type : {0}", Type == null ? "null" : Type.ToString()), 
				__jsonIgnore.ContainsKey("Updatetime") ? string.Empty : string.Format(", Updatetime : {0}", Updatetime == null ? "null" : Updatetime.Value.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds.ToString()), 
				__jsonIgnore.ContainsKey("Userid") ? string.Empty : string.Format(", Userid : {0}", Userid == null ? "null" : Userid.ToString()), " }");
			return string.Concat("{", json.Substring(1));
		}
		public IDictionary ToBson() {
			this.Init__jsonIgnore();
			IDictionary ht = new Hashtable();
			if (!__jsonIgnore.ContainsKey("Pkid")) ht["Pkid"] = Pkid;
			if (!__jsonIgnore.ContainsKey("Addtime")) ht["Addtime"] = Addtime;
			if (!__jsonIgnore.ContainsKey("Code")) ht["Code"] = Code;
			if (!__jsonIgnore.ContainsKey("Gate")) ht["Gate"] = Gate;
			if (!__jsonIgnore.ContainsKey("Mobile")) ht["Mobile"] = Mobile;
			if (!__jsonIgnore.ContainsKey("Number")) ht["Number"] = Number;
			if (!__jsonIgnore.ContainsKey("Param")) ht["Param"] = Param;
			if (!__jsonIgnore.ContainsKey("Server")) ht["Server"] = Server;
			if (!__jsonIgnore.ContainsKey("State")) ht["State"] = State;
			if (!__jsonIgnore.ContainsKey("Type")) ht["Type"] = Type;
			if (!__jsonIgnore.ContainsKey("Updatetime")) ht["Updatetime"] = Updatetime;
			if (!__jsonIgnore.ContainsKey("Userid")) ht["Userid"] = Userid;
			return ht;
		}
		private void Init__jsonIgnore() {
			if (__jsonIgnore == null) {
				lock (__jsonIgnore_lock) {
					if (__jsonIgnore == null) {
						FieldInfo field = typeof(ValidinfoInfo).GetField("JsonIgnore");
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
			ValidinfoInfo item = obj as ValidinfoInfo;
			if (item == null) return false;
			return this.ToString().Equals(item.ToString());
		}
		public override int GetHashCode() {
			return this.ToString().GetHashCode();
		}
		public static bool operator ==(ValidinfoInfo op1, ValidinfoInfo op2) {
			if (object.Equals(op1, null)) return object.Equals(op2, null);
			return op1.Equals(op2);
		}
		public static bool operator !=(ValidinfoInfo op1, ValidinfoInfo op2) {
			return !(op1 == op2);
		}
		public object this[string key] {
			get { return this.GetType().GetProperty(key).GetValue(this); }
			set { this.GetType().GetProperty(key).SetValue(this, value); }
		}
		#endregion

		#region properties
		public int? Pkid {
			get { return _Pkid; }
			set { _Pkid = value; }
		}
		public DateTime? Addtime {
			get { return _Addtime; }
			set { _Addtime = value; }
		}
		public string Code {
			get { return _Code; }
			set { _Code = value; }
		}
		public string Gate {
			get { return _Gate; }
			set { _Gate = value; }
		}
		public string Mobile {
			get { return _Mobile; }
			set { _Mobile = value; }
		}
		public int? Number {
			get { return _Number; }
			set { _Number = value; }
		}
		public string Param {
			get { return _Param; }
			set { _Param = value; }
		}
		public int? Server {
			get { return _Server; }
			set { _Server = value; }
		}
		public int? State {
			get { return _State; }
			set { _State = value; }
		}
		public int? Type {
			get { return _Type; }
			set { _Type = value; }
		}
		public DateTime? Updatetime {
			get { return _Updatetime; }
			set { _Updatetime = value; }
		}
		public int? Userid {
			get { return _Userid; }
			set { _Userid = value; }
		}
		#endregion

		public DC2016.DAL.Validinfo.SqlUpdateBuild UpdateDiy {
			get { return DC2016.BLL.Validinfo.UpdateDiy(this, _Pkid); }
		}
	}
}

