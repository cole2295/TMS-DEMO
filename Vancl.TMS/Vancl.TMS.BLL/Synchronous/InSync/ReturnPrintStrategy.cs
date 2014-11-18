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
    /// 返货交接单打印策略
    /// </summary>
    public class ReturnPrintStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            Enums.ReturnStatus? originReturnStatus = BillBLL.GetBillReturnStatus(model.WaybillNo.ToString());
            if (originReturnStatus.HasValue)
            {
                if (originReturnStatus.Value == model.ReturnStatus)
                {
                    return InfoResult(string.Format(Consts.SYNC_LMS2TMS_RETURN_STATUS_SAME_ERROR, (int)originReturnStatus.Value, EnumHelper.GetDescription(originReturnStatus.Value)));
                }
            }
            return UpdateResult(BillBLL.UpdateBillReturnStatus(model.WaybillNo.ToString(), model.ReturnStatus)
        , string.Format(Consts.SYNC_LMS2TMS_TMS_STATUS_CHANGED_LOG
        , originReturnStatus.HasValue ? ((int)originReturnStatus.Value).ToString() : ""
        , originReturnStatus.HasValue ? EnumHelper.GetDescription(originReturnStatus.Value) : ""
        , (int)model.ReturnStatus
        , EnumHelper.GetDescription(model.ReturnStatus)));

        }
    }
}
