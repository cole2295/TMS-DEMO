using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.Model.Sorting.Inbound
{
    /// <summary>
    /// 入库运单对象
    /// </summary>
    public class InboundBillModel : BillModel
    {
        /// <summary>
        /// 是否处于入库中
        /// </summary>
        public bool IsInbounding { get; set; }

        /// <summary>
        /// 客户收货地区
        /// </summary>
        public String ReceiveArea { get; set; }

        /// <summary>
        /// 客户移动电话
        /// </summary>
        public String ReceiveMobile { get; set; }

        /// <summary>
        /// 是否第一次分拣入库
        /// </summary>
        public bool IsFirstInbound { get; set; }

        /// <summary>
        /// 是否需要判断打印部门
        /// </summary>
        public bool IsJudgePrint { get; set; }

        /// <summary>
        /// 是否需要验证面单单号与商家单号对应
        /// </summary>
        public bool IsValidateBill { get; set; }

        /// <summary>
        /// 当前操作分拣中心
        /// </summary>
        public int DepartureID { get; set; }

    }
}
