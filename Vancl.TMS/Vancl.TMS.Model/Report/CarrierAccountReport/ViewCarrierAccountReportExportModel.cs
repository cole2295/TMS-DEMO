using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Report.CarrierAccountReport
{
    /// <summary>
    /// 承运商结算报表导出对象
    /// </summary>
    public class ViewCarrierAccountReportExportModel
    {
        /// <summary>
        /// 报表具体数据
        /// </summary>
        public List<ViewCarrierAccountReport> ReportData { get; set; }

    }
}
