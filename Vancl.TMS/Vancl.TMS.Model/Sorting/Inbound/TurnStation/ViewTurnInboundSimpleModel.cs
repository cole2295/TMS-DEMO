using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.TurnStation
{
    /// <summary>
    /// 逐单转站入库View对象
    /// </summary>
    public class ViewTurnInboundSimpleModel : ResultModel
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 当前入库数量
        /// </summary>
        public int InboundCount { get; set; }
    }
}
