using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Model.BaseInfo.Truck
{
    public class ViewTruckModel
    {
        /// <summary>
        /// 车辆主键Key
        /// </summary>
        public string TBID { get; set; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string TruckNO { get; set; }

        /// <summary>
        /// GPS编号
        /// </summary>
        public string GPSNO { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string ProvinceName { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// 配送商
        /// </summary>
        public string DistributionName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public  DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public  int UpdateBy { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public  DateTime UpdateTime { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public  bool IsDeleted { get; set; }
    }
}
