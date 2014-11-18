using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.Spot
{
    public class SiteAssSearchModel :BaseSearchModel
    {
        /// <summary>
        /// 发货仓库
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        public string CarrierID { get; set; }

        /// <summary>
        /// 目的站
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public Enums.DeliveryStatus?  DeliveryStatus { get; set; }

        /// <summary>
        /// 目的站省份
        /// </summary>
        public int ArrivalProvince { get; set; }


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
        /// 调度创建时间始
        /// </summary>
        public DateTime? CreateDateBegin { get; set; }

        /// <summary>
        /// 调度创建时间终
        /// </summary>
        public DateTime? CreateDateEnd { get; set; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public Enums.LineType? LineType { get; set; }
    }
}
