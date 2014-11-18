using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.Transport.Models
{
    public class TransportPlanBaseSettingViewModel
    {
        public string DepartureID { get; set; }
        public string ArrivalID { get; set; }
        public int GoodsType { get; set; }
        public bool IsTransit { get; set; }
    }
}