using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound.Packing;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Inbound;

namespace Vancl.TMS.IBLL.Sorting.Inbound
{
    /// <summary>
    /// 分拣装箱逻辑层接口
    /// </summary>
    public interface ISortingPackingBLL
    {
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
        /// 称重装箱
        /// </summary>
        /// <param name="inboundData"></param>
        /// <param name="boxNo"></param>
        /// <param name="weight"></param>
        /// <param name="lstFormCode"></param>
        /// <param name="arrivalID"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        ResultModel AddInboundBox(ViewInboundValidateModel inboundData, string boxNo, decimal weight, List<string> lstFormCode, int arrivalID, bool isUpdate);

        ResultModel AddInboundBoxV2(ViewInboundValidateModel inboundData, string boxno, decimal weight, string formCode, bool isNewbox, ViewPackingBoxToModel vpBox);
        /// <summary>
        /// 取得装箱打印信息
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        SortingPackingPrintModel GetPackingPrintModel(String boxNo);

        SortingPackingPrintModel GetPackingPrintModelV2(String boxNo);

        /// <summary>
        /// 根据运单号获得箱号
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        string GetBoxNoByFormcode(string formCode);

        /// <summary>
        /// 卸载箱号
        /// </summary>
        /// <param name="inboundData"></param>
        /// <param name="boxNo"></param>
        /// <param name="delFormCodes"></param>
        /// <returns></returns>
        ResultModel UnLoadBox(ViewInboundValidateModel inboundData, string boxNo, List<string> delFormCodes);

        /// <summary>
        /// 打印时更新重量
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="weight"></param>
        /// <param name="updateBy"></param>
        /// <returns></returns>
        bool UpdateBoxWeightWhenPrint(string boxNo, Decimal weight, int updateBy);

        /// <summary>
        /// 验证信息
        /// </summary>
        /// <returns></returns>
        bool Validate(string formCode, int expressId, bool isFirst);

        string ValidateMsg { get; }
    }
}
