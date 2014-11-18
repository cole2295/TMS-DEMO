using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 退货分拣称重扫描单号参数模型
    /// </summary>
    public class ReturnScanFormCodeModel
    {
        /// <summary>
        /// 隐藏域
        /// </summary>
        public string hidData { get; set; }
        /// <summary>
        /// 当前扫描的单号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 选择的单号类型：1--运单号，2--订单号
        /// </summary>
        public string selectedType { get; set; }

        /// <summary>
        /// 选择商家或分拣中心（配送商）名称
        /// </summary>
        public string selectStationName { get; set; }
        /// <summary>
        /// 选择的站点或商家ID
        /// </summary>
        public string selectStationValue { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 返货目的地
        /// </summary>
        public string ReturnTo { get; set; }
        /// <summary>
        /// 选择的单号类型：1--运单号，2--订单号
        /// </summary>
        public int FormType { get; set; }

        /// <summary>
        /// 已经成功扫描的运单号
        /// </summary>
        public string hidBillNos { get; set; }
                /// <summary>
        /// 是否是第一次扫描单号
        /// </summary>
        public bool isFirst { get; set; }

        /// <summary>
        /// 是否需要称重
        /// </summary>
        public bool isNeedWeight { get; set; }
        /// <summary>
        /// 是否要装箱称重
        /// </summary>
        public bool isInBox { get; set; }
    }
}
