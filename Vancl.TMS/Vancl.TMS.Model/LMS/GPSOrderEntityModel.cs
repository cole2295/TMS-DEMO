using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.LMS
{
    public class GPSOrderEntityModel
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderno { set; get; }

        /// <summary>
        /// 所在城市
        /// </summary>
        public string city { set; get; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string warehouse { set; get; }

        /// <summary>
        /// 配送站
        /// </summary>
        public string stations { set; get; }

        /// <summary>
        /// 客户ID
        /// </summary>
        public int? cid { set; get; }

        /// <summary>
        /// 客户地址
        /// </summary>
        public string address { set; get; }

        /// <summary>
        /// 客户地址经度
        /// </summary>
        public decimal? lng { set; get; }

        /// <summary>
        /// 客户地址纬度
        /// </summary>
        public decimal? lat { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? createtime { set; get; }

        /// <summary>
        /// 当前绑定GPS
        /// </summary>
        public string gpsid { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? time { set; get; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string truckno { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSyn { set; get; }
    }
}
