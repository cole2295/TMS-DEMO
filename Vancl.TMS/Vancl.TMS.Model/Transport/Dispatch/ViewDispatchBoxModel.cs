using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Transport.Dispatch
{
    /// <summary>
    /// 运输调度箱视图对象
    /// </summary>
    public class ViewDispatchBoxModel : BaseModel
    {
        /// <summary>
        /// 出发地ID
        /// </summary>
        public int DepartureID
        {
            get;
            set;
        }

        /// <summary>
        /// 出发地名称
        /// </summary>
        public string DepartureName
        {
            get;
            set;
        }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID
        {
            get;
            set;
        }

        /// <summary>
        /// 目的地名称
        /// </summary>
        public string ArrivalName
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
        /// 订单数量
        /// </summary>
        public int OrderCount
        {
            get;
            set;
        }

        /// <summary>
        /// 货物类型
        /// </summary>
        public Enums.GoodsType ContentType { get; set; }
    }
}
