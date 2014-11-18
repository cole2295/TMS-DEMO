using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.CustomizeFlow;
using Vancl.TMS.BLL.Sorting.Common;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.FormulaManager;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Sorting.Outbound;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.IDAL.Sorting.Outbound;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Outbound;

namespace Vancl.TMS.BLL.Sorting.Outbound
{
	public partial class OutboundBLLV2 : SortCenterBLL, IOutboundBLLV2
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

			/// <summary>
			/// 箱号对象
			/// </summary>
			public OutboundBoxModel BoxModel { get; set; }

		}

		#region IOutboundBLL 成员
		#region 出库相关服务
		/// <summary>
		/// 出库数据层
		/// </summary>
		IOutboundV2DAL outboundV2DAL = ServiceFactory.GetService<IOutboundV2DAL>("SC_OutboundV2DAL");

		private IExpressCompanyDAL expressCompanyDAL = ServiceFactory.GetService<IExpressCompanyDAL>();
		/// <summary>
		/// 出库批次数据层
		/// </summary>
		IOutboundBatchV2DAL outboundbatchV2DAL = ServiceFactory.GetService<IOutboundBatchV2DAL>("SC_OutboundBatchV2DAL");

		/// <summary>
		/// 装箱数据层
		/// </summary>
		ISortingPackingDAL _sortingPackingDAL = ServiceFactory.GetService<ISortingPackingDAL>("SortingPackingDAL");
		/// <summary>
		/// 分拣同步到TMS城际运输数据层
		/// </summary>
		ISC_SYN_TMS_OutboxDAL sc_syn_tms_outboxDAL = ServiceFactory.GetService<ISC_SYN_TMS_OutboxDAL>("SC_SYN_TMS_OutboxDAL");
		/// <summary>
		/// 出库批次号产生算法
		/// </summary>
		IFormula<string, SerialNumberModel> batchNoGenerator = FormulasFactory.GetFormula<IFormula<string, SerialNumberModel>>("OutboundBatchNoGenerateFormula");

		/// <summary>
		/// 流程自定义
		/// </summary>
		private FlowFunFacade TMSFlowFunFacade = new FlowFunFacade();
		
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
				UpdateBy = UserContext.CurrentUser.ID,
				DeliverStationID = billModel.DeliverStationID
			};
			outboundmodel.OBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = outboundmodel.SequenceName, TableCode = outboundmodel.TableCode });
			return outboundmodel;
		}

		/// <summary>
		/// 初始化出库后的运单对象[修改内部属性]
		/// </summary>
		/// <param name="OutboundEntityModel">出库实体对象</param>
		/// <param name="billModel">出库前运单对象</param>
		private void InitOutboundBillModel(OutboundEntityModel OutboundEntityModel, OutboundBillModel billModel, IOutboundArgModel argModel)
		{
			//billModel.Status = afterCondition.AfterStatus;
			billModel.Status = Enums.BillStatus.Outbounded;
			//billModel.CurrentDistributionCode = afterCondition.CurrentDistributionCode;
			//出库到配送商
			if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.Distributor)
			{
				billModel.CurrentDistributionCode = argModel.ToStation.DistributionCode;
				billModel.ToDistributionCode = argModel.ToStation.DistributionCode;
			}
			else
			{
				billModel.CurrentDistributionCode = UserContext.CurrentUser.DistributionCode;
				billModel.ToDistributionCode = null;
			}
			//出库到配送站
			if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.DistributionStation)
			{
				billModel.DeliverStationID = argModel.ToStation.ExpressCompanyID;
				//billModel.ToDeliverstationid = argModel.ToStation.ExpressCompanyID;
			}
			else
			{
				billModel.DeliverStationID = OutboundEntityModel.DeliverStationID;
				//billModel.ToDeliverstationid = 0;
			}
			//出库到分拣中心
			//if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.SortingCenter)
			//{
			//    billModel.ToSortingCenterId = argModel.ToStation.ExpressCompanyID;
			//}
			//else
			//{
			//    billModel.ToSortingCenterId = 0;
			//}
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
		protected virtual ResultModel Outbound_Write(IOutboundArgModel argModel, OutboundBillModel billModel, String BatchNo, int CurrentDisCount,bool isBatchOutbound)
		{
			using (IACID scope = ACIDScopeFactory.GetTmsACID())
			{
				var OutboundModel = CreateOutboundEntityModel(billModel, BatchNo);
				OutboundModel.ArrivalID = argModel.ToStation.ExpressCompanyID;
				outboundV2DAL.Add(OutboundModel);
				InitOutboundBillModel(OutboundModel, billModel,argModel);
				BillBLL.UpdateBillModelByOutboundV2(billModel);
				var billChangeLog = new BillChangeLogDynamicModel()
				{
					FormCode = billModel.FormCode,
					CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
					DeliverStationID = billModel.DeliverStationID,
					ToDistributionCode = billModel.ToDistributionCode,
					ToExpressCompanyID = argModel.ToStation.ExpressCompanyID,
					CurrentSatus = billModel.Status,
					OperateType = Enums.TmsOperateType.Outbound,
					PreStatus = argModel.PreCondition.PreStatus,
					CreateBy = UserContext.CurrentUser.ID,
					CreateDept = UserContext.CurrentUser.DeptID
				};
				billChangeLog.ExtendedObj.ArrivalMnemonicName = argModel.ToStation.MnemonicName;
				billChangeLog.ExtendedObj.SortCenterMnemonicName = argModel.OpUser.MnemonicName;
				//if (isBatchOutbound)
				//{
				//    billChangeLog.OperateType = Enums.TmsOperateType.BatchOutbound;
				//}
				//else
				//{
				//    billChangeLog.OperateType = Enums.TmsOperateType.Outbound;
				//}
				WriteBillChangeLog(billChangeLog);
				//todo：调用出库对应的LifeCycle

				//流程自定义
                WaybillTurn turn = new WaybillTurn();
                turn.WaybillNo = Convert.ToInt64(billModel.FormCode);
                turn.FromExpressCompanyId = Convert.ToInt32(argModel.OpUser.ExpressId);
                turn.ToExpressCompanyId = argModel.ToStation.ExpressCompanyID;
                turn.FromDistributionCode = UserContext.CurrentUser.DistributionCode;
                turn.ToDistributionCode = (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.Distributor)
                                              ? argModel.ToStation.DistributionCode
                                              : UserContext.CurrentUser.DistributionCode;
                turn.TurnType = (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.Distributor)
                                    ? Enums.TurnType.ToDistribution
                                    : Enums.TurnType.ToDep;

                TMSFlowFunFacade.WaybillTurn(turn);
				
				#region 更新出库到当前目的地的数量
				//var OutboundBatchModel = new OutboundBatchEntityModel()
				//    {
				//        BatchNo = BatchNo,
				//        OutboundCount = CurrentDisCount+1,
				//        UpdateBy = UserContext.CurrentUser.ID
				//    };
				//outboundbatchV2DAL.Update(OutboundBatchModel);
				#endregion
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
		protected virtual OutboundBLLV2.InnerResultModel Outbound_Validate(IOutboundArgModel argModel, Enums.SortCenterFormType FormType, String InputCode)
		{
			var Result = new OutboundBLLV2.InnerResultModel();
			//检查运单号合法性,并以运单号作为出库
			String FormCode = "";
			var checkResult = base.ValidateFormCode(FormType, InputCode, out FormCode);
			if (!checkResult.IsSuccess)
			{
				return Result.Failed(checkResult.Message) as OutboundBLLV2.InnerResultModel;
			}
			var outboundBillModel = BillBLL.GetOutboundBillModel(FormCode);
			if (outboundBillModel == null)
			{
				return Result.Failed("运单号不存在") as OutboundBLLV2.InnerResultModel;
			}
			Result.BillModel = outboundBillModel;
			

			if (outboundBillModel.Status != argModel.PreCondition.PreStatus)
			{
				return Result.Failed("运单状态不符合出库条件!") as OutboundBLLV2.InnerResultModel;
			}
			if (String.IsNullOrWhiteSpace(outboundBillModel.InboundKey))
			{
				return Result.Failed("订单尚未入库!") as OutboundBLLV2.InnerResultModel;
			}
			if (!String.IsNullOrWhiteSpace(outboundBillModel.OutboundKey))
			{
				return Result.Failed("订单已操作出库!") as OutboundBLLV2.InnerResultModel;
			}
			if (outboundBillModel.DepartureID != argModel.OpUser.ExpressId.Value)
			{
				return Result.Failed("非本分拣中心订单，不能出库!") as OutboundBLLV2.InnerResultModel;
			}
			if (_sortingPackingDAL.GetBoxNoByFormcode(FormCode) != null)
			{
				return Result.Failed("该单号已装箱，请操作按箱出库！") as OutboundBLLV2.InnerResultModel;
			}
			#region  删除
			//if ((argModel.PreCondition.PreStatus != null) && (outboundBillModel.ArrivalID != argModel.ToStation.ExpressCompanyID))
			//{
			//    return Result.Failed("出库目的地与入库分拣时选择的不一致！") as OutboundBLLV2.InnerResultModel;
			//}
			//if (outboundBillModel.DeliverStationID != argModel.ToStation.ExpressCompanyID)
			//{
			//    return Result.Failed("出库目的地与运单分配的站点的不一致！") as OutboundBLLV2.InnerResultModel;
			//}

			//if (argModel.ToStation.SortingType == Enums.SortCenterOperateType.SimpleSorting)
			//{
			//    if (outboundBillModel.InboundType != Enums.SortCenterOperateType.SimpleSorting
			//        && outboundBillModel.InboundType != Enums.SortCenterOperateType.TurnSorting)
			//    {
			//        return Result.Failed("订单出库分拣与入库分拣操作方式不一致!") as OutboundBLLV2.InnerResultModel;
			//    }
			//}
			//else
			//{
			//    if (outboundBillModel.InboundType != argModel.ToStation.SortingType)
			//    {
			//        return Result.Failed("订单出库分拣与入库分拣操作方式不一致!") as OutboundBLLV2.InnerResultModel;
			//    }
			//}
			#endregion
			return Result.Succeed() as OutboundBLLV2.InnerResultModel;
		}

		protected virtual OutboundBLLV2.InnerResultModel OutboundBox_Validate(OutboundByBoxArgModel argModel,String InputBoxNo)
		{
			var Result = new OutboundBLLV2.InnerResultModel();
			var outboundBoxModel = outboundV2DAL.GetOutboundBoxModel(InputBoxNo);
			if (outboundBoxModel == null)
			{
				return Result.Failed("此箱号不存在") as InnerResultModel;
			}
			Result.BoxModel = outboundBoxModel;

			if (outboundBoxModel.IsOutbounded==1)
			{
				return Result.Failed("此箱号已操作出库!") as InnerResultModel;
			}
			if (outboundBoxModel.DepartureID != argModel.OpUser.ExpressId.Value)
			{
				return Result.Failed("非本分拣中心的箱号，不能出库!") as InnerResultModel;
			}
			if (argModel.PreCondition != null && (outboundBoxModel.ArrivalID != argModel.ToStation.ExpressCompanyID))
			{
				return Result.Failed("出库目的地与入库装箱时选择的不一致！") as InnerResultModel;
			}
			return Result.Succeed() as InnerResultModel;
		}

		protected virtual OutboundBLLV2.InnerResultModel Outbound_ValidateV2(IOutboundArgModel argModel,
		                                                                     Enums.SortCenterFormType FormType,
		                                                                     String InputCode)
		{
			var Result = new OutboundBLLV2.InnerResultModel();
			//检查运单号合法性,并以运单号作为出库
			String FormCode = "";
			var checkResult = base.ValidateFormCode(FormType, InputCode, out FormCode);
			if (!checkResult.IsSuccess)
			{
				return Result.Failed(checkResult.Message) as OutboundBLLV2.InnerResultModel;
			}
			var outboundBillModel = BillBLL.GetOutboundBillModel(FormCode);
			if (outboundBillModel == null)
			{
				return Result.Failed("运单号不存在!") as OutboundBLLV2.InnerResultModel;
			}
			Result.BillModel = outboundBillModel;

			if (outboundBillModel.DepartureID != argModel.OpUser.ExpressId.Value)
			{
				return Result.Failed("非本分拣中心运单，不能出库!") as OutboundBLLV2.InnerResultModel;
			}
			if (outboundBillModel.Status == Enums.BillStatus.Outbounded)
			{
				return Result.Failed("订单已操作出库!") as OutboundBLLV2.InnerResultModel;
			}
			if((outboundBillModel.Status != Enums.BillStatus.HaveBeenSorting)&&(outboundBillModel.Status!=Enums.BillStatus.Inbounded))
			{
				return Result.Failed("订单尚未入库!") as OutboundBLLV2.InnerResultModel;
			}
			if (_sortingPackingDAL.GetBoxNoByFormcode(FormCode) != null)
			{
				return Result.Failed("该单号已装箱，请操作按箱出库！") as OutboundBLLV2.InnerResultModel;
			}
			#region 流程自定义

			var checkParameters = new OutboundSimpleCheckerArg();
			checkParameters.WaybillNo = Convert.ToInt64(FormCode);
			checkParameters.FromExpressCompanyId = Convert.ToInt32(argModel.OpUser.ExpressId);
			checkParameters.ToExpressCompanyId = Convert.ToInt32(argModel.ToStation.ExpressCompanyID);
			checkParameters.OutboundSimpleArg = argModel;
			checkParameters.OutboundBill = outboundBillModel;
			checkParameters.BoxNo = "";
			var flowCheckResult = new CheckerResult();
			if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.DistributionStation)
			{
				flowCheckResult = TMSFlowFunFacade.Check<OutboundSimpleCheckerArg>(checkParameters, FunCode.FunOutBoundStation);
			}
			if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.SortingCenter)
			{
				flowCheckResult = TMSFlowFunFacade.Check<OutboundSimpleCheckerArg>(checkParameters, FunCode.FunOutBoundDeliverCenter);
			}
			if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.Distributor)
			{
				flowCheckResult = TMSFlowFunFacade.Check<OutboundSimpleCheckerArg>(checkParameters, FunCode.FunOutBoundCompany);
			}
			if (!flowCheckResult.Result)
			{
				return Result.Failed(flowCheckResult.Message) as OutboundBLLV2.InnerResultModel;
			}
			#endregion

			return Result.Succeed() as OutboundBLLV2.InnerResultModel;
		}

		protected virtual OutboundBLLV2.InnerResultModel OutboundBox_ValidateV2(OutboundByBoxArgModel argModel,
		                                                                        String InputBoxNo)
		{
			var Result = new OutboundBLLV2.InnerResultModel();
			var outboundBoxModel = outboundV2DAL.GetOutboundBoxModel(InputBoxNo);
			if (outboundBoxModel == null)
			{
				return Result.Failed("此箱号不存在！") as InnerResultModel;
			}
			Result.BoxModel = outboundBoxModel;

			if (outboundBoxModel.DepartureID != argModel.OpUser.ExpressId.Value)
			{
				return Result.Failed("非本分拣中心的箱号，不能出库!") as InnerResultModel;
			}
			if (outboundBoxModel.IsOutbounded == 1)
			{
				return Result.Failed("此箱号已操作出库!") as InnerResultModel;
			}
			#region 流程自定义
			var checkParameters = new OutboundSimpleCheckerArg();
			//checkParameters.WaybillNo = long.;
			checkParameters.FromExpressCompanyId = Convert.ToInt32(argModel.OpUser.ExpressId);
			checkParameters.ToExpressCompanyId = Convert.ToInt32(argModel.ToStation.ExpressCompanyID);
			checkParameters.OutboundSimpleArg = argModel;
			checkParameters.OutboundBox = outboundBoxModel;
			checkParameters.BoxNo = InputBoxNo;
			var flowCheckResult = new CheckerResult();
			if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.DistributionStation)
			{
				flowCheckResult = TMSFlowFunFacade.Check<OutboundSimpleCheckerArg>(checkParameters, FunCode.FunOutBoundStation);
			}
			if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.SortingCenter)
			{
				flowCheckResult = TMSFlowFunFacade.Check<OutboundSimpleCheckerArg>(checkParameters, FunCode.FunOutBoundDeliverCenter);
			}
			if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.Distributor)
			{
				flowCheckResult = TMSFlowFunFacade.Check<OutboundSimpleCheckerArg>(checkParameters, FunCode.FunOutBoundCompany);
			}
			if (!flowCheckResult.Result)
			{
				return Result.Failed(flowCheckResult.Message) as OutboundBLLV2.InnerResultModel;
			}
			#endregion
			//if (argModel.PreCondition != null && (outboundBoxModel.ArrivalID != argModel.ToStation.ExpressCompanyID))
			//{
			//    return Result.Failed("出库目的地与入库装箱时选择的不一致！") as InnerResultModel;
			//}
			return Result.Succeed() as InnerResultModel;
		}

		/// <summary>
		/// 逐单出库
		/// </summary>
		/// <param name="argument">逐单出库对象</param>
		/// <returns></returns>
		public ViewOutboundSimpleModel SimpleOutbound(OutboundSimpleArgModel argument,bool isBatchOutbound)
		{
			if (argument == null) throw new ArgumentNullException("OutboundSimpleArgModel is null");
			if (argument.OpUser == null) throw new ArgumentNullException("OutboundSimpleArgModel.OpUser  is null");
			if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSimpleArgModel.PreCondition  is null");
			if (argument.ToStation == null) throw new ArgumentNullException("OutboundSimpleArgModel.ToStation  is null");
			ViewOutboundSimpleModel Result = new ViewOutboundSimpleModel();
			if (argument.OpUser.CompanyFlag != Enums.CompanyFlag.SortingCenter)
			{
				return Result.Failed("请用分拣中心帐号操作转站出库!") as ViewOutboundSimpleModel;
			}
			#endregion
			var checkResult = Outbound_ValidateV2(argument, argument.FormType, argument.FormCode);
			if (!checkResult.IsSuccess)
			{
				Result.Failed(checkResult.Message);
				//对应的单号对象不存在
				if (checkResult.BillModel == null)
				{
					Result.FormCode = argument.FormCode;
					Result.CustomerOrder = "";
				}
				else
				{
					Result.FormCode = checkResult.BillModel.FormCode;
					Result.CustomerOrder = checkResult.BillModel.CustomerOrder;
				}
				Result.OutboundDes = argument.ToStation.CompanyName;
				Result.OutboundStatus = "出库失败";
				return Result;
			}
			argument.BatchNo = "0";
			var writeResult = Outbound_Write(argument, checkResult.BillModel, argument.BatchNo,argument.CurrentDisCount,isBatchOutbound);
			if (!writeResult.IsSuccess)
			{
				Result.Failed(writeResult.Message);
				Result.FormCode = checkResult.BillModel.FormCode;
				Result.CustomerOrder = checkResult.BillModel.CustomerOrder;
				Result.OutboundDes = argument.ToStation.CompanyName;
				Result.OutboundStatus = "出库失败";
				return Result;
			}

			Result.Succeed();
			Result.FormCode = checkResult.BillModel.FormCode;
			Result.CustomerOrder = checkResult.BillModel.CustomerOrder;
			Result.OutboundDes = argument.ToStation.CompanyName;
			Result.OutboundStatus = "已出库";

			return Result;
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
			result.FormCodeList = outboundV2DAL.GetNeededOutboundFormCodeList(argument);
			result.InboundingCount = outboundV2DAL.GetInboundingCount(argument);
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
		private List<OutboundEntityModel> CreateOutboundEntityModelList(List<OutboundBillModel> listBillModel, String BatchNo, OutboundByBoxArgModel argument)
		{
			List<OutboundEntityModel> listOutboundModel = new List<OutboundEntityModel>(listBillModel.Count);
			listBillModel.ForEach(p =>
			{
				var outboundmodel = new OutboundEntityModel()
				{
					//ArrivalID = p.ArrivalID,
					ArrivalID = argument.ToStation.ExpressCompanyID,
					BatchNo = BatchNo,
					CreateBy = UserContext.CurrentUser.ID,
					DepartureID = p.DepartureID,
					FormCode = p.FormCode,
					IsDeleted = false,
					OutboundType = p.InboundType,
					SyncFlag = Enums.SyncStatus.NotYet,
					UpdateBy = UserContext.CurrentUser.ID,
					DeliverStationID = p.DeliverStationID
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
		private void InitOutboundBillModel(List<OutboundEntityModel> listOutboundEntityModel, List<OutboundBillModel> listBillModel, IOutboundArgModel argModel)
		{
			listBillModel.ForEach(p =>
			{
				var obModel = listOutboundEntityModel.FirstOrDefault(tmp => tmp.FormCode == p.FormCode);
				if (obModel == null)
				{
					throw new Exception("出库实体对象列表信息有误");
				}
				p.Status = Enums.BillStatus.Outbounded;
				//出库到配送商
				if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.Distributor)
				{
					p.CurrentDistributionCode = argModel.ToStation.DistributionCode;
					p.ToDistributionCode = argModel.ToStation.DistributionCode;
				}
				else
				{
					p.CurrentDistributionCode = UserContext.CurrentUser.DistributionCode;
					p.ToDistributionCode = null;
				}
				//出库到配送站
				if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.DistributionStation)
				{
					p.DeliverStationID = argModel.ToStation.ExpressCompanyID;
					//p.ToDeliverstationid = argModel.ToStation.ExpressCompanyID;
				}
				else
				{
					p.DeliverStationID = obModel.DeliverStationID;
					//p.ToDeliverstationid = 0;
				}
				//出库到分拣中心
				//if (argModel.ToStation.CompanyFlag == Enums.CompanyFlag.SortingCenter)
				//{
				//    p.ToSortingCenterId = argModel.ToStation.ExpressCompanyID;
				//}
				//else
				//{
				//    p.ToSortingCenterId = 0;
				//}
				p.OutboundKey = obModel.OBID;
				p.UpdateBy = UserContext.CurrentUser.ID;
				p.UpdateDept = UserContext.CurrentUser.DeptID;
			});
		}

		#region  查询出库
		/// <summary>
		/// 查询出库 
		/// </summary>
		/// <param name="argument">查询出库参数对象</param>
		/// <returns></returns>
		//public ViewOutboundBatchModel SearchOutbound(OutboundSearchArgModel argument)
		//{
		//    if (argument == null) throw new ArgumentNullException("OutboundSearchArgModel is null");
		//    if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
		//    if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
		//    if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
		//    if (argument.PerBatchCount <= 0) throw new ArgumentException("PerBatchCount must > 0", "argument.PerBatchCount", null);
		//    if (argument.FormType != Enums.SortCenterFormType.Waybill) throw new ArgumentException("FormType must equal Enums.SortCenterFormType.Waybill", "argument.FormType", null);
		//    ViewOutboundBatchModel result = new ViewOutboundBatchModel() { ListErrorBill = new List<SortCenterBatchErrorModel>() };
		//    List<SortCenterBatchErrorModel> validatedErrorList = null;
		//    var ValidatedFormCodeList = ValidateFormCode(argument.FormType, argument.ArrFormCode, out validatedErrorList);
		//    if (validatedErrorList != null)
		//    {
		//        result.ListErrorBill.AddRange(validatedErrorList);
		//    }
		//    if (ValidatedFormCodeList == null || ValidatedFormCodeList.Count <= 0)
		//    {
		//        string firstError = validatedErrorList != null ? validatedErrorList.First().ErrorMsg : "";
		//        return result.Failed(string.Format("选择列表单据验证未通过{0}", firstError)) as ViewOutboundBatchModel;
		//    }
		//    OutboundAfterConditionModel afterCondition = DistributionBLL.GetOutboundAfterConditionModel(
		//        argument.OpUser.DistributionCode,
		//        argument.ToStation.DistributionCode,
		//        argument.ToStation.SortingType);
		//    if (afterCondition == null)
		//    {
		//        return result.Failed("木有取得出库后置配置条件") as ViewOutboundBatchModel;
		//    }
		//    ///城际目的地ID
		//    int tmsArrivalID = -1;
		//    bool IsNeededDoTMS = base.IsNeedTMSTransfer(argument.OpUser.ExpressId.Value, argument.ToStation.ExpressCompanyID, out tmsArrivalID);
		//    String BatchNo = batchNoGenerator.Execute(new SerialNumberModel() { FillerCharacter = "0", NumberLength = 6 });
		//    if (String.IsNullOrWhiteSpace(BatchNo))
		//    {
		//        return result.Failed("出库批次号产生失败") as ViewOutboundBatchModel;
		//    }
		//    var batchModel = new OutboundBatchEntityModel()
		//    {
		//        BatchNo = BatchNo,
		//        DepartureID = argument.OpUser.ExpressId.Value,
		//        ArrivalID = argument.ToStation.ExpressCompanyID,
		//        OutboundCount = 0,
		//        SyncFlag = Enums.SyncStatus.NotYet,
		//        CreateBy = UserContext.CurrentUser.ID,
		//        UpdateBy = UserContext.CurrentUser.ID
		//    };
		//    batchModel.OBBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = batchModel.SequenceName, TableCode = batchModel.TableCode });
		//    if (outboundbatchV2DAL.Add(batchModel) <= 0)
		//    {
		//        return result.Failed("产生出库批次数据失败，请重试.") as ViewOutboundBatchModel;
		//    }
		//    int nDivValue = ValidatedFormCodeList.Count / argument.PerBatchCount;                   //倍数
		//    int nRemaiderValue = ValidatedFormCodeList.Count % argument.PerBatchCount;        //余数
		//    int nBatchCount = nDivValue + (nRemaiderValue == 0 ? 0 : 1);                                    //总批量处理的计数
		//    int nSuccessfulCount = 0;                                                                                           //出库成功的数量
		//    List<String> listFormCode = null;
		//    List<SortCenterBatchErrorModel> listErrorFormCode = new List<SortCenterBatchErrorModel>();
		//    Dictionary<OutboundBillModel, String> dicOutboundSMS = new Dictionary<OutboundBillModel, string>();
		//    var SMSConfig = GetSMSConfig();
		//    for (int i = 0; i < nBatchCount; i++)
		//    {
		//        if (i != nBatchCount - 1)
		//        {
		//            listFormCode = ValidatedFormCodeList.GetRange(i * argument.PerBatchCount, argument.PerBatchCount);
		//        }
		//        else
		//        {
		//            listFormCode = ValidatedFormCodeList.GetRange(i * argument.PerBatchCount, nRemaiderValue == 0 ? argument.PerBatchCount : nRemaiderValue);
		//        }
		//        var outboundBillInfo = BillBLL.GetOutboundBillModel_SearchOutbound(argument, listFormCode);
		//        if (outboundBillInfo == null || outboundBillInfo.Count <= 0)
		//        {
		//            listFormCode.ForEach(p =>
		//            {
		//                listErrorFormCode.Add(new SortCenterBatchErrorModel() { WaybillNo = p, CustomerOrder = "", ErrorMsg = "未找到出库运单信息" });
		//            });
		//            continue;
		//        }
		//        if (outboundBillInfo.Count != listFormCode.Count)
		//        {
		//            listFormCode.ForEach(p =>
		//            {
		//                var tmpContainObj = outboundBillInfo.FirstOrDefault(mp => mp.FormCode.Equals(p));
		//                if (tmpContainObj == null)
		//                {
		//                    listErrorFormCode.Add(new SortCenterBatchErrorModel() { WaybillNo = p, CustomerOrder = "", ErrorMsg = "未找到出库运单信息" });
		//                }
		//            });
		//        }
		//        try
		//        {
		//            using (IACID scope = ACIDScopeFactory.GetTmsACID())
		//            {
		//                var listOutboundModel = CreateOutboundEntityModelList(outboundBillInfo, BatchNo);
		//                outboundV2DAL.BatchAdd(listOutboundModel);
		//                InitOutboundBillModel(listOutboundModel, outboundBillInfo, afterCondition);
		//                BillBLL.BatchUpdateBillModelByOutbound(outboundBillInfo);
		//                List<BillChangeLogDynamicModel> listChangeLog = new List<BillChangeLogDynamicModel>();
		//                outboundBillInfo.ForEach(p =>
		//                {
		//                    var tmpChangeLog = new BillChangeLogDynamicModel()
		//                    {
		//                        FormCode = p.FormCode,
		//                        CurrentDistributionCode = p.CurrentDistributionCode,
		//                        DeliverStationID = p.DeliverStationID,
		//                        CurrentSatus = p.Status,
		//                        OperateType = Enums.TmsOperateType.Outbound,
		//                        PreStatus = argument.PreCondition.PreStatus,
		//                        CreateBy = UserContext.CurrentUser.ID,
		//                        CreateDept = UserContext.CurrentUser.DeptID
		//                    };
		//                    tmpChangeLog.ExtendedObj.ArrivalMnemonicName = argument.ToStation.MnemonicName;
		//                    tmpChangeLog.ExtendedObj.SortCenterMnemonicName = argument.OpUser.MnemonicName;
		//                    listChangeLog.Add(tmpChangeLog);
		//                });
		//                WriteBillChangeLog_Batch(listChangeLog);
		//                scope.Complete();
		//            }
		//            nSuccessfulCount += outboundBillInfo.Count;
		//            foreach (var item in outboundBillInfo)
		//            {
		//                var smsresult = SMSValidate(item, SMSConfig);
		//                if (smsresult != null && smsresult.IsSuccess)
		//                {
		//                    dicOutboundSMS.Add(item, smsresult.Content);
		//                }
		//            }
		//        }
		//        catch (Exception ex)
		//        {
		//            listFormCode.ForEach(p =>
		//            {
		//                listErrorFormCode.Add(new SortCenterBatchErrorModel() { WaybillNo = p, CustomerOrder = "", ErrorMsg = ex.Message });
		//            });
		//        }
		//    }
		//    if (IsNeededDoTMS && nSuccessfulCount > 0)
		//    {
		//        if (sc_syn_tms_outboxDAL.Add(new SC_SYN_TMS_OutboxEntityModel()
		//        {
		//            BoxNo = BatchNo,
		//            DepartureID = argument.OpUser.ExpressId.Value,
		//            ArrivalID = argument.ToStation.ExpressCompanyID,
		//            SC2TMSFlag = Enums.SyncStatus.NotYet,
		//            NoType = Enums.SyncNoType.Batch
		//        }) <= 0)
		//        {
		//            throw new Exception("产生分拣到TMS成绩运输临时中间数据失败");
		//        }
		//    }
		//    //if (dicOutboundSMS != null && dicOutboundSMS.Count > 0)
		//    //{
		//    //    OutboundSMSSend(dicOutboundSMS);
		//    //}
		//    result.SucceedCount = nSuccessfulCount;
		//    result.ListErrorBill.AddRange(listErrorFormCode);
		//    result.FailedCount = result.ListErrorBill.Count;
		//    return result.Succeed(String.Format("出库成功:{0},失败:{1}", result.SucceedCount, result.FailedCount)) as ViewOutboundBatchModel;
		//}
		
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
			return outboundV2DAL.GetReturnTo(formCode, arrivalID);
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
			return outboundV2DAL.GetNeededOutBoundBoxList(argument);
		}

		#endregion

		#region IOutboundBLL 成员


		public ViewOutBoundBoxDetailModel GetBoxBillsByBoxNo(string boxNo)
		{
			ViewOutBoundBoxDetailModel detail = outboundV2DAL.GetNeededOutBoundBillListByBoxNo(boxNo);
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
			return outboundV2DAL.GetBoxInfoByBoxNo(boxNo);
		}

		#endregion

		#region IOutboundBLL 成员

		/// <summary>
		/// 按箱出库（批量）
		/// </summary>
		/// <param name="argument"></param>
		/// <returns></returns>
		public List<ViewOutboundBatchModel> OutboundByBox(OutboundByBoxArgModel argument)
		{
			if (argument == null) throw new ArgumentNullException("argument is null");
			if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
			if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
			if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
			if (argument.BoxNos == null || argument.BoxNos.Count == 0) throw new ArgumentNullException("OutboundSearchArgModel.BoxNos is null or empty");
			ViewOutboundBatchModel result = new ViewOutboundBatchModel();
			List<ViewOutboundBatchModel> resultList = new List<ViewOutboundBatchModel>();
			int sCount = 0;
			foreach (var item in argument.BoxNos)
			{
				//返回成功状态和Message
				result = BoxOutbound(new OutboundByBoxArgModel()
				{
					OpUser = argument.OpUser,
					PreCondition = argument.PreCondition,
					ToStation = argument.ToStation,
					CurrentBoxNo = item
				});

				if (!result.IsSuccess)
				{
					result.BoxNo = item;
					result.BoxOutboundStatus = "出库失败";
					result.OutboundDes = argument.ToStation.CompanyName;
					result.SucceedCount = 0;
				}
				else
				{
					//sCount++;
					result.BoxNo = item;
					result.BoxOutboundStatus = "已出库";
					result.OutboundDes = argument.ToStation.CompanyName;
				}
				resultList.Add(result);
			}
			//if (sCount < argument.BoxNos.Count)
			//    result.Failed("成功出库" + sCount.ToString() + "箱;失败" + (argument.BoxNos.Count - sCount).ToString() + "箱");
			//else
			//    result.Succeed("出库成功");
			return resultList;
		}

		/// <summary>
		/// 按箱出库 
		/// </summary>
		/// <param name="argument">按箱出库参数对象</param>
		/// <returns></returns>
		public ViewOutboundBatchModel BoxOutbound(OutboundByBoxArgModel argument)
		{
			if (argument == null) throw new ArgumentNullException("OutboundSearchArgModel is null");
			if (argument.OpUser == null) throw new ArgumentNullException("OutboundSearchArgModel.OpUser is null");
			//if (argument.PreCondition == null) throw new ArgumentNullException("OutboundSearchArgModel.PreCondition is null");
			if (argument.ToStation == null) throw new ArgumentNullException("OutboundSearchArgModel.ToStation is null");
			if (string.IsNullOrWhiteSpace(argument.CurrentBoxNo)) throw new ArgumentNullException("OutboundSearchArgModel.BoxNo is null or empty");
			var result = new ViewOutboundBatchModel() { ListErrorBill = new List<SortCenterBatchErrorModel>() };
			//验证箱号的合法性
			var checkResult = OutboundBox_ValidateV2(argument, argument.CurrentBoxNo);
			if (!checkResult.IsSuccess)
			{
				return result.Failed(checkResult.Message) as ViewOutboundBatchModel;
			}
			argument.CurrentFormCodes = outboundV2DAL.GetNeededOutBoundBillListByBoxNo(argument.CurrentBoxNo).FormCodes;
			List<SortCenterBatchErrorModel> validatedErrorList = null;
			var ValidatedFormCodeList = ValidateFormCode(Enums.SortCenterFormType.Waybill, argument.CurrentFormCodes.ToArray(), out validatedErrorList);
			if (validatedErrorList != null && validatedErrorList.Count != 0)
			{
				result.ListErrorBill.AddRange(validatedErrorList);
			}
			if (ValidatedFormCodeList == null || ValidatedFormCodeList.Count <= 0)
			{
				string firstError = validatedErrorList != null ? validatedErrorList.First().ErrorMsg : "";
				return result.Failed(string.Format("选择列表单据验证未通过{0}", firstError)) as ViewOutboundBatchModel;
			}
			//城际目的地ID
			int tmsArrivalID = -1;
			bool IsNeededDoTMS = base.IsNeedTMSTransfer(argument.OpUser.ExpressId.Value, argument.ToStation.ExpressCompanyID, out tmsArrivalID);

			int nSuccessfulCount = 0;
			//List<SortCenterBatchErrorModel> listErrorFormCode = new List<SortCenterBatchErrorModel>();
			Dictionary<OutboundBillModel, String> dicOutboundSMS = new Dictionary<OutboundBillModel, string>();
			var SMSConfig = GetSMSConfig();
			var outboundBillInfo = BillBLL.GetOutboundBillModel_PackingOutboundV2(argument, ValidatedFormCodeList);
			if (outboundBillInfo.Count != ValidatedFormCodeList.Count)
			{
				return result.Failed("箱子中存在" + (ValidatedFormCodeList.Count - outboundBillInfo.Count).ToString() + "单不符合出库要求订单.") as ViewOutboundBatchModel;
			}
			//String BatchNo = argument.CurrentBoxNo;
			string BatchNo = "0";
			try
			{
				using (IACID scope = ACIDScopeFactory.GetTmsACID())
				{
					//新增出库批次数据(删除)
					#region
					//var batchModel = new OutboundBatchEntityModel()
					//{
					//    BatchNo = BatchNo,
					//    DepartureID = argument.OpUser.ExpressId.Value,
					//    ArrivalID = argument.ToStation.ExpressCompanyID,
					//    OutboundCount = 0,
					//    SyncFlag = Enums.SyncStatus.NotYet,
					//    CreateBy = UserContext.CurrentUser.ID,
					//    UpdateBy = UserContext.CurrentUser.ID
					//};
					//batchModel.OBBID = KeyCodeGenerator.Execute(new KeyCodeContextModel() { SequenceName = batchModel.SequenceName, TableCode = batchModel.TableCode });
					//if (outboundbatchV2DAL.Add(batchModel) <= 0)
					//{
					//    return result.Failed("产生出库批次数据失败，请重试.") as ViewOutboundBatchModel;
					//}
					#endregion
					var listOutboundModel = CreateOutboundEntityModelList(outboundBillInfo, BatchNo,argument);
					outboundV2DAL.BatchAdd(listOutboundModel);
					InitOutboundBillModel(listOutboundModel, outboundBillInfo,argument);
					BillBLL.BatchUpdateBillModelByOutboundV2(outboundBillInfo);
					List<BillChangeLogDynamicModel> listChangeLog = new List<BillChangeLogDynamicModel>();
					outboundBillInfo.ForEach(p =>
					{
						var tmpChangeLog = new BillChangeLogDynamicModel()
						{
							FormCode = p.FormCode,
							CurrentDistributionCode = UserContext.CurrentUser.DistributionCode,
							ToDistributionCode = p.ToDistributionCode,
							ToExpressCompanyID = argument.ToStation.ExpressCompanyID,
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
					//流程自定义:
					for (int i = 0; i < argument.CurrentFormCodes.Count; i++)
					{
					    WaybillTurn turn = new WaybillTurn();
					    turn.WaybillNo = Convert.ToInt64(argument.CurrentFormCodes[i]);
					    turn.FromExpressCompanyId = Convert.ToInt32(argument.OpUser.ExpressId);
					    turn.ToExpressCompanyId = argument.ToStation.ExpressCompanyID;
					    turn.FromDistributionCode = UserContext.CurrentUser.DistributionCode;
					    turn.ToDistributionCode = (argument.ToStation.CompanyFlag == Enums.CompanyFlag.Distributor)
					                                  ? argument.ToStation.DistributionCode
					                                  : UserContext.CurrentUser.DistributionCode;
					    turn.TurnType = (argument.ToStation.CompanyFlag == Enums.CompanyFlag.Distributor)
					                        ? Enums.TurnType.ToDistribution
					                        : Enums.TurnType.ToDep;

						TMSFlowFunFacade.WaybillTurn(turn);
					}
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
			result.SucceedCount = ValidatedFormCodeList.Count;
			return result.Succeed(String.Format("出库成功")) as ViewOutboundBatchModel;
		}

		#endregion

		#region IOutboundBLL 成员


		public int SetBoxOutBoundStatus(string boxNo, bool isOutBounded)
		{
			return outboundV2DAL.SetBoxOutBoundStatus(boxNo, isOutBounded);
		}

		#endregion

		#region  V2新增

		public int GetCurOutBoundCount(int departureId)
		{
			return outboundV2DAL.GetCurOutBoundCount(departureId);
		}

		public int GetCurDisOutBoundCount(int departureId, int arrivalid)
		{
			return outboundV2DAL.GetCurDisOutBoundCount(departureId, arrivalid);
		}

		/// <summary>
		/// 统计出库到当前目的地的数量和批次号（未截单）
		/// </summary>
		/// <returns></returns>
		public ViewGetCountAndBatchNo GetCountAndBatchNo(int departureId, int arrivalid)
		{
			return outboundV2DAL.GetCountAndBatchNo(departureId, arrivalid);
		}

		#endregion


		public ViewOutboundBatchModel SearchOutbound(OutboundSearchArgModel argument)
		{
			throw new NotImplementedException();
		}
	}
}
