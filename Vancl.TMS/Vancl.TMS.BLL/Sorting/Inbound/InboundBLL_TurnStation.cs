using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound.TurnStation;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.BLL.CloudBillService;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Sorting.Common;

namespace Vancl.TMS.BLL.Sorting.Inbound
{
    public partial class InboundBLL
    {
        /// <summary>
        /// 构建转站入库实体对象
        /// </summary>
        /// <param name="billModel">入库对象</param>
        /// <param name="argModel">入库参数</param>
        /// <param name="turnStationInfo">转站对象</param>
        /// <returns></returns>
        private InboundEntityModel CreateTurnInboundEntityModel(InboundBillModel billModel, IInboundArgModel argModel, TurnStationModel turnStationInfo)
        {
            var InboundEntityModel = new InboundEntityModel()
            {
                ApplyStation = turnStationInfo.ApplyStation,
                ArrivalID = argModel.ToStation.ExpressCompanyID,
                CreateBy = argModel.OpUser.UserId,
                DepartureID = argModel.OpUser.ExpressId.Value,
                FormCode = billModel.FormCode,
                InboundType = String.IsNullOrWhiteSpace(billModel.TurnstationKey) ? argModel.ToStation.SortingType : Enums.SortCenterOperateType.TurnSorting,
                IsDeleted = false,
                SyncFlag = Enums.SyncStatus.NotYet,
                UpdateBy = argModel.OpUser.UserId
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
        /// 转站入库写数据
        /// </summary>
        /// <param name="billModel">入库对象</param>
        /// <param name="argModel">入库参数对象</param>
        /// <param name="turnStationInfo">转站对象</param>
        /// <returns></returns>
        private ViewTurnInboundSimpleModel TurnInbound_Write(InboundBillModel billModel, IInboundArgModel argModel, TurnStationModel turnStationInfo)
        {
            ViewTurnInboundSimpleModel result = new ViewTurnInboundSimpleModel();
            var billPreStatus = billModel.Status;
            billModel.Status = Enums.BillStatus.HaveBeenSorting;
            billModel.UpdateBy = Convert.ToInt32(argModel.OpUser.UserId);
            billModel.UpdateDept = Convert.ToInt32(argModel.OpUser.ExpressId.Value);
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                var inboundModel = CreateTurnInboundEntityModel(billModel, argModel, turnStationInfo);
                if (InboundDAL.Add(inboundModel) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_Inbound表失败", billModel.FormCode));
                }
                billModel.InboundKey = inboundModel.IBID;
                billModel.OutboundKey = null;
                if (!BillBLL.UpdateBillModelByTurnInbound(billModel))
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
                    CreateBy = billModel.UpdateBy,
                    CreateDept = billModel.UpdateDept
                };
                billchangelogDynamicModel.ExtendedObj.DistributionMnemonicName = argModel.OpUser.DistributionMnemonicName;
                billchangelogDynamicModel.ExtendedObj.SortCenterMnemonicName = argModel.OpUser.MnemonicName;
                var agentUser = UserContext.AgentUser;
                if (agentUser != null && agentUser.ID > 0)
                {
                    billchangelogDynamicModel.ExtendedObj.AgentUser = agentUser;
                }
                if (WriteBillChangeLog(billchangelogDynamicModel) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_BillChangeLog表失败", billModel.FormCode));
                }
                scope.Complete();
            }
            return result.Succeed() as ViewTurnInboundSimpleModel;
        }

        /// <summary>
        /// 转站入库验证
        /// </summary>
        /// <param name="argModel"></param>
        /// <param name="FormType"></param>
        /// <param name="InputCode"></param>
        /// <returns></returns>
        private InnerResultModel TurnInbound_Validate(IInboundArgModel argModel, Enums.SortCenterFormType FormType, String InputCode)
        {
            var Result = new InnerResultModel();
            String FormCode = null;
            //检查运单号合法性,并以系统运单号作为主单号进行操作
            var checkResult = base.ValidateFormCode(FormType, InputCode, out FormCode);
            if (!checkResult.IsSuccess)
            {
                return Result.Failed(checkResult.Message) as InnerResultModel;
            }
            InboundBillModel billModel = BillBLL.GetInboundBillModel_TurnStation(FormCode);
            Result.BillModel = billModel;
            if (billModel == null)
            {
                return Result.Failed("订单号不存在.") as InnerResultModel;
            }
            if (billModel.Status != Enums.BillStatus.WaitingInbound)
            {
                return Result.Failed("订单状态不符合入库条件.") as InnerResultModel;
            }
            if (billModel.BillType == Enums.BillType.Return)
            {
                return Result.Failed("上门退货单,不允许操作转站入库.") as InnerResultModel;
            }
            if (String.IsNullOrWhiteSpace(billModel.TurnstationKey))
            {
                return Result.Failed("此订单非转站订单,请在入库分拣中操作.") as InnerResultModel;
            }
            using (BillServiceClient billProxy = new BillServiceClient())
            {
                var turnStationInfo = billProxy.GetTurnStationModel(long.Parse(billModel.TurnstationKey));
                if (turnStationInfo == null)
                {
                    return Result.Failed("未从物流系统取得此运单的转站申请记录,请先申请.") as InnerResultModel;
                }
                if (turnStationInfo.Isfast)
                {
                    return Result.Failed("快捷转站订单,不允许操作入库.") as InnerResultModel;
                }
                //转站申请通过之后，运单当前站点应该更新为申请的目的地接收站点
                if (billModel.DeliverStationID != turnStationInfo.RecvStation)
                {
                    return Result.Failed("此运单转站接收站点同当前运单实际配送的站点不相同.可能未从物流系统同步成功") as InnerResultModel;
                }
                if (billModel.DeliverStationID != argModel.ToStation.ExpressCompanyID)
                {
                    return Result.Failed("此运单转站接收站点同选择的站点不相同.") as InnerResultModel;
                }
                Result.TurnStationInfo = turnStationInfo;
            }
            if (!SortCenterDAL.IsBillBelongSortCenter(argModel.OpUser.ExpressId.Value, billModel))
            {
                return Result.Failed("此订单不属于当前分拣中心.") as InnerResultModel;
            }
            return Result.Succeed() as InnerResultModel;
        }

        /// <summary>
        /// 逐单转站入库
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>转站入库View Model</returns>
        public ViewTurnInboundSimpleModel TurnInbound(InboundSimpleArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("TurnInbound InboundSimpleArgModel  is null");
            if (argument.OpUser == null) throw new ArgumentNullException("TurnInbound InboundSimpleArgModel.OpUser is null");
            if (argument.ToStation == null) throw new ArgumentNullException("TurnInbound InboundSimpleArgModel.ToStation is null");
            if (argument.PreCondition == null) throw new ArgumentNullException("TurnInbound InboundSimpleArgModel.PreCondition is null");
            ViewTurnInboundSimpleModel Result = new ViewTurnInboundSimpleModel();
            if (argument.OpUser.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            {
                return Result.Failed("请用分拣中心帐号操作转站入库!") as ViewTurnInboundSimpleModel;
            }
            var checkResult = TurnInbound_Validate(argument, argument.FormType, argument.FormCode);
            if (!checkResult.IsSuccess)
            {
                return Result.Failed(checkResult.Message) as ViewTurnInboundSimpleModel;
            }
            var writeResult = TurnInbound_Write(checkResult.BillModel, argument, checkResult.TurnStationInfo);
            if (!writeResult.IsSuccess)
            {
                return Result.Failed(writeResult.Message) as ViewTurnInboundSimpleModel;                
            }

            Result.FormCode = checkResult.BillModel.FormCode;
            Result.CustomerOrder = checkResult.BillModel.CustomerOrder;

            return Result.Succeed() as ViewTurnInboundSimpleModel;
        }
    }
}
