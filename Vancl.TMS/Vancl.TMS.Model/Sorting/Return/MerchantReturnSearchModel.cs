using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Return
{
    public class MerchantReturnSearchModel
    {
        public string FormCode { get; set; }
        public string Source { get; set; }
        public string ReturnStatus { get; set; }
        public string DeliverStationID { get; set; }
        public string CurrentDistributionCode { get; set; }
        public string DistributionCode { get; set; }
        public string BoxNo { get; set; }
        public string DeliverCode { get; set; }
        public string CurrentDeptName { get; set; }
        public bool isHasPrint { get; set; }
    }
}
