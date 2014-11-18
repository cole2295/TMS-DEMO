using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Transport.Dispatch
{
    public class DispOrderDetailModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long DODID
        {
            get;
            set;
        }

        /// <summary>
        /// 调度明细表ID
        /// </summary>
        public long DDID
        {
            get;
            set;
        }

        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo
        {
            get;
            set;
        }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo
        {
            get;
            set;
        }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode
        {
            get;
            set;
        }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID
        {
            get;
            set;
        }

        /// <summary>
        /// 是否到达目的地
        /// </summary>
        public bool IsArrived
        {
            get;
            set;
        }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Price
        {
            get;
            set;
        }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_DISPORDERDETAIL_DODID"; }
        }

        #endregion
    }
}
