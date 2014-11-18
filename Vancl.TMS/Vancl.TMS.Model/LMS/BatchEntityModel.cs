using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 物流主库批次表实体对象
    /// </summary>
    public class BatchEntityModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long BatchID { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 总计金额 
        /// </summary>
        [Obsolete]
        public decimal? Amount { get; set; }

        /// <summary>
        /// 包裹数量
        /// </summary>
        [Obsolete]
        public int? PackageNum { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        [Obsolete]
        public float? Weight { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        public int BatchOperator { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OperTime { get; set; }

        /// <summary>
        /// 操作站点
        /// </summary>
        public int OperStation { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        [Obsolete]
        public int? ReceiveBy { get; set; }

        /// <summary>
        /// 接收时间
        /// </summary>
        [Obsolete]
        public DateTime? ReceiveTime { get; set; }

        /// <summary>
        /// 接收目的地站点ID
        /// </summary>
        public int ReceiveStation { get; set; }

        /// <summary>
        /// 配送员
        /// </summary>
        [Obsolete]
        public int? Driver { get; set; }

        /// <summary>
        /// 车号
        /// </summary>
        [Obsolete]
        public String CarNO { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateDept { get; set; }

        /// <summary>
        /// 最后更新站点
        /// </summary>
        public int UpdateDept { get; set; }

        /// <summary>
        /// 是否已经发过邮件
        /// </summary>
        [Obsolete]
        public bool? IsSendEmail { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "Batch";
            }
        }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_batch_batchid"; }
        }

        #endregion
    }
}
