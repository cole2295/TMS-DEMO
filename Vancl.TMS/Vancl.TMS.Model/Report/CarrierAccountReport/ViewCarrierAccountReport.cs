using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Vancl.TMS.Model.CustomerAttribute;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Report.CarrierAccountReport
{
    /// <summary>
    /// 承运商结算报表
    /// </summary>
    [Description("综合报表")]
    public class ViewCarrierAccountReport
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Description("序号")]
        [ShowRowNumber]
        public int SortRowID { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        [Description("发货时间")]
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        [Description("运输方式")]
        public Enums.TransportType TransportType { get; set; }

        /// <summary>
        /// 承运商名称
        /// </summary>
        [Description("承运商")]
        public String CarrierName { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        [Description("提货单号")]
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        [Description("物流单号")]
        public String CustWaybillNo { get; set; }

        /// <summary>
        /// 出发地名称
        /// </summary>
        [Description("出发地")]
        public String DepartureName { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        [Description("目的地")]
        public String ArrivalName { get; set; }

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

        /// <summary>
        /// 提货单状态
        /// </summary>
        [Description("提货单状态")]
        public Enums.DeliveryStatus DeliveryStatus { get; set; }

    }
}
