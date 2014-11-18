using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    public interface IGPSOrderDAL
    {
        long Add(GPSOrderEntityModel model);

        /// <summary>
        /// 判断运单轨迹是否存在
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        bool IsExist(string orderNo);
    }
}
