using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    /// <summary>
    /// 面单打印策略
    /// </summary>
    public class BillPrintStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            Enums.BillStatus originBillStatus = BillBLL.GetBillStatus(model.WaybillNo.ToString()).Value;
            if (originBillStatus == model.Status)
            {
                return InfoResult(string.Format(Consts.SYNC_LMS2TMS_STATUS_SAME_ERROR, (int)originBillStatus, EnumHelper.GetDescription(originBillStatus)));
            }
            return UpdateResult(BillBLL.UpdateBillStatus(model.WaybillNo.ToString(), model.Status)
                , string.Format(Consts.SYNC_LMS2TMS_TMS_STATUS_CHANGED_LOG
                    , (int)originBillStatus
                    , EnumHelper.GetDescription(originBillStatus)
                    , (int)model.Status
                    , EnumHelper.GetDescription(model.Status)));
        }
    }
}
