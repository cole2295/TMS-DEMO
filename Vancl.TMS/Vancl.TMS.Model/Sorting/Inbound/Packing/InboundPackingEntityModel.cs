using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.Packing
{
    /// <summary>
    /// 入库装箱表对象
    /// </summary>
    public class InboundPackingEntityModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string IPID { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 箱子数量
        /// </summary>
        public int BillCount { get; set; }

        /// <summary>
        /// 入库类型
        /// </summary>
        public Enums.SortCenterOperateType InboundType { get; set; }

        /// <summary>
        /// 当前操作分拣中心
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 是否已出库
        /// </summary>
        public bool IsOutbounded { get; set; }

        /// <summary>
        /// 同步标志
        /// </summary>
        public Enums.SyncStatus SyncFlag { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "SC_InboundPacking";
            }
        }

        #region IKeyCodeable 成员

        public string TableCode
        {
            get { return "005"; }
        }

        #endregion

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_SC_INBOUNDPACKING_IPID"; }
        }

        #endregion
    }
}
