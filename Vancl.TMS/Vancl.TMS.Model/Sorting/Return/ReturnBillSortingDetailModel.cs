using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 返货入库箱号明细模型
    /// </summary>
    public class ReturnBillSortingDetailModel:BaseModel
    {
        //序号
        public int rNow { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }
        /// <summary>
        /// 系统运单号
        /// </summary>
        public long FormCode { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public string formType { get; set; }
        /// <summary>
        /// 应退金额
        /// </summary>
        public decimal NeedAmount { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 操作部门
        /// </summary>
        public string CreateDept { get; set; }
        /// <summary>
        /// 返货目的地
        /// </summary>
        public string ReturnTo { get; set; }
    }
}
