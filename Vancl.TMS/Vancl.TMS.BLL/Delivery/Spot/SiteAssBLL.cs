using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Delivery.Spot;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Delivery.Spot;
using Vancl.TMS.IBLL.Delivery.Spot;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.Transport.Dispatch;

namespace Vancl.TMS.BLL.Delivery.Spot
{
    public class SiteAssBLL : BaseBLL, ISiteAssBLL
    {
        ISiteAssDAL _siteAssDAL = ServiceFactory.GetService<ISiteAssDAL>("SiteAssDAL");
        IDispatchBLL _dispatchBLL = ServiceFactory.GetService<IDispatchBLL>("DispatchBLL");
        IDispatchDAL _dispatchDAL = ServiceFactory.GetService<IDispatchDAL>("DispatchDAL");
        #region ISiteAssBLL 成员

        /// <summary>
        /// 新增发货现场数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Add(SiteAssessmentModel model)
        {
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _siteAssDAL.Add(model);
                //更新调度状态
                _dispatchBLL.UpdateDeliveryStatus(model.DeliveryNO, Enums.DeliveryStatus.InTransit);
                //更新预计到货时间
                if (_dispatchDAL.UpdateDispatchExpectArrivalDate(model.DeliveryNO, model.LeaveTime) <= 0)
                {
                    return ErrorResult("该提货单信息不存在或者已经被删除！");
                }
                WriteInsertLog<SiteAssessmentModel>(model);
                scope.Complete();
            }
            return AddResult(i, "现场考核信息");
        }

        /// <summary>
        /// 批量新增发货现场数据
        /// </summary>
        /// <param name="lstModel"></param>
        /// <returns></returns>
        public ResultModel AddBatch(SiteAssessmentBatchModel model)
        {
            if (model == null || model.ListDeliveryNo == null || model.ListDeliveryNo.Count == 0)
            {
                return ErrorResult("请选择");
            }
            SiteAssessmentModel sam = new SiteAssessmentModel();
            sam.ArrivalTime = model.ArrivalTime;
            sam.AssessmentStatus = model.AssessmentStatus;
            sam.LeaveTime = model.LeaveTime;
            sam.Reason = model.Reason;
            ResultModel r = null;
            int failCount = 0;
            int successCount = 0;
            foreach (string deliveryNo in model.ListDeliveryNo)
            {
                sam.DeliveryNO = deliveryNo;
                r = Add(sam);
                if (r.IsSuccess)
                {
                    successCount++;
                }
                else
                {
                    failCount++;
                }
            }
            return InfoResult(string.Format("录入现场考核成功{0}条，失败{1}条！", successCount, failCount));
        }

        /// <summary>
        /// 查询状态为“已调度”的调度信息
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public IList<ViewSiteAssModel> Search(SiteAssSearchModel searchModel)
        {
            return _siteAssDAL.Search(searchModel);
        }

        #endregion
    }
}
