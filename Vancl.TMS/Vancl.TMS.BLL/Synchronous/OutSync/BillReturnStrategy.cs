using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public class BillReturnStrategy : Tms2LmsStrategy
    {
        #region 私有属性

        private Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL _wl_WaybillReturnDetailInfo_sql = null;
        private Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL _wl_WaybillReturnBoxInfo_sql = null;
        private Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL _wl_WaybillReturnDetailInfo_oracle = null;
        private Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL _wl_WaybillReturnBoxInfo_oracle = null;
        private Vancl.TMS.IDAL.Sorting.Return.IBillReturnDetailInfoDAL _sc_BillReturnDetailInfo = null;
        private Vancl.TMS.IDAL.Sorting.Return.IBillReturnBoxInfoDAL _sc_BillReturnBoxInfo = null;

        #endregion

        /// <summary>
        /// TMS退货分拣称重单号数据层
        /// </summary>
        public Vancl.TMS.IDAL.Sorting.Return.IBillReturnDetailInfoDAL SC_BillReturnDetailInfo
        {
            get
            {
                if (_sc_BillReturnDetailInfo == null)
                {
                    _sc_BillReturnDetailInfo = ServiceFactory.GetService<Vancl.TMS.IDAL.Sorting.Return.IBillReturnDetailInfoDAL>();
                }
                return _sc_BillReturnDetailInfo;
            }
        }
        /// <summary>
        /// TMS退货分拣称重箱号数据层
        /// </summary>
        public Vancl.TMS.IDAL.Sorting.Return.IBillReturnBoxInfoDAL SC_BillReturnBoxInfo
        {
            get
            {
                if (_sc_BillReturnBoxInfo == null)
                {
                    _sc_BillReturnBoxInfo = ServiceFactory.GetService<Vancl.TMS.IDAL.Sorting.Return.IBillReturnBoxInfoDAL>();
                }
                return _sc_BillReturnBoxInfo;
            }
        }
        /// <summary>
        /// LMS物流主库WaybillDetailInfo【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL WL_WaybillReturnDetailInfo_SQLDAL
        {
            get
            {
                if (_wl_WaybillReturnDetailInfo_sql == null)
                {
                    _wl_WaybillReturnDetailInfo_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL>();
                }
                return _wl_WaybillReturnDetailInfo_sql;
            }
        }
        /// <summary>
        /// LMS物流主库WaybillReturnDetailInfo【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL WL_WaybillReturnDetailInfo_OracleDAL
        {
            get
            {
                if (_wl_WaybillReturnDetailInfo_oracle == null)
                {
                    _wl_WaybillReturnDetailInfo_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL>();
                }
                return _wl_WaybillReturnDetailInfo_oracle;
            }
        }
        /// <summary>
        /// LMS物流主库WaybillReturnBoxInfo【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL WL_WaybillReturnBoxInfo_OracleDAL
        {
            get
            {
                if (_wl_WaybillReturnBoxInfo_oracle == null)
                {
                    _wl_WaybillReturnBoxInfo_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL>();
                }
                return _wl_WaybillReturnBoxInfo_oracle;
            }
        }

        /// <summary>
        /// LMS物流主库WaybillBoxInfo【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL WL_WaybillReturnBoxInfo_SQLDAL
        {
            get
            {
                if (_wl_WaybillReturnBoxInfo_sql == null)
                {
                    _wl_WaybillReturnBoxInfo_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL>();
                }
                return _wl_WaybillReturnBoxInfo_sql;
            }
        }

        protected override Model.LMS.OperateLogEntityModel CreateLMSOperateLogEntityModel(BillChangeLogModel scchangelogmodel)
        {
            if (LmsOperateLogEntityModel == null)
            {
                LmsOperateLogEntityModel = base.CreateLMSOperateLogEntityModel(scchangelogmodel);
            }
            return LmsOperateLogEntityModel;
        }

        protected override Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Model.LMS.WaybillEntityModel waybillmodel, BillChangeLogModel scchangelogmodel)
        {
            var lmsChangelog = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            lmsChangelog.CurNode = Model.Common.Enums.StatusChangeNodeType.DeliverCenter;
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.ReturnInbound;
            lmsChangelog.SubStatus = (int)scchangelogmodel.ReturnStatus;
            return lmsChangelog;
        }
        /// <summary>
        /// 创建LMS物流主库退货分拣称重明细单号实体对象
        /// </summary>
        /// <param name="waybillmodel"></param>
        /// <param name="scBillReturnDetailInfoModel"></param>
        /// <param name="scchangelogmodel"></param>
        /// <returns></returns>
        private Vancl.TMS.Model.LMS.WaybillReturnDetailInfoEntityModel CreateLMSWaybillReturnDetailEntityModel(Vancl.TMS.Model.Sorting.Return.BillReturnDetailInfoModel scBillReturnDetailInfoModel)
        {
            return new Model.LMS.WaybillReturnDetailInfoEntityModel()
            {
                BoxNo=scBillReturnDetailInfoModel.BoxNo,
                CreateDep=scBillReturnDetailInfoModel.CreateDept,
                ReturnTo=scBillReturnDetailInfoModel.ReturnTo,
                Weight=scBillReturnDetailInfoModel.Weight,
                OrderNo=scBillReturnDetailInfoModel.CustomerOrder
            };
        }
        /// <summary>
        /// 创建LMS物流主库退货分拣称重箱号实体对象
        /// </summary>
        /// <param name="waybillmodel"></param>
        /// <param name="scBillReturnDetailInfoModel"></param>
        /// <param name="scchangelogmodel"></param>
        /// <returns></returns>
        private Vancl.TMS.Model.LMS.WaybillReturnBoxInfoEntityModel CreateLMSWaybillReturnBoxEntityModel(Vancl.TMS.Model.Sorting.Return.BillReturnBoxInfoModel scBillReturnBoxInfoModel)
        {
            return new Model.LMS.WaybillReturnBoxInfoEntityModel()
            {
                CreateBy=scBillReturnBoxInfoModel.CreateBy,
                IsPrintBackForm=scBillReturnBoxInfoModel.IsPrintBackForm,
                IsPrintBackPacking=scBillReturnBoxInfoModel.IsPrintBackPacking,
                ReturnMerchant=scBillReturnBoxInfoModel.ReturnMerchant,
                CreateTime=scBillReturnBoxInfoModel.CreateTime,
                BoxNo = scBillReturnBoxInfoModel.BoxNo,
                CreateDep = scBillReturnBoxInfoModel.CreateDept,
                ReturnTo = scBillReturnBoxInfoModel.ReturnTo,
                Weight = scBillReturnBoxInfoModel.Weight
            };
        }
        #region 物流主库[SQL]和[Oracle]共用的实体对象

        /// <summary>
        /// LMS物流主库OperateLog实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.OperateLogEntityModel LmsOperateLogEntityModel = null;
        #endregion


        #region 退货分拣称重单号同步
        /// <summary>
        /// 退货分拣称重单号明细同步到Oracle
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel BillReturnDetail2LmsOracle(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.Sorting.Return.BillReturnDetailInfoModel scBillReturnDetailEntityModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[Oracle]主库,逆向状态由{0}变更为{1}", (int)waybillmodel.ReturnStatus, (int)scchangelogmodel.ReturnStatus);
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
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
                    WL_OperateLog_OracleDAL.Add(CreateLMSOperateLogEntityModel(scchangelogmodel));
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
        private ResultModel BillReturnDetail2LmsSQL(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.Sorting.Return.BillReturnDetailInfoModel scBillReturnDetailEntityModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[SQL]主库,逆向状态由{0}变更为{1}", (int)waybillmodel.ReturnStatus, (int)scchangelogmodel.ReturnStatus);
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            waybillmodel.ReturnStatus =(Enums.ReturnStatus)scchangelogmodel.ReturnStatus;
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
        #endregion

        #region 退货分拣称重箱号同步
        /// <summary>
        /// 退货分拣称重箱号明细同步到Oracle
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel BillReturnBox2LmsOracle(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.Sorting.Return.BillReturnBoxInfoModel scBillReturnBoxEntityModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            //Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            //if (waybillmodel == null)
            //{
            //    return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象");
            //}
            //String strMsg = String.Format("LMS物流[Oracle]主库,逆向状态由{0}变更为{1}", (int)waybillmodel.ReturnStatus, (int)scchangelogmodel.ReturnStatus);
            //waybillmodel.Status = scchangelogmodel.CurrentStatus;
            //waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            //waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            //waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            //waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            //waybillmodel.ReturnStatus = (Enums.ReturnStatus)scchangelogmodel.ReturnStatus;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    long BillReturnBoxlID = WL_WaybillReturnBoxInfo_OracleDAL.Add(CreateLMSWaybillReturnBoxEntityModel(scBillReturnBoxEntityModel));
                    if (BillReturnBoxlID <= 0)
                    {
                        return result.Failed("LMS物流主库【Oracle】版本，新增WaybillReturnBoxInfo记录失败");
                    }
                    //waybillmodel.BillReturnBoxID = BillReturnBoxlID;
                    //WL_Waybill_OracleDAL.UpdateWaybillReturnStatus(waybillmodel);
                    //WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    //WL_OperateLog_OracleDAL.Add(CreateLMSOperateLogEntityModel(scchangelogmodel));
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【Oracle】版本异常：{0}", ex.StackTrace));
            }
            return result.Succeed("");
        }

        /// <summary>
        /// 退货分拣称重箱号明细同步到SQL
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel BillReturnBox2LmsSQL(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.Sorting.Return.BillReturnBoxInfoModel scBillReturnBoxEntityModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            //Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            //if (waybillmodel == null)
            //{
            //    return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            //}
            //String strMsg = String.Format("LMS物流[SQL]主库,逆向状态由{0}变更为{1}", (int)waybillmodel.ReturnStatus, (int)scchangelogmodel.ReturnStatus);
            //waybillmodel.Status = scchangelogmodel.CurrentStatus;
            //waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            //waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            //waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            //waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            //waybillmodel.ReturnStatus = (Enums.ReturnStatus)scchangelogmodel.ReturnStatus;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    long BillReturnBoxID = WL_WaybillReturnBoxInfo_SQLDAL.Add(CreateLMSWaybillReturnBoxEntityModel(scBillReturnBoxEntityModel));
                    if (BillReturnBoxID <= 0)
                    {
                        return result.Failed("LMS物流主库【Sql】版本，新增WaybillReturnBoxInfo记录失败");
                    }
                    //waybillmodel.BillReturnDetailID = BillReturnBoxID;
                    //WL_Waybill_SQLDAL.UpdateWaybillReturnStatus(waybillmodel);
                    //WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    //WL_OperateLog_SQLDAL.Add(CreateLMSOperateLogEntityModel(scchangelogmodel));
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【SQL】版本异常：{0}", ex.StackTrace));
            }
            return result.Succeed("");
        }


        #endregion

        protected override ResultModel Sync(BillChangeLogModel model)
        {
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null");
            ResultModel result = new ResultModel();
            ResultModel lmssqlResult = null;
            ResultModel lmsoracleResult = null;
            Vancl.TMS.Model.Sorting.Return.BillReturnDetailInfoModel SC_BillReturnDetailEntityModel = SC_BillReturnDetailInfo.GetBillReturnDetailIntoModel4TmsSync2Lms(model.FormCode);
            if (SC_BillReturnDetailEntityModel == null)
            {
                result.Failed("未取得TMS退货分拣称重BillReturnDetailModel对象");
            }
            if (IsOperateLMSSQL)
            {
                lmssqlResult = BillReturnDetail2LmsSQL(model, SC_BillReturnDetailEntityModel);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = BillReturnDetail2LmsOracle(model, SC_BillReturnDetailEntityModel);
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
    }
}
