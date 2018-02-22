using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using DC2016.Model;

namespace DC2016.BLL {

	public partial class Raffle_info_tbl {

		protected static readonly DC2016.DAL.Raffle_info_tbl dal = new DC2016.DAL.Raffle_info_tbl();
		protected static readonly int itemCacheTimeout;

		static Raffle_info_tbl() {
			var ini = IniHelper.LoadIni(@"../web.config");
			if (ini.ContainsKey("appSettings") && !int.TryParse(ini["appSettings"]["DC2016_ITEM_CACHE_TIMEOUT_Raffle_info_tbl"], out itemCacheTimeout))
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

		public static int Update(Raffle_info_tblInfo item) {
			if (itemCacheTimeout > 0) RemoveCache(item);
			return dal.Update(item);
		}
		public static DC2016.DAL.Raffle_info_tbl.SqlUpdateBuild UpdateDiy(uint? Id) {
			return UpdateDiy(null, Id);
		}
		public static DC2016.DAL.Raffle_info_tbl.SqlUpdateBuild UpdateDiy(Raffle_info_tblInfo item, uint? Id) {
			if (itemCacheTimeout > 0) RemoveCache(item != null ? item : GetItem(Id));
			return new DC2016.DAL.Raffle_info_tbl.SqlUpdateBuild(item, Id);
		}
		/// <summary>
		/// 用于批量更新
		/// </summary>
		public static DC2016.DAL.Raffle_info_tbl.SqlUpdateBuild UpdateDiyDangerous {
			get { return new DC2016.DAL.Raffle_info_tbl.SqlUpdateBuild(); }
		}

		/// <summary>
		/// 适用字段较少的表；避规后续改表风险，字段数较大请改用 Raffle_info_tbl.Insert(Raffle_info_tblInfo item)
		/// </summary>
		[Obsolete]
		public static Raffle_info_tblInfo Insert(string Activeid, string Gate, int? Rewarditem, int? State, string Tel, DateTime? Time) {
			return Insert(new Raffle_info_tblInfo {
				Activeid = Activeid, 
				Gate = Gate, 
				Rewarditem = Rewarditem, 
				State = State, 
				Tel = Tel, 
				Time = Time});
		}
		public static Raffle_info_tblInfo Insert(Raffle_info_tblInfo item) {
			item = dal.Insert(item);
			if (itemCacheTimeout > 0) RemoveCache(item);
			return item;
		}
		private static void RemoveCache(Raffle_info_tblInfo item) {
			if (item == null) return;
			RedisHelper.Remove(string.Concat("DC2016_BLL_Raffle_info_tbl_", item.Id));
			RedisHelper.Remove(string.Concat("DC2016_BLL_Raffle_info_tblByGateAndTel_", item.Gate, "_,_", item.Tel));
		}
		#endregion

		public static Raffle_info_tblInfo GetItem(uint? Id) {
			if (Id == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItem(Id);
			string key = string.Concat("DC2016_BLL_Raffle_info_tbl_", Id);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Raffle_info_tblInfo(value); } catch { }
			Raffle_info_tblInfo item = dal.GetItem(Id);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}
		public static Raffle_info_tblInfo GetItemByGateAndTel(string Gate, string Tel) {
			if (Gate == null || Tel == null) return null;
			if (itemCacheTimeout <= 0) return dal.GetItemByGateAndTel(Gate, Tel);
			string key = string.Concat("DC2016_BLL_Raffle_info_tblByGateAndTel_", Gate, "_,_", Tel);
			string value = RedisHelper.Get(key);
			if (!string.IsNullOrEmpty(value))
				try { return new Raffle_info_tblInfo(value); } catch { }
			Raffle_info_tblInfo item = dal.GetItemByGateAndTel(Gate, Tel);
			if (item == null) return null;
			RedisHelper.Set(key, item.Stringify(), itemCacheTimeout);
			return item;
		}

		public static List<Raffle_info_tblInfo> GetItems() {
			return Select.ToList();
		}
		public static Raffle_info_tblSelectBuild Select {
			get { return new Raffle_info_tblSelectBuild(dal); }
		}
	}
	public partial class Raffle_info_tblSelectBuild : SelectBuild<Raffle_info_tblInfo, Raffle_info_tblSelectBuild> {
		public Raffle_info_tblSelectBuild WhereId(params uint?[] Id) {
			return this.Where1Or("a.`Id` = {0}", Id);
		}
		public Raffle_info_tblSelectBuild WhereActiveid(params string[] Activeid) {
			return this.Where1Or("a.`Activeid` = {0}", Activeid);
		}
		public Raffle_info_tblSelectBuild WhereGate(params string[] Gate) {
			return this.Where1Or("a.`Gate` = {0}", Gate);
		}
		public Raffle_info_tblSelectBuild WhereRewarditem(params int?[] Rewarditem) {
			return this.Where1Or("a.`Rewarditem` = {0}", Rewarditem);
		}
		public Raffle_info_tblSelectBuild WhereState(params int?[] State) {
			return this.Where1Or("a.`State` = {0}", State);
		}
		public Raffle_info_tblSelectBuild WhereTel(params string[] Tel) {
			return this.Where1Or("a.`Tel` = {0}", Tel);
		}
		public Raffle_info_tblSelectBuild WhereTimeRange(DateTime? begin) {
			return base.Where("a.`Time` >= {0}", begin) as Raffle_info_tblSelectBuild;
		}
		public Raffle_info_tblSelectBuild WhereTimeRange(DateTime? begin, DateTime? end) {
			if (end == null) return WhereTimeRange(begin);
			return base.Where("a.`Time` between {0} and {1}", begin, end) as Raffle_info_tblSelectBuild;
		}
		protected new Raffle_info_tblSelectBuild Where1Or(string filterFormat, Array values) {
			return base.Where1Or(filterFormat, values) as Raffle_info_tblSelectBuild;
		}
		public Raffle_info_tblSelectBuild(IDAL dal) : base(dal, SqlHelper.Instance) { }
	}
}