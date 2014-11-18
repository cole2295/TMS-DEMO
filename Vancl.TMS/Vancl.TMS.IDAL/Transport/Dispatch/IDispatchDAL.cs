using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Common;
using System.Data;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Synchronous.OutSync;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.OutServiceProxy;

namespace Vancl.TMS.IDAL.Transport.Dispatch
{
    public interface IDispatchDAL : ISequenceDAL
    {
        /// <summary>
        /// 添加运输调度
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Obsolete("新增提货单导入功能以后,此方法作废")]
        int Add(DispatchModel model);
        /// <summary>
        /// 更新运输调度
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Update(DispatchModel model);

        /// <summary>
        /// 删除提货单
        /// </summary>
        /// <param name="did"></param>
        /// <returns></returns>
        int Delete(long did);

        /// <summary>
        /// 撤回提货单
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        int Delete(string DeliveryNo);

        /// <summary>
        /// 取得调度明细主键列表
        /// </summary>
        /// <param name="DID">调度主键ID</param>
        /// <returns></returns>
        List<long> GetDispatchDetailKeyIDList(long DID);

        /// <summary>
        /// 取得运输调度中存在运输计划预调度的PDID
        /// </summary>
        /// <param name="DeliveryNo">提货单号</param>
        /// <returns></returns>
        List<long> GetDispatchIsPlanedPDID(string DeliveryNo);

        /// <summary>
        /// 取得运输调度中存在运输计划预调度的PDID
        /// </summary>
        /// <param name="did">调度主表主键</param>
        /// <returns></returns>
        List<long> GetDispatchIsPlanedPDID(long did);

        /// <summary>
        /// 取得运输调度对象
        /// </summary>
        /// <param name="did">调度主键ID</param>
        /// <returns></returns>
        DispatchModel Get(long did);
        /// <summary>
        /// 取得运输调度对象
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <returns></returns>
        DispatchModel Get(string deliveryNo);
        /// <summary>
        /// 更新提货单状态
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <param name="deliveryStatus">状态</param>
        /// <returns></returns>
        int UpdateDeliveryStatus(string deliveryNO, Enums.DeliveryStatus deliveryStatus);

        /// <summary>
        /// 更新提货单为存在丢失(丢失审批通过后)
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <returns></returns>
        int UpdateDeliveryToExistsLost(string deliveryNO);

        /// <summary>
        /// 更新提货单为没有延误(复议审批通过后)
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <returns></returns>
        int UpdateDeliveryToNoDelay(string deliveryNO);

        /// <summary>
        /// 确认提货单到货
        /// </summary>
        /// <param name="isConfirmLimited">是否限制确认到货</param>
        /// <param name="deliveryNO">提货单对象</param>
        /// <param name="deliveryStatus">提货单状态</param>
        /// <returns></returns>
        int ConfirmDeliveryArrived(bool isConfirmLimited, DispatchModel dispModel, Enums.DeliveryStatus deliveryStatus);

        /// <summary>
        /// 更新调度确认到货时间
        /// </summary>
        /// <param name="did">调度主键ID</param>
        /// <param name="dt">到货时间</param>
        /// <returns></returns>
        int UpdateDispatchCofirmExpArrivalDate(long did, DateTime dt);

        /// <summary>
        /// 该出发地待调度的箱子已有的调度箱明细记录
        /// </summary>
        /// <param name="DepartureID">出发地</param>
        /// <param name="disPatchedBox">待调度的箱子</param>
        /// <returns></returns>
        List<DispatchDetailModel> DispatchBoxList(int DepartureID, string[] disPatchedBox);

        /// <summary>
        /// 运输调度主列表信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        PagedList<ViewDispatchModel> Search(DispatchSearchModel searchModel);

        /// <summary>
        /// 提货单打印检索列表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [Obsolete("新增提货单导入功能以后,此方法作废")]
        PagedList<ViewDeliveryPrintModel> Search(DeliveryPrintSearchModel searchModel);

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
        /// <param name="disPatchedBox">已添加到待调度的箱子</param>
        /// <param name="disPatchedBox">出发地名称</param>
        /// <param name="disPatchedBox">目的地名称</param>
        /// <returns></returns>
        List<ViewDispatchBoxModel> GetValidBoxList(int DepartureID, string[] disPatchedBox, string departureName, string arrivalName);


        /// <summary>
        /// 更新调度预计到货时间
        /// </summary>
        /// <remarks>modify by weiminga 20121203,修改预计到货时间计算逻辑:预计到货时间=离库时间+线路到货时效</remarks>
        /// <param name="deliveryNO">提货单号</param>
        /// <param name="leaveTime">离库时间</param>
        /// <returns></returns>
        int UpdateDispatchExpectArrivalDate(string deliveryNO, DateTime leaveTime);

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
        /// 取得计划外运输调度箱信息
        /// </summary>
        /// <param name="departureID">出发地</param>
        /// <param name="arrivalID">目的地</param>
        /// <param name="lineGoodsType">货物类型</param>
        /// <returns></returns>
        List<ViewDispatchBoxModel> GetDispatchBoxList(int departureID, int arrivalID, Enums.GoodsType lineGoodsType);


        IList<PrintDeliveryNoModel> GetPrintDeliveryInfo(IList<string> deliveryNoList);

        /// <summary>
        /// 运输调度主列表信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        PagedList<ViewDispatchModel> SearchEx(DispatchSearchModel searchModel);

        /// <summary>
        /// 添加运输调度(提货单导入扩展)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddEx(DispatchModel model);

        /// <summary>
        /// 运输调度统计信息
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        ViewDispatchStatisticModel GetStatisticInfoEx(DispatchSearchModel searchModel);

        /// <summary>
        /// 更新调度表(用于提货单导入后的确认调度操作)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateEx(DispatchModel model);

        /// <summary>
        /// 撤回(用于提货单导入后)
        /// </summary>
        /// <param name="did"></param>
        /// <param name="DeliveryStatus"></param>
        /// <returns></returns>
        int RejectDispatchEx(long did, Enums.DeliveryStatus DeliveryStatus);

        /// <summary>
        /// 提货单打印检索列表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<ViewDeliveryPrintModel> SearchEx(DeliveryPrintSearchModel searchModel);

        /// <summary>
        /// 修改保价金额
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateTotalAmountByID(long did, decimal totalAmount, decimal protectedPrice);

        /// <summary>
        /// 取得提货单日志记录4TMS2ThridParty
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<DeliveryFlowLogModel> GetDeliveryInfo4TMS2ThridParty(Tms2ThridPartyJobArgs args);

        /// <summary>
        /// 更新TMS提货单日志表同步标记
        /// </summary>
        /// <param name="keyID">主键ID</param>
        /// <param name="sync">更新为同步标记</param>
        void UpdateDeliveryFlowSyncFlag4TMS2ThridParty(long keyID, Enums.SyncStatus sync);

        /// <summary>
        /// 取得TMS城际运输时效日志列表
        /// </summary>
        /// <param name="dispModel"></param>
        /// <returns></returns>
        List<AgingMonitoringLogProxyModel> GetTMSAgingMonitoringLogList(DispatchModel dispModel, Enums.DeliverFlowType FlowType);

        /// <summary>
        /// 修改箱数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateDispatchBoxCount(DispatchModel model);
    }
}
