using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vancl.TMS.Model.CustomizeFlow.Parameter
{
    public class CheckerParameter
    {
        public long WaybillNo { get; set; }

        public int FromExpressCompanyId { get; set; }

        public int? ToExpressCompanyId { get; set; }

        public string FromDistributionCode { get; set; }

        public string ToDistributionCode { get; set; }


    }
}
