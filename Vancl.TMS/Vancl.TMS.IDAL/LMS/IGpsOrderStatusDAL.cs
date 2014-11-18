using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    public interface IGpsOrderStatusDAL
    {
        /// <summary>
        /// 添加订单轨迹状态信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddGpsOrderStatus(GPSOrderStatusEntityModel model);

        /// <summary>
        /// 获取订单轨迹状态信息
        /// </summary>
        /// <returns></returns>
        GPSOrderStatusEntityModel GetGPSOrderStatus(string waybillNo);
    }
}
