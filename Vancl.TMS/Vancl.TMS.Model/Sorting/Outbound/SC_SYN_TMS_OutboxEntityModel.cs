using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 分拣同步到TMS城际运输【出库】实体对象
    /// </summary>
    public class SC_SYN_TMS_OutboxEntityModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public String SSTOID { get; set; }

        /// <summary>
        /// 箱号、批次号
        /// </summary>
        public String BoxNo { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 分拣到TMS城际运输同步标志
        /// </summary>
        public Enums.SyncStatus SC2TMSFlag { get; set; }

        /// <summary>
        /// 分拣到TMS城际运输同步时间
        /// </summary>
        public DateTime SC2TMSSyncTime { get; set; }

        /// <summary>
        /// 编号类型
        /// </summary>
        public Enums.SyncNoType NoType { get; set; }


        public override string ModelTableName
        {
            get
            {
                return "SC_SYN_TMS_Outbox";
            }
        }

        #region IKeyCodeable 成员

        public string SequenceName
        {
            get { return "seq_sc_syn_tms_outbox_sstoid"; }
        }

        public string TableCode
        {
            get { return "011"; }
        }

        #endregion
    }
}
