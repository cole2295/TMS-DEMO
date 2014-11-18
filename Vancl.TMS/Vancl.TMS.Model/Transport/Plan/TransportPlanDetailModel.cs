
using Vancl.TMS.Model.CustomerAttribute;
using System;
namespace Vancl.TMS.Model.Transport.Plan
{
    /// <summary>
    /// 运输计划明细
    /// </summary>
    [LogNameAttribute("运输计划明细")]
    public class TransportPlanDetailModel : BaseModel, ISequenceable, IOperateLogable, IForceLog
    {
        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_TPDetail_TPDID"; }
        }

        #endregion

        /// <summary>
        /// 主键标识
        /// </summary>
        public long TPDID { get; set; }

        /// <summary>
        /// 运输计划ID
        /// </summary>
        public int TPID { get; set; }

        /// <summary>
        /// 线路编码
        /// </summary>
        public string LineID { get; set; }

        /// <summary>
        /// 顺序号
        /// </summary>
        public int SeqNo { get; set; }

        #region ILogable 成员

        public string PrimaryKey
        {
            get
            {
                return TPID.ToString();
            }
            set
            {
                TPID = Convert.ToInt32(value);
            }
        }

        #endregion
    }
}
