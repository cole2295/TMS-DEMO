using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    /// <summary>
    /// 运单装车策略
    /// </summary>
    public class BillLoadingStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            BillLoadingComparingModel comparingModel = BillBLL.GetBillForComparing<BillLoadingComparingModel>(model.WaybillNo.ToString());
            string currentDistributionCode = LmsDAL.GetCurrentDistributionCode(model.WaybillNo);
            if (string.IsNullOrWhiteSpace(currentDistributionCode))
            {
                throw new Exception("该运单在LMS中不存在");
            }
            BillModel billModel = new BillModel();
            billModel.FormCode = model.WaybillNo.ToString();
            billModel.CurrentDistributionCode = currentDistributionCode;
            billModel.Status = model.Status;
            return UpdateResult(BillBLL.Lms2Tms_BillLoadingUpdateBill(billModel)
                , string.Format(Consts.SYNC_LMS2TMS_TMS_STATUS_CHANGED_LOG
                    , (int)comparingModel.Status
                    , EnumHelper.GetDescription(comparingModel.Status)
                    , (int)model.Status
                    , EnumHelper.GetDescription(model.Status))
                + ","
                + string.Format(Consts.SYNC_LMS2TMS_TMS_CURRENTDISTRIBUTIONCODE_CHANGED_LOG
                    , comparingModel.CurrentDistributionCode, currentDistributionCode));
        }
    }
}
