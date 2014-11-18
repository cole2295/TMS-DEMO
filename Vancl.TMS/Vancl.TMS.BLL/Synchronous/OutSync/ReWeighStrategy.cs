using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    /// <summary>
    /// 重新称重
    /// </summary>
    public class ReWeighStrategy : Tms2LmsStrategy
    {
        protected override Model.Common.ResultModel Sync(Model.Log.BillChangeLogModel model)
        {
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null.");

            return this.BillWeightSync(model);
        }


        /// <summary>
        /// 面单软重新称重同步到LMS物流主库
        /// </summary>
        /// <param name="scchangelogmodel"></param>
        /// <returns></returns>
        private ResultModel BillWeightSync(
                        BillChangeLogModel scchangelogmodel)
        {
            WeighPrintSync weighPrintSync = new WeighPrintSync();
            ResultModel result = new Model.Common.ResultModel();
            decimal weight = weighPrintSync.GetBillWeight(scchangelogmodel.FormCode);

            try
            {
                if (IsOperateLMSSQL)
                {
                    using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                    {
                        //4.更新BillInfo Weight
                        result = weighPrintSync.SyncBillWeight(SyncDbType.Sqlserver, scchangelogmodel.FormCode, weight);
                        //     if (!result.IsSuccess) return result.Failed(result.Message);
                        //5.同步BillWeigh表
                        result = weighPrintSync.SyncBillPackageInfo(SyncDbType.Sqlserver, scchangelogmodel.FormCode);
                        //     if (!result.IsSuccess) return result.Failed(result.Message);
                        scope.Complete();
                    }
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【SQL】版本异常：{0}", ex.Message + ex.StackTrace));
            }

            try
            {
                //Oracle           
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    //4.更新BillInfo Weight
                    result = weighPrintSync.SyncBillWeight(SyncDbType.Oracle, scchangelogmodel.FormCode, weight);
                    //  if (!result.IsSuccess) return result.Failed(result.Message);
                    //5.同步BillWeigh表
                    result = weighPrintSync.SyncBillPackageInfo(SyncDbType.Oracle, scchangelogmodel.FormCode);

                    //更改同步状态
                    weighPrintSync.UpdateWeighSyncStatus(scchangelogmodel.FormCode, Enums.SyncStatus.NotYet, Enums.SyncStatus.Already);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【Oracle】版本异常：{0}", ex.Message+ex.StackTrace));
            }

            return result.Succeed("成功同步重量信息");
        }
    }
}
