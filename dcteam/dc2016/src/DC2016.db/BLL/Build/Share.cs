using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class Share {

		protected static readonly DC2016.DAL.Share dal = new DC2016.DAL.Share();
		protected static readonly int itemCacheTimeout;

		static Share() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_Share"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(int? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}

		public static int Update(ShareInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.Share.SqlUpdateBuild UpdateDiy(int? Id) {
			return UpdateDiy(null, Id);
		}
		public static DC2016.DAL.Share.SqlUpdateBuild UpdateDiy(ShareInfo item, int? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new DC2016.DAL.Share.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.Share.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.Share.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Share.Insert(ShareInfo item)
		/// </summary>
		[Obsolete]
		public static ShareInfo Insert(string Extends, string Gate, string Guid, string Ip, int? Number, int? Server, string Shareid, int? State, DateTime? Time, int? Used_number, int? Used_userid, int? Userid) {
			return Insert(new ShareInfo {
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
		public static ShareInfo Insert(ShareInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(ShareInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_Share_", item.Id));
		}
		#endregion

		public static ShareInfo GetItem(int? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("DC2016_BLL_Share_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new ShareInfo(value); } catch { }
			ShareInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<ShareInfo> GetItems() {
			return Select.ToList();
		}
		public static ShareSelectBuild Select {
			get { return new ShareSelectBuild(dal); }
		}
	}
	public partial class ShareSelectBuild : SelectBuild<ShareInfo, ShareSelectBuild> {
		public ShareSelectBuild WhereId(params int?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public ShareSelectBuild WhereGate(params string[] Gate) {
			return this.Where1Or("a.`Gate` = {0}", Gate);
		}
		public ShareSelectBuild WhereGuid(params string[] Guid) {
			return this.Where1Or("a.`Guid` = {0}", Guid);
		}
		public ShareSelectBuild WhereIp(params string[] Ip) {
			return this.Where1Or("a.`Ip` = {0}", Ip);
		}
		public ShareSelectBuild WhereNumber(params int?[] Number) {
			return this.Where1Or("a.`Number` = {0}", Number);
		}
		public ShareSelectBuild WhereServer(params int?[] Server) {
			return this.Where1Or("a.`Server` = {0}", Server);
		}
		public ShareSelectBuild WhereShareid(params string[] Shareid) {
			return this.Where1Or("a.`Shareid` = {0}", Shareid);
		}
		public ShareSelectBuild WhereState(params int?[] State) {
			return this.Where1Or("a.`State` = {0}", State);
		}
		public ShareSelectBuild WhereTimeRange(DateTime? begin) {
			return base.Where("a.`Time` >= {0}", begin) as ShareSelectBuild;
		}
		public ShareSelectBuild WhereTimeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereTimeRange(begin);
			return base.Where("a.`Time` between {0} and {1}", begin, end) as ShareSelectBuild;
		}
		public ShareSelectBuild WhereUsed_number(params int?[] Used_number) {
			return this.Where1Or("a.`Used_number` = {0}", Used_number);
		}
		public ShareSelectBuild WhereUsed_userid(params int?[] Used_userid) {
			return this.Where1Or("a.`Used_userid` = {0}", Used_userid);
		}
		public ShareSelectBuild WhereUserid(params int?[] Userid) {
			return this.Where1Or("a.`Userid` = {0}", Userid);
		}
		protected new ShareSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as ShareSelectBuild;
		}
		public ShareSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}