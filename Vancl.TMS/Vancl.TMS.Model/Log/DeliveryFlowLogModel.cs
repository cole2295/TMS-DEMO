using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// 提货单日志实体对象
    /// </summary>
    [Serializable]
    public class DeliveryFlowLogModel : BaseLogModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long DFLID { get; set; }
        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNO { get; set; }
        /// <summary>
        /// 运输流程类型
        /// </summary>
        public Enums.DeliverFlowType FlowType { get; set; }
        
        /// <summary>
        /// 是否官网显示
        /// </summary>
        public bool IsShow { get; set; }
        
        /// <summary>
        /// 同步城际运输数据到LMS数据库
        /// </summary>
        public Enums.SyncStatus? SyncToLMS { get; set; }

        /// <summary>
        /// 提货单状态
        /// </summary>
        public Enums.DeliveryStatus? DeliveryStatus { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_DeliveryFlowLog_DFLID"; }
        }

        #endregion
    }
}
