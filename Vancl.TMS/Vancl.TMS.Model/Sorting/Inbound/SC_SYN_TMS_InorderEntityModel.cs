using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 分拣同步到TMS城际运输【逐单入库】实体对象
    /// </summary>
    public class SC_SYN_TMS_InorderEntityModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public String SSTID { get; set; }

        /// <summary>
        /// 系统运单号
        /// </summary>
        public String FormCode { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public String CustomerOrder { get; set; }

        /// <summary>
        /// 入库分拣中心ID
        /// </summary>
        public int InboundID { get; set; }

        /// <summary>
        /// SC分拣同步到TMS的同步时间
        /// </summary>
        public DateTime? SC2TMSSyncTime { get; set; }

        /// <summary>
        /// SC分拣同步到TMS标记
        /// </summary>
        public Enums.SC2TMSSyncFlag SC2TMSFlag { get; set; }

        public override string ModelTableName
        {
            get
            {
                return "SC_SYN_TMS_Inorder";
            }
        }

        #region IKeyCodeable 成员

        public string SequenceName
        {
            get { return "seq_sc_syn_tms_inorder_sstid"; }
        }

        public string TableCode
        {
            get { return "010"; }
        }

        #endregion
    }
}
