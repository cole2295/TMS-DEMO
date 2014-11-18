using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Common;
using System.Reflection;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Claim;
using Vancl.TMS.Model.Claim.Lost;

namespace Vancl.TMS.Core.Logging
{
    public class DeliveryLogStrategy<T> : LogStrategy<T> where T : BaseModel, ILogable, new()
    {
        private string _deliveryNo = "";

        public override string GetDeliveryNo()
        {
            if (string.IsNullOrEmpty(_deliveryNo))
            {
                PropertyInfo piDeliveryNo = _nowModel.GetType().GetProperty("DeliveryNo");
                if (piDeliveryNo != null)
                {
                    _deliveryNo = Convert.ToString(piDeliveryNo.GetValue(_nowModel, null));
                }
            }
            return _deliveryNo;
        }
        public override string GetLogNote()
        {
            if (_nowModel is DispatchModel)
            {
                return GetDispatchNote();
            }
            if (_nowModel is LostModel)
            {
                return GetLostNote();
            }
            if (_nowModel is ExpectDelayModel)
            {
                return GetExpectDelayNote();
            }
            return "";
        }

        private string GetDispatchNote()
        {
            string deliveryNo = GetDeliveryNo();
            if (GetFlowType() == Enums.DeliverFlowType.Reject)
            {
                return "提货单[" + deliveryNo + "]已被撤回";
            }
            PropertyInfo piDeliveryStatus = _nowModel.GetType().GetProperty("DeliveryStatus");
            if (piDeliveryStatus != null)
            {
                Enums.DeliveryStatus ds = (Enums.DeliveryStatus)piDeliveryStatus.GetValue(_nowModel, null);
                switch (ds)
                {
                    case Enums.DeliveryStatus.Dispatched:
                        if (_pastModel != null)
                        {
                            return "提货单[" + deliveryNo + "]进行了修改";
                        }
                        else
                        {
                            return "提货单[" + deliveryNo + "]完成调度确认";
                        }
                    case Enums.DeliveryStatus.InTransit:
                        return "提货单[" + deliveryNo + "]已经出库";
                    case Enums.DeliveryStatus.ArrivedOnTime:
                        return "提货单[" + deliveryNo + "]已经准时到达";
                    case Enums.DeliveryStatus.ArrivedDelay:
                        PropertyInfo piConfirmExpArrivalDate = _nowModel.GetType().GetProperty("ConfirmExpArrivalDate");
                        PropertyInfo piDesReceiveDate = _nowModel.GetType().GetProperty("DesReceiveDate");
                        double delay = Math.Round((Convert.ToDateTime(piDesReceiveDate.GetValue(_nowModel, null))
                                - Convert.ToDateTime(piConfirmExpArrivalDate.GetValue(_nowModel, null))).TotalHours, 2);
                        return "提货单[" + deliveryNo + "]已经到达，延误时间为[" + delay.ToString() + "]小时";
                    case Enums.DeliveryStatus.AllLost:
                        return "提货单[" + deliveryNo + "]已全部丢失";
                    case Enums.DeliveryStatus.KPIApproved:
                        return "提货单[" + deliveryNo + "]已经进行KPI审核";
                }
            }
            return "";
        }

        private string GetLostNote()
        {
            string deliveryNo = GetDeliveryNo();
            switch (GetFlowType())
            {
                case Enums.DeliverFlowType.AddLost:
                    return "提货单[" + deliveryNo + "]添加了丢失信息";
                case Enums.DeliverFlowType.UpdateLost:
                    return "提货单[" + deliveryNo + "]更新了丢失信息";
                case Enums.DeliverFlowType.ApproveLost:
                    return "提货单[" + deliveryNo + "]审核通过了丢失信息";
                case Enums.DeliverFlowType.DismissLost:
                    return "提货单[" + deliveryNo + "]驳回了丢失信息";
                case Enums.DeliverFlowType.DeleteLost:
                    return "提货单[" + deliveryNo + "]删除了丢失信息";
            }
            return "";
        }

        private string GetExpectDelayNote()
        {
            string deliveryNo = GetDeliveryNo();
            switch (GetFlowType())
            {
                case Enums.DeliverFlowType.AddExpectDelay:
                    return "提货单[" + deliveryNo + "]添加了预计延误信息";
                case Enums.DeliverFlowType.UpdateExpectDelay:
                    return "提货单[" + deliveryNo + "]更新了预计延误信息";
                case Enums.DeliverFlowType.ApproveExpectDelay:
                    return "提货单[" + deliveryNo + "]审核通过了预计延误信息";
                case Enums.DeliverFlowType.DismissExpectDelay:
                    return "提货单[" + deliveryNo + "]驳回了预计延误信息";
                case Enums.DeliverFlowType.DeleteExpectDelay:
                    return "提货单[" + deliveryNo + "]删除了预计延误信息";
            }
            return "";
        }

        public override bool IsDoOperation()
        {
            return true;
        }

        public override bool GetIsShow()
        {
            return true;
        }

        public override Enums.DeliverFlowType GetFlowType()
        {
            if (_nowModel is DispatchModel)
            {
                return GetDispatchFlowType();
            }
            if (_nowModel is LostModel)
            {
                return GetLostFlowType();
            }
            if (_nowModel is ExpectDelayModel)
            {
                return GetExpectDelayFlowType();
            }
            return base.GetFlowType();
        }

        private Enums.DeliverFlowType GetDispatchFlowType()
        {
            if (_pastModel == null)
            {
                if (_nowModel.IsDeleted)
                {
                    return Enums.DeliverFlowType.Reject;
                }
                return Enums.DeliverFlowType.Add;
            }
            PropertyInfo piDeliveryStatus = _nowModel.GetType().GetProperty("DeliveryStatus");
            if (piDeliveryStatus != null)
            {
                Enums.DeliveryStatus ds = (Enums.DeliveryStatus)piDeliveryStatus.GetValue(_nowModel, null);
                switch (ds)
                {
                    case Enums.DeliveryStatus.Dispatched:
                        return Enums.DeliverFlowType.Update;
                    case Enums.DeliveryStatus.InTransit:
                        return Enums.DeliverFlowType.Outbound;
                    case Enums.DeliveryStatus.ArrivedDelay:
                        return Enums.DeliverFlowType.Inbound;
                    case Enums.DeliveryStatus.ArrivedOnTime:
                        return Enums.DeliverFlowType.Inbound;
                    case Enums.DeliveryStatus.AllLost:
                        return Enums.DeliverFlowType.AllLost;
                    case Enums.DeliveryStatus.KPIApproved:
                        return Enums.DeliverFlowType.Approve;
                }
            }
            return Enums.DeliverFlowType.None;
        }

        private Enums.DeliverFlowType GetLostFlowType()
        {
            if (_pastModel == null)
            {
                if (_nowModel.IsDeleted)
                {
                    return Enums.DeliverFlowType.DeleteLost;
                }
                return Enums.DeliverFlowType.AddLost;
            }
            PropertyInfo piApproveStatus = _nowModel.GetType().GetProperty("ApproveStatus");
            if (piApproveStatus != null)
            {
                Enums.ApproveStatus s = (Enums.ApproveStatus)piApproveStatus.GetValue(_nowModel, null);
                if (s == Enums.ApproveStatus.Approved)
                {
                    return Enums.DeliverFlowType.ApproveLost;
                }
                if (s == Enums.ApproveStatus.Dismissed)
                {
                    return Enums.DeliverFlowType.DismissLost;
                }
            }
            return Enums.DeliverFlowType.UpdateLost;
        }

        private Enums.DeliverFlowType GetExpectDelayFlowType()
        {
            if (_pastModel == null)
            {
                if (_nowModel.IsDeleted)
                {
                    return Enums.DeliverFlowType.DeleteExpectDelay;
                }
                return Enums.DeliverFlowType.AddExpectDelay;
            }
            PropertyInfo piApproveStatus = _nowModel.GetType().GetProperty("ApproveStatus");
            if (piApproveStatus != null)
            {
                Enums.ApproveStatus s = (Enums.ApproveStatus)piApproveStatus.GetValue(_nowModel, null);
                if (s == Enums.ApproveStatus.Approved)
                {
                    return Enums.DeliverFlowType.ApproveExpectDelay;
                }
                if (s == Enums.ApproveStatus.Dismissed)
                {
                    return Enums.DeliverFlowType.DismissExpectDelay;
                }
            }
            return Enums.DeliverFlowType.UpdateExpectDelay;
        }
    }
}
