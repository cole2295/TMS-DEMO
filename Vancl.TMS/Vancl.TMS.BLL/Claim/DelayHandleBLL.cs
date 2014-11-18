using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IDAL.Claim;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Claim;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Claim
{
    public class DelayHandleBLL : BaseBLL, IDelayHandleBLL
    {
        IDelayHandleDAL _delayHandleDAL = ServiceFactory.GetService<IDelayHandleDAL>();
        IDispatchBLL _dispatchBLL = ServiceFactory.GetService<IDispatchBLL>("DispatchBLL");
        IDelayDAL _delayDAL = ServiceFactory.GetService<IDelayDAL>("DelayDAL");

        public PagedList<ViewDelayHandleModel> Search(DelayHandleSearchModel searchModel)
        {
            return _delayHandleDAL.Search(searchModel);
        }

        public PagedList<ViewDelayHandleModel> SearchDelayHandleApply(DelayHandleSearchModel searchModel)
        {
            return _delayHandleDAL.SearchDelayHandleApply(searchModel);
        }

        public ResultModel Add(DelayHandleModel model)
        {
            if (model == null)
            {
                throw new CodeNotValidException();
            }
            DelayHandleModel pastModel = _delayHandleDAL.GetDelayHandle(model.DelayID, Enums.ApproveStatus.NotApprove);
            int i = 0;
            if (pastModel == null)
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    WriteInsertLog<DelayHandleModel>(model);
                    i = _delayHandleDAL.Add(model);
                    scope.Complete();
                }
                return AddResult(i, "延误复议");
            }
            else
            {
                model.DHID = pastModel.DHID;
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    WriteUpdateLog<DelayHandleModel>(model, pastModel);
                    i = _delayHandleDAL.UpdateDelayHandleApplyReason(model);
                    scope.Complete();
                }
                return UpdateResult(i, "延误复议");
            }
        }

        public ResultModel Approve(DelayHandleModel model)
        {
            if (model == null)
            {
                throw new CodeNotValidException();
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                WriteInsertLog<DelayHandleModel>(model);
                i = _delayHandleDAL.Approve(model);
                if (model.ApproveStatus == Enums.ApproveStatus.Approved)
                {
                    string deliveryNo = _delayDAL.GetDeliveryNo(model.DelayID);
                    if (string.IsNullOrEmpty(deliveryNo))
                    {
                        return ErrorResult("延误信息不存在或者已经被删除！");
                    }
                    //延误复议被审核通过，更新提货单状态为准时到货
                    _dispatchBLL.UpdateDeliveryStatus(deliveryNo, Enums.DeliveryStatus.ArrivedOnTime);
                    _dispatchBLL.UpdateDeliveryToNoDelay(deliveryNo);
                }
                scope.Complete();
            }
            return i > 0 ? InfoResult("延误复议审核成功！") : ErrorResult("延误复议审核失败！");
        }
    }
}
