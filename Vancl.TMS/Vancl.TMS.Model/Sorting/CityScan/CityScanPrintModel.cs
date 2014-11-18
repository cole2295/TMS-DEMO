using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.CityScan
{
    /// <summary>
    /// 同城单量统计交接表
    /// </summary>
    public class CityScanPrintModel
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 扫描分拣中心名称
        /// </summary>
        public String ScanSortCenterName { get; set; }

        /// <summary>
        /// 扫描批次明细
        /// </summary>
        public IList<CityScanBatchDetail> Details { get; set; }
    }

    public class CityScanBatchDetail
    {
        /// <summary>
        /// 单号
        /// </summary>
        public String FormCode { get; set; }
    }
}
