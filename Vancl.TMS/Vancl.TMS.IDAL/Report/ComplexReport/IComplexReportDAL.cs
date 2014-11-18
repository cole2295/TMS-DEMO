using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Report.ComplexReport;

namespace Vancl.TMS.IDAL.Report.ComplexReport
{
    /// <summary>
    /// 综合报表
    /// </summary>
    public interface IComplexReportDAL
    {
        /// <summary>
        /// 综合报表查询
        /// </summary>
        /// <param name="searchModel">检索</param>
        /// <returns></returns>
        PagedList<ViewComplexReport> Search(ComplexReportSearchModel searchModel);

        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        List<ViewComplexReport> Export(ComplexReportSearchModel searchModel);
    }
}
