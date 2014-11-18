using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.Model.Common;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.IBLL.Sorting.Return;
using Vancl.TMS.IDAL.Sorting.Return;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.BLL.Sorting.Return
{
    /// <summary>
    /// 退货业务实现
    /// </summary>
    public class ReturnBLL : SortCenterBLL, IReturnBLL
    {
        IBillReturnBoxInfoDAL BillReturnBoxInfoDAL = ServiceFactory.GetService<IBillReturnBoxInfoDAL>();
        IBillReturnDetailInfoDAL BillReturnDetailInfoDAL = ServiceFactory.GetService<IBillReturnDetailInfoDAL>();
        IBillDAL BillDAL = ServiceFactory.GetService<IBillDAL>();
        IBillReturnDAL BillReturnDAL = ServiceFactory.GetService<IBillReturnDAL>();
        /// <summary>
        /// 退货前置条件对象
        /// </summary>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>
        public ReturnBillPreConditionModel GetPreCondition(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode Is null or empty.");
            Enums.ReturnStatus? PreStatus = BillBLL.GetBillReturnStatus(FormCode);
            if (PreStatus.HasValue)
            {
                ReturnBillPreConditionModel preCondition = new ReturnBillPreConditionModel()
                {
                    PreStatus = PreStatus.Value
                };
                return preCondition;
            }
            return null;
        }

        /// <summary>
        /// 获取最大的箱号
        /// </summary>
        /// <param name="boxNoHead"></param>
        /// <returns></returns>
        public string GetMaxBoxNO(string boxNoHead)
        {
            return BillReturnBoxInfoDAL.GetMaxBoxNO(boxNoHead);
        }

        /// <summary>
        /// 装箱打印操作
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool InBoxPrint(string BoxNo, decimal Weight, out string Message)
        {
            Message = string.Empty;
            if (string.IsNullOrEmpty(BoxNo))
            {
                Message = "箱号为空";
                return false;
            }
            if (BillReturnBoxInfoDAL.IsInBoxPrint(BoxNo))
            {
                Message = "该箱号已经装箱打印";
                return false;
            }
            else
            {
                var lists = BillReturnDetailInfoDAL.GetListByBoxNo(BoxNo);
                if (lists.Count == 0)
                {
                    Message = "该箱号中没有运单";
                    return false;
                }
                else
                {
                    using (IACID scope = ACIDScopeFactory.GetTmsACID())
                    {
                        try
                        {
                            BillReturnBoxInfoModel model = BillReturnBoxInfoDAL.GetBoxInfoByBoxNo(BoxNo);
                            var logModel = new BillChangeLogDynamicModel
                            {
                                CreateBy = UserContext.CurrentUser.ID,
                                CreateDept = UserContext.CurrentUser.DeptID,
                                CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                                CurrentSatus = Enums.BillStatus.ReturnOnStation,
                                ReturnStatus = Enums.ReturnStatus.ReturnInbounded,
                                DeliverStationID = -1,
                                FormCode = BoxNo,
                                OperateType = Enums.TmsOperateType.ReturnBoxPrintBackPacking,
                                PreStatus = Enums.BillStatus.ReturnInBound,
                            };
                            if (BillReturnBoxInfoDAL.UpdateIsPrintBackPacking(BoxNo,Weight))
                            {
                                logModel.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                            }
                            WriteBillChangeLog(logModel);
                            //foreach (var item in lists)
                            //{
                            //    BillModel billmodel = BillDAL.GetBillByFormCode(item.FormCode);
                            //    var billLogModel = new BillChangeLogDynamicModel
                            //    {
                            //        CreateBy = billmodel.CreateBy,
                            //        CreateDept = billmodel.CreateDept,
                            //        CurrentDistributionCode =billmodel.CurrentDistributionCode,
                            //        CurrentSatus = billmodel.Status,
                            //        ReturnStatus = Enums.ReturnStatus.ReturnInbounded,
                            //        DeliverStationID = -1,
                            //        FormCode = billmodel.FormCode,
                            //        OperateType = Enums.TmsOperateType.ReturnInbound,
                            //        PreStatus = billmodel.Status
                            //    };
                            //    billLogModel.ExtendedObj.CreateDeptName = item.CreateDept;
                            //    if (BillDAL.UpdateBillReturnStatus(item.FormCode, Enums.ReturnStatus.ReturnInbounded)>0)
                            //    {
                            //        BillReturnDetailInfoDAL.UpdateSyncedStatus(item.FormCode);
                            //        logModel.ReturnStatus = Enums.ReturnStatus.ReturnInbounded;
                            //    }
                            //    WriteBillChangeLog(billLogModel);
                            //}
                            scope.Complete();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("更新箱号装箱打印信息失败", ex);
                        }
                    }
                    return true;
                }
            }
        }
        /// <summary>
        /// 退货出库操作
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public ResultModel ReturnOutBound(string BoxNoOrFormCodes, Enums.ReturnStatus status, bool isBox)
        {
            if (string.IsNullOrEmpty(BoxNoOrFormCodes)) return new ResultModel() { IsSuccess=false,Message="没有需要操作的箱号或单号"};
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                try
                {
                    if (isBox)
                    {
                        var lists = BillReturnDetailInfoDAL.GetListByBoxNo(BoxNoOrFormCodes);
                        if (lists != null && lists.Count > 0)
                        {
                            foreach (var item in lists)
                            {
                                BillReturnModel model = BillReturnDAL.GetBillByFormCode(item.FormCode);
                                var logModel = new BillChangeLogDynamicModel
                                {
                                    CreateBy = UserContext.CurrentUser.ID,
                                    CreateDept = UserContext.CurrentUser.DeptID,
                                    CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                                    CurrentSatus = Enums.BillStatus.ReturnOnStation,
                                    ReturnStatus = status,
                                    DeliverStationID = model.DeliverStationID,
                                    FormCode = model.FormCode,
                                    OperateType = Enums.TmsOperateType.ReturnOutbound,
                                    PreStatus = model.Status,
                                };
                                logModel.ExtendedObj.CreateDeptName = UserContext.CurrentUser.DeptName;
                                if(BillReturnDetailInfoDAL.IsSynced(item.FormCode))
                                {
                                    return new ResultModel() { IsSuccess=false,Message=item.FormCode+"尚未完成回传LMS，请稍后操作。"};
                                }
                                if (BillDAL.UpdateBillReturnStatus(item.FormCode, status) > 0)
                                {
                                    BillReturnDetailInfoDAL.UpdateSyncedStatus(item.FormCode);
                                    logModel.ReturnStatus = Enums.ReturnStatus.ReturnInTransit;
                                }
                                WriteBillChangeLog(logModel);
                            }
                        }
                    }
                    else
                    {
                        string[] forms = BoxNoOrFormCodes.Split(',');
                        foreach (var formcode in forms)
                        {
                            BillReturnModel model = BillReturnDAL.GetBillByFormCode(formcode);
                            var logModel = new BillChangeLogDynamicModel
                            {
                                CreateBy = UserContext.CurrentUser.ID,
                                CreateDept = UserContext.CurrentUser.DeptID,
                                CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                                CurrentSatus = Enums.BillStatus.ReturnOnStation,
                                ReturnStatus = status,
                                DeliverStationID = model.DeliverStationID,
                                FormCode = model.FormCode,
                                OperateType = Enums.TmsOperateType.ReturnOutbound,
                                PreStatus = model.Status,
                            };
                            logModel.ExtendedObj.CreateDeptName = UserContext.CurrentUser.DeptName;
                            if(BillReturnDetailInfoDAL.IsSynced(formcode))
                            {
                                return new ResultModel() { IsSuccess = false, Message = formcode + "尚未完成回传LMS，请稍后操作。" };
                            }
                            if (BillDAL.UpdateBillReturnStatus(formcode, status) > 0)
                            {
                                BillReturnDetailInfoDAL.UpdateSyncedStatus(formcode);
                                logModel.ReturnStatus = Enums.ReturnStatus.ReturnInTransit;
                            }
                            WriteBillChangeLog(logModel);
                        }
                    }
                    //return BillReturnDetailInfoDAL.ReturnOutBound(BoxNoOrFormCodes, status, isBox);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    return new ResultModel() {IsSuccess=false,Message=ex.Message };
                }
            }
            return new ResultModel().Succeed();
        }
        /// <summary>
        /// 更新运单返货状态
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool UpdateBillReturnStatus(string FormCode, Enums.ReturnStatus status)
        {
            if (string.IsNullOrEmpty(FormCode)) return false;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                try
                {
                    Enums.BillStatus billstatus = Enums.BillStatus.ReturnInBound;
                    Enums.TmsOperateType TmsOperatetype = Enums.TmsOperateType.ReturnInbound;
                    if(status == Enums.ReturnStatus.ReturnInbounded)
                    {
                        billstatus = Enums.BillStatus.ReturnInBound;
                        TmsOperatetype = Enums.TmsOperateType.ReturnInbound;
                    }
                    if (status == Enums.ReturnStatus.ReturnInTransit)
                    {
                        billstatus = Enums.BillStatus.ReturnOnStation;
                        TmsOperatetype = Enums.TmsOperateType.ReturnOutbound;
                    }
                    if (status == Enums.ReturnStatus.RejectedInbounded)
                    {
                        billstatus = Enums.BillStatus.RefusedBounded;
                        TmsOperatetype = Enums.TmsOperateType.RejectBillInbound;
                    }
                    BillModel model = BillDAL.GetBillByFormCode(FormCode);
                    var logModel = new BillChangeLogDynamicModel
                    {
                        CreateBy = UserContext.CurrentUser.ID,
                        CreateDept = UserContext.CurrentUser.DeptID,
                        CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
                        CurrentSatus = billstatus,
                        ReturnStatus = status,
                        DeliverStationID = model.DeliverStationID,
                        FormCode = model.FormCode,
                        OperateType = TmsOperatetype,
                        PreStatus = model.Status,

                    };
                    if (BillDAL.UpdateBillReturnStatus(FormCode, status) > 0)
                    {
                        logModel.ReturnStatus = status;
                    }
                    logModel.ExtendedObj.CreateDeptName = UserContext.CurrentUser.DeptName;
                    WriteBillChangeLog(logModel);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    throw new Exception("更单号的逆向状态失败", ex);
                }
            }
            return true;
        }
        /// <summary>
        /// 改变周转箱使用状态
        /// </summary>
        /// <param name="isUsing"></param>
        /// <returns></returns>
        public bool UpdateUsingStatus(bool isUsing, string boxNo)
        {
            return true;
        }
        /// <summary>
        /// 验证周转箱
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public string ValidBox(string BoxNo)
        {
            string Message = "";
            if (string.IsNullOrEmpty(BoxNo)) Message = "请输入周转箱号！";
            using (CloudBillService.BillServiceClient client = new CloudBillService.BillServiceClient())
            {
                var box = client.GetTransitBoxModel(BoxNo);
                if (box == null)
                {
                    Message = "箱号无效!";
                }
                else
                {
                    if (!Convert.ToBoolean(box.IsUsing))
                    {
                        Message = "该周转箱无效！";
                    }
                }
            }
            return Message;
        }


    }
}
