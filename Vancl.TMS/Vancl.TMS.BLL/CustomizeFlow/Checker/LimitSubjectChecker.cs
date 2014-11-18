using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.PmsService;
using Vancl.TMS.BLL.WaybillTurnService;
using Vancl.TMS.IBLL.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;

namespace Vancl.TMS.BLL.CustomizeFlow.Checker
{
	public class LimitSubjectChecker : IChecker<InboundCheckParameter>
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

		private WaybillTurnServiceClient _waybillTurnClient = new WaybillTurnServiceClient();

	    private WaybillTurnServiceClient GetWaybillTurnClient()
	    {
		    if (_waybillTurnClient == null)
		    {
			    _waybillTurnClient = new WaybillTurnServiceClient();
		    }
		    return _waybillTurnClient;
	    }

	    #endregion
		public CheckerResult Check(InboundCheckParameter model)
        {
            CheckerResult result = new CheckerResult();
            result.Result = true;
            
            WaybillTurnModel lastTurn = GetWaybillTurnClient().GetWaybillTurn(model.WaybillNo);

            if (lastTurn != null && !lastTurn.ToDistributionCode.Equals(model.FromDistributionCode))
            {
                result.Result = false;
                result.Message = "非本配送商订单，无法操作！";
                return result;
            }


            if (lastTurn != null && !lastTurn.ToExpressCompanyId.Equals(model.FromExpressCompanyId))
            {
                result.Result = false;
                result.Message = "非本部门订单，无法操作！";
                return result;
            }

            return result;
        }

        public bool IsMatchChecker(string checkerType)
        {
            if (checkerType.Equals("LimitSubject"))
                return true;
            return false;
        }



    }
}

