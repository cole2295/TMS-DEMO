using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库实体对象
    /// </summary>
    public class OutboundEntityModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public String OBID { get; set; }

        /// <summary>
        /// 系统单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 出发地ID
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

		public int DeliverStationID { get; set; }

	    /// <summary>
        /// 出库类型
        /// </summary>
        public Enums.SortCenterOperateType OutboundType { get; set; }

        /// <summary>
        /// 同步标记
        /// </summary>
        public Enums.SyncStatus SyncFlag { get; set; }


        public override string ModelTableName
        {
            get
            {
                return "SC_Outbound";
            }
        }

        #region IKeyCodeable 成员

        public string SequenceName
        {
            get { return "seq_sc_outbound_obid"; }
        }

        public string TableCode
        {
            get { return "002"; }
        }

        #endregion
    }
}
