using System.Collections.Generic;
using Vancl.TMS.IBLL.Delivery.InTransit;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Transport.PreDispatch;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.IDAL.Claim;
using System;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.Util.ConfigUtil;

namespace Vancl.TMS.BLL.Delivery.InTransit
{
    public class InTransitBLL : BaseBLL, IInTransitBLL
    {
        IInTransitDAL _InTransitDAL = ServiceFactory.GetService<IInTransitDAL>("InTransitDAL");
        IDispatchBLL _dispatchBLL = ServiceFactory.GetService<IDispatchBLL>("DispatchBLL");
        IDelayDAL _DelayDAL = ServiceFactory.GetService<IDelayDAL>("DelayDAL");
        ICarrierWaybillDAL _CarrierWaybillDAL = ServiceFactory.GetService<ICarrierWaybillDAL>("CarrierWaybillDAL");
        ILostDAL _lostDAL = ServiceFactory.GetService<ILostDAL>("LostDAL");
        IExpectDelayDAL _expectDelayDAL = ServiceFactory.GetService<IExpectDelayDAL>("ExpectDelayDAL");
        IPreDispatchDAL _preDispatchDAL = ServiceFactory.GetService<IPreDispatchDAL>("PreDispatchDAL");
        #region IInTransitBLL 成员

        public PagedList<ViewInTransitModel> Search(InTransitSearchModel searchModel)
        {
            return _InTransitDAL.Search(searchModel);
        }

        public ViewInTransitModel Get(long dispatchID)
        {
            return _InTransitDAL.Get(dispatchID);
        }

        public ResultModel SetArrive(long dispatchID, Enums.DelayType? delayType, string delayReason, string signedUser, DateTime? desReceiveDate = null)
        {
            bool ConfirmLimited = bool.Parse(ConfigurationHelper.GetAppSetting("ConfirmLimited"));
            DispatchModel pastDispatch = _dispatchBLL.Get(dispatchID);
            if (pastDispatch == null)
            {
                return ErrorResult("该提货单不存在或者已经被删除！");
            }
            if (pastDispatch.DeliveryStatus != Enums.DeliveryStatus.InTransit)
            {
                return ErrorResult("该提货单状态发生变化，请刷新页面重试！");
            }
            //如果不限制,以UI为准
            if (!ConfirmLimited)
            {
                pastDispatch.DesReceiveDate = desReceiveDate;
            }
            pastDispatch.SignedUser = signedUser;

            #region 增加修改为调度完成逻辑

            PreDispatchPublicQueryModel QueryModel = new PreDispatchPublicQueryModel();
            QueryModel.DID = dispatchID;
            QueryModel.Status = Enums.DispatchStatus.Dispatching;
            List<PreDispatchModel> listPreDisModelSuc = _preDispatchDAL.GetModelByDispatch(QueryModel);//需要改成调度完成的数据
            List<long> sucPDID = new List<long>();
            List<long> curPDID = new List<long>();
            if (listPreDisModelSuc.Count > 0)
            {
                foreach (PreDispatchModel sucModel in listPreDisModelSuc)
                {
                    sucPDID.Add(sucModel.PDID);
                }
            }


            QueryModel.Status = Enums.DispatchStatus.EstiDispatch;

            List<PreDispatchModel> listPreDisModelCur = _preDispatchDAL.GetModelByDispatch(QueryModel);//需要改为可调度的数据

            if (listPreDisModelCur.Count > 0)
            {
                foreach (PreDispatchModel curModel in listPreDisModelCur)
                {
                    curPDID.Add(curModel.PDID);
                }
            }


            #endregion


            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                if (pastDispatch.ConfirmExpArrivalDate > pastDispatch.DesReceiveDate)
                {
                    //准时到达
                    ResultModel result = _dispatchBLL.ConfirmDeliveryArrived(ConfirmLimited, pastDispatch, Enums.DeliveryStatus.ArrivedOnTime);
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                }
                else
                {
                    //到达延误
                    ResultModel result = _dispatchBLL.ConfirmDeliveryArrived(ConfirmLimited, pastDispatch, Enums.DeliveryStatus.ArrivedDelay);
                    if (!result.IsSuccess)
                    {
                        return result;
                    }
                    string carrierWaybill = _CarrierWaybillDAL.GetWaybillNoByID(pastDispatch.CarrierWaybillID);
                    DelayModel model = new DelayModel();
                    model.CarrierWaybillNo = carrierWaybill;
                    model.DelayReason = delayReason;
                    model.DelayTimeSpan = Convert.ToDecimal((pastDispatch.DesReceiveDate.Value - pastDispatch.ConfirmExpArrivalDate.Value).TotalMinutes / 60.0);
                    model.DelayType = delayType.Value;
                    model.DeliveryNo = pastDispatch.DeliveryNo;
                    _DelayDAL.Add(model);
                }
                //不管是准时到达还是到货延误，都需要更新TMS_Order表的OrderTMSStatus为完成状态
                _InTransitDAL.UpdateOrderTMSStatusToFinished(dispatchID);

                _preDispatchDAL.UpdateToDisabledDispatchV1(sucPDID, Enums.DispatchStatus.DispatchSuccess);
                _preDispatchDAL.UpdateToDisabledDispatchV1(curPDID, Enums.DispatchStatus.CanDispatch);

                scope.Complete();
            }
            return InfoResult("提货单确认到货成功！");
        }

        public bool IsNeedConfirm(string deliveryNo, out string message)
        {
            message = GetLostConfirmMessage(deliveryNo);
            message += GetExpectDelayConfirmMessage(deliveryNo);
            if (string.IsNullOrWhiteSpace(message))
            {
                return false;
            }
            return true;
        }

        #endregion

        private string GetLostConfirmMessage(string deliveryNo)
        {
            string message = "";
            if (_lostDAL.IsExistNotApproveInfo(deliveryNo))
            {
                message = "存在[未审批]的丢失信息";
            }
            else
            {
                if (_lostDAL.IsExistNotInputLostInfo(deliveryNo))
                {
                    message += Environment.NewLine;
                    message += "存在[未录入]的丢失信息";
                }
            }
            if (!string.IsNullOrEmpty(message))
            {
                message += Environment.NewLine;
                message += "如果继续操作，将会忽略丢失信息";
                message += Environment.NewLine;
            }
            return message;
        }

        private string GetExpectDelayConfirmMessage(string deliveryNo)
        {
            string message = "";
            if (_expectDelayDAL.IsExistExpectDelayInfo(deliveryNo))
            {
                message = "存在未审批或已驳回状态的预计延误信息";
            }
            if (!string.IsNullOrEmpty(message))
            {
                message += Environment.NewLine;
                message += "如果继续操作，将会忽略预计延误信息";
                message += Environment.NewLine;
            }
            return message;
        }

        public ViewSetArriveModel GetSetArriveView(long dispatchID)
        {
            return _InTransitDAL.GetSetArriveView(dispatchID);
        }
    }
}
