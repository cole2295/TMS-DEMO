using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vancl.TMS.Web.Areas.Sorting.Models
{
    public class BatchBillPrintResultModel
    {
        public string FormCode { get; set; }
        public long WaybillNo { get; set; }
        public string CustomerOrder { get; set; }
        public bool IsDealSuccess { get; set; }
        public string Message { get; set; }
    }
}