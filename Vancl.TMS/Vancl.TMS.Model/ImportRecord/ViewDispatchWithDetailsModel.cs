using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.ImportRecord
{
    public class ViewDispatchWithDetailsModel
    {
        /// <summary>
        /// 线路计划ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 出发地ID
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public Enums.DeliverySource Source { get; set; }

        /// <summary>
        /// 批次/箱号明细
        /// </summary>
        public List<ViewPreDispatchModel> Details { get; set; }
    }
}
