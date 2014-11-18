using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.BaseInfo.Sorting
{
    /// <summary>
    /// 商家单号对应关系类
    /// </summary>
    public class MerchantFormCodeRelationModel
    {
        /// <summary>
        /// 商家编号
        /// </summary>
        public int MerchantID { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        public string DeliverCode { get; set; }

        public bool IsSkipPrintBill { get; set; }
        public bool IsNeedWeight { get; set; }
        public bool IsCheckWeight { get; set; }
        public decimal CheckWeight { get; set; }


        /// <summary>
        /// 运单状态
        /// </summary>
        public Vancl.TMS.Model.Common.Enums.BillStatus Status { get; set; }

        public string StatusName { get; set; }
    }
}
