using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Vancl.TMS.Model.Common
{
    public class LineCodeContextModel
    {
        /// <summary>
        /// 编号格式化属性
        /// </summary>
        public SerialNumberModel NumberModel { get; set; }

        public string Header
        {
            get
            {
                string h = "N";
                int current = (int)GoodsType;
                int nValue = (int)(Enums.GoodsType.Normal | Enums.GoodsType.Frangible);
                int aValue=(int)(Enums.GoodsType.Normal | Enums.GoodsType.Frangible | Enums.GoodsType.Contraband);
                int dValue = (int)Enums.GoodsType.Contraband;
                if ((current | nValue) == nValue)
                {
                    return "N";
                }
                if (current == aValue)
                {
                    return "A";
                }
                if (current == dValue)
                {
                    return "D";
                }

                return h;
            }
        }

        /// <summary>
        /// 始发地ID
        /// </summary>
        public string Departure { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public string Arrival { get; set; }

        /// <summary>
        /// 承运商ID
        /// </summary>
        public string CarrierID { get; set; }

        public string BusinessTypeString
        {
            get
            {
                if (BusinessType == Vancl.TMS.Model.Common.Enums.BusinessType.Outsourcing)
                    return "01";
                else
                    return "00";
            }
        }

        public string TransportTypeString
        {
            get
            {
                switch (TransportType)
                {
                    case Vancl.TMS.Model.Common.Enums.TransportType.Highway:
                        return "01";
                    case Vancl.TMS.Model.Common.Enums.TransportType.Aviation:
                        return "02";
                    case Vancl.TMS.Model.Common.Enums.TransportType.Railway:
                        return "03";
                    default:
                        return "01";
                }
            }
        }

        /// <summary>
        /// 运输方式
        /// </summary>
        public Vancl.TMS.Model.Common.Enums.TransportType TransportType { get; set; }

        /// <summary>
        /// 营业类型
        /// </summary>
        public Vancl.TMS.Model.Common.Enums.BusinessType BusinessType { get; set; }

        /// <summary>
        /// 货物类型
        /// </summary>
        public Vancl.TMS.Model.Common.Enums.GoodsType GoodsType { get; set; }


    }
}
