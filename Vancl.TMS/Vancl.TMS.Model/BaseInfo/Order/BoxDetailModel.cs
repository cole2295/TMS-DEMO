using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.BaseInfo.Order
{
    /// <summary>
    /// 箱明细表
    /// </summary>
    [Serializable]
    public class BoxDetailModel : BaseModel, ISequenceable
    {
        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_BOXDETAIL_BDID"; }
        }

        #endregion

        /// <summary>
        /// 主键ID
        /// </summary>
        public long BDID { get; set; }

        /// <summary>
        /// 批次号，箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }
    }
}
