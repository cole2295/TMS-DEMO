using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.IBLL.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IDAL.Sorting.Outbound;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Model.Sorting.Outbound.SMS;

namespace Vancl.TMS.BLL.Sorting.Outbound
{
    /// <summary>
    /// 出库业务实现
    /// </summary>
    public partial class OutboundBLL : SortCenterBLL, IOutboundBLL
    {
        /// <summary>
        /// 内部通用结果对象
        /// </summary>
        protected class InnerResultModel : ResultModel
        {

            /// <summary>
            /// 单号对象
            /// </summary>
            public OutboundBillModel BillModel { get; set; }

        }

        #region IOutboundBLL 成员
        #region 出库相关服务
        /// <summary>
        /// 出库数据层
        /// </summary>
        IOutboundDAL outboundDAL = ServiceFactory.GetService<IOutboundDAL>("SC_OutboundDAL");
        /// <summary>
        /// 出库批次数据层
        /// </summary>
        IOutboundBatchDAL outboundbatchDAL = ServiceFactory.GetService<IOutboundBatchDAL>("SC_OutboundBatchDAL");
        /// <summary>
        /// 分拣同步到TMS城际运输数据层
        /// </summary>
        ISC_SYN_TMS_OutboxDAL sc_syn_tms_outboxDAL = ServiceFactory.GetService<ISC_SYN_TMS_OutboxDAL>("SC_SYN_TMS_OutboxDAL");
        /// <summary>
        /// 出库批次号产生算法
        /// </summary>
        IFormula<string, SerialNumberModel> batchNoGenerator = FormulasFactory.GetFormula<IFormula<string, SerialNumberModel>>("OutboundBatchNoGenerateFormula");

        #endregion
        /// <summary>
        /// 取得出库前置条件
        /// </summary>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>
        public OutboundPreConditionModel GetPreCondition(string DistributionCode)
        {
            return new OutboundPreConditionModel()
            {
                PreStatus = Enums.BillStatus.HaveBeenSorting
            };
        }

        /// <summary>
        /// 创建出库实体对象
        /// </summary>
        /// <param name="billModel">出库运单对象</param>
        /// <param name="BatchNo">出库批次号</param>
        /// <returns></returns>
        private OutboundEntityModel CreateOutboundEntityModel(OutboundBillModel billModel, String BatchNo)
        {
            var outboundmodel = new OutboundEntityModel()
            {
                ArrivalID = billModel.ArrivalID,
                BatchNo = BatchNo,
                CreateBy = UserContext.CurrentUser.ID,
                DepartureID = billModel.DepartureID,
                FormCode = billModel.FormCode,
                IsDeleted = false,
                OutboundType = billModel.InboundType,
                SyncFlag = Enums.SyncStatus.NotYet,
                UpdateBy = UserContext.CurrentUser.ID
            };
            outboundmodel.OBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = outboundmodel.SequenceName, TableCode = outboundmodel.TableCode });
            return outboundmodel;
        }

        /// <summary>
        /// 初始化出库后的运单对象[修改内部属性]
        /// </summary>
        /// <param name="OutboundEntityModel">出库实体对象</param>
        /// <param name="billModel">出库前运单对象</param>
        /// <param name="afterCondition">出库后置条件</param>
        private void InitOutboundBillModel(OutboundEntityModel OutboundEntityModel, OutboundBillModel billModel, OutboundAfterConditionModel afterCondition)
        {
            billModel.Status = afterCondition.AfterStatus;
            billModel.CurrentDistributionCode = afterCondition.CurrentDistributionCode;
            billModel.OutboundKey = OutboundEntityModel.OBID;
            billModel.UpdateBy = UserContext.CurrentUser.ID;
            billModel.UpdateDept = UserContext.CurrentUser.DeptID;
        }


        /// <summary>
        /// 出库写入数据
        /// </summary>
        /// <param name="argModel">出库参数对象</param>
        /// <param name="billModel">运单对象</param>
        /// <returns></returns>
        protected virtual ResultModel Outbound_Write(IOutboundArgModel argModel, OutboundBillModel billModel, String BatchNo, OutboundAfterConditionModel afterCondition)
        {
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                var OutboundModel = CreateOutboundEntityModel(billModel, BatchNo);
                outboundDAL.Add(OutboundModel);
                InitOutboundBillModel(OutboundModel, billModel, afterCondition);
                BillBLL.UpdateBillModelByOutbound(billModel);
                var billChangeLog = new BillChangeLogDynamicModel()
                {
                    FormCode = billModel.FormCode,
                    CurrentDistributionCode = billModel.CurrentDistributionCode,
                    DeliverStationID = billModel.DeliverStationID,
                    CurrentSatus = billModel.Status,
                    OperateType = Enums.TmsOperateType.Outbound,
                    PreStatus = argModel.PreCondition.PreStatus,
                    CreateBy = UserContext.CurrentUser.ID,
                    CreateDept = UserContext.CurrentUser.DeptID
                };
                billChangeLog.ExtendedObj.ArrivalMnemonicName = argModel.ToStation.MnemonicName;
                billChangeLog.ExtendedObj.SortCenterMnemonicName = argModel.OpUser.MnemonicName;
                WriteBillChangeLog(billChangeLog);
                scope.Complete();
            }
            return new ResultModel().Succeed();
        }


        /// <summary>
        /// 出库验证
        /// </summary>
        /// <param name="argModel">出库参数对象</param>
        /// <param name="FormType">单号类型</param>
        /// <param name="InputCode">单号</param>
        /// <returns></returns>
        protected virtual InnerResultModel Outbound_Validate(IOutboundArgModel argModel, Enums.SortCenterFormType FormType, String InputCode)
        {
            var Result = new InnerResultModel();
            //检查运单号合法性,并以运单号作为出库
            String FormCode = "";
            var checkResult = base.ValidateFormCode(FormType, InputCode, out FormCode);
            if (!checkResult.IsSuccess)
            {
                return Result.Failed(checkResult.Message) as InnerResultModel;
            }
            var outboundBillModel = BillBLL.GetOutboundBillModel(FormCode);
            if (outboundBillModel == null)
            {
                return Result.Failed("运单号不存在") as InnerResultModel;
            }
            Result.BillModel = outboundBillModel;

            if (outboundBillModel.Status != argModel.PreCondition.PreStatus)
            {
                return Result.Failed("运单状态不符合出库条件!") as InnerResultModel;
            }
            if (String.IsNullOrWhiteSpace(outboundBillModel.InboundKey))
            {
                return Result.Failed("订单尚未入库!") as InnerResultModel;
            }
            if (!String.IsNullOrWhiteSpace(outboundBillModel.OutboundKey))
            {
                return Result.Failed("订单已操作出库!") as InnerResultModel;
            }
            if (outboundBillModel.DepartureID != argModel.OpUser.ExpressId.Value)
            {
                return Result.Failed("非本分拣中心订单，不能出库!") as InnerResultModel;
            }
            if (outboundBillModel.ArrivalID != argModel.ToStation.ExpressCompanyID)
            {
                return Result.Failed("出库目的地与入库分拣时选择的不一致！") as InnerResultModel;
            }
            if (argModel.ToStation.SortingType == Enums.SortCenterOperateType.SimpleSorting)
            {
                if (outboundBillModel.InboundType != Enums.SortCenterOperateType.SimpleSorting
                    && outboundBillModel.InboundType != Enums.SortCenterOperateType.TurnSorting)
                {
                    return Result.Failed("订单出库分拣与入库分拣操作方式不一致!") as InnerResultModel;
                }
            }
            else
            {
                if (outboundBillModel.InboundType != argModel.ToStation.SortingType)
                {
                    return Result.Failed("订单出库分拣与入库分拣操作方式不一致!") as InnerResultModel;
                }
            }

            return Result.Succeed() as InnerResultModel;
        }


        /// <summary>
        /// 逐单出库
        /// </summary>
        /// <param name="argument">逐单出库对象</param>
        /// <returns></returns>
        public ViewOutboundSimpleModel SimpleOutbound(OutboundSimpleArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("OutboundSimpleArgModel is null");
            if (argument.OpUser == null) throw new ArgumentNullException("OutboundSimpleArgModel.OpUser  is null");
            if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSimpleArgModel.PreCondition  is null");
            if (argument.ToStation == null) throw new ArgumentNullException("OutboundSimpleArgModel.ToStation  is null");
            ViewOutboundSimpleModel Result = new ViewOutboundSimpleModel();
            if (argument.OpUser.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            {
                return Result.Failed("请用分拣中心帐号操作转站入库!") as ViewOutboundSimpleModel;
            }
            OutboundAfterConditionModel afterCondition = DistributionBLL.GetOutboundAfterConditionModel(
                argument.OpUser.DistributionCode,
                argument.ToStation.DistributionCode,
                argument.ToStation.SortingType);
            if (afterCondition == null)
            {
                return Result.Failed("木有取得出库后置配置条件") as ViewOutboundSimpleModel;
            }
            var SMSConfig = GetSMSConfig();

            var checkResult = Outbound_Validate(argument, argument.FormType, argument.FormCode);
            if (!checkResult.IsSuccess)
            {
                return Result.Failed(checkResult.Message) as ViewOutboundSimpleModel;
            }

            if (String.IsNullOrWhiteSpace(argument.BatchNo))
            {
                String BatchNo = batchNoGenerator.Execute(new SerialNumberModel() { FillerCharacter = "0", NumberLength = 6 });
                if (String.IsNullOrWhiteSpace(BatchNo))
                {
                    return Result.Failed("出库批次号产生失败") as ViewOutboundSimpleModel;
                }
                var batchModel = new OutboundBatchEntityModel()
                {
                    BatchNo = BatchNo,
                    DepartureID = argument.OpUser.ExpressId.Value,
                    ArrivalID = argument.ToStation.ExpressCompanyID,
                    OutboundCount = 0,
                    SyncFlag = Enums.SyncStatus.NotYet,
                    CreateBy = UserContext.CurrentUser.ID,
                    UpdateBy = UserContext.CurrentUser.ID
                };
                batchModel.OBBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = batchModel.SequenceName, TableCode = batchModel.TableCode });
                if (outboundbatchDAL.Add(batchModel) <= 0)
                {
                    return Result.Failed("产生出库批次数据失败，请重试.") as ViewOutboundSimpleModel;
                }
                argument.BatchNo = BatchNo;
            }

            var writeResult = Outbound_Write(argument, checkResult.BillModel, argument.BatchNo, afterCondition);
            if (!writeResult.IsSuccess)
            {
                return Result.Failed(writeResult.Message) as ViewOutboundSimpleModel;
            }
            //var smsresult = SMSValidate(checkResult.BillModel, SMSConfig);
            //if (smsresult != null && smsresult.IsSuccess)
            //{
            //    OutboundSMSSend(checkResult.BillModel, smsresult.Content);
            //}

            Result.BatchNo = argument.BatchNo;
            Result.FormCode = checkResult.BillModel.FormCode;
            Result.CustomerOrder = checkResult.BillModel.CustomerOrder;

            return Result.Succeed() as ViewOutboundSimpleModel;
        }

        /// <summary>
        /// 取得需要出库的运单列表信息
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        public ViewOutboundSearchListModel GetNeededOutboundInfo(OutboundSearchArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("OutboundSearchArgModel is null.");
            if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
            if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
            if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
            if (!argument.InboundStartTime.HasValue) throw new ArgumentNullException("OutboundSearchArgModel.InboundStartTime is null or empty");
            if (!argument.InboundEndTime.HasValue) throw new ArgumentNullException("OutboundSearchArgModel.InboundEndTime is null or empty");
            ViewOutboundSearchListModel result = new ViewOutboundSearchListModel();
            result.FormCodeList = outboundDAL.GetNeededOutboundFormCodeList(argument);
            result.InboundingCount = outboundDAL.GetInboundingCount(argument);
            return result;
        }


        public ViewOutboundBatchModel BatchOutbound(OutboundBatchArgModel argument)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 创建出库实体对象列表
        /// </summary>
        /// <param name="listBillModel">运单对象列表</param>
        /// <returns></returns>
        private List<OutboundEntityModel> CreateOutboundEntityModelList(List<OutboundBillModel> listBillModel, String BatchNo)
        {
            List<OutboundEntityModel> listOutboundModel = new List<OutboundEntityModel>(listBillModel.Count);
            listBillModel.ForEach(p =>
            {
                var outboundmodel = new OutboundEntityModel()
                {
                    ArrivalID = p.ArrivalID,
                    BatchNo = BatchNo,
                    CreateBy = UserContext.CurrentUser.ID,
                    DepartureID = p.DepartureID,
                    FormCode = p.FormCode,
                    IsDeleted = false,
                    OutboundType = p.InboundType,
                    SyncFlag = Enums.SyncStatus.NotYet,
                    UpdateBy = UserContext.CurrentUser.ID
                };
                outboundmodel.OBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = outboundmodel.SequenceName, TableCode = outboundmodel.TableCode });
                listOutboundModel.Add(outboundmodel);
            });
            return listOutboundModel;
        }

        /// <summary>
        /// 初始化出库后的运单对象[修改内部属性]
        /// </summary>
        /// <param name="listOutboundEntityModel">出库实体列表对象</param>
        /// <param name="listBillModel">出库前运单列表对象</param>
        /// <param name="afterCondition">出库后置列表条件</param>
        private void InitOutboundBillModel(List<OutboundEntityModel> listOutboundEntityModel, List<OutboundBillModel> listBillModel, OutboundAfterConditionModel afterCondition)
        {
            listBillModel.ForEach(p =>
            {
                var obModel = listOutboundEntityModel.FirstOrDefault(tmp => tmp.FormCode == p.FormCode);
                if (obModel == null)
                {
                    throw new Exception("出库实体对象列表信息有误");
                }
                p.Status = afterCondition.AfterStatus;
                p.CurrentDistributionCode = afterCondition.CurrentDistributionCode;
                p.OutboundKey = obModel.OBID;
                p.UpdateBy = UserContext.CurrentUser.ID;
                p.UpdateDept = UserContext.CurrentUser.DeptID;
            });
        }


        /// <summary>
        /// 查询出库 
        /// </summary>
        /// <param name="argument">查询出库参数对象</param>
        /// <returns></returns>
        public ViewOutboundBatchModel SearchOutbound(OutboundSearchArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("OutboundSearchArgModel is null");
            if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
            if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
            if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
            if (argument.PerBatchCount <= 0) throw new ArgumentException("PerBatchCount must > 0", "argument.PerBatchCount", null);
            if (argument.FormType != Enums.SortCenterFormType.Waybill) throw new ArgumentException("FormType must equal Enums.SortCenterFormType.Waybill", "argument.FormType", null);
            ViewOutboundBatchModel result = new ViewOutboundBatchModel() { ListErrorBill = new List<SortCenterBatchErrorModel>() };
            List<SortCenterBatchErrorModel> validatedErrorList = null;
            var ValidatedFormCodeList = ValidateFormCode(argument.FormType, argument.ArrFormCode, out validatedErrorList);
            if (validatedErrorList != null)
            {
                result.ListErrorBill.AddRange(validatedErrorList);
            }
            if (ValidatedFormCodeList == null || ValidatedFormCodeList.Count <= 0)
            {
                string firstError = validatedErrorList != null ? validatedErrorList.First().ErrorMsg : "";
                return result.Failed(string.Format("选择列表单据验证未通过{0}", firstError)) as ViewOutboundBatchModel;
            }
            OutboundAfterConditionModel afterCondition = DistributionBLL.GetOutboundAfterConditionModel(
                argument.OpUser.DistributionCode,
                argument.ToStation.DistributionCode,
                argument.ToStation.SortingType);
            if (afterCondition == null)
            {
                return result.Failed("木有取得出库后置配置条件") as ViewOutboundBatchModel;
            }
            ///城际目的地ID
            int tmsArrivalID = -1;
            bool IsNeededDoTMS = base.IsNeedTMSTransfer(argument.OpUser.ExpressId.Value, argument.ToStation.ExpressCompanyID, out tmsArrivalID);
            String BatchNo = batchNoGenerator.Execute(new SerialNumberModel() { FillerCharacter = "0", NumberLength = 6 });
            if (String.IsNullOrWhiteSpace(BatchNo))
            {
                return result.Failed("出库批次号产生失败") as ViewOutboundBatchModel;
            }
            var batchModel = new OutboundBatchEntityModel()
            {
                BatchNo = BatchNo,
                DepartureID = argument.OpUser.ExpressId.Value,
                ArrivalID = argument.ToStation.ExpressCompanyID,
                OutboundCount = 0,
                SyncFlag = Enums.SyncStatus.NotYet,
                CreateBy = UserContext.CurrentUser.ID,
                UpdateBy = UserContext.CurrentUser.ID
            };
            batchModel.OBBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = batchModel.SequenceName, TableCode = batchModel.TableCode });
            if (outboundbatchDAL.Add(batchModel) <= 0)
            {
                return result.Failed("产生出库批次数据失败，请重试.") as ViewOutboundBatchModel;
            }
            int nDivValue = ValidatedFormCodeList.Count / argument.PerBatchCount;                   //倍数
            int nRemaiderValue = ValidatedFormCodeList.Count % argument.PerBatchCount;        //余数
            int nBatchCount = nDivValue + (nRemaiderValue == 0 ? 0 : 1);                                    //总批量处理的计数
            int nSuccessfulCount = 0;                                                                                           //出库成功的数量
            List<String> listFormCode = null;
            List<SortCenterBatchErrorModel> listErrorFormCode = new List<SortCenterBatchErrorModel>();
            Dictionary<OutboundBillModel, String> dicOutboundSMS = new Dictionary<OutboundBillModel, string>();
            var SMSConfig = GetSMSConfig();
            for (int i = 0; i < nBatchCount; i++)
            {
                if (i != nBatchCount - 1)
                {
                    listFormCode = ValidatedFormCodeList.GetRange(i * argument.PerBatchCount, argument.PerBatchCount);
                }
                else
                {
                    listFormCode = ValidatedFormCodeList.GetRange(i * argument.PerBatchCount, nRemaiderValue == 0 ? argument.PerBatchCount : nRemaiderValue);
                }
                var outboundBillInfo = BillBLL.GetOutboundBillModel_SearchOutbound(argument, listFormCode);
                if (outboundBillInfo == null || outboundBillInfo.Count <= 0)
                {
                    listFormCode.ForEach(p =>
                    {
                        listErrorFormCode.Add(new SortCenterBatchErrorModel() { WaybillNo = p, CustomerOrder = "", ErrorMsg = "未找到出库运单信息" });
                    });
                    continue;
                }
                if (outboundBillInfo.Count != listFormCode.Count)
                {
                    listFormCode.ForEach(p =>
                    {
                        var tmpContainObj = outboundBillInfo.FirstOrDefault(mp => mp.FormCode.Equals(p));
                        if (tmpContainObj == null)
                        {
                            listErrorFormCode.Add(new SortCenterBatchErrorModel() { WaybillNo = p, CustomerOrder = "", ErrorMsg = "未找到出库运单信息" });
                        }
                    });
                }
                try
                {
                    using (IACID scope = ACIDScopeFactory.GetTmsACID())
                    {
                        var listOutboundModel = CreateOutboundEntityModelList(outboundBillInfo, BatchNo);
                        outboundDAL.BatchAdd(listOutboundModel);
                        InitOutboundBillModel(listOutboundModel, outboundBillInfo, afterCondition);
                        BillBLL.BatchUpdateBillModelByOutbound(outboundBillInfo);
                        List<BillChangeLogDynamicModel> listChangeLog = new List<BillChangeLogDynamicModel>();
                        outboundBillInfo.ForEach(p =>
                        {
                            var tmpChangeLog = new BillChangeLogDynamicModel()
                            {
                                FormCode = p.FormCode,
                                CurrentDistributionCode = p.CurrentDistributionCode,
                                DeliverStationID = p.DeliverStationID,
                                CurrentSatus = p.Status,
                                OperateType = Enums.TmsOperateType.Outbound,
                                PreStatus = argument.PreCondition.PreStatus,
                                CreateBy = UserContext.CurrentUser.ID,
                                CreateDept = UserContext.CurrentUser.DeptID
                            };
                            tmpChangeLog.ExtendedObj.ArrivalMnemonicName = argument.ToStation.MnemonicName;
                            tmpChangeLog.ExtendedObj.SortCenterMnemonicName = argument.OpUser.MnemonicName;
                            listChangeLog.Add(tmpChangeLog);
                        });
                        WriteBillChangeLog_Batch(listChangeLog);
                        scope.Complete();
                    }
                    nSuccessfulCount += outboundBillInfo.Count;
                    foreach (var item in outboundBillInfo)
                    {
                        var smsresult = SMSValidate(item, SMSConfig);
                        if (smsresult != null && smsresult.IsSuccess)
                        {
                            dicOutboundSMS.Add(item, smsresult.Content);
                        }
                    }
                }
                catch (Exception ex)
                {
                    listFormCode.ForEach(p =>
                    {
                        listErrorFormCode.Add(new SortCenterBatchErrorModel() { WaybillNo = p, CustomerOrder = "", ErrorMsg = ex.Message });
                    });
                }
            }
            if (IsNeededDoTMS && nSuccessfulCount > 0)
            {
                if (sc_syn_tms_outboxDAL.Add(new SC_SYN_TMS_OutboxEntityModel()
                {
                    BoxNo = BatchNo,
                    DepartureID = argument.OpUser.ExpressId.Value,
                    ArrivalID = argument.ToStation.ExpressCompanyID,
                    SC2TMSFlag = Enums.SyncStatus.NotYet,
                    NoType = Enums.SyncNoType.Batch
                }) <= 0)
                {
                    throw new Exception("产生分拣到TMS成绩运输临时中间数据失败");
                }
            }
            //if (dicOutboundSMS != null && dicOutboundSMS.Count > 0)
            //{
            //    OutboundSMSSend(dicOutboundSMS);
            //}
            result.SucceedCount = nSuccessfulCount;
            result.ListErrorBill.AddRange(listErrorFormCode);
            result.FailedCount = result.ListErrorBill.Count;
            return result.Succeed(String.Format("出库成功:{0},失败:{1}", result.SucceedCount, result.FailedCount)) as ViewOutboundBatchModel;
        }

        public ViewOutboundPackingModel PackingOutbound(OutboundPackingArgModel argument)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 查询返货目的地
        /// </summary>
        /// <returns></returns>
        public string GetReturnTo(string formCode, int arrivalID)
        {
            if (string.IsNullOrWhiteSpace(formCode)) throw new ArgumentNullException();
            return outboundDAL.GetReturnTo(formCode, arrivalID);
        }
        #endregion


        #region IOutboundBLL 成员


        public List<ViewOutBoundByBoxModel> SearchOutBoundByBox(OutboundSearchModel argument)
        {
            if (argument == null) throw new ArgumentNullException("OutboundSearchArgModel is null.");
            if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
            //if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
            //if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
            if (!argument.InboundStartTime.HasValue) throw new ArgumentNullException("OutboundSearchArgModel.InboundStartTime is null or empty");
            if (!argument.InboundEndTime.HasValue) throw new ArgumentNullException("OutboundSearchArgModel.InboundEndTime is null or empty");
            return outboundDAL.GetNeededOutBoundBoxList(argument);
        }

        #endregion

        #region IOutboundBLL 成员


        public ViewOutBoundBoxDetailModel GetBoxBillsByBoxNo(string boxNo)
        {
            ViewOutBoundBoxDetailModel detail = outboundDAL.GetNeededOutBoundBillListByBoxNo(boxNo);
            if (detail != null)
            {
                ViewOutBoundByBoxModel boxInfo = GetBoxInfoByBoxNo(boxNo);
                detail.BillCount = boxInfo.BillCount;
                detail.BoxNo = boxInfo.BoxNo;
                detail.Weight = boxInfo.Weight;
            }
            return detail;
        }

        #endregion

        #region IOutboundBLL 成员


        public ViewOutBoundByBoxModel GetBoxInfoByBoxNo(string boxNo)
        {
            return outboundDAL.GetBoxInfoByBoxNo(boxNo);
        }

        #endregion

        #region IOutboundBLL 成员


        public ResultModel OutboundByBox(OutboundByBoxArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("argument is null");
            if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
            if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
            if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
            if (argument.BoxNos == null || argument.BoxNos.Count == 0) throw new ArgumentNullException("OutboundSearchArgModel.BoxNos is null or empty");
            ResultModel result = new ResultModel();
            int sCount = 0;
            foreach (var item in argument.BoxNos)
            {
                result = BoxOutbound(new OutboundByBoxArgModel()
                {
                    OpUser = argument.OpUser,
                    PreCondition = argument.PreCondition,
                    ToStation = argument.ToStation,
                    CurrentBoxNo = item
                });

                if (!result.IsSuccess)
                {
                    continue;
                }
                else
                {
                    sCount++;
                }
            }
            if (sCount < argument.BoxNos.Count)
                result.Failed("成功出库" + sCount.ToString() + "箱;失败" + (argument.BoxNos.Count - sCount).ToString() + "箱");
            else
                result.Succeed("出库成功");
            return result;
        }

        /// <summary>
        /// 按箱出库 
        /// </summary>
        /// <param name="argument">按箱出库参数对象</param>
        /// <returns></returns>
        private ResultModel BoxOutbound(OutboundByBoxArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("OutboundSearchArgModel is null");
            if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
            if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
            if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
            if (string.IsNullOrWhiteSpace(argument.CurrentBoxNo)) throw new ArgumentNullException("OutboundSearchArgModel.BoxNo is null or empty");
            argument.CurrentFormCodes = outboundDAL.GetNeededOutBoundBillListByBoxNo(argument.CurrentBoxNo).FormCodes;
            ViewOutboundBatchModel result = new ViewOutboundBatchModel() { ListErrorBill = new List<SortCenterBatchErrorModel>() };
            List<SortCenterBatchErrorModel> validatedErrorList = null;
            var ValidatedFormCodeList = ValidateFormCode(Enums.SortCenterFormType.Waybill, argument.CurrentFormCodes.ToArray(), out validatedErrorList);
            if (validatedErrorList != null)
            {
                result.ListErrorBill.AddRange(validatedErrorList);
            }
            if (ValidatedFormCodeList == null || ValidatedFormCodeList.Count <= 0)
            {
                string firstError = validatedErrorList != null ? validatedErrorList.First().ErrorMsg : "";
                return result.Failed(string.Format("选择列表单据验证未通过{0}",firstError)) as ViewOutboundBatchModel;
            }
            OutboundAfterConditionModel afterCondition = DistributionBLL.GetOutboundAfterConditionModel(
                argument.OpUser.DistributionCode,
                argument.ToStation.DistributionCode,
                argument.ToStation.SortingType);
            if (afterCondition == null)
            {
                return result.Failed("木有取得出库后置配置条件") as ViewOutboundBatchModel;
            }
            ///城际目的地ID
            int tmsArrivalID = -1;
            bool IsNeededDoTMS = base.IsNeedTMSTransfer(argument.OpUser.ExpressId.Value, argument.ToStation.ExpressCompanyID, out tmsArrivalID);
            
            int nSuccessfulCount = 0;
            //List<SortCenterBatchErrorModel> listErrorFormCode = new List<SortCenterBatchErrorModel>();
            Dictionary<OutboundBillModel, String> dicOutboundSMS = new Dictionary<OutboundBillModel, string>();
            var SMSConfig = GetSMSConfig();
            var outboundBillInfo = BillBLL.GetOutboundBillModel_SearchOutbound(argument, ValidatedFormCodeList);
            if (outboundBillInfo.Count != ValidatedFormCodeList.Count)
            {
                return result.Failed("箱子中存在" + (ValidatedFormCodeList.Count - outboundBillInfo.Count).ToString() + "单不符合出库要求订单.") as ViewOutboundBatchModel;
            }
            String BatchNo = argument.CurrentBoxNo;
            try
            {
                using (IACID scope = ACIDScopeFactory.GetTmsACID())
                {
                    var batchModel = new OutboundBatchEntityModel()
                    {
                        BatchNo = BatchNo,
                        DepartureID = argument.OpUser.ExpressId.Value,
                        ArrivalID = argument.ToStation.ExpressCompanyID,
                        OutboundCount = 0,
                        SyncFlag = Enums.SyncStatus.NotYet,
                        CreateBy = UserContext.CurrentUser.ID,
                        UpdateBy = UserContext.CurrentUser.ID
                    };
                    batchModel.OBBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = batchModel.SequenceName, TableCode = batchModel.TableCode });
                    if (outboundbatchDAL.Add(batchModel) <= 0)
                    {
                        return result.Failed("产生出库批次数据失败，请重试.") as ViewOutboundBatchModel;
                    }

                    var listOutboundModel = CreateOutboundEntityModelList(outboundBillInfo, BatchNo);
                    outboundDAL.BatchAdd(listOutboundModel);
                    InitOutboundBillModel(listOutboundModel, outboundBillInfo, afterCondition);
                    BillBLL.BatchUpdateBillModelByOutbound(outboundBillInfo);
                    List<BillChangeLogDynamicModel> listChangeLog = new List<BillChangeLogDynamicModel>();
                    outboundBillInfo.ForEach(p =>
                    {
                        var tmpChangeLog = new BillChangeLogDynamicModel()
                        {
                            FormCode = p.FormCode,
                            CurrentDistributionCode = p.CurrentDistributionCode,
                            DeliverStationID = p.DeliverStationID,
                            CurrentSatus = p.Status,
                            OperateType = Enums.TmsOperateType.Outbound,
                            PreStatus = argument.PreCondition.PreStatus,
                            CreateBy = UserContext.CurrentUser.ID,
                            CreateDept = UserContext.CurrentUser.DeptID
                        };
                        tmpChangeLog.ExtendedObj.ArrivalMnemonicName = argument.ToStation.MnemonicName;
                        tmpChangeLog.ExtendedObj.SortCenterMnemonicName = argument.OpUser.MnemonicName;
                        listChangeLog.Add(tmpChangeLog);
                    });
                    WriteBillChangeLog_Batch(listChangeLog);
                    SetBoxOutBoundStatus(argument.CurrentBoxNo, true);
                    scope.Complete();
                }
                nSuccessfulCount += outboundBillInfo.Count;
                foreach (var item in outboundBillInfo)
                {
                    var smsresult = SMSValidate(item, SMSConfig);
                    if (smsresult != null && smsresult.IsSuccess)
                    {
                        dicOutboundSMS.Add(item, smsresult.Content);
                    }
                }
            }
            catch (Exception ex)
            {
                return result.Failed("出库异常:" + ex.Message) as ViewOutboundBatchModel;
            }
            if (IsNeededDoTMS && nSuccessfulCount > 0)
            {
                if (sc_syn_tms_outboxDAL.Add(new SC_SYN_TMS_OutboxEntityModel()
                {
                    BoxNo = BatchNo,
                    DepartureID = argument.OpUser.ExpressId.Value,
                    ArrivalID = argument.ToStation.ExpressCompanyID,
                    SC2TMSFlag = Enums.SyncStatus.NotYet,
                    NoType = Enums.SyncNoType.Box
                }) <= 0)
                {
                    throw new Exception("产生分拣到TMS成绩运输临时中间数据失败");
                }
            }
            //if (dicOutboundSMS != null && dicOutboundSMS.Count > 0)
            //{
            //    OutboundSMSSend(dicOutboundSMS);
            //}
            return result.Succeed(String.Format("出库成功")) as ViewOutboundBatchModel;
        }

        #endregion

        #region IOutboundBLL 成员


        public int SetBoxOutBoundStatus(string boxNo, bool isOutBounded)
        {
            return outboundDAL.SetBoxOutBoundStatus(boxNo, isOutBounded);
        }

        #endregion
    }
}
