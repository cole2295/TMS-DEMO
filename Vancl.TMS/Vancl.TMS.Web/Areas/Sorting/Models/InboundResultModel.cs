using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vancl.TMS.Web.Areas.Sorting.Models
{
    public class InboundResultModel
    {
        public string FormCode { get; set; }
        public string DeliverCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}