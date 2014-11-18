using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.BaseInfo
{
    public class MerchantModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 商家编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 商家编码
        /// </summary>
        public string MerchantCode { get; set; }

        /// <summary>
        /// 是否跳过面单打印
        /// </summary>
        public bool IsSkipPrintBill { get; set; }
        /// <summary>
        /// 是否要进行称重
        /// </summary>
        public bool IsNeedWeight { get; set; }
        /// <summary>
        /// 是否要进行复核称重
        /// </summary>
        public bool IsCheckWeight { get; set; }
        /// <summary>
        /// 复核称重的阈值
        /// </summary>
        public decimal CheckWeight { get; set; }

        /// <summary>
        /// 是否需要校验面单
        /// </summary>
        public bool IsValidateBill { get; set; }

        /// <summary>
        /// 是否快递商家
        /// </summary>
        public bool IsExpressDelivery { get; set; }
    }
}
