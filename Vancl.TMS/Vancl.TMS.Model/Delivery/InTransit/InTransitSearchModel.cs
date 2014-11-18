using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Delivery.InTransit
{
    public class InTransitSearchModel : BaseSearchModel
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

        private string _WaybillNO;
        /// <summary>
        /// 物流运单号
        /// </summary>
        public string WaybillNO
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_WaybillNO))
                {
                    return _WaybillNO;
                }
                return _WaybillNO.ToUpper();
            }
            set
            {
                _WaybillNO = value;
            }
        }

        /// <summary>
        /// 城际承运商
        /// </summary>
        public int? CarrierID { get; set; }

        /// <summary>
        /// 目的城市
        /// </summary>
        public string ArrivalCity { get; set; }

        /// <summary>
        /// 发货开始时间
        /// </summary>
        public DateTime? DeliveryTimeBegin { get; set; }

        /// <summary>
        /// 发货结束时间
        /// </summary>
        public DateTime? DeliveryTimeEnd { get; set; }

        /// <summary>
        /// 预计到货开始时间
        /// </summary>
        public DateTime? ExpectTimeBegin { get; set; }

        /// <summary>
        /// 预计到货结束时间
        /// </summary>
        public DateTime? ExpectTimeEnd { get; set; }
    }
}
