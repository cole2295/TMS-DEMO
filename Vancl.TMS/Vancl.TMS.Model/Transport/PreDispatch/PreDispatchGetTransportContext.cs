using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.PreDispatch
{

    /// <summary>
    /// 预调度取得运输计划
    /// </summary>
    public class PreDispatchGetTransportContext
    {

        /// <summary>
        /// 出发地ID
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 货物属性
        /// </summary>
        public Enums.GoodsType ContentType { get; set; }

    }

}
