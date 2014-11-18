using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.BaseInfo;
using Vancl.TMS.BLL.CloudBillService;
using Vancl.TMS.BLL.CustomizeFlow;
using Vancl.TMS.BLL.WaybillLifeCycleService;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.Util;

namespace Vancl.TMS.BLL.Sorting.Inbound
{
    public class InboundBLLV2 :IInboundBLLV2
    {
        protected class InnerResultModel : ResultModel
        {
            /// <summary>
            /// 单号对象
            /// </summary>
            public InboundBillModel BillModel { get; set; }

        }
        private string _formCode;
        public string FormCode
        {
            set
            {
                if (_formCode != value)
                {
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

        private BillInfoModel _billInfoModel = null;
        private BillInfoModel BillInfoModel
        {
            get
            {
                if (_billInfoModel == null)
                {
                    _billInfoModel = BillInfoDal.GetBillInfoByFormCode(FormCode);
                }
                return _billInfoModel;
            }
        }

        private InboundBillModel _billModel = null;
        private InboundBillModel BillModel
        {
            get
            {
                if (_billModel == null)
                {
                    _billModel = BillBLL.GetInboundBillModelV2(FormCode);
                    _billModel.IsFirstInbound = string.IsNullOrWhiteSpace(BillModel.InboundKey) ||
                                                InboundDAL.GetDistributionInboundCount(FormCode,
                                                                                       UserContext.CurrentUser
                                                                                                  .DistributionCode) ==
                                                0;
                }
                   
                return _billModel;
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

        private WaybillLifeCycleClient _waybillLifeCycle = null;
        private WaybillLifeCycleClient WaybillLifeCycle
        {
            get
            {
                if (_waybillLifeCycle == null)
                {
                    _waybillLifeCycle = new WaybillLifeCycleClient();
                }
                return _waybillLifeCycle;
            }
        }

        //入库服务
        private IInboundDAL InboundDAL = ServiceFactory.GetService<IInboundDAL>("InboundDAL");

        private IInboundQueueDAL InboundQueueDAL = ServiceFactory.GetService<IInboundQueueDAL>("InboundQueueDAL");

        private IFormula<String, KeyCodeContextModel> KeyCodeGenerator = FormulasFactory.GetFormula<IFormula<String, KeyCodeContextModel>>("keycodeBLLFormula");

        private IBillBLL BillBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
        /// <summary>
        /// 表单扩展信息业务服务
        /// </summary>
        private IBillInfoDAL BillInfoDal = ServiceFactory.GetService<IBillInfoDAL>("BillInfoDAL");

        private IBillWeighDAL BillWeighDAL = ServiceFactory.GetService<IBillWeighDAL>();

        private  FlowFunFacade TMSFlowFunFacade = new FlowFunFacade();
        private  InboundCheckParameter paramter = new InboundCheckParameter();

       // private Stopwatch sw = new Stopwatch();


        public InboundResultModelV2 SimpleInboundV2(Model.Sorting.Inbound.InboundSimpleArgModelV2 argument)
        {
            try
            {
               
                
                //sw.Start();
                var result = CheckFormCode(argument);
               
                if (!result.IsSuccess)
                {
                    return result;
                }


                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
					if (string.IsNullOrEmpty(BillModel.InboundKey))
                    {
                        InboundResultModelV2 weightResult;
                        if (result.IsNeedWeight)
                        {
                            //sw.Reset();
                            //sw.Start();
                            weightResult = SimpleScanWeightV2(argument);
                            //sw.Stop();
                            //MessageCollector.Instance.Collect("", "入库称重：" + sw.ElapsedMilliseconds);
                            if (!weightResult.IsSuccess) return weightResult;

                        }
                    }
                    //sw.Reset();
                    //sw.Start();
                    if (AddToInboundQueueV2(BillModel, argument.OpUser) <= 0)
                        return new InboundResultModelV2() { IsSuccess = false, Message = "入库失败！", DeliverCode = result.DeliverCode, FormCode = result.FormCode };
                    //sw.Stop();
                    //MessageCollector.Instance.Collect("", "入库 ：" + sw.ElapsedMilliseconds);
                    //sw.Reset();
                    //sw.Start();
                    //流程自定义  
                   // TMSFlowFunFacade.WaybillTurn(Convert.ToInt64(this._formCode), Convert.ToInt32(argument.OpUser.ExpressId), 0);
                    //sw.Stop();
                    //MessageCollector.Instance.Collect("", "入库回写自定义流程：" + sw.ElapsedMilliseconds);
                    ////// 调用lifeCycle
                    //sw.Reset();
                    //sw.Start();
                    //if (string.IsNullOrEmpty(BillModel.InboundKey))
                    //{
                    //    WaybillLifeCycle.Inbound(new InBound()
                    //        {
                    //            first = true,
                    //            opdate = DateTime.Now,
                    //            opmanId = argument.OpUser.UserId,
                    //            sortingCenterId = Convert.ToInt32(argument.OpUser.ExpressId),
                    //            status = (int) Model.Common.Enums.BillStatus.HaveBeenSorting,
                    //            waybillno = Convert.ToInt64(this._formCode),
                    //            weight = argument.BillWeight
                    //        });
                    //}
                    //else
                    //{
                    //    WaybillLifeCycle.Inbound(new InBound()
                    //    {
                    //        first = false,
                    //        opdate = DateTime.Now,
                    //        opmanId = argument.OpUser.UserId,
                    //        sortingCenterId = Convert.ToInt32(argument.OpUser.ExpressId),
                    //        status = (int)Model.Common.Enums.BillStatus.HaveBeenSorting,
                    //        waybillno = Convert.ToInt64(this._formCode),
                    //        weight= BillInfoModel.CustomerWeight
                    //    });
                    //}
                    //sw.Stop();
                    //MessageCollector.Instance.Collect("", "调用lifecycle：" + sw.ElapsedMilliseconds);
	                scope.Complete();
                }
                if (!result.IsSkipPrint)
                {
                    //面单打印
                }

                return new InboundResultModelV2()
                {
                    IsSuccess = true,
                    Message = "入库成功",
                    FormCode = FormCode,
                    DeliverCode = result.DeliverCode,
                    CustomerWeight = result.CustomerWeight,
                    TotalInboundCount = GetInboundCountV2(argument.OpUser),
                    CurrentCount = BillInfoModel.PackageCount,
                    IsNeedWeight = result.IsNeedWeight,
                    IsSkipPrint = result.IsSkipPrint,
                };
            }
            catch (Exception ex)
            {

                  throw new Exception("入库异常："+ex.Message);
            }
          

        }

        public int GetInboundCountV2(SortCenterUserModel userModel)
        {

            if (userModel == null) throw new ArgumentNullException("GetInboundCount argument.OpUser is null");
            return InboundDAL.GetInboundCountNew(
                userModel.ExpressId.Value
                , DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"))
                , DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")));
        }

        public int GetFormCodeInboundCount(string FormCode)
        {
            if (string.IsNullOrEmpty(FormCode)) throw new ArgumentNullException("FormCode is null");
            return InboundDAL.GetFormCodeInbound(FormCode);
        }

        public BillInfoModel GetBillInfoByFormCode(string FormCode)
        {
            if (string.IsNullOrEmpty(FormCode)) throw new ArgumentNullException("FormCode is null");
            return BillInfoDal.GetBillInfoByFormCode(FormCode);
        }


        /// <summary>
        /// 添加到入库队列中(新)
        /// </summary>
        /// <param name="billModel"></param>
        /// <param name="opUser"></param>
        /// <param name="toStation"></param>
        /// <returns></returns>
        private int AddToInboundQueueV2(InboundBillModel billModel, SortCenterUserModel opUser)
        {
            InboundQueueEntityModel model = new InboundQueueEntityModel()
            {
                CreateDept = opUser.ExpressId.Value,
                CreateBy = opUser.UserId,
                DepartureID = opUser.ExpressId.Value,
                OpCount = 0,
                ArrivalID = 0,
                FormCode = billModel.FormCode,
                Status = billModel.Status,
                SeqStatus = Enums.SeqStatus.NoHand,
                Version = 2,
                DistributionCode = UserContext.CurrentUser.DistributionCode
            };
            var agentUser = UserContext.AgentUser;
            if (agentUser != null && agentUser.ID > 0)
            {
                model.AgentType = Enums.AgentType.SingleAgent;
                model.AgentUserID = agentUser.ID;
            }
            return InboundQueueDAL.AddV2(model);
        }

        private InboundResultModelV2 SimpleScanWeightV2(InboundSimpleArgModelV2 argument)
        {
            try
            {
                var newBillWeigh = new BillPackageModel()
                {
                    FormCode = FormCode,
                    PackageIndex = BillWeightList.Count + 1,
                    Weight = argument.BillWeight,
                    CreateTime = DateTime.Now,
                    IsDeleted = false,
                    UpdateTime = DateTime.Now,
                    CreateBy = UserContext.CurrentUser.ID,
                    UpdateBy = UserContext.CurrentUser.ID,
                    SyncFlag = Enums.SyncStatus.NotYet,
                };
                var PackageCount = BillInfoModel.PackageCount <= 0 ? 1 : BillInfoModel.PackageCount;
                //总称重重量
                decimal currentWeightSum = BillWeightList.Sum(item => item.Weight) ?? 0.000m;
                BillWeightList.Add(newBillWeigh);
                if (PackageCount >= BillWeightList.Count)
                {
                    if (BillWeighDAL.Add(newBillWeigh) <= 0)
                    {
                        return new InboundResultModelV2()
                        {
                            IsSuccess = false,
                            Message = "称重录入失败！; " ,
                            FormCode = FormCode,
                            IsSkipPrint = true,
                            IsNeedWeight = true,
                            CustomerWeight = BillInfoModel.CustomerWeight,
                            TotalInboundCount = GetInboundCountV2(argument.OpUser),
                            CurrentCount = BillInfoModel.PackageCount
                        };
                    };
                    currentWeightSum += newBillWeigh.Weight ?? 0;

                }
                else
                {
                    //更新箱重                      
                   if (
                       BillWeighDAL.UpdateWeight(argument.FormCode, BillWeightList.Count - 1, newBillWeigh.Weight ?? 0) <=
                       0)
                   {
                       
                           return new InboundResultModelV2()
                           {
                               IsSuccess = false,
                               Message = "称重更新失败！; ",
                               FormCode = FormCode,
                               IsSkipPrint = true,
                               IsNeedWeight = true,
                               CustomerWeight = BillInfoModel.CustomerWeight,
                               TotalInboundCount = GetInboundCountV2(argument.OpUser)
                           };
                     
                   };
                    newBillWeigh.PackageIndex = newBillWeigh.PackageIndex - 1;
                    currentWeightSum = BillWeightList.Where(x => x.PackageIndex != BillWeightList.Count - 1).Sum(item => item.Weight ?? 0) + newBillWeigh.Weight ?? 0;
                }

                BillInfoDal.UpdateBillInfoWeight(FormCode, currentWeightSum);
                 return new InboundResultModelV2()
                     {
                         IsSuccess = true,
                         Message = "称重成功！"
                     };
            }
            catch (Exception ex)
            {
                return new InboundResultModelV2()
                {
                    IsSuccess = false,
                    Message = "称重录入异常！; "+ex.Message,
                    FormCode = FormCode,
                    IsSkipPrint = true,
                    IsNeedWeight = true,
                    CustomerWeight = BillInfoModel.CustomerWeight,
                    TotalInboundCount = GetInboundCountV2(argument.OpUser)
                };
                
            }
           
                       
                    
                  
        }

        private InboundResultModelV2 CheckFormCode(InboundSimpleArgModelV2 argument)
        {
            var modelList = BillBLL.GetMerchantFormCodeRelation(argument.ScanType, argument.FormCode, argument.MerchantId);
            if (modelList == null || modelList.Count == 0)
            {
                if (argument.ScanType == Enums.SortCenterFormType.Waybill)
                {
                    return
                        new InboundResultModelV2
                            {
                                IsSuccess = false,
                                FormCode = argument.FormCode,
                                DeliverCode = string.Empty,
                                Message = "该运单号不存在",
                                TotalInboundCount = GetInboundCountV2(argument.OpUser),

                            }
                        ;
                }
                if (argument.ScanType == Enums.SortCenterFormType.DeliverCode)
                {
                    return
                            new InboundResultModelV2
                            {
                                IsSuccess = false,
                                DeliverCode = argument.FormCode,
                                FormCode = string.Empty,
                                Message = "该配送单号不存在",
                                TotalInboundCount = GetInboundCountV2(argument.OpUser)
                            };
                }
            }

            if (modelList.Count > 1)
            {
                string merchantnames = "";
                modelList.ForEach(x => { merchantnames += x.MerchantName + ","; });
                return new InboundResultModelV2()
                    {
                        IsSuccess = false,
                        DeliverCode =
                            argument.ScanType == Enums.SortCenterFormType.DeliverCode ? argument.FormCode : string.Empty,
                        FormCode =
                            argument.ScanType == Enums.SortCenterFormType.Waybill ? argument.FormCode : string.Empty,
                        Message = "存在多个商家" + merchantnames.Trim(','),
                        TotalInboundCount = GetInboundCountV2(argument.OpUser),
                    };

            }
            this._formCode = modelList[0].FormCode;

			#region   退货单可以入库
			//if (BillBLL.GetBillByFormCode(FormCode).BillType == Model.Common.Enums.BillType.Return)
			//{
			//    return new InboundResultModelV2()
			//    {
			//        IsSuccess = false,
			//        DeliverCode = modelList[0].DeliverCode,
			//        FormCode = modelList[0].FormCode,
			//        Message = "该订单为退货单",
			//        TotalInboundCount = GetInboundCountV2(argument.OpUser),
			//        CustomerWeight = BillInfoModel.CustomerWeight,
			//        IsNeedWeight = false,
			//        IsSkipPrint = true,
                    
			//    };
			//}
			#endregion

			//称重校验
			if (modelList[0].IsNeedWeight && argument.BillWeight == 0 && string.IsNullOrEmpty(BillModel.InboundKey))
            {
                return new InboundResultModelV2()
                    {
                        IsSuccess = false,
                        DeliverCode = modelList[0].DeliverCode,
                        FormCode = modelList[0].FormCode,
                        Message = "该订单必须称重",
                        IsSkipPrint = modelList[0].IsSkipPrintBill,
                        IsNeedWeight = modelList[0].IsNeedWeight,
                        CustomerWeight = BillInfoModel.CustomerWeight,
                        TotalInboundCount = GetInboundCountV2(argument.OpUser)
                    };
            }

            //复核称重校验
			if (modelList[0].IsNeedWeight && modelList[0].IsCheckWeight && string.IsNullOrEmpty(BillModel.InboundKey))
            {
                if (Math.Abs(BillInfoModel.CustomerWeight - argument.BillWeight) > modelList[0].CheckWeight)
                {
                    return new InboundResultModelV2()
                        {
                            IsSuccess = false,
                            DeliverCode = modelList[0].DeliverCode,
                            FormCode = FormCode,
                            Message = "复核称重异常",
                            IsSkipPrint = modelList[0].IsSkipPrintBill,
                            IsNeedWeight = modelList[0].IsNeedWeight,
                            CustomerWeight = BillInfoModel.CustomerWeight,
                            TotalInboundCount = GetInboundCountV2(argument.OpUser)
                        };
                }
            }

			#region 操作配送商等于当前配送商才可入库
			// BillModel.IsFirstInbound 表示当前操作配送商是否第一次入库
			// string.IsNullOrEmpty(BillModel.InboundKey) 表示当前Formcode是否第一次入库
			if (!string.IsNullOrEmpty(BillModel.CurrentDistributionCode) && BillModel.CurrentDistributionCode != argument.OpUser.DistributionCode)
			{
				return new InboundResultModelV2()
				{
					IsSuccess = false,
					DeliverCode = modelList[0].DeliverCode,
					FormCode = FormCode,
					Message = "此单不属于该配送商",
					CustomerWeight = BillInfoModel.CustomerWeight,
					CurrentCount = BillInfoModel.PackageCount,
					TotalInboundCount = GetInboundCountV2(argument.OpUser),
					IsSkipPrint = modelList[0].IsSkipPrintBill,
					IsNeedWeight = modelList[0].IsNeedWeight
				};
			}
			#endregion

            //sw.Stop();
            //MessageCollector.Instance.Collect("", "入库一般校验：" + sw.ElapsedMilliseconds);
            //sw.Reset();
            //sw.Start();
            #region 流程自定义设置
           
            paramter.WaybillNo = Convert.ToInt64(this.FormCode);
            paramter.FromExpressCompanyId = Convert.ToInt32(argument.OpUser.ExpressId);
            paramter.ToExpressCompanyId = Convert.ToInt32(BillModel.DeliverStationID);
	        paramter.IsFirstSorting = BillModel.IsFirstInbound;
	        paramter.CurDistributionCode = UserContext.CurrentUser.DistributionCode;
           
			var flowCheckResult = TMSFlowFunFacade.Check<InboundCheckParameter>(paramter, FunCode.FunInBound);
            
            if (!flowCheckResult.Result)
            {
                return new InboundResultModelV2()
                {
                    IsSuccess = false,
                    DeliverCode = modelList[0].DeliverCode,
                    FormCode = FormCode,
                    Message = flowCheckResult.Message,
                    CustomerWeight = BillInfoModel.CustomerWeight,
                    CurrentCount = BillInfoModel.PackageCount,
                    TotalInboundCount = GetInboundCountV2(argument.OpUser),
                    IsSkipPrint = modelList[0].IsSkipPrintBill,
                    IsNeedWeight = modelList[0].IsNeedWeight
                };
            }
            #endregion
            //sw.Stop();
            //MessageCollector.Instance.Collect("", "入库自定义流程校验：" + sw.ElapsedMilliseconds);
            //sw.Reset();
            //sw.Restart();
            if (BillModel.IsInbounding ||(BillModel.Status == Enums.BillStatus.HaveBeenSorting))
            {
                return new InboundResultModelV2()
                {
                    IsSuccess = false,
                    DeliverCode = modelList[0].DeliverCode,
                    FormCode = FormCode,
                    Message = "此单已经入库",
                    CustomerWeight = BillInfoModel.CustomerWeight,
                    CurrentCount = BillInfoModel.PackageCount,
                    TotalInboundCount = GetInboundCountV2(argument.OpUser),
                    IsSkipPrint = modelList[0].IsSkipPrintBill,
                    IsNeedWeight = modelList[0].IsNeedWeight
                };
            }
            //sw.Stop();
            //MessageCollector.Instance.Collect("", "重复入库校验：" + sw.ElapsedMilliseconds);
            return new InboundResultModelV2()
                {
                    IsSuccess = true,
                    DeliverCode = modelList[0].DeliverCode,
                    FormCode = FormCode,
                    CurrentCount = BillInfoModel.PackageCount,
                    CustomerWeight = BillInfoModel.CustomerWeight,
                    IsSkipPrint = modelList[0].IsSkipPrintBill,
                    IsNeedWeight = modelList[0].IsNeedWeight
                };
        }

        public InboundResultModelV2 ReWeight(InboundSimpleArgModelV2 argument)
        {
            this.FormCode = argument.FormCode;

            var modelList = BillBLL.GetMerchantFormCodeRelation(argument.ScanType, argument.FormCode, argument.MerchantId);

            if (modelList == null || modelList.Count == 0)
            {
                return new InboundResultModelV2()
                    {
                        IsSuccess = false,
                        Message = "重新称重失败，该单号不存在"
                    };
            }

            if (modelList[0].IsCheckWeight && BillModel.IsFirstInbound)
            {
                if (Math.Abs(argument.BillWeight - BillInfoModel.CustomerWeight) > modelList[0].CheckWeight)
                {
                    return new InboundResultModelV2()
                    {
                        IsSuccess = false,
                        Message = "重新称重失败，复核称重异常"
                    };
                }
            }

            if (BillWeighDAL.UpdateWeight(FormCode, BillWeightList.Count, argument.BillWeight) <= 0)
            {
                return new InboundResultModelV2()
                    {
                        IsSuccess = false,
                        Message = "重新称重失败"
                    };
            }

            BillInfoDal.UpdateBillInfoWeight(FormCode, argument.BillWeight);
            // 调用lifeCycle
            WaybillLifeCycle.Inbound(new InBound()
            {
                first = false,
                opdate = DateTime.Now,
                opmanId = argument.OpUser.UserId,
                sortingCenterId = Convert.ToInt32(argument.OpUser.ExpressId),
                status = (int)Model.Common.Enums.BillStatus.HaveBeenSorting,
                waybillno = Convert.ToInt64(this._formCode),
                weight = argument.BillWeight
            });
          
            return new InboundResultModelV2()
            {
                IsSuccess = true,
                Message = "重新称重成功"
            };
        }
    }
}
