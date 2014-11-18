using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.OrdersForWdsAssignService;
using Vancl.TMS.BLL.PmsService;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.CustomizeFlow;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;

namespace Vancl.TMS.BLL.CustomizeFlow.Checker
{
	/// <summary>
	/// 运单装箱
	/// 1.校验运单所属配送商/配送站属于所选分拣中心时才可装箱
	/// </summary>
	public class LimitSubjectBySortingDefined : IChecker<CheckerParameter>
	{
		#region 构建对象

		IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");

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

		public CheckerResult Check(CheckerParameter model)
		{
			CheckerResult result = new CheckerResult();
			result.Result = true;
			
			//操作配送商
			//string distributionCode = GetPmsClient().GetDistributionCodeByCompanyId(model.ExpressCompanyId);
			if (model.ToExpressCompanyId.HasValue)
			{
				//出库配送商
				//string toDistributionCode = GetPmsClient().GetDistributionCodeByCompanyId(model.ToExpressCompanyId.Value);
				//分单配送商
				string assginDistribution = GetWdsAssignClient().GetAssignDistribution(model.WaybillNo.ToString());
				//分单配送站
				int assignStation = GetWdsAssignClient().GetAssignStation(model.WaybillNo.ToString());
				var distributionModel = new ExpressCompanyModel();
				if (assignStation > 0)
				{
					var scs1 = _expressCompanyBLL.GetSortingCenterByStation(Convert.ToInt32(assignStation));
					if (!scs1.Contains(Convert.ToInt32(model.ToExpressCompanyId)))
					{
						string stationName = _expressCompanyBLL.GetModel(assignStation).CompanyAllName;
						result.Result = false;
						result.Message = string.Format("该运单已分配给{0}，无法发往所选分拣中心！", stationName);
						return result;
					}
				}
				else if (!string.IsNullOrEmpty(assginDistribution) && assginDistribution != "-1")
				{
					//获取到得是配送商的简码对应的ExpressCompanyId
					distributionModel = _expressCompanyBLL.GetDistributor(assginDistribution);
					int distributionID = Convert.ToInt32(distributionModel.ID);
					var scs = _expressCompanyBLL.GetSortingCenterByStation(distributionID);
					if (!scs.Contains(Convert.ToInt32(model.ToExpressCompanyId)))
					{
						string distributionName = _expressCompanyBLL.GetModel(distributionID).CompanyAllName;
						result.Result = false;
						result.Message = string.Format("该运单已分配给{0}，无法发往所选分拣中心！", distributionName);
						return result;
					}
				}
				else
				{
					result.Result = false;
					result.Message = "该运单未分配！";
					return result;
				}
				#region   删除
				//if ((assginDistribution != null && assginDistribution != "-1"))
				//{
				//    //获取到得是配送商的简码对应的ExpressCompanyId
				//    distributionModel = _expressCompanyBLL.GetDistributor(assginDistribution);
				//}
				//var scs = _expressCompanyBLL.GetSortingCenterByStation(Convert.ToInt32(distributionModel.ID));
				//var scs1 = _expressCompanyBLL.GetSortingCenterByStation(Convert.ToInt32(assignStation));
				//if ((!scs.Contains(Convert.ToInt32(model.ToExpressCompanyId))) && (!scs1.Contains(Convert.ToInt32(model.ToExpressCompanyId))))
				//{
				//    result.Result = false;
				//    result.Message = "该运单不属于所选分拣中心！";
				//    return result;
				//}
				#endregion
			}
			return result;
		}

		public bool IsMatchChecker(string checkerType)
		{
			if (checkerType.Equals("LimitSubjectBySortingDefined"))
				return true;
			return false;
		}
	}
}
