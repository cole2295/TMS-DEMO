using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// TMS出库数据同步到LMS物流主库策略
    /// </summary>
    public class OutboundStrategy : Tms2LmsStrategy
    {
        #region 出库相关私有属性
        
        private Vancl.TMS.IDAL.LMS.IOutboundDAL _wl_outbound_sql = null;
        private Vancl.TMS.IDAL.LMS.IOutboundDAL _wl_outbound_oracle = null;
        private Vancl.TMS.IDAL.LMS.IWaybill_SortCenterDAL _wl_WaybillSortCenter_sql = null;
        private Vancl.TMS.IDAL.LMS.IWaybill_SortCenterDAL _wl_WaybillSortCenter_oracle = null;
        private Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL _wl_LMS_SYN_FMS_COD_sql = null;
        private Vancl.TMS.IDAL.LMS.ILMS_SYN_FMS_CODDAL _wl_LMS_SYN_FMS_COD_oracle = null;
        private Vancl.TMS.IDAL.LMS.IBatchDAL _wl_batch_sql = null;
        private Vancl.TMS.IDAL.LMS.IBatchDAL _wl_batch_oracle = null;
        private Vancl.TMS.IDAL.Sorting.Outbound.IOutboundDAL _sc_outbound = null;
        private Vancl.TMS.IDAL.Sorting.Outbound.IOutboundBatchDAL _sc_outboundbatch = null;
        
        #endregion

        /// <summary>
        /// TMS分拣出库数据层
        /// </summary>
        public Vancl.TMS.IDAL.Sorting.Outbound.IOutboundDAL SC_OutboundDAL
        {
            get 
            {
                if (_sc_outbound == null)
                {
                    _sc_outbound = ServiceFactory.GetService<Vancl.TMS.IDAL.Sorting.Outbound.IOutboundDAL>("SC_OutboundDAL");
                }
                return _sc_outbound;
            }
        }

        /// <summary>
        /// TMS分拣出库批次数据层
        /// </summary>
        public Vancl.TMS.IDAL.Sorting.Outbound.IOutboundBatchDAL SC_OutboundBatchDAL
        {
            get
            {
                if (_sc_outboundbatch == null)
                {
                    _sc_outboundbatch = ServiceFactory.GetService<Vancl.TMS.IDAL.Sorting.Outbound.IOutboundBatchDAL>("SC_OutboundBatchDAL");
                }
                return _sc_outboundbatch;
            }
        }

        /// <summary>
        /// LMS物流主库Batch【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IBatchDAL WL_Batch_SQLDAL
        {
            get 
            {
                if (_wl_batch_sql == null)
                {
                    _wl_batch_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IBatchDAL>("LMSBatchDAL_SQL");
                }
                return _wl_batch_sql;
            }
        }

        /// <summary>
        /// LMS物流主库Batch【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IBatchDAL WL_Batch_OracleDAL
        {
            get
            {
                if (_wl_batch_oracle == null)
                {
                    _wl_batch_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IBatchDAL>("LMSBatchDAL_Oracle");
                }
                return _wl_batch_oracle;
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

        /// <summary>
        /// LMS物流主库Outbound【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IOutboundDAL WL_Outbound_SQLDAL
        {
            get
            {
                if (_wl_outbound_sql == null)
                {
                    _wl_outbound_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IOutboundDAL>("LMSOutboundDAL_SQL");
                }
                return _wl_outbound_sql;
            }
        }

        /// <summary>
        /// LMS物流主库Outbound【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IOutboundDAL WL_Outbound_OracleDAL
        {
            get
            {
                if (_wl_outbound_oracle == null)
                {
                    _wl_outbound_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IOutboundDAL>("LMSOutboundDAL_Oracle");
                }
                return _wl_outbound_oracle;
            }
        }

        /// <summary>
        /// 物流主库Waybill_SortCenter SQL数据层实现
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybill_SortCenterDAL WL_WaybillSortCenter_SQLDAL
        {
            get
            {
                if (_wl_WaybillSortCenter_sql == null)
                {
                    _wl_WaybillSortCenter_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybill_SortCenterDAL>("LMSWaybill_SortCenterDAL_SQL");
                }
                return _wl_WaybillSortCenter_sql;
            }
        }

        /// <summary>
        /// 物流主库Waybill_SortCenter Oracle数据层实现
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybill_SortCenterDAL WL_WaybillSortCenter_OracleDAL
        {
            get
            {
                if (_wl_WaybillSortCenter_oracle == null)
                {
                    _wl_WaybillSortCenter_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybill_SortCenterDAL>("LMSWaybill_SortCenterDAL_Oracle");
                }
                return _wl_WaybillSortCenter_oracle;
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
            var lmsChangelog  = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            lmsChangelog.CurNode = Model.Common.Enums.StatusChangeNodeType.DeliverCenter;
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.Outbound;
            lmsChangelog.SubStatus = (int)scchangelogmodel.CurrentStatus;
            return lmsChangelog;
        }

        /// <summary>
        /// 创建LMS物流主库Waybill_SortCenter实体对象
        /// </summary>
        /// <param name="scInboundModel"></param>
        /// <returns></returns>
        private Vancl.TMS.Model.LMS.Waybill_SortCenterEntityModel CreateLMSWaybillSortCenterEntityModel(Vancl.TMS.Model.Sorting.Outbound.OutboundEntityModel scOutboundModel)
        {
            if (LmsWaybillSortCenterEntityModel == null)
            {
                LmsWaybillSortCenterEntityModel = new Model.LMS.Waybill_SortCenterEntityModel()
                           {
                               CreateBy = scOutboundModel.CreateBy,
                               CreateTime = scOutboundModel.CreateTime,
                               InboundGuid = null,
                               InBoundKid = null,
                               IsDeleted = false,
                               OutboundGuid = null,
                               OutBoundKid = scOutboundModel.OBID,
                               UpdateBy = scOutboundModel.CreateBy,
                               UpdateTime = scOutboundModel.CreateTime,
                               WaybillNO = long.Parse(scOutboundModel.FormCode)
                           };
            }
            return LmsWaybillSortCenterEntityModel;
        }

        /// <summary>
        /// 创建LMS物流主库LMS_SYN_FMS_COD实体对象
        /// </summary>
        /// <param name="scOutboundModel"></param>
        /// <returns></returns>
        private Vancl.TMS.Model.LMS.LMS_SYN_FMS_CODEntityModel CreateLMSSynFmsCodEntityModel(Vancl.TMS.Model.Sorting.Outbound.OutboundEntityModel scOutboundModel)
        {
            if (LmsSYN_FMS_CODEntityModel == null)
            {
                LmsSYN_FMS_CODEntityModel = new Model.LMS.LMS_SYN_FMS_CODEntityModel()
                {
                    CODCreateBy = "TransferOUT",
                    WaybillNo = long.Parse(scOutboundModel.FormCode),
                    StationID = scOutboundModel.ArrivalID,
                    OperateType = 7,
                    OperateTime = scOutboundModel.CreateTime
                };
                IKeyCodeable ikey = LmsSYN_FMS_CODEntityModel as IKeyCodeable;
                LmsSYN_FMS_CODEntityModel.LmsSynFMSCodKid = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = ikey.SequenceName, TableCode = ikey.TableCode });
            }
            return LmsSYN_FMS_CODEntityModel;
        }

        /// <summary>
        /// 创建LMS物流主库Outbound实体对象
        /// </summary>
        /// <param name="waybillmodel"></param>
        /// <param name="scOutboundModel"></param>
        /// <param name="scchangelogmodel"></param>
        /// <returns></returns>
        private Vancl.TMS.Model.LMS.OutboundEntityModel CreateLMSOutboundEntityModel(Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel, Vancl.TMS.Model.Sorting.Outbound.OutboundEntityModel scOutboundModel, BillChangeLogModel scchangelogmodel)
        {
            return new Model.LMS.OutboundEntityModel()
            {
                BatchNo = scOutboundModel.BatchNo,
                CreateBy = scOutboundModel.CreateBy,
                CreateDept = scchangelogmodel.CreateDept,
                CreateTime = scOutboundModel.CreateTime,
                DeliveryDriver = waybillmodel.DeliveryDriver,
                DeliveryMan = waybillmodel.DeliveryMan,
                IsDeleted = false,
                IsPrint = null,
                OutboundGuid = null,
                OutboundKid = scOutboundModel.OBID,
                OutboundOperator = scOutboundModel.CreateBy,
                OutboundStation = scOutboundModel.DepartureID,
                OutboundTime = scOutboundModel.CreateTime,
                OutStationType = scOutboundModel.OutboundType,
                ToStation = scOutboundModel.ArrivalID,
                UpdateBy = scOutboundModel.UpdateBy,
                UpdateDept = scchangelogmodel.CreateDept,
                UpdateTime = scOutboundModel.UpdateTime,
                WaybillNo = long.Parse(scOutboundModel.FormCode)
            };
        }

        /// <summary>
        /// 创建LMS物流主库Batch实体对象
        /// </summary>
        /// <param name="scOutboundModel"></param>
        /// <param name="scchangelogmodel"></param>
        /// <returns></returns>
        private Vancl.TMS.Model.LMS.BatchEntityModel CreateLMSBatchEntityModel(Vancl.TMS.Model.Sorting.Outbound.OutboundBatchEntityModel scBacthModel, BillChangeLogModel scchangelogmodel)
        {
            if (LmsBatchEntityModel == null)
            {
                LmsBatchEntityModel = new Model.LMS.BatchEntityModel()
                {
                    BatchNo = scBacthModel.BatchNo,
                    BatchOperator = scBacthModel.CreateBy,
                    CreateBy = scBacthModel.CreateBy,
                    CreateDept = scchangelogmodel.CreateDept,
                    CreateTime = scBacthModel.CreateTime,
                    IsDeleted = false,
                    OperStation = scBacthModel.DepartureID,
                    ReceiveStation = scBacthModel.ArrivalID,
                    OperTime = scBacthModel.CreateTime,
                    UpdateBy = scBacthModel.UpdateBy,
                    UpdateDept = scchangelogmodel.CreateDept,
                    UpdateTime = scBacthModel.CreateTime
                };
            }
            return LmsBatchEntityModel;
        }

        #region 物流主库[SQL]和[Oracle]共用的实体对象

        /// <summary>
        /// LMS物流主库OperateLog实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.OperateLogEntityModel LmsOperateLogEntityModel = null;
        /// <summary>
        /// LMS物流主库batch实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.BatchEntityModel LmsBatchEntityModel = null;
        /// <summary>
        /// LMS物流主库分拣关系实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.Waybill_SortCenterEntityModel LmsWaybillSortCenterEntityModel = null;
        /// <summary>
        /// LMS物流主库COD财务实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.LMS_SYN_FMS_CODEntityModel LmsSYN_FMS_CODEntityModel = null;

        #endregion

        /// <summary>
        /// 出库同步到Oracle
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel Outbound2LmsOracle(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.Sorting.Outbound.OutboundEntityModel scOutboundEntityModel, bool IsBatchSynced, Vancl.TMS.Model.Sorting.Outbound.OutboundBatchEntityModel scBacthModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象，可能由于之前功能未上线，暂时认为是正确的");
            }
            bool IsCodRelated = false;
            if (waybillmodel.Source == Enums.BillSource.Others)
            {
                if (scOutboundEntityModel.OutboundType == Enums.SortCenterOperateType.SimpleSorting
                    || scOutboundEntityModel.OutboundType == Enums.SortCenterOperateType.DistributionSorting)
                {
                    if (waybillmodel.CurrentDistributionCode.ToString().Equals("rfd"))
                    {
                        IsCodRelated = true;
                    }
                }
            }
            String strMsg = String.Format("LMS物流[Oracle]主库,状态由{0}变更为{1},当前配送商由{2}变更为{3}"
    , (int)waybillmodel.Status
    , (int)scchangelogmodel.CurrentStatus
    , waybillmodel.CurrentDistributionCode
    , scchangelogmodel.CurrentDistributionCode
    );
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            if (IsCodRelated)
            {
                waybillmodel.DeliveryTime = DateTime.Now;
            }
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    long OutboundID = WL_Outbound_OracleDAL.Add(CreateLMSOutboundEntityModel(waybillmodel, scOutboundEntityModel, scchangelogmodel));
                    if (OutboundID <= 0)
                    {
                        return result.Failed("LMS物流主库【Oracle】版本，新增Outbound记录失败");
                    }
                    waybillmodel.OutboundID = OutboundID;
                    WL_Waybill_OracleDAL.UpdateWaybillModelByOutbound(waybillmodel);
                    WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    WL_WaybillSortCenter_OracleDAL.Merge(CreateLMSWaybillSortCenterEntityModel(scOutboundEntityModel));
                    if (IsCodRelated)
                    {
                        WL_LMS_SYN_FMS_COD_OracleDAL.Add(CreateLMSSynFmsCodEntityModel(scOutboundEntityModel));
                    }
                    //批次未同步，批次对象不是空【多线程并发，取得的scBacthModel可能为null】
                    if (!IsBatchSynced && scBacthModel != null)
                    {
                        //采用merge防止多线程并发产生重复数据
                        WL_Batch_OracleDAL.Add(CreateLMSBatchEntityModel(scBacthModel, scchangelogmodel));
                    }
                    scope.Complete();
                }

                WL_OperateLog_OracleDAL.Add(CreateLMSOperateLogEntityModel(scchangelogmodel));
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【Oracle】版本异常：{0}", ex.StackTrace));
            }
            return result.Succeed(strMsg);
        }

        /// <summary>
        /// 出库同步到SQL
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel Outbound2LmsSQL(BillChangeLogModel scchangelogmodel, Vancl.TMS.Model.Sorting.Outbound.OutboundEntityModel scOutboundEntityModel, bool IsBatchSynced, Vancl.TMS.Model.Sorting.Outbound.OutboundBatchEntityModel scBacthModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            bool IsCodRelated = false;
            if (waybillmodel.Source == Enums.BillSource.Others)
            {
                if (scOutboundEntityModel.OutboundType == Enums.SortCenterOperateType.SimpleSorting
                    || scOutboundEntityModel.OutboundType == Enums.SortCenterOperateType.DistributionSorting)
                {
                    if (waybillmodel.CurrentDistributionCode.ToString().Equals("rfd"))
                    {
                        IsCodRelated = true;
                    }
                }
            }
            String strMsg = String.Format("LMS物流[SQL]主库,状态由{0}变更为{1},当前配送商由{2}变更为{3}"
                , (int)waybillmodel.Status
                , (int)scchangelogmodel.CurrentStatus
                , waybillmodel.CurrentDistributionCode
                , scchangelogmodel.CurrentDistributionCode
                );
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;          
            if (IsCodRelated)
            {
                waybillmodel.DeliveryTime = DateTime.Now;
            }
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    long OutboundID = WL_Outbound_SQLDAL.Add(CreateLMSOutboundEntityModel(waybillmodel,scOutboundEntityModel,scchangelogmodel));
                    if (OutboundID <= 0)
                    {
                        return result.Failed("LMS物流主库【SQL】版本，新增Outbound记录失败");
                    }
                    waybillmodel.OutboundID = OutboundID;
                    WL_Waybill_SQLDAL.UpdateWaybillModelByOutbound(waybillmodel);
                    WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                    WL_WaybillSortCenter_SQLDAL.Merge(CreateLMSWaybillSortCenterEntityModel(scOutboundEntityModel));
                    WL_OperateLog_SQLDAL.Add(CreateLMSOperateLogEntityModel(scchangelogmodel));
                    if (IsCodRelated)
                    {
                        WL_LMS_SYN_FMS_COD_SQLDAL.Add(CreateLMSSynFmsCodEntityModel(scOutboundEntityModel));
                    }
                    //批次未同步，批次对象不是空【多线程并发，取得的scBacthModel可能为null】
                    if (!IsBatchSynced && scBacthModel != null)
                    {
                        //采用merge防止多线程并发产生重复数据
                        WL_Batch_SQLDAL.Add(CreateLMSBatchEntityModel(scBacthModel, scchangelogmodel));
                    }
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
            Vancl.TMS.Model.Sorting.Outbound.OutboundEntityModel SC_OutboundEntityModel = SC_OutboundDAL.GetOutboundEntityModel4TmsSync2Lms(model.FormCode);
            if (SC_OutboundEntityModel == null)
            {
                result.Failed("未取得TMS分拣出库OutboundEntityModel对象");
            }
            ///是否出库批次已经同步
            bool IsBatchSynced = SC_OutboundBatchDAL.IsTmsSync2Lms(SC_OutboundEntityModel.BatchNo);
            Vancl.TMS.Model.Sorting.Outbound.OutboundBatchEntityModel scBatchEntityModel = null;
            if (!IsBatchSynced)
            {
                scBatchEntityModel = SC_OutboundBatchDAL.GetOutboundBatchEntityModel4TmsSync2Lms(SC_OutboundEntityModel.BatchNo);
            }
            if (IsOperateLMSSQL)
            {
                lmssqlResult = Outbound2LmsSQL(model, SC_OutboundEntityModel, IsBatchSynced, scBatchEntityModel);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = Outbound2LmsOracle(model, SC_OutboundEntityModel, IsBatchSynced, scBatchEntityModel);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }
            if (!IsBatchSynced && scBatchEntityModel != null)
            {
                if (SC_OutboundBatchDAL.UpdateSyncedStatus4Tms2Lms(scBatchEntityModel.OBBID) <= 0)
                {
                    return result.Failed("更新TMS 出库批次表同步状态失败");
                }
            }
            if (SC_OutboundDAL.UpdateSyncedStatus4Tms2Lms(SC_OutboundEntityModel.OBID) <= 0)
            {
                return result.Failed("更新TMS 出库表同步状态失败");
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }






    }

}
