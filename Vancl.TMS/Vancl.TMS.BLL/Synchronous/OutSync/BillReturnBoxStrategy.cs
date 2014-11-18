using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public class BillReturnBoxStrategy : Tms2LmsStrategy
    {
        #region 私有属性

        private Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL _wl_WaybillReturnBoxInfo_sql = null;
        private Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL _wl_WaybillReturnBoxInfo_oracle = null;
        private Vancl.TMS.IDAL.Sorting.Return.IBillReturnBoxInfoDAL _sc_BillReturnBoxInfo = null;

        #endregion

        #region 数据层
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
        /// LMS物流主库WaybillReturnBoxInfo【Oracle】数据层
        /// </summary>
        public Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL WL_WaybillReturnBoxInfo_OracleDAL
        {
            get
            {
                if (_wl_WaybillReturnBoxInfo_oracle == null)
                {
                    _wl_WaybillReturnBoxInfo_oracle = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL>("LMSWayBillReturnBoxDAL_Oracle");
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
                    _wl_WaybillReturnBoxInfo_sql = ServiceFactory.GetService<Vancl.TMS.IDAL.LMS.IWaybillReturnBoxInfoDAL>("LMSWayBillReturnBoxDAL_SQL");
                }
                return _wl_WaybillReturnBoxInfo_sql;
            }
        }
        #endregion

        #region 构建实体对象
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
                CreateBy = scBillReturnBoxInfoModel.CreateBy,
                IsPrintBackForm = scBillReturnBoxInfoModel.IsPrintBackForm,
                IsPrintBackPacking = scBillReturnBoxInfoModel.IsPrintBackPacking,
                ReturnMerchant = scBillReturnBoxInfoModel.ReturnMerchant,
                CreateTime = scBillReturnBoxInfoModel.CreateTime,
                BoxNo = scBillReturnBoxInfoModel.BoxNo,
                CreateDep = scBillReturnBoxInfoModel.CreateDept,
                ReturnTo = scBillReturnBoxInfoModel.ReturnTo,
                Weight = scBillReturnBoxInfoModel.Weight
            };
        }
        #endregion

        #region 退货分拣称重箱号同步
        /// <summary>
        /// 退货分拣称重箱号明细同步到Oracle
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <param name="scOutboundEntityModel"></param>
        /// <returns></returns>
        private ResultModel BillReturnBox2LmsOracle( Vancl.TMS.Model.Sorting.Return.BillReturnBoxInfoModel scBillReturnBoxEntityModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    long BillReturnBoxlID = WL_WaybillReturnBoxInfo_OracleDAL.Add(CreateLMSWaybillReturnBoxEntityModel(scBillReturnBoxEntityModel));
                    if (BillReturnBoxlID <= 0)
                    {
                        return result.Failed("LMS物流主库【Oracle】版本，新增WaybillReturnBoxInfo记录失败");
                    }
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
        private ResultModel BillReturnBox2LmsSQL(Vancl.TMS.Model.Sorting.Return.BillReturnBoxInfoModel scBillReturnBoxEntityModel)
        {
            ResultModel result = new Model.Common.ResultModel();
            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    long BillReturnBoxID = WL_WaybillReturnBoxInfo_SQLDAL.Add(CreateLMSWaybillReturnBoxEntityModel(scBillReturnBoxEntityModel));
                    if (BillReturnBoxID <= 0)
                    {
                        return result.Failed("LMS物流主库【Sql】版本，新增WaybillReturnBoxInfo记录失败");
                    }
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

        #region 同步方法
        protected override ResultModel Sync(BillChangeLogModel model)
        {
            ResultModel result = new ResultModel();
            ResultModel lmssqlResult = null;
            ResultModel lmsoracleResult = null;
            Vancl.TMS.Model.Sorting.Return.BillReturnBoxInfoModel SC_BillReturnBoxEntityModel = SC_BillReturnBoxInfo.GetBillReturnBoxIntoModel4TmsSync2Lms(model.FormCode);
            if (SC_BillReturnBoxEntityModel == null)
            {
                return result.Failed("未取得TMS退货分拣称重BillReturnBoxInfo对象");
            }
            if (IsOperateLMSSQL)
            {
                lmssqlResult = BillReturnBox2LmsSQL(SC_BillReturnBoxEntityModel);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = BillReturnBox2LmsOracle(SC_BillReturnBoxEntityModel);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }

            if (SC_BillReturnBoxInfo.UpdateSyncedStatus4Tms2Lms(SC_BillReturnBoxEntityModel.RBoxID) <= 0)
            {
                return result.Failed("更新TMS退货分拣称重单号表同步状态失败");
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));
        }
        #endregion

    }
}
