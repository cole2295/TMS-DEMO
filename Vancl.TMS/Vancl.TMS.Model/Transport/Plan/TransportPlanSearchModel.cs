using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Plan
{
    /// <summary>
    /// 运输计划搜索Model
    /// </summary>
    public class TransportPlanSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int? DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int? ArrivalID { get; set; }

        /// <summary>
        /// 运输计划状态
        /// </summary>
        public Enums.TransportStatus? Status { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Enums.TransportType? TransportType { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        public Enums.GoodsType? GoodsType { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public int? CarrierID { get; set; }
    }
}
