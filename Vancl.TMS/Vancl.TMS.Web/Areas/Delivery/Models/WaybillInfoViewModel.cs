using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.Delivery.Models
{
    public class WaybillInfoViewModel
    {
        public long CWID { get; set; }

        [Display(Name = "物流单号")]
        public string WaybillNo { get; set; }

        [Required]
        [Display(Name = "件数")]
        public long TotalCount { get; set; }

        [Required]
        [Display(Name = "批次数/箱数")]
        public Int32 Boxcount { get; set; }

        [Required]
        [Display(Name = "重量")]
        public decimal Weight { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        [Required]
        [Display(Name = "保价金额")]
        public decimal ProtectedPrice { get; set; }

        [Required]
        [Display(Name = "金额")]
        public decimal TotalAmount { get; set; }

        //[Required]
        //[Display(Name = "到货时间")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? ArrivalTime { get; set; }
    }
}