using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    /// <summary>
    /// 运单收发以及配送站相关模型
    /// </summary>
    public class TakeSend_DeliverStationEntityModel
    {
        /// <summary>
        /// 收件人地址
        /// </summary>
        public string ReceiveAddress { get; set; }

        /// <summary>
        /// 配送站所属城市
        /// </summary>
        public string DeliverStationCityName { get; set; }

        /// <summary>
        /// 配送站ID
        /// </summary>
        public string DeliverStationID { get; set; }

        /// <summary>
        /// 配送站父级站点ID
        /// </summary>
        public string DeliverStationParentID { get; set; }
    }
}
