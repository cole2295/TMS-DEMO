using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Sorting.Inbound.Packing
{
    /// <summary>
    /// 入库装箱明细表对象
    /// </summary>
    public class InboundPackingDetailModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string IPDID { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 同步标志
        /// </summary>
        public Enums.SyncStatus SyncFlag { get; set; }

        #region IKeyCodeable 成员

        public string TableCode
        {
            get { return "006"; }
        }

        #endregion

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "seq_sc_inpackingdetail_ipdid"; }
        }

        #endregion
    }
}
