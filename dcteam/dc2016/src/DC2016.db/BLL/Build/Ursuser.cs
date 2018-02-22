using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class Ursuser {

		protected static readonly DC2016.DAL.Ursuser dal = new DC2016.DAL.Ursuser();
		protected static readonly int itemCacheTimeout;

		static Ursuser() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_Ursuser"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(int? UrsNumber) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(UrsNumber));
			return dal.Delete(UrsNumber);
		}

		public static int Update(UrsuserInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.Ursuser.SqlUpdateBuild UpdateDiy(int? UrsNumber) {
			return UpdateDiy(null, UrsNumber);
		}
		public static DC2016.DAL.Ursuser.SqlUpdateBuild UpdateDiy(UrsuserInfo item, int? UrsNumber) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(UrsNumber));
			return new DC2016.DAL.Ursuser.SqlUpdateBuild(item, UrsNumber);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.Ursuser.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.Ursuser.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Ursuser.Insert(UrsuserInfo item)
		/// </summary>
		[Obsolete]
		public static UrsuserInfo Insert(int? UrsNumber, int? UrsBirthDay, string UrsIDCard, string UrsMobile, int? UrsQQ, DateTime? UrsTime) {
			return Insert(new UrsuserInfo {
				UrsNumber = UrsNumber, 
				UrsBirthDay = UrsBirthDay, 
				UrsIDCard = UrsIDCard, 
				UrsMobile = UrsMobile, 
				UrsQQ = UrsQQ, 
				UrsTime = UrsTime});
		}
		public static UrsuserInfo Insert(UrsuserInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(UrsuserInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_Ursuser_", item.UrsNumber));
		}
		#endregion

		public static UrsuserInfo GetItem(int? UrsNumber) {
			if (UrsNumber == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(UrsNumber);
			string key = string.Concat("DC2016_BLL_Ursuser_", UrsNumber);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new UrsuserInfo(value); } catch { }
			UrsuserInfo item = dal.GetItem(UrsNumber);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<UrsuserInfo> GetItems() {
			return Select.ToList();
		}
		public static UrsuserSelectBuild Select {
			get { return new UrsuserSelectBuild(dal); }
		}
	}
	public partial class UrsuserSelectBuild : SelectBuild<UrsuserInfo, UrsuserSelectBuild> {
		public UrsuserSelectBuild WhereUrsNumber(params int?[] UrsNumber) {
			return this.Where1Or("a.`UrsNumber` = {0}", UrsNumber);
		}
		public UrsuserSelectBuild WhereUrsBirthDay(params int?[] UrsBirthDay) {
			return this.Where1Or("a.`UrsBirthDay` = {0}", UrsBirthDay);
		}
		public UrsuserSelectBuild WhereUrsIDCard(params string[] UrsIDCard) {
			return this.Where1Or("a.`UrsIDCard` = {0}", UrsIDCard);
		}
		public UrsuserSelectBuild WhereUrsMobile(params string[] UrsMobile) {
			return this.Where1Or("a.`UrsMobile` = {0}", UrsMobile);
		}
		public UrsuserSelectBuild WhereUrsQQ(params int?[] UrsQQ) {
			return this.Where1Or("a.`UrsQQ` = {0}", UrsQQ);
		}
		public UrsuserSelectBuild WhereUrsTimeRange(DateTime? begin) {
			return base.Where("a.`UrsTime` >= {0}", begin) as UrsuserSelectBuild;
		}
		public UrsuserSelectBuild WhereUrsTimeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereUrsTimeRange(begin);
			return base.Where("a.`UrsTime` between {0} and {1}", begin, end) as UrsuserSelectBuild;
		}
		protected new UrsuserSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as UrsuserSelectBuild;
		}
		public UrsuserSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}