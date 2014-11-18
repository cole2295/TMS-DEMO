using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Transport.Dispatch;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Transport.Dispatch;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Model.ImportRecord;
using Vancl.TMS.IDAL.DeliveryImport;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.IDAL.Transport.Plan;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.BLL.BaseInfo;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.Transport.PreDispatch;

namespace Vancl.TMS.BLL.Transport.Dispatch
{
    public class DispatchBLL : BaseBLL, IDispatchBLL
    {
        IDispatchDAL _dispatchDAL = ServiceFactory.GetService<IDispatchDAL>("DispatchDAL");
        IDispatchDetailDAL _dispatchDetailDAL = ServiceFactory.GetService<IDispatchDetailDAL>("DispatchDetailDAL");
        IDispOrderDetailDAL _dispOrderDetailDAL = ServiceFactory.GetService<IDispOrderDetailDAL>("DispOrderDetailDAL");
        IDispTransitionDAL _dispTransitionDAL = ServiceFactory.GetService<IDispTransitionDAL>("DispTransitionDAL");
        ILinePlanDAL _lineplanDAL = ServiceFactory.GetService<ILinePlanDAL>("LinePlanDAL");
        IPreDispatchDAL _preDispatchDAL = ServiceFactory.GetService<IPreDispatchDAL>("PreDispatchDAL");
        ICarrierWaybillDAL _carrierWaybillDAL = ServiceFactory.GetService<ICarrierWaybillDAL>("CarrierWaybillDAL");
        IBoxDAL _boxDAL = ServiceFactory.GetService<IBoxDAL>("BoxDAL");
        IDeliveryImportDAL dal = ServiceFactory.GetService<IDeliveryImportDAL>();
        ITransportPlanDAL transPlanDal = ServiceFactory.GetService<ITransportPlanDAL>();

        private bool _isUpdate = false;
        #region IDispatchBLL 成员

        public PagedList<ViewDispatchModel> Search(DispatchSearchModel searchModel)
        {
            return _dispatchDAL.Search(searchModel);
        }

        public PagedList<ViewDeliveryPrintModel> Search(DeliveryPrintSearchModel searchModel)
        {
            return _dispatchDAL.Search(searchModel);
        }

        public PagedList<ViewDeliveryPrintModel> SearchEx(DeliveryPrintSearchModel searchModel)
        {
            return _dispatchDAL.SearchEx(searchModel);
        }

        public ViewDispatchStatisticModel GetStatisticInfo(DispatchSearchModel searchModel)
        {
            return _dispatchDAL.GetStatisticInfo(searchModel);
        }

        public ViewDispatchStatisticModel GetStatisticInfoEx(DispatchSearchModel searchModel)
        {
            return _dispatchDAL.GetStatisticInfoEx(searchModel);
        }

        public DataTable ExportReport(DispatchSearchModel searchModel)
        {
            return _dispatchDAL.ExportReport(searchModel);
        }

        public List<ViewDispatchBoxModel> GetValidBoxList(int DepartureID, string[] disPatchedBox, string departureName, string arrivalName)
        {
            return _dispatchDAL.GetValidBoxList(DepartureID, disPatchedBox, departureName, arrivalName);
        }

        /// <summary>
        /// 取得物流单号实体对象
        /// </summary>
        /// <param name="waybillno"></param>
        /// <returns></returns>
        private CarrierWaybillModel GetWaybillModel(string waybillno)
        {
            CarrierWaybillModel model = new CarrierWaybillModel()
            {
                WaybillNo = waybillno,
                Weight = 0,
                CreateBy = UserContext.CurrentUser.ID,
                UpdateBy = UserContext.CurrentUser.ID,
                IsDeleted = false
            };
            model.CWID = _carrierWaybillDAL.GetNextSequence(model.SequenceName);
            return model;
        }

        /// <summary>
        /// 获取提货单主表Model对象
        /// </summary>
        /// <param name="DeliveryNo"></param>
        /// <param name="waybillModel"></param>
        /// <param name="lineplanModel"></param>
        /// <returns></returns>
        private DispatchModel GetDispatchModel(string DeliveryNo, CarrierWaybillModel waybillModel, LinePlanModel lineplanModel)
        {
            DispatchModel model = new DispatchModel()
            {
                ArrivalID = lineplanModel.ArrivalID,
                ArrivalTiming = lineplanModel.ArrivalTiming,
                CarrierID = lineplanModel.CarrierID,
                CarrierWaybillID = waybillModel.CWID,
                CreateBy = UserContext.CurrentUser.ID,
                DeliveryNo = DeliveryNo,
                DeliveryStatus = Enums.DeliveryStatus.Dispatched,
                DepartureID = lineplanModel.DepartureID,
                IsDeleted = false,
                LineGoodsType = lineplanModel.LineGoodsType,
                LPID = lineplanModel.LPID,
                TransportType = lineplanModel.TransportType,
                UpdateBy = UserContext.CurrentUser.ID
            };
            model.DID = _dispatchDAL.GetNextSequence(model.SequenceName);
            return model;
        }

        /// <summary>
        /// 获取提货单主表Model对象
        /// </summary>
        /// <param name="DeliveryNo"></param>
        /// <param name="waybillModel"></param>
        /// <param name="lineplanModel"></param>
        /// <returns></returns>
        private DispatchModel GetDispatchModelEx(string DeliveryNo, CarrierWaybillModel waybillModel, LinePlanModel lineplanModel)
        {
            DispatchModel model = new DispatchModel()
            {
                ArrivalID = lineplanModel.ArrivalID,
                ArrivalTiming = lineplanModel.ArrivalTiming,
                CarrierID = lineplanModel.CarrierID,
                CarrierWaybillID = waybillModel.CWID,
                CreateBy = UserContext.CurrentUser.ID,
                DeliveryNo = DeliveryNo,
                DeliveryStatus = Enums.DeliveryStatus.WaitForDispatch,
                DepartureID = lineplanModel.DepartureID,
                IsDeleted = false,
                LineGoodsType = lineplanModel.LineGoodsType,
                LPID = lineplanModel.LPID,
                TransportType = lineplanModel.TransportType,
                UpdateBy = UserContext.CurrentUser.ID
            };
            model.DID = _dispatchDAL.GetNextSequence(model.SequenceName);
            return model;
        }

        /// <summary>
        /// 获取提货单箱明细Model列表对象
        /// </summary>
        /// <param name="dispModel">调度对象</param>
        /// <param name="disPatchedBox">待调度的箱</param>
        /// <returns></returns>
        private List<DispatchDetailModel> GetDispatchDetailModelList(DispatchModel dispModel, string[] disPatchedBox)
        {
            //取得待调度箱子中属于预调度的列表信息
            List<PreDispatchModel> listPredispatch = _preDispatchDAL.GetCurPreDispatchList(disPatchedBox, _isUpdate);
            List<DispatchDetailModel> listDispatchDetail = new List<DispatchDetailModel>(disPatchedBox.Length);
            for (int i = 0; i < disPatchedBox.Length; i++)
            {
                PreDispatchModel predispatchModel = (listPredispatch != null && listPredispatch.Count > 0) ? listPredispatch.FirstOrDefault(p => p.BoxNo.Equals(disPatchedBox[i])) : null;
                DispatchDetailModel model = new DispatchDetailModel()
                {
                    BoxNo = disPatchedBox[i],
                    CreateBy = UserContext.CurrentUser.ID,
                    DeliveryNo = dispModel.DeliveryNo,
                    DID = dispModel.DID,
                    IsDeleted = false,
                    IsPlan = predispatchModel != null ? (predispatchModel.LPID == dispModel.LPID ? true : false) : false,   //同预调度中线路相同为true
                    PDID = predispatchModel != null ? predispatchModel.PDID : (long?)null,          //取得箱子在预调度中的PDID,用于撤回
                    UpdateBy = UserContext.CurrentUser.ID
                };
                model.DDID = _dispatchDetailDAL.GetNextSequence(model.SequenceName);
                listDispatchDetail.Add(model);
            }
            return listDispatchDetail;
        }

        /// <summary>
        /// 获取提货单箱明细Model列表对象
        /// </summary>
        /// <param name="dispModel">调度对象</param>
        /// <param name="disPatchedBox">待调度的箱</param>
        /// <param name="listPredispatch"> 待调度批次中已经存在的预先调度信息 </param>
        /// <returns></returns>
        private List<DispatchDetailModel> GetDispatchDetailModelListEx(DispatchModel dispModel, string[] disPatchedBox, List<PreDispatchModel> listPredispatch)
        {
            List<DispatchDetailModel> listDispatchDetail = new List<DispatchDetailModel>(disPatchedBox.Length);
            for (int i = 0; i < disPatchedBox.Length; i++)
            {
                PreDispatchModel predispatchModel = (listPredispatch != null && listPredispatch.Count > 0) ? listPredispatch.FirstOrDefault(p => p.BoxNo.Equals(disPatchedBox[i])) : null;
                DispatchDetailModel model = new DispatchDetailModel()
                {
                    BoxNo = disPatchedBox[i],
                    CreateBy = UserContext.CurrentUser.ID,
                    DeliveryNo = dispModel.DeliveryNo,
                    DID = dispModel.DID,
                    IsDeleted = false,
                    IsPlan = predispatchModel != null ? (predispatchModel.LPID == dispModel.LPID ? true : false) : false,   //同预调度中线路相同为true
                    PDID = predispatchModel != null ? predispatchModel.PDID : (long?)null,          //取得箱子在预调度中的PDID,用于撤回
                    UpdateBy = UserContext.CurrentUser.ID
                };
                model.DDID = _dispatchDetailDAL.GetNextSequence(model.SequenceName);
                listDispatchDetail.Add(model);
            }
            return listDispatchDetail;
        }

        /// <summary>
        /// 获取提货单订单明细Model列表对象
        /// </summary>
        /// <param name="dispModel"></param>
        /// <param name="listDispDetail"></param>
        /// <returns></returns>
        private List<DispOrderDetailModel> GetDispOrderDetailModelList(DispatchModel dispModel, List<DispatchDetailModel> listDispDetail)
        {
            string[] disPatchedBox = listDispDetail.Select(p => p.BoxNo).ToArray<string>();
            //取得待调度的订单明细
            List<OrderModel> listOrder = _boxDAL.GetUnLostOrderList(disPatchedBox);
            if (null == listOrder || listOrder.Count < 1)
            {
                throw new Exception("没有待调度的订单明细");
            }
            List<DispOrderDetailModel> listdispOrderDetail = new List<DispOrderDetailModel>(listOrder.Count);
            for (int i = 0; i < listOrder.Count; i++)
            {
                listdispOrderDetail.Add(new DispOrderDetailModel()
                {
                    ArrivalID = dispModel.ArrivalID,
                    BoxNo = listOrder[i].BoxNo,
                    CreateBy = UserContext.CurrentUser.ID,
                    DDID = listDispDetail.FirstOrDefault(p => p.BoxNo.Equals(listOrder[i].BoxNo)).DDID,
                    DeliveryNo = dispModel.DeliveryNo,
                    FormCode = listOrder[i].FormCode,
                    IsArrived = false,
                    IsDeleted = false,
                    UpdateBy = UserContext.CurrentUser.ID,
                    Price = listOrder[i].Price,
                    ProtectedPrice = listOrder[i].ProtectedPrice
                });
            }
            return listdispOrderDetail;
        }

        /// <summary>
        /// 重建提货单箱明细数据
        /// </summary>
        /// <param name="listdispOrderDetailModel">提货单订单明细</param>
        /// <param name="listdispDetailModel">原始提货单箱明细</param>
        private void ReBuildDispatchDetail(List<DispOrderDetailModel> listdispOrderDetailModel, List<DispatchDetailModel> listdispDetailModel)
        {
            var orderSum = from p in listdispOrderDetailModel
                           group p by p.BoxNo into g
                           select new { BoxNo = g.Key, OrderCount = g.Count(), TotalAmount = g.Sum(p => p.Price), ProtectedPrice = g.Sum(p => p.ProtectedPrice) };
            foreach (var item in orderSum)
            {
                DispatchDetailModel dispdetailModel = listdispDetailModel.FirstOrDefault(p => p.BoxNo.Equals(item.BoxNo));
                if (dispdetailModel != null)
                {
                    dispdetailModel.TotalAmount = item.TotalAmount;
                    dispdetailModel.OrderCount = item.OrderCount;
                    dispdetailModel.ProtectedPrice = item.ProtectedPrice;
                }
            }
        }

        /// <summary>
        /// 重建提货单数据
        /// </summary>
        /// <param name="listdispDetailModel">提货单箱明细数据</param>
        /// <param name="dispModel">原始提货单</param>
        private void ReBuildDispatch(List<DispatchDetailModel> listdispDetailModel, DispatchModel dispModel)
        {
            dispModel.TotalAmount = listdispDetailModel.Sum(p => p.TotalAmount);
            dispModel.BoxCount = listdispDetailModel.Count;
            //报价金额采用其他方式处理
            //dispModel.ProtectedPrice = listdispDetailModel.Sum(p => p.ProtectedPrice);
        }

        /// <summary>
        /// 根据是否预调度分隔
        /// </summary>
        /// <param name="listdispDetailModel"></param>
        /// <param name="listPreDispatchKeyID"></param>
        /// <param name="listUnPreDispatchBoxD"></param>
        private void SplitDispatchDetailByPreDispatch(List<DispatchDetailModel> listdispDetailModel, out List<long> listPreDispatchKeyID, out List<long> listUnPreDispatchKeyD)
        {
            List<long> tmplistPreDispatchKeyID = new List<long>();                       //使用预调度的KeyID
            List<long> tmplistUnPreDispatchKeyD = new List<long>();                //未使用预调度的箱号列表
            listdispDetailModel.ForEach(p =>
            {
                if (p.IsPlan)
                {
                    tmplistPreDispatchKeyID.Add(p.PDID.Value);
                }
                else
                {
                    if (p.PDID.HasValue)
                    {
                        tmplistUnPreDispatchKeyD.Add(p.PDID.Value);
                    }
                }
            });
            listPreDispatchKeyID = tmplistPreDispatchKeyID;
            listUnPreDispatchKeyD = tmplistUnPreDispatchKeyD;
        }

        public ResultModel ConfirmDispatch(string deliveryNo, string waybillno, int LPID, string[] disPatchedBox)
        {
            if (string.IsNullOrWhiteSpace(deliveryNo)) throw new ArgumentNullException("DeliveryNo");
            //waybillno可为空
            if (string.IsNullOrWhiteSpace(waybillno))
            {
                //  throw new ArgumentNullException("waybillno");
                waybillno = " ";
            }
            if (LPID < 1) throw new ArgumentNullException("LPID");
            if (null == disPatchedBox || disPatchedBox.Length < 1) throw new ArgumentNullException("disPatchedBox");

            //取得线路计划Model
            LinePlanModel lineplanModel = _lineplanDAL.GetLinePlan(LPID);
            if (null == lineplanModel)
            {
                return ErrorResult("所选择的线路不存在");
            }
            //已经存在运输调度的箱明细
            List<DispatchDetailModel> listExistedDispatchDetail = _dispatchDAL.DispatchBoxList(lineplanModel.DepartureID, disPatchedBox);
            //除去已经在事务里取消调度的箱子（更新时用）
            RebuildExistedDispatchDetail(deliveryNo, listExistedDispatchDetail);
            if (null != listExistedDispatchDetail && listExistedDispatchDetail.Count > 0)
            {
                return ErrorResult("所选择的箱子已经被运输调度");
            }
            //构建CarrierWaybillModel
            CarrierWaybillModel waybillModel = GetWaybillModel(waybillno);
            //构建提货单Model
            DispatchModel dispModel = GetDispatchModel(deliveryNo, waybillModel, lineplanModel);
            //构建提货单箱明细Model列表
            List<DispatchDetailModel> listdispDetailModel = GetDispatchDetailModelList(dispModel, disPatchedBox);
            // 获取提货单订单明细Model列表对象
            List<DispOrderDetailModel> listdispOrderDetailModel = GetDispOrderDetailModelList(dispModel, listdispDetailModel);
            //重建提货单箱明细数据
            ReBuildDispatchDetail(listdispOrderDetailModel, listdispDetailModel);
            //重建提货单数据
            ReBuildDispatch(listdispDetailModel, dispModel);
            //重建物流单对象
            waybillModel.TotalCount = listdispOrderDetailModel.Count;

            List<long> listPreDispatchKeyID = null;                              //使用预调度的KeyID
            List<long> listUnPreDispatchKeyD = null;                         //未使用预调度的箱号列表
            //根据是否预调度分隔
            SplitDispatchDetailByPreDispatch(listdispDetailModel, out listPreDispatchKeyID, out listUnPreDispatchKeyD);
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                _dispOrderDetailDAL.Add(listdispOrderDetailModel);
                _dispatchDetailDAL.Add(listdispDetailModel);
                _dispatchDAL.Add(dispModel);
                _carrierWaybillDAL.Add(waybillModel);
                if (listPreDispatchKeyID != null && listPreDispatchKeyID.Count > 0)
                {
                    _preDispatchDAL.UpdateToDispatched(listPreDispatchKeyID);
                }
                if (listUnPreDispatchKeyD != null && listUnPreDispatchKeyD.Count > 0)
                {
                    _preDispatchDAL.UpdateToInvalid(listUnPreDispatchKeyD);
                }
                //写日志
                WriteInsertLog<DispatchModel>(dispModel);
                scope.Complete();
            }

            return InfoResult(String.Format(@"提货单:{0} 运输调度成功", deliveryNo));
        }

        public ResultModel ComfirmUnplannedDispatch(string DeliveryNo, string waybillno, int LPID, string[] disPatchedBox)
        {
            return ConfirmDispatch(DeliveryNo, waybillno, LPID, disPatchedBox);
        }

        public ResultModel RejectDispatch(List<string> deliveryNos)
        {
            if (deliveryNos == null || deliveryNos.Count == 0)
            {
                throw new CodeNotValidException();
            }
            int iSuccess = 0;
            int iFailed = 0;
            int i = 0;
            foreach (string deliveryNo in deliveryNos)
            {
                i = RejectDispatch(deliveryNo);
                if (i > 0)
                {
                    iSuccess++;
                }
                else
                {
                    iFailed++;
                }
            }
            string strSuccessNoticeInfo = String.Format(@"置回成功{0}单！" + Environment.NewLine + "置回失败{1}单！", iSuccess, iFailed);
            return InfoResult(strSuccessNoticeInfo);
        }

        public ResultModel RejectDispatchEx(List<string> deliveryNos)
        {
            if (deliveryNos == null || deliveryNos.Count == 0)
            {
                throw new CodeNotValidException();
            }
            int iSuccess = 0;
            int iFailed = 0;
            int i = 0;
            foreach (string deliveryNo in deliveryNos)
            {
                i = RejectDispatchEx(deliveryNo);
                if (i > 0)
                {
                    iSuccess++;
                }
                else
                {
                    iFailed++;
                }
            }
            string strSuccessNoticeInfo = String.Format(@"置回成功{0}单！" + Environment.NewLine + "置回失败{1}单！", iSuccess, iFailed);
            return InfoResult(strSuccessNoticeInfo);
        }

        private int RejectDispatchEx(string deliveryNo)
        {
            if (string.IsNullOrWhiteSpace(deliveryNo))
            {
                throw new CodeNotValidException();
            }
            DispatchModel dispatchModel = _dispatchDAL.Get(deliveryNo);
            if (null == dispatchModel)
            {
                return 0;
            }
            DispatchModel newDispatchModel = VanclConverter.ConvertModel<DispatchModel, DispatchModel>(dispatchModel);
            newDispatchModel.IsDeleted = true;
            if (newDispatchModel.DeliveryStatus != Enums.DeliveryStatus.Dispatched)
            {
                return 0;
            }
            //撤回
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dispatchDAL.RejectDispatchEx(newDispatchModel.DID, Enums.DeliveryStatus.WaitForDispatch);
                WriteUpdateLog<DispatchModel>(newDispatchModel, dispatchModel);
                scope.Complete();
            }
            return i;
        }

        private int RejectDispatch(string deliveryNo)
        {
            if (string.IsNullOrWhiteSpace(deliveryNo))
            {
                throw new CodeNotValidException();
            }
            DispatchModel dispatchModel = _dispatchDAL.Get(deliveryNo);
            if (null == dispatchModel)
            {
                return 0;
            }
            dispatchModel.IsDeleted = true;
            if (dispatchModel.DeliveryStatus != Enums.DeliveryStatus.Dispatched)
            {
                return 0;
            }
            //取得提货单中已经采用预调度的预调度主键ID列表
            List<long> listPDID = _dispatchDAL.GetDispatchIsPlanedPDID(deliveryNo);
            //撤回
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                _dispOrderDetailDAL.Delete(deliveryNo);
                _dispatchDetailDAL.Delete(deliveryNo);
                _dispTransitionDAL.Delete(deliveryNo);
                i = _dispatchDAL.Delete(deliveryNo);
                if (null != listPDID && listPDID.Count > 0)
                {
                    _preDispatchDAL.UpdateToCanDispatch(listPDID);
                }
                WriteDeleteLog<DispatchModel>(dispatchModel);
                scope.Complete();
            }
            return i;
        }

        public ResultModel UpdateDeliveryStatus(string deliveryNO, Enums.DeliveryStatus deliveryStatus)
        {
            DispatchModel pastDispatch = _dispatchDAL.Get(deliveryNO);
            if (pastDispatch == null)
            {
                return ErrorResult("该提货单不存在或者已经被删除！");
            }
            int i = 0;
            DispatchModel nowDispatch = VanclConverter.ConvertModel<DispatchModel, DispatchModel>(pastDispatch);
            nowDispatch.DeliveryStatus = deliveryStatus;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dispatchDAL.UpdateDeliveryStatus(deliveryNO, deliveryStatus);
                WriteUpdateLog<DispatchModel>(nowDispatch, pastDispatch);
                scope.Complete();
            }
            return InfoResult("更新提货单状态成功！");
        }

        /// <summary>
        /// 确认提货单到货
        /// </summary>
        /// <param name="isConfirmLimited">是否限制确认到货</param>
        /// <param name="deliveryNO">提货单对象</param>
        /// <param name="deliveryStatus">提货单状态</param>
        /// <returns></returns>
        public ResultModel ConfirmDeliveryArrived(bool isConfirmLimited, DispatchModel dispModel, Enums.DeliveryStatus deliveryStatus)
        {
            DispatchModel pastDispatch = _dispatchDAL.Get(dispModel.DeliveryNo);
            if (pastDispatch == null)
            {
                return ErrorResult("该提货单不存在或者已经被删除！");
            }
            int i = 0;
            DispatchModel nowDispatch = VanclConverter.ConvertModel<DispatchModel, DispatchModel>(pastDispatch);
            nowDispatch.DeliveryStatus = deliveryStatus;
            nowDispatch.SignedUser = dispModel.SignedUser;

            dispModel.IsDelay = deliveryStatus == Enums.DeliveryStatus.ArrivedDelay;

            if (!isConfirmLimited)
            {
                nowDispatch.DesReceiveDate = dispModel.DesReceiveDate;
            }
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _dispatchDAL.ConfirmDeliveryArrived(isConfirmLimited, dispModel, deliveryStatus);
                WriteUpdateLog<DispatchModel>(nowDispatch, pastDispatch);
                scope.Complete();
            }
            return InfoResult("更新提货单状态成功！");
        }


        public DispatchModel Get(long did)
        {
            return _dispatchDAL.Get(did);
        }

        public DispatchModel Get(string deliveryNo)
        {
            return _dispatchDAL.Get(deliveryNo);
        }

        public ResultModel Update(string deliveryNo, string waybillno, int LPID, string[] disPatchedBox)
        {
            DispatchModel dispatchModel = _dispatchDAL.Get(deliveryNo);
            if (null == dispatchModel)
            {
                return ErrorResult("该提货单不存在或者已经被撤回！");
            }
            ResultModel rm = null;
            int i = 0;
            _isUpdate = true;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = RejectDispatch(deliveryNo);
                if (i > 0)
                {
                    WriteUpdateLog<DispatchModel>(dispatchModel, dispatchModel);
                    rm = ConfirmDispatch(deliveryNo, waybillno, LPID, disPatchedBox);
                    if (rm.IsSuccess)
                    {
                        scope.Complete();
                    }
                }
            }
            _isUpdate = false;
            if (rm == null)
            {
                rm = new ResultModel() { IsSuccess = false };
            }
            if (rm.IsSuccess)
            {
                return InfoResult(string.Format("提货单：{0}更新成功！", deliveryNo));
            }
            else
            {
                return ErrorResult(string.Format("提货单：{0}更新失败！", deliveryNo));
            }
        }

        public List<ViewDispatchBoxModel> GetDispatchedBoxList(string deliveryNo)
        {
            return _dispatchDAL.GetDispatchedBoxList(deliveryNo);
        }

        public PrintDeliveryNoModel GetPrintDeliveryInfo(string deliveryNo)
        {
            return _dispatchDAL.GetPrintDeliveryInfo(deliveryNo);
        }

        public IList<PrintDeliveryNoModel> GetPrintDeliveryInfo(IList<string> deliveryNoList)
        {
            return _dispatchDAL.GetPrintDeliveryInfo(deliveryNoList);
        }

        public List<ViewDispatchBoxModel> GetDispatchBoxList(int departureID, int arrivalID, Enums.GoodsType lineGoodsType)
        {
            return _dispatchDAL.GetDispatchBoxList(departureID, arrivalID, lineGoodsType);
        }

        public ResultModel UpdateWaybillNoByDeliveryNo(string waybillNo, string deliveryNo)
        {
            DispatchModel dispModel = _dispatchDAL.Get(deliveryNo);
            if (dispModel == null)
            {
                return ErrorResult("该提货单不存在或者已经被删除！");
            }
            CarrierWaybillModel cwPastModel = _carrierWaybillDAL.Get(dispModel.CarrierWaybillID);
            if (cwPastModel == null)
            {
                return ErrorResult("该运单信息不存在或者已经被删除！");
            }
            int i = 0;
            CarrierWaybillModel cwNowModel = VanclConverter.ConvertModel<CarrierWaybillModel, CarrierWaybillModel>(cwPastModel);
            cwNowModel.WaybillNo = waybillNo;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _carrierWaybillDAL.UpdateWaybillNo(waybillNo, dispModel.CarrierWaybillID);
                WriteUpdateLog<CarrierWaybillModel>(cwNowModel, cwPastModel);
                scope.Complete();
            }
            if (i > 0)
            {
                return InfoResult("修改运单号成功！");
            }
            else
            {
                return ErrorResult("修改运单号失败！");
            }
        }
        #endregion

        protected override int WriteDeleteLog<T>(T model)
        {
            if (!_isUpdate)
            {
                return base.WriteDeleteLog<T>(model);
            }
            else
            {
                return 1;
            }
        }

        protected override int WriteInsertLog<T>(T model)
        {
            if (!_isUpdate)
            {
                return base.WriteInsertLog<T>(model);
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 除去已经在事务里取消调度的箱子（更新时用）
        /// </summary>
        /// <param name="deliveryNo">提货单号</param>
        /// <param name="listExistedDispatchDetail">已经存在运输调度的箱明细</param>
        private void RebuildExistedDispatchDetail(string deliveryNo, List<DispatchDetailModel> listExistedDispatchDetail)
        {
            if (_isUpdate && listExistedDispatchDetail != null && listExistedDispatchDetail.Count > 0)
            {
                List<ViewDispatchBoxModel> lstDispatchedBox = _dispatchDAL.GetDispatchedBoxList(deliveryNo);
                if (lstDispatchedBox != null && lstDispatchedBox.Count > 0)
                {
                    foreach (ViewDispatchBoxModel model in lstDispatchedBox)
                    {
                        DispatchDetailModel ddm = listExistedDispatchDetail.Find(m => m.BoxNo == model.BoxNo);
                        listExistedDispatchDetail.Remove(ddm);
                    }
                }
            }
        }


        #region IDispatchBLL 成员


        /// <summary>
        /// 更新提货单为没有延误(复议审批通过后)
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <returns></returns>
        public ResultModel UpdateDeliveryToNoDelay(string deliveryNO)
        {
            if (String.IsNullOrWhiteSpace(deliveryNO)) throw new ArgumentNullException("DeliveryNo is null");
            ResultModel result = new ResultModel();
            result.IsSuccess = _dispatchDAL.UpdateDeliveryToNoDelay(deliveryNO) > 0;
            if (!result.IsSuccess)
            {
                result.Message = "更新提货单延误标志出错.";
            }
            return result;
        }

        /// <summary>
        /// 更新提货单为存在丢失(丢失审批通过后)
        /// </summary>
        /// <param name="deliveryNO">提货单号</param>
        /// <returns></returns>
        public ResultModel UpdateDeliveryToExistsLost(string deliveryNO)
        {
            if (String.IsNullOrWhiteSpace(deliveryNO)) throw new ArgumentNullException("DeliveryNo is null");
            ResultModel result = new ResultModel();
            result.IsSuccess = _dispatchDAL.UpdateDeliveryToExistsLost(deliveryNO) > 0;
            if (!result.IsSuccess)
            {
                result.Message = "更新提货单为存在丢失出错.";
            }
            return result;
        }

        #endregion

        #region IDispatchBLL 成员

        /// <summary>
        /// 获取线路货物类型
        /// </summary>
        /// <param name="enumDescription"></param>
        /// <returns></returns>
        private void TryGetLineGoodsType(string enumDescription, out string msg, out Enums.GoodsType? goodsType)
        {
            msg = string.Empty;
            goodsType = null;
            if (!string.IsNullOrWhiteSpace(enumDescription))
            {
                string[] goodstypes = enumDescription.Split(';');
                List<int> enumValues = new List<int>();
                foreach (var item in goodstypes)
                {
                    int enumValue = EnumHelper.GetEnumValue<Enums.GoodsType>(item);
                    if (enumValue > -1)
                    {
                        enumValues.Add(enumValue);
                    }
                    else
                    {
                        msg = "未能找到对应的货物品类:" + item + ";";
                        break;
                    }
                }

                if (string.IsNullOrWhiteSpace(msg))
                {
                    int temp = enumValues[0];
                    for (int i = 1; i < enumValues.Count; i++)
                    {
                        temp = temp | enumValues[i];
                    }

                    goodsType = (Enums.GoodsType)temp;
                }
            }
            else
            {
                msg = "货物类型不能为空";
            }
        }

        /// <summary>
        /// 获取运输方式
        /// </summary>
        /// <param name="enumDescription"></param>
        /// <param name="msg"></param>
        /// <param name="transportType"></param>
        private void TryGetTransportType(string enumDescription, out string msg, out Enums.TransportType? transportType)
        {
            msg = string.Empty;
            transportType = null;
            if (!string.IsNullOrWhiteSpace(enumDescription))
            {
                try
                {
                    transportType = EnumHelper.GetValue<Enums.TransportType>(enumDescription);
                }
                catch
                {
                    msg = "未能找到对应的运输方式:" + enumDescription;
                }
            }
            else
            {
                msg = "运输方式不能为空";
            }
        }

        public ResultModel Add(ImportTemplateModel model)
        {
            ResultModel r = new ResultModel();
            using (PmsService.PermissionOpenServiceClient client = new PmsService.PermissionOpenServiceClient())
            {
                try
                {
                    model.ArrivalID = client.GetExpressCompanyID(model.Arrival, null);
                    model.DepartrueID = client.GetExpressCompanyID(model.Departrue, UserContext.CurrentUser.DistributionCode);
                }
                catch (Exception e)
                {
                    r.IsSuccess = false;
                    r.Message = e.Message;
                }
            }

            CarrierBLL carrierBll = new CarrierBLL();
            model.CarrierID = carrierBll.GetCarrierIdByName(model.Carrier, UserContext.CurrentUser.DistributionCode);

            string getGoodsTypeMsg = string.Empty;
            Enums.GoodsType? goodsType = null;
            TryGetLineGoodsType(model.GoodsType, out getGoodsTypeMsg, out goodsType);

            string getTransportTypeMsg = string.Empty;
            Enums.TransportType? transportType = null;
            TryGetTransportType(model.TransportType, out getTransportTypeMsg, out transportType);

            if (model.CarrierID == -1)
            {
                r.IsSuccess = false;
                r.Message = "未能找到符合要求的承运商";
            }
            else if (model.ArrivalID == -1)
            {
                r.IsSuccess = false;
                r.Message = "未能找到符合要求的目的地";
            }
            else if (model.DepartrueID == -1)
            {
                r.IsSuccess = false;
                r.Message = "未能找到符合要求的始发地";
            }
            else if (!goodsType.HasValue)
            {
                r.IsSuccess = false;
                r.Message = getGoodsTypeMsg;
            }
            else if (!transportType.HasValue)
            {
                r.IsSuccess = false;
                r.Message = getTransportTypeMsg;
            }
            else
            {

                //新增承运商运单信息
                CarrierWaybillModel cwModel = new CarrierWaybillModel();
                cwModel.CWID = _carrierWaybillDAL.GetNextSequence(cwModel.SequenceName);
                cwModel.CreateBy = UserContext.CurrentUser.ID;
                cwModel.TotalCount = int.Parse(model.OrderCount);
                cwModel.WaybillNo = model.CustomerWaybillNo;
                cwModel.Weight = decimal.Parse(model.TotalWeight);

                LinePlanModel linePlan = transPlanDal.GetLinePlan(new TransportPlanSearchModel()
                {
                    DepartureID = model.DepartrueID,
                    ArrivalID = model.ArrivalID,
                    CarrierID = model.CarrierID,
                    GoodsType = goodsType.Value,
                    TransportType = transportType.Value
                });

                if (linePlan != null)
                {
                    DispatchModel dispModel = GetDispatchModelEx(" ", cwModel, linePlan);
                    dispModel.BoxCount = int.Parse(model.BoxCount);
                    dispModel.TotalAmount = decimal.Parse(model.TotalAmount);
                    dispModel.DeliverySource = model.Source.HasValue ? model.Source.Value : Enums.DeliverySource.Import;
                    //保价金额=总价x线路费率
                    dispModel.ProtectedPrice = dispModel.TotalAmount * linePlan.InsuranceRate;
                    dispModel.BatchNo = model.BatchNo;
                    using (IACID scope = ACIDScopeFactory.GetTmsACID())
                    {
                        if (_dispatchDAL.AddEx(dispModel) == 1)
                        {
                            _carrierWaybillDAL.AddEx(cwModel);
                            scope.Complete();

                            r.IsSuccess = true;
                        }
                        else
                        {
                            r.IsSuccess = false;
                            r.Message = "承运商运单号重复";
                        }
                    }
                }
                else
                {
                    r.IsSuccess = false;
                    r.Message = "未能找到符合要求的线路计划或者未设置运输计划:" + model.Departrue + "+" + model.Arrival + "+" + model.Carrier + "+" + model.GoodsType + "+" + model.TransportType;
                }
            }

            return r;
        }

        /// <summary>
        /// 获取订单数量,总重量,总价格
        /// </summary>
        /// <param name="details"></param>
        /// <param name="orderCount"></param>
        /// <param name="totalWeight"></param>
        /// <param name="totalAmount"></param>
        private void GetDeliveryDetailsInfo(List<ViewPreDispatchModel> details, out int orderCount, out decimal totalWeight)
        {
            orderCount = 0;
            totalWeight = 0;
            List<string> boxNos = new List<string>();
            if (details.Count > 0)
            {
                foreach (ViewPreDispatchModel item in details)
                {
                    if (item != null)
                    {
                        boxNos.Add(item.BatchNo);
                    }
                }
                //订单数量，重量以批次主表为准，总金额按照明细汇总
                List<BoxModel> boxs = _boxDAL.GetBatchInfo(boxNos.ToArray());
                orderCount = boxs.Sum(b => b.TotalCount);
                totalWeight = boxs.Sum(b => b.Weight);
            }
        }

        public ResultModel Add(ViewDispatchWithDetailsModel model, string batchNo, Enums.DeliverySource source)
        {
            ResultModel r = new ResultModel();
            int orderCount = 0;
            decimal totalWeight = 0;
            GetDeliveryDetailsInfo(model.Details, out orderCount, out totalWeight);
            CarrierWaybillModel carrierWaybillModel = new CarrierWaybillModel();
            carrierWaybillModel.CWID = _carrierWaybillDAL.GetNextSequence(carrierWaybillModel.SequenceName);
            carrierWaybillModel.CreateBy = UserContext.CurrentUser.ID;
            carrierWaybillModel.TotalCount = orderCount;
            carrierWaybillModel.WaybillNo = " ";
            carrierWaybillModel.Weight = totalWeight;

            DispatchModel dspmodel = new DispatchModel();
            LinePlanModel linePlan = _lineplanDAL.GetLinePlan(model.LPID);
            DispatchModel dispModel = GetDispatchModelEx(" ", carrierWaybillModel, linePlan);
            dispModel.BoxCount = model.Details.Count;
            dispModel.DeliverySource = source;
            dispModel.BatchNo = batchNo;

            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                r = AddToDispatchDetail(dispModel, model.Details);
                if (!r.IsSuccess)
                {
                    return r;
                }
                //保价金额=总价x线路费率
                dispModel.ProtectedPrice = dispModel.TotalAmount * linePlan.InsuranceRate;
                if (_dispatchDAL.AddEx(dispModel) == 1)
                {
                    _carrierWaybillDAL.AddEx(carrierWaybillModel);
                    scope.Complete();

                    r.IsSuccess = true;
                }
                else
                {
                    r.IsSuccess = false;
                    r.Message = "承运商运单号重复";
                }
            }
            return r;
        }

        private ResultModel AddToDispatchDetail(DispatchModel dispModel, List<ViewPreDispatchModel> details)
        {
            if (details == null || details.Count == 0) throw new Exception("提货单明细为空.");
            ResultModel result = new ResultModel();
            List<string> dispatchedBoxs = new List<string>();
            List<long> PDIDs = new List<long>();
            List<PreDispatchModel> listPreDispatch = new List<PreDispatchModel>();
            int SQNO = 0;
            foreach (ViewPreDispatchModel item in details)
            {
                if (item != null)
                {
                    PreDispatchModel preDispatch = new PreDispatchModel();
                    preDispatch.ArrivalID = item.ArrivalID;
                    preDispatch.BoxNo = item.BatchNo;
                    preDispatch.DepartureID = item.DepartureID;
                    preDispatch.DispatchStatus = Enums.DispatchStatus.CanDispatch;
                    preDispatch.LPID = item.LPID;
                    preDispatch.PDID = item.PDID;
                    preDispatch.TPID = item.TPID;
                    listPreDispatch.Add(preDispatch);
                    dispatchedBoxs.Add(item.BatchNo);
                    PDIDs.Add(item.PDID);
                }
            }

            PreDispatchModel preModel = _preDispatchDAL.GetPreDispatchModelByPDID(PDIDs[0]);

            List<PreDispatchModel> NextPreDispatch = _preDispatchDAL.GetNextPreDispatchListByBoxNos(dispatchedBoxs,
                                                                                                    preModel.SeqNo + 1);
            List<long> NextPDIDs = new List<long>();
            foreach (PreDispatchModel item in NextPreDispatch)
            {
                if (item != null)
                {
                    PreDispatchModel preDispatch = new PreDispatchModel();

                    NextPDIDs.Add(item.PDID);
                }
            }
            _preDispatchDAL.UpdateToDisabledDispatchV1(NextPDIDs, Enums.DispatchStatus.EstiDispatch);//将下一条数据改成可预调度5

            //已经存在运输调度的箱明细
            List<DispatchDetailModel> listExistedDispatchDetail = _dispatchDAL.DispatchBoxList(dispModel.DepartureID, dispatchedBoxs.ToArray());
            if (null != listExistedDispatchDetail && listExistedDispatchDetail.Count > 0)
            {
                return result.Failed("本次操作批次中存在已合并的批次号/箱号,请重置页面,重新操作.");
            }
            List<DispatchDetailModel> listdispDetailModel = GetDispatchDetailModelListEx(dispModel, dispatchedBoxs.ToArray(), listPreDispatch);
            List<DispOrderDetailModel> listdispOrderDetailModel = GetDispOrderDetailModelList(dispModel, listdispDetailModel);
            ReBuildDispatchDetail(listdispOrderDetailModel, listdispDetailModel);
            ReBuildDispatch(listdispDetailModel, dispModel);
            Dictionary<String, decimal> listBatchTotalAmount = new Dictionary<string, decimal>(listdispDetailModel.Count);
            listdispDetailModel.ForEach(p => { listBatchTotalAmount.Add(p.BoxNo, p.TotalAmount); });
            _boxDAL.UpdateBatchTotalAmount(listBatchTotalAmount);



            //_preDispatchDAL.UpdateToDisabledDispatch(PDIDs);
            _preDispatchDAL.UpdateToDisabledDispatchV1(PDIDs, Enums.DispatchStatus.EstiDispatching);//生成提货单的时候状态改成预可调度中


            _dispOrderDetailDAL.Add(listdispOrderDetailModel);
            _dispatchDetailDAL.Add(listdispDetailModel);
            return result.Succeed();
        }

        #endregion

        #region IDispatchBLL 成员


        public PagedList<ViewDispatchModel> SearchEx(DispatchSearchModel searchModel)
        {
            if (searchModel != null)
            {
                searchModel.OrderByString = "CreateTime";
                return _dispatchDAL.SearchEx(searchModel);
            }
            return null;
        }

        public ResultModel Delete(List<long> dids)
        {
            if (dids == null || dids.Count == 0)
            {
                throw new CodeNotValidException();
            }
            int iSuccess = 0;
            int iFailed = 0;
            int i = 0;
            foreach (long did in dids)
            {
                i = Delete(did);
                if (i > 0)
                {
                    iSuccess++;
                }
                else
                {
                    iFailed++;
                }
            }
            string strSuccessNoticeInfo = String.Format(@"删除成功{0}单！" + Environment.NewLine + "删除失败{1}单！", iSuccess, iFailed);
            return InfoResult(strSuccessNoticeInfo);
        }

        private int Delete(long did)
        {
            if (did == 0)
            {
                throw new CodeNotValidException();
            }
            DispatchModel dispatchModel = _dispatchDAL.Get(did);
            if (null == dispatchModel)
            {
                return 0;
            }
            dispatchModel.IsDeleted = true;
            if (dispatchModel.DeliveryStatus != Enums.DeliveryStatus.WaitForDispatch)
            {
                return 0;
            }
            List<DispatchDetailModel> disDetail = _dispatchDetailDAL.GetDispatchDetailByDID(did);
            List<string> dispatchedBoxs = new List<string>();

            if (disDetail != null && disDetail.Count > 0)
            {
                foreach (DispatchDetailModel item in disDetail)
                {
                    dispatchedBoxs.Add(item.BoxNo);
                }
            }

            //撤回
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                List<long> detailKeyList = _dispatchDAL.GetDispatchDetailKeyIDList(did);
                if (dispatchModel.DeliverySource != Enums.DeliverySource.Import)
                {
                    List<long> preDispatchKeyList = _dispatchDAL.GetDispatchIsPlanedPDID(dispatchModel.DID);

                    #region 执行将下一条数据更新为不可调度
                    PreDispatchModel preModel = _preDispatchDAL.GetPreDispatchModelByPDID(preDispatchKeyList[0]);

                    List<PreDispatchModel> NextPreDispatch = _preDispatchDAL.GetNextPreDispatchListByBoxNos(dispatchedBoxs,
                                                                                                            preModel.SeqNo + 1);
                    List<long> NextPDIDs = new List<long>();
                    foreach (PreDispatchModel item in NextPreDispatch)
                    {
                        if (item != null)
                        {
                            PreDispatchModel preDispatch = new PreDispatchModel();

                            NextPDIDs.Add(item.PDID);
                        }
                    }
                    _preDispatchDAL.UpdateToDisabledDispatchV1(NextPDIDs, Enums.DispatchStatus.CanNotDispatch);//将下一条数据改成可预调度5
                    #endregion

                    if (preDispatchKeyList != null && preDispatchKeyList.Count > 0)
                    {
                        //更新预调度表记录为"可调度"
                        _preDispatchDAL.UpdateToCanDispatchV1(preDispatchKeyList);


                    }
                    if (detailKeyList != null && detailKeyList.Count > 0)
                    {
                        //删除调度订单明细
                        _dispOrderDetailDAL.Delete(detailKeyList);
                    }
                    //删除调度明细
                    _dispatchDetailDAL.Delete(did);
                }
                i = _dispatchDAL.Delete(did);
                WriteDeleteLog<DispatchModel>(dispatchModel);
                scope.Complete();
            }
            return i;
        }

        public ResultModel ConfirmDispatchEx(string DeliveryNo, string waybillno, int LPID, long did)
        {
            if (did < 0) throw new ArgumentNullException("did");
            if (string.IsNullOrWhiteSpace(DeliveryNo)) throw new ArgumentNullException("DeliveryNo");
            //waybillno可为空
            if (string.IsNullOrWhiteSpace(waybillno))
            {
                //  throw new ArgumentNullException("waybillno");
                waybillno = " ";
            }
            if (LPID < 1) throw new ArgumentNullException("LPID");

            //取得线路计划Model
            LinePlanModel lineplanModel = _lineplanDAL.GetLinePlan(LPID);
            if (null == lineplanModel)
            {
                return ErrorResult("所选择的线路不存在");
            }
            //构建提货单Model
            DispatchModel dispModel = _dispatchDAL.Get(did);
            dispModel.DeliveryNo = DeliveryNo;
            dispModel.LPID = LPID;
            dispModel.UpdateBy = UserContext.CurrentUser.ID;
            dispModel.DeliveryStatus = Enums.DeliveryStatus.Dispatched;
            dispModel.CarrierID = lineplanModel.CarrierID;
            dispModel.TransportType = lineplanModel.TransportType;
            dispModel.LineGoodsType = lineplanModel.LineGoodsType;
            //保价金额=总价x线路保险费率
            dispModel.ProtectedPrice = dispModel.TotalAmount * lineplanModel.InsuranceRate;
            //更新调度时间为当前时间
            dispModel.DispatchTime = DateTime.Now;
            List<long> detailKeyList = _dispatchDAL.GetDispatchDetailKeyIDList(did);
            PreDispatchPublicQueryModel QueryModel = new PreDispatchPublicQueryModel();
            QueryModel.DID = did;
            QueryModel.Status = Enums.DispatchStatus.EstiDispatching;
            //
            List<PreDispatchModel> preList = _preDispatchDAL.GetModelByDispatch(QueryModel);
            if (null == preList && preList.Count == 0)
            {
                return ErrorResult("所选择的线路不存在");
            }
            List<long> firstPDIDs = new List<long>();
            List<string> BoxNos = new List<string>();

            if (preList.Count > 0)
            {
                foreach (PreDispatchModel premodel in preList)
                {
                    firstPDIDs.Add(premodel.PDID);
                    BoxNos.Add(premodel.BoxNo);
                }
            }


            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                if (dispModel.DeliverySource != Enums.DeliverySource.Import)
                {
                    _dispOrderDetailDAL.UpdateBy_ConfirmDispatch(detailKeyList, dispModel.DeliveryNo);
                    _dispatchDetailDAL.UpdateBy_ConfirmDispatch(did, dispModel.DeliveryNo);
                }
                _dispatchDAL.UpdateEx(dispModel);
                _carrierWaybillDAL.UpdateWaybillNo(waybillno, dispModel.CarrierWaybillID);


                _preDispatchDAL.UpdateToDisabledDispatchV1(firstPDIDs, Enums.DispatchStatus.Dispatching);
                // _preDispatchDAL.UpdateToDisabledDispatchV1();
                //写日志
                WriteInsertLog<DispatchModel>(dispModel);
                scope.Complete();
            }

            return InfoResult(String.Format(@"提货单:{0} 运输调度成功", DeliveryNo));
        }


        public ResultModel UpdateTotalAmountByID(long did, decimal totalAmount, decimal protectedPrice)
        {
            ResultModel r = new ResultModel();
            if (_dispatchDAL.UpdateTotalAmountByID(did, totalAmount, protectedPrice) == 1)
                r.Succeed();
            else
                r.Failed("更新保价金额失败.");

            return r;
        }

        #endregion
    }
}
