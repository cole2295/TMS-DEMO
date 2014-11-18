using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Outbound
{
    /// <summary>
    /// 出库批次实体对象
    /// </summary>
    public class OutboundBatchEntityModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public String OBBID { get; set; }
        
        /// <summary>
        /// 批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 出发地
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 出库总数量
        /// </summary>
        public int OutboundCount { get; set; }

        /// <summary>
        /// 同步标记
        /// </summary>
        public Enums.SyncStatus SyncFlag { get; set; }


        public override string ModelTableName
        {
            get
            {
                return "SC_OutboundBatch";
            }
        }




        #region IKeyCodeable 成员

        public string SequenceName
        {
            get { return "seq_sc_outboundbatch_obbid"; }
        }

        public string TableCode
        {
            get { return "009"; }
        }

        #endregion
    }
}
