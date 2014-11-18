using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// 分拣装箱策略
    /// </summary>
    public class PackingStrategy: Tms2LmsStrategy
    {

        protected override Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Model.LMS.WaybillEntityModel waybillmodel, Model.Log.BillChangeLogModel scchangelogmodel)
        {
            var lmsChangelog = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            lmsChangelog.CurNode = Model.Common.Enums.StatusChangeNodeType.DeliverCenter;
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.SortingPacking;
            lmsChangelog.SubStatus = (int)scchangelogmodel.CurrentStatus;
            return lmsChangelog;
        }

        protected override Model.LMS.OperateLogEntityModel CreateLMSOperateLogEntityModel(Model.Log.BillChangeLogModel scchangelogmodel)
        {
            var logModel= base.CreateLMSOperateLogEntityModel(scchangelogmodel);
            logModel.LogType = 10;  //不显示在官网
            return logModel;
        }

        protected override Model.Common.ResultModel Sync(Model.Log.BillChangeLogModel model)
        {
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null.");
            ResultModel result = new ResultModel();
            ResultModel lmssqlResult = null;
            ResultModel lmsoracleResult = null;
            var lmsoperatelog = CreateLMSOperateLogEntityModel(model);
            if (IsOperateLMSSQL)
            {
                lmssqlResult = Packing2LmsSQL(model, lmsoperatelog);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = Packing2LmsOracle(model, lmsoperatelog);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }

        private ResultModel Packing2LmsOracle(Model.Log.BillChangeLogModel scchangelogmodel, Model.LMS.OperateLogEntityModel lmsoperatelog)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象，可能由于之前功能未上线，暂时认为是正确的");
            }
            String strMsg = String.Format("LMS物流【Oracle】主库，状态不变，记录日志：{0}", scchangelogmodel.Note);
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    WL_OperateLog_OracleDAL.Add(lmsoperatelog);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【Oracle】版本异常：{0}", ex.StackTrace));
            }
            return result.Succeed(strMsg);
        }

        private ResultModel Packing2LmsSQL(Model.Log.BillChangeLogModel scchangelogmodel, Model.LMS.OperateLogEntityModel lmsoperatelog)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[SQL]主库，状态不变，记录日志：{0}", scchangelogmodel.Note);
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    WL_OperateLog_SQLDAL.Add(lmsoperatelog);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【SQL】版本异常：{0}", ex.StackTrace));
            }
            return result.Succeed(strMsg);
        }
    }
}
