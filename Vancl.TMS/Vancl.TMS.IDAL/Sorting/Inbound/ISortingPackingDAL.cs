using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound.Packing;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.IDAL.Sorting.Inbound
{
    /// <summary>
    /// 分拣装箱数据层接口
    /// </summary>
    public interface ISortingPackingDAL
    {
        /// <summary>
        /// 添加装箱数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddInboundPacking(InboundPackingEntityModel model);

        /// <summary>
        /// 更新装箱数据
        /// </summary>
        /// <param name="model">装箱对象</param>
        /// <returns></returns>
        int UpdateInboundPacking(InboundPackingEntityModel model);

        /// <summary>
        /// 批量添加装箱数据明细
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <param name="formCodeList">单号列表</param>
        /// <param name="userID">操作人</param>
        /// <returns></returns>
        int BatchAddInboundPackingDetail(string boxNo, List<string> formCodeList, int userID);

        /// <summary>
        /// 取得分拣装箱检查信息
        /// </summary>
        /// <param name="lstFormCode">单号列表</param>
        /// <param name="expressID">操作站点</param>
        /// <returns></returns>
        List<SortingPackingCheckModel> GetPackingCheckModel(List<string> lstFormCode, int expressID);

        /// <summary>
        /// 根据箱号取得运单主对象
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        List<BillModel> GetBillModelByBoxNo(String boxNo);

        /// <summary>
        /// 取得装箱单号
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        List<string> GetFormCodesByBoxNo(string boxNo);

        /// <summary>
        /// 批量删除装箱明细记录
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <param name="lstFormCode">单号列表</param>
        /// <returns></returns>
        int BatchDeleteInboundPackingDetail(string boxNo, List<string> lstFormCode);

        /// <summary>
        /// 删除装箱记录
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        int DeleteInboundPacking(string boxNo);

        /// <summary>
        /// 取得箱子出库状态
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        Enums.BoxOutBoundedStatus GetBoxOutBoundedStatus(string boxNo);

        /// <summary>
        /// 根据箱号取得箱中所有订单
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        List<SortingPackingBillModel> GetPackingBillsByBoxNo(string boxNo);

        /// <summary>
        /// 根据单号取得箱中的所有订单
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <returns></returns>
        List<SortingPackingBillModel> GetPackingBillsByFormCode(string formCode);

        /// <summary>
        /// 取得箱子对象
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        SortingPackingBoxModel GetPackingBox(string boxNo);

        /// <summary>
        /// 该运单是否在当前操作站点已经装箱
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <param name="expressID">操作站点</param>
        /// <returns></returns>
        bool IsBillAlreadyPacked(string formCode, int expressID);

        /// <summary>
        /// 取得分拣装箱订单
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <returns></returns>
        SortingPackingBillModel GetSortingPackingBill(string formCode);

        /// <summary>
        /// 取得装箱打印信息
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        SortingPackingPrintModel GetPackingPrintModel(String boxNo);

        /// <summary>
        /// 根据运单号获得箱号
        /// </summary>
        /// <param name="formcode"></param>
        /// <returns></returns>
        string GetBoxNoByFormcode(string formcode);

        /// <summary>
        /// 打印时更新重量
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="weight"></param>
        /// <param name="updateBy"></param>
        /// <returns></returns>
        int UpdateBoxWeightWhenPrint(string boxNo, Decimal weight, int updateBy);

        /// <summary>
        /// 根据发货地，目的地，日期，获得装了多少箱
        /// </summary>
        /// <param name="departureId"></param>
        /// <param name="arrivalId"></param>
        /// <param name="packingDate"></param>
        /// <returns></returns>
        List<SortingPackingPrintModel> GetPackingModelList(int departureId, int arrivalId, DateTime packingDate);
    }
}
