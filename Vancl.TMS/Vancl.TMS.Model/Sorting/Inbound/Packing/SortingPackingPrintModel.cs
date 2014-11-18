using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Inbound.Packing
{
    /// <summary>
    /// 分拣装箱打印对象
    /// </summary>
    public class SortingPackingPrintModel
    {

        /// <summary>
        /// 箱号
        /// </summary>
        public String BoxNo { get; set; }

        /// <summary>
        /// 发货日期
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        public String ArrivalName { get; set; }

        /// <summary>
        /// 出发地名称
        /// </summary>
        public String DepartureName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int BillCount { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 装箱人
        /// </summary>
        public String PackingOpUser { get; set; }

        /// <summary>
        /// 配送公司名称
        /// </summary>
        public String DistributionName { get; set; }

        /// <summary>
        /// 第几箱
        /// </summary>
        public string TheN { get; set; }

        public int DepartureID { get; set; }
        public int ArrivalID { get; set; }
    }
}
