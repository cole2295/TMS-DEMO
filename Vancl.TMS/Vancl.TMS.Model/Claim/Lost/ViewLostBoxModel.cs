using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Claim.Lost
{
    public class ViewLostBoxModel : BaseModel
    {
        /// <summary>
        /// 系统批次号
        /// </summary>
        public string BoxNO { get; set; }

        /// <summary>
        /// 客户批次号
        /// </summary>
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderCount { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 箱子内订单的丢失状态
        /// </summary>
        public Enums.BoxLostStatus BoxLostStatus { get; set; }
    }
}
