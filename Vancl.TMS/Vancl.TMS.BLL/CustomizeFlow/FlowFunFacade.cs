using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vancl.TMS.BLL.CustomizeFlow;
using Vancl.TMS.BLL.PmsService;
using Vancl.TMS.BLL.WaybillTurnService;
using Vancl.TMS.Core.ObjectFactory;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.CustomizeFlow;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;

namespace Vancl.TMS.BLL.CustomizeFlow
{
    public class FlowFunFacade
    {
        #region Load

        private PermissionOpenServiceClient _pmsClient = null;
        private PermissionOpenServiceClient PmsClient()
        {
            if (_pmsClient == null)
            {
                _pmsClient = new PermissionOpenServiceClient();
            }
            return _pmsClient;
        }
        private CustomizeFlowClient _customizeFlowclient = null;
        private CustomizeFlowClient GetClient()
        {
            if (_customizeFlowclient == null)
            {
                _customizeFlowclient = new CustomizeFlowClient();
            }

            return _customizeFlowclient;
        }

        private  WaybillTurnServiceClient _waybillTurnclient = null;
        private  WaybillTurnServiceClient waybillTurnClient()
        {
            if (_waybillTurnclient == null)
            {
                _waybillTurnclient = new WaybillTurnServiceClient();
            }
            return _waybillTurnclient;
        }
        private IDictionary<string, string> _Checkers = null;
        private IDictionary<string, string> GetAllCheckers()
        {
            if (_Checkers == null)
            {
                _Checkers = new Dictionary<string, string>();
                _Checkers.Add("LimitSubject", "LimitSubjectChecker");
				_Checkers.Add("LimitSubjectByAssignInbound", "LimitSubjectByAssignInbound");
				_Checkers.Add("LimitSubjectByAssignStationInbound", "LimitSubjectByAssignStationInbound");
                _Checkers.Add("LimitSubjectByAssign", "LimitSubjectByAssign");
				_Checkers.Add("LimitSubjectByAssignOrByBox", "LimitSubjectByAssignOrByBox");
				_Checkers.Add("LimitSubjectBySortingDefined", "LimitSubjectBySortingDefined");
				_Checkers.Add("LimitSubjectBySortingDefinedOrByBox", "LimitSubjectBySortingDefinedOrByBox");
				
            }
            return _Checkers;
        }

        #endregion

        #region 页面加载控制显示
        /// <summary>
        /// 页面显示
        /// </summary>
        /// <param name="distributionCode"></param>
        /// <param name="funCode"></param>
        /// <returns></returns>
        public bool IsExitsCurrFun(string distributionCode, FunCode funCode)
        {
            return GetClient().IsExitsFun(distributionCode, funCode);
        }
        #endregion

        #region 操作之前的限制

        public CheckerResult Check<T>(T model, FunCode funCode) where T : CheckerParameter
        {
            CheckerResult result = new CheckerResult();
            result.Result = true;

            //pms->当前功能的限制
            var disCheckList = GetDistributionFunCheckers(model.FromDistributionCode, funCode);
            var allCheckList = GetAllCheckers();

            IChecker<T> check = null;

            foreach (var disCheck in disCheckList)
            {
                if (allCheckList.ContainsKey(disCheck.CheckerType))
                {
                    check = ServiceFactory.GetService<IChecker<T>>(allCheckList[disCheck.CheckerType]);

                    if (check.IsMatchChecker(disCheck.CheckerType))
                    {
                        result = check.Check(model);

                        if (!result.Result)
                        {
                            return result;
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region 操作之后记录

        public void WaybillTurn(Vancl.TMS.Model.CustomizeFlow.WaybillTurn waybillTurn)
        {
            //waybillTurn->dps.waybillTurn
            WaybillTurnModel model=new WaybillTurnModel();
            model.WaybillNo = waybillTurn.WaybillNo;
            model.FromExpressCompanyId = waybillTurn.FromExpressCompanyId;
            model.ToExpressCompanyId = waybillTurn.ToExpressCompanyId;
            model.FromDistributionCode = waybillTurn.FromDistributionCode;
            model.ToDistributionCode = waybillTurn.ToDistributionCode;
            model.TurnType = (EnumCommonWaybillTurnType) waybillTurn.TurnType;
            waybillTurnClient().WaybillTurn(model);
        }
        
        #endregion

        #region private

        private List<FlowNodeCheckerModel> GetDistributionFunCheckers(string distributionCode, FunCode funCode)
        {
            List<FlowNodeCheckerModel> checkersList = GetClient().GetDistributionFunChecker(distributionCode, funCode);

            return checkersList;
        }
        
        #endregion
    }
}

