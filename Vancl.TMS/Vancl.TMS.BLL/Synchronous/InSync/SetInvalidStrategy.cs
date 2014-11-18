using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    /// <summary>
    /// 置为无效策略
    /// </summary>
    public class SetInvalidStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            Enums.BillStatus originBillStatus = BillBLL.GetBillStatus(model.WaybillNo.ToString()).Value;
            return UpdateResult(BillBLL.UpdateBillStatus(model.WaybillNo.ToString(), model.Status)
         , string.Format(Consts.SYNC_LMS2TMS_TMS_STATUS_CHANGED_LOG
        , (int)originBillStatus
        , EnumHelper.GetDescription(originBillStatus)
        , (int)model.Status
        , EnumHelper.GetDescription(model.Status)));
        }
    }
}
