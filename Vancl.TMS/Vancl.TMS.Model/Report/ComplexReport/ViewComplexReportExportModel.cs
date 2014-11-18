using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Report.ComplexReport
{
    /// <summary>
    /// 综合报表页面导出对象
    /// </summary>
    public class ViewComplexReportExportModel
    {
        /// <summary>
        /// 报表具体数据
        /// </summary>
        public List<Model.Report.ComplexReport.ViewComplexReport> ReportData { get; set; }

        /// <summary>
        /// 报表筛选对象
        /// </summary>
        public List<ReportFilterModel> Filter { get; set; }
    }
}
