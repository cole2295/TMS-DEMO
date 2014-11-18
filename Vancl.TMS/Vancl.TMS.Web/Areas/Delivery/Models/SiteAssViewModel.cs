using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vancl.TMS.Model.Delivery.Spot;
using System.ComponentModel.DataAnnotations;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Web.Areas.Delivery.Models
{
    public class SiteAssViewModel : SiteAssessmentModel
    {
        [Display(Name = "提货单号")]
        public override string DeliveryNO { get; set; }

        [Display(Name = "实际到库时间")]
        public override DateTime ArrivalTime { get; set; }

        [Display(Name = "实际离库时间")]
        public override DateTime LeaveTime { get; set; }

        [Display(Name = "考核状态")]
        public override bool AssessmentStatus { get; set; }

        [Display(Name = "异常原因")]
        public override string Reason { get; set; }
    }
}