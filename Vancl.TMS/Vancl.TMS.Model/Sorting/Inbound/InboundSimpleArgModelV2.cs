using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.Model.Sorting.Inbound
{
   public class InboundSimpleArgModelV2
    {
        public Enums.SortCenterFormType ScanType { get; set; }
        public string FormCode { get; set; }
        public int MerchantId { get; set; }
        /// <summary>
        /// 是否不打印面单
        /// </summary>
        public bool IsSkipPrintBill { get; set; }
        /// <summary>
        /// 是否需要称重
        /// </summary>
        public bool IsNeedWeight { get; set; }
        public decimal BillWeight { get; set; }
        /// <summary>
        /// 是否复核重量
        /// </summary>
        public bool IsCheckWeight { get; set; }
        public decimal CheckWeight { get; set; }
        public int TemplateId { get; set; }

        public SortCenterUserModel OpUser { get; set; }
    }
}
