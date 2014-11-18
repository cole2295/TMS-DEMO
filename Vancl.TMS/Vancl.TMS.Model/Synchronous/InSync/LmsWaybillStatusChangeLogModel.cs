using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.Synchronous.InSync
{
    public class LmsWaybillStatusChangeLogModel : BaseModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }

        /// <summary>
        /// 返货状态
        /// </summary>
        public Enums.ReturnStatus ReturnStatus { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int MerchantID { get; set; }

        /// <summary>
        /// 配送商编号
        /// </summary>
        public string DistributionCode { get; set; }

        /// <summary>
        /// 配送站点编号
        /// </summary>
        public int DeliverStationID { get; set; }

        /// <summary>
        /// 创建部门
        /// </summary>
        public int CreateStation { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public Enums.Lms2TmsOperateType OperateType { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "LMS_WaybillStatusChangeLog";
            }
        }
    }
}
