using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Return
{
    public class BillReturnEntityModel
    {
        private int? _boxstatus = 0;

        /// <summary>
        /// 自增ID
        /// </summary>
        public long BillReturnInfoID { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public long FormCode { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// 标签号
        /// </summary>
        public string LabelNo { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int? CreateBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新
        /// </summary>
        public int? UpdateBy { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 装箱状态（0:未装箱，1:已装箱,2:已拆箱）
        /// </summary>
        public int? BoxStatus { get; set; }
        /// <summary>
        /// 封箱帖号
        /// </summary>
        public string BoxLabelNo { get; set; }
    }
}
