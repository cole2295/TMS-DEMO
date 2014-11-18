using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 入库实体对象
    /// </summary>
    public class InboundEntityModel : BaseModel, ISequenceable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long InboundID { get; set; }

        /// <summary>
        /// 系统运单号
        /// </summary>
        public long WaybillNO { get; set; }

        /// <summary>
        /// 系统批次号
        /// </summary>
        public String CustomerBatchNO { get; set; }

        /// <summary>
        /// [已废弃列-暂不添加废弃标记]
        /// </summary>
        public long? InboundNO { get; set; }

        /// <summary>
        /// 转站申请的站点
        /// </summary>
        public int? FromStation { get; set; }

        /// <summary>
        /// 当前操作人
        /// </summary>
        public int CurOperator { get; set; }

        /// <summary>
        /// 入库时间
        /// </summary>
        public DateTime IntoTime { get; set; }

        /// <summary>
        /// 当前分拣中心,出发地
        /// </summary>
        public int IntoStation { get; set; }

        /// <summary>
        /// 入库类型
        /// </summary>
        public Enums.SortCenterOperateType IntoStationType { get; set; }

        /// <summary>
        /// 配送人员
        /// </summary>
        public int? DeliveryMan { get; set; }

        /// <summary>
        /// 配送时间
        /// </summary>
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 司机
        /// </summary>
        public int? DeliveryDriver { get; set; }

        /// <summary>
        /// 车牌号码
        /// </summary>
        public String DeliveryCarNO { get; set; }

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint { get; set; }

        /// <summary>
        /// 打印时间
        /// </summary>
        public DateTime? PrintTime { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateDept { get; set; }

        /// <summary>
        /// 更改人所在部门
        /// </summary>
        public int UpdateDept { get; set; }

        /// <summary>
        /// 入库目的地ID
        /// </summary>
        public int ToStation { get; set; }

        /// <summary>
        /// [废弃]
        /// </summary>
        public String InBoundGuid { get; set; }

        /// <summary>
        /// 入库KEY 
        /// </summary>
        public String InBoundKid { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "SC_Inbound";
            }
        }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_inbound_inboundid"; }
        }

        #endregion
    }
}
