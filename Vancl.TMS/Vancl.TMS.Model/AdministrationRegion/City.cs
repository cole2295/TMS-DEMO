using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.AdministrationRegion
{
    public class City : BaseModel
    {
        public string CityID { get; set; }
        public string ProvinceID { get; set; }
        public string CityName { get; set; }
        public string ExpressCompanyId { get; set; }
        public string DistributionCode { get; set; }
    }
}
