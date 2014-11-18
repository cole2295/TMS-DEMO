using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Claim.Lost
{
    public class ViewLostOrderModel : BaseModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 总值
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 系统箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 箱号
        /// </summary>
        public string CustomerBoxNo { get; set; }
    }
}
