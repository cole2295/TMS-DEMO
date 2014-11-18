using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.AdministrationRegion
{
    public class Province : BaseModel
    {
        public string ProvinceID { get; set; }
        public string DistrictID { get; set; }
        public string ProvinceName { get; set; }
        public string DistributionCode { get; set; }
    }
}
