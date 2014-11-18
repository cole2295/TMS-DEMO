using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.Packing
{
    /// <summary>
    /// 装箱入库前检查对象
    /// </summary>
    public class SortingPackingCheckModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 目的地id
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 是否已装箱
        /// </summary>
        public bool IsPacked { get; set; }

        /// <summary>
        /// 入库分拣中心id
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 运单分拣类型
        /// </summary>
        public Enums.SortCenterOperateType InboundType { get; set; }
    }
}
