using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class Shareinfo {

		protected static readonly DC2016.DAL.Shareinfo dal = new DC2016.DAL.Shareinfo();
		protected static readonly int itemCacheTimeout;

		static Shareinfo() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_Shareinfo"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(int? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}

		public static int Update(ShareinfoInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.Shareinfo.SqlUpdateBuild UpdateDiy(int? Id) {
			return UpdateDiy(null, Id);
		}
		public static DC2016.DAL.Shareinfo.SqlUpdateBuild UpdateDiy(ShareinfoInfo item, int? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new DC2016.DAL.Shareinfo.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.Shareinfo.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.Shareinfo.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Shareinfo.Insert(ShareinfoInfo item)
		/// </summary>
		[Obsolete]
		public static ShareinfoInfo Insert(string Extends, string Gate, string Guid, string Ip, int? Number, int? Server, string Shareid, int? State, DateTime? Time, int? Used_number, int? Used_userid, int? Userid) {
			return Insert(new ShareinfoInfo {
				Extends = Extends, 
				Gate = Gate, 
				Guid = Guid, 
				Ip = Ip, 
				Number = Number, 
				Server = Server, 
				Shareid = Shareid, 
				State = State, 
				Time = Time, 
				Used_number = Used_number, 
				Used_userid = Used_userid, 
				Userid = Userid});
		}
		public static ShareinfoInfo Insert(ShareinfoInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ShareinfoInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_Shareinfo_", item.Id));
		}
		#endregion

		public static ShareinfoInfo GetItem(int? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("DC2016_BLL_Shareinfo_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ShareinfoInfo(value); } catch { }
			ShareinfoInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ShareinfoInfo> GetItems() {
			return Select.ToList();
		}
		public static ShareinfoSelectBuild Select {
			get { return new ShareinfoSelectBuild(dal); }
		}
	}
	public partial class ShareinfoSelectBuild : SelectBuild<ShareinfoInfo, ShareinfoSelectBuild> {
		public ShareinfoSelectBuild WhereId(params int?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public ShareinfoSelectBuild WhereExtends(params string[] Extends) {
			return this.Where1Or("a.`Extends` = {0}", Extends);
		}
		public ShareinfoSelectBuild WhereGate(params string[] Gate) {
			return this.Where1Or("a.`Gate` = {0}", Gate);
		}
		public ShareinfoSelectBuild WhereGuid(params string[] Guid) {
			return this.Where1Or("a.`Guid` = {0}", Guid);
		}
		public ShareinfoSelectBuild WhereIp(params string[] Ip) {
			return this.Where1Or("a.`Ip` = {0}", Ip);
		}
		public ShareinfoSelectBuild WhereNumber(params int?[] Number) {
			return this.Where1Or("a.`Number` = {0}", Number);
		}
		public ShareinfoSelectBuild WhereServer(params int?[] Server) {
			return this.Where1Or("a.`Server` = {0}", Server);
		}
		public ShareinfoSelectBuild WhereShareid(params string[] Shareid) {
			return this.Where1Or("a.`Shareid` = {0}", Shareid);
		}
		public ShareinfoSelectBuild WhereState(params int?[] State) {
			return this.Where1Or("a.`State` = {0}", State);
		}
		public ShareinfoSelectBuild WhereTimeRange(DateTime? begin) {
			return base.Where("a.`Time` >= {0}", begin) as ShareinfoSelectBuild;
		}
		public ShareinfoSelectBuild WhereTimeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereTimeRange(begin);
			return base.Where("a.`Time` between {0} and {1}", begin, end) as ShareinfoSelectBuild;
		}
		public ShareinfoSelectBuild WhereUsed_number(params int?[] Used_number) {
			return this.Where1Or("a.`Used_number` = {0}", Used_number);
		}
		public ShareinfoSelectBuild WhereUsed_userid(params int?[] Used_userid) {
			return this.Where1Or("a.`Used_userid` = {0}", Used_userid);
		}
		public ShareinfoSelectBuild WhereUserid(params int?[] Userid) {
			return this.Where1Or("a.`Userid` = {0}", Userid);
		}
		protected new ShareinfoSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ShareinfoSelectBuild;
		}
		public ShareinfoSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}