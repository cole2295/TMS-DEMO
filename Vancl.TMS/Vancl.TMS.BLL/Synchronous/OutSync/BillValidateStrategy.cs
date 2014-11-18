using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Synchronous.OutSync
{
    public class BillValidateStrategy : Tms2LmsStrategy
    {
        protected override Model.Common.ResultModel Sync(Model.Log.BillChangeLogModel model)
        {
        //    return ResultModel.Create(true);
            if (model == null) throw new ArgumentNullException("BillChangeLogModel is null.");

            var operateLog = base.CreateLMSOperateLogEntityModel(model);
            operateLog.LogType = 10; //logtype为10时不会同步到官方日志

            return this.BillValidateSync(model,operateLog);
        }

        private ResultModel BillValidateSync(Model.Log.BillChangeLogModel scchangelogmodel, Model.LMS.OperateLogEntityModel lmsOperatelog)
        {
            ResultModel result = new Model.Common.ResultModel();

            try
            {
                if (IsOperateLMSSQL)
                {
                    using (IACID scope = ACIDScopeFactory.GetLmsSqlACID())
                    {
                        //添加操作日志
                        WL_OperateLog_SQLDAL.Add(lmsOperatelog);
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
                using (IACID scope = ACIDScopeFactory.GetLmsOracleACID())
                {
                    //添加操作日志
                    WL_OperateLog_OracleDAL.Add(lmsOperatelog);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return result.Failed(String.Format("LMS物流主库【Oracle】版本异常：{0}", ex.Message + ex.StackTrace));
            }

            return result.Succeed("面单校验通过");
        }
    }
}
