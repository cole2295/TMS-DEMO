using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库实体对象
    /// </summary>
    [Serializable]
    public class InboundEntityModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 入库主键Key
        /// </summary>
        public String IBID { get; set; }

        /// <summary>
        /// 系统运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 入库类型
        /// </summary>
        public Enums.SortCenterOperateType InboundType { get; set; }

        /// <summary>
        /// 转站申请的站点ID
        /// </summary>
        public int? ApplyStation { get; set; }

        /// <summary>
        /// 分拣出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 分拣目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 同步标记
        /// </summary>
        public Enums.SyncStatus SyncFlag { get; set; }

        public string DistributionCode { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "SC_Inbound";
            }
        }

        #region IKeyCodeable 成员

        public string TableCode
        {
            get { return "001"; }
        }

        #endregion

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_sc_inbound_ibid"; }
        }

        #endregion
    }
}
