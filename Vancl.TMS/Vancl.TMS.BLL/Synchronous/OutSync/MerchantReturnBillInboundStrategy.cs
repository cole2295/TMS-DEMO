using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public class MerchantReturnBillInboundStrategy : Tms2LmsStrategy
    {

        #region 私有属性
        private Vancl.TMS.IDAL.LMS.ITransitBoxDAL _wl_TransitBox_sql = null;
        private Vancl.TMS.IDAL.LMS.ITransitBoxDAL _wl_TransitBox_oracle = null;
        private Vancl.TMS.IDAL.LMS.IWayBillReturnDAL _wl_WayBillReturn_sql = null;
        private Vancl.TMS.IDAL.LMS.IWayBillReturnDAL _wl_WayBillReturn_oracle = null;
        private Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL _wl_LMS_SYN_FMS_COD_sql = null;
        private Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL _wl_LMS_SYN_FMS_COD_oracle = null;

        #endregion

        #region 数据层
        /// <summary>
        /// LMS物流主库TransitBoxDAL【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.ITransitBoxDAL WL_TransitBox_SQLDAL
        {
            get
            {
                if (_wl_TransitBox_sql == null)
                {
                    _wl_TransitBox_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.ITransitBoxDAL>("LMSTransitBoxDAL_SQL");
                }
                return _wl_TransitBox_sql;
            }
        }
        /// <summary>
        /// LMS物流主库TransitBoxDAL【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.ITransitBoxDAL WL_TransitBox_OracleDAL
        {
            get
            {
                if (_wl_TransitBox_oracle == null)
                {
                    _wl_TransitBox_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.ITransitBoxDAL>("LMSTransitBoxDAL_Oracle");
                }
                return _wl_TransitBox_oracle;
            }
        }
        /// <summary>
        /// LMS物流主库WayBillReturnDAL【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWayBillReturnDAL WL_WayBillReturn_SQLDAL
        {
            get
            {
                if (_wl_WayBillReturn_sql == null)
                {
                    _wl_WayBillReturn_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWayBillReturnDAL>("LMSWayBillReturnDAL_SQL");
                }
                return _wl_WayBillReturn_sql;
            }
        }
        /// <summary>
        /// LMS物流主库WayBillReturnDAL【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWayBillReturnDAL WL_WayBillReturn_OracleDAL
        {
            get
            {
                if (_wl_WayBillReturn_oracle == null)
                {
                    _wl_WayBillReturn_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWayBillReturnDAL>("LMSWayBillReturnDAL_Oracle");
                }
                return _wl_WayBillReturn_oracle;
            }
        }
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

        #endregion
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
        #region 构建操作日志对象
        protected override Model.LMS.OperateLogEntityModel CreateLMSOperateLogEntityModel(BillChangeLogModel scchangelogmodel)
        {
            if (LmsOperateLogEntityModel == null)
            {
                LmsOperateLogEntityModel = base.CreateLMSOperateLogEntityModel(scchangelogmodel);
            }
            LmsOperateLogEntityModel.Status = scchangelogmodel.CurrentStatus;
            return LmsOperateLogEntityModel;
        }
        #endregion
        #region 构建billstatuschangeLog对象
        protected override Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Model.LMS.WaybillEntityModel waybillmodel, BillChangeLogModel scchangelogmodel)
        {
            var lmsChangelog = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            lmsChangelog.CurNode = Model.Common.Enums.StatusChangeNodeType.DeliverCenter;
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.MerchantBillRefund;
            lmsChangelog.SubStatus = (int)scchangelogmodel.ReturnStatus;
            lmsChangelog.Status = waybillmodel.Status;
            return lmsChangelog;
        }
        #endregion
        #region 构建LMS物流主库LMS_SYN_FMS_COD实体对象
        /// <summary>
        /// 创建LMS物流主库LMS_SYN_FMS_COD实体对象
        /// </summary>
        /// <param name="scOutboundModel"></param>
        /// <returns></returns>
        private Vancl.TMS.Model.LMS.LMS_SYN_FMS_CODEntityModel CreateLMSSynFmsCodEntityModel(Vancl.TMS.Model.LMS.WaybillEntityModel waybillModel)
        {
            if (LmsSYN_FMS_CODEntityModel == null)
            {
                LmsSYN_FMS_CODEntityModel = new Model.LMS.LMS_SYN_FMS_CODEntityModel()
                {
                    CODCreateBy = Vancl.TMS.Model.LMS.RFDDeclare.CODCreateBy.ReturnInbound,
                    WaybillNo = waybillModel.WaybillNo,
                    StationID = (int)waybillModel.DeliverStationID,
                    OperateType = (int)Enums.CODOperateType.ReverseInbound,
                    OperateTime = DateTime.Now
                };
                IKeyCodeable ikey = LmsSYN_FMS_CODEntityModel as IKeyCodeable;
                LmsSYN_FMS_CODEntityModel.LmsSynFMSCodKid = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = ikey.SequenceName, TableCode = ikey.TableCode });
            }
            return LmsSYN_FMS_CODEntityModel;
        }
        #endregion
        #region 退库单号同步到
        /// <summary>
        /// 退库单号同步到Oracle
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel MerchantBillReturn2LmsOracle(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.LMS.OperateLogEntityModel lmsOperatelog)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[Oracle]主库,逆向状态由{0}变更为{1}", (int)waybillmodel.ReturnStatus, (int)scchangelogmodel.ReturnStatus);
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = DateTime.Now;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            waybillmodel.ReturnStatus = (Enums.ReturnStatus)scchangelogmodel.ReturnStatus;
            waybillmodel.ReturnExpressCompanyId = scchangelogmodel.CreateDept;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    WL_Waybill_OracleDAL.WaybillReturnInBound(waybillmodel);
                    // WL_Waybill_OracleDAL.UpdateWaybillReturnStatus(waybillmodel);
                    WL_LMS_SYN_FMS_COD_OracleDAL.Add(CreateLMSSynFmsCodEntityModel(waybillmodel));
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
        /// 退库单号同步到SQL
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel MerchantBillReturn2LmsSQL(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.LMS.OperateLogEntityModel lmsOperatelog)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[SQL]主库,逆向状态由{0}变更为{1}", (int)waybillmodel.ReturnStatus, (int)scchangelogmodel.ReturnStatus);
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            waybillmodel.ReturnStatus = (Enums.ReturnStatus)scchangelogmodel.ReturnStatus;
            waybillmodel.ReturnExpressCompanyId = scchangelogmodel.CreateDept;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    WL_Waybill_SQLDAL.WaybillReturnInBound(waybillmodel);
                    //WL_Waybill_SQLDAL.UpdateWaybillReturnStatus(waybillmodel);
                    WL_LMS_SYN_FMS_COD_SQLDAL.Add(CreateLMSSynFmsCodEntityModel(waybillmodel));
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
            var lmsoperatelog = CreateLMSOperateLogEntityModel(model);
            if (IsOperateLMSSQL)
            {
                lmssqlResult = MerchantBillReturn2LmsSQL(model, lmsoperatelog);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = MerchantBillReturn2LmsOracle(model, lmsoperatelog);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }
        #endregion

    }
}
