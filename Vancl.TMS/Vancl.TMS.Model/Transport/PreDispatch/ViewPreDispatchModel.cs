using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using System.ComponentModel.DataAnnotations;

namespace Vancl.TMS.Model.Transport.PreDispatch
{
    /// <summary>
    /// 预调度相关对象
    /// </summary>
    public class ViewPreDispatchModel
    {
        /// <summary>
        /// 预调度表主键
        /// </summary>
        public long PDID { get; set; }

        /// <summary>
        /// 运输计划
        /// </summary>
        public int TPID { get; set; }

        /// <summary>
        /// 线路计划ID
        /// </summary>
        public int LPID { get; set; }

        /// <summary>
        /// 系统批次号
        /// </summary>
        public String BatchNo { get; set; }

        /// <summary>
        /// 批次来源
        /// </summary>
        [EnumDataType(typeof(Enums.TMSEntranceSource))]
        public Enums.TMSEntranceSource Source { get; set; }

        /// <summary>
        /// 客户批次号
        /// </summary>
        public String CustomerBatchNo { get; set; }

        /// <summary>
        /// 始发地
        /// </summary>
        public String DepartureName { get; set; }

        /// <summary>
        /// 始发地ID
        /// </summary>
        public int DepartureID { get; set; }

        /// <summary>
        /// 目的地
        /// </summary>
        public String ArrivalName { get; set; }

        /// <summary>
        /// 目的地ID
        /// </summary>
        public int ArrivalID { get; set; }

        /// <summary>
        /// 运输方式
        /// </summary>
        [EnumDataType(typeof(Enums.TransportType))]
        public Enums.TransportType TransportType { get; set; }

        /// <summary>
        /// 运输方式描述
        /// </summary>
        public string TransportTypeDescription
        {
            get
            {
                return EnumHelper.GetDescription(TransportType);
            }
        }

        /// <summary>
        /// 出库日期
        /// </summary>
        public DateTime OutBoundTime { get; set; }

        public string OutBoundTimeString
        {
            get
            {
                return this.OutBoundTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        /// <summary>
        /// 商家名称，多个显示“混装”
        /// </summary>
        public string MerchantName { get; set; }
    }
}
