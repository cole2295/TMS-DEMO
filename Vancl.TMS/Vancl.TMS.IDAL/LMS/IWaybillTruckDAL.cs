using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    public interface IWaybillTruckDAL
    {
        /// <summary>
        /// 新增加装车记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(WaybillTruckEntityModel model);

        /// <summary>
        /// 是否已经运单装车
        /// </summary>
        /// <param name="waybillNo">运单号</param>
        /// <param name="batchNo">出库批次号</param>
        /// <returns></returns>
        bool IsWaybillLoading(long waybillNo, String batchNo);

        /// <summary>
        /// 移除装车记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int RemoveBillTruck(WaybillTruckEntityModel model);

    }
}
