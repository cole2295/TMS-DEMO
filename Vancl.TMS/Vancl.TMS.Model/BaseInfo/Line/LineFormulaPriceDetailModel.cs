using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    /// <summary>
    /// 线路公式价格续价
    /// </summary>
    [Serializable]
    [LogNameAttribute("线路公式价格续价")]
    public class LineFormulaPriceDetailModel : BaseModel, ISequenceable, IForceLog, IOperateLogable
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int LFEID { get; set; }

        /// <summary>
        /// 线路ID
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

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_LineFormulaEx_LFEID"; }
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
