using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
    /// <summary>
    /// 称重复核
    /// </summary>
    [Serializable]
    public class WeighingReviewModel
    {
        /// <summary>
        /// 是否扫描成功
        /// </summary>
        public bool IsScanOK { get; set; }

        /// <summary>
        /// 订单是否异常
        /// </summary>
        public bool IsAbnormal { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 称重重量
        /// </summary>
        public decimal WaybillWeight { get; set; }

        /// <summary>
        /// 客户重量
        /// </summary>
        public decimal MerchantWeight { get; set; }

        /// <summary>
        /// 是否已称重
        /// </summary>
        public bool IsWeighed { get; set; }
    }
}
