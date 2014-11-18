using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    public class RFDDeclare
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public class CODCreateBy
        {
            /// <summary>
            /// 转站
            /// </summary>
            public const string TransferStation = "TransferStation";
            /// <summary>
            /// Vancl
            /// </summary>
            public const string Vancl = "Vancl";
            /// <summary>
            /// VJIA
            /// </summary>
            public const string VJIA = "VJIA";
            /// <summary>
            /// 小米第一次导入
            /// </summary>
            public const string XiaomiOne = "XiaomiOne";
            /// <summary>
            /// 小米第二次导入
            /// </summary>
            public const string XiaomiTwo = "XiaomiTwo";
            /// <summary>
            /// GIS分单页面
            /// </summary>
            public const string GISWEB = "GISWEB";
            /// <summary>
            /// GIS分单服务
            /// </summary>
            public const string GISServices = "GISServices";
            /// <summary>
            /// 外单
            /// </summary>
            public const string OutsideOrders = "OutsideOrders";
            /// <summary>
            /// 修改分配
            /// </summary>
            public const string Modify = "Modify";
            /// <summary>
            /// 分配配送站
            /// </summary>
            public const string SetStation = "SetStation";
            /// <summary>
            /// 手工分单
            /// </summary>
            public const string ModifyByMan = "ModifyByMan";
            /// <summary>
            /// 出库
            /// </summary>
            public const string TransferOUT = "TransferOUT";
            /// <summary>
            /// 入库
            /// </summary>
            public const string TransferIN = "TransferIN";
            /// <summary>
            /// 导入
            /// </summary>
            public const string Import = "Import";
            /// <summary>
            /// 批量归班
            /// </summary>
            public const string BatchReturn = "BatchReturn";
            /// <summary>
            /// 提交配送结果
            /// </summary>
            public const string Return = "Return";
            /// <summary>
            /// 置为无效
            /// </summary>
            public const string Invalid = "Invalid";
            /// <summary>
            /// 退换货or拒收入库
            /// </summary>
            public const string ReturnInbound = "ReturnInbound";

            /// <summary>
            /// 运单装车
            /// </summary>
            public const string TruckIN = "TruckIN";

            /// <summary>
            /// 快递单
            /// </summary>
            public const string ExpressOrder = "ExpressOrder";

            /// <summary>
            /// 外接订单
            /// </summary>
            public const string OrderForThirdPartyV3 = "OrderForThirdPartySerivceV3";

        }
    }
}
