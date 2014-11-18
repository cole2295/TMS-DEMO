using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.BLL.OrdersForWdsAssignService;
using Vancl.TMS.BLL.PmsService;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.CustomizeFlow;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;

namespace Vancl.TMS.BLL.CustomizeFlow.Checker
{
	/// <summary>
	/// 运单装箱
	/// 1.校验运单所属配送站与所选配送站一致时才可装箱
	/// 2.校验运单所属配送商与所选配送商一致时才可装箱
	/// </summary>
    public class LimitSubjectByAssign : IChecker<CheckerParameter>  
    {
		#region 构建对象

		private IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");

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
				string toDistributionCode = GetPmsClient().GetDistributionCodeByCompanyId(model.ToExpressCompanyId.Value);
				//分单配送商
				string assginDistribution = GetWdsAssignClient().GetAssignDistribution(model.WaybillNo.ToString());
				//分配配送站
				int assignStation = GetWdsAssignClient().GetAssignStation(model.WaybillNo.ToString());
	            var companyFlag = _expressCompanyBLL.Get((int) model.ToExpressCompanyId).CompanyFlag;
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
	            #region 
				//if (toDistributionCode == assginDistribution)
				//{
				//    //配送商内出库
				//    if (distributionCode == assginDistribution &&
				//        _expressCompanyBLL.Get((int)model.ToExpressCompanyId).CompanyFlag == Enums.CompanyFlag.DistributionStation)
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
			}
            return result;
        }

        public bool IsMatchChecker(string checkerType)
        {
            if (checkerType.Equals("LimitSubjectByAssign"))
                return true;
            return false;
        }
    }
}
