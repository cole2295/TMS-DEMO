using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    public class LineFormulaPriceViewModel
    {
        [Display(Name = "线路主键ID")]
        public string LPID { get; set; }

        [Required]
        [Display(Name = "基价")]
        public string BasePrice { get; set; }

        [Required]
        [Display(Name = "基重")]
        public string BaseWeight { get; set; }

        [Required]
        [Display(Name = "续价")]
        public string OverPrice { get; set; }

        [Display(Name = "描述")]
        public string Note { get; set; }
    }
}