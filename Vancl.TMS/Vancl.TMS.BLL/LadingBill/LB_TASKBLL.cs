using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.IDAL.LadingBill;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util.Net;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.BLL.LadingBill
{
    public class LB_TASKBLL : BaseBLL, ILB_TASKBLL
    {
        ILB_TASKDAL _dal = ServiceFactory.GetService<ILB_TASKDAL>();
        IMERCHANTWAREHOUSEBLL _merchantwarehousebll = ServiceFactory.GetService<IMERCHANTWAREHOUSEBLL>();


        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public IList<TaskViewModel> GetTaskExport(TaskSearchModel searchModel)
        {
            return _dal.GetTaskExport(searchModel);
        }

        /// <summary>
        /// 生成任务编号
        /// </summary>
        /// <returns></returns>
        public string CreateTaskCode(string distributionCode)
        {
            string taskcode = string.Empty;

            DateTime dbtime = _dal.GetDBTime();
            //需改
            taskcode = distributionCode.ToUpper()
                             + dbtime.Year.ToString().Substring(2, 2)
                             + dbtime.Month.ToString().PadLeft(2, '0')
                             + dbtime.Day.ToString().PadLeft(2, '0');

            string todayCount = _dal.ToDayTaskCount().ToString();

            if (Convert.ToInt32(todayCount) < 10)
            {
                todayCount = "00" + todayCount;
            }
            else if (Convert.ToInt32(todayCount) > 9 && Convert.ToInt32(todayCount) < 99)
            {
                todayCount = "0" + todayCount;
            }

            return taskcode + "-" + todayCount;
        }

        /// <summary>
        /// 获得TaskViewModel 实体
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public TaskViewModel GetTaskModel(TaskSearchModel searchModel)
        {
            return _dal.GetTaskModel(searchModel);
        }

        public bool UpdateByAudit(LB_TASK model)
        {
            return _dal.UpdateByAudit(model);
        }

        public DateTime GetDBTime()
        {
            return _dal.GetDBTime();
        }

        public bool Exists(decimal ID)
        {
            throw new NotImplementedException();
        }

        #region 添加任务

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="model">任务实体</param>
        /// <returns></returns>
        public bool Add(Model.LadingBill.LB_TASK model)
        {
            var result = _dal.Add(model);
            if (result)
            {
                UserModel curUser = UserContext.AgentUser;
                if (curUser == null)
                {
                    curUser = UserContext.CurrentUser;
                }

                //LOG_OPERATELOG logOperatelog = new LOG_OPERATELOG();
                //logOperatelog.SYSTEMID = model.TASKCODE;
                //logOperatelog.MODULE = "添加";
                //logOperatelog.LOGMAN = curUser.UserName;
                //logOperatelog.LOGTIME = DateTime.Now;
                //logOperatelog.DISTRIBUTIONCODE = model.FROMDISTRIBUTIONCODE;
                //logOperatelog.LOGTXT = "新增";

                //LogManager.AddOperate(logOperatelog);
            }

            return result;
        }

        #endregion

        #region 发送任务邮件
        /// <summary>
        /// 发送任务邮件
        /// </summary>
        /// <param name="model"></param>
        public void senMailByTask(Model.LadingBill.LB_TASK model, int type = 1)
        {
            MERCHANTWAREHOUSE merchantwarehouse = _merchantwarehousebll.GetModelByid(model.WAREHOUSEID);
            string head = "";
            string title = "";
            UserModel curUser = UserContext.AgentUser;
            if (curUser == null)
            {
                curUser = UserContext.CurrentUser;
            }

            TaskSearchModel taskSearch = new TaskSearchModel();
            taskSearch.FROMDISTRIBUTIONCODE = model.FROMDISTRIBUTIONCODE;
            var taskView = _dal.GetTaskModel(taskSearch);

            //北京分拣中心已下达任务[201307241543]
            switch (type)
            {
                case 1:
                    head = "已下达如下提货任务，请安排提货。";
                    title = taskView.fromdistributionname + " 已下达提货任务[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "]";
                    break;
                case 2:
                    //上海分拣中心 已取消提货任务[RFD130803-005]
                    head = "已下任务已取消，请不要安排提货。";
                    title = taskView.fromdistributionname + " 已取消提货任务[" + model.TASKCODE + "]";
                    break;
                case 3:
                    head = "提货任务已取消，请不要安排提货。";
                    title = taskView.fromdistributionname + " 已取消提货任务[" + model.TASKCODE + "]";
                    break;
            }

            #region 邮件主体信息

            string MailBody = string.Empty;
            MailBody +=
                string.Format(
                    "<p style=\"font-size: 10pt\">你好！ 【{0}】{1}</p>  <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">",
                    taskView.fromdistributionname, head);
            MailBody += "<div align=\"center\">";
            MailBody +=
                "<tr>                 <td style=\"border-top: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC; width: 150px\">                     任务编号                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC; width: 100px\">                     商家                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC; width: 360px\">                     库房地址                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC; width: 100px\">                     联系人                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC; width: 100px\">                     联系电话                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC; width: 100px\">                     预计提货重量                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC; width: 100px\">                     预计提货单量                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-right: 1px solid #CCCCCC; width: 100px;                     border-left: 1px solid #CCCCCC\">                     提货时间                 </td>             </tr>";

            MailBody +=
                "<tr>                 <td style=\"border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC\">                     " + model.TASKCODE + "                </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC\">                     " + merchantwarehouse.MERCHANTNAME + "                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC\">                     " + merchantwarehouse.WAREHOUSEADDRESS + "                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC\">                     " + merchantwarehouse.LINKMAN + "                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC\">                     " + merchantwarehouse.PHONE + "                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC\">                     " + model.PREDICTORDERQUANTITY + "                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC\">                     " + model.PREDICTWEIGHT + "                 </td>                 <td style=\"border-top: 1px solid #CCCCCC; border-right: 1px solid #CCCCCC; border-bottom: 1px solid #CCCCCC; border-left: 1px solid #CCCCCC\">                     " + model.PREDICTTIME + "                 </td>             </tr>";
            MailBody += "</table>";
            MailBody += "</div>";

            #endregion

            try
            {
                #region 发送邮件
                if (model.RECEIVEEMAIL.IndexOf(';') > -1)
                {
                    MailMessage mail = new MailMessage(Consts.SMTP_FROM, model.RECEIVEEMAIL.Substring(0, model.RECEIVEEMAIL.IndexOf(';')), taskView.fromdistributionname + " 已下达提货任务[" + DateTime.Now.ToString("yyyyMMddHHmmss") + "]", MailBody);
                    mail.IsBodyHtml = true;

                    for (int i = 0; i < model.RECEIVEEMAIL.Split(';').Length; i++)
                    {
                        if (i + 1 == model.RECEIVEEMAIL.Split(';').Length) { break; }
                        mail.To.Add(model.RECEIVEEMAIL.Split(';')[i + 1]);
                    }
                    MailHelper.Send(Consts.SMTP_HOST, new NetworkCredential(Consts.SMTP_ACCOUNT, Consts.SMTP_PASSWORD), mail);
                }
                else
                {
                    MailMessage mail = new MailMessage(Consts.SMTP_FROM, model.RECEIVEEMAIL, title, MailBody);
                    mail.IsBodyHtml = true;

                    MailHelper.Send(Consts.SMTP_HOST, new NetworkCredential("", ""), mail);
                }
                #endregion
            }
            catch (Exception)
            {

            }

        }

        #endregion

        public bool Update(Model.LadingBill.LB_TASK model)
        {
            return _dal.Update(model);
        }

        public bool Delete(decimal ID)
        {
            throw new NotImplementedException();
        }

        public bool DeleteList(string IDlist)
        {
            throw new NotImplementedException();
        }

        #region 根据任务id获得任务实体
        /// <summary>
        /// 根据任务id获得任务实体
        /// </summary>
        /// <param name="ID">任务ID</param>
        /// <returns></returns>
        public Model.LadingBill.LB_TASK GetModel(decimal ID)
        {
            return _dal.GetModel(ID);
        }

        #endregion

        public Model.LadingBill.LB_TASK DataRowToModel(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataSet GetList(string strWhere)
        {
            throw new NotImplementedException();
        }

        public int ToDayTaskCount()
        {
            return _dal.ToDayTaskCount();
        }

        public IList<TaskViewModel> GetTaskPage(string taskIDS)
        {
            return _dal.GetTaskPage(taskIDS);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="searchModel">查询条件</param>
        /// <returns></returns>
        public PagedList<TaskViewModel> GetTaskPage(TaskSearchModel searchModel)
        {
            return _dal.GetTaskPage(searchModel);
        }

        public bool UpdateTaskEdit(LB_TASK model)
        {
            return _dal.UpdateTaskEdit(model);
        }

        /// <summary>
        /// 取消，恢复
        /// </summary>
        /// <param name="taskid">任务ID</param>
        /// <param name="taskStatus">状态id</param>
        /// <returns></returns>
        public bool SetIsEnabled(System.Collections.Generic.List<string> taskid, int taskStatus)
        {
            bool result = _dal.SetIsEnabled(taskid, taskStatus);
            if (result)
            {
                if (taskStatus == 4)
                {
                    IList<LB_TASK> listTask = _dal.GeTasks(taskid);
                    foreach (var lbTask in listTask)
                    {
                        try
                        {
                            senMailByTask(lbTask, 2);
                        }
                        catch (Exception)
                        {

                        }

                    }
                }
            }
            return result;
        }

        public LB_TASK GetModelAll(decimal ID)
        {
            return _dal.GetModelAll(ID);
        }

        public IList<LB_TASK> GeTasks(List<string> taskid)
        {
            return _dal.GeTasks(taskid);
        }
    }
}
