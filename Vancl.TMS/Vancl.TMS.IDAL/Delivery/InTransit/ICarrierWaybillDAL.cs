using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.InTransit;

namespace Vancl.TMS.IDAL.Delivery.InTransit
{
    public interface ICarrierWaybillDAL : ISequenceDAL
    {
        /// <summary>
        /// 新增承运商运单信息
        /// </summary>
        /// <param name="model">承运商运单信息</param>
        /// <returns></returns>
        [Obsolete("新增提货单导入功能以后,此方法作废")]
        int Add(CarrierWaybillModel model);
        /// <summary>
        /// 删除承运商运单信息
        /// </summary>
        /// <param name="lstCwid">承运商运单信息主键id列表</param>
        /// <returns></returns>
        int Delete(List<long> lstCwid);
        /// <summary>
        /// 更新承运商运单信息
        /// </summary>
        /// <param name="model">承运商运单信息</param>
        /// <returns></returns>
        int Update(CarrierWaybillModel model);
        /// <summary>
        /// 根据承运商运单信息主键id取得承运商运单信息
        /// </summary>
        /// <param name="cwid">承运商运单信息主键id</param>
        /// <returns></returns>
        CarrierWaybillModel Get(long cwid);
        /// <summary>
        /// 根据提货单主键id取得承运商运单信息
        /// </summary>
        /// <param name="dispatchID">提货单主键id</param>
        /// <returns></returns>
        CarrierWaybillModel GetByDispatchID(long dispatchID);
        /// <summary>
        /// 根据主键id取得承运商运单号
        /// </summary>
        /// <param name="cwid">承运商运单信息主键id</param>
        /// <returns></returns>
        string GetWaybillNoByID(long cwid);

        /// <summary>
        /// 修改运单号
        /// </summary>
        /// <param name="waybillNo">运单号</param>
        /// <param name="cwid">主键id</param>
        /// <returns></returns>
        int UpdateWaybillNo(string waybillNo, long cwid);

        /// <summary>
        /// 新增承运商运单信息(提货单导入扩展)
        /// </summary>
        /// <param name="model">承运商运单信息</param>
        /// <returns></returns>
        int AddEx(CarrierWaybillModel model);
    }
}
