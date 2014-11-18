using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    /// <summary>
    /// 分配站点策略
    /// </summary>
    public class AssignStationStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            AssignStationComparingModel comparingModel = BillBLL.GetBillForComparing<AssignStationComparingModel>(model.WaybillNo.ToString());
            BillModel billModel = new BillModel();
            billModel.FormCode = model.WaybillNo.ToString();
            billModel.DeliverStationID = model.DeliverStationID;
            billModel.Status = model.Status;
            return UpdateResult(BillBLL.Lms2Tms_AssignStationUpdateBill(billModel)
                , string.Format(Consts.SYNC_LMS2TMS_TMS_STATUS_CHANGED_LOG
                    , (int)comparingModel.Status
                    , EnumHelper.GetDescription(comparingModel.Status)
                    , (int)model.Status
                    , EnumHelper.GetDescription(model.Status))
                + ","
                + string.Format(Consts.SYNC_LMS2TMS_TMS_DELIVERSTATION_CHANGED_LOG
                    , comparingModel.DeliverStationID, model.DeliverStationID));
        }
    }
}
