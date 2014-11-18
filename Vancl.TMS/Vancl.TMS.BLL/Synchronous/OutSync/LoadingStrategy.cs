using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.IDAL.Sorting.Loading;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Model.Sorting.Loading;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public class LoadingStrategy : Tms2LmsStrategy
    {
        private IWaybillTruckDAL _wl_waybillTruck_sql = null;
        private IWaybillTruckDAL _wl_waybillTruck_oracle = null;
        private IGPSOrderDAL _wl_gpsOrder_sql = null;
        private IGPSOrderDAL _wl_gpsOrder_oracle = null;
        private IWaybillTakeSendInfoDAL _wl_takeSendInfo_sql = null;
        private IWaybillTakeSendInfoDAL _wl_takeSendInfo_oracle = null;
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

        public IGPSOrderDAL WL_GPSOrder_SQLDAL
        {
            get
            {
                if (_wl_gpsOrder_sql == null)
                {
                    _wl_gpsOrder_sql = ServiceFactory.GetService<IGPSOrderDAL>("GPSOrderDAL_SQL");
                }
                return _wl_gpsOrder_sql;
            }
        }

        public IGPSOrderDAL WL_GPSOrder_OracleDAL
        {
            get
            {
                if (_wl_gpsOrder_oracle == null)
                {
                    _wl_gpsOrder_oracle = ServiceFactory.GetService<IGPSOrderDAL>("GPSOrderDAL_Oracle");
                }
                return _wl_gpsOrder_oracle;
            }
        }

        public IWaybillTakeSendInfoDAL WL_TakeSendInfo_SQLDAL
        {
            get
            {
                if ( _wl_takeSendInfo_sql == null)
                {
                    _wl_takeSendInfo_sql = ServiceFactory.GetService<IWaybillTakeSendInfoDAL>("TakeSendInfoDAL_SQL");
                }
                return _wl_takeSendInfo_sql;
            }
        }

        public IWaybillTakeSendInfoDAL WL_TakeSendInfo_OracleDAL
        {
            get
            {
                if (_wl_takeSendInfo_oracle == null)
                {
                    _wl_takeSendInfo_oracle = ServiceFactory.GetService<IWaybillTakeSendInfoDAL>("TakeSendInfoDAL_Oracle");
                }
                return _wl_takeSendInfo_oracle;
            }
        }

        protected override Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Model.LMS.WaybillEntityModel waybillmodel, Model.Log.BillChangeLogModel scchangelogmodel)
        {
            var lmsChangelog = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            lmsChangelog.CurNode = Model.Common.Enums.StatusChangeNodeType.TransportCenter;
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.BillLoading;
            lmsChangelog.SubStatus = (int)scchangelogmodel.CurrentStatus;
            return lmsChangelog;
        }

        /// <summary>
        /// 创建运单GPS轨迹记录
        /// </summary>
        /// <param name="billTruckModel"></param>
        /// <returns></returns>
        protected GPSOrderEntityModel CreateLMSGPSOrderEntityModel(BillTruckModel billTruckModel)
        {
            return  new GPSOrderEntityModel()
            {
                //address = billTruckModel
                //city = billTruckModel
                createtime = billTruckModel.CreateTime,
                time = billTruckModel.CreateTime,
                gpsid = billTruckModel.GPSNO,
                IsSyn = false,
                orderno = billTruckModel.FormCode,
                //stations = 
                truckno = billTruckModel.TruckNO,
                //warehouse = 
            };
        }

        /// <summary>
        /// 创建运单装车实体对象
        /// </summary>
        /// <param name="billTruckModel"></param>
        /// <returns></returns>
        protected WaybillTruckEntityModel CreateLMSWaybillTruckEntityModel(BillTruckModel billTruckModel)
        {
            return new WaybillTruckEntityModel()
            {
                BatchNO = long.Parse(billTruckModel.BatchNO),
                CreateBy = billTruckModel.CreateBy,
                CreateTime = billTruckModel.CreateTime,
                DriverId = billTruckModel.DriverID,
                GpsID = billTruckModel.GPSNO,
                IsDelete = billTruckModel.IsDeleted ,
                TruckNO = billTruckModel.TruckNO,
                UpdateBy = billTruckModel.UpdateBy,
                UpdateTime = billTruckModel.UpdateTime,
                WaybillNO = long.Parse(billTruckModel.FormCode)
            };
        }

        private ResultModel BillTruck2LmsOracle(Model.Log.BillChangeLogModel scchangelogmodel, OperateLogEntityModel lmsOperatelog, GPSOrderEntityModel gpsOrderEntityModel, WaybillTruckEntityModel waybillTruckEntityModel)
        {
            ResultModel result = new ResultModel();
            WaybillEntityModel waybillmodel = WL_Waybill_OracleDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillEntityModel对象，未全部切换暂时认为正确");
            }
            if (waybillmodel.Status != scchangelogmodel.PreStatus)
            {
                return result.Failed("LMS物流主库【Oracle】运单状态同装车前置状态不相同，可能已在LMS系统操作装车及后续操作");
            }
            if (WL_WaybillTruck_OracleDAL.IsWaybillLoading(waybillTruckEntityModel.WaybillNO, waybillTruckEntityModel.BatchNO.ToString()))
            {
                return result.Failed("LMS物流主库【Oracle】存在装车记录，可能已在LMS系统操作装车及后续操作");
            }
            String strMsg = String.Format("LMS物流[Oracle]主库,状态由{0}变更为{1},当前配送商由{2}变更为{3}"
, (int)waybillmodel.Status
, (int)scchangelogmodel.CurrentStatus
, waybillmodel.CurrentDistributionCode
, scchangelogmodel.CurrentDistributionCode
);
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            TakeSend_DeliverStationEntityModel takeSendModel = WL_TakeSendInfo_OracleDAL.GetTakeSendAndDeliverStationInfo(long.Parse(scchangelogmodel.FormCode));
            if (takeSendModel != null)
            {
                gpsOrderEntityModel.address = takeSendModel.ReceiveAddress;
                gpsOrderEntityModel.city = takeSendModel.DeliverStationCityName;
                gpsOrderEntityModel.warehouse = takeSendModel.DeliverStationParentID;
                gpsOrderEntityModel.stations = takeSendModel.DeliverStationID;
            }
            using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
            {
                WL_Waybill_OracleDAL.UpdateWaybillModelByLoading(waybillmodel);
                WL_WaybillTruck_OracleDAL.Add(waybillTruckEntityModel);
                if (!string.IsNullOrEmpty(waybillTruckEntityModel.GpsID))
                {
                    if (!WL_GPSOrder_OracleDAL.IsExist(waybillTruckEntityModel.WaybillNO.ToString())) //判断运单轨迹是否存在
                    {
                        WL_GPSOrder_OracleDAL.Add(gpsOrderEntityModel);
                    }
                }
                WL_WaybillStatusChangeLog_OracleDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                WL_OperateLog_OracleDAL.Add(lmsOperatelog);
                scope.Complete();
            }
            return result.Succeed(strMsg);
        }

        private ResultModel BillTruck2LmsSQL(Model.Log.BillChangeLogModel scchangelogmodel, OperateLogEntityModel lmsOperatelog, GPSOrderEntityModel gpsOrderEntityModel, WaybillTruckEntityModel waybillTruckEntityModel)
        {
            ResultModel result = new ResultModel();
            WaybillEntityModel waybillmodel = WL_Waybill_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillmodel == null)
            {
                return result.Failed("未取得LMS物流主库【SQL】WaybillEntityModel对象");
            }
            if (waybillmodel.Status != scchangelogmodel.PreStatus)
            {
                return result.Failed("LMS物流主库【SQL】运单状态同装车前置状态不相同，可能已在LMS系统操作装车及后续操作");
            }
            if (WL_WaybillTruck_SQLDAL.IsWaybillLoading(waybillTruckEntityModel.WaybillNO, waybillTruckEntityModel.BatchNO.ToString()))
            {
                return result.Failed("LMS物流主库【SQL】存在装车记录，可能已在LMS系统操作装车及后续操作");
            }
            String strMsg = String.Format("LMS物流[SQL]主库,状态由{0}变更为{1},当前配送商由{2}变更为{3}"
    , (int)waybillmodel.Status
    , (int)scchangelogmodel.CurrentStatus
    , waybillmodel.CurrentDistributionCode
    , scchangelogmodel.CurrentDistributionCode
    );
            waybillmodel.Status = scchangelogmodel.CurrentStatus;
            waybillmodel.CurrentDistributionCode = scchangelogmodel.CurrentDistributionCode;
            waybillmodel.UpdateBy = scchangelogmodel.CreateBy;
            waybillmodel.UpdateDept = scchangelogmodel.CreateDept;
            waybillmodel.UpdateTime = scchangelogmodel.CreateTime;
            TakeSend_DeliverStationEntityModel takeSendModel = WL_TakeSendInfo_SQLDAL.GetTakeSendAndDeliverStationInfo(long.Parse(scchangelogmodel.FormCode));
            if (takeSendModel != null)
            {
                gpsOrderEntityModel.address = takeSendModel.ReceiveAddress;
                gpsOrderEntityModel.city = takeSendModel.DeliverStationCityName;
                gpsOrderEntityModel.warehouse = takeSendModel.DeliverStationParentID;
                gpsOrderEntityModel.stations = takeSendModel.DeliverStationID;
            }
            using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
            {
                WL_Waybill_SQLDAL.UpdateWaybillModelByLoading(waybillmodel);
                WL_WaybillTruck_SQLDAL.Add(waybillTruckEntityModel);
                if (!String.IsNullOrEmpty(waybillTruckEntityModel.GpsID))
                {
                    if (!WL_GPSOrder_SQLDAL.IsExist(waybillTruckEntityModel.WaybillNO.ToString()))  //判断运单轨迹是否存在
                    {
                        WL_GPSOrder_SQLDAL.Add(gpsOrderEntityModel);
                    }
                }
                WL_WaybillStatusChangeLog_SQLDAL.Add(CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel));
                WL_OperateLog_SQLDAL.Add(lmsOperatelog);
                scope.Complete();
            }
            return result.Succeed(strMsg);
        }

        protected override Model.Common.ResultModel Sync(Model.Log.BillChangeLogModel model)
        {
            ResultModel result = new ResultModel();
            ResultModel lmssqlResult = null;
            ResultModel lmsoracleResult = null;
            BillTruckModel billTruckModel = SC_BillTruck.GetBillTruckModelTmsSync2Lms(model.FormCode,false);
            if (billTruckModel == null)
            {
                return result.Failed("未取得TMS分拣BillTruckModel对象");
            }
            GPSOrderEntityModel gpsOrderEntityModel = CreateLMSGPSOrderEntityModel(billTruckModel);
            WaybillTruckEntityModel waybillTruckEntityModel = CreateLMSWaybillTruckEntityModel(billTruckModel);
            var lmsoperatelog = CreateLMSOperateLogEntityModel(model);
            if (IsOperateLMSSQL)
            {
                lmssqlResult = BillTruck2LmsSQL(model, lmsoperatelog, gpsOrderEntityModel, waybillTruckEntityModel);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = BillTruck2LmsOracle(model, lmsoperatelog, gpsOrderEntityModel, waybillTruckEntityModel);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }
            if (SC_BillTruck.UpdateSyncedStatus4Tms2Lms(billTruckModel.BTID) <= 0)
            {
                return result.Failed("更新TMS运单装车同步状态失败");
            } 
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }
    }
}
