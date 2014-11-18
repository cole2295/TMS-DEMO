using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    /// <summary>
    /// 订单同步服务策略
    /// 空实现，已在外部实现
    /// </summary>
    public class BillSyncServiceStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            return InfoResult(string.Format(Consts.SYNC_LMS2TMS_TMS_INSERT_LOG
                , (int)model.Status
                , EnumHelper.GetDescription(model.Status)));
        }
    }
}
