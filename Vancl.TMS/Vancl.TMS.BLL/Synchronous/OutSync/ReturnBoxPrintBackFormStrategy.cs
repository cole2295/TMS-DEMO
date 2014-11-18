using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public class ReturnBoxPrintBackFormStrategy:BillReturnBoxPrintStrategy 
    {
        #region 物流主库[SQL]和[Oracle]共用的实体对象
        /// <summary>
        /// LMS物流主库OperateLog实体对象
        /// </summary>
        private Vancl.TMS.Model.LMS.WaybillStatusChangeLogEntityModel lmsChangelog = null;
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
            Vancl.TMS.Model.LMS.WaybillReturnBoxInfoEntityModel waybillBoxmodel = WL_WaybillReturnBoxInfo_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillBoxmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillReturnBoxInfoEntityModel对象");
            }
            waybillBoxmodel.IsPrintBackForm = true;

            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    WL_WaybillReturnBoxInfo_OracleDAL.PrintBackForm(waybillBoxmodel);
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
            Vancl.TMS.Model.LMS.WaybillReturnBoxInfoEntityModel waybillBoxmodel = WL_WaybillReturnBoxInfo_SQLDAL.GetModel(long.Parse(scchangelogmodel.FormCode));
            if (waybillBoxmodel == null)
            {
                return result.Succeed("未取得LMS物流主库【Oracle】WaybillReturnBoxInfoEntityModel对象");
            }
            waybillBoxmodel.IsPrintBackForm = true;

            try
            {
                using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                {
                    WL_WaybillReturnBoxInfo_SQLDAL.PrintBackForm(waybillBoxmodel);
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
                result.Failed("未取得TMS退货分拣称重BillReturnDetailModel对象");
            }
            if (IsOperateLMSSQL)
            {
                lmssqlResult = BillReturnBox2LmsSQL(model, SC_BillReturnBoxEntityModel);
                if (!lmssqlResult.IsSuccess)
                {
                    return result.Failed(lmssqlResult.Message);
                }
            }
            lmsoracleResult = BillReturnBox2LmsOracle(model, SC_BillReturnBoxEntityModel);
            if (!lmsoracleResult.IsSuccess)
            {
                return result.Failed(lmsoracleResult.Message);
            }

            if (SC_BillReturnBoxInfo.UpdateSyncedStatus4Tms2Lms(SC_BillReturnBoxEntityModel.RBoxID) <= 0)
            {
                return result.Failed("更新TMS退货分拣称重箱号表同步状态失败");
            }
            return result.Succeed(String.Format("同步日志:{0},{1}", IsOperateLMSSQL ? lmssqlResult.Message : "SQL 版本停用", lmsoracleResult.Message));

        }
        #endregion

    }
}
