using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// 返货入库
    /// </summary>
    public class ReturnInboundStrategy:BillReturnDetailStrategy
    {
        #region 退货分拣称重单号同步
        /// <summary>
        /// 退货分拣称重单号明细同步到Oracle
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel BillReturnDetail2LmsOracle(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.LMS.OperateLogEntityModel lmsOperatelog, Vancl.TMS.Model.Sorting.Return.BillReturnDetailInfoModel scBillReturnDetailEntityModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[Oracle]主库,逆向状态由{0}变更为{1}", (int)waybillmodel.ReturnStatus, (int)scchangelogmodel.ReturnStatus);
            //waybillmodel.Status = scchangelogmodel.PreStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            waybillmodel.ReturnStatus = (Enums.ReturnStatus)scchangelogmodel.ReturnStatus;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    long BillReturnDetailID = WL_WaybillReturnDetailInfo_OracleDAL.Add(CreateLMSWaybillReturnDetailEntityModel(scBillReturnDetailEntityModel));
                    if (BillReturnDetailID <= 0)
                    {
                        return result.Failed("LMS物流主库【Oracle】版本，新增WaybillReturnDetailInfo记录失败");
                    }
                    waybillmodel.BillReturnDetailID = BillReturnDetailID;
                    WL_Waybill_OracleDAL.UpdateWaybillReturnStatus(waybillmodel);
                    WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
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

        /// <summary>
        /// 退货分拣称重单号明细同步到SQL
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel BillReturnDetail2LmsSQL(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.LMS.OperateLogEntityModel lmsOperatelog, Vancl.TMS.Model.Sorting.Return.BillReturnDetailInfoModel scBillReturnDetailEntityModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[SQL]主库,逆向状态由{0}变更为{1}", (int)waybillmodel.ReturnStatus, (int)scchangelogmodel.ReturnStatus);
            //waybillmodel.Status = scchangelogmodel.PreStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            waybillmodel.ReturnStatus = (Enums.ReturnStatus)scchangelogmodel.ReturnStatus;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    long BillReturnDetailID = WL_WaybillReturnDetailInfo_SQLDAL.Add(CreateLMSWaybillReturnDetailEntityModel(scBillReturnDetailEntityModel));
                    if (BillReturnDetailID <= 0)
                    {
                        return result.Failed("LMS物流主库【Sql】版本，新增WaybillReturnDetailInfo记录失败");
                    }
                    waybillmodel.BillReturnDetailID = BillReturnDetailID;
                    WL_Waybill_SQLDAL.UpdateWaybillReturnStatus(waybillmodel);
                    WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
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
        #endregion

        #region 同步方法
        protected override ResultModel Sync(BillChangeLogModel model)
        {
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null");
            ResultModel result = new ResultModel();
            ResultModel lmssqlResult = null;
            ResultModel lmsoracleResult = null;
            Vancl.TMS.Model.Sorting.Return.BillReturnDetailInfoModel SC_BillReturnDetailEntityModel = SC_BillReturnDetailInfo.GetBillReturnDetailIntoModel4TmsSync2Lms(model.FormCode);
            if (SC_BillReturnDetailEntityModel == null)
            {
                return result.Failed("未取得TMS退货分拣称重BillReturnDetailModel对象");
            }
            var lmsoperatelog = CreateLMSOperateLogEntityModel(model);
            if (IsOperateLMSSQL)
            {
                lmssqlResult = BillReturnDetail2LmsSQL(model, lmsoperatelog,SC_BillReturnDetailEntityModel);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = BillReturnDetail2LmsOracle(model, lmsoperatelog,SC_BillReturnDetailEntityModel);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }

            if (SC_BillReturnDetailInfo.UpdateSyncedStatus4Tms2Lms(SC_BillReturnDetailEntityModel.RBID) <= 0)
            {
                return result.Failed("更新TMS退货分拣称重单号表同步状态失败");
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }
        #endregion
    }
}
