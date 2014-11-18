using System;
using System.Collections.Generic;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IDAL.LadingBill
{
    /// <summary>
    /// 接口层提货任务
    /// </summary>
    public interface ILB_TASKDAL
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
        /// 获得TaskViewModel 实体
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        TaskViewModel GetTaskModel(TaskSearchModel searchModel);

        /// <summary>
        /// 获得数据库当前时间
        /// </summary>
        /// <returns></returns>
        DateTime GetDBTime();

        /// <summary>
        /// 根据任务ID查询
        /// </summary>
        /// <param name="taskIDS">日志ID</param>
        /// <returns></returns>
        IList<TaskViewModel> GetTaskPage(string taskIDS);

        /// <summary>
        /// 任务录入
        /// </summary>
        /// <param name="model">任务实体</param>
        /// <returns></returns>
        bool UpdateTaskEdit(LB_TASK model);

        /// <summary>
        /// 任务考核
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateByAudit(LB_TASK model);

        /// <summary>
        /// 根据ID 获得任务实体
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        LB_TASK GetModelAll(decimal ID);

        /// <summary>
        /// 取消，恢复
        /// </summary>
        /// <param name="taskid">任务ID</param>
        /// <param name="taskStatus">状态id</param>
        /// <returns></returns>
        bool SetIsEnabled(List<string> taskid, int taskStatus);

        /// <summary>
        /// 根据任务ID查询数据
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        IList<LB_TASK> GeTasks(List<string> taskid);

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

        /// <summary>
        /// 多条删除
        /// </summary>
        /// <param name="IDlist"></param>
        /// <returns></returns>
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

        #endregion  成员方法

    }
}
