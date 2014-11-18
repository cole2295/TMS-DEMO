using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    public class TurnStationBaseStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            TurnStationComparingModel comparingModel = BillBLL.GetBillForComparing<TurnStationComparingModel>(model.WaybillNo.ToString());
            WaybillEntityModel turnStationModel = LmsDAL.GetWaybillModel4Lms2Tms_TurnStation(model.WaybillNo);
            if (turnStationModel == null)
            {

                throw new ArgumentNullException("LMS物流主库turnStationModel对象是null或TranStationID不一致,单号：" + model.WaybillNo);
            }
            if (!turnStationModel.TurnStationID.HasValue)
            {
                return ErrorResult("未取得转站相关主键Key，请确实数据是否有误");
            }
            if (!turnStationModel.Isfast.HasValue
                || turnStationModel.Isfast.Value)
            {
                return InfoResult("快捷转站运单，默认不同步数据");
            }
            BillModel billModel = new BillModel();
            billModel.FormCode = model.WaybillNo.ToString();
            billModel.Status = model.Status;
            billModel.DeliverStationID = model.DeliverStationID;
            billModel.TurnstationKey = turnStationModel.TurnStationID.Value.ToString();
            return UpdateResult(BillBLL.Lms2Tms_TurnStationApplyUpdateBill(billModel)
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
