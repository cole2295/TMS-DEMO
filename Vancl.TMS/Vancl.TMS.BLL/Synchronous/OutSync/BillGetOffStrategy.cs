using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Sorting.Loading;
using Vancl.TMS.IDAL.Sorting.Loading;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// 运单下车策略
    /// </summary>
    public class BillGetOffStrategy : Tms2LmsStrategy
    {
        private IWaybillTruckDAL _wl_waybillTruck_sql = null;
        private IWaybillTruckDAL _wl_waybillTruck_oracle = null;
        private IGpsOrderStatusDAL _wl_gpsOrderStatus_sql = null;
        private IGpsOrderStatusDAL _wl_gpsOrderStatus_oracle =null;
        private IBillTruckDAL _sc_billTruck = null;

        /// <summary>
        /// TMS 订单装车数据层接口
        /// </summary>
        public IBillTruckDAL SC_BillTruck
        {
            get
            {
                if (_sc_billTruck == null)
                {
                    _sc_billTruck = ServiceFactory.GetService<IBillTruckDAL>("BillTruckDAL");
                }
                return _sc_billTruck;
            }
        }

        public IWaybillTruckDAL WL_WaybillTruck_SQLDAL
        {
            get
            {
                if (_wl_waybillTruck_sql == null)
                {
                    _wl_waybillTruck_sql = ServiceFactory.GetService<IWaybillTruckDAL>("WaybillTruckDAL_SQL");
                }
                return _wl_waybillTruck_sql;
            }
        }

        public IWaybillTruckDAL WL_WaybillTruck_OracleDAL
        {
            get
            {
                if (_wl_waybillTruck_oracle == null)
                {
                    _wl_waybillTruck_oracle = ServiceFactory.GetService<IWaybillTruckDAL>("WaybillTruckDAL_Oracle");
                }
                return _wl_waybillTruck_oracle;
            }
        }

        public IGpsOrderStatusDAL WL_GpsOrderStatus_SQLDAL
        {
            get
            {
                if (_wl_gpsOrderStatus_sql == null)
                {
                    _wl_gpsOrderStatus_sql = ServiceFactory.GetService<IGpsOrderStatusDAL>("GpsOrderStatusDAL_SQL");
                }
                return _wl_gpsOrderStatus_sql;
            }
        }

        public IGpsOrderStatusDAL WL_GpsOrderStatus_OracleDAL
        {
            get
            {
                if (_wl_gpsOrderStatus_oracle == null)
                {
                    _wl_gpsOrderStatus_oracle = ServiceFactory.GetService<IGpsOrderStatusDAL>("GpsOrderStatusDAL_Oracle");
                }
                return _wl_gpsOrderStatus_oracle;
            }
        }

        protected override Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Model.LMS.WaybillEntityModel waybillmodel, Model.Log.BillChangeLogModel scchangelogmodel)
        {
            var lmsChangelog = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            lmsChangelog.CurNode = Model.Common.Enums.StatusChangeNodeType.TransportCenter;
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.BillGetOff;
            lmsChangelog.SubStatus = (int)scchangelogmodel.CurrentStatus;
            return lmsChangelog;
        }

        protected WaybillTruckEntityModel CreateLMSWaybillTruckEntityModel(BillTruckModel billTruckModel)
        {
            return new WaybillTruckEntityModel()
            {
                BatchNO = long.Parse(billTruckModel.BatchNO),
                CreateBy = billTruckModel.CreateBy,
                CreateTime = billTruckModel.CreateTime,
                DriverId = billTruckModel.DriverID,
                GpsID = billTruckModel.GPSNO,
                IsDelete = billTruckModel.IsDeleted,
                TruckNO = billTruckModel.TruckNO,
                UpdateBy = billTruckModel.UpdateBy,
                UpdateTime = billTruckModel.UpdateTime,
                WaybillNO = long.Parse(billTruckModel.FormCode)
            };
        }

        private ResultModel BillTruck2LmsOracle(Model.Log.BillChangeLogModel scchangelogmodel, OperateLogEntityModel lmsOperatelog, WaybillTruckEntityModel waybillTruckEntityModel)
        {
            ResultModel result = new ResultModel();
            WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象，未全部切换暂时认为正确");
            }
            String strMsg = String.Format("LMS物流[Oracle]主库,状态由{0}变更为{1}"
            , (int)waybillmodel.Status
            , (int)scchangelogmodel.CurrentStatus
            );
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;

            using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
            {
                GPSOrderStatusEntityModel gpssmodel = WL_GpsOrderStatus_OracleDAL.GetGPSOrderStatus(waybillTruckEntityModel.WaybillNO.ToString());
                if (gpssmodel != null)
                {
                    gpssmodel.status = 5;
                    WL_GpsOrderStatus_OracleDAL.AddGpsOrderStatus(gpssmodel);
                }
                WL_WaybillTruck_OracleDAL.RemoveBillTruck(waybillTruckEntityModel);
                WL_Waybill_OracleDAL.UpdateWaybillStatus(waybillmodel);
                WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                WL_OperateLog_OracleDAL.Add(lmsOperatelog);
                scope.Complete();
            }
            return result.Succeed(strMsg);
        }

        private ResultModel BillTruck2LmsSQL(Model.Log.BillChangeLogModel scchangelogmodel, OperateLogEntityModel lmsOperatelog, WaybillTruckEntityModel waybillTruckEntityModel)
        {
            ResultModel result = new ResultModel();
            WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            String strMsg = String.Format("LMS物流[SQL]主库,状态由{0}变更为{1}"
            , (int)waybillmodel.Status
            , (int)scchangelogmodel.CurrentStatus
            );
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;

            using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
            {
                GPSOrderStatusEntityModel gpssmodel = WL_GpsOrderStatus_SQLDAL.GetGPSOrderStatus(waybillTruckEntityModel.WaybillNO.ToString());
                if (gpssmodel != null)
                {
                    gpssmodel.status = 5;
                    WL_GpsOrderStatus_SQLDAL.AddGpsOrderStatus(gpssmodel);
                }
                WL_WaybillTruck_SQLDAL.RemoveBillTruck(waybillTruckEntityModel);
                WL_Waybill_SQLDAL.UpdateWaybillStatus(waybillmodel);
                WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                WL_OperateLog_SQLDAL.Add(lmsOperatelog);
                scope.Complete();
            }
            return result.Succeed(strMsg);
        }

        protected override ResultModel Sync(Model.Log.BillChangeLogModel model)
        {
            ResultModel result = new ResultModel();
            ResultModel lmssqlResult = null;
            ResultModel lmsoracleResult = null;
            BillTruckModel billTruckModel = SC_BillTruck.GetBillTruckModelTmsSync2Lms(model.FormCode,true);
            if (billTruckModel == null)
            {
                return result.Failed("未取得TMS分拣BillTruckModel对象");
            }
            WaybillTruckEntityModel waybillTruckEntityModel = CreateLMSWaybillTruckEntityModel(billTruckModel);
            var lmsoperatelog = CreateLMSOperateLogEntityModel(model);

            if (IsOperateLMSSQL)
            {
                lmssqlResult = BillTruck2LmsSQL(model, lmsoperatelog, waybillTruckEntityModel);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = BillTruck2LmsOracle(model, lmsoperatelog, waybillTruckEntityModel);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }
            if (SC_BillTruck.UpdateSyncedStatus4Tms2Lms(billTruckModel.BTID) <= 0)
            {
                return result.Failed("更新TMS运单下车同步状态失败");
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }
    }
}
