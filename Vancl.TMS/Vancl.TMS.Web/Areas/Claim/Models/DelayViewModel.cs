using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Web.Areas.Claim.Models
{
    /// <summary>
    /// 到货延误列表
    /// </summary>
    public class DelayViewModel
    {
        [Display(Name = "发货仓库")]
        public  string DepartureID { get; set; }

        [Display(Name = "目的站")]
        public  string ArrivalID { get; set; }

        [Display(Name = "订单数量")]
        public  string BoxCount { get; set; }

        //[Display(Name = "线路计划编号")]
        //public  string BoxCount { get; set; } 

        [Display(Name = "目的城市")]
        public  string ArrivalCity { get; set; }

        [Display(Name = "城际运输商")]
        public  string CarrierName { get; set; }

        [Display(Name = "运输方式")]
        public  string TransPortType { get; set; }

        [Display(Name = "时效H")]
        public  string ArrivalTiming { get; set; }

        [Display(Name = "提货单号")]
        public  string DeliveryNO { get; set; }

        [Display(Name = "物流运单号")]
        public  string CarrierWaybillNO { get; set; }

        [Display(Name = "危险品标记")]
        public  string GoodsType { get; set; }

        [Display(Name = "总价")]
        public  string TotalAmount { get; set; }

        [Display(Name = "预计到货时间")]
        public  string ExpectArrivalDate { get; set; }

        [Display(Name = "状态")]
        public  string DeliveryStatus { get; set; }

        [Display(Name = "录入预计延误时长")]
        public  string ExpectDelayTime{ get; set; }

        [Display(Name = "录入预计延误原因")]
        public  string ExpectDelayDesc { get; set; }

        [Display(Name = "系统计算修正到货日期")]
        public  string ConfirmExpArrivalDate { get; set; }

        [Display(Name = "延误类型")]
        public  string DelayType { get; set; }

        [Display(Name = "延迟交货申请是否成立")]
        public  string ExpectDelayApproveStatus { get; set; }

        [Display(Name = "申请复议理由")]
        public  string DelayHandleNode { get; set; }

        [Display(Name = "复议处理标识")]
        public  string DelayHandleApproveStatus { get; set; }      

    }
}