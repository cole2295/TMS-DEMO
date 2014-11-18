using System;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Carrier
{
    /// <summary>
    /// 承运商适用范围
    /// </summary>
    [LogName("承运商适用范围")]
    [Serializable]
    public class CoverageModel : BaseModel, ISequenceable, IOperateLogable, IForceLog
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int CoverageID { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        public int CarrierID { get; set; }

        /// <summary>
        /// 城市ID
        /// </summary>
        public string CityID { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_COVERAGE_COVERAGEID"; }
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
