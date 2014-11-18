using System;
using System.Collections.Generic;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.LadingBill;

namespace Vancl.TMS.IBLL.LadingBill
{
    public interface ILB_TASKBLL
    {
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(decimal ID);

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        IList<TaskViewModel> GetTaskExport(TaskSearchModel searchModel);

        /// <summary>
        /// 生成任务编号
        /// </summary>
        /// <returns></returns>
        string CreateTaskCode(string distributionCode);

        /// <summary>
        /// 获得TaskViewModel 实体
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        TaskViewModel GetTaskModel(TaskSearchModel searchModel);

        DateTime GetDBTime();

        IList<TaskViewModel> GetTaskPage(string taskIDS);

        IList<LB_TASK> GeTasks(List<string> taskid);

        bool UpdateTaskEdit(LB_TASK model);

        bool UpdateByAudit(LB_TASK model);

        /// <summary>
        /// 发送任务邮件
        /// </summary>
        /// <param name="model">任务实体</param>
        /// <param name="type">1下达任务     2取消任务</param>
        void senMailByTask(Model.LadingBill.LB_TASK model, int type = 1);

        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="taskid">任务ID</param>
        /// <param name="taskStatus">状态id</param>
        /// <returns></returns>
        bool SetIsEnabled(List<string> taskid, int taskStatus);

        LB_TASK GetModelAll(decimal ID);

        /// <summary>
        /// 查询任务计划
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        PagedList<TaskViewModel> GetTaskPage(TaskSearchModel searchModel);

        /// <summary>
        /// 获得当天任务数量
        /// </summary>
        /// <returns></returns>
        int ToDayTaskCount();

        /// <summary>
        /// 增加一条数据
        /// </summary>
        bool Add(Vancl.TMS.Model.LadingBill.LB_TASK model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(Vancl.TMS.Model.LadingBill.LB_TASK model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(decimal ID);
        bool DeleteList(string IDlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        Vancl.TMS.Model.LadingBill.LB_TASK GetModel(decimal ID);
        Vancl.TMS.Model.LadingBill.LB_TASK DataRowToModel(DataRow row);
        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetList(string strWhere);
        /// <summary>
        /// 根据分页获得数据列表
        /// </summary>
        //DataSet GetList(int PageSize,int PageIndex,string strWhere);
        #endregion  成员方法
    }
}
