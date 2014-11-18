using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.ImportRecord
{
    /// <summary>
    /// 提货单导入记录实体对象
    /// </summary>
    public class DeliveryInRecordModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int RecordID { get; set; }

        /// <summary>
        /// 导入记录数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public Enums.DeliverySource DeliverySource { get; set; }

        /// <summary>
        /// 导入失败记录数
        /// </summary>
        public int FaultCount { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateByName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 导入批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 回执错误文件路径
        /// </summary>
        public string FilePath { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_DELIVERYINRECORD"; }
        }

        #endregion
    }
}
