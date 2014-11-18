using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 运单主表数据接口
    /// </summary>
    public interface IWaybillDAL
    {

        /// <summary>
        /// 根据运单号取得运单信息
        /// </summary>
        /// <param name="WaybillNo"></param>
        /// <returns></returns>
        WaybillEntityModel GetModel(long WaybillNo);

        /// <summary>
        /// 入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>影响行数</returns>
        int UpdateWaybillModelByInbound(WaybillEntityModel waybillModel);

        /// <summary>
        /// 入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>影响行数</returns>
        int UpdateWaybillModelByInbound_NoLimitedStation(WaybillEntityModel waybillModel);

        /// <summary>
        /// 更新运单状态
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        int UpdateWaybillStatus(WaybillEntityModel waybillModel);
        /// <summary>
        /// 更新运单逆向状态
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        int UpdateWaybillReturnStatus(WaybillEntityModel waybillModel);
        /// <summary>
        /// 出库时修改主单对象
        /// </summary>
        /// <param name="waybillModel">需要修改为的对象</param>
        /// <returns>影响行数</returns>
        int UpdateWaybillModelByOutbound(WaybillEntityModel waybillModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="waybillModel"></param>
        /// <returns></returns>
        int UpdateWaybillModelByLoading(WaybillEntityModel waybillModel);

        /// <summary>
        /// 返货的入库
        /// </summary>
        /// <param name="waybillModel"></param>
        /// <returns></returns>
        int WaybillReturnInBound(WaybillEntityModel waybillModel);

    }
}
