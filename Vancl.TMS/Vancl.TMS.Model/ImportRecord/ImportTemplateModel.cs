using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Vancl.TMS.Util.Base;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.Model.ImportRecord
{
    /// <summary>
    /// 提货单导入模版实体对象
    /// </summary>
    public class ImportTemplateModel : BaseEntity
    {
        /// <summary>
        /// 物流运单号
        /// </summary>
        [Description("物流单号")]
        public string CustomerWaybillNo { get; set; }

        /// <summary>
        /// 批次数/箱数
        /// </summary>
        [Description("批次数/箱数")]
        public string BoxCount { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        [Description("订单数量")]
        public string OrderCount { get; set; }

        /// <summary>
        /// 总重量
        /// </summary>
        [Description("总重量")]
        public string TotalWeight { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [Description("总价")]
        public string TotalAmount { get; set; }

        /// <summary>
        /// 始发地ID
        /// </summary>
        public int DepartrueID { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 始发地
        /// </summary>
        [Description("始发地")]
        public string Departrue { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        [Description("目的地")]
        public string Arrival { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        public int CarrierID { get; set; }

        /// <summary>
        /// 承运商
        /// </summary>
        [Description("承运商")]
        public string Carrier { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        [Description("货物类型")]
        public string GoodsType { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        [Description("运输方式")]
        public string TransportType { get; set; }

        /// <summary>
        /// 错误原因
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public Enums.DeliverySource? Source { get; set; }

        ///// <summary>
        ///// 运输方式对应值
        ///// </summary>
        //public Enums.TransportType TransportTypeOnEnum
        //{
        //    get
        //    {
        //        if (!string.IsNullOrWhiteSpace(this.TransportType))
        //        {
        //            try
        //            {
        //                return EnumHelper.GetValue<Enums.TransportType>(this.TransportType);
        //            }
        //            catch
        //            {
        //                throw new Exception("未能找到对应的运输方式:" + this.TransportType);
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("运输方式不能为空");
        //        }
        //    }
        //}

        ///// <summary>
        ///// 货物类型对应值
        ///// </summary>
        //public Enums.GoodsType GoodsTypeOnEnum
        //{
        //    get
        //    {
        //        if (!string.IsNullOrWhiteSpace(this.GoodsType))
        //        {
        //            string[] goodstypes = this.GoodsType.Split(';');
        //            List<int> enumValues = new List<int>();
        //            foreach (var item in goodstypes)
        //            {
        //                int enumValue = EnumHelper.GetEnumValue<Enums.GoodsType>(item);
        //                if (enumValue > -1)
        //                {
        //                    enumValues.Add(enumValue);
        //                }
        //                else
        //                {
        //                    throw new Exception("未能找到对应的货物品类:" + item);
        //                }
        //            }

        //            int temp = enumValues[0];
        //            for (int i = 1; i < enumValues.Count; i++)
        //            {
        //                temp = temp | enumValues[i];
        //            }

        //            return (Enums.GoodsType)temp;
        //        }
        //        else
        //        {
        //            throw new Exception("货物类型不能为空");
        //        }
        //    }
        //}

    }
}
