using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Claim.Lost;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.Claim
{
    public interface ILostDAL : ISequenceDAL
    {
        /// <summary>
        /// 是否存在未录入的丢失信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        bool IsExistNotInputLostInfo(string deliveryNo);

        /// <summary>
        /// 是否存在未审核的丢失信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        bool IsExistNotApproveInfo(string deliveryNo);

        /// <summary>
        /// 录入丢失信息的查询
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        IList<ViewLostModel> Search(LostSearchModel searchModel);

        /// <summary>
        /// 取得提货单下的箱子信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        IList<ViewLostBoxModel> GetBoxDetail(string deliveryNo);

        /// <summary>
        /// 取得【预计】丢失的订单信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        IList<LostOrderDetailModel> GetPreLostOrderDetail(string deliveryNo);

        /// <summary>
        /// 取得丢失的订单信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        IList<LostOrderDetailModel> GetLostOrderDetail(string deliveryNo);

        /// <summary>
        /// 取得订单信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="lstBoxNo">箱号</param>
        /// <returns></returns>
        IList<LostOrderDetailModel> GetOrderDetail(string deliveryNo, List<string> lstBoxNo);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(LostModel model);

        /// <summary>
        /// 获取给定箱号内的所有订单信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        IList<ViewLostOrderModel> GetOrderList(string boxNo);

        /// <summary>
        /// 根据提货单号判断丢失信息是否已经存在
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        bool IsExistLost(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得丢失信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        LostModel Get(string deliveryNo);

        /// <summary>
        /// 审核丢失信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="approveStatus">审核状态</param>
        /// <returns></returns>
        int Approve(string deliveryNo, Enums.ApproveStatus approveStatus);

        /// <summary>
        /// 删除丢失信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        int Delete(string deliveryNo);

        /// <summary>
        /// 更新订单tms状态为丢失状态
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="status">状态</param>
        /// <param name="IsAllLost">是否是全部丢失</param>
        /// <returns></returns>
        int UpdateOrderTMSStatus(string deliveryNo, bool IsAllLost);

        /// <summary>
        /// 查询指定箱号内的丢失订单
        /// </summary>
        /// <param name="boxNo">箱</param>
        /// <returns></returns>
        IList<ViewLostOrderModel> GetLostOrderList(string boxNo);

        /// <summary>
        /// 取得丢失应扣款项
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        decimal GetLostDeduction(string deliveryNo);

    }
}
