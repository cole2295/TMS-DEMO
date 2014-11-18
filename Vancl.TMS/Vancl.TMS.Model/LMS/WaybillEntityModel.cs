using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 运单主表实体对象
    /// </summary>
    public class WaybillEntityModel : BaseModel
    {

        /// <summary>
        /// 运单号
        /// </summary>
        public long WaybillNo { get; set; }

        /// <summary>
        /// 运单来源
        /// </summary>
        public Enums.BillSource Source { get; set; }

        /// <summary>
        /// 运单状态
        /// </summary>
        public Enums.BillStatus Status { get; set; }
        /// <summary>
        /// 运单逆向状态
        /// </summary>
        public Enums.ReturnStatus ReturnStatus { get; set; }
        /// <summary>
        /// 入库ID
        /// </summary>
        public long? InboundID { get; set; }

        /// <summary>
        /// 配送站点
        /// </summary>
        public int? DeliverStationID { get; set; }

        /// <summary>
        /// 出库ID
        /// </summary>
        public long? OutboundID { get; set; }
        /// <summary>
        /// 退货分拣称重装箱单号明细ID
        /// </summary>
        public long? BillReturnDetailID { get; set; }
        /// <summary>
        /// 退货分拣称重装箱箱号ID
        /// </summary>
        public long? BillReturnBoxID { get; set; }
        /// <summary>
        /// 更新部门
        /// </summary>
        public int UpdateDept { get; set; }

        /// <summary>
        /// 系统批次号
        /// </summary>
        public String CustomerBatchNO { get; set; }

        /// <summary>
        /// 配送人员
        /// </summary>
        public int? DeliveryMan { get; set; }

        /// <summary>
        /// 交接司机
        /// </summary>
        public int? DeliveryDriver { get; set; }

        /// <summary>
        /// 是否快捷转站运单
        /// </summary>
        public bool? Isfast { get; set; }

        /// <summary>
        /// 转站ID
        /// </summary>
        public long? TurnStationID { get; set; }

        /// <summary>
        /// 配送时间
        /// </summary>
        public DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// 客户订单号
        /// </summary>
        public String CustomerOrder { get; set; }

        /// <summary>
        /// 接货配送商
        /// </summary>
        public String DistributionCode { get; set; }

        /// <summary>
        /// 当前配送商code
        /// </summary>
        public String CurrentDistributionCode { get; set; }

        /// <summary>
        /// 商家ID
        /// </summary>
        public int MerchantID { get; set; }
        public DateTime? ReturnTime { get; set; }

        public DateTime? ReturnExpressCompanyTime { get; set; }
        /// <summary>
        /// 返货分拣中心
        /// </summary>
        public int? ReturnExpressCompanyId { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        public override string ModelTableName
        {
            get
            {
                return "Waybill";
            }
        }


    }

}
