using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    /// <summary>
    /// 出库策略
    /// </summary>
    public class OutboundStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            WaybillEntityModel waybillmodel  = LmsDAL.GetWaybillModel4Lms2Tms_Outbound(model.WaybillNo);
            if (waybillmodel == null)
            {
                throw new ArgumentNullException("LMS物流主库waybill对象是null");
            }
            OutboundComparingModel comparingModel = BillBLL.GetBillForComparing<OutboundComparingModel>(model.WaybillNo.ToString());
            BillModel billModel = new BillModel();
            billModel.FormCode = model.WaybillNo.ToString();           
            billModel.Status = model.Status;
            billModel.CurrentDistributionCode = waybillmodel.CurrentDistributionCode;
            billModel.OutboundKey = waybillmodel.OutboundID.HasValue ? waybillmodel.OutboundID.Value.ToString() : " ";
            return UpdateResult(BillBLL.Lms2Tms_OutboundUpdateBill(billModel)
                , string.Format(Consts.SYNC_LMS2TMS_OUTBOUND_CHANGE_LOG
                    , (int)comparingModel.Status
                    , EnumHelper.GetDescription(comparingModel.Status)
                    , (int)model.Status
                    , EnumHelper.GetDescription(model.Status)
                    , billModel.OutboundKey 
                    , comparingModel.CurrentDistributionCode
                    , waybillmodel.CurrentDistributionCode             
                    )
                    );
        }
    }
}
