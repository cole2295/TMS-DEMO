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
    /// <summary>
    /// 返货入库单号明细
    /// </summary>
    public abstract class BillReturnDetailStrategy : Tms2LmsStrategy
    {
        #region 私有属性

        private Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL _wl_WaybillReturnDetailInfo_sql = null;
        private Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL _wl_WaybillReturnDetailInfo_oracle = null;
        private Vancl.TMS.IDAL.Sorting.Return.IBillReturnDetailInfoDAL _sc_BillReturnDetailInfo = null;

        #endregion

        #region 数据层
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
        /// LMS物流主库WaybillDetailInfo【SQL】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL WL_WaybillReturnDetailInfo_SQLDAL
        {
            get
            {
                if (_wl_WaybillReturnDetailInfo_sql == null)
                {
                    _wl_WaybillReturnDetailInfo_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL>("LMSWayBillReturnDetailDAL_SQL");
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
                    _wl_WaybillReturnDetailInfo_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnDetailInfoDAL>("LMSWayBillReturnDetailDAL_Oracle");
                }
                return _wl_WaybillReturnDetailInfo_oracle;
            }
        }

        #endregion

        #region 构建实体对象
        #region 物流主库[SQL]和[Oracle]共用的实体对象

        /// <summary>
        /// LMS物流主库OperateLog实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.OperateLogEntityModel LmsOperateLogEntityModel = null;
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
            lmsChangelog.OperateType = Enums.Lms2TmsOperateType.ReturnInbound;
            lmsChangelog.SubStatus = (int)scchangelogmodel.ReturnStatus;
            lmsChangelog.Status = waybillmodel.Status;
            return lmsChangelog;
        }

        #endregion
        #region 创建物流主库单号明细对象
        /// <summary>
        /// 创建LMS物流主库退货分拣称重明细单号实体对象
        /// </summary>
        /// <param name="waybillmodel"></param>
        /// <param name="scBillReturnDetailInfoModel"></param>
        /// <param name="scchangelogmodel"></param>
        /// <returns></returns>
        protected virtual Vancl.TMS.Model.LMS.WaybillReturnDetailInfoEntityModel CreateLMSWaybillReturnDetailEntityModel(Vancl.TMS.Model.Sorting.Return.BillReturnDetailInfoModel scBillReturnDetailInfoModel)
        {
            return new Model.LMS.WaybillReturnDetailInfoEntityModel()
            {
                WaybillNo=long.Parse(scBillReturnDetailInfoModel.FormCode),
                BoxNo = scBillReturnDetailInfoModel.BoxNo,
                CreateDep = scBillReturnDetailInfoModel.CreateDept,
                ReturnTo = scBillReturnDetailInfoModel.ReturnTo,
                Weight = scBillReturnDetailInfoModel.Weight,
                OrderNo = scBillReturnDetailInfoModel.CustomerOrder,
                CreateBy=scBillReturnDetailInfoModel.CreateBy,
                CreateTime=scBillReturnDetailInfoModel.CreateTime
            };
        }

        #endregion
        #endregion

    }
}
