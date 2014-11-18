using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Claim.Lost;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Claim
{
    public interface ILostBLL
    {
        IList<ViewLostModel> Search(LostSearchModel searchModel);

        /// <summary>
        /// 根据提货单号取得【预】丢失明细数据
        /// 新增时使用
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        ViewLostDetailModel GetPreLostDetail(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得丢失明细数据
        /// 修改或者查看时使用
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        ViewLostDetailModel GetLostDetail(string deliveryNo);

        /// <summary>
        /// 根据提货单号和箱号取得订单明细
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        IList<ViewDispOrderDetailModel> GetOrderDetail(string deliveryNo, string boxNo);

        /// <summary>
        /// 添加部分丢失信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="lstBoxNo"></param>
        /// <param name="lstFormCode"></param>
        /// <returns></returns>
        ResultModel AddLost(string deliveryNo, List<string> lstBoxNo, List<string> lstFormCode);

        /// <summary>
        /// 添加全部丢失信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        ResultModel AddAllLost(string deliveryNo);

        /// <summary>
        /// 修改丢失信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="lstBoxNo">箱号列表（可空）</param>
        /// <param name="lstFormCode">单号列表（全部丢失时可空）</param>
        /// <param name="isAllLost">是否修改为全部丢失</param>
        /// <returns></returns>
        ResultModel UpdateLost(string deliveryNo, List<string> lstBoxNo, List<string> lstFormCode, bool isAllLost);

        /// <summary>
        /// 根据提货单号获得丢失信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        LostModel Get(string deliveryNo);

        /// <summary>
        /// 判断丢失信息是否存在
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        bool IsExistLost(string deliveryNo);

        /// <summary>
        /// 审批丢失信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <param name="approveStatus"></param>
        /// <returns></returns>
        ResultModel Approve(string deliveryNo, Enums.ApproveStatus approveStatus);

        /// <summary>
        /// 查询给定箱号内的所有订单信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        IList<ViewLostOrderModel> GetOrderList(string boxNo);

        /// <summary>
        /// 删除丢失信息
        /// </summary>
        /// <param name="deliveryNo"></param>
        /// <returns></returns>
        ResultModel Delete(string deliveryNo);

        /// <summary>
        /// 查询指定箱号内的丢失订单
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        IList<ViewLostOrderModel> GetLostOrderList(string boxNo);

        /// <summary>
        /// 检查能够添加或修改提货单丢失信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="isAddLost">是否为添加</param>
        /// <returns></returns>
        ResultModel CheckLostStatus(string deliveryNo, bool isAddLost);
    }
}
