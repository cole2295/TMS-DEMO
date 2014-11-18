using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Return
{
    public class ReturnBillInfoModel
    {
        [DisplayName("运单号")]
        public long OrderNo { get; set; }

        [DisplayName("订单号")]
        public string CustomerOrder { get; set; }

        [DisplayName("配送单号")]
        public string DeliverCode { get; set; }

        [DisplayName("站点打印时间")]
        public DateTime PrintTime { get; set; }

        [DisplayName("站点归班时间")]
        public DateTime BackTime { get; set; }

        //[DisplayName("6	客户来源")]
        //public string SourceName { get; set; }

        [DisplayName("商家名称")]
        public string MerchantName { get; set; }

        [DisplayName("订单类型")]
        public Enums.BillType BillType { get; set; }

        [DisplayName("订单状态")]
        public Enums.BillStatus BillStatus { get; set; }

        [DisplayName("应收金额")]
        public decimal NeedAmount { get; set; }

        [DisplayName("应退金额")]
        public decimal NeedBackAmount { get; set; }

        [DisplayName("配送站点")]
        public string CompanyName { get; set; }

        [DisplayName("退货原因")]
        public string BackReason { get; set; }
    }
}
