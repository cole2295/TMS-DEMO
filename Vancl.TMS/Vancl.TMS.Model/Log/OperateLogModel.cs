using System;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// 操作日志实体对象
    /// </summary>
    [Serializable]
    public class OperateLogModel : BaseLogModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long OLID { get; set; }
        /// <summary>
        /// 操作相关表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public Enums.LogOperateType OperateType { get; set; }
        /// <summary>
        /// 操作主键值
        /// </summary>
        public string KeyValue { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_OperateLog_OLID"; }
        }

        #endregion
    }
}
