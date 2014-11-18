using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// 称重打印
    /// </summary>
    public partial class WeighPrintStrategy : Tms2LmsStrategy
    {
        protected override ResultModel Sync(BillChangeLogModel model)
        {
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null.");
            var r = this.BillWeightSyncORACLE(model, base.CreateLMSOperateLogEntityModel(model));
            var r1=new ResultModel();
            if (IsOperateLMSSQL)
            {
                r1 = this.BillWeightSyncSQL(model, base.CreateLMSOperateLogEntityModel(model));
            }
            if (!string.IsNullOrWhiteSpace(r1.Message))
                r.Message += "," + r1.Message;
            return r;

        }


        protected override Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Model.LMS.WaybillEntityModel waybillmodel, BillChangeLogModel scchangelogmodel)
        {
            var model = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            model.OperateType = Enums.Lms2TmsOperateType.BillPrint;
            model.SubStatus = (int)scchangelogmodel.CurrentStatus;
            model.CurNode = Enums.StatusChangeNodeType.DeliverCenter;

            return model;
        }


        /// <summary>
        /// 面单称重打印同步到LMS物流SQL主库
        /// </summary>
        /// <param name="waybillmodel">运单</param>
        /// <param name="scchangelogmodel"></param>
        /// <param name="lmsOperatelog"></param>
        /// <param name="operateLogEntityModel"></param>
        /// <returns></returns>
        private ResultModel BillWeightSyncSQL(
                        BillChangeLogModel scchangelogmodel,
                        OperateLogEntityModel lmsOperatelog)
        {
            WeighPrintSync weighPrintSync = new WeighPrintSync();
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            decimal weight = weighPrintSync.GetBillWeight(scchangelogmodel.FormCode);

            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    //1、更新运单状态
                    if (scchangelogmodel.CurrentStatus != scchangelogmodel.PreStatus)
                    {
                        WL_Waybill_SQLDAL.UpdateWaybillStatus(waybillmodel);
                        //2.添加状态改变日志
                        WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    }
                    //3.添加操作日志
                    WL_OperateLog_SQLDAL.Add(lmsOperatelog);
                    //4.更新BillInfo Weight
                    result = weighPrintSync.SyncBillWeight(SyncDbType.Sqlserver, scchangelogmodel.FormCode, weight);
                    //     if (!result.IsSuccess) return result.Failed(result.Message);
                    //5.同步BillWeigh表
                    result = weighPrintSync.SyncBillPackageInfo(SyncDbType.Sqlserver, scchangelogmodel.FormCode);
                    //     if (!result.IsSuccess) return result.Failed(result.Message);
                    scope.Complete();
                }

            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【SQL】版本异常：{0}", ex.Message + ex.StackTrace));
            }


            //更改同步状态
            weighPrintSync.UpdateWeighSyncStatus(scchangelogmodel.FormCode, Enums.SyncStatus.NotYet, Enums.SyncStatus.Already);
            return result.Succeed("成功同步重量信息");

        }

        /// <summary>
        /// 面单称重打印同步到LMS物流ORACLE主库
        /// </summary>
        /// <param name="waybillmodel">运单</param>
        /// <param name="scchangelogmodel"></param>
        /// <param name="lmsOperatelog"></param>
        /// <param name="operateLogEntityModel"></param>
        /// <returns></returns>
        private ResultModel BillWeightSyncORACLE(
                        BillChangeLogModel scchangelogmodel,
                        OperateLogEntityModel lmsOperatelog)
        {
            WeighPrintSync weighPrintSync = new WeighPrintSync();
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【ORACLE】WaybillEntityModel对象");
            }
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            decimal weight = weighPrintSync.GetBillWeight(scchangelogmodel.FormCode);

            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    //Oracle
                    //1、更新运单状态
                    if (scchangelogmodel.CurrentStatus != scchangelogmodel.PreStatus)
                    {
                        WL_Waybill_OracleDAL.UpdateWaybillStatus(waybillmodel);
                        //2.添加状态改变日志
                        WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    }
                    //3.添加操作日志
                    WL_OperateLog_OracleDAL.Add(lmsOperatelog);
                    //4.更新BillInfo Weight
                    result = weighPrintSync.SyncBillWeight(SyncDbType.Oracle, scchangelogmodel.FormCode, weight);
                    //  if (!result.IsSuccess) return result.Failed(result.Message);

                    scope.Complete();
                }

                //5.同步BillWeigh表
                result = weighPrintSync.SyncBillPackageInfo(SyncDbType.Oracle, scchangelogmodel.FormCode);
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【Oracle】版本异常：{0}", ex.Message + ex.StackTrace));
            }

            //更改同步状态
            weighPrintSync.UpdateWeighSyncStatus(scchangelogmodel.FormCode, Enums.SyncStatus.NotYet, Enums.SyncStatus.Already);
            return result.Succeed("成功同步重量信息");

        }


    }
}
