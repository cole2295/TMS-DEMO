using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util.OfficeUtil;
using Vancl.TMS.Web.Common;

namespace Vancl.TMS.Web.Areas.LadingBill.Controllers
{
    public class FeedbackController : Controller
    {
        private ILB_TASKBLL _lbTaskbll = ServiceFactory.GetService<ILB_TASKBLL>();

        public ActionResult Export(TaskSearchModel searchModel, string TotalItemCount = "10")
        {

            searchModel.Trim();
            searchModel.PageSize = Convert.ToInt32(TotalItemCount);
            var reportList = _lbTaskbll.GetTaskExport(searchModel);

            if (reportList == null)
            {
                return View("SaveResult", new ResultModel() { IsSuccess = false, Message = "木有数据" });
            }
            List<TaskExportEnter> listExport = new List<TaskExportEnter>();

            foreach (var taskExport in reportList)
            {
                TaskExportEnter taskModel = new TaskExportEnter();
                taskModel.taskcode = taskExport.taskcode;
                taskModel.merchantname = taskExport.merchantname;
                taskModel.warehousename = taskExport.warehousename;
                taskModel.warehouseaddress = taskExport.warehouseaddress;
                taskModel.fromdistributionname = taskExport.fromdistributionname;
                taskModel.todistributioncode = taskExport.distributionname;
                taskModel.department = taskExport.department;
                taskModel.receiveemail = taskExport.receiveemail;
                taskModel.PICKMAN = taskExport.PICKMAN;

                if (taskExport.taskstatus == 1)
                {
                    taskModel.orderquantity = taskExport.PREDICTORDERQUANTITY;
                    taskModel.weight = taskExport.PREDICTWEIGHT;
                }
                else
                {
                    taskModel.orderquantity = taskExport.orderquantity;
                    taskModel.weight = taskExport.weight;
                }
                taskModel.mileage = taskExport.mileage;
                if (taskExport.PICKPRICETYPE == 0)
                {
                    taskModel.ORDERAMOUNT = taskExport.ONCEAMOUNT + "元/次";
                }
                else
                {
                    taskModel.ORDERAMOUNT = taskExport.ORDERAMOUNT + "单/次";
                }

                taskModel.pickgoodsamount = taskExport.pickgoodsamount;
                taskModel.PREDICTTIME = taskExport.PREDICTTIME.ToString();
                if (taskExport.FINISHTIME.Year > 1)
                {
                    taskModel.FINISHTIME = taskExport.FINISHTIME.ToString();
                }


                switch (taskExport.taskstatus.ToString())
                {
                    case "1":
                        taskModel.taskstatus = "新任务";
                        break;
                    case "2":
                        taskModel.taskstatus = "已完成";
                        break;
                    case "3":
                        taskModel.taskstatus = "已考核";
                        break;
                    case "4":
                        taskModel.taskstatus = "已取消";
                        break;
                }

                listExport.Add(taskModel);
            }



            MemoryStream stream = new MemoryStream();
            using (OpenXMLHelper helper = new OpenXMLHelper(stream, OpenExcelMode.CreateNew))
            {
                helper.CreateNewWorksheet("提货反馈");
                helper.WriteData(listExport.To2Array());
                helper.Save();
            }
            stream.Seek(0, 0);
            return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = HttpUtility.UrlPathEncode("提货反馈.xlsx") };
        }
       

        public ActionResult PrintTask(string taskid = "")
        {
            taskid = taskid.Remove(taskid.LastIndexOf(","), 1);
            IList<TaskViewModel> taskView = _lbTaskbll.GetTaskPage(taskid);
            ViewBag.taskView = taskView;

            return View(taskView);
        }

        public ActionResult List()
        {
            #region 登陆用户信息

            UserModel curUser = UserContext.AgentUser;
            if (curUser == null)
            {
                curUser = UserContext.CurrentUser;
            }

            #endregion

            this.SetSearchListAjaxOptions();
            TaskSearchModel taskSearchModel = GetSearchTaskSearchModel();
            taskSearchModel.todisribution = curUser.DistributionCode;
            //taskSearchModel.todisribution = "cqhy";
            if (Request.IsAjaxRequest())
            {
                IList<TaskViewModel> taskView = _lbTaskbll.GetTaskPage(taskSearchModel);
                return PartialView("_PartialFeedBackList", taskView);
            }

            return View();
        }

        /// <summary>
        /// 录入
        /// </summary>
        /// <returns></returns>
        public ActionResult TaskEdit(string taskid = "")
        {
            LB_TASK task = _lbTaskbll.GetModelAll(Convert.ToInt32(taskid));
            ViewBag.taskid = taskid;

            return View(task);
        }

        [HttpPost]
        public JsonResult TaskEdit(LB_TASK model)
        {
            decimal reult = 0;
            if (!decimal.TryParse(model.PREDICTORDERQUANTITY.ToString(), out reult))
            {
                return Json("单量输入不正确", JsonRequestBehavior.AllowGet);
            }
            if (!decimal.TryParse(model.PREDICTWEIGHT.ToString(), out reult))
            {
                return Json("重量输入不正确", JsonRequestBehavior.AllowGet);
            }
            if (!decimal.TryParse(model.MILEAGE.ToString(), out reult))
            {
                return Json("公里数输入不正确", JsonRequestBehavior.AllowGet);
            }

            LB_TASK lbTask = _lbTaskbll.GetModelAll(model.ID);

            model.TASKSTATUS = 2;
            model.FINISHTIME = model.TASKTIME;
            model.ORDERQUANTITY = model.PREDICTORDERQUANTITY;
            model.WEIGHT = model.PREDICTWEIGHT;

            if (lbTask.PICKPRICETYPE == 0)
            {
                //次数计费
                model.PICKGOODSAMOUNT = lbTask.ONCEAMOUNT;
            }
            else
            {
                //单量计费
                model.PICKGOODSAMOUNT = Convert.ToDecimal(lbTask.ORDERAMOUNT) * Convert.ToDecimal(model.ORDERQUANTITY);
            }


            ResultModel resultModel = new ResultModel();

            if (_lbTaskbll.UpdateTaskEdit(model))
            {
                resultModel.IsSuccess = true;
                resultModel.Message = "录入成功";
            }
            else
            {
                resultModel.IsSuccess = false;
                resultModel.Message = "录入失败";
            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获得查询条件
        /// </summary>
        /// <returns></returns>
        public TaskSearchModel GetSearchTaskSearchModel()
        {
            TaskSearchModel taskSearchModel = new TaskSearchModel();

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            taskSearchModel.PageIndex = pageIndex;
            taskSearchModel.PageSize = pageSize;

            #region 取值，赋值
            if (Request["predictTime_sl"] != null && Request["predictTime_sl"] != "")
            {
                taskSearchModel.timeType = Convert.ToInt32(Request["predictTime_sl"]);
            }

            if (Request["startDate"] != null && Request["startDate"] != "")
            {
                taskSearchModel.starTime = Convert.ToDateTime(Request["startDate"]);
            }

            if (Request["endDate"] != null && Request["endDate"] != "")
            {
                taskSearchModel.endTime = Convert.ToDateTime(Request["endDate"]);
            }

            if (Request["taskStatus"] != null && Request["taskStatus"] != "")
            {
                taskSearchModel.taskStatus = Convert.ToInt32(Request["taskStatus"]);
            }

            if (Request["waybillSource_List_hide"] != null && Request["waybillSource_List_hide"] != "")
            {
                taskSearchModel.merchantid = Convert.ToString((Request["waybillSource_List_hide"]));
            }

            if (Request["WAREHOUSEID"] != null && Request["WAREHOUSEID"] != "")
            {
                taskSearchModel.warehouseid = Convert.ToString((Request["WAREHOUSEID"]));
            }

            if (Request["TASKCODE"] != null && Request["TASKCODE"] != "")
            {
                taskSearchModel.taskCode = Convert.ToString((Request["TASKCODE"])).Trim();
            }

            if (Request["distributionDiv_List_hide"] != null && Request["distributionDiv_List_hide"] != "")
            {
                taskSearchModel.FROMDISTRIBUTIONCODE = Convert.ToString((Request["distributionDiv_List_hide"]));
            }

            if (Request["DEPARTMENT"] != null && Request["DEPARTMENT"] != "")
            {
                taskSearchModel.DEPARTMENT = Convert.ToString((Request["DEPARTMENT"])).Trim();
            }
            #endregion

            return taskSearchModel;
        }


    }
}
