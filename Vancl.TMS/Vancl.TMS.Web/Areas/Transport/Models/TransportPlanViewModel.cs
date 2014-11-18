using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vancl.TMS.Model.Transport.Plan;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.Transport.Models
{
    public class TransportPlanViewModel : TransportPlanModel
    {
        /// <summary>
        /// 主键标识
        /// </summary>
        public override int TPID { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        [Required]
        [Display(Name = "出发地")]
        public override int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        [Required]
        [Display(Name = "目的地")]
        public override int ArrivalID { get; set; }

        /// <summary>
        /// 货物类型(0.普通,1.违禁品,2.易碎)
        /// </summary>
        [Required]
        [Display(Name = "货物类型")]
        public override Vancl.TMS.Model.Common.Enums.GoodsType LineGoodsType { get; set; }

        /// <summary>
        /// 是否中转
        /// </summary>
        [Display(Name = "是否中转")]
        public override bool IsTransit { get; set; }

        /// <summary>
        /// 截至日期
        /// </summary>
        [Required]
        [Display(Name = "截止日期")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public override DateTime Deadline { get; set; }
    }
}