using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Delivery.KPIAppraisal
{
    /// <summary>
    /// KPI费用输出对象
    /// </summary>
    public class KPICalcOutputModel
    {
        #region 私有字段
        private decimal _InsuranceAmount;
        private decimal _BaseAmount;
        private decimal _LongTransferAmount;
        private decimal _NeedAmount;
        private decimal _ComplementAmount;
        private decimal _ApprovedAmount;
        private decimal _LongPickPrice;
        private decimal _ConfirmedAmount;
        #endregion

        /// <summary>
        /// 保险费 = 保价金额 * 保险费率
        /// </summary>
        public decimal InsuranceAmount
        {
            get
            {
                return Math.Round(_InsuranceAmount, 2);
            }
            set
            {
                _InsuranceAmount = value;
            }
        }

        /// <summary>
        /// 基准运费 = 运输单价 * 重量
        /// </summary>
        public decimal BaseAmount
        {
            get
            {
                return Math.Round(_BaseAmount, 2);
            }
            set
            {
                _BaseAmount = value;            
            }
        }

        /// <summary>
        /// 超远转运费 = 超远转运费率 * 重量
        /// </summary>
        public decimal LongTransferAmount
        {
            get
            {
                return Math.Round(_LongTransferAmount, 2);
            }
            set
            {
                _LongTransferAmount = value;
            }
        }

        /// <summary>
        /// 应付运费 = 基准运费 + 保险费+ 超远转运费 + 超远送货费 + 超远提货费用 + 运费补足
        /// </summary>
        public decimal NeedAmount
        {
            get
            {
                return Math.Round(_NeedAmount, 2);
            }
            set
            {
                _NeedAmount = value;
            }
        }

        /// <summary>
        /// 运费补足
        /// </summary>
        public decimal ComplementAmount
        {
            get
            {
                return Math.Round(_ComplementAmount, 2);
            }
            set
            {
                _ComplementAmount = value;
            }
        }

        /// <summary>
        /// 审核费用 = 延误折扣 * 应付运费 | 应付运费 - 延误扣款
        /// </summary>
        public decimal ApprovedAmount
        {
            get 
            {
                return Math.Round(_ApprovedAmount, 2);
            }
            set
            {
                _ApprovedAmount = value;
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
        /// 结算费用 = 审核费用 - 丢失扣款 + 其他金额
        /// </summary>
        public decimal ConfirmedAmount
        {
            get
            {
                return Math.Round(_ConfirmedAmount, 2);
            }
            set
            {
                _ConfirmedAmount = value;
            }
        }

    }
}
