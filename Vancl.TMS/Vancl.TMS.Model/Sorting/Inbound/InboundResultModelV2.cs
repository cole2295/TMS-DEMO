using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    public class InboundResultModelV2
    {
        public bool IsSuccess { get; set; }
        public string FormCode { get; set; }
        public string DeliverCode { get; set; }
        public string Message { get; set; }
        public decimal CustomerWeight { get; set; }
        public int TotalInboundCount { get; set;}
        public int CurrentIndex { get; set; }
        public int CurrentCount { get; set; }
        private bool _isskipPrint = true;
        public bool IsSkipPrint { get { return _isskipPrint; } set { _isskipPrint = value; } }
        public bool IsNeedWeight { get; set; }
    }
}
