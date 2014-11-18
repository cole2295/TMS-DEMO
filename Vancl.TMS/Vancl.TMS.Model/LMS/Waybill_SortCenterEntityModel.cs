using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 分拣对应关系表
    /// </summary>
    public class Waybill_SortCenterEntityModel : BaseModel
    {
        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNO { get; set; }

        /// <summary>
        /// 入库交接单[废弃]
        /// </summary>
        public String InboundGuid { get; set; }

        /// <summary>
        /// 出库交接单[废弃]
        /// </summary>
        public String OutboundGuid { get; set; }

        /// <summary>
        /// 入库KEY
        /// </summary>
        public String InBoundKid { get; set; }

        /// <summary>
        /// 出库KEY
        /// </summary>
        public String OutBoundKid { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "Waybill_SortCenter";
            }
        }

    }
}
