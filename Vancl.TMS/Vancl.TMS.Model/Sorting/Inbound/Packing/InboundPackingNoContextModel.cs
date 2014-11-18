using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.Packing
{
    /// <summary>
    /// 分拣装箱号产生上下文对象
    /// </summary>
    public class InboundPackingNoContextModel : SerialNumberModel
    {
        /// <summary>
        /// 分检中心ID
        /// </summary>
        public int SortingCenterID { get; set; }
    }
}
