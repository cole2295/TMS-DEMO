using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IBLL.LadingBill
{
    public interface ILB_PLANBLL
    {
        #region  成员方法

        /// <summary>
        /// 把计划生成状态 改为0
        /// </summary>
        /// <returns></returns>
        int UpPlanIsCreate();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        int Add(Vancl.TMS.Model.LadingBill.LB_PLAN model);

        /// <summary>
        /// 更新计划表是否已生成
        /// </summary>
        /// <param name="id">计划ID</param>
        /// <param name="iscreateCode">0没有生成，1已生成</param>
        /// <returns></returns>
        int UpCreate(Decimal id, int iscreateCode);

        /// <summary>
        /// 获得未生成的计划
        /// </summary>
        /// <returns></returns>
        IList<LB_PLAN> GetList();

        /// <summary>
        /// 查询提货计划明细
        /// </summary>
        /// <param name="planID">提货计划ID</param>
        /// <returns></returns>
        LB_PLAN GetPlanDetails(decimal planID);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        int Update(Vancl.TMS.Model.LadingBill.LB_PLAN model);

        PagedList<LB_PLANDTO> GetPlanList(LB_PLANDTO searchModel);

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
        /// 设置线路启用停用状态
        /// </summary>
        /// <param name="lineID">线路id列表</param>
        /// <param name="isEnabled">是否启用</param>
        /// <returns></returns>
        ResultModel SetIsEnabled(List<string> lineID, bool isEnabled);

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        ResultModel Delete(List<string> PlanID);

        #endregion  成员方法
    }
}
