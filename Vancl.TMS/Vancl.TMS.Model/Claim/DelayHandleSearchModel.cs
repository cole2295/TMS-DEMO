using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Claim
{
    public class DelayHandleSearchModel : BaseSearchModel
    {
        private string _deliveryNO;
        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNO
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_deliveryNO))
                    return _deliveryNO;
                return _deliveryNO.ToUpper();
            }
            set
            {
                _deliveryNO = value;
            }
        }

        private string _carrierWaybillNO;
        /// <summary>
        /// 物流运单号
        /// </summary>
        public string CarrierWaybillNO
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_carrierWaybillNO))
                    return _carrierWaybillNO;
                return _carrierWaybillNO.ToUpper();
            }
            set
            {
                _carrierWaybillNO = value;
            }
        }

        /// <summary>
        /// 目的城市
        /// </summary>
        public string ArrivalCity { get; set; }

        /// <summary>
        /// 到货延误复议处理状态
        /// </summary>
        public Enums.DelayHandleApproveStatus? ApproveStatus { get; set; }

        /// <summary>
        /// 发货开始时间
        /// </summary>
        public DateTime? CreateDateBegin { get; set; }

        /// <summary>
        /// 发货结束时间
        /// </summary>
        public DateTime? CreateDateEnd { get; set; }

        /// <summary>
        /// 是否录入
        /// </summary>
        public bool? IsInput { get; set; }
    }
}
