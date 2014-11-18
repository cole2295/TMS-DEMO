using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Transport.Plan;
using Vancl.TMS.IBLL.Transport.Plan;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Web.Areas.Transport.Models;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.IBLL.Log;

namespace Vancl.TMS.Web.Areas.Transport.Controllers
{
    public class PlanController : Controller
    {
        ITransportPlanBLL _transportPlanService = ServiceFactory.GetService<ITransportPlanBLL>();
        ILinePlanBLL _linePlanService = ServiceFactory.GetService<ILinePlanBLL>();

        /// <summary>
        /// 操作日志
        /// </summary>
        /// <param name="LPID"></param>
        /// <param name="LineID"></param>
        /// <returns></returns>
        public ActionResult ShowOperateLog(String TPID)
        {
            IOperateLogBLL oplog = _transportPlanService as IOperateLogBLL;
            return View("OperateLog",
             oplog.Read(new Model.Log.BaseOperateLogSearchModel()
             {
                 KeyValue = TPID
             })
            );
        }

        public ActionResult List()
        {
            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            this.SetSearchListAjaxOptions();
            this.SendEnumSelectListToView<Enums.TransportStatus>("Status");
            var searchModel = new TransportPlanSearchModel();
            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;
            searchModel.Status = (string.IsNullOrEmpty(Request["Status"]) || int.Parse(Request["Status"]) == -1) ? (Enums.TransportStatus?)null : (Enums.TransportStatus)(int.Parse(Request["Status"]));
            searchModel.ArrivalID = string.IsNullOrEmpty(Request["ArrivalID"]) ? (int?)null : int.Parse(Request["ArrivalID"]);
            searchModel.DepartureID = string.IsNullOrEmpty(Request["DepartureID"]) ? (int?)null : int.Parse(Request["DepartureID"]);
            searchModel.Deadline = string.IsNullOrEmpty(Request["Deadline"]) ? (DateTime?)null : DateTime.Parse(Request["Deadline"]);

            if (!Request.IsAjaxRequest())
            {
                searchModel.DepartureID = UserContext.CurrentUser.DeptID;
            }
            PagedList<ViewTransportPlanModel> pagelist = null;
            if (Request.IsAjaxRequest())
            {
                pagelist = _transportPlanService.Search(searchModel);
                return PartialView("_PartialPlanList", pagelist);
            }
            return View(pagelist);
        }

        public ActionResult Create()
        {
            EnumDataBind(0);
            var model = new Vancl.TMS.Model.Transport.Plan.ViewTansportEditorModel();
            model.DeadLine = DateTime.Now;
            model.EffectiveTime = DateTime.Now.AddMinutes(5);
            ViewBag.OperateType = "Add";
            ViewBag.TpId = 0;
            return View(model);
        }

        private void EnumDataBind(int selectedGoodsType)
        {
            List<SelectListItem> goodsTypeList = new List<SelectListItem>();
            Dictionary<int, string> enums = EnumHelper.GetEnumValueAndDescriptions<Enums.GoodsType>();
            foreach (KeyValuePair<int, string> item in enums)
            {
                goodsTypeList.Add(new SelectListItem()
                {
                    Text = item.Value,
                    Value = item.Key.ToString(),
                    Selected = item.Key == selectedGoodsType
                });
            }
            List<SelectListItem> deadLineTypeList = new List<SelectListItem>();
            Dictionary<int, string> enumsDeadLineType = EnumHelper.GetEnumValueAndDescriptions<Enums.TransPortPlanDeadLineType>();
            foreach (KeyValuePair<int, string> item in enumsDeadLineType)
            {
                deadLineTypeList.Add(new SelectListItem()
                {
                    Text = item.Value,
                    Value = item.Key.ToString()
                });
            }
            ViewBag.DeadLineType = deadLineTypeList;
            ViewBag.LineType = goodsTypeList;
        }

        public ActionResult Details(int id)
        {
            ViewTansportEditorModel model = _transportPlanService.GetViewData(id);
            EnumDataBind((int)(model.GoodsType));
            ViewBag.OperateType = "Update";
            ViewBag.TpId = id;
            return View("Create", model);
        }

        /// <summary>
        /// 选择线路计划
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateLinePlanList(LinePlanSearchModel searchModel)
        {
            searchModel.Status = Enums.LineStatus.Effective;
            if (searchModel.LineGoodsType == null)
                searchModel.LineGoodsType = (Enums.GoodsType)Convert.ToInt32(Request["GoodsType"] == "" ? "0" : Request["GoodsType"]);
            string LineID = Request["LineID"];
            if (LineID != string.Empty)
                ViewBag.LineID = LineID;
            PagedList<ViewLinePlanModel> list = _linePlanService.GetLinePlan(searchModel);

            return View("LinePlanList", list);
        }

        public ActionResult CreateTransitStationList()
        {
            PointPathSearchModel searchModel = new PointPathSearchModel();
            searchModel.ArrivalID = Convert.ToInt32(Request["ArrivalID"]);
            searchModel.DepartureID = Convert.ToInt32(Request["DepartureID"]);
            searchModel.IsTransit = true;
            PointPathModel ppm = _transportPlanService.SearchAllPath(searchModel);
            return View("TransitStationList", ppm.TransferStation);
        }

        [HttpPost]
        public JsonResult Save(FormCollection form)
        {
            int ArrivalID = Convert.ToInt32(form["ArrivalID"] == "" ? "-1" : form["ArrivalID"]);
            int DepartureID = Convert.ToInt32(form["DepartureID"] == "" ? "-1" : form["DepartureID"]);
            DateTime DeadLine = Convert.ToDateTime(form["DeadLine"] == "" ? DateTime.Now.ToString("yyyy-MM-dd") : form["DeadLine"]);
            DateTime EffectiveTime = Convert.ToDateTime(form["EffectiveTime"] == "" ? DateTime.Now.ToString("yyyy-MM-dd HH:mm") : form["EffectiveTime"]);
            string Transit = form["TransitID"] == "" ? null : form["TransitID"].ToString();
            bool IsTransit = Transit == null ? false : true;
            Enums.GoodsType GoodsType = (Enums.GoodsType)Convert.ToInt32(form["GoodsType"] == "" ? "0" : form["GoodsType"]);
            string Line1 = form["Line1"];
            string Line2 = form["Line2"];
            string OperateType = form["OperateType"];
            int TpId = form["TpId"] == "" ? 0 : int.Parse(form["TpId"]);
            string TransitLines = form["TransitLines"];
            var Lines = TransitLines.Split(',');
            TransportPlanModel tpm = new TransportPlanModel();
            tpm.TPID = TpId;
            tpm.DepartureID = DepartureID;
            tpm.ArrivalID = ArrivalID;
            tpm.Deadline = DeadLine;
            tpm.IsTransit = IsTransit;
            tpm.LineGoodsType = GoodsType;
            tpm.TransferStationMulti = Transit;
            tpm.EffectiveTime = EffectiveTime;

            List<TransportPlanDetailModel> list = new List<TransportPlanDetailModel>();
            //if (Line1 != String.Empty)
            //{
            //    list.Add(new TransportPlanDetailModel()
            //    {
            //        LineID = Line1,
            //        SeqNo = 1
            //    });
            //}

            //if (Line2 != String.Empty)
            //{
            //    list.Add(new TransportPlanDetailModel()
            //    {
            //        LineID = Line2,
            //        SeqNo = 2
            //    });
            //}
            if (Lines != null && Lines.Length > 0)
            {
                for (int i = 0; i < Lines.Length; i++)
                {
                    list.Add(new TransportPlanDetailModel()
                    {
                        LineID = Lines[i],
                        SeqNo = i + 1
                    });
                    if (!IsTransit)
                    {
                        break;
                    }

                }

            }

            ResultModel r = new ResultModel();
            if (OperateType == "Add")
            {
                r = _transportPlanService.Add(tpm, list);
            }
            if (OperateType == "Update")
            {
                r = _transportPlanService.Update(tpm, list);
            }

            if (r.IsSuccess)
                return Json(string.Empty, JsonRequestBehavior.AllowGet);
            else
                return Json(r.Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            ViewBag.IsTransit = Convert.ToBoolean(form["isTransit"]);
            if (ViewBag.IsTransit)
            {
            }
            else
            {
            }

            var list = new List<Vancl.TMS.Model.Transport.Plan.ViewTansportEditorModel>();
            return PartialView("_PartialChooseLineList", list);
        }

        [HttpPost]
        public ActionResult ChooseLine(TransportPlanBaseSettingViewModel model)
        {
            ViewBag.IsTransit = model.IsTransit;
            if (model.IsTransit)
            {//中转

            }
            else
            {//

            }
            var list = new List<Vancl.TMS.Model.Transport.Plan.ViewTansportEditorModel>();
            return View("create", list);
        }

        [HttpGet]
        public JsonResult Delete()
        {
            string ids = Request["ids"];
            ResultModel rm = _transportPlanService.Delete(ids.Split(',').ConvertArray<int>().ToList());
            return Json(rm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LineList(int id)
        {
            var model = _transportPlanService.GetLinePlanByTpid(id);
            //System.Threading.Thread.Sleep(1000);
            return PartialView("_PartialLineList", model);
        }

        [HttpGet]
        public JsonResult SetIsEnabled()
        {
            string[] arrTPID = Request["listID"].Split(',');
            bool isEnable = Convert.ToInt16(Request["isEnabled"]) == 0 ? false : true;
            if (arrTPID != null && arrTPID.Length > 0)
            {
                List<int> listTPID = new List<int>(arrTPID.Length);
                foreach (var item in arrTPID)
                {
                    listTPID.Add(int.Parse(item));
                }
                ResultModel rm = isEnable ? _transportPlanService.BatchSetToEnabled(listTPID) : _transportPlanService.BatchSetToDisabled(listTPID);
                return Json(rm, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultModel() { IsSuccess = false, Message = "请选择运输计划" }, JsonRequestBehavior.AllowGet);
        }
    }
}
