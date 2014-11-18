using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Sorting.Loading;

namespace Vancl.TMS.IDAL.Sorting.Loading
{
    public interface IBillTruckDAL
    {
        /// <summary>
        /// 获取装车批次信息
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        PagedList<BillTruckBatchModel> GetBillTruckList(BillTruckSearchModel searchModel);

        /// <summary>
        /// 新增装车信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int addWaybillTruck(BillTruckModel model);

        /// <summary>
        /// 通过批次号获取未装车出库运单
        /// </summary>
        /// <param name="batchNO"></param>
        /// <returns></returns>
        IList<OutBoundLoadingModel> GetNotLoadingBill(string batchNO);

        /// <summary>
        /// 通过批次号和订单号列表获取未装车出库运单
        /// </summary>
        /// <param name="batchNO"></param>
        /// <param name="formCodeList"></param>
        /// <returns></returns>
        IList<OutBoundLoadingModel> GetNotLoadingBill(string batchNO, List<string> formCodeList);

        /// <summary>
        /// 验证是否已装车
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool IsExistBillTruck(BillTruckModel model);


        /// <summary>
        /// 取得运单装车(下车)记录需要同步到LMS物流主库的实体对象
        /// </summary>
        /// <param name="formCode">运单号</param>
        /// <param name="isGetOff">ture:运单下车，false:运单装车</param>
        /// <returns></returns>
        BillTruckModel GetBillTruckModelTmsSync2Lms(string formCode, bool isGetOff);

        /// <summary>
        /// 通过批次号和运单号列表获取已装车运单信息
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="formCodeList"></param>
        /// <returns></returns>
        IList<ViewBillTruckModel> GetLoadingBill(string batchNo, List<string> formCodeList);

        /// <summary>
        /// 移除装车信息（运单下车）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int RemoveBillTruck(BillTruckModel model);

        /// <summary>
        /// 获取给定批次号下的出库运单明细
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        IList<ViewBillTruckModel> GetOutbondByBatch(BillTruckSearchModel searchModel);

        /// <summary>
        /// 更新同步状态为已经同步成功【TMS同步会LMS主库时调用】
        /// </summary>
        /// <param name="BillTruckKey">运单装车主键</param>
        /// <returns></returns>
        int UpdateSyncedStatus4Tms2Lms(string BillTruckKey);
    }
}
