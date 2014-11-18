using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Report.CarrierAccountReport;

namespace Vancl.TMS.IBLL.Report
{
    /// <summary>
    /// 承运商结算报表
    /// </summary>
    public interface ICarrierAccountReportBLL
    {
        /// <summary>
        /// 检索动态报表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        ViewCarrierAccountReportPageModel SearchDynamicReport(CarrierAccountReportSearchModel searchModel);

        /// <summary>
        /// 检索
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        ViewCarrierAccountReportPageModel Search(CarrierAccountReportSearchModel searchModel);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        ViewCarrierAccountReportExportModel Export(CarrierAccountReportSearchModel searchModel);
    }
}
