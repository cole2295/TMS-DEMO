using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.DataAPI.Models
{
    public class ScanBatchNoModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "不能为空")]
        public string BatchNo { get; set; }
    }
}