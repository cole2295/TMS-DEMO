using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Delivery.InTransit
{
    public class ViewSetArriveModel : BaseModel
    {
        /// <summary>
        /// 调度ID
        /// </summary>
        public long DispatchID { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 物流运单号
        /// </summary>
        public string WaybillNo { get; set; }

        /// <summary>
        /// 预计到货时间
        /// </summary>
        public DateTime ExpectArrivalDate { get; set; }

        /// <summary>
        /// 预计到货日期
        /// </summary>
        public DateTime ConfirmExpArrivalDate { get; set; }

        /// <summary>
        /// 目的地接收日期
        /// </summary>
        public DateTime? DesReceiveDate { get; set; }

        /// <summary>
        /// 是否预计延误
        /// </summary>
        public bool IsExpectDelay { get; set; }

        /// <summary>
        /// 预计延误时长
        /// </summary>
        public int ExpectDelayTime { get; set; }

        /// <summary>
        /// 预计延误类型
        /// </summary>
        public Enums.ExpectDelayType ExpectDelayType { get; set; }

        /// <summary>
        /// 预计延误说明
        /// </summary>
        public string ExpectDelayDesc { get; set; } 
    }
}
