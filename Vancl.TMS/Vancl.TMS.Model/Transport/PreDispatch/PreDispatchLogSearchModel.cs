using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    /// <summary>
    /// 预调度日志
    /// </summary>
    public class PreDispatchLogSearchModel : BaseSearchModel
    {
        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 货物属性
        /// </summary>
        public Enums.GoodsType? GoodsType { get; set; }

    }
}
