using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.Model.Sorting.BillPrint
{
    /// <summary>
    /// 面单打印数据
    /// </summary>
    public class BillPrintViewModel
    {
        /// <summary>
        /// 商家名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 运单类型
        /// WaybillTypeStrEnum
        /// </summary>
        public Enums.BillType BillType { get; set; }
        public string BillTypeDescription
        {
            get
            {
                return EnumHelper.GetDescription(BillType);
            }
        }

        /// <summary>
        /// 打印时间
        /// </summary>
        public string PrintTime { get; set; }

        /// <summary>
        /// 商家单号
        /// </summary>
        public string CustomerOrder { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        public string FormCode { get; set; }

        /// <summary>
        /// 始发站
        /// </summary>
        public string SendStation { get; set; }

        /// <summary>
        /// 配送站助记名
        /// </summary>
        public string DeliveryStation { get; set; }
        /// <summary>
        /// 区域代码
        /// </summary>
        public string ZoneCode { get; set; }
        /// <summary>
        /// 站点编号
        /// </summary>
        public string SiteNo { get; set; }
        /// <summary>
        /// 线路编号
        /// </summary>
        public string LineCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string ReceiveComment { get; set; }
        /// <summary>
        /// 送货时间要求
        /// </summary>
        public string SendTime { get; set; }
        /// <summary>
        /// 属性 ：易碎
        /// </summary>
        public Enums.BillGoodsType BillGoodsType { get; set; }
        public string BillGoodsTypeDescription
        {
            get
            {
                return EnumHelper.GetDescription(BillGoodsType);
            }
        }

        /// <summary>
        /// 收货人
        /// </summary>
        public string ReceiveBy { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string ReceiveTel { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string ReceiveMobile { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string ReceiveProvince { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string ReceiveCity { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string ReceiveArea { get; set; }
        /// <summary>
        ///地址
        /// </summary>
        public string ReceiveAddress { get; set; }
        /// <summary>
        /// 总计
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 应收款
        /// </summary>
        public decimal ReceivableAmount { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public Enums.PayType PayType { get; set; }
        public string PayTypeDescription
        {
            get
            {
                return EnumHelper.GetDescription(PayType);
            }
        }
        /// <summary>
        /// 包装方式
        /// </summary>
        public string PackageMode { get; set; }
        /// <summary>
        /// 箱码
        /// </summary>
        public string CustomerBoxNo { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }
        /// <summary>
        /// 配送商
        /// </summary>
        public string DistributionName { get; set; }
        /// <summary>
        /// 箱号
        /// </summary>
        public int PackageIndex { get; set; }
        /// <summary>
        /// 箱数
        /// </summary>
        public int PackageCount { get; set; }

        public byte[] BarCode { get; set; }
    }
}
