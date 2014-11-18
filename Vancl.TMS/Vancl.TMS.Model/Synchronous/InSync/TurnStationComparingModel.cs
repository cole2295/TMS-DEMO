using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Synchronous.InSync
{
    public class TurnStationComparingModel : IBillLms2TmsForComparing
    {
        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 配送站点
        /// </summary>
        public int DeliverStationID { get; set; }
    }
}
