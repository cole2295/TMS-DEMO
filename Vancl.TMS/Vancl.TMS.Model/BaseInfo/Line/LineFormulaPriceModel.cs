using System;
using System.Collections.Generic;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    /// <summary>
    /// 线路公式价格
    /// </summary>
    [Serializable]
    [LogName("线路公式价格")]
    public class LineFormulaPriceModel : BaseModel, IOperateLogable, ILinePrice
    {
        public string PrimaryKey
        {
            get { return LPID.ToString(); }
            set { LPID = int.Parse(value); }
        }

        /// <summary>
        /// 线路主键ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 基价
        /// </summary>
        [LogName("基价")]
        public decimal BasePrice { get; set; }

        /// <summary>
        /// 基重
        /// </summary>
        [LogName("基重")]
        public int BaseWeight { get; set; }

        /// <summary>
        /// 续价
        /// </summary>
        [Obsolete]
        [LogName("续价")]
        public decimal OverPrice { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [LogName("描述")]
        public string Note { get; set; }

        /// <summary>
        /// 公式续价
        /// </summary>
        [ColumnNot4LogAttribute]
        public List<LineFormulaPriceDetailModel> Detail { get; set; }
    }
}
