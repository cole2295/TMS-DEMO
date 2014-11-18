using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public class MerchantBillRefundStrategy : Tms2LmsStrategy
    {
        #region 私有属性
        private Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL _wl_LMS_SYN_FMS_COD_sql = null;
        private Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL _wl_LMS_SYN_FMS_COD_oracle = null;
        #endregion

        #region 数据层
        /// <summary>
        /// LMS物流主库LMS_SYN_FMS_COD【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL WL_LMS_SYN_FMS_COD_SQLDAL
        {
            get
            {
                if (_wl_LMS_SYN_FMS_COD_sql == null)
                {
                    _wl_LMS_SYN_FMS_COD_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL>("LMS_SYN_FMS_CODDAL_SQL");
                }
                return _wl_LMS_SYN_FMS_COD_sql;
            }
        }

        /// <summary>
        /// LMS物流主库LMS_SYN_FMS_COD【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL WL_LMS_SYN_FMS_COD_OracleDAL
        {
            get
            {
                if (_wl_LMS_SYN_FMS_COD_oracle == null)
                {
                    _wl_LMS_SYN_FMS_COD_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL>("LMS_SYN_FMS_CODDAL_Oracle");
                }
                return _wl_LMS_SYN_FMS_COD_oracle;
            }
        }
        protected override Model.LMS.OperateLogEntityModel CreateLMSOperateLogEntityModel(BillChangeLogModel scchangelogmodel)
        {
            if (LmsOperateLogEntityModel == null)
            {
                LmsOperateLogEntityModel = base.CreateLMSOperateLogEntityModel(scchangelogmodel);
            }
            LmsOperateLogEntityModel.Status = scchangelogmodel.CurrentStatus;
            return LmsOperateLogEntityModel;
        }

        protected override Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Model.LMS.WaybillEntityModel waybillmodel, BillChangeLogModel scchangelogmodel)
        {
            var lmsChangelog = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            lmsChangelog.CurNode = Model.Common.Enums.StatusChangeNodeType.DeliverCenter;
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.Outbound;
            lmsChangelog.SubStatus = (int)scchangelogmodel.CurrentStatus;
            lmsChangelog.Status = waybillmodel.Status;
            return lmsChangelog;
        }
        /// <summary>
        /// 创建LMS物流主库LMS_SYN_FMS_COD实体对象
        /// </summary>
        /// <param name="scOutboundModel"></param>
        /// <returns></returns>
        private Vancl.TMS.Model.LMS.LMS_SYN_FMS_CODEntityModel CreateLMSSynFmsCodEntityModel(Vancl.TMS.Model.LMS.WaybillEntityModel billModel)
        {
            if (LmsSYN_FMS_CODEntityModel == null)
            {
                LmsSYN_FMS_CODEntityModel = new Model.LMS.LMS_SYN_FMS_CODEntityModel()
                {
                    CODCreateBy = "ReturnInbound",
                    WaybillNo = billModel.WaybillNo,
                    StationID = (int)billModel.DeliverStationID,
                    UpdateTime=DateTime.Now,
                    OperateType = 6,
                    OperateTime = billModel.CreateTime
                };
                IKeyCodeable ikey = LmsSYN_FMS_CODEntityModel as IKeyCodeable;
                LmsSYN_FMS_CODEntityModel.LmsSynFMSCodKid = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = ikey.SequenceName, TableCode = ikey.TableCode });
            }
            return LmsSYN_FMS_CODEntityModel;
        }


        #region 物流主库[SQL]和[Oracle]共用的实体对象

        /// <summary>
        /// LMS物流主库OperateLog实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.OperateLogEntityModel LmsOperateLogEntityModel = null;
        /// <summary>
        /// LMS物流主库COD财务实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.LMS_SYN_FMS_CODEntityModel LmsSYN_FMS_CODEntityModel = null;


        #endregion



        /// <summary>
        /// 商家入库确认同步到Oracle
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel MerchantBillRefund2LmsOracle(BillChangeLogModel scchangelogmodel)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象，可能由于之前功能未上线，暂时认为是正确的");
            }
            String strMsg = String.Format("LMS物流[Oracle]主库,逆向状态由{0}变更为{1},当前配送商由{2}变更为{3}"
    , (int)waybillmodel.ReturnStatus
    , (int)scchangelogmodel.ReturnStatus
    , waybillmodel.CurrentDistributionCode
    , scchangelogmodel.CurrentDistributionCode
    );
            //waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    
                    WL_Waybill_OracleDAL.UpdateWaybillReturnStatus(waybillmodel);
                    WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    WL_OperateLog_OracleDAL.Add(CreateLMSOperateLogEntityModel(scchangelogmodel));

                    WL_LMS_SYN_FMS_COD_OracleDAL.Add(CreateLMSSynFmsCodEntityModel(waybillmodel));

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
        /// 商家入库确认同步到SQL
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel MerchantBillRefund2LmsSQL(BillChangeLogModel scchangelogmodel)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[SQL]主库,逆向状态由{0}变更为{1},当前配送商由{2}变更为{3}"
                , (int)waybillmodel.ReturnStatus
                , (int)scchangelogmodel.ReturnStatus
                , waybillmodel.CurrentDistributionCode
                , scchangelogmodel.CurrentDistributionCode
                );
            //waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    WL_Waybill_SQLDAL.UpdateWaybillReturnStatus(waybillmodel);
                    WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    WL_OperateLog_SQLDAL.Add(CreateLMSOperateLogEntityModel(scchangelogmodel));
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【SQL】版本异常：{0}", ex.StackTrace));
            }
            return result.Succeed(strMsg);
        }
        protected override ResultModel Sync(BillChangeLogModel model)
        {
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null");
            ResultModel result = new ResultModel();
            ResultModel lmssqlResult = null;
            ResultModel lmsoracleResult = null;
            if (IsOperateLMSSQL)
            {
                lmssqlResult = MerchantBillRefund2LmsSQL(model);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = MerchantBillRefund2LmsOracle(model);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }

        #endregion

    }
}
