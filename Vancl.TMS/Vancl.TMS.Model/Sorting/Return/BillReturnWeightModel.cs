using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.Sorting.Return
{
    /// <summary>
    /// 退货分拣称重模型
    /// </summary>
    public class BillReturnWeightModel
    {
        /// <summary>
        /// 单号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public string BoxNo { get; set; }

        /// <summary>
        /// 选择的商家或者分拣中心（配送商）类型
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
        /// 选择的单号类型
        /// </summary>
        public int FormType { get; set; }

        /// <summary>
        /// 已经成功扫描的运单号
        /// </summary>
        public string hidFormCodes { get; set; }
        
        }
}
