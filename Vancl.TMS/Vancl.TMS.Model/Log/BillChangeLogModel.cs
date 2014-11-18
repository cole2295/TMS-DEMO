using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Log
{
    /// <summary>
    /// TMS修改日志实体对象
    /// </summary>
    [Serializable]
    public class BillChangeLogModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public String BCID { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 当前状态[正向]
        /// </summary>
        public Enums.BillStatus CurrentStatus { get; set; }

        /// <summary>
        /// 上一个状态[正向]
        /// </summary>
        public Enums.BillStatus PreStatus { get; set; }

        /// <summary>
        /// 逆向流程[操作完成后的逆向状态]
        /// </summary>
        public Enums.ReturnStatus? ReturnStatus { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public Enums.TmsOperateType OperateType { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateDept { get; set; }

        /// <summary>
        /// 当前配送商编号
        /// </summary>
        public string CurrentDistributionCode { get; set; }

        /// <summary>
        /// 目的配送商
        /// </summary>
        public string ToDistributionCode { get; set; }

        /// <summary>
        /// 运单分配的配送站点
        /// </summary>
        public int DeliverStationID { get; set; }

        /// <summary>
        /// 目的部门
        /// </summary>
        public int ToExpressCompanyID { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 同步状态
        /// </summary>
        public Enums.SyncStatus SyncFlag { get; set; }

        /// <summary>
        /// 同步时间
        /// </summary>
        public DateTime? SyncTime { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// 客户端信息
        /// </summary>
        public string ClientInfo { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "SC_BillChangeLog";
            }
        }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_SC_BillChangeLog_BCID"; }
        }

        #endregion

        #region IKeyCodeable 成员

        public string TableCode
        {
            get { return "003"; }
        }

        #endregion
    }
}
