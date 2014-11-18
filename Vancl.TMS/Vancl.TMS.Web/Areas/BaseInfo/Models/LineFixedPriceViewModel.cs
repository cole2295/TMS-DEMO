using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    public class LineFixedPriceViewModel
    {
        [Display(Name = "线路主键ID")]
        public string LPID { get; set; }

        [Required]
        [Display(Name = "价格")]
        public string Price { get; set; }

        [Display(Name = "描述")]
        public string Note { get; set; }

    }
}