using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class BaishiReward {

		protected static readonly DC2016.DAL.BaishiReward dal = new DC2016.DAL.BaishiReward();
		protected static readonly int itemCacheTimeout;

		static BaishiReward() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_BaishiReward"], out itemCacheTimeout))
				int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT"], out itemCacheTimeout);
		}

		#region delete, update, insert

		public static int Delete(uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(GetItem(Id));
			return dal.Delete(Id);
		}
		public static int DeleteByGateAndTel(string Gate, string Tel) {
			return dal.DeleteByGateAndTel(Gate, Tel);
		}

		public static int Update(BaishiRewardInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.BaishiReward.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static DC2016.DAL.BaishiReward.SqlUpdateBuild UpdateDiy(BaishiRewardInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new DC2016.DAL.BaishiReward.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.BaishiReward.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.BaishiReward.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 BaishiReward.Insert(BaishiRewardInfo item)
		/// </summary>
		[Obsolete]
		public static BaishiRewardInfo Insert(string Activeid, string Gate, int? Rewarditem, int? State, string Tel, DateTime? Time) {
			return Insert(new BaishiRewardInfo {
				Activeid = Activeid, 
				Gate = Gate, 
				Rewarditem = Rewarditem, 
				State = State, 
				Tel = Tel, 
				Time = Time});
		}
		public static BaishiRewardInfo Insert(BaishiRewardInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(BaishiRewardInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_BaishiReward_", item.Id));
			RedisHelper.Remove(string.Concat("DC2016_BLL_BaishiRewardByGateAndTel_", item.Gate, "_,_", item.Tel));
		}
		#endregion

		public static BaishiRewardInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("DC2016_BLL_BaishiReward_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new BaishiRewardInfo(value); } catch { }
			BaishiRewardInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static BaishiRewardInfo GetItemByGateAndTel(string Gate, string Tel) {
			if (Gate == null || Tel == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByGateAndTel(Gate, Tel);
			string key = string.Concat("DC2016_BLL_BaishiRewardByGateAndTel_", Gate, "_,_", Tel);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new BaishiRewardInfo(value); } catch { }
			BaishiRewardInfo item = dal.GetItemByGateAndTel(Gate, Tel);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<BaishiRewardInfo> GetItems() {
			return Select.ToList();
		}
		public static BaishiRewardSelectBuild Select {
			get { return new BaishiRewardSelectBuild(dal); }
		}
	}
	public partial class BaishiRewardSelectBuild : SelectBuild<BaishiRewardInfo, BaishiRewardSelectBuild> {
		public BaishiRewardSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public BaishiRewardSelectBuild WhereActiveid(params string[] Activeid) {
			return this.Where1Or("a.`Activeid` = {0}", Activeid);
		}
		public BaishiRewardSelectBuild WhereGate(params string[] Gate) {
			return this.Where1Or("a.`Gate` = {0}", Gate);
		}
		public BaishiRewardSelectBuild WhereRewarditem(params int?[] Rewarditem) {
			return this.Where1Or("a.`Rewarditem` = {0}", Rewarditem);
		}
		public BaishiRewardSelectBuild WhereState(params int?[] State) {
			return this.Where1Or("a.`State` = {0}", State);
		}
		public BaishiRewardSelectBuild WhereTel(params string[] Tel) {
			return this.Where1Or("a.`Tel` = {0}", Tel);
		}
		public BaishiRewardSelectBuild WhereTimeRange(DateTime? begin) {
			return base.Where("a.`Time` >= {0}", begin) as BaishiRewardSelectBuild;
		}
		public BaishiRewardSelectBuild WhereTimeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereTimeRange(begin);
			return base.Where("a.`Time` between {0} and {1}", begin, end) as BaishiRewardSelectBuild;
		}
		protected new BaishiRewardSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as BaishiRewardSelectBuild;
		}
		public BaishiRewardSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}