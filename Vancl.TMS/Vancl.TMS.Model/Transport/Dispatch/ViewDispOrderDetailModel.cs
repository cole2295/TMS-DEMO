using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Dispatch
{
    public class ViewDispOrderDetailModel : BaseModel
    {
        /// <summary>
        /// 提货单号
        /// </summary>
        public string DeliveryNo
        {
            get;
            set;
        }

        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo
        {
            get;
            set;
        }

        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode
        {
            get;
            set;
        }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 保价金额
        /// </summary>
        public decimal ProtectedPrice { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        public Enums.GoodsType GoodsType { get; set; }
    }
}
