using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 运单装车实体对象
    /// </summary>
    public class WaybillTruckEntityModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 订单装车批次号
        /// </summary>
        public long BatchNO { set; get; }

        /// <summary>
        /// 订单装车自增编号
        /// </summary>
        public long WaybillNOGpsID { set; get; }

        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNO { set; get; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string TruckNO { set; get; }

        /// <summary>
        /// GPS号
        /// </summary>
        public string GpsID { set; get; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy { set; get; }

        /// <summary>
        /// 装车时间
        /// </summary>
        public DateTime CreateTime { set; get; }

        /// <summary>
        /// 更新人
        /// </summary>
        public int UpdateBy { set; get; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { set; get; }

        /// <summary>
        /// 删除标记
        /// </summary>
        public bool IsDelete { set; get; }


        /// <summary>
        /// 司机ID
        /// </summary>
        public int DriverId { set; get; }


        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "Seq_WaybillTruck_ID"; }
        }

        #endregion
    }
}
