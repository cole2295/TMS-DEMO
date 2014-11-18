using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.CustomerAttribute;

namespace Vancl.TMS.Model.BaseInfo.Order
{
    /// <summary>
    /// 订单明细表
    /// </summary>
    [Serializable]
    public class OrderDetailModel : BaseModel, IOperateLogable, ISequenceable
    {
        public string PrimaryKey
        {
            get { return ODID.ToString(); }
            set { ODID = long.Parse(value); }
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public long ODID { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        [LogName("单号")]
        public string FormCode { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [LogName("商品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 商品编码
        /// </summary>
        [LogName("商品编码")]
        public string ProductCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [LogName("数量")]
        public decimal ProductCount { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        [LogName("计量单位")]
        public string ProductUnit { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [LogName("单价")]
        public decimal ProductPrice { get; set; }

        /// <summary>
        /// 尺码
        /// </summary>
        [LogName("尺码")]
        public string ProductSize { get; set; }

        #region ISequenceable 成员

        public string SequenceName
        {
            get { return "SEQ_TMS_OrderDetail_ODID"; }
        }

        #endregion
    }
}
