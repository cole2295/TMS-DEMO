using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// 服务日志对象
    /// </summary>
    [Serializable]
    public class ServiceLogModel : BaseModel, ISequenceable
    {
        public override string ModelTableName
        {
            get
            {
                return "SC_ServiceLog";
            }
        }

        /// <summary>
        /// 自增主键
        /// </summary>
        public long LogID { get; set; }

        /// <summary>
        /// 系统单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 是否处理成功
        /// </summary>
        public bool IsSuccessed { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public Enums.ServiceLogType LogType { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int OpFunction { get; set; }

        /// <summary>
        /// 同步中间表唯一标识
        /// </summary>
        public string SyncKey { get; set; }

        /// <summary>
        /// 是否已重新同步
        /// </summary>
        public bool IsHandled { get; set; }

        /// <summary>
        /// 重新处理时间
        /// </summary>
        public DateTime? ProcessingTime { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_SC_ServiceLog_LOGID"; }
        }

        #endregion
    }
}
