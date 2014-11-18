using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Delivery.InTransit
{
    public interface ICarrierWaybillBLL
    {
        /// <summary>
        /// 增加承运商运单信息
        /// </summary>
        /// <param name="model">承运商运单信息</param>
        /// <returns></returns>
        ResultModel Add(CarrierWaybillModel model);
        /// <summary>
        /// 批量删除承运商运单信息
        /// </summary>
        /// <param name="lstCwid">承运商运单信息主键id</param>
        /// <returns></returns>
        ResultModel Delete(List<long> lstCwid);
        /// <summary>
        /// 更新承运商运单信息
        /// </summary>
        /// <param name="model">承运商运单信息</param>
        /// <returns></returns>
        ResultModel Update(CarrierWaybillModel model);
        /// <summary>
        /// 根据主键id取得承运商运单信息
        /// </summary>
        /// <param name="cwid">主键id</param>
        /// <returns></returns>
        CarrierWaybillModel Get(long cwid);
        /// <summary>
        /// 根据调度id取得承运商运单信息
        /// </summary>
        /// <param name="dispatchID">调度id</param>
        /// <returns></returns>
        CarrierWaybillModel GetByDispatchID(long dispatchID);
    }
}
