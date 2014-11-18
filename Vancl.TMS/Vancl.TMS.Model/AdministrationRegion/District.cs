using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.AdministrationRegion
{
    public class District : BaseModel
    {
        public string DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string DistributionCode { get; set; }
    }
}
