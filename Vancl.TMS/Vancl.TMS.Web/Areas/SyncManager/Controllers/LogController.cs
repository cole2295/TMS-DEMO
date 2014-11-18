using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Web.Areas.SyncManager.Controllers
{
    public class LogController : Controller
    {
        IServiceLogBLL logService = ServiceFactory.GetService<IServiceLogBLL>();

        public ActionResult List()
        {
            this.SetSearchListAjaxOptions();

            ServiceLogSearchModel searchModel = new ServiceLogSearchModel();
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            searchModel.IsSuccessed = false;
            searchModel.IsHandeld = false;

            if (string.IsNullOrWhiteSpace(Request["CreateTime"]))
            {
                searchModel.CreateTime = DateTime.Now.AddDays(-3);
            }
            else
            {
                searchModel.CreateTime = DateTime.Parse(Request["CreateTime"].ToString());
            }

            if (!string.IsNullOrWhiteSpace(Request["FormCode"]))
            {
                searchModel.FormCode = Request["FormCode"];
            }

            if (!string.IsNullOrWhiteSpace(Request["IsSuccessed"]))
            {
                bool isAll = Convert.ToBoolean(Request["IsSuccessed"] == "no");
                if (!isAll)
                    searchModel.IsSuccessed = null;
            }

            if (!string.IsNullOrWhiteSpace(Request["IsHandled"]))
            {
                bool isAll = Convert.ToBoolean(Request["IsHandled"] == "no");
                if (!isAll)
                    searchModel.IsHandeld = null;
            }

            if (!string.IsNullOrWhiteSpace(Request["OpFunction"]))
            {
                searchModel.OpFunction = Convert.ToInt32(Request["OpFunction"]);
            }

            if (!string.IsNullOrWhiteSpace(Request["ServiceLogType"]))
            {
                searchModel.LogType = (Model.Common.Enums.ServiceLogType)Convert.ToInt32(Request["ServiceLogType"]);
            }

            searchModel.Trim();

            var pagelist = logService.ReadLogs(searchModel);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_PartialLogList", pagelist);
            }

            this.SendEnumSelectListToView<Enums.ServiceLogType>("ServiceLogType");
            this.SendEnumSelectListToView<Enums.TmsOperateType>("Tms2LmsOperateType");
            this.SendEnumSelectListToView<Enums.Lms2TmsOperateType>("Lms2TmsOperateType");
            return View(pagelist);
        }

        [HttpGet]
        public JsonResult CompareOperatePwd()
        {
            string inputValue = Request["password"].ToString();
            bool r = inputValue == Consts.SYNC_OPERATE_PWD;
            ResultModel result = new ResultModel();
            if (r)
            {
                result.IsSuccess = true;
                result.Message = "验证通过";
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "密码错误";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ResetLogStatus()
        {
            string logIds = Request["ids"];
            string[] keys = logIds.Split(';');
            ResultModel result = new ResultModel();
            if (keys.Length > 0)
            {
                int j = 0;
                for (int i = 0; i < keys.Length; i++)
                {
                    string[] arr = keys[i].Split(',');
                    if (logService.ResetSyncFlag(long.Parse(arr[0]), arr[1], (Enums.ServiceLogType)int.Parse(arr[2])) == 1)
                        j++;
                }
                if (j == keys.Length)
                {
                    result.IsSuccess = true;
                    result.Message = "操作成功,已重新同步" + j + "条记录";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "操作失败或部分操作成功,影响记录" + j + "条";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}
