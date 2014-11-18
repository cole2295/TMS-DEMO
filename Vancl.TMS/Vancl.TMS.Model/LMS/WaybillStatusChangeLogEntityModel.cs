using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 状态变更实体对象
    /// </summary>
    public class WaybillStatusChangeLogEntityModel:BaseModel,ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long WaybillStatusChangeLogID { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNO { get; set; }

        /// <summary>
        /// 操作环节
        /// </summary>
        public Enums.StatusChangeNodeType CurNode { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 正向[正向状态BillStatus],逆向[逆向状态ReturnStatus]
        /// </summary>
        public int SubStatus { get; set; }

        /// <summary>
        /// 商家编号
        /// </summary>
        public int MerchantID { get; set; }

        /// <summary>
        /// 配送商Code
        /// </summary>
        public String DistributionCode { get; set; }

        /// <summary>
        /// 配送站点编号
        /// </summary>
        public int DeliverStationID { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateDept { get; set; }

        /// <summary>
        /// 是否已同步
        /// </summary>
        public bool IsSyn { get; set; }
        
        /// <summary>
        /// 描述
        /// </summary>
        public String Note { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public String CustomerOrder { get; set; }

        /// <summary>
        /// BI相关
        /// </summary>
        public bool? IsBISyn { get; set; }

        /// <summary>
        /// 状态变更
        /// </summary>
        public String LMS_WaybillStatusChangeLogKid { get; set; }

        public String IsM2sSyn { get; set; }

        /// <summary>
        /// 操作模块
        /// </summary>
        public Enums.Lms2TmsOperateType OperateType { get; set; }

        /// <summary>
        /// TMS同步标记
        /// </summary>
        public Enums.SyncStatus TmsSyncStatus { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "lms_waybillstatuschangelog";
            }
        }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_lms_waybillstatuschangelog"; }
        }

        #endregion
    }
}
