using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using System.ComponentModel;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 批次运单信息
    /// (供出库打印菜单发邮件使用)
    /// </summary>
    public class BatchBillInfoForOutBound
    {
        /// <summary>
        /// 运单号
        /// </summary>
        [Description("订单类型")]
        public string FormCode { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        [Description("订单类型")]
        public Enums.BillType BillType { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        [Description("批次号")]
        public string BatchNo { get; set; }
        /// <summary>
        /// 配送站
        /// </summary>
        [Description("配送站")]
        public string CompanyName { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        [Description("收货人")]
        public string ReceiveBy { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [Description("地址")]
        public string ReceiveAddress { get; set; }
        /// <summary>
        /// 收货人电话
        /// </summary>
        [Description("收货人电话")]
        public string ReceiveMobile { get; set; }
        /// <summary>
        /// 订单来源
        /// </summary>
        [Description("订单来源")]
        public string Source { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        [Description("省")]
        public string ReceiveProvince { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        [Description("市")]
        public string ReceiveCity { get; set; }
        /// <summary>
        /// 区
        /// </summary>
        [Description("区")]
        public string ReceiveArea { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        [Description("邮编")]
        public string ReceivePost { get; set; }
        /// <summary>
        /// 送货时间
        /// </summary>
        [Description("送货时间")]
        public string SendTimeType { get; set; }
        /// <summary>
        /// 客户备注
        /// </summary>
        [Description("客户备注")]
        public string ReceiveComment { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        [Description("发货时间")]
        public DateTime OutBoundTime { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        [Description("支付方式")]
        public string AcceptType { get; set; }
        /// <summary>
        /// 总价格
        /// </summary>
        [Description("总价格")]
        public decimal Amount { get; set; }
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        public decimal PaidAmount { get; set; }
        /// <summary>
        /// 应付款
        /// </summary>
        [Description("应付款")]
        public decimal NeedAmount { get; set; }
        /// <summary>
        /// 应退款
        /// </summary>
        [Description("应退款")]
        public decimal NeedBackAmount { get; set; }
        /// <summary>
        /// 保价金额
        /// </summary>
        [Description("保价金额")]
        public decimal ProtectedPrice { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        [Description("重量")]
        public decimal Weight { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        [Description("箱号")]
        public int CustomerBoxNo { get; set; }
    }
}
