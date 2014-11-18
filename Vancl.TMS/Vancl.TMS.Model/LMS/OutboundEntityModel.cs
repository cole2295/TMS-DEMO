using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 出库实体对象
    /// </summary>
    public class OutboundEntityModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long OutboundID { get; set; }

        /// <summary>
        /// 出库编码
        /// </summary>
        [Obsolete]
        public long? OutboundNO { get; set; }

        /// <summary>
        /// 系统运单号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        [Obsolete]
        public int? TransferType { get; set; }

        /// <summary>
        /// 目的地ID 
        /// </summary>
        public int ToStation { get; set; }

        /// <summary>
        /// 接货站点负责人
        /// </summary>
        [Obsolete]
        public int? ToStationPrincipal { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int OutboundOperator { get; set; }

        /// <summary>
        /// 出库类型
        /// </summary>
        public Enums.SortCenterOperateType OutStationType { get; set; }

        /// <summary>
        /// 是否打印
        /// </summary>
        [Obsolete]
        public bool? IsPrint { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        [Obsolete]
        public DateTime? PrintTime { get; set; }

        /// <summary>
        /// 当前操作站点
        /// </summary>
        public int OutboundStation { get; set; }

        /// <summary>
        /// 交货人
        /// </summary>
        public int? DeliveryMan { get; set; }

        /// <summary>
        /// 司机
        /// </summary>
        public int? DeliveryDriver { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public DateTime OutboundTime { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateDept { get; set; }

        /// <summary>
        /// 最后更新部门
        /// </summary>
        public int UpdateDept { get; set; }

        /// <summary>
        /// 出库GUID单号
        /// </summary>
        [Obsolete]
        public String OutboundGuid { get; set; }

        /// <summary>
        /// 出库KEY
        /// </summary>
        public String OutboundKid { get; set; }


        public override string ModelTableName
        {
            get
            {
                return "Outbound";
            }
        }


        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_outbound_outboundid"; }
        }

        #endregion
    }

}
