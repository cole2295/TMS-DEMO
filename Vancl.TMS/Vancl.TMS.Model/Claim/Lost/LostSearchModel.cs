using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Claim.Lost
{
    public class LostSearchModel : BaseSearchModel
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
        /// 发货开始时间
        /// </summary>
        public DateTime? CreateDateBegin { get; set; }

        /// <summary>
        /// 发货结束时间
        /// </summary>
        public DateTime? CreateDateEnd { get; set; }

        /// <summary>
        /// 是否是添加丢失信息查询
        /// </summary>
        public bool IsAddLost { get; set; }

        /// <summary>
        /// 丢失信息审核状态
        /// </summary>
        public Enums.ApproveStatus? ApproveStatus { get; set; }

        /// <summary>
        /// 是否录入
        /// </summary>
        public bool? IsInput { get; set; }
    }
}
