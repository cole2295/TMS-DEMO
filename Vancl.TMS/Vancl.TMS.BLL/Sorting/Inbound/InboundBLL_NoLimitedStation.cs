using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.BLL.Sorting.Inbound
{
    /// <summary>
    /// 入库不限制站点
    /// </summary>
    public partial class InboundBLL
    {
        /// <summary>
        /// 构建入库实体对象
        /// </summary>
        /// <param name="billModel">入库运单对象</param>
        /// <param name="argModel">入库参数对象</param>
        /// <returns></returns>
        private InboundEntityModel CreateInboundEntityModel(InboundBillModel billModel, IInboundArgModel argModel)
        {
            var InboundEntityModel = new InboundEntityModel()
            {
                ApplyStation = null,
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
        /// 入库【不限制站点】【写最终数据】
        /// </summary>
        /// <param name="billModel"></param>
        /// <param name="argModel"></param>
        /// <returns></returns>
        private ViewInboundSimpleModel Inbound_NoLimitedStation_Write(InboundBillModel billModel, IInboundArgModel argModel)
        {
            ViewInboundSimpleModel result = new ViewInboundSimpleModel();
            var billPreStatus = billModel.Status;
            billModel.Status = Enums.BillStatus.HaveBeenSorting;
            billModel.UpdateBy = Convert.ToInt32(argModel.OpUser.UserId);
            billModel.UpdateDept = Convert.ToInt32(argModel.OpUser.ExpressId.Value);
            billModel.DeliverStationID = argModel.ToStation.ExpressCompanyID;       //需要修改配送站信息
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                var inboundModel = CreateInboundEntityModel(billModel, argModel);
                if (InboundDAL.Add(inboundModel) <= 0)
                {
                    throw new Exception(String.Format("系统运单:{0}写SC_Inbound表失败", billModel.FormCode));
                }
                billModel.InboundKey = inboundModel.IBID;
                billModel.OutboundKey = null;
                if (!BillBLL.UpdateBillModelByInbound_NoLimitedStation(billModel))
                {
                    throw new Exception(String.Format("系统运单:{0}更新SC_Bill表失败", billModel.FormCode));
                }
                var billchangelogDynamicModel = new Model.Log.BillChangeLogDynamicModel()
                {
                    FormCode = billModel.FormCode,
                    CurrentDistributionCode = billModel.CurrentDistributionCode,
                    DeliverStationID = billModel.DeliverStationID,
                    CurrentSatus = billModel.Status,
                    OperateType = Enums.TmsOperateType.Inbound_NoLimitedStation,
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
            return result.Succeed() as ViewInboundSimpleModel;
        }


        /// <summary>
        /// 入库【不限制站点】【验证】
        /// </summary>
        /// <param name="argModel"></param>
        /// <param name="FormType"></param>
        /// <param name="InputCode"></param>
        /// <returns></returns>
        private InnerResultModel Inbound_NoLimitedStation_Validate(IInboundArgModel argModel, Enums.SortCenterFormType FormType, String InputCode)
        {
            var Result = new InnerResultModel();
            String FormCode = null;
            //检查运单号合法性,并以系统运单号作为主单号进行操作
            var checkResult = base.ValidateFormCode(FormType, InputCode, out FormCode);
            if (!checkResult.IsSuccess)
            {
                return Result.Failed(checkResult.Message) as InnerResultModel;
            }
            InboundBillModel billModel = BillBLL.GetInboundBillModel_NoLimitedStation(FormCode);
            Result.BillModel = billModel;
            if (billModel == null)
            {
                return Result.Failed("运单号不存在!") as InnerResultModel;
            }
            if (billModel.BillType == Enums.BillType.Return)
            {
                return Result.Failed("该订单为上门退货单，不允许操作!") as InnerResultModel;
            }
            if (!String.IsNullOrWhiteSpace(billModel.TurnstationKey))
            {
                return Result.Failed("此运单为转站运单，请在转站入库中操作!") as InnerResultModel;
            }
            //待入库，已分拣，均不限制入库
            if (billModel.Status != argModel.PreCondition.PreStatus
                && billModel.Status != Enums.BillStatus.HaveBeenSorting)
            {
                return Result.Failed("运单状态不符合入库条件!") as InnerResultModel;
            } 
            if (!InboundDAL.ValidateBillWeight(billModel.FormCode))
            {
                return Result.Failed("此单未称重，请输入重量后再操作入库!") as InnerResultModel;
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
            if (InboundDAL.ExistsLine_EqualLastInboud(billModel.FormCode, argModel.OpUser.ExpressId.Value, argModel.ToStation.ExpressCompanyID))
            {
                return Result.Failed("此运单已分拣入库相同的站点，请勿重复操作!") as InnerResultModel;
            }

            return Result.Succeed() as InnerResultModel;
        }


        /// <summary>
        /// 逐单入库【不限制站点】
        /// </summary>
        /// <param name="argument">参数</param>
        /// <returns>逐单入库 View Model</returns>
        public ViewInboundSimpleModel SimpleInbound_NoLimitedStation(InboundSimpleArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundSimpleArgModel  is null");
            if (argument.OpUser == null) throw new ArgumentNullException("InboundSimpleArgModel.OpUser is null");
            if (argument.ToStation == null) throw new ArgumentNullException("InboundSimpleArgModel.ToStation is null");
            if (argument.PreCondition == null) throw new ArgumentNullException("InboundSimpleArgModel.PreCondition is null");
            
            ViewInboundSimpleModel Result = new ViewInboundSimpleModel();
            if (argument.OpUser.CompanyFlag != Model.Common.Enums.CompanyFlag.SortingCenter)
            {
                return Result.Failed("请用分拣中心帐号操作入库!") as ViewInboundSimpleModel;
            }
            var checkResult = Inbound_NoLimitedStation_Validate(argument, argument.FormType, argument.FormCode);
            if (!checkResult.IsSuccess)
            {
                return Result.Failed(checkResult.Message) as ViewInboundSimpleModel;
            }
            var writeResult = Inbound_NoLimitedStation_Write(checkResult.BillModel, argument);
            if (!writeResult.IsSuccess)
            {
                return Result.Failed(writeResult.Message) as ViewInboundSimpleModel;
            }

            Result.FormCode = checkResult.BillModel.FormCode;
            Result.CustomerOrder = checkResult.BillModel.CustomerOrder;
            int nCurrentInboundCount = GetInboundCount(argument);
            Result.InboundCount = nCurrentInboundCount;

            return Result.Succeed() as ViewInboundSimpleModel;
        }

    }
}
