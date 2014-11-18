using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.Model.BaseInfo.Sorting;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.Synchronous
{
    public interface ILms2TmsSyncLMSDAL
    {
        /// <summary>
        /// 读取lms中间表数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<LmsWaybillStatusChangeLogModel> ReadLmsChangeLogs(Lms2TmsJobArgs args);

        /// <summary>
        /// 取得Lms运单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        DataTable GetLmsWayBillData(LmsWaybillStatusChangeLogModel model);

        /// <summary>
        /// 更新lms中间表同步状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int UpdateSyncStatus(long id, Enums.SyncStatus status);

        /// <summary>
        /// 取得LMS物流主库运单信息【用于LMS到TMS入库的同步】
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        WaybillEntityModel GetWaybillModel4Lms2Tms_Inbound(long waybillNo);

        /// <summary>
        /// 取得LMS物流主库运单信息【用于LMS到TMS出库的同步】
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        WaybillEntityModel GetWaybillModel4Lms2Tms_Outbound(long waybillNo);

        /// <summary>
        /// 获取当前配送商编号
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        string GetCurrentDistributionCode(long waybillNo);

        /// <summary>
        /// 获取转站key
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <returns></returns>
        WaybillEntityModel GetWaybillModel4Lms2Tms_TurnStation(long waybillNo);
    }
}
