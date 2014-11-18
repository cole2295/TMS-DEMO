using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    public class DelayCriteriaViewModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long DCID { get; set; }

        /// <summary>
        /// 承运商编号
        /// </summary>
        public int CarrierID { get; set; }

        /// <summary>
        /// 开始区间
        /// </summary>
        public int StartRegion { get; set; }

        /// <summary>
        /// 结束区间
        /// </summary>
        public int? EndRegion { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }
    }
}