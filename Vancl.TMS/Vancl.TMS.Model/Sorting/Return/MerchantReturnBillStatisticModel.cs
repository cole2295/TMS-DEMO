using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Return
{
    public class MerchantReturnBillStatisticModel
    {
        /// <summary>
        /// 商家名称
        /// </summary>
        public string SourceName { get; set; }
        /// <summary>
        /// 换货成功数量
        /// </summary>
        public int ChangeCount { get; set; }
        /// <summary>
        /// 退货成功数量
        /// </summary>
        public int BackCount { get; set; }
        /// <summary>
        /// 正常拒收数量
        /// </summary>
        public int RefuseCount { get; set; }
        /// <summary>
        /// 换货拒收数量
        /// </summary>
        public int ChangeRefuseCount { get; set; }
        /// <summary>
        /// 换货重量
        /// </summary>
        public decimal ChangeWeight { get; set; }
        /// <summary>
        /// 退货重量
        /// </summary>
        public decimal BackWeight { get; set; }
        /// <summary>
        /// 拒收重量
        /// </summary>
        public decimal RefuseWeight { get; set; }
        /// <summary>
        /// 换货拒收重量
        /// </summary>
        public decimal ChangeRefuseWeight { get; set; }
    }
}
