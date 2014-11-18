using System;
using System.Collections.Generic;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    /// <summary>
    /// 线路阶梯价格
    /// </summary>
    [Serializable]
    [LogNameAttribute("线路阶梯价格")]
    public class LineLadderPriceModel : BaseModel, ISequenceable, ILinePrice, IForceLog, IOperateLogable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long LLPID { get; set; }

        /// <summary>
        /// 线路主键ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 开始重量
        /// </summary>
        public int StartWeight { get; set; }

        /// <summary>
        /// 结束重量
        /// </summary>
        public int? EndWeight { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Note { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_LineLadderPrice_LLPID"; }
        }

        #endregion

        #region ILogable 成员

        public string PrimaryKey
        {
            get
            {
                return LPID.ToString();
            }
            set
            {
                LPID = Convert.ToInt32(value);
            }
        }

        #endregion
    }
}
