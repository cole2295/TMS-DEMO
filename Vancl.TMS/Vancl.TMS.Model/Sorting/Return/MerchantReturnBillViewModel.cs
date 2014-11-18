using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 商家入库确认显示模型
    /// </summary>
    [Serializable]
    [DataContract]
    public class MerchantReturnBillViewModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }
        
        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 配送单号
        /// </summary>
        public string DeliverCode { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public string BillFormType { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string BillStatus { get; set; }
        /// <summary>
        /// 返货状态
        /// </summary>
        public string ReturnStatus { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal NeedAmount { get; set; }
        /// <summary>
        /// 应退金额
        /// </summary>
        public decimal NeedBackAmount { get; set; }
        /// <summary>
        /// 配送员姓名
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 退货原因
        /// </summary>
        public string ReturnReason { get; set; }
        /// <summary>
        /// 商家名称
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 配送商
        /// </summary>
        public string DistributionName { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 运单来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 运单类型
        /// </summary>
        public string FormType { get; set; }
        /// <summary>
        /// 运单状态
        /// </summary>
        public string Status { get; set; }
    }
}
