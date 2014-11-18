using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.CityScan
{
    /// <summary>
    /// 同城单量统计查询模板
    /// </summary>
    public class CityScanSearchModel:BaseSearchModel
    {
        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 扫描开始时间
        /// </summary>
        public DateTime? ScanStartTime { get; set; }

        /// <summary>
        /// 扫描结束时间
        /// </summary>
        public DateTime? ScanEndTime { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 分拣中心
        /// </summary>
        public String ExpressCompanyID { get; set; }
    }
}
