using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// 入库数据同步到LMS物流主库
    /// </summary>
    public class InboundStrategy : InboundBaseStrategy
    {

        /// <summary>
        /// 入库同步到LMS物流SQL主库
        /// </summary>
        private ResultModel Inbound2LmsSQL(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.Sorting.Inbound.InboundEntityModel scInboundModel, Vancl.TMS.Model.LMS.OperateLogEntityModel lmsOperatelog, Vancl.TMS.Model.LMS.Waybill_SortCenterEntityModel lmswaybillSortcenter)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[SQL]主库,状态由{0}变更为{1}", (int)waybillmodel.Status, (int)Model.Common.Enums.BillStatus.HaveBeenSorting);
            waybillmodel.Status = Model.Common.Enums.BillStatus.HaveBeenSorting;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.OutboundID = null;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    long InboundID = WL_Inbound_SQLDAL.Add(CreateLMSInboundEntityModel(waybillmodel, scInboundModel, scchangelogmodel));
                    if (InboundID <= 0)
                    {
                        return result.Failed("LMS物流主库【SQL】版本，新增inbound记录失败");
                    }
                    waybillmodel.InboundID = InboundID;
                    WL_Waybill_SQLDAL.UpdateWaybillModelByInbound(waybillmodel);
                    WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    WL_WaybillSortCenter_SQLDAL.Merge(lmswaybillSortcenter);
                    WL_OperateLog_SQLDAL.Add(lmsOperatelog);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【SQL】版本异常：{0}", ex.StackTrace));
            }
            return result.Succeed(strMsg);
        }

        /// <summary>
        /// 入库同步到LMS物流Oracle主库
        /// </summary>
        /// <returns></returns>
        private ResultModel Inbound2LmsOracle(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.Sorting.Inbound.InboundEntityModel scInboundModel, Vancl.TMS.Model.LMS.OperateLogEntityModel lmsOperatelog, Vancl.TMS.Model.LMS.Waybill_SortCenterEntityModel lmswaybillSortcenter)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象，可能由于之前功能未上线，暂时认为是正确的");
            }
            String strMsg = String.Format("LMS物流[Oracle]主库,状态由{0}变更为{1}", (int)waybillmodel.Status, (int)Model.Common.Enums.BillStatus.HaveBeenSorting);
            waybillmodel.Status = Model.Common.Enums.BillStatus.HaveBeenSorting;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.OutboundID = null;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    long InboundID = WL_Inbound_OracleDAL.Add(CreateLMSInboundEntityModel(waybillmodel, scInboundModel, scchangelogmodel));
                    if (InboundID <= 0)
                    {
                        return result.Failed("LMS物流主库【Oracle】版本，新增inbound记录失败");
                    }
                    waybillmodel.InboundID = InboundID;
                    WL_Waybill_OracleDAL.UpdateWaybillModelByInbound(waybillmodel);
                    WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    WL_WaybillSortCenter_OracleDAL.Merge(lmswaybillSortcenter);
                    WL_OperateLog_OracleDAL.Add(lmsOperatelog);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【Oracle】版本异常：{0}", ex.StackTrace));
            }
            return result.Succeed(strMsg);
        }


        protected override ResultModel Sync(BillChangeLogModel model)
        {
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null.");
            ResultModel result = new ResultModel();
            ResultModel lmssqlResult = null;
            ResultModel lmsoracleResult = null;
            Vancl.TMS.Model.Sorting.Inbound.InboundEntityModel SCInboundModel = SC_Inbound.GetInboundEntityModel4TmsSync2Lms(model.FormCode);
            if (SCInboundModel == null)
            {
                return result.Failed("未取得TMS分拣InboundEntityModel对象");
            }
            var lmsoperatelog = CreateLMSOperateLogEntityModel(model);
            var lmswaybillsortcenter = CreateLMSWaybillSortCenterEntityModel(SCInboundModel);
            if (IsOperateLMSSQL)
            {
                lmssqlResult = Inbound2LmsSQL(model, SCInboundModel, lmsoperatelog, lmswaybillsortcenter);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = Inbound2LmsOracle(model, SCInboundModel, lmsoperatelog, lmswaybillsortcenter);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }
            if (SC_Inbound.UpdateSyncedStatus4Tms2Lms(SCInboundModel.IBID) <= 0)
            {
                return result.Failed("更新TMS入库表同步状态失败");
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }
    }
}
