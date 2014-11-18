using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Claim;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Claim;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.BLL.Claim
{
    public class ExpectDelayBLL : BaseBLL, IExpectDelayBLL
    {
        IExpectDelayDAL _expectDelayDAL = ServiceFactory.GetService<IExpectDelayDAL>("ExpectDelayDAL");
        IDispatchDAL _dispatchDAL = ServiceFactory.GetService<IDispatchDAL>("DispatchDAL");
        #region IExpectDelayBLL 成员


        /// <summary>
        /// 根据条件搜索出预期延误的分页列表
        /// </summary>
        /// <param name="searchModel">搜索条件</param>
        /// <returns>预期延误的分页列表</returns>
        public PagedList<ViewExpectDelayModel> Search(ExpectDelaySearchModel searchModel)
        {
            return _expectDelayDAL.Search(searchModel);
        }

        /// <summary>
        /// 根据调度ID获取预期延误
        /// </summary>
        /// <param name="dispatchID">调度ID</param>
        /// <returns>预期延误信息</returns>
        public ViewExpectDelayModel Get(long dispatchID)
        {
            return _expectDelayDAL.GetViewExpectDelayModel(dispatchID);
        }

        /// <summary>
        /// 申请预期延误
        /// </summary>
        /// <param name="dispatchID">调度ID</param>
        /// <param name="expectDelayType">预期延误类型</param>
        /// <param name="delayTime">延误时间</param>
        /// <param name="delayDesc">延误描述</param>
        public ResultModel ApplyForExpectDelay(long dispatchID, Enums.ExpectDelayType expectDelayType, int delayTime, string delayDesc)
        {
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                var dispatch = _expectDelayDAL.GetViewExpectDelayModel(dispatchID);
                if (dispatch == null)
                {
                    return ErrorResult(string.Format("未找到ID为{0}的调度信息！", dispatchID));
                }

                ExpectDelayModel pastExpectDelayModel = _expectDelayDAL.GetByDeliveryNo(dispatch.DeliveryNo);
                if (pastExpectDelayModel != null)
                {
                    //删除原申请
                    _expectDelayDAL.Delete(pastExpectDelayModel.EDID);
                }

                ExpectDelayModel nowExpectDelayModel = new ExpectDelayModel
                {
                    EDID = 0,
                    ApproveStatus = Enums.ApproveStatus.NotApprove,
                    CarrierWaybillNo = dispatch.WaybillNo ?? " ",
                    DelayDesc = delayDesc,
                    DelayTime = delayTime,
                    DeliveryNo = dispatch.DeliveryNo,
                    ExpectDelayType = expectDelayType,
                    ExpectTime = dispatch.ExpectArrivalDate.Value.AddHours(delayTime)
                };
                //添加新申请
                _expectDelayDAL.Add(nowExpectDelayModel);
                if (pastExpectDelayModel != null)
                {
                    //写修改日志
                    WriteUpdateLog<ExpectDelayModel>(nowExpectDelayModel, pastExpectDelayModel);
                }
                else
                {
                    //写新增日志
                    WriteInsertLog<ExpectDelayModel>(nowExpectDelayModel);
                }
                scope.Complete();
            }
            return InfoResult("申请预计延误成功！");
        }

        /// <summary>
        /// 预期延误审核
        /// </summary>
        /// <param name="dispatchID">调度ID</param>
        /// <param name="approveStatus">审核状态</param>
        public ResultModel Audit(long dispatchID, Enums.ApproveStatus approveStatus)
        {
            var dispatch = _expectDelayDAL.GetViewExpectDelayModel(dispatchID);
            if (dispatch == null)
            {
                return ErrorResult(string.Format("未找到ID为{0}的调度信息！", dispatchID));
            }
            ExpectDelayModel pastExpectDelayModel = _expectDelayDAL.GetByDeliveryNo(dispatch.DeliveryNo);
            if (pastExpectDelayModel == null)
            {
                return ErrorResult("不存在需要审核的信息！");
            }
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                if (approveStatus == Enums.ApproveStatus.Approved)
                {
                    _dispatchDAL.UpdateDispatchCofirmExpArrivalDate(dispatchID, pastExpectDelayModel.ExpectTime);
                }
                ExpectDelayModel nowExpectDelayModel = VanclConverter.ConvertModel<ExpectDelayModel, ExpectDelayModel>(pastExpectDelayModel);
                nowExpectDelayModel.ApproveStatus = approveStatus;
                _expectDelayDAL.Approve(nowExpectDelayModel);
                WriteUpdateLog<ExpectDelayModel>(nowExpectDelayModel, pastExpectDelayModel);
                scope.Complete();
            }
            return InfoResult("审核预计延误成功！");
        }

        #endregion
    }
}
