using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vancl.TMS.Web.Areas.Sorting.Models
{
    public class BatchBillPrintArgModel
    {
        public Vancl.TMS.Model.Common.Enums.SortCenterFormType FormType { get; set; }
        public string FormCode { get; set; }
        public decimal Weight { get; set; }
        public int MerchantId { get; set; }
    }
}