using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库打印模型
    /// </summary>
    public class OutboundPrintModel
    {
        public int ExpressCompanyID { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string BatchNo{get;set;}
        public int PackageNum { get; set; }
        public DateTime OutboundTime { get; set; }
        public decimal BoxWeight { get; set; }
        public decimal InsuredAmount { get; set; }
    }
}
