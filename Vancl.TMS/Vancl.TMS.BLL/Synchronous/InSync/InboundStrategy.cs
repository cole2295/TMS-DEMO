using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.BLL.Synchronous.InSync
{
    /// <summary>
    /// 入库策略
    /// </summary>
    public class InboundStrategy : Lms2TmsStrategy
    {
        protected override ResultModel Sync(LmsWaybillStatusChangeLogModel model)
        {
            WaybillEntityModel waybillmodel = LmsDAL.GetWaybillModel4Lms2Tms_Inbound(model.WaybillNo);
            if (waybillmodel == null)
            {
                throw new ArgumentNullException("LMS物流主库waybill对象是null");
            }
            Enums.BillStatus originBillStatus = BillBLL.GetBillStatus(model.WaybillNo.ToString()).Value;
            BillModel billmodel = new BillModel()
            {
                FormCode = waybillmodel.WaybillNo.ToString(),
                InboundKey = waybillmodel.InboundID.HasValue ? waybillmodel.InboundID.Value.ToString() : "",
                OutboundKey = null,
                Status = model.Status
            };
            return UpdateResult(
                BillBLL.Lms2Tms_InboundUpdateBill(billmodel)
                , string.Format(Consts.SYNC_LMS2TMS_INBOUND_CHANGE_LOG
                    , (int)originBillStatus
                    , EnumHelper.GetDescription(originBillStatus)
                    , (int)model.Status
                    , EnumHelper.GetDescription(model.Status)
                    , billmodel.InboundKey)
                   );
        }
    }
}
