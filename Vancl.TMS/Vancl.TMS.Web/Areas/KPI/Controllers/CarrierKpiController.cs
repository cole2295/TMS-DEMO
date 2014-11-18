using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Model.Common;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Delivery.KPIAppraisal;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Core.Logging;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.Web.Areas.KPI.Controllers
{
    public class CarrierKpiController : Controller
    {
        ICarrierBLL CarrierService = ServiceFactory.GetService<ICarrierBLL>();
        IDeliveryAssessmentBLL AssessmenService = ServiceFactory.GetService<IDeliveryAssessmentBLL>();

        public ActionResult List(DeliveryAssessmentSearchModel searchModel)
        {
            this.SetSearchListAjaxOptions();
            InitSelectList();

            int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
            int pageIndex = Convert.ToInt32(Request["page"] ?? "1");

            searchModel.PageIndex = pageIndex;
            searchModel.PageSize = pageSize;

            if (!string.IsNullOrWhiteSpace(Request["BeginTime"]))
            {
                searchModel.BeginTime = Convert.ToDateTime(Request["BeginTime"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["EndTime"]))
            {
                searchModel.EndTime = Convert.ToDateTime(Request["EndTime"]);
            }
            if (!String.IsNullOrWhiteSpace(Request["DepartureID"]))
            {
                searchModel.DepartureID = Convert.ToInt32(Request["DepartureID"]);
            }
            if (!String.IsNullOrWhiteSpace(Request["CarrierID"]))
            {
                searchModel.CarrierID = Convert.ToInt32(Request["CarrierID"]);
            }
            if (!String.IsNullOrWhiteSpace(Request["DelaySpanBegin"]))
            {
                searchModel.DelaySpanBegin = Convert.ToDecimal(Request["DelaySpanBegin"]);
            }
            if (!String.IsNullOrWhiteSpace(Request["DelaySpanEnd"]))
            {
                searchModel.DelaySpanEnd = Convert.ToDecimal(Request["DelaySpanEnd"]);
            }
            if (!String.IsNullOrWhiteSpace(Request["DeliveryStatus"]))
            {
                searchModel.DeliveryStatus = (Enums.DeliveryStatus)Convert.ToInt32(Request["DeliveryStatus"]);
            }
               
            searchModel.Trim();
            IList<ViewDeliveryAssessmentModel> model=null;
            if (!Request.IsAjaxRequest())
            {
                if (!string.IsNullOrWhiteSpace(Request["isReturn"]) && Request["isReturn"] == "1")
                {
                    model = AssessmenService.Search(searchModel);
                 }
            }
            if (Request.IsAjaxRequest())
            {
                model = AssessmenService.Search(searchModel);
                return PartialView("_PartialCarrierkpiList", model);
            }
            return View(model);
        }

        /// <summary>
        /// 考核
        /// </summary>
        public ActionResult Appraisal(string id)
        {
            var assPriceModel = AssessmenService.SearchAssPrice(id);
            KPICalcInputModel inputModel = new KPICalcInputModel()
            {
                AssPriceList = assPriceModel.AssPriceList,
                DeliveryNo = assPriceModel.DeliveryNo,
                Discount = assPriceModel.KPIDelayType == Enums.KPIDelayType.DelayDiscount ? assPriceModel.Discount : assPriceModel.DelayAmount,
                ExpressionType = assPriceModel.ExpressionType,
                InsuranceRate = assPriceModel.InsuranceRate,
                IsDelayAssess = assPriceModel.IsDelayAssess,
                KPIDelayType = assPriceModel.KPIDelayType,
                OtherAmount = assPriceModel.OtherAmount,
                LongDeliveryAmount = assPriceModel.LongDeliveryAmount,
                LongPickPrice = assPriceModel.LongPickPrice,
                LongTransferRate = assPriceModel.LongTransferRate,
                LostDeduction = assPriceModel.LostDeduction,
                ProtectedPrice = assPriceModel.ProtectedPrice
            };
            ViewBag.ViewAssPriceModel = assPriceModel;
            //KPI延误计费类型
            ViewBag.KPIDelayType = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.KPIDelayType>()
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = (int)assPriceModel.KPIDelayType == x.Key ? true : false,
                });
            ViewBag.PreParams = Request["preParams"];

            return View(inputModel);
        }

        [HttpPost]
        public ActionResult Appraisal(string id, KPICalcInputModel inputModel, string assPriceList, string submit)
        {
            try
            {
                switch (inputModel.ExpressionType)
                {
                    case Enums.ExpressionType.Fixed:
                        inputModel.AssPriceList = System.Web.Helpers.Json.Decode<List<AssFixedPriceModel>>(assPriceList)
                            .Select(x => x as IAssPriceModel).ToList();
                        break;
                    case Enums.ExpressionType.Formula:
                        inputModel.AssPriceList = System.Web.Helpers.Json.Decode<List<AssFormulaPriceModel>>(assPriceList)
                            .Select(x => x as IAssPriceModel).ToList();
                        break;
                    case Enums.ExpressionType.Ladder:
                        inputModel.AssPriceList = System.Web.Helpers.Json.Decode<List<AssLadderPriceModel>>(assPriceList)
                            .Select(x => x as IAssPriceModel).ToList();
                        break;
                    default:
                        throw new Exception("系统未设置" + inputModel.ExpressionType + "的运费类型。");
                }
                if (submit == "计算")
                {
                    var model = AssessmenService.KPICalculate(inputModel);
                    model.LongPickPrice = inputModel.LongPickPrice;
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else if (submit == "保存")
                {
                    ResultModel model;
                    if (AssessmenService.IsExist(inputModel.DeliveryNo))
                    {
                        model = AssessmenService.Update(inputModel);
                    }
                    else
                    {
                        model = AssessmenService.Add(inputModel);
                    }
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("未知提交信息");
                }
            }
            catch (Exception ex)
            {
                Log.loggeremail.Error(ex.Message, ex);
                return Json(new ResultModel { IsSuccess = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 初始化搜索选择列表
        /// </summary>
        private void InitSelectList()
        {
            //承运商
            ViewBag.CarrierID = CarrierService.GetAll()
                .Select(x => new SelectListItem
                {
                    Text = x.CarrierName,
                    Value = x.CarrierID.ToString(),
                    Selected = false,
                });
            //提货单状态
            ViewBag.DeliveryStatus = EnumHelper.GetEnumValueAndDescriptionsEx<Enums.DeliveryStatus>()
                .Where(x => x.Key >= (int)Enums.DeliveryStatus.ArrivedOnTime)
                .Select(x => new SelectListItem
                {
                    Text = x.Value,
                    Value = x.Key.ToString(),
                    Selected = false,
                });
        }
    }
}
