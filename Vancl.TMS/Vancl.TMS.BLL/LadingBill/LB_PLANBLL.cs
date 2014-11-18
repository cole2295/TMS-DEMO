using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ACIDManager;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.IDAL.LadingBill;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.BLL.LadingBill
{
    public class LB_PLANBLL : BaseBLL, ILB_PLANBLL
    {
        ILB_PLANDAL _planDAL = ServiceFactory.GetService<ILB_PLANDAL>("LB_PLANDAL");


        /// <summary>
        /// 把计划生成状态 改为0
        /// </summary>
        /// <returns></returns>
        public int UpPlanIsCreate()
        {
            return _planDAL.UpPlanIsCreate();
        }

        public int Add(Model.LadingBill.LB_PLAN model)
        {
            return _planDAL.Add(model);
        }

        /// <summary>
        /// 更新计划表是否已生成
        /// </summary>
        /// <param name="id">计划ID</param>
        /// <param name="iscreateCode">0没有生成，1已生成</param>
        /// <returns></returns>
        public int UpCreate(Decimal id, int iscreateCode)
        {
            return _planDAL.UpCreate(id, iscreateCode);
        }

        public IList<LB_PLAN> GetList()
        {
            return _planDAL.GetList();
        }

        /// <summary>
        /// 查询提货计划明细
        /// </summary>
        /// <param name="planID">提货计划ID</param>
        /// <returns></returns>
        public LB_PLAN GetPlanDetails(decimal planID)
        {
            return _planDAL.GetPlanDetails(planID);
        }

        public int Update(Model.LadingBill.LB_PLAN model)
        {
            return _planDAL.Update(model);
        }

        public PagedList<LB_PLANDTO> GetPlanList(LB_PLANDTO searchModel)
        {
            return _planDAL.GetPlanList(searchModel);
        }

        /// <summary>
        /// 删除提货计划
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public ResultModel Delete(List<string> PlanID)
        {
            if (PlanID == null || PlanID.Count == 0)
            {
                throw new CodeNotValidException();
            }
            PlanID = PlanID.Distinct().ToList();
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = _planDAL.Delete(PlanID);
                foreach (string id in PlanID)
                {
                    //TODO:增加日志
                }
                scope.Complete();
            }
            if (i <= 0)
            {
                return ErrorResult("删除提货计划失败！");
            }
            else
            {
                return InfoResult("删除提货计划成功！");
            }
        }

        public Model.LadingBill.LB_PLAN GetModel()
        {
            throw new NotImplementedException();
        }

        public Model.LadingBill.LB_PLAN DataRowToModel(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet GetList(string strWhere)
        {
            throw new NotImplementedException();
        }


        public ResultModel SetIsEnabled(List<string> PlanID, bool isEnabled)
        {
            if (PlanID == null || PlanID.Count == 0)
            {
                throw new CodeNotValidException();
            }
            PlanID = PlanID.Distinct().ToList();
            int i = 0;
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                i = isEnabled ? _planDAL.SetIsEnabled(PlanID) : _planDAL.SetIsDisabled(PlanID);
                foreach (string id in PlanID)
                {
                    //TODO:增加日志
                    //var log = new LOG_OPERATELOG();
                    //log.DISTRIBUTIONCODE = UserContext.CurrentUser.DistributionCode;
                    //Cloud.Log.API.Tool.AddLogs.AddLOG_OPERATELOG();
                }
                scope.Complete();
            }
            string strPre = isEnabled ? "启用" : "停用";
            if (i <= 0)
            {
                return ErrorResult(strPre + "提货计划失败！");
            }
            else
            {
                return InfoResult(strPre + "提货计划成功！");
            }
        }
    }
}
