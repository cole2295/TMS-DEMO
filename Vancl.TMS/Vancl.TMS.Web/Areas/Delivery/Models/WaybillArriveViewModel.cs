using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Web.Areas.Delivery.Models
{
    /// <summary>
    /// 确认到货的视图
    /// </summary>
    public class WaybillArriveViewModel
    {
        /// <summary>
        /// 调度主键
        /// </summary>
        public long DispatchID { get; set; }

        [Display(Name = "预计到货时间")]
        public DateTime? ExpectArrivalDate { get; set; }

        [Display(Name = "修正预计到货时间")]
        public DateTime? ConfirmExpArrivalDate { get; set; }

        [Required]
        [Display(Name = "目的地接收时间")]
        public DateTime? DesReceiveDate { get; set; }

        [Display(Name = "物流单号")]
        public string WaybillNo { get; set; }

        [Display(Name = "提货单号")]
        public string DeliveryNo { get; set; }

        [Display(Name = "延误类型")]
        public Enums.DelayType DelayType { get; set; }

        [Display(Name = "延误原因")]
        public string DelayReason { get; set; }

        [Display(Name = "预计延误时长")]
        public int ExpectDelayTime { get; set; }

        [Display(Name = "预计延误类型")]
        public Enums.ExpectDelayType ExpectDelayType { get; set; }

        [Display(Name = "预计延误原因")]
        public string ExpectDelayDesc { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "签收人")]
        public string SignedUser { get; set; }

        /// <summary>
        /// 是否延误
        /// </summary>
        public bool IsDelay { get; set; }

        /// <summary>
        /// 延误时长（小时）
        /// </summary>
        public int DelayTimeSpan
        {
            get
            {
                var ts = DateTime.Now - ConfirmExpArrivalDate;
                return ts.HasValue ? ts.Value.Hours : 0;
            }
        }

        /// <summary>
        /// 是否有预计延误
        /// </summary>
        public bool IsExpectDelay { get; set; }
    }
}