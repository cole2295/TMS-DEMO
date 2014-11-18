using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.WaybillLifeCycleService;
using Vancl.TMS.BLL.WaybillTurnService;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.Security;
using Vancl.TMS.BLL.CloudBillService;
using Vancl.TMS.Model.BaseInfo.Sorting;
using System.Diagnostics;

namespace Vancl.TMS.BLL.Sorting.Inbound
{
    /// <summary>
    /// 入库业务实现
    /// </summary>
    public partial class InboundBLL : SortCenterBLL, IInboundBLL
    {
        /// <summary>
        /// 内部通用结果对象
        /// </summary>
        protected class InnerResultModel : ResultModel
        {

            /// <summary>
            /// 单号对象
            /// </summary>
            public InboundBillModel BillModel { get; set; }

            /// <summary>
            /// 转站对象
            /// </summary>
            public TurnStationModel TurnStationInfo { get; set; }

        }

        #region IInboundBLL 成员

        //入库服务
        private IInboundDAL InboundDAL = ServiceFactory.GetService<IInboundDAL>("InboundDAL");

        private ISC_SYN_TMS_InorderDAL sc2tmsDAL =
            ServiceFactory.GetService<ISC_SYN_TMS_InorderDAL>("SC_SYN_TMS_InorderDAL");

        private IInboundQueueDAL InboundQueueDAL = ServiceFactory.GetService<IInboundQueueDAL>("InboundQueueDAL");

        private IInboundSMSQueueDAL InboundSMSQueueDAL =
            ServiceFactory.GetService<IInboundSMSQueueDAL>("InboundSMSQueueDAL");

        private IFormula<InboundSMSGetContentResult, InboundSMSContext> MerchantSMSFormula =
            FormulasFactory.GetFormula<IFormula<InboundSMSGetContentResult, InboundSMSContext>>("Merchant");

        private IFormula<InboundSMSGetContentResult, InboundSMSContext> LineAreaSMSFormula =
            FormulasFactory.GetFormula<IFormula<InboundSMSGetContentResult, InboundSMSContext>>("LineArea");

        private IBillInfoDAL billInfoDAL = ServiceFactory.GetService<IBillInfoDAL>("BillInfoDAL");

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

        private WaybillTurnServiceClient _waybillTurnclient = null;
        private WaybillTurnServiceClient WaybillTurnClient
        {
           get
           {
               if (_waybillTurnclient == null)
               {
                   _waybillTurnclient = new WaybillTurnServiceClient();
               }
               return _waybillTurnclient;
           }
           
        }

        /// <summary>
        /// 添加到入库队列中
        /// </summary>
        /// <param name="billModel">入库运单对象</param>
        /// <param name="opUser">当前操作人</param>
        /// <param name="toStation">目的地</param>
        /// <returns></returns>
        private int AddToInboundQueue(InboundBillModel billModel, SortCenterUserModel opUser,
                                      SortCenterToStationModel toStation)
        {
            InboundQueueEntityModel model = new InboundQueueEntityModel()
                {
                    ArrivalID = toStation.ExpressCompanyID,
                    CreateDept = opUser.ExpressId.Value,
                    CreateBy = opUser.UserId,
                    DepartureID = opUser.ExpressId.Value,
                    OpCount = 0,
                    FormCode = billModel.FormCode,
                    Status = billModel.Status,
                    SeqStatus = Enums.SeqStatus.NoHand
                };
            var agentUser = UserContext.AgentUser;
            if (agentUser != null && agentUser.ID > 0)
            {
                model.AgentType = Enums.AgentType.SingleAgent;
                model.AgentUserID = agentUser.ID;
            }
            return InboundQueueDAL.Add(model);
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
                    Version = 2
                };
            var agentUser = UserContext.AgentUser;
            if (agentUser != null && agentUser.ID > 0)
            {
                model.AgentType = Enums.AgentType.SingleAgent;
                model.AgentUserID = agentUser.ID;
            }
            return InboundQueueDAL.AddV2(model);
        }

        /// <summary>
        /// 创建入库实体对象
        /// </summary>
        /// <param name="queueModel"></param>
        /// <param name="billModel"></param>
        /// <param name="toStation"></param>
        /// <returns></returns>
        private InboundEntityModel CreateInboundModel(InboundQueueEntityModel queueModel, InboundBillModel billModel,
                                                      SortCenterToStationModel toStation)
        {
            var InboundEntityModel = new InboundEntityModel()
                {
                    ApplyStation = null,
                    ArrivalID = queueModel.ArrivalID,
                    CreateBy = queueModel.CreateBy,
                    DepartureID = queueModel.DepartureID,
                    FormCode = queueModel.FormCode,
                    InboundType =
                        String.IsNullOrWhiteSpace(billModel.TurnstationKey)
                            ? toStation.SortingType
                            : Enums.SortCenterOperateType.TurnSorting,
                    IsDeleted = false,
                    SyncFlag = Enums.SyncStatus.NotYet,
                    UpdateBy = queueModel.CreateBy
                };
            InboundEntityModel.IBID = KeyCodeGenerator.Execute(
                new KeyCodeContextModel()
                    {
                        SequenceName = InboundEntityModel.SequenceName,
                        TableCode = InboundEntityModel.TableCode
                    }
                );
            return InboundEntityModel;
        }

        private InboundEntityModel CreateInboundModelV2(InboundQueueEntityModel queueModel, InboundBillModel billModel)
        {
            var InboundEntityModel = new InboundEntityModel()
                {
                    ApplyStation = null,
                    CreateBy = queueModel.CreateBy,
                    DepartureID = queueModel.DepartureID,
                    FormCode = queueModel.FormCode,
                    ArrivalID = 0,
                    IsDeleted = false,
                    SyncFlag = Enums.SyncStatus.NotYet,
                    UpdateBy = queueModel.CreateBy,
                    DistributionCode = queueModel.DistributionCode
                    
                };
            InboundEntityModel.IBID = KeyCodeGenerator.Execute(
                new KeyCodeContextModel()
                    {
                        SequenceName = InboundEntityModel.SequenceName,
                        TableCode = InboundEntityModel.TableCode
                    }
                );
            return InboundEntityModel;
        }

        /// <summary>
        /// 创建入库短信队列实体对象
        /// </summary>
        /// <param name="queueModel"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        private InboundSMSQueueEntityModel CreateInboundSMSQueueModel(InboundQueueEntityModel queueModel,
                                                                      InboundSMSGetContentResult Result)
        {
            return new InboundSMSQueueEntityModel()
                {
                    ArrivalID = queueModel.ArrivalID,
                    CreateBy = queueModel.CreateBy,
                    DepartureID = queueModel.DepartureID,
                    FormCode = queueModel.FormCode,
                    OpCount = 0,
                    SendedContent = Result.Content,
                    SendTime = Result.SendTime,
                    SeqStatus = Enums.SeqStatus.NoHand,
                    UpdateBy = queueModel.CreateBy
                };
        }

        /// <summary>
        /// 通用入库接口
        /// </summary>
        /// <param name="argModel">通用入库参数</param>
        /// <param name="FormType">单号类型</param>
        /// <param name="InputCode">输入单号</param>
        /// <returns></returns>
        private InnerResultModel CommonInbound(IInboundArgModel argModel, Enums.SortCenterFormType FormType,
                                               String InputCode, bool ValidateFormCode = true)
        {
            StringBuilder sbTimeStr = new StringBuilder();
            long tmpTime = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var Result = new InnerResultModel();
            String FormCode = InputCode;
            if (ValidateFormCode)
            {
                //检查运单号合法性,并以系统运单号作为主单号进行操作
                String tmpCode = null;
                var checkResult = base.ValidateFormCode(FormType, InputCode, out tmpCode);
                if (!checkResult.IsSuccess)
                {
                    return Result.Failed(checkResult.Message) as InnerResultModel;
                }
                FormCode = tmpCode;
            }
            sbTimeStr.Append(string.Format("前期验证(通)：{0};", sw.ElapsedMilliseconds));
            tmpTime = sw.ElapsedMilliseconds;
            InboundBillModel billModel = BillBLL.GetInboundBillModel(FormCode);
            sbTimeStr.Append(string.Format("获取信息(通)：{0};", sw.ElapsedMilliseconds - tmpTime));
            tmpTime = sw.ElapsedMilliseconds;
            Result.BillModel = billModel;
            if (billModel == null)
            {
                return Result.Failed("运单号不存在!") as InnerResultModel;
            }
            //暂时注掉，因为小米问题
            //if (string.IsNullOrEmpty(billModel.InboundKey) && billModel.IsJudgePrint)
            //{
            //    Int32 printDept = InboundDAL.GetPrintDept(long.Parse(FormCode));
            //    if (printDept > 0)
            //    {
            //        if (argModel.OpUser.ExpressId != printDept)
            //            return Result.Failed("当前入库分拣中心与实际接货分拣中心不一致!") as InnerResultModel;
            //    }
            //}
            if (billModel.IsValidateBill)
            {
                BillInfoModel infoModel = BillInfoDal.GetBillInfoByFormCode(FormCode);
                sbTimeStr.Append(string.Format("获取面单校验(通)：{0};", sw.ElapsedMilliseconds - tmpTime));
                tmpTime = sw.ElapsedMilliseconds;
                if (infoModel != null && !infoModel.IsValidateBill)
                {
                    return Result.Failed("该运单没有通过面单校验，请使用面单校验通过后入库!") as InnerResultModel;
                }
            }
            if (billModel.BillType == Enums.BillType.Return)
            {
                return Result.Failed("该订单为上门退货单，不允许操作!") as InnerResultModel;
            }
            if (!String.IsNullOrWhiteSpace(billModel.TurnstationKey))
            {
                return Result.Failed("此运单为转站运单，请在转站入库中操作!") as InnerResultModel;
            }
            if (billModel.Status != argModel.PreCondition.PreStatus)
            {
                return Result.Failed("运单状态不符合入库条件!") as InnerResultModel;
            }
            if (!InboundDAL.ValidateBillWeight(billModel.FormCode))
            {
                return Result.Failed("此单未称重，请输入重量后再操作入库!") as InnerResultModel;
            }
            if (billModel.IsInbounding
                ||
                (!String.IsNullOrWhiteSpace(billModel.InboundKey) && String.IsNullOrWhiteSpace(billModel.OutboundKey)))
            {
                return Result.Failed("该订单已完成入库操作!") as InnerResultModel;
            }
            if (billModel.CurrentDistributionCode != argModel.OpUser.DistributionCode)
            {
                return Result.Failed("此运单尚未运输到当前分拣中心!") as InnerResultModel;
            }
            if (argModel.IsLimitedQuantity)
            {
                int nCurrentInboundCount = GetInboundCount(new InboundSimpleArgModel()
                    {
                        OpUser = argModel.OpUser,
                        ToStation = argModel.ToStation
                    });
                if (nCurrentInboundCount >= argModel.LimitedInboundCount)
                {
                    return Result.Failed(string.Format("今天已经入库{0}单", argModel.LimitedInboundCount)) as InnerResultModel;
                }
            }
            if (argModel.ToStation.SortingType == Enums.SortCenterOperateType.SimpleSorting)
            {
                if (billModel.DeliverStationID != argModel.ToStation.ExpressCompanyID)
                {
                    return Result.Failed("分拣站点选择错误!") as InnerResultModel;
                }
            }
            if (argModel.ToStation.SortingType == Enums.SortCenterOperateType.DistributionSorting)
            {
                if (billModel.DeliverStationID <= 0)
                {
                    return Result.Failed("订单未分配站点!") as InnerResultModel;
                }
                if (
                    !InboundDAL.ValidateDistributionDeliverStation(argModel.ToStation.DistributionCode,
                                                                   billModel.DeliverStationID))
                {
                    return Result.Failed("分拣配送商选择错误!") as InnerResultModel;
                }
            }
            if (argModel.ToStation.SortingType == Enums.SortCenterOperateType.SecondSorting)
            {

                //int[] iSortCenterList = GetSecondSortCenterID(billModel.DeliverStationID);

                //if (!(iSortCenterList as ICollection<int>).Contains(argModel.ToStation.ExpressCompanyID))
                //{
                //    return Result.Failed("二级分拣中心选择错误!") as InnerResultModel;
                //}

                //int iSortCenterList = GetSecondSortCenterIDByCityLine(billModel.DeliverStationID);

                //if (iSortCenterList !=argModel.ToStation.ExpressCompanyID)
                //{
                //    return Result.Failed("该运单不属于此二级分拣中心!") as InnerResultModel;
                //}

                if (InboundDAL.ExistsLine_EqualLastInboud(billModel.FormCode, argModel.OpUser.ExpressId.Value,
                                                          argModel.ToStation.ExpressCompanyID))
                {
                    return Result.Failed("此运单已二级分拣入库，请勿重复操作!") as InnerResultModel;
                }
            }
            if (AddToInboundQueue(billModel, argModel.OpUser, argModel.ToStation) <= 0)
            {
                return Result.Failed("入库失败!") as InnerResultModel;
            }
            sw.Stop();
            sbTimeStr.Append(string.Format("后期验证(通)：{0};", sw.ElapsedMilliseconds - tmpTime));
            object logTime = sbTimeStr.ToString();
            return Result.Succeed(logTime) as InnerResultModel;
        }


        /// <summary>
        /// 入库通用新接口
        /// </summary>
        /// <param name="argModel"></param>
        /// <param name="FormType"></param>
        /// <param name="InputCode"></param>
        /// <param name="ValidateFormCode"></param>
        /// <returns></returns>
        private InnerResultModel CommonInboundV2(IInboundArgModel argModel, String InputCode, ref BillInfoModel billInfo)
        {
            StringBuilder sbTimeStr = new StringBuilder();
            long tmpTime = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var Result = new InnerResultModel();
            String FormCode = InputCode;

            tmpTime = sw.ElapsedMilliseconds;
            InboundBillModel billModel = BillBLL.GetInboundBillModel(FormCode);
            billInfo = GetBillInfoByFormCode(FormCode);
            int currentCount = GetFormCodeInboundCount(FormCode);
            sbTimeStr.Append(string.Format("获取信息(通)：{0};", sw.ElapsedMilliseconds - tmpTime));
            tmpTime = sw.ElapsedMilliseconds;
            Result.BillModel = billModel;

            if (currentCount >= (billInfo.PackageCount == 0 ? 1 : billInfo.PackageCount))
            {
                return Result.Failed("该订单所有件已经入库！") as InnerResultModel;
            }

            //if (billModel.IsInbounding
            //    || (!String.IsNullOrWhiteSpace(billModel.InboundKey) && String.IsNullOrWhiteSpace(billModel.OutboundKey)))
            //{
            //    return Result.Failed("该订单已完成入库操作!") as InnerResultModel;
            //}
            //if (billModel.CurrentDistributionCode != argModel.OpUser.DistributionCode)
            //{
            //    return Result.Failed("此运单尚未运输到当前分拣中心!") as InnerResultModel;
            //}


            if (AddToInboundQueueV2(billModel, argModel.OpUser) <= 0)
            {
                return Result.Failed("入库失败!") as InnerResultModel;
            }
            sw.Stop();
            sbTimeStr.Append(string.Format("后期验证(通)：{0};", sw.ElapsedMilliseconds - tmpTime));
            object logTime = sbTimeStr.ToString();
            return Result.Succeed(logTime) as InnerResultModel;
        }


        /// <summary>
        /// 逐单入库
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>逐单入库 View Model</returns>
        public ViewInboundSimpleModel SimpleInbound(InboundSimpleArgModel argument)
        {
            StringBuilder sbTimeStr = new StringBuilder();
            long tmpTime = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (argument == null) throw new ArgumentNullException("InboundSimpleArgModel  is null");
            if (argument.OpUser == null) throw new ArgumentNullException("InboundSimpleArgModel.OpUser is null");
            if (argument.ToStation == null) throw new ArgumentNullException("InboundSimpleArgModel.ToStation is null");
            if (argument.PreCondition == null)
                throw new ArgumentNullException("InboundSimpleArgModel.PreCondition is null");
            ViewInboundSimpleModel Result = new ViewInboundSimpleModel();
            if (argument.OpUser.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            {
                return Result.Failed("请用分拣中心帐号操作入库!") as ViewInboundSimpleModel;
            }
            var ComResult = CommonInbound(argument, argument.FormType, argument.FormCode);
            sbTimeStr.Append(ComResult.PromptMessage);
            tmpTime = sw.ElapsedMilliseconds;
            if (!ComResult.IsSuccess)
            {
                return Result.Failed(ComResult.Message) as ViewInboundSimpleModel;
            }

            int nCurrentInboundCount = GetInboundCount(argument);

            sbTimeStr.Append(string.Format("查询入库量耗时：{0};", sw.ElapsedMilliseconds - tmpTime));
            Result.FormCode = ComResult.BillModel.FormCode;
            Result.InboundCount = nCurrentInboundCount;
            Result.CustomerOrder = ComResult.BillModel.CustomerOrder;
            if (argument.LimitedInboundCount == nCurrentInboundCount)
            {
                Result.Message = String.Format("当前已经入库成功{0}单", nCurrentInboundCount);
            }
            sw.Stop();
            object logTime = string.Format("{0}总耗时(毫秒)：{1}({2})", argument.FormCode, sw.ElapsedMilliseconds,
                                           sbTimeStr.ToString());
            return Result.Succeed(logTime) as ViewInboundSimpleModel;
        }

        /// <summary>
        /// 批量入库
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>批量入库 View Model</returns>
        public ViewInboundBatchModel BatchInbound(InboundBatchArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundBatchArgModel  is null");
            if (argument.OpUser == null) throw new ArgumentNullException("InboundBatchArgModel.OpUser is null");
            if (argument.ToStation == null) throw new ArgumentNullException("InboundBatchArgModel.ToStation is null");
            if (argument.PreCondition == null)
                throw new ArgumentNullException("InboundBatchArgModel.PreCondition is null");
            ViewInboundBatchModel Result = new ViewInboundBatchModel()
                {
                    ListErrorBill = new List<SortCenterBatchErrorModel>()
                };
            if (argument.ArrFormCode == null || argument.ArrFormCode.Length <= 0)
            {
                return Result.Failed("入库单号列表为空!") as ViewInboundBatchModel;
            }
            if (argument.OpUser.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            {
                return Result.Failed("请用分拣中心帐号操作入库!") as ViewInboundBatchModel;
            }
            List<SortCenterBatchErrorModel> TmpListErrorBill = null;
            var SysFormCodeList = base.ValidateFormCode(argument.FormType, argument.ArrFormCode, out TmpListErrorBill);
            if (TmpListErrorBill != null || TmpListErrorBill.Count > 0)
            {
                Result.ListErrorBill.AddRange(TmpListErrorBill);
            }
            if (SysFormCodeList == null || SysFormCodeList.Count <= 0)
            {
                return Result.Failed("入库单号验证失败!") as ViewInboundBatchModel;
            }
            InnerResultModel tmpResult = null;
            foreach (var item in SysFormCodeList)
            {
                tmpResult = CommonInbound(argument, Enums.SortCenterFormType.Waybill, item, false);
                if (!tmpResult.IsSuccess)
                {
                    if (tmpResult.BillModel != null)
                    {
                        Result.ListErrorBill.Add(new SortCenterBatchErrorModel()
                            {
                                WaybillNo = tmpResult.BillModel.FormCode,
                                CustomerOrder = tmpResult.BillModel.CustomerOrder,
                                ErrorMsg = tmpResult.Message
                            });
                    }
                    else
                    {
                        Result.ListErrorBill.Add(new SortCenterBatchErrorModel()
                            {
                                WaybillNo = item,
                                CustomerOrder = "",
                                ErrorMsg = tmpResult.Message
                            });
                    }
                }
                else
                {
                    Result.SucceedCount++;
                }
            }
            int nCurrentInboundCount = GetInboundCount(argument);
            Result.InboundCount = nCurrentInboundCount;
            Result.FailedCount = argument.ArrFormCode.Length - Result.SucceedCount;
            return
                Result.Succeed(String.Format("入库成功{0}单，失败{1}单", Result.SucceedCount, Result.FailedCount)) as
                ViewInboundBatchModel;
        }

        /// <summary>
        /// 取得当前操作入库数量
        /// </summary>
        /// <param name="argument"></param>
        /// <returns>当前操作入库数量</returns>
        public int GetInboundCount(ISortCenterArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("GetInboundCount argument is null");
            if (argument.OpUser == null) throw new ArgumentNullException("GetInboundCount argument.OpUser is null");
            if (argument.ToStation == null)
                throw new ArgumentNullException("GetInboundCount argument.ToStation is null");
            return InboundDAL.GetInboundCount(
                argument.OpUser.ExpressId.Value
                , argument.ToStation.ExpressCompanyID
                , DateTime.Parse(DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"))
                , DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")));
        }

        /// <summary>
        /// 入库队列处理
        /// </summary>
        /// <param name="argument"></param>
        public void HandleInboundQueue(InboundQueueArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("HandleInboundQueue.argument IS NULL");
            if (argument.QueueItem == null)
                throw new ArgumentNullException("HandleInboundQueue.argument.QueueItem IS NULL");
            if (argument.MerchantSMSConfig == null)
                throw new ArgumentNullException("HandleInboundQueue.argument.MerchantSMSConfig IS NULL");
            if (argument.LineAreaSMSConfig == null)
                throw new ArgumentNullException("HandleInboundQueue.argument.LineAreaSMSConfig IS NULL");

            SortCenterToStationModel ToStation = base.GetToStationModel(argument.QueueItem.ArrivalID);
            SortCenterUserModel CurUserModel = base.GetUserModel(argument.QueueItem.CreateBy);
            InboundBillModel billModel = BillBLL.GetInboundBillModel_ByQueueHandled(argument.QueueItem.FormCode);
            if (billModel.Status != argument.QueueItem.Status)
            {
                InboundQueueDAL.UpdateToError(argument.QueueItem.IBSID);
                return;
            }
            var billPreStatus = billModel.Status;

            billModel.Status = Enums.BillStatus.HaveBeenSorting; //运单状态修改
            billModel.UpdateBy = argument.QueueItem.CreateBy;
            billModel.UpdateDept = argument.QueueItem.CreateDept;

            InboundSMSGetContentResult SMSResult = InboundSMSCheck(argument, billModel, CurUserModel);
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                var inboundModel = CreateInboundModel(argument.QueueItem, billModel, ToStation);
                if (InboundDAL.Add(inboundModel) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_Inbound表失败", billModel.FormCode));
                }
                billModel.InboundKey = inboundModel.IBID;
                billModel.OutboundKey = null;
                if (!BillBLL.UpdateBillModelByInbound(billModel))
                {
                    throw new Exception(String.Format("系统运单:{0}更新SC_Bill表失败", billModel.FormCode));
                }
                var billchangelogDynamicModel = new Model.Log.BillChangeLogDynamicModel()
                    {
                        FormCode = billModel.FormCode,
                        CurrentDistributionCode = billModel.CurrentDistributionCode,
                        DeliverStationID = billModel.DeliverStationID,
                        CurrentSatus = billModel.Status,
                        OperateType = Enums.TmsOperateType.Inbound,
                        PreStatus = billPreStatus,
                        CreateBy = argument.QueueItem.CreateBy,
                        CreateDept = argument.QueueItem.CreateDept
                    };
                billchangelogDynamicModel.ExtendedObj.DistributionMnemonicName = CurUserModel.DistributionMnemonicName;
                billchangelogDynamicModel.ExtendedObj.SortCenterMnemonicName = CurUserModel.MnemonicName;
                if (argument.QueueItem.AgentType == Enums.AgentType.SingleAgent)
                {
                    if (!argument.QueueItem.AgentUserID.HasValue)
                        throw new Exception(String.Format("程序严重错误，单号{0}:启用了代理，但是代理用户ID为空", billModel.FormCode));
                    SortCenterUserModel AgentUser = base.GetUserModel(argument.QueueItem.AgentUserID.Value);
                    if (AgentUser == null)
                        throw new Exception(String.Format("程序严重错误，单号{0}:启用了代理，但是代理用户对象为null", billModel.FormCode));
                    billchangelogDynamicModel.ExtendedObj.AgentUser = new UserModel()
                        {
                            ID = AgentUser.UserId,
                            DeptID = AgentUser.ExpressId.Value,
                            DistributionCode = AgentUser.DistributionCode
                        };
                }
                if (WriteBillChangeLog(billchangelogDynamicModel) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_BillChangeLog表失败", billModel.FormCode));
                }
                if (sc2tmsDAL.Add(new SC_SYN_TMS_InorderEntityModel()
                    {
                        FormCode = billModel.FormCode,
                        CustomerOrder = billModel.CustomerOrder,
                        InboundID = argument.QueueItem.DepartureID,
                        SC2TMSFlag = Enums.SC2TMSSyncFlag.Notyet
                    }) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_SYN_TMS_Inorder表失败", billModel.FormCode));
                }
                if (SMSResult.IsSuccess)
                {
                    InboundSMSQueueDAL.Add(CreateInboundSMSQueueModel(argument.QueueItem, SMSResult));
                }
                InboundQueueDAL.UpdateToHandled(argument.QueueItem.IBSID);
                scope.Complete();
            }
        }

        /// <summary>
        /// 入库前置条件对象
        /// </summary>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>
        public InboundPreConditionModel GetPreCondition(string DistributionCode)
        {
            if (String.IsNullOrWhiteSpace(DistributionCode))
                throw new ArgumentNullException("DistributionCode Is null or empty.");
            Enums.BillStatus? PreStatus = DistributionBLL.GetDistributionPreBillStatus(DistributionCode,
                                                                                       Enums.BillStatus.HaveBeenSorting);
            if (PreStatus.HasValue)
            {
                InboundPreConditionModel preCondition = new InboundPreConditionModel()
                    {
                        PreStatus = PreStatus.Value
                    };
                return preCondition;
            }
            return null;
        }

        #endregion



        public ViewInboundSimpleModel SimpleInboundNew(InboundSimpleArgModel argument)
        {
            StringBuilder sbTimeStr = new StringBuilder();
            long tmpTime = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (argument == null) throw new ArgumentNullException("InboundSimpleArgModel  is null");
            if (argument.OpUser == null) throw new ArgumentNullException("InboundSimpleArgModel.OpUser is null");
            ViewInboundSimpleModel Result = new ViewInboundSimpleModel();
            if (argument.OpUser.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            {
                return Result.Failed("请用分拣中心帐号操作入库!") as ViewInboundSimpleModel;
            }
            BillInfoModel billInfo = new BillInfoModel();
            var ComResult = CommonInboundV2(argument, argument.FormCode, ref billInfo);
            sbTimeStr.Append(ComResult.PromptMessage);
            tmpTime = sw.ElapsedMilliseconds;
            if (!ComResult.IsSuccess)
            {
                return Result.Failed(ComResult.Message) as ViewInboundSimpleModel;
            }

            int nCurrentInboundCount = GetInboundCountNew(argument);

            sbTimeStr.Append(string.Format("查询入库量耗时：{0};", sw.ElapsedMilliseconds - tmpTime));
            Result.FormCode = ComResult.BillModel.FormCode;
            Result.InboundCount = nCurrentInboundCount;
            Result.CustomerOrder = ComResult.BillModel.CustomerOrder;
            Result.Message = String.Format("当前已经入库成功{0}单", nCurrentInboundCount);
            Result.CurrentCount = GetFormCodeInboundCount(argument.FormCode);
            Result.PackegeCount = billInfo.PackageCount;
            Result.CustomerWeight = billInfo.CustomerWeight;
            sw.Stop();
            object logTime = string.Format("{0}总耗时(毫秒)：{1}({2})", argument.FormCode, sw.ElapsedMilliseconds,
                                           sbTimeStr.ToString());
            return Result.Succeed(logTime) as ViewInboundSimpleModel;
        }


        public int GetInboundCountNew(ISortCenterArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("GetInboundCount argument is null");
            if (argument.OpUser == null) throw new ArgumentNullException("GetInboundCount argument.OpUser is null");
            return InboundDAL.GetInboundCountNew(
                argument.OpUser.ExpressId.Value
                , DateTime.Parse(DateTime.Now.AddHours(-24).ToString("yyyy-MM-dd HH:mm:ss"))
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
            return billInfoDAL.GetBillInfoByFormCode(FormCode);
        }


        public void HandleInboundQueueV2(InboundQueueArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("HandleInboundQueue.argument IS NULL");
            if (argument.QueueItem == null)
                throw new ArgumentNullException("HandleInboundQueue.argument.QueueItem IS NULL");
            //if (argument.MerchantSMSConfig == null)
            //    throw new ArgumentNullException("HandleInboundQueue.argument.MerchantSMSConfig IS NULL");
            //if (argument.LineAreaSMSConfig == null)
            //    throw new ArgumentNullException("HandleInboundQueue.argument.LineAreaSMSConfig IS NULL");


            SortCenterUserModel CurUserModel = base.GetUserModel(argument.QueueItem.CreateBy);
            InboundBillModel billModel = BillBLL.GetInboundBillModel_ByQueueHandled(argument.QueueItem.FormCode);
            BillInfoModel billInfo = billInfoDAL.GetBillInfoByFormCode(argument.QueueItem.FormCode);
            bool fisrtInbound = string.IsNullOrWhiteSpace(billModel.InboundKey);
            billModel.Status = Enums.BillStatus.HaveBeenSorting; //运单状态修改
            billModel.UpdateBy = argument.QueueItem.CreateBy;
            billModel.UpdateDept = argument.QueueItem.CreateDept;
           
           // InboundSMSGetContentResult SMSResult = InboundSMSCheck(argument, billModel, CurUserModel);
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                var inboundModel = CreateInboundModelV2(argument.QueueItem, billModel);
                if (InboundDAL.AddV2(inboundModel) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_Inbound表失败", billModel.FormCode));
                }
                billModel.InboundKey = inboundModel.IBID;
                billModel.OutboundKey = null;
                if (!BillBLL.UpdateBillModelByInbound(billModel))
                {
                    throw new Exception(String.Format("系统运单:{0}更新SC_Bill表失败", billModel.FormCode));
                }
                var billchangelogDynamicModel = new Model.Log.BillChangeLogDynamicModel()
                    {
                        FormCode = billModel.FormCode,
                        CurrentDistributionCode = billModel.CurrentDistributionCode,
                        DeliverStationID = billModel.DeliverStationID,
                        CurrentSatus = billModel.Status,
                        OperateType = Enums.TmsOperateType.Inbound,
                        CreateBy = argument.QueueItem.CreateBy,
                        CreateDept = argument.QueueItem.CreateDept
                    };
                billchangelogDynamicModel.ExtendedObj.DistributionMnemonicName = CurUserModel.DistributionMnemonicName;
                billchangelogDynamicModel.ExtendedObj.SortCenterMnemonicName = CurUserModel.MnemonicName;
                if (argument.QueueItem.AgentType == Enums.AgentType.SingleAgent)
                {
                    if (!argument.QueueItem.AgentUserID.HasValue)
                        throw new Exception(String.Format("程序严重错误，单号{0}:启用了代理，但是代理用户ID为空", billModel.FormCode));
                    SortCenterUserModel AgentUser = base.GetUserModel(argument.QueueItem.AgentUserID.Value);
                    if (AgentUser == null)
                        throw new Exception(String.Format("程序严重错误，单号{0}:启用了代理，但是代理用户对象为null", billModel.FormCode));
                    billchangelogDynamicModel.ExtendedObj.AgentUser = new UserModel()
                        {
                            ID = AgentUser.UserId,
                            DeptID = AgentUser.ExpressId.Value,
                            DistributionCode = AgentUser.DistributionCode
                        };
                }
                if (WriteBillChangeLog(billchangelogDynamicModel) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_BillChangeLog表失败", billModel.FormCode));
                }
                if (sc2tmsDAL.Add(new SC_SYN_TMS_InorderEntityModel()
                    {
                        FormCode = billModel.FormCode,
                        CustomerOrder = billModel.CustomerOrder,
                        InboundID = argument.QueueItem.DepartureID,
                        SC2TMSFlag = Enums.SC2TMSSyncFlag.Notyet
                    }) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_SYN_TMS_Inorder表失败", billModel.FormCode));
                }
                //if (SMSResult.IsSuccess)
                //{
                //    InboundSMSQueueDAL.Add(CreateInboundSMSQueueModel(argument.QueueItem, SMSResult));
                //}
                InboundQueueDAL.UpdateToHandled(argument.QueueItem.IBSID);

                if (fisrtInbound)
                {
                    WaybillLifeCycle.Inbound(new InBound()
                    {
                        first = true,
                        opdate = DateTime.Now,
                        opmanId = argument.QueueItem.CreateBy,
                        sortingCenterId = argument.QueueItem.CreateDept,
                        status = (int)Model.Common.Enums.BillStatus.HaveBeenSorting,
                        waybillno = Convert.ToInt64(argument.QueueItem.FormCode),
                        weight = billInfo.CustomerWeight
                    });
                }
                else
                {
                    WaybillLifeCycle.Inbound(new InBound()
                    {
                        first = false,
                        opdate = DateTime.Now,
                        opmanId = argument.QueueItem.CreateBy,
                        sortingCenterId = argument.QueueItem.CreateDept,
                        status = (int)Model.Common.Enums.BillStatus.HaveBeenSorting,
                        waybillno = Convert.ToInt64(argument.QueueItem.FormCode),
                        weight = billInfo.CustomerWeight
                    });
                }

                WaybillTurnModel turn = new WaybillTurnModel();
                turn.WaybillNo = Convert.ToInt64(argument.QueueItem.FormCode);
                turn.FromExpressCompanyId = argument.QueueItem.CreateDept;
                turn.ToExpressCompanyId = argument.QueueItem.CreateDept;
                turn.FromDistributionCode = UserContext.CurrentUser.DistributionCode;
                turn.ToDistributionCode = UserContext.CurrentUser.DistributionCode;
                turn.TurnType = EnumCommonWaybillTurnType.NoTurn;
                WaybillTurnClient.WaybillTurn(turn);

                scope.Complete();
            }
        }
    }
}
