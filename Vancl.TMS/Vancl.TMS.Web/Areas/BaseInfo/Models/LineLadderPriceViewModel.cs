using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    public class LineLadderPriceViewModel
    {
        [Display(Name = "主键ID")]
        public string LLPID { get; set; }

        [Display(Name = "线路主键ID")]
        public string LPID { get; set; }

        [Required]
        [Display(Name = "最小重量")]
        public string StartWeight { get; set; }

        [Display(Name = "最大重量")]
        public string EndWeight { get; set; }

        [Required]
        [Display(Name = "价格")]
        public string Price { get; set; }

        [Display(Name = "描述")]
        public string Desc { get; set; }
    }
}