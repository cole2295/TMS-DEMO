using System;
using System.Collections.Generic;
using System.Globalization;
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
using Vancl.TMS.Web.Areas.LadingBill.Helper;
using Vancl.TMS.Web.Common;

namespace Vancl.TMS.Web.Areas.LadingBill.Controllers
{
    public class TaskController : Controller
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
            List<TaskExport> listExport = new List<TaskExport>();

            foreach (var taskExport in reportList)
            {
                TaskExport taskModel = new TaskExport();
                taskModel.taskcode = taskExport.taskcode;
                taskModel.merchantname = taskExport.merchantname;
                taskModel.warehousename = taskExport.warehousename;
                taskModel.warehouseaddress = taskExport.warehouseaddress;
                taskModel.distributionname = taskExport.distributionname;
                taskModel.department = taskExport.department;
                taskModel.receiveemail = taskExport.receiveemail;


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
                helper.CreateNewWorksheet("提货任务");
                helper.WriteData(listExport.To2Array());
                helper.Save();
            }
            stream.Seek(0, 0);
            return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = HttpUtility.UrlPathEncode("提货任务.xlsx") };
        }


        /// <summary>
        /// 重新指派
        /// </summary>
        /// <param name="todiscode"></param>
        /// <param name="tmail"></param>
        /// <returns></returns>
        public ActionResult Reinstall(string todiscode = "", string tmail = "")
        {
            ViewBag.todiscode = todiscode;
            ViewBag.tmail = tmail;
            return View();
        }

        /// <summary>
        /// 重新指派
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Reinstall(LB_TASK model, string distributionDiv_List_hide = "", string tmail = "", string taskid = "")
        {
            if (distributionDiv_List_hide == "")
            {
                return Json(ResultModel.Create(false, "请选择提货公司！"), JsonRequestBehavior.AllowGet);
            }
            model.ID = Convert.ToInt32(taskid);
            model.TODISTRIBUTIONCODE = distributionDiv_List_hide;
            model.RECEIVEEMAIL = model.RECEIVEEMAIL.ToLower();
            if (_lbTaskbll.Update(model))
            {
                ResultModel resultModel = new ResultModel();
                resultModel.IsSuccess = true;
                resultModel.Message = "指派成功";

                LB_TASK lbTask = _lbTaskbll.GetModelAll(model.ID);
                //发送新的指派邮件
                _lbTaskbll.senMailByTask(lbTask, 1);

                //发送取消邮件
                lbTask.RECEIVEEMAIL = tmail;
                _lbTaskbll.senMailByTask(lbTask, 3);

                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(ResultModel.Create(false, "指派失败！"), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 取消/恢复
        /// </summary>
        /// <param name="lineids"></param>
        /// <returns></returns>
        public JsonResult SetEnable(string lineids = "")
        {
            bool isEnabled = bool.Parse(Request["isEnabled"]);
            List<string> lineidList = lineids.Split(',').ToList<string>();

            ResultModel resultModel = new ResultModel();

            #region 提交

            if (!isEnabled)
            {
                if (_lbTaskbll.SetIsEnabled(lineidList, 4))
                {
                    resultModel.IsSuccess = true;
                    resultModel.Message = "取消成功";
                }
                else
                {
                    resultModel.IsSuccess = false;
                    resultModel.Message = "取消失败";
                }
            }
            else
            {
                if (_lbTaskbll.SetIsEnabled(lineidList, 1))
                {
                    resultModel.IsSuccess = true;
                    resultModel.Message = "恢复成功";
                }
                else
                {
                    resultModel.IsSuccess = false;
                    resultModel.Message = "恢复失败";
                }
            }

            #endregion

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 考核&批量考核
        /// </summary>
        /// <param name="taskid"></param>
        /// <returns></returns>
        public JsonResult AuditTask(string lpids = "")
        {
            List<string> lineidList = lpids.Split(',').ToList<string>();

            ResultModel resultModel = new ResultModel();

            #region 提交

            if (_lbTaskbll.SetIsEnabled(lineidList, 3))
            {
                resultModel.IsSuccess = true;
                resultModel.Message = "批量审核成功";
            }
            else
            {
                resultModel.IsSuccess = false;
                resultModel.Message = "取消失败";
            }


            #endregion

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 单个任务考核页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Audit(string taskid = "10010")
        {
            LB_TASK task = _lbTaskbll.GetModelAll(Convert.ToInt32(taskid));
            ViewBag.taskid = taskid;
            return View(task);
        }

        /// <summary>
        /// 单个任务考核页面
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Audit(LB_TASK model)
        {
            ResultModel resultModel = new ResultModel();
            decimal reI = 0;
            var kpim = model.KPIAMOUNT;


            model.TASKSTATUS = 3;

            if (_lbTaskbll.UpdateByAudit(model))
            {
                resultModel.IsSuccess = true;
                resultModel.Message = "考核成功";
            }
            else
            {
                resultModel.IsSuccess = false;
                resultModel.Message = "考核失败";
            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            this.SetSearchListAjaxOptions();
            TaskSearchModel taskSearchModel = GetSearchTaskSearchModel();

            if (Request.IsAjaxRequest())
            {
                IList<TaskViewModel> taskView = _lbTaskbll.GetTaskPage(taskSearchModel);
                return PartialView("_PartialTaskList", taskView);
            }

            return View();
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

            UserModel curUser = UserContext.AgentUser;
            if (curUser == null)
            {
                curUser = UserContext.CurrentUser;

                taskSearchModel.FROMDISTRIBUTIONCODE = curUser.DistributionCode;
            }

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
                taskSearchModel.todisribution = Convert.ToString((Request["distributionDiv_List_hide"]));
            }

            if (Request["DEPARTMENT"] != null && Request["DEPARTMENT"] != "")
            {
                taskSearchModel.DEPARTMENT = Convert.ToString((Request["DEPARTMENT"])).Trim();
            }
            #endregion

            return taskSearchModel;
        }



        public ActionResult Details(int id)
        {
            return View();
        }



        public ActionResult Create()
        {
            ViewBag.operate = "create";
            LB_TASK lbTask = new LB_TASK();
            return View(lbTask);
        }



        #region 创建任务

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="model">任务实体</param>
        /// <param name="waybillSource_List_hide">商家ID</param>
        /// <param name="distributionDiv_List_hide">配送商ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(LB_TASK model, string waybillSource_List_hide = "",
                                   string distributionDiv_List_hide = "")
        {
            #region 登陆用户信息

            UserModel curUser = UserContext.AgentUser;
            if (curUser == null)
            {
                curUser = UserContext.CurrentUser;
            }

            #endregion

            DateTime dt = DateTime.Now;

            #region 任务添加

            model.MERCHANTID = Convert.ToInt32(waybillSource_List_hide);
            model.TODISTRIBUTIONCODE = distributionDiv_List_hide;
            model.FROMDISTRIBUTIONCODE = curUser.DistributionCode;
            model.RECEIVEEMAIL = model.RECEIVEEMAIL.ToLower();
            if (model.PICKPRICETYPE == 0)
            {
                model.PICKGOODSAMOUNT = Math.Round(model.ONCEAMOUNT, 2);
            }
            if (model.PICKPRICETYPE == 1)
            {
                model.PICKGOODSAMOUNT = Convert.ToDecimal(model.ORDERAMOUNT) * Convert.ToDecimal(model.ORDERQUANTITY);
            }

            //select sysdate from dual
            DateTime dbtime = _lbTaskbll.GetDBTime();

            model.TASKCODE = _lbTaskbll.CreateTaskCode(curUser.DistributionCode);


            bool resultAdd = false;
            try
            {
                model.TASKSTATUS = 1;
                model.CREATETIME = DateTime.Now;
                if (_lbTaskbll.Add(model))
                {
                    resultAdd = true;
                    _lbTaskbll.senMailByTask(model);
                }
            }
            catch (Exception ex)
            {

            }

            #endregion
            if (resultAdd)
            {
                TempData["msg"] = "添加成功";
            }
            else
            {
                TempData["msg"] = "添加失败";
            }

            return View(model);
        }

        #endregion

    }
}
