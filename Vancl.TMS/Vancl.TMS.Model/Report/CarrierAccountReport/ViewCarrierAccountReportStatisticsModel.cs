using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Vancl.TMS.Model.Report.CarrierAccountReport
{
    /// <summary>
    /// 承运商结算报表统计对象
    /// </summary>
    public class ViewCarrierAccountReportStatisticsModel
    {
        /// <summary>
        /// 订单数量
        /// </summary>
        [Description("订单数量")]
        public int OrderCount { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        [Description("总重量")]
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 基础运费
        /// </summary>
        [Description("基础运费")]
        public decimal BaseAmount { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        [Description("保价金额")]
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 保险费
        /// </summary>
        [Description("保险费")]
        public decimal InsuranceAmount { get; set; }

        /// <summary>
        /// 运费补足
        /// </summary>
        [Description("运费补足")]
        public decimal ComplementAmount { get; set; }

        /// <summary>
        /// 送货费
        /// </summary>
        [Description("送货费")]
        public decimal LongDeliveryAmount { get; set; }

        /// <summary>
        /// 转运费
        /// </summary>
        [Description("转运费")]
        public decimal LongTransferAmount { get; set; }

        /// <summary>
        /// 提货费用
        /// </summary>
        [Description("提货费用")]
        public decimal LongPickPrice { get; set; }

        /// <summary>
        /// 应付运费
        /// </summary>
        [Description("应付运费")]
        public decimal NeedAmount { get; set; }

        /// <summary>
        /// KPI考核
        /// </summary>
        [Description("KPI考核")]
        public decimal? KPIAmount { get; set; }

        /// <summary>
        /// 丢失赔付
        /// </summary>
        [Description("丢失赔付")]
        public decimal LostDeduction { get; set; }

        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        public decimal OtherAmount { get; set; }

        /// <summary>
        /// 实际支付运输费
        /// </summary>
        [Description("实际支付运输费")]
        public decimal ConfirmedAmount { get; set; }
    }
}
