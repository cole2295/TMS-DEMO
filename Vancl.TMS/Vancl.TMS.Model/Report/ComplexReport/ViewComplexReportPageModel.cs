using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Report.ComplexReport
{
    /// <summary>
    /// 综合报表页面View对象
    /// </summary>
    public class ViewComplexReportPageModel
    {
        /// <summary>
        /// 报表具体数据
        /// </summary>
        public Util.Pager.PagedList<Model.Report.ComplexReport.ViewComplexReport> ReportData { get; set; }

        /// <summary>
        /// 报表筛选对象
        /// </summary>
        public List<ReportFilterModel> Filter { get; set; }

    }
}
