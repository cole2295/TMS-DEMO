using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    /// <summary>
    /// 提货单考核信息
    /// </summary>
    [Serializable]
    public class DeliveryAssessmentModel : BaseModel, IOperateLogable
    {
        #region ILogable 成员

        public string PrimaryKey
        {
            get { return DeliveryNo; }
            set { DeliveryNo = value; }
        }

        #endregion

        /// <summary>
        /// 提货单号(主键)
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 保险费率
        /// </summary>
        public decimal InsuranceRate { get; set; }

        /// <summary>
        /// 保险费
        /// </summary>
        public decimal InsuranceAmount { get; set; }

        /// <summary>
        /// 基准运费
        /// </summary>
        public decimal BaseAmount { get; set; }

        /// <summary>
        /// 应付运费
        /// </summary>
        public decimal NeedAmount { get; set; }

        /// <summary>
        /// 超远转运费率
        /// </summary>
        public decimal LongTransferRate { get; set; }

        /// <summary>
        /// 超远转运费(超远转运费率*重量)
        /// </summary>
        public decimal LongTransferAmount { get; set; }

        /// <summary>
        /// 超远送货费
        /// </summary>
        public decimal LongDeliveryAmount { get; set; }

        /// <summary>
        /// 超远提货费用
        /// </summary>
        public decimal LongPickPrice { get; set; }

        /// <summary>
        /// 运费补足
        /// </summary>
        public decimal ComplementAmount { get; set; }

        /// <summary>
        /// KPI延误计费类型
        /// </summary>
        public Enums.KPIDelayType KPIDelayType { get; set; }

        /// <summary>
        /// 延误金额
        /// </summary>
        public decimal? DelayAmount { get; set; }

        /// <summary>
        /// 延误折扣
        /// </summary>
        public decimal? Discount { get; set; }

        /// <summary>
        /// 丢失应扣款
        /// </summary>
        public decimal LostDeduction { get; set; }

        /// <summary>
        /// 审核费用
        /// </summary>
        public decimal ApprovedAmount { get; set; }

        /// <summary>
        /// 运费类型
        /// </summary>
        public  Enums.ExpressionType ExpressionType { get; set; }

        /// <summary>
        /// 结算费用
        /// </summary>
        public decimal ConfirmedAmount { get; set; }


        /// <summary>
        /// 是否给予延误考核
        /// </summary>
        public bool IsDelayAssess { get; set; }

        /// <summary>
        /// 其他金额
        /// </summary>
        public decimal OtherAmount { get; set; }

    
    }
}
