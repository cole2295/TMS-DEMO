using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.CustomizeFlow
{
    /// <summary>
    /// 运单流转表
    /// </summary>
    public class WaybillTurn
    {

        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        public int FromExpressCompanyId { get; set; }

        /// <summary>
        /// 转入部门编号
        /// </summary>
        public int ToExpressCompanyId { get; set; }

        /// <summary>
        /// 配送商编码
        /// </summary>
        public string FromDistributionCode { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string ToDistributionCode { get; set; }

        /// <summary>
        /// 转类型0为没有转部门，1为部门转，2为配送商转
        /// </summary>
        public Enums.TurnType TurnType { get; set; }
    }
}
