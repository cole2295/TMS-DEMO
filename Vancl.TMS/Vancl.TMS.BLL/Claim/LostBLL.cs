using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Claim;
using Vancl.TMS.IDAL.Claim;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Claim.Lost;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Util.EnumUtil;

namespace Vancl.TMS.BLL.Claim
{
    public class LostBLL : BaseBLL, ILostBLL
    {
        ILostDAL _lostDAL = ServiceFactory.GetService<ILostDAL>("LostDAL");
        ILostDetailDAL _lostDetailDAL = ServiceFactory.GetService<ILostDetailDAL>("LostDetailDAL");
        IDispOrderDetailDAL _dodDAL = ServiceFactory.GetService<IDispOrderDetailDAL>("DispOrderDetailDAL");
        IDispatchBLL _dispatchBLL = ServiceFactory.GetService<IDispatchBLL>("DispatchBLL");

        #region ILostBLL 成员
        public IList<ViewLostModel> Search(LostSearchModel searchModel)
        {
            return _lostDAL.Search(searchModel);
        }

        public ViewLostDetailModel GetPreLostDetail(string deliveryNo)
        {
            ViewLostDetailModel model = new ViewLostDetailModel();
            List<ViewLostBoxModel> boxList = (List<ViewLostBoxModel>)_lostDAL.GetBoxDetail(deliveryNo);
            List<LostOrderDetailModel> orderList = (List<LostOrderDetailModel>)_lostDAL.GetPreLostOrderDetail(deliveryNo);
            ChangeBoxLostStatus(boxList, orderList);
            model.BoxList = boxList;
            model.PreLostOrderList = orderList;
            return model;
        }

        public ViewLostDetailModel GetLostDetail(string deliveryNo)
        {
            ViewLostDetailModel model = new ViewLostDetailModel();
            List<ViewLostBoxModel> boxList = (List<ViewLostBoxModel>)_lostDAL.GetBoxDetail(deliveryNo);
            List<LostOrderDetailModel> orderList = (List<LostOrderDetailModel>)_lostDAL.GetLostOrderDetail(deliveryNo);
            ChangeBoxLostStatus(boxList, orderList);
            model.BoxList = boxList;
            model.PreLostOrderList = orderList;
            return model;
        }

        private void ChangeBoxLostStatus(List<ViewLostBoxModel> boxList, List<LostOrderDetailModel> orderList)
        {
            if (boxList == null || boxList.Count == 0)
            {
                return;
            }
            if (orderList == null || orderList.Count == 0)
            {
                return;
            }
            int orderCount = 0;
            foreach (ViewLostBoxModel b in boxList)
            {
                orderCount = 0;
                foreach (LostOrderDetailModel o in orderList)
                {
                    if (o.BoxNo.Equals(b.BoxNO))
                    {
                        b.BoxLostStatus = Enums.BoxLostStatus.Has;
                        orderCount++;
                    }
                }
                if (orderCount == b.OrderCount)
                {
                    b.BoxLostStatus = Enums.BoxLostStatus.AllLost;
                }
            }
        }

        public IList<ViewDispOrderDetailModel> GetOrderDetail(string deliveryNo, string boxNo)
        {
            return _dodDAL.GetOrderDetail(deliveryNo, boxNo);
        }

        public ResultModel AddLost(string deliveryNo, List<string> lstBoxNo, List<string> lstFormCode)
        {
            var result = CheckLostStatus(deliveryNo, true);
            if (!result.IsSuccess)
            {
                return result;
            }
            lstFormCode = GetMergedFormCodes(deliveryNo, lstBoxNo, lstFormCode);
            if (lstFormCode == null)
            {
                return ErrorResult("请选择丢失的订单！");
            }
            decimal totalAmount = _dodDAL.GetOrderTotalAmountByFormCodes(lstFormCode);
            decimal protectedPrice = _dodDAL.GetTotalProtectedPriceByFormCodes(lstFormCode);
            string errorMessage = "";
            LostModel model = BuiltLostModel(deliveryNo, false, totalAmount, protectedPrice, true, out errorMessage);
            if (model == null)
            {
                return ErrorResult(errorMessage);
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _lostDAL.Add(model);
                WriteInsertLog<LostModel>(model);
                IList<ViewDispOrderDetailModel> list = _dodDAL.GetOrderDetail(lstFormCode);
                LostDetailModel dm = new LostDetailModel();
                dm.LID = model.LID;
                foreach (ViewDispOrderDetailModel m in list)
                {
                    dm.BoxNo = m.BoxNo;
                    dm.FormCode = m.FormCode;
                    dm.Price = m.Price;
                    dm.ProtectedPrice = m.ProtectedPrice;
                    dm.DeliveryNo = m.DeliveryNo;
                    _lostDetailDAL.Add(dm);
                }
                scope.Complete();
            }
            return AddResult(i, "丢失信息");
        }

        private List<string> GetMergedFormCodes(string deliveryNo, List<string> lstBoxNo, List<string> lstFormCode)
        {
            if (lstBoxNo == null && lstFormCode == null)
            {
                return null;
            }
            //合并单号
            if (lstBoxNo != null && lstBoxNo.Count > 0)
            {
                List<LostOrderDetailModel> orderList = (List<LostOrderDetailModel>)_lostDAL.GetOrderDetail(deliveryNo, lstBoxNo);
                foreach (LostOrderDetailModel m in orderList)
                {
                    if (!lstFormCode.Contains(m.FormCode))
                    {
                        lstFormCode.Add(m.FormCode);
                    }
                }
            }
            if (lstFormCode == null || lstFormCode.Count == 0)
            {
                throw new CodeNotValidException();
            }
            return lstFormCode;
        }

        public ResultModel AddAllLost(string deliveryNo)
        {
            var result = CheckLostStatus(deliveryNo, true);
            if (!result.IsSuccess)
            {
                return result;
            }
            decimal totalAmount = _dodDAL.GetOrderTotalAmountByDeliveryNo(deliveryNo);
            decimal protectedPrice = _dodDAL.GetTotalProtectedPriceByDeliveryNo(deliveryNo);
            string errorMessage = "";
            LostModel model = BuiltLostModel(deliveryNo, true, totalAmount, protectedPrice, true, out errorMessage);
            if (model == null)
            {
                return ErrorResult(errorMessage);
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _lostDAL.Add(model);
                WriteInsertLog<LostModel>(model);
                scope.Complete();
            }
            return InfoResult("全部丢失成功！");
        }

        private LostModel BuiltLostModel(string deliveryNo, bool isAllLost, decimal totalAmount, decimal protectedPrice, bool isAdd, out string errorMessage)
        {
            errorMessage = "";
            if (isAdd)
            {
                if (IsExistLost(deliveryNo))
                {
                    errorMessage = "该提货单丢失信息已存在！";
                    return null;
                }
            }
            DispatchModel disModel = _dispatchBLL.Get(deliveryNo);
            if (disModel == null)
            {
                errorMessage = "该提货单不存在或者已经被删除！";
                return null;
            }
            LostModel model = new LostModel();
            model.LID = _lostDAL.GetNextSequence(model.SequenceName);
            model.ApproveStatus = Enums.ApproveStatus.NotApprove;
            model.ArrivalID = disModel.ArrivalID;
            model.DeliveryNo = deliveryNo;
            model.DepartureID = disModel.DepartureID;
            model.IsAllLost = isAllLost;
            model.LostAmount = totalAmount;
            model.ProtectedPrice = protectedPrice;
            return model;
        }

        public ResultModel UpdateLost(string deliveryNo, List<string> lstBoxNo, List<string> lstFormCode, bool isAllLost)
        {
            var result = CheckLostStatus(deliveryNo, false);
            if (!result.IsSuccess)
            {
                return result;
            }
            decimal totalAmount = 0;
            decimal protectedPrice = 0;
            if (isAllLost)
            {
                totalAmount = _dodDAL.GetOrderTotalAmountByDeliveryNo(deliveryNo);
                protectedPrice = _dodDAL.GetTotalProtectedPriceByDeliveryNo(deliveryNo);
            }
            else
            {
                lstFormCode = GetMergedFormCodes(deliveryNo, lstBoxNo, lstFormCode);
                if (lstFormCode == null)
                {
                    return Delete(deliveryNo);
                }
                totalAmount = _dodDAL.GetOrderTotalAmountByFormCodes(lstFormCode);
                protectedPrice = _dodDAL.GetTotalProtectedPriceByFormCodes(lstFormCode);
            }
            LostModel pastLostModel = Get(deliveryNo);
            string errorMessage = "";
            LostModel nowLostModel = BuiltLostModel(deliveryNo, isAllLost, totalAmount, protectedPrice, false, out errorMessage);
            if (nowLostModel == null)
            {
                return ErrorResult(errorMessage);
            }
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                //删除以前的丢失信息
                _lostDAL.Delete(deliveryNo);
                _lostDetailDAL.Delete(deliveryNo);
                //增加新的丢失信息
                i = _lostDAL.Add(nowLostModel);
                WriteUpdateLog<LostModel>(nowLostModel, pastLostModel);
                if (!isAllLost)
                {
                    IList<ViewDispOrderDetailModel> list = _dodDAL.GetOrderDetail(lstFormCode);
                    LostDetailModel dm = new LostDetailModel();
                    dm.LID = nowLostModel.LID;
                    foreach (ViewDispOrderDetailModel m in list)
                    {
                        dm.BoxNo = m.BoxNo;
                        dm.FormCode = m.FormCode;
                        dm.Price = m.Price;
                        dm.ProtectedPrice = m.ProtectedPrice;
                        dm.DeliveryNo = m.DeliveryNo;
                        _lostDetailDAL.Add(dm);
                    }
                }
                scope.Complete();
            }
            return UpdateResult(i, "丢失信息");
        }

        public LostModel Get(string deliveryNo)
        {
            return _lostDAL.Get(deliveryNo);
        }

        public bool IsExistLost(string deliveryNo)
        {
            return _lostDAL.IsExistLost(deliveryNo);
        }

        public ResultModel Approve(string deliveryNo, Enums.ApproveStatus approveStatus)
        {
            int i = 0;
            LostModel pastModel = Get(deliveryNo);
            if (pastModel == null)
            {
                return ErrorResult("该提货单丢失信息不存在或者已经被删除！");
            }
            if (pastModel.ApproveStatus == Enums.ApproveStatus.Approved
                || pastModel.ApproveStatus == Enums.ApproveStatus.Dismissed)
            {
                return ErrorResult("该提货单丢失信息已经被审核");
            }
            DispatchModel dispModel = _dispatchBLL.Get(deliveryNo);
            if (dispModel == null)
            {
                return ErrorResult("该提货单不存在或已经被删除！");
            }
            if (dispModel.DeliveryStatus != Enums.DeliveryStatus.InTransit)
            {
                return ErrorResult(string.Format("该提货单状态已经更新为【{0}】，不能进行审核操作！", EnumHelper.GetDescription(dispModel.DeliveryStatus)));
            }
            LostModel nowModel = VanclConverter.ConvertModel<LostModel, LostModel>(pastModel);
            nowModel.ApproveStatus = approveStatus;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _lostDAL.Approve(deliveryNo, approveStatus);
                WriteUpdateLog<LostModel>(pastModel, pastModel);
                //如果审核通过了全部丢失的丢失信息，更新dispatch表和order表
                if (approveStatus == Enums.ApproveStatus.Approved)
                {
                    if (nowModel.IsAllLost)
                    {
                        _dispatchBLL.UpdateDeliveryStatus(deliveryNo, Enums.DeliveryStatus.AllLost);
                    }
                    _dispatchBLL.UpdateDeliveryToExistsLost(deliveryNo);
                    _lostDAL.UpdateOrderTMSStatus(deliveryNo, nowModel.IsAllLost);
                }
                scope.Complete();
            }
            string s;
            if (approveStatus == Enums.ApproveStatus.Approved)
            {
                s = "审核";
            }
            else
            {
                s = "驳回";
            }
            if (i > 0)
            {
                return InfoResult(string.Format("丢失信息{0}成功！", s));
            }
            else
            {
                return ErrorResult(string.Format("丢失信息{0}失败！", s));
            }
        }

        public IList<ViewLostOrderModel> GetOrderList(string boxNo)
        {
            return _lostDAL.GetOrderList(boxNo);
        }

        public ResultModel Delete(string deliveryNo)
        {
            int i = 0;
            LostModel model = Get(deliveryNo);
            if (model == null)
            {
                return ErrorResult("该丢失信息不存在或者已经被删除！");
            }
            model.IsDeleted = true;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _lostDAL.Delete(deliveryNo);
                WriteDeleteLog<LostModel>(model);
                _lostDetailDAL.Delete(deliveryNo);
                scope.Complete();
            }
            return DeleteResult(i, "丢失信息");
        }

        /// <summary>
        /// 查询指定箱号内的丢失订单
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public IList<ViewLostOrderModel> GetLostOrderList(string boxNo)
        {
            return _lostDAL.GetLostOrderList(boxNo);
        }

        public ResultModel CheckLostStatus(string deliveryNo, bool isAddLost)
        {
            LostModel model = _lostDAL.Get(deliveryNo);
            if (model == null && !isAddLost)
            {
                return ErrorResult("该丢失信息不存在或已经被删除！");
            }
            if (model != null && isAddLost)
            {
                return ErrorResult("该丢失信息已经存在！");
            }
            DispatchModel dispModel = _dispatchBLL.Get(deliveryNo);
            if (dispModel == null)
            {
                return ErrorResult("该提货单不存在或已经被删除！");
            }
            if (dispModel.DeliveryStatus != Enums.DeliveryStatus.InTransit)
            {
                return ErrorResult(string.Format("该提货单状态已经更新为【{0}】，不能进行丢失操作！", EnumHelper.GetDescription(dispModel.DeliveryStatus)));
            }
            return InfoResult("");
        }
        #endregion
    }
}
