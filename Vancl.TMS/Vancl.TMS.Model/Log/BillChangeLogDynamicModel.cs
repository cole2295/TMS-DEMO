using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Vancl.TMS.Model.Common;
using System.Runtime.CompilerServices;

namespace Vancl.TMS.Model.Log
{
    public class BillChangeLogDynamicModel
    {
        public BillChangeLogDynamicModel()
        {
            _d = new DynamicDictionaryModel();
        }
        /// <summary>
        /// 单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public Enums.BillStatus CurrentSatus { get; set; }

        /// <summary>
        /// 上一个状态
        /// </summary>
        public Enums.BillStatus PreStatus { get; set; }

        /// <summary>
        /// 逆向状态
        /// </summary>
        public Enums.ReturnStatus ReturnStatus { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public Enums.TmsOperateType OperateType { get; set; }

        /// <summary>
        /// 当前配送商编号
        /// </summary>
        public string CurrentDistributionCode { get; set; }

		/// <summary>
		/// 目的配送商
		/// </summary>
		public string ToDistributionCode { get; set; }

		/// <summary>
		/// 目的部门
		/// </summary>
		public int ToExpressCompanyID { get; set; }

	    /// <summary>
        /// 运单分配的配送站点
        /// </summary>
        public int DeliverStationID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateDept { get; set; }

        private dynamic _d;
        /// <summary>
        /// 动态扩展对象
        /// </summary>
        public dynamic ExtendedObj
        {
            get
            {
                return _d;
            }
        }
    }
}
