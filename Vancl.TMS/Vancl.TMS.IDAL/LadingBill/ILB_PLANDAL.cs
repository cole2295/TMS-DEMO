using System;
using System.Collections.Generic;
using System.Data;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IDAL.LadingBill
{
    /// <summary>
    /// 接口层提货计划
    /// </summary>
    public interface ILB_PLANDAL
    {
        #region  成员方法

        /// <summary>
        /// 把计划生成状态 改为0
        /// </summary>
        /// <returns></returns>
        int UpPlanIsCreate();

        /// <summary>
        /// 获得未生成的计划
        /// </summary>
        /// <returns></returns>
        IList<LB_PLAN> GetList();

        /// <summary>
        /// 更新计划表是否已生成
        /// </summary>
        /// <param name="id">计划ID</param>
        /// <param name="iscreateCode">0没有生成，1已生成</param>
        /// <returns></returns>
        int UpCreate(Decimal id, int iscreateCode);

        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(Vancl.TMS.Model.LadingBill.LB_PLAN model);

        LB_PLAN GetPlanDetails(decimal planID);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        int Update(Vancl.TMS.Model.LadingBill.LB_PLAN model);

        PagedList<LB_PLANDTO> GetPlanList(LB_PLANDTO searchModel);

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        int Delete(List<string> PlanID);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        Vancl.TMS.Model.LadingBill.LB_PLAN GetModel();
        Vancl.TMS.Model.LadingBill.LB_PLAN DataRowToModel(DataRow row);
        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetList(string strWhere);

        /// <summary>
        /// 置为可用
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        int SetIsEnabled(List<string> PlanID);

        /// <summary>
        /// 置为不可用
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        int SetIsDisabled(List<string> PlanID);

        #endregion  成员方法

        #region  MethodEx

        #endregion  MethodEx
    }
}
