using System;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Line
{
    /// <summary>
    /// 线路固定价格
    /// </summary>
    [Serializable]
    [LogName("线路固定价格")]
    public class LineFixedPriceModel : BaseModel, IOperateLogable, ILinePrice
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
        /// 价格
        /// </summary>
        [LogName("价格")]
        public decimal Price { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [LogName("描述")]
        public string Note { get; set; }
    }
}
