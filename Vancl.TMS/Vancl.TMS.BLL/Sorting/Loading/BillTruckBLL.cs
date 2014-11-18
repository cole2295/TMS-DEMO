using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Sorting.Loading;
using Vancl.TMS.IDAL.Sorting.Loading;
using Vancl.TMS.IBLL.Sorting.Loading;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.BLL.Sorting.Loading
{
    public class BillTruckBLL : BaseBLL, IBillTruckBLL
    {
        IBillTruckDAL _dal = ServiceFactory.GetService<IBillTruckDAL>("BillTruckDAL");
        IExpressCompanyDAL _expressCompanyDal = ServiceFactory.GetService<IExpressCompanyDAL>("ExpressCompanyDAL");
        IDistributionDAL _distributionDal = ServiceFactory.GetService<IDistributionDAL>("DistributionDAL");
        IBillDAL _billDal = ServiceFactory.GetService<IBillDAL>("BillDAL");
        IFormula<String, KeyCodeContextModel> KeyCodeGenerator = FormulasFactory.GetFormula<IFormula<String, KeyCodeContextModel>>("keycodeBLLFormula");

        #region IBillTruckBLL成员

        public PagedList<BillTruckBatchModel> GetBillTruckList(BillTruckSearchModel searchModel)
        {
            return _dal.GetBillTruckList(searchModel);
        }

        public IList<ViewBillTruckModel> GetOutbondByBatch(BillTruckSearchModel searchModel)
        {
            IList<ViewBillTruckModel> list = _dal.GetOutbondByBatch(searchModel);
            if (list != null)
                return list.OrderBy(m => m.FormCode).ToList();
            return null;
        }

        #endregion

        #region 订单装车

        /// <summary>
        /// 按批次号列表装车
        /// </summary>
        /// <param name="batchNoList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel LoadingByBatchNoList(List<string> batchNoList, BillTruckModel model)
        {
            var rm = new ResultModel();
            try
            {
                var totalSuccessCount = 0;
                foreach (string batchNo in batchNoList)
                {
                    model.BatchNO = batchNo;
                    IList<OutBoundLoadingModel> outBoundList = _dal.GetNotLoadingBill(batchNo);
                    var successCount = 0;
                    BatchLoading(outBoundList, model, out successCount);
                    totalSuccessCount += successCount;
                }
                rm.IsSuccess = true;
                rm.Message = "装车成功" + totalSuccessCount.ToString() + "条！";
            }
            catch (Exception ex)
            {
                rm.IsSuccess = false;
                rm.Message = ex.Message;
            }
            return rm;
        }

        /// <summary>
        /// 按运单列表装车
        /// </summary>
        /// <param name="formCodeList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel LoadingByFormCodeList(List<string> formCodeList, BillTruckModel model)
        {
            var rm = new ResultModel();
            try
            {
                var successCount = 0;
                IList<OutBoundLoadingModel> outBoundList = _dal.GetNotLoadingBill(model.BatchNO, formCodeList);
                BatchLoading(outBoundList, model, out successCount);
                rm.IsSuccess = true;
                rm.Message = "装车成功" + successCount.ToString() + "条！";
            }
            catch (Exception ex)
            {
                rm.IsSuccess = false;
                rm.Message = ex.Message;
            }
            return rm;
        }

        /// <summary>
        /// 批量装车
        /// </summary>
        /// <param name="list"></param>
        /// <param name="model"></param>
        private void BatchLoading(IList<OutBoundLoadingModel> outBoundList, BillTruckModel model, out int successCount)
        {
            successCount = 0;
            if (outBoundList != null && outBoundList.Count > 0)
            {
                #region 对于发往配送商的状态逻辑
                Enums.BillStatus? preStatus = null;
                #endregion

                foreach (OutBoundLoadingModel outBountBill in outBoundList)
                {
                    model.FormCode = outBountBill.FormCode;

                    var expressCompanyModel = _expressCompanyDal.GetModel(outBountBill.ArrivalID);
                    outBountBill.CurrentDistributionCode = expressCompanyModel.DistributionCode;

                    #region 对于发往配送商的状态逻辑
                    if (preStatus == null && outBountBill.OutboundType == Enums.SortCenterOperateType.DistributionSorting)
                    {
                        if (_distributionDal.ExistsSortCenter(expressCompanyModel.DistributionCode))
                        {
                            preStatus = _distributionDal.GetDistributionPreBillStatus(expressCompanyModel.DistributionCode, Enums.BillStatus.HaveBeenSorting);
                        }
                        else
                        {
                            preStatus = _distributionDal.GetDistributionPreBillStatus(expressCompanyModel.DistributionCode, Enums.BillStatus.InStation);
                        }
                        //TODO:北京东明走特殊逻辑，TMS后续等实现总子公司后需要读取PMS的配置
                        //if (outBountBill.ArrivalID == 158)
                        //{
                        //    preStatus = Enums.BillStatus.InTransit;
                        //}
                        if (preStatus == null)
                        {
                            throw new Exception("配送商业务配置未定义！");
                        }
                        else
                        {
                            outBountBill.CurBillStatus = (Enums.BillStatus)preStatus;
                        }
                    }
                    else
                    {
                        outBountBill.CurBillStatus = preStatus == null ? Enums.BillStatus.InTransit : (Enums.BillStatus)preStatus;
                    }
                    #endregion

                    if (IsTruckValid(outBountBill, model))
                    {
                        if (LoadingBusiness(outBountBill, model))
                        {
                            successCount++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 订单装车事务
        /// </summary>
        /// <param name="model"></param>
        private bool LoadingBusiness(OutBoundLoadingModel outBountBill, BillTruckModel model)
        {
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = AddBillTruck(model);
                BillModel billModel = new BillModel()
                {
                    FormCode = model.FormCode,
                    CurrentDistributionCode = outBountBill.CurrentDistributionCode,
                    Status = outBountBill.CurBillStatus,
                    UpdateTime = DateTime.Now
                };
                i = _billDal.UpdateWaybillByTruck(billModel);
                i = WriteLoadingLog(outBountBill, model);
                scope.Complete();
            }
            return i > 0 ? true : false;
        }

        #region 订单装车私有方法

        /// <summary>
        /// 判断运单是否可以进行装车
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private bool IsTruckValid(OutBoundLoadingModel outBountBill, BillTruckModel model)
        {
            //订单状态不为已出库状态则无法进行装车
            if (outBountBill.BillStatus != Enums.BillStatus.Outbounded && outBountBill.BillStatus != Enums.BillStatus.WaitingInbound)
            {
                return false;
            }

            if (Consts.ISCHECKLMSSTATUSFORWAYBILLLOADING)
            {
                //如果在LMS已经操作装车则无法进行装车
                using (CloudBillService.BillServiceClient billProxy = new CloudBillService.BillServiceClient())
                {
                    if (billProxy.IsWaybillLoading(long.Parse(model.FormCode), model.BatchNO))
                    {
                        return false;
                    }
                }
            }

            //如果已经装车则无法进行装车
            if (_dal.IsExistBillTruck(model))
            {
                return false;
            }
            return true;
        }

        private int AddBillTruck(BillTruckModel model)
        {
            model.BTID = KeyCodeGenerator.Execute(
                          new KeyCodeContextModel()
                          {
                              SequenceName = model.SequenceName,
                              TableCode = model.TableCode
                          });
            return _dal.addWaybillTruck(model);
        }


        /// <summary>
        /// 记录运单上车日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private int WriteLoadingLog(OutBoundLoadingModel outBountBill, BillTruckModel model)
        {
            var billchangelogDynamicModel = new Model.Log.BillChangeLogDynamicModel()
            {
                FormCode = model.FormCode,
                CurrentDistributionCode = outBountBill.CurrentDistributionCode,
                DeliverStationID = outBountBill.DeliverStationID,
                CurrentSatus = outBountBill.CurBillStatus,
                PreStatus = outBountBill.BillStatus,
                CreateBy = model.CreateBy,
                CreateDept = model.CreateDept
            };
            if (outBountBill.OutboundType == Enums.SortCenterOperateType.DistributionSorting)
            {
                billchangelogDynamicModel.OperateType = Enums.TmsOperateType.DistributionLoading;
            }
            else
            {
                billchangelogDynamicModel.OperateType = Enums.TmsOperateType.BillLoading;
            }

            var expressCompanyModel = _expressCompanyDal.GetModel(outBountBill.DeliverStationID);

            billchangelogDynamicModel.ExtendedObj.OpStationMnemonicName = model.OpStationName;
            billchangelogDynamicModel.ExtendedObj.DeliverStationMnemonicName = expressCompanyModel.MnemonicName;
            return WriteBillChangeLog(billchangelogDynamicModel);
        }

        #endregion

        #endregion

        #region 订单下车

        public ResultModel RemovBillTruck(List<string> formCodeList, BillTruckModel model)
        {
            ResultModel result = new ResultModel();
            IList<ViewBillTruckModel> billTruckList = _dal.GetLoadingBill(model.BatchNO, formCodeList);
            if (billTruckList != null && billTruckList.Count > 0)
            {
                var removeCount = 0;
                foreach (ViewBillTruckModel loadingObj in billTruckList)
                {
                    if (loadingObj != null)
                    {
                        //只有运输在途的车辆才可以下车
                        if (loadingObj.BillStatus != Enums.BillStatus.InTransit)
                        {
                            continue;
                        }
                        using (IACID scope = ACIDScopeFactory.GetTmsACID())
                        {
                            model.FormCode = loadingObj.FormCode;
                            loadingObj.CurBillStatus = Enums.BillStatus.Outbounded;
                            _dal.RemoveBillTruck(model);
                            _billDal.UpdateBillStatus(loadingObj.FormCode, Enums.BillStatus.Outbounded);
                            WriteRemoveBillTruckLog(loadingObj, model);
                            scope.Complete();
                            removeCount++;
                        }
                    }
                }
                if (removeCount == 0)
                {
                    result.IsSuccess = false;
                    result.Message = "只有运输在途的车辆才可以下车";
                }
                else
                {
                    result.IsSuccess = true;
                    result.Message = "下车成功";
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "你选择的订单还未装车！";
            }
            return result;
        }

        /// <summary>
        /// 记录运单下车日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private int WriteRemoveBillTruckLog(ViewBillTruckModel viewBillTruck, BillTruckModel model)
        {
            var billchangelogDynamicModel = new Model.Log.BillChangeLogDynamicModel()
            {
                FormCode = viewBillTruck.FormCode,
                OperateType = Enums.TmsOperateType.BillGetOff,
                CurrentDistributionCode = viewBillTruck.CurrentDistributionCode,
                DeliverStationID = viewBillTruck.DeliverStationID,
                CurrentSatus = viewBillTruck.CurBillStatus,
                PreStatus = viewBillTruck.BillStatus,
                CreateBy = model.CreateBy,
                CreateDept = model.CreateDept
            };
            billchangelogDynamicModel.ExtendedObj.FormCode = viewBillTruck.FormCode;
            billchangelogDynamicModel.ExtendedObj.TruckNo = viewBillTruck.TruckNo;
            return WriteBillChangeLog(billchangelogDynamicModel);
        }

        #endregion


    }
}
