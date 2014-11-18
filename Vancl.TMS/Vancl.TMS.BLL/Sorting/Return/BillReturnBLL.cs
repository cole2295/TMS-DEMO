using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Return;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Sorting.Return;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.BLL.Sorting.Return
{
    public class BillReturnBLL : SortCenterBLL, IBillReturnBLL
    {
        IBillReturnDetailInfoDAL BillReturnDetailInfoDAL = ServiceFactory.GetService<IBillReturnDetailInfoDAL>();
        IBillReturnDAL BillReturnDAL = ServiceFactory.GetService<IBillReturnDAL>();
        IBillDAL BillDAL = ServiceFactory.GetService<IBillDAL>();
        /// <summary>
        /// 添加一条返货入库数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(BillReturnModel model)
        {
            return BillReturnDAL.Add(model);
        }
        /// <summary>
        /// 根据运单号查询一条返货入库的记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public BillReturnModel GetModel(string FormCode)
        {
            if (string.IsNullOrEmpty(FormCode)) return null;
            return BillReturnDAL.GetModel(FormCode);
        }
        /// <summary>
        /// 根据运单号或者箱号或者标签号查询一条返货入库的记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <param name="CodeType"></param>
        /// <returns></returns>
        public BillReturnEntityModel GetModel(string FormCode, string weight, int codetype, out string err)
        {
            err = "";
            if (string.IsNullOrEmpty(FormCode)) return null;
            using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
            {
                var billInfo = client.GetWaybillReturnModel(long.Parse(FormCode),codetype);
                if (billInfo != null)
                {
                    BillReturnEntityModel model = new BillReturnEntityModel()
                    {
                        BillReturnInfoID = billInfo.WaybillReturnInfoID,
                        Weight = billInfo.Weight,
                        FormCode = billInfo.WaybillNo,
                        BoxLabelNo = billInfo.BoxLabelNo,
                        BoxNo = billInfo.BoxNo,
                        BoxStatus = billInfo.BoxStatus,
                        CreateBy = billInfo.CreateBy,
                        UpdateBy = billInfo.UpdateBy,
                        CreateTime = billInfo.CreateTime,
                        LabelNo = billInfo.LabelNo,
                        UpdateTime = billInfo.UpdateTime
                    };
                    return model;
                }
                else
                {
                    if (codetype == 0)
                    {
                        err = "该运单还未称重！";
                    }
                    else
                    {
                        err = "标签号不存在！";
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public ViewBillReturnInfoModel GetReturnBillInfo(string FormCode)
        {
            if (string.IsNullOrEmpty(FormCode)) return null;
            using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
            {
                var waybill = client.GetWaybillReturnInfoModel(FormCode);
                if (waybill != null)
                {
                    ViewBillReturnInfoModel model = new ViewBillReturnInfoModel()
                    {
                        billReturnInfoId = waybill.WaybillReturnInfoId,
                        FormCode = waybill.WaybillNo,
                        LabelNo = waybill.LabelNo,
                        BoxNo = waybill.BoxNo,
                        BoxLabelNo = waybill.BoxLabelNo,
                        StatusName = waybill.StatusName,
                        OpTime = waybill.OpTime,
                        CreateTime = waybill.CreateTime,
                        OpMessage = waybill.OpMessage,
                        UpdateTime = waybill.UpdateTime,
                        Weight = waybill.Weight,
                        BillSource = waybill.WaybillSource,
                        BillType = waybill.WaybillType,
                        Code = waybill.Code,
                        IsBox = waybill.IsBox,
                        MerchantName = waybill.MerchantName,
                        NeedAmount = waybill.NeedAmount,
                        NeedBackAmount = waybill.NeedBackAmount
                    };
                    return model;
                }
                return null;
            }
        }
        /// <summary>
        /// 退换货入库
        /// </summary>
        /// <param name="billModel"></param>
        /// <param name="logModel"></param>
        public void BillReturnInbound(BillReturnEntityModel billReturnEntityModel, out string Message)
        {
            IBillDAL BillDAL = ServiceFactory.GetService<IBillDAL>();
            Message = "";
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                try
                {
                    if (billReturnEntityModel != null)
                    {
                        var billModel = BillDAL.GetBillByFormCode(billReturnEntityModel.FormCode.ToString());
                        if (billModel == null)
                        {
                            Message = "该运单不存在！";
                            return;
                        }
                        if (!IsReturnbill(billModel))
                        {
                            Message = "该运单不是退库单!";
                            return;
                        }
                        if (billModel.ReturnStatus == null)
                        {
                            Message = "该运单还未打印退库交接!";
                            return;
                        }
                        if (billModel.ReturnStatus > 0)
                        {
                            Message = "该运单已分拣入库！";
                        }
                        billModel.CreateDept = UserContext.CurrentUser.DeptID;
                        ReturnWaybillInBound(billModel);
                        scope.Complete();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("更单号的逆向状态失败", ex);
                }
            }
        }
        #region 私有方法

        /// <summary>
        /// 退库单入库
        /// </summary>
        /// <param name="wbObj">运单信息</param>
        /// <param name="model">退库单信息</param>
        /// <param name="opLog">日志</param>
        /// <returns></returns>
        private bool ReturnWaybillInBound(BillModel bogj)
        {
            if (bogj != null)
            {
                bogj.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                var log = new BillChangeLogDynamicModel
                {
                    CreateBy = UserContext.CurrentUser.ID,
                    CreateDept = UserContext.CurrentUser.DeptID,
                    CurrentSatus=Enums.BillStatus.ReturnInBound,
                    CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                    DeliverStationID = bogj.DeliverStationID,
                    FormCode = bogj.FormCode,
                    OperateType = Enums.TmsOperateType.ReturnBillInbound,
                    PreStatus = bogj.Status,
                };
                bogj.UpdateBy = UserContext.CurrentUser.ID;
                bogj.UpdateDept = UserContext.CurrentUser.DeptID;
                if (BillDAL.BillReturnInBound(bogj))
                {
                    log.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                }
                WriteBillChangeLog(log);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 改变周转箱使用状态
        /// </summary>
        /// <param name="isUsing"></param>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        private bool UpdateUsingStatus(bool isUsing, string boxNo)
        {
            return true;
        }

        /// <summary>
        /// 是否为退库单
        /// </summary>
        /// <param name="wbObj"></param>
        /// <returns></returns>
        private static bool IsReturnbill(BillModel wbObj)
        {
            if (wbObj.Status == Enums.BillStatus.Rejected ||
                wbObj.Status == Enums.BillStatus.DeliverySuccess)
            {
                if (wbObj.Status == Enums.BillStatus.Rejected)
                {
                    return wbObj.Source == (Enums.BillSource)Enums.BillType.Normal ||
                           wbObj.Source == (Enums.BillSource)Enums.BillType.Exchange;
                }
                else
                {
                    return wbObj.BillType == Enums.BillType.Exchange ||
                           wbObj.BillType == Enums.BillType.Return;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 退换货or拒收入库
        /// </summary>
        /// <param name="wbObj"></param>
        /// <param name="opLog"></param>
        /// <returns></returns>
        public bool ChangeOrRefuseInBound(BillModel bill, OperateLogEntityModel opLog)
        {
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                if (bill != null)
                {
                    opLog.WaybillNO = long.Parse(bill.FormCode);

                    if (bill.Source == Enums.BillSource.Others)
                    {
                        if (bill.Status == Enums.BillStatus.DeliverySuccess)
                        {
                            bill.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                            opLog.Status = Enums.BillStatus.ReturnBounded;
                        }
                        else if (bill.Status == Enums.BillStatus.Rejected)
                        {
                            bill.ReturnStatus = Enums.ReturnStatus.RejectedInbounded;
                            opLog.Status = Enums.BillStatus.RefusedBounded;
                        }
                    }
                    var log = new BillChangeLogDynamicModel
                    {
                        CreateBy = UserContext.CurrentUser.ID,
                        CreateDept = UserContext.CurrentUser.DeptID,
                        CurrentSatus=opLog.Status,
                        CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                        DeliverStationID = bill.DeliverStationID,
                        FormCode = bill.FormCode,
                        OperateType = Enums.TmsOperateType.MerchantBillRefund,
                        PreStatus = bill.Status,
                    };
                    bill.UpdateBy = UserContext.CurrentUser.ID;
                    bill.UpdateDept = UserContext.CurrentUser.DeptID;
                    if (BillDAL.BillReturnInBound(bill))
                    {
                        log.ReturnStatus =(Enums.ReturnStatus)bill.ReturnStatus;
                    }
                    WriteBillChangeLog(log);
                    scope.Complete();
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 运单是否已经逆向入库
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public bool IsBillReturning(string FormCode)
        {
            using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
            {
                return client.IsWaybillReturning(long.Parse(FormCode));
            }
        }
        #endregion
    }
}
