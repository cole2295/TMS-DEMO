using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// 入库同步回LMS基策略
    /// </summary>
    public abstract class InboundBaseStrategy : Tms2LmsStrategy
    {
        private Vancl.TMS.IDAL.LMS.IInboundDAL _wl_inbound_sql = null;
        private Vancl.TMS.IDAL.LMS.IInboundDAL _wl_inbound_oracle = null;
        private Vancl.TMS.IDAL.LMS.IWaybill_SortCenterDAL _wl_WaybillSortCenter_oracle = null;
        private Vancl.TMS.IDAL.LMS.IWaybill_SortCenterDAL _wl_WaybillSortCenter_sql = null;
        private Vancl.TMS.IDAL.Sorting.Inbound.IInboundDAL _sc_inbound = null;

        /// <summary>
        /// TMS 分拣入库数据层接口
        /// </summary>
        public Vancl.TMS.IDAL.Sorting.Inbound.IInboundDAL SC_Inbound
        {
            get
            {
                if (_sc_inbound == null)
                {
                    _sc_inbound = ServiceFactory.GetService<Vancl.TMS.IDAL.Sorting.Inbound.IInboundDAL>("InboundDAL");
                }
                return _sc_inbound;
            }
        }

        /// <summary>
        /// 物流主库Inbound SQL数据层实现
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IInboundDAL WL_Inbound_SQLDAL
        {
            get
            {
                if (_wl_inbound_sql == null)
                {
                    _wl_inbound_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IInboundDAL>("LMSInboundDAL_SQL");
                }
                return _wl_inbound_sql;
            }
        }

        /// <summary>
        /// 物流主库Inbound Oracle数据层实现
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IInboundDAL WL_Inbound_OracleDAL
        {
            get
            {
                if (_wl_inbound_oracle == null)
                {
                    _wl_inbound_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IInboundDAL>("LMSInboundDAL_Oracle");
                }
                return _wl_inbound_oracle;
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

        /// <summary>
        /// 创建LMS物流主库WaybillStatusChangeLog实体对象
        /// </summary>
        /// <param name="waybillmodel">LMS物流主库运单对象</param>
        /// <param name="scchangelogmodel">TMS分拣billchanglog对象</param>
        /// <returns></returns>
        protected override Vancl.TMS.Model.LMS.WaybillStatusChangeLogEntityModel CreateLMSWaybillStatusChangeLogEntityModel(Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel, BillChangeLogModel scchangelogmodel)
        {
            var lmsChangelog = base.CreateLMSWaybillStatusChangeLogEntityModel(waybillmodel, scchangelogmodel);
            lmsChangelog.CurNode = Model.Common.Enums.StatusChangeNodeType.DeliverCenter;
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.Inbound;
            lmsChangelog.SubStatus = (int)scchangelogmodel.CurrentStatus;
            return lmsChangelog;
        }


        /// <summary>
        /// 创建LMS物流主库Waybill_SortCenter实体对象
        /// </summary>
        /// <param name="scInboundModel"></param>
        /// <returns></returns>
        protected virtual Vancl.TMS.Model.LMS.Waybill_SortCenterEntityModel CreateLMSWaybillSortCenterEntityModel(Vancl.TMS.Model.Sorting.Inbound.InboundEntityModel scInboundModel)
        {
            return new Model.LMS.Waybill_SortCenterEntityModel()
            {
                CreateBy = scInboundModel.CreateBy,
                CreateTime = scInboundModel.CreateTime,
                InboundGuid = null,
                InBoundKid = scInboundModel.IBID,
                IsDeleted = false,
                OutboundGuid = null,
                OutBoundKid = "",
                UpdateBy = scInboundModel.CreateBy,
                UpdateTime = scInboundModel.CreateTime,
                WaybillNO = long.Parse(scInboundModel.FormCode)
            };
        }


        /// <summary>
        /// 创建LMS物流主库Inbound实体对象
        /// </summary>
        /// <param name="waybillmodel">LMS物流主库waybill对象</param>
        /// <param name="scInboundModel">TMS分拣Inbound对象</param>
        /// <returns></returns>
        protected virtual Vancl.TMS.Model.LMS.InboundEntityModel CreateLMSInboundEntityModel(Vancl.TMS.Model.LMS.WaybillEntityModel waybillmodel, Vancl.TMS.Model.Sorting.Inbound.InboundEntityModel scInboundModel, BillChangeLogModel scchangelogmodel)
        {
            return new Model.LMS.InboundEntityModel()
            {
                CreateBy = scInboundModel.CreateBy,
                CreateDept = scchangelogmodel.CreateDept,
                CreateTime = scInboundModel.CreateTime,
                CurOperator = scInboundModel.CreateBy,
                CustomerBatchNO = waybillmodel.CustomerBatchNO,
                DeliveryCarNO = null,
                DeliveryDriver = null,
                DeliveryMan = waybillmodel.DeliveryMan,
                DeliveryTime = waybillmodel.DeliveryTime,
                FromStation = scInboundModel.ApplyStation,
                InBoundGuid = null,
                InBoundKid = scInboundModel.IBID,
                InboundNO = null,
                IntoStation = scInboundModel.DepartureID,
                IntoStationType = scInboundModel.InboundType,
                IntoTime = scInboundModel.CreateTime,
                IsDeleted = false,
                IsPrint = false,
                PrintTime = null,
                ToStation = scInboundModel.ArrivalID,
                UpdateBy = scInboundModel.CreateBy,
                UpdateDept = scchangelogmodel.CreateDept,
                UpdateTime = scInboundModel.CreateTime,
                WaybillNO = long.Parse(scInboundModel.FormCode)
            };
        }

    }
}
