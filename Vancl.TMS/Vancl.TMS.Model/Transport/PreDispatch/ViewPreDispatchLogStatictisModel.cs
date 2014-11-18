using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    /// <summary>
    /// 预调度异常统计信息
    /// </summary>
    public class ViewPreDispatchLogStatisticModel
    {
        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 出发地名称
        /// </summary>
        public String DepartureName { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        public String ArrivalName { get; set; }

        /// <summary>
        /// 货物属性
        /// </summary>
        public Enums.GoodsType GoodsType { get; set; }

        /// <summary>
        /// 总批次数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总订单数量
        /// </summary>
        public int TotalOrderCount { get; set; }

    }
}
