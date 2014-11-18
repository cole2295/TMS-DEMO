using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.OrdersForWdsAssignService;
using Vancl.TMS.BLL.PmsService;
using Vancl.TMS.IBLL.CustomizeFlow;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;

namespace Vancl.TMS.BLL.CustomizeFlow.Checker
{
	/// <summary>
	/// 运单出库
	/// 1.	所选配送站与分单配送站或装箱目的地一致时才可出库
	/// 3.	所选配送商与分单配送商或装箱目的地一致时才可出库
	/// </summary>
	public class LimitSubjectByAssignOrByBox : IChecker<OutboundSimpleCheckerArg>
	{
		#region 构建对象

		private PermissionOpenServiceClient _pmsClient = null;

		PermissionOpenServiceClient GetPmsClient()
		{
			if (_pmsClient == null)
			{
				_pmsClient = new PermissionOpenServiceClient();
			}
			return _pmsClient;
		}

		private OrdersForWdsAssignClient _orderAssignClient = new OrdersForWdsAssignClient();

		private OrdersForWdsAssignClient GetWdsAssignClient()
		{
			if (_orderAssignClient == null)
			{
				_orderAssignClient = new OrdersForWdsAssignClient();
			}
			return _orderAssignClient;
		}
		#endregion
		public CheckerResult Check(OutboundSimpleCheckerArg model)
		{
			CheckerResult result = new CheckerResult();
			result.Result = true;
			
			//操作配送商
			//string distributionCode = GetPmsClient().GetDistributionCodeByCompanyId(model.ExpressCompanyId);
			if (model.ToExpressCompanyId.HasValue)
			{
				//如果是按箱出库
				if (!string.IsNullOrEmpty(model.BoxNo))
				{
					if (model.OutboundBox.ArrivalID != model.ToExpressCompanyId)
					{
						result.Result = false;
						result.Message = "出库目的地与入库装箱时选择的不一致！";
						return result;
					}

				}
				else
				{
					//出库配送商
					string toDistributionCode = GetPmsClient().GetDistributionCodeByCompanyId(model.ToExpressCompanyId.Value);
					//分单配送商
					string assginDistribution = GetWdsAssignClient().GetAssignDistribution(model.WaybillNo.ToString());
					//分配配送站
					int assignStation = GetWdsAssignClient().GetAssignStation(model.WaybillNo.ToString());
					var companyFlag = model.OutboundSimpleArg.ToStation.CompanyFlag;
					if (companyFlag == Enums.CompanyFlag.DistributionStation)
					{
						if (assignStation != model.ToExpressCompanyId.Value)
						{
							result.Result = false;
                            result.Message = "该运单不属于所选配送站！";
							return result;
						}

					}
					else if (companyFlag == Enums.CompanyFlag.Distributor)
					{
						if (toDistributionCode != assginDistribution)
						{
							result.Result = false;
                            result.Message = "该运单不属于所选配送商！";
							return result;
						}
					}
					//删除

					#region
					//if (toDistributionCode == assginDistribution)
					//{
					//    //配送商内出库
					//    if (distributionCode == assginDistribution && companyFlag == Enums.CompanyFlag.DistributionStation)
					//    {
					//        if (assignStation != model.ToExpressCompanyId.Value)
					//        {
					//            result.Result = false;
					//            result.Message = "该订单未分配给我，没有权限操作！";
					//            return result;
					//        }

					//    }
					//    else
					//    {

					//    }
					//}
					//else
					//{
					//    result.Result = false;
					//    result.Message = "出库目的地与分单不一致！";
					//    return result;
					//}
					#endregion
					
					#region
					//if (distributionCode == toDistributionCode)
					//{
					//    //如果是按箱出库
					//    if (!string.IsNullOrEmpty(model.BoxNo))
					//    {
					//        if (model.OutboundBox.ArrivalID != model.ToExpressCompanyId)
					//        {
					//            result.Result = false;
					//            result.Message = "出库目的地与入库装箱时选择的不一致！";
					//            return result;
					//        }

					//    }
					//    else
					//    {
					//        //OrdersForWdsAssignClient GetWdsAssignClient() = new OrdersForWdsAssignClient();
					//        //string assginDistribution = GetWdsAssignClient().GetAssignDistribution(model.WaybillNo.ToString());
					//        if (assginDistribution != toDistributionCode)
					//        {
					//            result.Result = false;
					//            result.Message = "该订单未分配给我，没有权限操作！";
					//            return result;
					//        }
					//    }
					//}
					//else
					//{
					//    //如果是按箱出库
					//    if (!string.IsNullOrEmpty(model.BoxNo))
					//    {
					//        if (model.OutboundBox.ArrivalID != model.ToExpressCompanyId)
					//        {
					//            result.Result = false;
					//            result.Message = "出库目的地与入库装箱时选择的不一致！";
					//            return result;
					//        }
					//    }
					//    else
					//    {
					//        //OrdersForWdsAssignClient GetWdsAssignClient() = new OrdersForWdsAssignClient();
					//        int assignStation = GetWdsAssignClient().GetAssignStation(model.WaybillNo.ToString());
					//        if (assignStation != model.ToExpressCompanyId.Value)
					//        {
					//            result.Result = false;
					//            result.Message = "该订单未分配给我，没有权限操作！";
					//            return result;
					//        }
					//    }
					//}
				#endregion
				}
			}
			
			return result;
		}

		public bool IsMatchChecker(string checkerType)
		{
			if (checkerType.Equals("LimitSubjectByAssignOrByBox"))
				return true;
			return false;
		}
	}
}
