using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Transport.Dispatch
{
    /// <summary>
    /// 提货单交接信息
    /// </summary>
    public class DispTransitionModel : BaseModel, IOperateLogable, ISequenceable
    {
        #region ILogable 成员

        public string PrimaryKey
        {
            get { return DTID.ToString(); }
            set { DTID = long.Parse(value); }
        }

        #endregion

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_DISPTRANSITION_DTID"; }
            
        }

        #endregion
        
        /// <summary>
        /// 主键ID
        /// </summary>
        public virtual long DTID { get; set; }

        /// <summary>
        /// 提货单号
        /// </summary>
        public virtual string DeliveryNo { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public virtual string PlateNo { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        public virtual string Consignor { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public virtual string Consignee { get; set; }

        /// <summary>
        /// 收货人电话
        /// </summary>
        public virtual string ConsigneePhone { get; set; }

        /// <summary>
        /// 收货人地址
        /// </summary>
        public virtual string ReceiveAddress { get; set; }

    }
}
