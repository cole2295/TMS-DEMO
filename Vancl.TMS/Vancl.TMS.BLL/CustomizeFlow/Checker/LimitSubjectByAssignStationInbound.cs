using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.OrdersForWdsAssignService;
using Vancl.TMS.BLL.PmsService;
using Vancl.TMS.IBLL.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;

namespace Vancl.TMS.BLL.CustomizeFlow.Checker
{
	/// <summary>
	/// 已分配至当前配送商时，状态=已分配配送站才可入库 
	/// </summary>
	public class LimitSubjectByAssignStationInbound : IChecker<InboundCheckParameter>
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
		public CheckerResult Check(InboundCheckParameter model)
		{
			CheckerResult result = new CheckerResult();
			result.Result = true;
			//第一次分拣入库
			if (model.IsFirstSorting)
			{
				//分单配送商
				string assginDistribution = GetWdsAssignClient().GetAssignDistribution(model.WaybillNo.ToString());
				if (model.CurDistributionCode == assginDistribution)
				{
					if (!GetWdsAssignClient().IsAssignStation(model.WaybillNo.ToString()))
					{
						result.Result = false;
						result.Message = "该订单分配到当前配送商但是未分配配送站，无法入库";
					}
				}
			}
			return result;
		}

		public bool IsMatchChecker(string checkerType)
		{
			if (checkerType.Equals("LimitSubjectByAssignStationInbound"))
				return true;
			return false;
		}
	}
}
