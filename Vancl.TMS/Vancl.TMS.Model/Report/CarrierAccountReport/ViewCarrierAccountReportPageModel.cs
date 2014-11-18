using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Report.CarrierAccountReport
{
    /// <summary>
    /// 承运商结算报表web页面对象
    /// </summary>
    public class ViewCarrierAccountReportPageModel
    {
        /// <summary>
        /// 报表具体数据
        /// </summary>
        public Util.Pager.PagedList<ViewCarrierAccountReport> ReportData { get; set; }

        /// <summary>
        /// 报表具体数据(动态)
        /// </summary>
        public List<ViewCarrierAccountReport> DynamicReportData { get; set; }

        /// <summary>
        /// 统计数据
        /// </summary>
        public ViewCarrierAccountReportStatisticsModel StatisticsData { get; set; }

    }
}
