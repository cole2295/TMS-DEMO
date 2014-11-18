using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Dispatch;

namespace Vancl.TMS.IDAL.Transport.Dispatch
{
    /// <summary>
    /// 提货单订单明细数据接口
    /// </summary>
    public interface IDispOrderDetailDAL : ISequenceDAL
    {
        /// <summary>
        /// 添加提货单订单明细
        /// </summary>
        /// <param name="model"></param>
        void Add(DispOrderDetailModel model);

        /// <summary>
        /// 确认调度修改调度订单明细记录
        /// </summary>
        /// <param name="DDID">调度明细表DDID列表</param>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        int UpdateBy_ConfirmDispatch(List<long> DDID, String DeliveryNo);

        /// <summary>
        /// 添加提货单订单明细
        /// </summary>
        /// <param name="model"></param>
        void Add(List<DispOrderDetailModel> listmodel);

        /// <summary>
        /// 获得调度订单明细
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="BoxNo">箱号</param>
        /// <returns></returns>
        IList<ViewDispOrderDetailModel> GetOrderDetail(string deliveryNo, string boxNo);

        /// <summary>
        /// 获得调度订单明细
        /// </summary>
        /// <param name="lstFormCode">单号</param>
        /// <returns></returns>
        IList<ViewDispOrderDetailModel> GetOrderDetail(List<string> lstFormCode);

        /// <summary>
        /// 根据提货单号获取订单总价格
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        decimal GetOrderTotalAmountByDeliveryNo(string deliveryNo);

        /// <summary>
        /// 根据提货单号获取订单总保价金额
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        decimal GetTotalProtectedPriceByDeliveryNo(string deliveryNo);

        /// <summary>
        /// 根据单号获取订单总价格
        /// </summary>
        /// <param name="lstFormCode">单号</param>
        /// <returns></returns>
        decimal GetOrderTotalAmountByFormCodes(List<string> lstFormCode);

        /// <summary>
        /// 根据单号获取订单总保价金额
        /// </summary>
        /// <param name="lstFormCode">单号</param>
        /// <returns></returns>
        decimal GetTotalProtectedPriceByFormCodes(List<string> lstFormCode);

        /// <summary>
        /// 撤回提货单订单明细
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        [Obsolete("采用新操作模式，此方法作废")]
        int Delete(string DeliveryNo);

        /// <summary>
        /// 逻辑删除调度订单明细数据
        /// </summary>
        /// <param name="listDDID">调度明细主键ID</param>
        /// <returns></returns>
        int Delete(List<long> listDDID);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <param name="arrivalID">目的地id</param>
        /// <returns></returns>
        bool IsExists(string formCode, int arrivalID);
    }
}
