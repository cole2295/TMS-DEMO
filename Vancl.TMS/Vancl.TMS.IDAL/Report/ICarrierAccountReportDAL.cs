using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Report.CarrierAccountReport;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IDAL.Report
{
    /// <summary>
    /// 承运商结算报表数据层
    /// </summary>
    public interface ICarrierAccountReportDAL
    {
        /// <summary>
        /// 动态承运商结算报表查询
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        List<ViewCarrierAccountReport> SearchDynamicReport(CarrierAccountReportSearchModel searchModel);

        /// <summary>
        /// 取得动态统计信息
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        ViewCarrierAccountReportStatisticsModel GetDynamicStatisticsInfo(CarrierAccountReportSearchModel searchModel);

        /// <summary>
        /// 综合报表查询
        /// </summary>
        /// <param name="searchModel">检索</param>
        /// <returns></returns>
        PagedList<ViewCarrierAccountReport> Search(CarrierAccountReportSearchModel searchModel);

        /// <summary>
        /// 取得统计信息
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        ViewCarrierAccountReportStatisticsModel GetStatisticsInfo(CarrierAccountReportSearchModel searchModel);

        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        List<ViewCarrierAccountReport> Export(CarrierAccountReportSearchModel searchModel);
    }
}
