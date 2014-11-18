using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Report
{
    /// <summary>
    /// 报表筛选View对象
    /// </summary>
    public class ViewReportFilterModel
    {
        /// <summary>
        /// 筛选列表
        /// </summary>
        public List<ReportFilterModel> Filter { get; set; }

        /// <summary>
        /// 报表种类
        /// </summary>
        public Enums.ReportCategory ReportType { get; set; }

    }
}
