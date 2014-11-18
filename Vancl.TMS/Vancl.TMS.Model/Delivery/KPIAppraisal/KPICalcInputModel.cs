using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    /// <summary>
    /// KPI考核费用计算输入参数
    /// </summary>
    public class KPICalcInputModel
    {
        #region 私有字段
        private decimal _InsuranceRate;
        private decimal _LongDeliveryAmount;
        private decimal _LongPickPrice;
        private decimal _LongTransferRate;
        private decimal _OtherAmount;
        private decimal _LostDeduction;
        #endregion

        public KPICalcInputModel()
            : this(false)
        {

        }

        public KPICalcInputModel(bool init)
        {
            IsInit = init;
        }

        /// <summary>
        /// 是否没有计算过KPI，第一次初始化
        /// </summary>
        public bool IsInit
        {
            get;
            private set;
        }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 运费
        /// </summary>
        public List<IAssPriceModel> AssPriceList { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 运费类型
        /// </summary>
        public Enums.ExpressionType ExpressionType { get; set; }

        /// <summary>
        /// 保险费率
        /// </summary>
        public decimal InsuranceRate
        {
            get
            {
                return Math.Round(_InsuranceRate, 4);
            }
            set
            {
                _InsuranceRate = value;
            }
        }

        /// <summary>
        /// 超远送货费
        /// </summary>
        public decimal LongDeliveryAmount
        {
            get
            {
                return Math.Round(_LongDeliveryAmount, 2);
            }
            set
            {
                _LongDeliveryAmount = value;
            }
        }
        /// <summary>
        /// 超远提货费用
        /// </summary>
        public decimal LongPickPrice
        {
            get
            {
                return Math.Round(_LongPickPrice, 2);
            }
            set
            {
                _LongPickPrice = value;
            }
        }

        /// <summary>
        /// 超远转运费率
        /// </summary>
        public decimal LongTransferRate
        {
            get
            {
                return Math.Round(_LongTransferRate, 2);
            }
            set
            {
                _LongTransferRate = value;
            }
        }


        /// <summary>
        /// KPI考核【延误计费类型】
        /// </summary>
        public Enums.KPIDelayType? KPIDelayType { get; set; }

        /// <summary>
        /// KPI考核【延误折扣或者延误金额】
        /// </summary>
        public decimal? Discount { get; set; }

        /// <summary>
        /// 其他金额
        /// </summary>
        public decimal OtherAmount
        {
            get 
            {
                return Math.Round(_OtherAmount, 2);
            }
            set
            {
                _OtherAmount = value;
            }
        }

        /// <summary>
        /// 丢失应扣款
        /// </summary>
        public decimal LostDeduction
        {
            get
            {
                return Math.Round(_LostDeduction, 2);
            }
            set
            {
                _LostDeduction = value;
            }
        }

        /// <summary>
        /// 是否延误考核
        /// </summary>
        public bool IsDelayAssess { get; set; }

    }
}
