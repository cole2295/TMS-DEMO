using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Claim
{
    public class ExpectDelaySearchModel : BaseSearchModel
    {

        private string _DeliveryNO;
        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNO
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_DeliveryNO))
                {
                    return _DeliveryNO;
                }
                return _DeliveryNO.ToUpper();
            }
            set
            {
                _DeliveryNO = value;
            }
        }

        private string _CarrierWaybillNO;
        /// <summary>
        /// 物流运单号
        /// </summary>
        public string CarrierWaybillNO
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CarrierWaybillNO))
                {
                    return _CarrierWaybillNO;
                }
                return _CarrierWaybillNO.ToUpper();
            }
            set
            {
                _CarrierWaybillNO = value;
            }
        }

        /// <summary>
        /// 发货开始时间
        /// </summary>
        public DateTime? CreateDateBegin { get; set; }

        /// <summary>
        /// 发货结束时间
        /// </summary>
        public DateTime? CreateDateEnd { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public Enums.ApproveStatus? ApproveStatus { get; set; }

        /// <summary>
        /// 是否是申请，否则为审核
        /// </summary>
        public bool IsApply { get; set; }

        /// <summary>
        /// 预计到货时间开始
        /// </summary>
        public DateTime? ArrivalTimeBegin { get; set; }

        /// <summary>
        /// 预计到货时间结束
        /// </summary>
        public DateTime? ArrivalTimeEnd { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int? DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int? ArrivalID { get; set; }

        /// <summary>
        /// 是否录入
        /// </summary>
        public bool? IsInput { get; set; }
    }
}
