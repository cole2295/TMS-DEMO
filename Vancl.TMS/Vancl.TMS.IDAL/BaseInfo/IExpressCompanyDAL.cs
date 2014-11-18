using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo;
using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.RangeDefined;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IDAL.BaseInfo
{
    /// <summary>
    /// 基础组织架构数据层服务
    /// </summary>
    public interface IExpressCompanyDAL
    {
        /// <summary>
        /// 根据仓库编码取得物流系统出发地[Vancl,V+统一]
        /// </summary>
        /// <param name="wmsCode"></param>
        /// <returns></returns>
        int? GetDepartureByWMSCode(String wmsCode);

        /// <summary>
        /// 根据Vancl WMS提供的SCM目的地ID取得物流系统的目的地
        /// </summary>
        /// <param name="scmArrival"></param>
        /// <returns></returns>
        int? GetArrivalByVanclSCMArrival(String scmArrival);

        /// <summary>
        /// 根据VJIA WMS提供的SCM目的地ID取得物流系统的目的地
        /// </summary>
        /// <param name="scmArrival"></param>
        /// <returns></returns>
        int? GetArrivalByVJIASCMArrival(String scmArrival);

        /// <summary>
        /// 取得子站点
        /// </summary>
        /// <param name="parentId">父站点id</param>
        /// <returns></returns>
        IList<ExpressCompanyModel> GetChildExpressCompany(int parentId);

        ExpressCompanyModel Get(int expressCompanyId);
        /// <summary>
        /// 取得所有站点名
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllCompanyNames();
        /// <summary>
        /// 根据ID列表检索对象
        /// </summary>
        /// <param name="listExpressCompanyID"></param>
        /// <returns></returns>
        IList<ExpressCompanyModel> Search(List<int> listExpressCompanyID);

        /// <summary>
        /// 通过站点名称查询站点ID
        /// </summary>
        /// <param name="expressCompanyName"></param>
        /// <returns></returns>
        int GetExpressCompanyIDByName(string expressCompanyName);

        /// <summary>
        /// 根据配送商编码取得分拣中心列表不包含登录人所在分拣中心
        /// </summary>
        /// <returns></returns>
        List<ExpressCompanyModel> GetSortingCentersByDistributionCodeWithoutSelf();

		/// <summary>
		/// 根据配送商编码取得分拣中心列表
		/// </summary>
		/// <returns></returns>
	    List<ExpressCompanyModel> GetSortingCentersByDistributionCode();

		List<ExpressCompanyModel> GetDistributionCooprelation(string distributionCode);

        /// <summary>
        /// 获得所有配送商
        /// </summary>
        /// <returns></returns>
        List<ExpressCompanyModel> GetAllDistributors();

	    ExpressCompanyModel GetDistributor(string code);

        /// <summary>
        /// 依据部门ID获取部门Model
        /// </summary>
        /// <param name="expressCompanyID"></param>
        /// <returns></returns>
        ExpressCompanyModel GetModel(int expressCompanyID);

        /// <summary>
        /// 取得分拣中心下属站点
        /// </summary>
        /// <param name="SortCenterID">分拣中心ID</param>
        /// <returns></returns>
        List<int> GetSortCenterStationList(int SortCenterID);

        /// <summary>
        /// 取得联系电话
        /// </summary>
        /// <param name="ExpresscompanyID"></param>
        /// <returns></returns>
        String GetContacterPhone(int ExpresscompanyID);

	    /// <summary>
	    /// 查询某个分拣中心对应的分拣范围
	    /// </summary>
	    /// <param name="searchaModel"></param>
	    /// <returns></returns>
		PagedList<ViewRangeDefinedSearchListModel> GetRangeDefineInfoList(RangeDefinedSearchModel searchaModel);

	    /// <summary>
	    /// 获取已存在分拣范围定义的对象
	    /// </summary>
	    /// <param name="companyFlag"></param>
	    /// <param name="rangedExpressCompanyId"></param>
	    /// <returns></returns>
	    ViewRangeDefinedSearchListModel GetExistInfoByStationId(int companyFlag, int rangedExpressCompanyId);
		/// <summary>
		/// 判断新增的分拣范围定义是否存在
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		bool IsExistsOfRangeDefined(RangeDefinedModel model);

		/// <summary>
		/// 新增分拣范围定义
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
	    int AddRangeDefined(RangeDefinedModel model);

		/// <summary>
		/// 删除某个分拣范围定义
		/// </summary>
		/// <param name="rangeDefinedIds"></param>
		/// <returns></returns>
		int DeleteRangeDefined(List<int> rangeDefinedIds);

	    /// <summary>
		/// 导出选中的数据
	    /// </summary>
	    /// <param name="rangeIdList"></param>
	    /// <param name="chooseType"></param>
	    /// <returns></returns>
	    IList<RangeDefinedPrintExportModel> GetRangeDefinedPrintExportModel(IList<string> rangeIdList, int chooseType);

        /// <summary>
        /// 根据站点获取分拣中心
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        RangeDefinedModel GetSortingCenterByStation(int station);
		
    }
}
