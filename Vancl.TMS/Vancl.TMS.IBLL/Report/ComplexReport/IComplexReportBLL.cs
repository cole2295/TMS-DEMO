using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Report.ComplexReport;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Report;

namespace Vancl.TMS.IBLL.Report.ComplexReport
{
    /// <summary>
    /// 综合报表
    /// </summary>
    public interface IComplexReportBLL
    {
        /// <summary>
        /// 综合报表查询
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        ViewComplexReportPageModel Search(ComplexReportSearchModel searchModel);

        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        ViewComplexReportExportModel Export(ComplexReportSearchModel searchModel);

        /// <summary>
        /// 取得当前用户报表筛选列表
        /// </summary>
        /// <returns></returns>
        List<ReportFilterModel> GetCurrentUserReportFilter();
    }


}
