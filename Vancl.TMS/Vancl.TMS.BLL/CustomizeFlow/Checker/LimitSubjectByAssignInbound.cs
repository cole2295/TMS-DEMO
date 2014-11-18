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
	//状态=已分配配送商才可入库
	public class LimitSubjectByAssignInbound : IChecker<InboundCheckParameter>
	{
	    private OrdersForWdsAssignClient _orderAssignClient = null;
		OrdersForWdsAssignClient GetWdsAssignClient()
        {
            if (_orderAssignClient == null)
            {
                _orderAssignClient = new OrdersForWdsAssignClient();
            }
            return _orderAssignClient;
        }
		public CheckerResult Check(InboundCheckParameter model)
		{
			CheckerResult result = new CheckerResult();
			result.Result = true;
			//第一次分拣入库
			if (model.IsFirstSorting)
			{
				//未分配配送商
				if (!GetWdsAssignClient().IsAssignDistribution(model.WaybillNo.ToString()))
				{
					result.Result = false;
					result.Message = "该订单未分配配送商，无法入库";
				}
			}
			return result;
		}

		public bool IsMatchChecker(string checkerType)
		{
			if (checkerType.Equals("LimitSubjectByAssignInbound"))
				return true;
			return false;
		}
	}
}
