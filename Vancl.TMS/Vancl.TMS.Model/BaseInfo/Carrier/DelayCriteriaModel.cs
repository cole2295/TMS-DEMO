using System;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Carrier
{
    /// <summary>
    /// 延误考核标准
    /// </summary>
    [LogName("延误考核标准")]
    [Serializable]
    public class DelayCriteriaModel : BaseModel, ISequenceable,IOperateLogable, IForceLog
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long DCID { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        public int CarrierID { get; set; }

        /// <summary>
        /// 开始区间
        /// </summary>
        public int StartRegion { get; set; }

        /// <summary>
        /// 结束区间
        /// </summary>
        public int? EndRegion { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_DELAYCRITERIA_DCID"; }
        }

        #endregion

        #region ILogable 成员

        public string PrimaryKey
        {
            get
            {
                return CarrierID.ToString();
            }
            set
            {
                CarrierID = Int32.Parse(value);
            }
        }

        #endregion
    }
}
