using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Transport.Dispatch;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.ImportRecord;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Model.Delivery.InTransit;

namespace Vancl.TMS.IBLL.Transport.Dispatch
{
    public interface IDispatchBLL
    {
        /// <summary>
        /// 运输调度主列表信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        PagedList<ViewDispatchModel> Search(DispatchSearchModel searchModel);

        /// <summary>
        /// 提货单打印列表信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        [Obsolete("新增提货单导入功能以后,此方法作废")]
        PagedList<ViewDeliveryPrintModel> Search(DeliveryPrintSearchModel searchModel);

        /// <summary>
        /// 提货单打印列表信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        PagedList<ViewDeliveryPrintModel> SearchEx(DeliveryPrintSearchModel searchModel);

        /// <summary>
        /// 运输调度主列表信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        PagedList<ViewDispatchModel> SearchEx(DispatchSearchModel searchModel);

        /// <summary>
        /// 运输调度统计信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        [Obsolete("新增提货单导入功能以后,此方法作废")]
        ViewDispatchStatisticModel GetStatisticInfo(DispatchSearchModel searchModel);

        /// <summary>
        /// 导出报表数据
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        DataTable ExportReport(DispatchSearchModel searchModel);

        /// <summary>
        /// 运输调度取得已验证的可添加的箱列表信息
        /// </summary>
        /// <param name="DepartureID">出发地ID</param>
        /// <param name="disPatchedBox">已添加到调度的箱子</param>
        /// <param name="disPatchedBox">出发地名称</param>
        /// <param name="disPatchedBox">目的地名称</param>
        /// <returns></returns>
        List<ViewDispatchBoxModel> GetValidBoxList(int DepartureID, string[] disPatchedBox, string departureName, string arrivalName);

        /// <summary>
        /// 取得计划外运输调度箱信息
        /// </summary>
        /// <param name="departureID">出发地</param>
        /// <param name="arrivalID">目的地</param>
        /// <param name="lineGoodsType">货物类型</param>
        /// <returns></returns>
        List<ViewDispatchBoxModel> GetDispatchBoxList(int departureID, int arrivalID, Enums.GoodsType lineGoodsType);

        /// <summary>
        /// 运输调度确认调度
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <param name="waybillno">物流单号</param>
        /// <param name="LPID">线路计划主键ID</param>
        /// <param name="disPatchedBox">待调度的箱子</param>
        /// <returns></returns>
        [Obsolete("新增提货单导入功能以后,此方法作废")]
        ResultModel ConfirmDispatch(string DeliveryNo, string waybillno, int LPID, string[] disPatchedBox);

        /// <summary>
        /// 运输调度确认调度
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <param name="waybillno">物流单号</param>
        /// <param name="LPID">线路计划主键ID</param>
        /// <param name="disPatchedBox">待调度的箱子</param>
        /// <returns></returns>
        ResultModel ConfirmDispatchEx(string DeliveryNo, string waybillno, int LPID, long did);

        /// <summary>
        /// 运输调度计划外确认调度
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <param name="waybillno">物流单号</param>
        /// <param name="LPID">线路计划主键ID</param>
        /// <param name="disPatchedBox">待调度的箱子</param>
        /// <returns></returns>
        ResultModel ComfirmUnplannedDispatch(string DeliveryNo, string waybillno, int LPID, string[] disPatchedBox);

        /// <summary>
        /// 运输调度置回
        /// </summary>
        /// <param name="deliveryNos">提货单号列表</param>
        /// <returns></returns>
        [Obsolete("新增提货单导入功能以后,此方法作废")]
        ResultModel RejectDispatch(List<string> deliveryNos);

        /// <summary>
        /// 运输调度置回
        /// </summary>
        /// <param name="deliveryNos">提货单号列表</param>
        /// <returns></returns>
        ResultModel RejectDispatchEx(List<string> deliveryNos);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deliveryNos">提货单号列表</param>
        /// <returns></returns>
        ResultModel Delete(List<long> dids);

        /// <summary>
        /// 更新提货单状态
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <param name="deliveryStatus">提货单状态</param>
        /// <returns></returns>
        ResultModel UpdateDeliveryStatus(string deliveryNO, Enums.DeliveryStatus deliveryStatus);

        /// <summary>
        /// 更新提货单为存在丢失(丢失审批通过后)
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <returns></returns>
        ResultModel UpdateDeliveryToExistsLost(string deliveryNO);

        /// <summary>
        /// 更新提货单为没有延误(复议审批通过后)
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <returns></returns>
        ResultModel UpdateDeliveryToNoDelay(string deliveryNO);

        /// <summary>
        /// 确认提货单到货
        /// </summary>
        /// <param name="isConfirmLimited">是否限制确认到货</param>
        /// <param name="deliveryNO">提货单对象</param>
        /// <param name="deliveryStatus">提货单状态</param>
        /// <returns></returns>
        ResultModel ConfirmDeliveryArrived(bool isConfirmLimited, DispatchModel dispModel, Enums.DeliveryStatus deliveryStatus);

        /// <summary>
        /// 根据主键id取得提货单
        /// </summary>
        /// <param name="did">主键id</param>
        /// <returns></returns>
        DispatchModel Get(long did);

        /// <summary>
        /// 根据提货单号取得提货单
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        DispatchModel Get(string deliveryNo);

        /// <summary>
        /// 运输调度修改调度
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="waybillno">物流单号</param>
        /// <param name="LPID">线路计划主键ID</param>
        /// <param name="disPatchedBox">待调度的箱子</param>
        /// <returns></returns>
        ResultModel Update(string deliveryNo, string waybillno, int LPID, string[] disPatchedBox);

        /// <summary>
        /// 运输调度修改/查看根据提货单号取得调度信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        List<ViewDispatchBoxModel> GetDispatchedBoxList(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得提货单打印信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        PrintDeliveryNoModel GetPrintDeliveryInfo(string deliveryNo);

        /// <summary>
        /// 根据提货单号取得提货单打印信息
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        IList<PrintDeliveryNoModel> GetPrintDeliveryInfo(IList<string> deliveryNoList);

        /// <summary>
        /// 根据提货单号修改运单号
        /// </summary>
        /// <param name="waybillNo">运单号</param>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        ResultModel UpdateWaybillNoByDeliveryNo(string waybillNo, string deliveryNo);

        /// <summary>
        /// 新增提货单信息
        /// </summary>
        /// <param name="model">提货单对象</param>
        /// <returns></returns>
        ResultModel Add(ImportTemplateModel model);

        /// <summary>
        /// 运输调度统计信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        ViewDispatchStatisticModel GetStatisticInfoEx(DispatchSearchModel searchModel);

        /// <summary>
        /// 新增待调度提货单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel Add(ViewDispatchWithDetailsModel model, string batchNo, Enums.DeliverySource source);

        /// <summary>
        /// 修改金额
        /// </summary>
        /// <param name="did">主键ID</param>
        /// <param name="totalAmount">总金额</param>
        /// <param name="protectedPrice">保价金额</param>
        /// <returns></returns>
        ResultModel UpdateTotalAmountByID(long did, decimal totalAmount, decimal protectedPrice);
    }
}
