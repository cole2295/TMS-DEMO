using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库队列实体对象
    /// </summary>
    [Serializable]
    public class InboundQueueEntityModel : BaseModel, ISequenceable
    {

        /// <summary>
        /// 主键ID
        /// </summary>
        public long IBSID { get; set; }

        /// <summary>
        /// 系统单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 处理次数
        /// </summary>
        public int OpCount { get; set; }

        /// <summary>
        /// 出发地:当前分检中心
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 外包代理类型
        /// </summary>
        public Enums.AgentType AgentType { get; set; }

        /// <summary>
        /// 代理用户ID
        /// </summary>
        public int? AgentUserID { get; set; }

        /// <summary>
        /// 入库前一状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public Enums.SeqStatus SeqStatus { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateDept { get; set; }

        /// <summary>
        /// 所属商家
        /// </summary>
        public int MERCHANTID { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "SC_InboundQueue";
            }
        }

        public int Version { get; set;}

        public string DistributionCode { get; set; }

        public int CreateBy { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_sc_inboundqueue_ibsid"; }
        }

        #endregion
    }
}
