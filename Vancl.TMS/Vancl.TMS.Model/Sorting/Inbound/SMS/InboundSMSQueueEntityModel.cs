using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.SMS
{
    /// <summary>
    /// 入库短信队列表
    /// </summary>
    [Serializable]
    public class InboundSMSQueueEntityModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long QUID { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 处理计数
        /// </summary>
        public int OpCount { get; set; }

        /// <summary>
        /// 当前操作分检中心
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public Enums.SeqStatus SeqStatus { get; set; }

        /// <summary>
        /// 发送短信时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 已经发送的短信内容
        /// </summary>
        public String SendedContent { get; set; }

        /// <summary>
        /// 处理错误信息
        /// </summary>
        public String ErrorInfo { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "SC_InboundSMSQueue";
            }
        }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_sc_inboundsmsqueue_quid"; }
        }

        #endregion
    }


}
