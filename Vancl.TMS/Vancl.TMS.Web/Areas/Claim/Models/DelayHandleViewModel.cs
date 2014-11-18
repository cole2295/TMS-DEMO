using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vancl.TMS.Model.Claim;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.Claim.Models
{
    public class DelayHandleViewModel : DelayHandleModel
    {
        [Required]
        [Display(Name = "申请复议理由")]
        public override string NOTE { get; set; }
    }
}