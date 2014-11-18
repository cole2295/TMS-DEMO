using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vancl.TMS.Model.Common;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.Claim.Models
{
    public class ApplyForExpectDelayViewModel
    {
        [Required]
        public long DispatchID { get; set; }

        [Display(Name="提货单号")]
        public string DeliveryNo { get; set; }

        [Required]
        [Display(Name = "预计延误类型 ")]
        public Enums.ExpectDelayType ExpectDelayType { get; set; }

        [Required]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "*")]
        [Display(Name = "预计延误时长")]
        public int DelayTime { get; set; }

        [Display(Name = "预计延误原因  ")]
        public string DelayDesc { get; set; }
    }
}