using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.RangeDefined;
using Vancl.TMS.Util.Extensions;
using Vancl.TMS.Web.Common;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
	public class RangeDefinedController : Controller
	{
		IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
		//
		// GET: /Sorting/RangeDefined/

		public ActionResult Index()
		{
			return View();
		}

		//
		//GET: /Sorting/RangeDefined/SearchRange
		public ActionResult SearchRange()
		{
			this.SetSearchListAjaxOptions();
			ViewBag.RedirectAction = "SearchRange";
			ViewBag.DistributionCode = UserContext.CurrentUser.DistributionCode;
			if (String.Equals("get", Request.HttpMethod, StringComparison.CurrentCultureIgnoreCase))
			{
				InitRangeSearchSelectList();
				//初始化页面时不查询
				return View();
			}
			//获取查询Model
			RangeDefinedSearchModel searchModel = new RangeDefinedSearchModel();

			int pageSize = Convert.ToInt32(Request["PageSize"] ?? "10");
			int pageIndex = Convert.ToInt32(Request["page"] ?? "1");
			searchModel.PageIndex = pageIndex;
			searchModel.PageSize = pageSize;

			string ids = "-1";
			int chooseType = -1;
			GetConditions(ref chooseType, ref ids);

			searchModel.CompanyFlag = chooseType;
			searchModel.RangedExpressCompanyIds = ids;
			searchModel.BaseExpressCompanyId = Convert.ToInt32(Request["SortingCenterList"]);

			if (Request.IsAjaxRequest())
			{
				var ViewRangeDefined = _expressCompanyBLL.GetRangeDefineInfo(searchModel);
				ViewBag.chooseType = chooseType;
				return PartialView("_PartialSearchRangeList", ViewRangeDefined);
			}
			return View();
		}

		//添加分拣范围
		//GET: /Sorting/RangeDefined/AddRangeDefined
		public ActionResult AddRangeDefined()
		{
			this.SetSearchListAjaxOptions();
			ViewBag.DistributionCode = UserContext.CurrentUser.DistributionCode;
			if (String.Equals("get", Request.HttpMethod, StringComparison.CurrentCultureIgnoreCase))
			{
				InitRangeSearchSelectList();
				return View();
			}
			//获取Create对应的Model
			#region
			RangeDefinedModel createModel = new RangeDefinedModel();

			string ids = "-1";
			int chooseType = -1;
			GetConditions(ref chooseType, ref ids);
			if (chooseType == (int)Enums.CompanyFlag.Distributor)
			{
				ids = ids.Substring(0, ids.IndexOf(','));
			}
			createModel.RangedCompanyFlag = chooseType;
			createModel.RangedExpressCompanyId = Convert.ToInt32(ids);
			createModel.BaseExpressCompanyId = Convert.ToInt32(Request["SortingCenterList"]);
			#endregion
			if (Request.IsAjaxRequest())
			{
				ResultModel result = new ResultModel();
				//判断是否已添加
				if (!_expressCompanyBLL.IsExistsOfRangeDefined(createModel))
				{
					//新增数据
					bool resultAdd = false;
					try
					{
						var i = _expressCompanyBLL.AddRangeDefined(createModel);
						if (i > 0)
						{
							resultAdd = true;
						}
					}
					catch (Exception ex)
					{
						throw;
					}
					if (resultAdd)
					{
						result.IsSuccess = true;
						result.Message = "保存成功";
					}
					else
					{
						result.IsSuccess = false;
						result.Message = "保存失败";
					}
				}
				else
				{
					//已存在
					var ExistInfo = _expressCompanyBLL.GetExistInfoByStationId(chooseType, Convert.ToInt32(ids));

					result.IsSuccess = false;
					result.Message = "将要添加的分拣范围：【" + ExistInfo.RangeDefined + "】"
									 + "已被定义到" + "【" + ExistInfo.SortingCenter + "】";
				}
				return Json(result, JsonRequestBehavior.AllowGet);
			}

			return View();
		}

		//删除分拣范围定义
		[HttpGet]
		public JsonResult Delete()
		{
			string ids = Request["ids"];
			List<int> rangeidList = ids.Split(',').Select(n => int.Parse(n)).ToList();
			ResultModel result = _expressCompanyBLL.DeleteRangeDefined(rangeidList);
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		private void GetConditions(ref int chooseType, ref string ids)
		{
			chooseType = Convert.ToInt32(Request["sortingCenterSelect"]);

			if (chooseType == (int)Enums.CompanyFlag.SortingCenter)
			{
				var SortingCenterList = Request["SortingCenterListWithoutSelf"];
				if (SortingCenterList != null && SortingCenterList != "-1")
				{
					ids = SortingCenterList;
				}
			}
			else if (chooseType == (int)Enums.CompanyFlag.DistributionStation)
			{
				var selCityAndStation_Station = Request["selCityAndStation_Station"];
				if (selCityAndStation_Station != null && selCityAndStation_Station != "-1")
				{
					ids = selCityAndStation_Station;
				}
			}
			else
			{
				var DistributorList = Request["distributionDiv_List_hide"];
				if (DistributorList != null && DistributorList != "-1")
				{
					//获取到得是配送商的简码
					//var DistributorList = Request["distributionIds"];
					var exps = _expressCompanyBLL.GetAllDistributors();
					string s = "";
					foreach (var exp in exps)
					{
						if (DistributorList.Split(',').ToList().Contains(exp.Expresscompanycode))
						{
							s += exp.ID + ",";
						}
					}
					ids = s;
				}
			}
		}

		[HttpPost]
		public ActionResult Export(string rangeIds = "", int ExportChooseType = 0)
		{
			var list = rangeIds.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
			var ReportList = _expressCompanyBLL.GetRangeDefinedPrintExportModel(list, ExportChooseType);
			var bs = ReportList.ExportToExcel("分拣范围定义");
			MemoryStream stream = new MemoryStream(bs);
			stream.Seek(0, 0);
			return new FileStreamResult(stream, "application/vnd.ms-excel") { FileDownloadName = "分拣范围定义" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls" };
		}
		/// <summary>
		/// 初始化查询分拣范围检索条件
		/// </summary>
		private void InitRangeSearchSelectList()
		{
			//分拣中心列表
			List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCode();
			this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);

			//分拣中心列表
			List<ExpressCompanyModel> expressListWithoutSelf =
				_expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
			this.SendSelectListToView<ExpressCompanyModel>(expressListWithoutSelf, "SortingCenterListWithoutSelf", true);

			//配送商列表
			List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
			this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
		}

	}
}
