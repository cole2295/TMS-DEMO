using System;
using System.Collections.Generic;
using System.Linq;
using Vancl.TMS.IBLL.Sorting.BillPrint;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.Security;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Model.Log;
using Vancl.TMS.BLL.CloudBillService;
using Vancl.TMS.IBLL.BaseInfo;
namespace Vancl.TMS.BLL.Sorting.BillPrint
{
    public class BillPrintBLL : SortCenterBLL, IBillPrintBLL
    {
        IMerchantBLL MerchantBLL = ServiceFactory.GetService<IMerchantBLL>();
        IBillDALForBillPrint BillDALForBillPrint = ServiceFactory.GetService<IBillDALForBillPrint>();
        IBillDAL BillDAL = ServiceFactory.GetService<IBillDAL>();
        IBillInfoDAL BillInfoDAL = ServiceFactory.GetService<IBillInfoDAL>();
        IBillWeighDAL BillWeighDAL = ServiceFactory.GetService<IBillWeighDAL>();
        IDistributionDAL DistributionDAL = ServiceFactory.GetService<IDistributionDAL>();
        IServiceLogBLL ServiceLogBLL = ServiceFactory.GetService<IServiceLogBLL>();

        private BillScanWeightArgModel BillScanWeightModel { get; set; }

        #region  变量
        private string _formCode;
        public string FormCode
        {
            set
            {
                if (_formCode != value)
                {
                    _printBillInfo = null;
                    _billInfoModel = null;
                    _billWeightList = null;
                }
                _formCode = value;
            }
            get
            {
                if (string.IsNullOrWhiteSpace(_formCode)) throw new Exception("调用方法前需先设置FormCode的值。");
                return _formCode;
            }
        }

        private PrintBillModel _printBillInfo = null;
        private PrintBillModel PrintBillInfo
        {
            get
            {
                if (_printBillInfo == null)
                {
                    if (string.IsNullOrWhiteSpace(_formCode))
                    {
                        return null;
                    }
                    _printBillInfo = BillDALForBillPrint.GetBillByFormCode(FormCode);
                }
                return _printBillInfo;
            }
        }

        private BillInfoModel _billInfoModel = null;
        private BillInfoModel BillInfoModel
        {
            get
            {
                if (_billInfoModel == null)
                {
                    _billInfoModel = BillInfoDAL.GetBillInfoByFormCode(FormCode);
                }
                return _billInfoModel;
            }
        }

        private IList<BillPackageModel> _billWeightList = null;
        private IList<BillPackageModel> BillWeightList
        {
            get
            {
                if (_billWeightList == null)
                {
                    _billWeightList = BillWeighDAL.GetListByFormCode(FormCode) ?? new List<BillPackageModel>();
                }
                return _billWeightList;
            }
        }
        #endregion

        /// <summary>
        /// 扫描称重
        /// </summary>
        /// <param name="argModel"></param>
        /// <returns></returns>
        public BillPageViewModel ScanWeight(BillScanWeightArgModel argModel)
        {
            BillPageViewModel billPageData = new BillPageViewModel();
            billPageData.ScanResult = ScanResult.Scuccess;
            billPageData.FormCode = argModel.FormCode;
            try
            {
                this._formCode = argModel.FormCode;
                this.BillScanWeightModel = argModel;

                if (PrintBillInfo == null) return BillPageViewModel.Create(ScanResult.Failed, "未能查到此单号数据。");

                var rm = Check();
                if (!rm.IsSuccess)
                {
                    return SetBillPageData(ScanResult.Failed, rm.Message, billPageData);
                }

                //箱数
                var PackageCount = BillInfoModel.PackageCount <= 0 ? 1 : BillInfoModel.PackageCount;

                billPageData.TotalPackageCount = PackageCount;

                if (BillWeightList.Count >= PackageCount && PrintBillInfo.Status != Enums.BillStatus.GisAssigned)
                {//重新分配后可修改重量
                    return SetBillPageData(ScanResult.Failed, "箱数不正确,该单所有箱子已称重", billPageData);
                }

                //添加重量
                var newBillWeigh = new BillPackageModel()
                {
                    FormCode = PrintBillInfo.FormCode,
                    PackageIndex = BillWeightList.Count + 1,
                    Weight = argModel.BillWeight,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    UpdateTime = DateTime.Now,
                    CreateBy = UserContext.CurrentUser.ID,
                    UpdateBy = UserContext.CurrentUser.ID,
                    SyncFlag = Enums.SyncStatus.NotYet,
                };
                //   newBillWeigh.BWID = BillWeighDAL.GetNextSequence(newBillWeigh.SequenceName);

                //总称重重量
                decimal currentWeightSum = BillWeightList.Sum(item => item.Weight) ?? 0.000m;
                BillWeightList.Add(newBillWeigh);

                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    try
                    {
                        if (PackageCount >= BillWeightList.Count)
                        {//添加箱重                            
                            BillWeighDAL.Add(newBillWeigh);
                            currentWeightSum += newBillWeigh.Weight ?? 0;
                            //   currentWeightSum = BillWeightList.Sum(item => item.Weight) ?? 0.000m;
                        }
                        else
                        {
                            //更新箱重                      
                            BillWeighDAL.UpdateWeight(PrintBillInfo.FormCode, BillWeightList.Count - 1, newBillWeigh.Weight ?? 0);
                            newBillWeigh.PackageIndex = newBillWeigh.PackageIndex - 1;
                            currentWeightSum = BillWeightList.Where(x => x.PackageIndex != BillWeightList.Count - 1).Sum(item => item.Weight ?? 0) + newBillWeigh.Weight ?? 0;
                        }
                        billPageData.TotalWeight = currentWeightSum;
                        billPageData.CurrentPackageIndex = newBillWeigh.PackageIndex;
                        //更新总重量
                        BillInfoDAL.UpdateBillInfoWeight(PrintBillInfo.FormCode, currentWeightSum);
                        var logModel = new BillChangeLogDynamicModel
                            {
                                CreateBy = UserContext.CurrentUser.ID,
                                CreateDept = UserContext.CurrentUser.DeptID,
                                CurrentDistributionCode = PrintBillInfo.CurrentDistributionCode,
                                CurrentSatus = PrintBillInfo.Status,
                                DeliverStationID = PrintBillInfo.DeliverStationID,
                                FormCode = argModel.FormCode,
                                OperateType = Enums.TmsOperateType.WeighPrint,
                                PreStatus = PrintBillInfo.Status,
                            };

                        //日志扩展属性
                        logModel.ExtendedObj.IsPrint = false;
                        logModel.ExtendedObj.Weight = newBillWeigh.Weight;
                        logModel.ExtendedObj.PackageIndex = newBillWeigh.PackageIndex;
                        logModel.ExtendedObj.PackageCount = BillInfoModel.PackageCount;

                        //需要打印面单
                        if (!argModel.IsSkipPrintBill)
                        {
                            logModel.ExtendedObj.IsPrint = true;
                            //由已分配站点状态改为待入库
                            if (UpdateBillStatus(Enums.BillStatus.GisAssigned, Enums.BillStatus.WaitingInbound))
                            {
                                //    logModel.PreStatus = Enums.BillStatus.GisAssigned;
                                logModel.CurrentSatus = Enums.BillStatus.WaitingInbound;
                            }
                        }
                        WriteBillChangeLog(logModel);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        //todo: 将ex通过邮件通知开发者
                        return SetBillPageData(ScanResult.Failed, "添加重量失败", billPageData);
                        // return ResultModel.Create(false, "添加重量失败。", ex);
                    }
                }

                //需要复核称重
                if (argModel.IsNeedWeightReview)
                {
                    var reviewModel = WeighingReview(argModel.MerchantID);
                    billPageData.IsReviewWeightAbnormal = reviewModel.IsAbnormal;
                    if (reviewModel.IsAbnormal)
                    {
                        billPageData.ScanResult = ScanResult.Warming;
                    }
                    billPageData.Message = reviewModel.Message;
                }

                if (billPageData.ScanResult == ScanResult.Scuccess && string.IsNullOrWhiteSpace(billPageData.Message))
                {
                    billPageData.Message = "已成功扫描称重";
                }
                return SetBillPageData(billPageData.ScanResult, billPageData.Message, billPageData);

            }
            catch (Exception ex)
            {
                return SetBillPageData(ScanResult.Failed, "添加重量失败!异常：" + ex.Message, billPageData);
            }
        }

        private BillPageViewModel SetBillPageData(ScanResult scanResult, string message, BillPageViewModel billPageData)
        {
            billPageData.ScanResult = scanResult;
            billPageData.Message = message;
            //获取箱子
            billPageData.PackageInfo = GetBillPackage().Select(x => new BillPackageInfo
            {
                PackageIndex = x.PackageIndex,
                ScanTime = x.UpdateTime,
                Weight = x.Weight ?? 0,
            }).OrderBy(x => x.PackageIndex);
            //总箱数
            billPageData.TotalPackageCount = BillInfoModel.PackageCount == 0 ? 1 : BillInfoModel.PackageCount;
            //当前箱号
            // billPageData.CurrentPackageIndex = billPageData.PackageInfo.Count();
            //总重量
            billPageData.TotalWeight = billPageData.PackageInfo.Sum(x => x.Weight);
            billPageData.MerchantWeight = BillInfoModel.CustomerWeight;
            billPageData.BillStatus = EnumHelper.GetDescription(PrintBillInfo.Status);
            billPageData.CustomerOrder = PrintBillInfo.CustomerOrder;
            billPageData.StationId = PrintBillInfo.DeliverStationID;
            billPageData.StationName = PrintBillInfo.DeliverStationName;
            if (PrintBillInfo.Source == Enums.BillSource.Others)
                billPageData.BillSource = PrintBillInfo.MerchantName;
            else
                billPageData.BillSource = EnumHelper.GetDescription(PrintBillInfo.Source);
            billPageData.BillType = EnumHelper.GetDescription(PrintBillInfo.BillType);
            //  PrintBillInfo.DistributionCode = PrintBillInfo.DistributionCode;
            billPageData.SiteNo = PrintBillInfo.SiteNo;
            billPageData.CompanyFlag = PrintBillInfo.CompanyFlag;
            billPageData.CompanyName = PrintBillInfo.CompanyName;

            return billPageData;
        }

        /// <summary>
        /// 运单检测  
        /// </summary>
        /// <returns></returns>
        private ResultModel Check()
        {
            //属性检查
            if (BillScanWeightModel == null) return ResultModel.Create(false, "属性BillScanWeightModel值不能为空。");
            if (PrintBillInfo.MerchantID != this.BillScanWeightModel.MerchantID) return ResultModel.Create(false, "该单与所选商家不符。");

            ResultModel rm = new ResultModel();
            rm.IsSuccess = true;

            if (PrintBillInfo.BillType == Model.Common.Enums.BillType.Return)
            {
                rm.IsSuccess = false;
                rm.Message = "此单为上门退货单";
                return rm;
            }

            Enums.BillStatus? preStatus = null;
            //状态检查
            if (!string.IsNullOrWhiteSpace(PrintBillInfo.DistributionCode))
            {
                preStatus = DistributionDAL.GetDistributionPreBillStatus(PrintBillInfo.DistributionCode, Enums.BillStatus.WaitingInbound);
            }
            //PrintBillInfo.Status == preStatus
            //不能为 退货单&& (待入分拣中心||已分配站点||前一状态)
            if (PrintBillInfo.Status == Enums.BillStatus.WaitingInbound || PrintBillInfo.Status == Enums.BillStatus.GisAssigned || PrintBillInfo.Status == preStatus)
            {
                rm.IsSuccess = true;
            }
            else
            {
                rm.IsSuccess = false;
                rm.Message = "运单状态错误，当前状态为:" + EnumHelper.GetDescription(PrintBillInfo.Status);
                return rm;
            }

            return rm;
        }


        private ResultModel CheckNew()
        {
            ResultModel rm = new ResultModel();
            rm.IsSuccess = true;

            if (PrintBillInfo.BillType == Model.Common.Enums.BillType.Return)
            {
                rm.IsSuccess = false;
                rm.Message = "此单为上门退货单";
                return rm;
            }
            return rm;

        }

        /// <summary>
        /// 复核称重
        /// </summary>
        /// <returns></returns>
        private WeighingReviewModel WeighingReview(long merchantId)
        {
            this._formCode = FormCode;
            WeighingReviewModel model = new WeighingReviewModel();
            var merModel = MerchantBLL.GetByID(merchantId);
            if (!merModel.IsCheckWeight)
            {
                model.IsAbnormal = false;
                return model;
            }
            if (Math.Abs(this.BillWeightList.Sum(x => x.Weight ?? 0) - BillInfoModel.CustomerWeight) > merModel.CheckWeight)
            {
                //重量异常
                model.IsAbnormal = true;
                model.Message = "已记录重量，此单重量称重复核异常";
            }
            else
            {
                //重量正常
                model.IsAbnormal = false;
                model.Message = "已记录重量，此单重量称重复核正常";
            }
            return model;
        }

        /// <summary>
        /// 更新运单状态
        /// </summary>
        /// <param name="prevStatus"></param>
        /// <param name="currStatus"></param>
        /// <returns></returns>
        private bool UpdateBillStatus(Enums.BillStatus prevStatus, Enums.BillStatus currStatus)
        {
            return BillDALForBillPrint.UpdateBillStatus(FormCode, prevStatus, currStatus) > 0;
        }

        /// <summary>
        /// 获取箱子
        /// </summary>
        /// <returns></returns>
        private IEnumerable<BillPackageModel> GetBillPackage()
        {
            var list = BillWeighDAL.GetListByFormCode(FormCode) ?? new List<BillPackageModel>();
            return list.OrderBy(x => x.PackageIndex);
        }



        public BillPrintViewModel GetPrintData(string formCode, int packageIndex)
        {
            if (string.IsNullOrWhiteSpace(formCode)) throw new ArgumentNullException();

            this.FormCode = formCode;

            BillPrintViewModel printModel = new BillPrintViewModel();

            //从lms中获取TakeSendInfo
            WaybillTakeSendInfo wayBillTakeSendInfo;
            using (BillServiceClient client = new BillServiceClient())
            {
                try
                {
                    wayBillTakeSendInfo = client.GetWaybillTakeSendInfo(long.Parse(FormCode));
                    client.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("从LMS获取运单TakeSendInfo数据失败", ex);
                }
            }

            if (wayBillTakeSendInfo == null)
            {
                wayBillTakeSendInfo = new WaybillTakeSendInfo();
                // throw new Exception("未能从LMS获取运单TakeSendInfo数据");
            }

            printModel = BillDALForBillPrint.GetBillPrintMenuData(PrintBillInfo.FormCode, PrintBillInfo.MerchantID, packageIndex, wayBillTakeSendInfo.ReceiveArea);
            if (printModel == null) throw new Exception("不能打印运单，运单状态不正确！");
            printModel.ReceiveBy = wayBillTakeSendInfo.ReceiveBy;
            printModel.ReceiveTel = wayBillTakeSendInfo.ReceiveMobile + "/" + wayBillTakeSendInfo.ReceiveTel;
            printModel.ReceiveProvince = wayBillTakeSendInfo.ReceiveProvince;
            printModel.ReceiveCity = wayBillTakeSendInfo.ReceiveCity;
            printModel.ReceiveArea = wayBillTakeSendInfo.ReceiveArea;
            printModel.ReceiveAddress = wayBillTakeSendInfo.ReceiveAddress;
            printModel.SendTime = wayBillTakeSendInfo.SendTimeType;
            printModel.ReceiveMobile = wayBillTakeSendInfo.ReceiveMobile;
            printModel.PrintTime = DateTime.Now.ToString("yyyy年MM月dd日");
            printModel.LineCode = String.Empty;

            return printModel;
        }


        public PrintBillNewModel GetPrintData(string formCode)
        {
            if (string.IsNullOrWhiteSpace(formCode)) throw new ArgumentNullException();

            this.FormCode = formCode;

            PrintBillNewModel printDataModel = new PrintBillNewModel();

            //从lms中获取TakeSendInfo
            WaybillTakeSendInfo wayBillTakeSendInfo;
            using (BillServiceClient client = new BillServiceClient())
            {
                try
                {
                    wayBillTakeSendInfo = client.GetWaybillTakeSendInfo(long.Parse(FormCode));
                    client.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("从LMS获取运单TakeSendInfo数据失败", ex);
                }
            }

            if (wayBillTakeSendInfo == null)
            {
                wayBillTakeSendInfo = new WaybillTakeSendInfo();
                // throw new Exception("未能从LMS获取运单TakeSendInfo数据");
            }

            var printModel = BillDALForBillPrint.GetBillPrintMenuData(PrintBillInfo.FormCode, PrintBillInfo.MerchantID, 0, wayBillTakeSendInfo.ReceiveArea);
            if (printModel == null) throw new Exception("不能打印运单，运单状态不正确！");

            printDataModel.setValue("总数",printModel.TotalAmount.ToString());
            printDataModel.setValue("订单来源",printModel.MerchantName);
            printDataModel.setValue("订单号",formCode);
            printDataModel.setValue("订单类型",printModel.BillTypeDescription);
            printDataModel.setValue("收件人", wayBillTakeSendInfo.ReceiveBy);
            printDataModel.setValue("支付方式", printModel.PayTypeDescription);
            printDataModel.setValue("手机",wayBillTakeSendInfo.ReceiveMobile);
            printDataModel.setValue("省", wayBillTakeSendInfo.ReceiveProvince);
            printDataModel.setValue("市", wayBillTakeSendInfo.ReceiveCity);
            printDataModel.setValue("(区)县", wayBillTakeSendInfo.ReceiveArea);
            printDataModel.setValue("地址", wayBillTakeSendInfo.ReceiveAddress);
            printDataModel.setValue("邮编",wayBillTakeSendInfo.ReceivePost);
            printDataModel.setValue("电话",wayBillTakeSendInfo.ReceiveTel);
            printDataModel.setValue("应收金额",printModel.ReceivableAmount.ToString());
            printDataModel.setValue("送货时间",wayBillTakeSendInfo.SendTimeType);
            printDataModel.setValue("备注", wayBillTakeSendInfo.ReceiveComment);
            printDataModel.setValue("货物名称",string.Empty);
            printDataModel.setValue("金额",printModel.TotalAmount.ToString());
            printDataModel.setValue("客户重量",printModel.Weight.ToString());
            printDataModel.setValue("箱码",printModel.CustomerBoxNo??"");
            printDataModel.setValue("发件省",wayBillTakeSendInfo.SendProvince);
            printDataModel.setValue("发件市",wayBillTakeSendInfo.SendCity);
            printDataModel.setValue("发件区",wayBillTakeSendInfo.SendArea);
            printDataModel.setValue("发件地址", wayBillTakeSendInfo.SendAddress);
            printDataModel.setValue("货品属性",printModel.BillGoodsTypeDescription);
            printDataModel.setValue("配送站名称",printModel.DeliveryStation);
            printDataModel.setValue("称重重量", printModel.Weight.ToString());
            return printDataModel;
        }

        public ResultModel ReWeigh(string formCode, int packageIndex, decimal weight, int MerchantId, out decimal totalWeight)
        {
            this.FormCode = formCode;
            totalWeight = 0;
            if (PrintBillInfo == null) return ResultModel.Create(false, "未能查到此单号数据。");

            var list = BillWeighDAL.GetListByFormCode(formCode);
            if (list == null || list.Count == 0)
            {
                return ResultModel.Create(false, "未能查到此单号数据。");
            }
            var packageInfo = list.FirstOrDefault(x => x.PackageIndex == packageIndex);
            if (packageInfo == null)
            {
                return ResultModel.Create(false, "未能查到此单号数据。");
            }
            packageInfo.Weight = weight;

            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                try
                {
                    //更新包重量
                    int count = BillWeighDAL.UpdateWeight(formCode, packageIndex, weight);
                    totalWeight = list.Sum(x => x.Weight ?? 0);
                    //更新总重量
                    count = BillInfoDAL.UpdateBillInfoWeight(formCode, totalWeight);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    return ResultModel.Create(false, "更新重量出错。" + ex.Message);
                }
            }

            var logModel = new BillChangeLogDynamicModel
            {
                CreateBy = UserContext.CurrentUser.ID,
                CreateDept = UserContext.CurrentUser.DeptID,
                CurrentDistributionCode = PrintBillInfo.CurrentDistributionCode,
                CurrentSatus = PrintBillInfo.Status,
                DeliverStationID = PrintBillInfo.DeliverStationID,
                FormCode = FormCode,
                OperateType = Enums.TmsOperateType.ReWeigh,
                PreStatus = PrintBillInfo.Status,
            };

            //日志扩展属性
            logModel.ExtendedObj.IsPrint = false;
            logModel.ExtendedObj.Weight = weight;
            logModel.ExtendedObj.PackageIndex = packageIndex;
            logModel.ExtendedObj.PackageCount = BillInfoModel.PackageCount;

            WriteBillChangeLog(logModel);

            //需要复核称重
            var reviewModel = WeighingReview(MerchantId);
            if (reviewModel.IsAbnormal)
            {
                return ResultModel.Create(true, reviewModel.Message);
            }

            return ResultModel.Create(true);
        }


        public BillValidateResultModel ValidateBill(string customerOrder, string formCode)
        {
            this.FormCode = formCode;

            if (PrintBillInfo == null) return new BillValidateResultModel { Result = BillValidateResult.Failure, Message = "未找到运单" };
            //var merchant = MerchantBLL.GetByID(PrintBillInfo.MerchantID);
            //if (merchant == null) return new BillValidateResultModel { Result = BillValidateResult.Warning, Message = "未找到商家" };
            //if (!merchant.IsValidateBill)
            //    return new BillValidateResultModel { Result = BillValidateResult.Warning, Message = string.Format("商家【{0}】不需要面单校验", merchant.Name) };
            BillValidateResultModel rm = new BillValidateResultModel();
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                try
                {
                    if (PrintBillInfo.CustomerOrder.Equals(customerOrder))
                    {
                        BillInfoDAL.UpdateValidateStatus(formCode, true);
                        rm.Result = BillValidateResult.Success;
                    }
                    else
                    {
                        rm.Result = BillValidateResult.Failure;
                    }
                    var logModel = new BillChangeLogDynamicModel
                    {
                        CreateBy = UserContext.CurrentUser.ID,
                        CreateDept = UserContext.CurrentUser.DeptID,
                        CurrentDistributionCode = PrintBillInfo.CurrentDistributionCode,
                        CurrentSatus = PrintBillInfo.Status,
                        DeliverStationID = PrintBillInfo.DeliverStationID,
                        FormCode = formCode,
                        OperateType = Enums.TmsOperateType.BillValidate,
                        PreStatus = PrintBillInfo.Status,
                    };
                    logModel.ExtendedObj.CustomerOrder = customerOrder;
                    logModel.ExtendedObj.FormCode = formCode;
                    logModel.ExtendedObj.Validated = rm.Result == BillValidateResult.Success;

                    this.WriteBillChangeLog(logModel);
                    scope.Complete();
                    return rm;
                }
                catch (Exception ex)
                {
                    return new BillValidateResultModel { Result = BillValidateResult.Failure, Message = "执行异常", Exception = ex };
                }
            }
        }


        public BillPageViewModel ScanWeightNew(BillScanWeightArgModel argModel)
        {
       

            BillPageViewModel billPageData = new BillPageViewModel();
            billPageData.ScanResult = ScanResult.Scuccess;
            billPageData.FormCode = argModel.FormCode;
            try
            {
                this._formCode = argModel.FormCode;
                this.BillScanWeightModel = argModel;

                var rm = CheckNew();
                if (!rm.IsSuccess)
                {
                    return SetBillPageData(ScanResult.Failed, rm.Message, billPageData);
                }

                //箱数
                var PackageCount = BillInfoModel.PackageCount <= 0 ? 1 : BillInfoModel.PackageCount;

                billPageData.TotalPackageCount = PackageCount;

                //if (BillWeightList.Count >= PackageCount)
                //{
                //    return SetBillPageData(ScanResult.Failed, "箱数不正确,该单所有箱子已称重", billPageData);
                //}

                //添加重量
                var newBillWeigh = new BillPackageModel()
                {
                    FormCode = PrintBillInfo.FormCode,
                    PackageIndex = BillWeightList.Count + 1,
                    Weight = argModel.BillWeight,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    UpdateTime = DateTime.Now,
                    CreateBy = UserContext.CurrentUser.ID,
                    UpdateBy = UserContext.CurrentUser.ID,
                    SyncFlag = Enums.SyncStatus.NotYet,
                };
                //   newBillWeigh.BWID = BillWeighDAL.GetNextSequence(newBillWeigh.SequenceName);

                //总称重重量
                decimal currentWeightSum = BillWeightList.Sum(item => item.Weight) ?? 0.000m;
                BillWeightList.Add(newBillWeigh);

                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    try
                    {
                        if (PackageCount >= BillWeightList.Count)
                        {//添加箱重                            
                            BillWeighDAL.Add(newBillWeigh);
                            currentWeightSum += newBillWeigh.Weight ?? 0;
                            //   currentWeightSum = BillWeightList.Sum(item => item.Weight) ?? 0.000m;
                        }
                        else
                        {
                            //更新箱重                      
                            BillWeighDAL.UpdateWeight(PrintBillInfo.FormCode, BillWeightList.Count - 1, newBillWeigh.Weight ?? 0);
                            newBillWeigh.PackageIndex = newBillWeigh.PackageIndex - 1;
                            currentWeightSum = BillWeightList.Where(x => x.PackageIndex != BillWeightList.Count - 1).Sum(item => item.Weight ?? 0) + newBillWeigh.Weight ?? 0;
                        }
                        billPageData.TotalWeight = currentWeightSum;
                        billPageData.CurrentPackageIndex = newBillWeigh.PackageIndex;
                        //更新总重量
                        BillInfoDAL.UpdateBillInfoWeight(PrintBillInfo.FormCode, currentWeightSum);
                        scope.Complete();
                    }
                    catch (Exception ex)
                    {
                        return SetBillPageData(ScanResult.Failed, "添加重量失败", billPageData);
                    }
                }

                //需要复核称重
                if (argModel.IsNeedWeightReview)
                {
                    var reviewModel = WeighingReview(argModel.MerchantID);
                    billPageData.IsReviewWeightAbnormal = reviewModel.IsAbnormal;
                    if (reviewModel.IsAbnormal)
                    {
                        billPageData.ScanResult = ScanResult.Warming;
                    }
                    billPageData.Message = reviewModel.Message;
                }

                if (billPageData.ScanResult == ScanResult.Scuccess && string.IsNullOrWhiteSpace(billPageData.Message))
                {
                    billPageData.Message = "已成功扫描称重";
                }
                return SetBillPageData(billPageData.ScanResult, billPageData.Message, billPageData);

            }
            catch (Exception ex)
            {
                return SetBillPageData(ScanResult.Failed, "添加重量失败!异常：" + ex.Message, billPageData);
            }




        }
    }
}
