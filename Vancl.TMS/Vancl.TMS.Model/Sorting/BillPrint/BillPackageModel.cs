using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
    /// <summary>
    /// 称重实体对象
    /// </summary>
    [Serializable]
    public class BillPackageModel : BaseModel, IKeyCodeable
    {
        /// <summary>
        /// 称重表主键
        /// </summary>
        public string BWID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }
        /// <summary>
        /// 包装序号（原LMS箱号）
        /// </summary>
        public int PackageIndex { get; set; }
        /// <summary>
        /// 称重重量
        /// </summary>
        public decimal? Weight { get; set; }

        public string TableCode
        {
            get { return "008"; }
        }

        public Vancl.TMS.Model.Common.Enums.SyncStatus SyncFlag { get; set; }

        public string SequenceName
        {
            get { return "seq_sc_billweigh_bwid"; }
        }
    }
}
