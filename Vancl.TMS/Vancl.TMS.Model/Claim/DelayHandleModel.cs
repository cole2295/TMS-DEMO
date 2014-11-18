using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.Claim
{
    /// <summary>
    /// 到货延误复议
    /// </summary>
    [Serializable]
    public class DelayHandleModel : BaseModel, IOperateLogable, ISequenceable
    {
        public string PrimaryKey
        {
            get { return DHID.ToString(); }
            set { DHID = int.Parse(value); }
        }

        /// <summary>
        /// 复议主键ID
        /// </summary>
        public long DHID { get; set; }

        /// <summary>
        /// 延误主键ID
        /// </summary>
        [LogName("延误主键ID")]
        public long DelayID { get; set; }

        /// <summary>
        /// 复议理由
        /// </summary>
        [LogName("复议理由")]
        public virtual string NOTE { get; set; }

        /// <summary>
        /// 审核状态(0.未审核，1.已审核)
        /// </summary>
        [LogName("审核状态")]
        public Enums.ApproveStatus ApproveStatus { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [LogName("审核人")]
        public int ApproveBY { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [LogName("审核时间")]
        public DateTime ApproveTime { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_DELAYHANDLE_DHID"; }
        }

        #endregion
    }
}
