using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.Delivery.Models
{
    public class InputWaybillNoModel
    {
        [Required]
        [Display(Name = "提货单号")]
        public string DeliveryNo { get; set; }

        [Display(Name = "物流单号")]
        public string WaybillNo { get; set; }
    }
}