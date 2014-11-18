using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using System.ComponentModel;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Report.ComplexReport
{
    /// <summary>
    /// 综合报表数据对象
    /// </summary>
    [Description("综合报表")]
    public class ViewComplexReport
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
        public DateTime? DepartureTime { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        [Description("提货单号")]
        public String DeliveryNo { get; set; }

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
        /// 目的地城市
        /// </summary>
        [Description("目的地城市")]
        public String ArrivalCity { get; set; }

        /// <summary>
        /// 目的地名称
        /// </summary>
        [Description("目的地")]
        public String ArrivalName { get; set; }

        /// <summary>
        /// 承运商名称
        /// </summary>
        [Description("承运商")]
        public String CarrierName { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        [Description("运输方式")]
        public Enums.TransportType? TransportType { get; set; }

        /// <summary>
        /// 到货时效
        /// </summary>
        [Description("时效H")]
        public decimal? ArrivalTiming { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        [Description("货物类型")]
        public Enums.GoodsType Goodstype { get; set; }

        /// <summary>
        /// 箱数
        /// </summary>
        [Description("箱数")]
        public int BoxCount { get; set; }

        /// <summary>
        /// 总单量
        /// </summary>
        [Description("总单量")]
        public int OrderCount { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        [Description("总重量")]
        public decimal TotalWeight { get; set; }

        /// <summary>
        /// 总单价
        /// </summary>
        [Description("总单价")]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        [Description("保价金额")]
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 提货单状态
        /// </summary>
        [Description("状态")]
        public Enums.ComplexReportDeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// 修正预计到货时间
        /// </summary>
        [Description("修正预计到货时间")]
        [DateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime? ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 预计到货日期
        /// </summary>
        [Description("预计到货时间")]
        [DateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime? ExpectArrivalDate { get; set; }

        /// <summary>
        /// 实际到货时间
        /// </summary>
        [Description("实际到货时间")]
        [DateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime? DesReceiveDate { get; set; }

        /// <summary>
        /// 签收人
        /// </summary>
        [Description("签收人")]
        public String SignedUser { get; set; }

        /// <summary>
        /// 是否延误
        /// </summary>
        [Description("是否延迟")]
        public bool? IsDelay { get; set; }

        /// <summary>
        /// 延误时长H
        /// </summary>
        [Description("延误时长H")]
        public decimal? Delaytimespan { get; set; }

        /// <summary>
        /// 延误类型
        /// </summary>
        [Description("延误类型")]
        public Enums.DelayType? DelayType { get; set; }

        /// <summary>
        /// 延误原因
        /// </summary>
        [Description("延误原因")]
        public string DelayReason { get; set; }

        /// <summary>
        /// 丢失类型
        /// </summary>
        [Description("丢失类型")]
        public Enums.LostType? LostType { get; set; }

    }
}
