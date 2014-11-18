using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Model.Common;
using System;
using Vancl.TMS.Web.WebControls.Mvc;
using System.Web.UI.WebControls;

namespace Vancl.TMS.Web.Areas.BaseInfo.Models
{
    public class LinePlanViewModel : Vancl.TMS.Model.BaseInfo.Line.LinePlanModel
    {
        public LinePlanViewModel()
        {

        }

        public LinePlanViewModel(LinePlanModel model)
        {
            this.ApproveBy = model.ApproveBy;
            this.ApproveTime = model.ApproveTime;
            this.ArrivalAssessmentTime = model.ArrivalAssessmentTime;
            this.ArrivalID = model.ArrivalID;
            this.ArrivalName = model.ArrivalName;
            this.ArrivalTiming = model.ArrivalTiming;
            this.CarrierID = model.CarrierID;
            this.CarrierName = model.CarrierName;
            this.CreateBy = model.CreateBy;
            this.CreateTime = model.CreateTime;
            this.DepartureID = model.DepartureID;
            this.DepartureName = model.DepartureName;
            this.EffectiveTime = model.EffectiveTime;
            this.ExpressionType = model.ExpressionType;
            this.LineGoodsType = model.LineGoodsType;
            this.InsuranceRate = model.InsuranceRate;
            this.IsDeleted = model.IsDeleted;
            this.LeaveAssessmentTime = model.LeaveAssessmentTime;
            this.LineID = model.LineID;
            this.LineType = model.LineType;
            this.LongDeliveryPrice = model.LongDeliveryPrice;
            this.LongPickPrice = model.LongPickPrice;
            this.LongTransferRate = model.LongTransferRate;
            this.LowestPrice = model.LowestPrice;
            this.LPID = model.LPID;
            this.Priority = model.Priority;
            this.Status = model.Status;
            this.TransportType = model.TransportType;
            this.UpdateBy = model.UpdateBy;
            this.UpdateTime = model.UpdateTime;
            this.BusinessType = model.BusinessType;
        }

        [Display(Name = "线路主键ID")]
        public override int LPID { get; set; }

        [Display(Name = "线路计划编号")]
        public override string LineID { get; set; }

        [Required]
        [Display(Name = "出发地")]
        public override int DepartureID { get; set; }

        [Required]
        [Display(Name = "出发地")]
        public override string DepartureName { get; set; }

        [Required]
        [CompareValue("DepartureID", ValidationCompareOperator.NotEqual,
            ValidationDataType.Integer, ErrorMessage = "发货地与目的地不能相同")]
        [Display(Name = "目的地")]
        public override int ArrivalID { get; set; }

        [Required]
        [Display(Name = "目的地")]
        public override string ArrivalName { get; set; }

        [Required]
        [Display(Name = "承运商")]
        public override int CarrierID { get; set; }

        public override string CarrierName { get; set; }

        [Required]
        [Display(Name = "运输方式")]
        public override Enums.TransportType TransportType { get; set; }

        [Required]
        [Display(Name = "到库考核点")]
        public override DateTime ArrivalAssessmentTime { get; set; }

        [Required]
        [CompareValue("ArrivalAssessmentTime", ValidationCompareOperator.GreaterThan,
            ValidationDataType.Date, ErrorMessage = "离库考核时间不得早于提货到库时间")]
        [Display(Name = "离库考核点")]
        public override DateTime LeaveAssessmentTime { get; set; }

        [Required]
        [RegularExpression(@"^(([0-9]{1,16}\.[0-9]{1,2})|([0-9]{1,16}))$", ErrorMessage = "到货时效格式不正确,整数位数不能超过16位,小数位数不能超过两位")]
        [Display(Name = "到货时效")]
        public override decimal ArrivalTiming { get; set; }

        [Required]
        [RegularExpression(@"^(([0-9]{1,16}\.[0-9]{1,4})|([0-9]{1,16}))$", ErrorMessage = "保险费率格式不正确,整数位数不能超过16位,小数位数不能超过4位")]
        [Display(Name = "保险费率")]
        public override decimal InsuranceRate { get; set; }

        [Required]
        [RegularExpression(@"^(([0-9]{1,16}\.[0-9]{1,2})|([0-9]{1,16}))$", ErrorMessage = "最低收费格式不正确,整数位数不能超过16位,小数位数不能超过两位")]
        [Display(Name = "最低收费")]
        public override decimal LowestPrice { get; set; }

        [Required]
        [RegularExpression(@"^(([0-9]{1,16}\.[0-9]{1,2})|([0-9]{1,16}))$", ErrorMessage = "超远送货费用格式不正确,整数位数不能超过16位,小数位数不能超过两位")]
        [Display(Name = "超远送货费用")]
        public override decimal LongDeliveryPrice { get; set; }

        [Required]
        [RegularExpression(@"^(([0-9]{1,16}\.[0-9]{1,2})|([0-9]{1,16}))$", ErrorMessage = "超远提货费格式不正确,整数位数不能超过16位,小数位数不能超过两位")]
        [Display(Name = "超远提货费")]
        public override decimal LongPickPrice { get; set; }

        [Required]
        [RegularExpression(@"^(([0-9]{1,16}\.[0-9]{1,2})|([0-9]{1,16}))$", ErrorMessage = "超远转运费率格式不正确,整数位数不能超过16位,小数位数不能超过两位")]
        [Display(Name = "超远转运费率")]
        public override decimal LongTransferRate { get; set; }

        [Required]
        [Display(Name = "优先级别")]
        public override Enums.LinePriority Priority { get; set; }

        //[Display(Name = "状态")]
        //public string Status { get; set; }

        //[Display(Name = "最后审核人")]
        //public string AuditBy { get; set; }

        //[Display(Name = "最后审核时间")]
        //public string AuditTime { get; set; }

        [Required]
        [Display(Name = "城际线路类型")]
        public override Enums.LineType LineType { get; set; }

        //[Required]
        [Display(Name = "货物类型")]
        public override Enums.GoodsType LineGoodsType { get; set; }

        [Required]
        [Display(Name = "运费类型")]
        public override Enums.ExpressionType ExpressionType { get; set; }

        [Display(Name = "生效时间")]
        public override DateTime EffectiveTime { get; set; }

        [Display(Name = "营业类型")]
        public override Enums.BusinessType BusinessType { get; set; }

        //[Display(Name = "创建人")]
        //public string CreateBy { get; set; }

        //[Display(Name = "创建时间")]
        //public string CreateTime { get; set; }

        #region 扩展属性

        [Required]
        [Display(Name = "线路运费")]
        public string LineExpression { get; set; }

        #endregion

    }
}