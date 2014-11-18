using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;
using System.Collections;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Security;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Util.JsonUtil;
using Vancl.TMS.Web.Areas.Sorting.Models;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Inbound.Packing;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.IBLL.Sorting.Return;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.IBLL.Sorting.Outbound;
using System.IO;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class ReturnController : Controller
    {
        IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
        //退货分拣称重
        IReturnBLL _returnBLL = ServiceFactory.GetService<IReturnBLL>();
        IBillReturnDetailInfoBLL _returnBillDetailInfoBLL = ServiceFactory.GetService<IBillReturnDetailInfoBLL>();
        IBillBLL _billBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
        IBillReturnBoxInfoBLL _returnBillBoxBLL = ServiceFactory.GetService<IBillReturnBoxInfoBLL>();
        IMerchantBLL _merchantBLL = ServiceFactory.GetService<IMerchantBLL>();
        ISortingPackingBLL _sortingPackingBLL = ServiceFactory.GetService<ISortingPackingBLL>("SortingPackingBLL");
        //返货入库
        IBillReturnBLL _billReturn = ServiceFactory.GetService<IBillReturnBLL>();
        IOutboundBLL _outBoundBLL = ServiceFactory.GetService<IOutboundBLL>("SC_OutboundBLL");

        //商家入库确认
        ITruckBLL _truckService = ServiceFactory.GetService<ITruckBLL>("TruckBLL");
        IEmployeeBLL _employeeService = ServiceFactory.GetService<IEmployeeBLL>("EmployeeBLL");

        //
        // GET: /Sorting/BackSortPacking/
        #region 退货分拣称重页面相关
        #region 构建页面
        public ActionResult Index()
        {
            //商家列表
            ViewBag.MerchantList = _merchantBLL.GetMerchantListByDistributionCode(UserContext.CurrentUser.DistributionCode).OrderBy(x => x.Name).ToList();

            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            expressList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //接货配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetAllDistributors();
            distributorList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
            this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
            SortCenterUserModel userModel = _returnBLL.GetUserModel(UserContext.CurrentUser.ID);
            ViewReturnValidateModel viewModel = new ViewReturnValidateModel(userModel);
            ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));
            return View();
        }
        #endregion
        #region 扫描运单号
        /// <summary>
        /// 扫描单号
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="selectedType"></param>
        /// <param name="selectStationValue"></param>
        /// <param name="code"></param>
        /// <param name="boxNo"></param>
        /// <param name="isFirst"></param>
        /// <param name="formType"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ScanFormCode(ReturnScanFormCodeModel scanFormCodeModel)
        {
            BillModel model = new BillModel();
            string Returnto = "";
            if (!string.IsNullOrEmpty(scanFormCodeModel.selectStationName))
            {
                Returnto = scanFormCodeModel.selectStationName.Trim();
            }
            string BoxNo = scanFormCodeModel.BoxNo;
            var ReturnData = JsonHelper.ConvertToObject<ViewReturnValidateModel>(DES.Decrypt3DES(scanFormCodeModel.hidData));
            if (!ReturnData.IsValidate)
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = ReturnData.ValidateMsg });
            }
            string Message = isValid(scanFormCodeModel.selectedType, scanFormCodeModel.Code, scanFormCodeModel.selectStationValue, scanFormCodeModel.FormType, out model);
            if (!string.IsNullOrEmpty(Message))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = Message });
            }
            string returnBoxNo = "";
            bool isreturn = _returnBillDetailInfoBLL.IsReturn(model.FormCode, Returnto, out returnBoxNo);
            if (isreturn)
            {
                if (!string.IsNullOrEmpty(returnBoxNo))
                {
                    return Json(new BillReturnResultModel() { IsSuccess = false, Message = "此单已经退货装箱称重，箱号为：" + returnBoxNo });
                }
                else
                {
                    return Json(new BillReturnResultModel() { IsSuccess = false, Message = "此单已经退货称重！" });
                }
            }
            //构造隐藏的今日扫描运单号数据
            var hidBill = scanFormCodeModel.hidBillNos;
            int count = 0;
            if (scanFormCodeModel.isInBox)
            {

                string ReturnMerchant = scanFormCodeModel.selectedType == "rdoMerchant" ? (scanFormCodeModel.selectStationValue == "0" ? "" : scanFormCodeModel.selectStationName) : Returnto;

                if (scanFormCodeModel.isFirst)
                {
                    if (scanFormCodeModel.BoxNo == null) scanFormCodeModel.BoxNo = "";
                    BoxNo = GetBoxNo(scanFormCodeModel.BoxNo.Trim(), ReturnMerchant, Returnto);
                }
            }
            //运单不称重直接添加信息
            //if (!scanFormCodeModel.isNeedWeight)
            //{
            BillReturnDetailInfoModel returnModel = new BillReturnDetailInfoModel()
            {
                BoxNo = BoxNo,
                CreateBy = UserContext.CurrentUser.ID,
                UpdateBy = UserContext.CurrentUser.ID,
                FormCode = model.FormCode,
                ReturnTo = Returnto,
                CustomerOrder = model.CustomerOrder,
                Weight = scanFormCodeModel.Weight,
                CreateDept = UserContext.CurrentUser.DeptName
            };
            if (_returnBillDetailInfoBLL.AddDetail(returnModel) > 0)
            {
                hidBill = string.IsNullOrEmpty(hidBill) ? model.FormCode : hidBill + "," + model.FormCode;
                count = string.IsNullOrEmpty(hidBill) ? 0 : hidBill.Split(',').Length;
            }
            else
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "扫描单号失败", ExtendedObj = new { hidBill = hidBill, count = count, BoxNo = BoxNo } });
            }
            return Json(new BillReturnResultModel() { IsSuccess = true, ExtendedObj = new { hidBill = hidBill, count = count, BoxNo = BoxNo, model = returnModel } });
            //}
            //return Json(new BillReturnResultModel() { IsSuccess = true, ExtendedObj = new { hidBill = hidBill, count = count, BoxNo = BoxNo } });
        }
        #endregion
        #region 查找已经扫描的运单
        /// <summary>
        /// 获取已经扫描的单号信息
        /// </summary>
        /// <param name="billNos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetScanBillOrBox(string billNosOrBoxNo, bool isBox)
        {
            if (string.IsNullOrEmpty(billNosOrBoxNo))
            { return Json(new BillReturnResultModel() { IsSuccess = false, Message = "未扫描运单" }); }
            List<BillReturnDetailInfoModel> Lists = new List<BillReturnDetailInfoModel>();
            BillReturnBoxInfoModel bModel = new BillReturnBoxInfoModel();
            string Message = string.Empty;
            if (isBox)
            {
                if (_returnBillBoxBLL.GetBoxInfoByBoxNo(billNosOrBoxNo) == null)
                {
                    Message = "不存在该箱号信息";
                    return Json(new BillReturnResultModel() { IsSuccess = false, Message = Message });
                }
                bModel = _returnBillBoxBLL.GetBoxInfoByBoxNo(billNosOrBoxNo);
                Lists = _returnBillDetailInfoBLL.GetListByBoxNo(billNosOrBoxNo);
            }
            else
            {
                Lists = _returnBillDetailInfoBLL.GetListByFormCodes(billNosOrBoxNo);
            }

            return Json(new BillReturnResultModel() { IsSuccess = true, Message = Message, ExtendedObj = new { list = Lists, Weight = bModel.Weight } });
        }
        #endregion

        #region 查询退货出库后的剩余订单
        /// <summary>
        /// 查询退货出库后的剩余订单
        /// </summary>
        /// <param name="billNos"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBillAfterOutBound(string billNos)
        {
            if (string.IsNullOrEmpty(billNos))
            { return Json(new BillReturnResultModel() { IsSuccess = false, Message = "无订单" }); }
            List<BillReturnDetailInfoModel> Lists = new List<BillReturnDetailInfoModel>();
            Lists = _returnBillDetailInfoBLL.GetBillAfterOutBound(billNos,UserContext.CurrentUser.DeptName);
            return Json(new BillReturnResultModel() { IsSuccess = true, ExtendedObj = new { list = Lists } });
        }
        #endregion
        #region 扫描箱号操作
        /// <summary>
        /// 根据箱号查找运单并更新ReturnStatus
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetBillsByBoxNo(string boxNo, string SelectedType, string SelectedStationName)
        {
            List<BillReturnDetailInfoModel> lists = new List<BillReturnDetailInfoModel>();

            if (string.IsNullOrEmpty(boxNo))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "箱号为空，请输入正确的箱号" });
            }

            if (_returnBillBoxBLL.GetBoxInfoByBoxNo(boxNo) == null)
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "不存在该箱号信息", ExtendedObj = new { Lists = lists } });
            }
            BillReturnCountModel countModel = _returnBillDetailInfoBLL.GetReturnToCount(boxNo);

            string Message = "";
            if (SelectedType == "rdoMerchant" && countModel.MerchantCount > 1)
            {
                Message = "该箱中订单是来自多个的商家的，请逐单扫描，谢谢";
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = Message, ExtendedObj = new { Lists = lists } });
            }
            if (string.IsNullOrEmpty(SelectedStationName))
            {
                if (SelectedType == "rdoMerchant")
                {
                    Message = "请选择商家";
                }
                if (SelectedType == "rdoSortingCenter")
                {
                    Message = "请选择上级分拣中心";
                }
                if (SelectedType == "rdoDistribution")
                {
                    Message = "请选择接货配送商";
                }
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = Message, ExtendedObj = new { Lists = lists } });
            }
            lists = _returnBillDetailInfoBLL.ScanBoxNo(boxNo, UserContext.CurrentUser.DeptName,SelectedStationName.Trim());
            return Json(new BillReturnResultModel() { IsSuccess = true, Message = "扫描成功", ExtendedObj = new { list = lists } });
        }
        #endregion
        #region 打印箱号明细
        /// <summary>
        /// 打印箱号明细
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public ActionResult BackPackingPrint(string boxno)
        {
            var modelList = _returnBillDetailInfoBLL.GetDetailListByBoxNo(boxno);
            var Box = _returnBillBoxBLL.GetBoxInfoByBoxNo(boxno);
            Box.CurrentDeptName = UserContext.CurrentUser.DeptName;
            ViewBag.Box = Box;
            return View(modelList);
            //var model = _returnBillBoxBLL.GetBoxInfoByBoxNo(boxno);
            //model.CurrentDeptName = UserContext.CurrentUser.DeptName;
            //return View(model);
        }
        #endregion
        #region 删除已经扫描的运单
        /// <summary>
        /// 删除已经扫描的运单
        /// </summary>
        /// <param name="FormCodelists"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteBill(string FormCodelists, string hidCodeLists)
        {
            //判断是否选择了运单
            if (string.IsNullOrEmpty(FormCodelists))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "未选择运单" });
            }
            var deleteLists = FormCodelists.TrimEnd(',');
            var hidBillNos = "";
            if (_returnBillDetailInfoBLL.Delete(deleteLists) > 0)
            {
                if (!string.IsNullOrEmpty(hidCodeLists))
                {
                    var hidLists = hidCodeLists.Trim().Split(',');
                    var delete = deleteLists.Split(',');
                    foreach (var code in hidLists)
                    {
                        if (!delete.Contains(code)) hidBillNos += code + ',';
                    }
                    hidBillNos = hidBillNos == "" ? "" : hidBillNos.TrimEnd(',');
                }
                return Json(new BillReturnResultModel() { IsSuccess = true, Message = "删除运单成功!", ExtendedObj = new { hidBillNos = hidBillNos } });
            }
            //if (_returnBillDetailInfoBLL.Delete(FormCodelists.TrimEnd(',')) > 0)
            //{
            //    return Json(new BillReturnResultModel() { IsSuccess = true });
            //}
            return Json(new BillReturnResultModel() { IsSuccess = false, Message = "删除运单失败" });
        }
        #endregion
        #region 退货出库按钮操作
        /// <summary>
        /// 退货出库
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public JsonResult ReturnOutBound(string BoxNoOrFormCodes, bool isBox, string hidCodeLists)
        {
            if (string.IsNullOrEmpty(BoxNoOrFormCodes))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "未扫描订单或箱号" });
            }
            string hidBillNos = "";
            if (!string.IsNullOrEmpty(hidCodeLists) && isBox == false)
            {
                BoxNoOrFormCodes = BoxNoOrFormCodes.TrimEnd(',');
                var hidLists = hidCodeLists.Trim().Split(',');
                var delete = BoxNoOrFormCodes.Split(',');
                foreach (var code in hidLists)
                {
                    if (!delete.Contains(code)) hidBillNos += code + ',';
                }
                hidBillNos = hidBillNos == "" ? "" : hidBillNos.TrimEnd(',');
            }
            ResultModel r=_returnBLL.ReturnOutBound(BoxNoOrFormCodes, Enums.ReturnStatus.ReturnInTransit, isBox);
            if (r.IsSuccess)
            {
                return Json(new BillReturnResultModel() { IsSuccess = true, Message = "退货出库完成", ExtendedObj = new { HidBillNos = hidBillNos } });
            }
            return Json(new BillReturnResultModel() { IsSuccess = false, Message = r.Message });

        }
        #endregion
        #region 装箱打印按钮操作
        /// <summary>
        /// 装箱打印
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public JsonResult InBoxPrint(string BoxNo, decimal weight)
        {
            decimal Weight = weight;
            if (string.IsNullOrEmpty(BoxNo))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "未装箱，不能装箱打印" });
            }
            string Message = string.Empty;
            if (_returnBLL.InBoxPrint(BoxNo, Weight, out Message))
            {
                return Json(new BillReturnResultModel() { IsSuccess = true, Message = "装箱打印完成" });
            }
            return Json(new BillReturnResultModel() { IsSuccess = false, Message = Message });

        }
        #endregion
        #endregion

        #region 退货交接表打印
        #region 构建页面
        /// <summary>
        /// 退货交接表打印
        /// </summary>
        /// <returns></returns>
        public ActionResult FormPrint()
        {
            this.SetSearchListAjaxOptions();
            //商家列表
            ViewBag.MerchantList = _merchantBLL.GetMerchantListByDistributionCode(UserContext.CurrentUser.DistributionCode).OrderBy(x => x.Name).ToList();
            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetAllDistributors();
            this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
            SortCenterUserModel userModel = _returnBLL.GetUserModel(UserContext.CurrentUser.ID);
            ViewReturnValidateModel viewModel = new ViewReturnValidateModel(userModel);
            ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));
            ViewBag.hidBillNos = string.Empty;

            return View();
        }
        #endregion
        #region 查询信息列表
        /// <summary>
        /// 退货交接表打印获取值
        /// </summary>
        /// <param name="formColl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult FormPrint(FormCollection formColl)
        {
            var selectType = formColl["sortingCenterSelect"];
            int station = -1;
            string returnTo = string.Empty;
            if (selectType != null)
            {
                var merchant = formColl["merchantSelect"];
                if (merchant != null && merchant != "-1")
                {
                    station = Convert.ToInt32(merchant);
                    var m = _merchantBLL.GetByID(station);
                    returnTo = m == null ? "" : m.Name;
                }
                var SortingCenterList = formColl["SortingCenterList"];
                if (SortingCenterList != null && SortingCenterList != "-1")
                {
                    station = Convert.ToInt32(SortingCenterList);
                    var e = _expressCompanyBLL.Get(station);
                    returnTo = e == null ? "" : e.Name;
                }
                var DistributorList = formColl["DistributorList"];
                if (DistributorList != null && DistributorList != "-1")
                {
                    station = Convert.ToInt32(DistributorList);
                    var e = _expressCompanyBLL.Get(station);
                    returnTo = e == null ? "" : e.Name;
                }
            }
            string BeginTime = formColl["ReturnBeginTime"];
            string EndTime = formColl["ReturnBoundEndTime"];
            string formtype = formColl["FormType"];
            string code = formColl["Number"];
            var list = _returnBillBoxBLL.GetReturnDetailList(returnTo, BeginTime, EndTime, formtype, code, UserContext.CurrentUser.DeptName);
            return PartialView("_PartialFormPrintList", list);
        }
        #endregion

        /// <summary>
        /// 退货交接表打印
        /// </summary>
        /// <returns></returns>
        public JsonResult SearchForPrint(string returnto, string startDate, string endDate, string formType, string code)
        {
            List<BillReturnBoxInfoModel> Lists = new List<BillReturnBoxInfoModel>();
            Lists = _returnBillBoxBLL.GetReturnDetailList(returnto, startDate, endDate, formType, code, UserContext.CurrentUser.DeptName);
            return Json(new BillReturnResultModel() { IsSuccess = true, ExtendedObj = new { Lists = Lists } });
        }
        /// <summary>
        /// 打印箱号明细
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public ActionResult FormPrintDetail(string BoxNo)
        {
            var modelList = _returnBillDetailInfoBLL.GetDetailListByBoxNo(BoxNo);
            var Box = _returnBillBoxBLL.GetBoxInfoByBoxNo(BoxNo);
            Box.CurrentDeptName = UserContext.CurrentUser.DeptName;
            ViewBag.OpUser = UserContext.CurrentUser.UserName;
            ViewBag.Box = Box;
            return View(modelList);
        }
        /// <summary>
        /// 显示箱号明细
        /// </summary>
        /// <param name="batchNo"></param>
        /// <returns></returns>
        public ActionResult ReturnBoxDetail(string BoxNo)
        {
            var modelList = _returnBillDetailInfoBLL.GetDetailListByBoxNo(BoxNo);
            var Box = _returnBillBoxBLL.GetBoxInfoByBoxNo(BoxNo);
            Box.CurrentDeptName = UserContext.CurrentUser.DeptName;
            ViewBag.Box = Box;
            return View(modelList);
        }

        /// <summary>
        /// 退货交接表打印操作
        /// </summary>
        /// <param name="formColl"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BackFormPrint(string boxNo)
        {
            if (string.IsNullOrEmpty(boxNo))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "箱号为空！" });
            }
            string BoxNo = boxNo.Trim();
            if (_returnBillBoxBLL.UpdateBoxIsPrintBackForm(BoxNo))
            {
                return Json(new BillReturnResultModel() { IsSuccess = true, Message = "退货交接表打印成功！" });
            }
            return Json(new BillReturnResultModel() { IsSuccess = false, Message = "退货交接表打印失败！" });
        }
        #endregion

        #region 返货入库相关
        //返货入库
        public ActionResult ReturnBillSorting()
        {
            return View();
        }
        #region 扫描运单或者标签号
        /// <summary>
        /// 返货入库扫描运
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ScanForm(string FormCode, string weight, bool weightValid, int codeType)
        {
            const decimal errorValue = 20;
            ViewBillReturnInfoModel BillReturnInfo = new ViewBillReturnInfoModel() { Code = 0 };
            string Err = "";
            BillReturnEntityModel model = _billReturn.GetModel(FormCode, weight, codeType, out Err);
            if (model == null)
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, ResultAlertType = Models.ResultAlertType.Alert, Message = Err });
            }
            if (weightValid)
            {
                if (WeightCompare(model.Weight, weight, errorValue))
                {
                    BillReturnInfo.Code = 2;
                    return Json(new BillReturnResultModel() { IsSuccess = false, ResultAlertType = Models.ResultAlertType.Confirm, Message = "重量不符，是否入库？", ExtendedObj = new { Model = model } });
                }
            }
            string Message = "";
            BillReturnInbound(model, out Message);
            BillReturnInfo = _billReturn.GetReturnBillInfo(model.FormCode.ToString());
            if (string.IsNullOrEmpty(Message))
            {
                return Json(new BillReturnResultModel() { IsSuccess = true, Message = "", ExtendedObj = new { BillReturnInfo = BillReturnInfo } });
            }
            return Json(new BillReturnResultModel() { IsSuccess = false, ResultAlertType = Models.ResultAlertType.Alert, Message = Message });
        }
        /// <summary>
        /// 退货单入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReturnInbound(BillReturnEntityModel model, int codeType)
        {
            string Message = "";
            if (model == null)
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, ResultAlertType = Models.ResultAlertType.Alert, Message = Message });
            }
            BillReturnInbound(model, out Message);
            if (!string.IsNullOrEmpty(Message))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, ResultAlertType = Models.ResultAlertType.Alert, Message = Message });
            }
            ViewBillReturnInfoModel BillReturnInfo = new ViewBillReturnInfoModel() { Code = 0 };
            BillReturnInfo = _billReturn.GetReturnBillInfo(model.FormCode.ToString());
            return Json(new BillReturnResultModel() { IsSuccess = true, Message = "", ExtendedObj = new { BillReturnInfo = BillReturnInfo } });
        }
        #endregion
        #region 扫描封箱贴
        /// <summary>
        /// 扫描封箱贴
        /// </summary>
        [HttpPost]
        public JsonResult ScanningBoxLabel(string boxLabelNo, string boxNo, bool ValidBox)
        {
            ViewBillReturnInfoModel BillReturnInfo = new ViewBillReturnInfoModel() { Code = 0 };
            var Message = _returnBLL.ValidBox(boxNo);
            if (!string.IsNullOrEmpty(Message))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = Message, ResultAlertType = Models.ResultAlertType.Alert });
            }
            if (_returnBLL.UpdateUsingStatus(false, boxNo))
            {
                return Json(new BillReturnResultModel() { IsSuccess = true, Message = "拆箱成功！", ResultAlertType = Models.ResultAlertType.Alert });
            }
            return Json(new BillReturnResultModel() { IsSuccess = true, Message = "", ExtendedObj = new { BillReturnInfo = BillReturnInfo } });
        }
        #endregion
        #endregion

        #region 商家入库确认

        #region 构建页面
        IMerchantReturnBLL _merchantReturnBLL = ServiceFactory.GetService<IMerchantReturnBLL>();
        /// <summary>
        /// 商家入库确认
        /// </summary>
        /// <returns></returns>
        public ActionResult MerchantReturnBill()
        {
            this.SetSearchListAjaxOptions();
            //商家列表
            ViewBag.MerchantList = _merchantBLL.GetMerchantListByDistributionCode(UserContext.CurrentUser.DistributionCode).OrderBy(x => x.Name).ToList();
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetAllDistributors();
            this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
            SortCenterUserModel userModel = _returnBLL.GetUserModel(UserContext.CurrentUser.ID);
            ViewReturnValidateModel viewModel = new ViewReturnValidateModel(userModel);
            ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));
            List<IDAndNameModel> citys = _expressCompanyBLL.GetCitiesHasAuthority();
            if (citys != null)
            {
                List<string> cityIDList = citys.Select(m => m.ID).ToList<string>();
                ViewBag.Drivers = _employeeService.GetDriverByCityList(cityIDList).Select(m =>
                            new SelectListItem() { Text = m.EmployeeName, Value = m.EmployeeID.ToString() });
            }
            else
            {
                var result = new List<SelectListItem>();
                ViewBag.Drivers = result;
            }

            ViewBag.TruckNos = _truckService.GetAll().Select(m =>
                             new SelectListItem() { Text = m.TruckNO, Value = m.TruckNO + "|" + m.GPSNO });
            ViewBag.ReturnBillStatus = new List<SelectListItem>() { 
            new SelectListItem(){Text="返货在途",Value="-1"},
            new SelectListItem(){Text="返货入库",Value="1"},
            new SelectListItem(){Text="退换货入库",Value="6"},
            new SelectListItem(){Text="拒收入库",Value="7"},
            new SelectListItem(){Text="签单返回入库",Value="13"}
            };
            return View();
        }
        #endregion
        #region 查询数据
        /// <summary>
        /// 商家入库确认
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MerchantReturnBill(FormCollection Form)
        {
            ReturnBillSeachModel SearchModel = new ReturnBillSeachModel();
            SearchModel.BeginTime = Form["ReturnBeginTime"];
            SearchModel.EndTime = Form["ReturnBoundEndTime"];
            SearchModel.MerchantId = Form["merchantSelect"];
            SearchModel.StationId = Form["selCityAndStation_Station"];
            SearchModel.LabelNo = Form["Label"];
            SearchModel.BoxNo = Form["BoxNo"];
            SearchModel.FormCode = Form["FormCode"].Trim(' ').Trim('　');
            if (!string.IsNullOrEmpty(SearchModel.FormCode))
            {
                Int64 formcode = 0;
                if (!Int64.TryParse(SearchModel.FormCode, out formcode))
                {
                    return PartialView("_PartialMerchantReturnBillList", null);
                }
            }
            if (!string.IsNullOrEmpty(SearchModel.FormCode) || !string.IsNullOrEmpty(SearchModel.BoxNo))
            {
                SearchModel.BeginTime = "";
                SearchModel.EndTime = "";
            }
            SearchModel.ReturnStatus = Form["S_ReturnBillStatus"];
            SearchModel.DistributionCode = Form["DistributorList"];
            SearchModel.CurrentDistributionCode = UserContext.CurrentUser.DistributionCode;
            SearchModel.Source = Convert.ToString((int)Enums.BillSource.Others);
            //SearchModel.CityId = Form["selCityAndStation_City"];
            var isHasPrint = Form["hdHasPrint"];

            string FormCodelists = "";
            var list = _merchantReturnBLL.GetMerchantReturnBillList(SearchModel, isHasPrint,UserContext.CurrentUser.DeptName, out FormCodelists);
            var billStatistic = _merchantReturnBLL.GetReturnbillStatistic(FormCodelists);
            ViewBag.statistic = billStatistic;
            return PartialView("_PartialMerchantReturnBillList", list);
        }
        #endregion
        #region  交接确认按钮
        /// <summary>
        /// 交接确认按钮
        /// </summary>
        /// <param name="Codes"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MerchantReturnBillConfirm(string Codes)
        {
            if (string.IsNullOrEmpty(Codes))
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "没有选择运单！" });
            }

            string Message = "";
            _merchantReturnBLL.ReturnbillInBound(Codes, out Message);
            if (Message != "")
            {
                return Json(new BillReturnResultModel() { IsSuccess = false, Message = "交接确认失败！" });
            }
            return Json(new BillReturnResultModel() { IsSuccess = true, Message = "交接确认成功！" });
        }
        #endregion
        #region 打印按钮
        /// <summary>
        /// 商家入库确认打印按钮
        /// </summary>
        /// <param name="Codes"></param>
        /// <returns></returns>
        public ActionResult MerchantReturnBillPrintDetail(string codeLists)
        {
            string Title = UserContext.CurrentUser.DeptName + DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日" +
                    "退货返库交接表";
            ViewBag.Title = Title;
            List<string> MerchantNames = new List<string>();
            List<string> BillStatus = new List<string>();
            var list = _merchantReturnBLL.GetMerchantReturnBillListForPrint(codeLists, out MerchantNames, out BillStatus);
            ViewBag.list = list;
            ViewBag.MerchantNames = MerchantNames;
            ViewBag.BillStatus = BillStatus;
            return View();
        }
        #endregion
        #endregion



        #region 私有方法
        /// <summary>
        /// 验证运单是否有效
        /// </summary>
        /// <param name="selectType"></param>
        /// <param name="FormCode"></param>
        /// <param name="selectStationValue"></param>
        /// <param name="formType"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private string isValid(string selectType, string FormCode, string selectStationValue, int formType, out BillModel model)
        {
            Enums.SortCenterFormType sortCenterFormType = (Enums.SortCenterFormType)formType;
            string formCode = FormCode;
            model = new BillModel();
            if (string.IsNullOrEmpty(formCode))
            {
                return "单号为空！";
            }
            if (sortCenterFormType == Enums.SortCenterFormType.Waybill)
            {
                model = _billBLL.GetBillByFormCode(formCode);
                if (model == null)
                {
                    return "该单不存在,请重新录入！";
                }
                if (_billReturn.IsBillReturning(formCode))
                {
                    return "该单已经操作逆向入库了，请不要重复操作，谢谢！";
                }
            }
            else
            {
                if (selectType != "rdoMerchant")
                {
                    return "扫描的订单号需要选择商家！";
                }
                else
                {
                    var formcodelist = _billBLL.GetFormCodeByCustomerOrder(formCode);

                    if (formcodelist == null)
                    {
                        return "订单号不存在或与所选商家不符，请重新录入！";
                    }
                    model = _billBLL.GetBillByFormCode(formcodelist[0]);
                }
            }
            if (selectType == "rdoMerchant")
            {
                if (selectStationValue == "-1")
                {
                    return "请选择退货商家！";
                }
                if (model.MerchantID != int.Parse(selectStationValue))
                {
                    return "此单与所选退货商家不符，不能退库装箱！";
                }
            }
            if (selectType == "rdoSortingCenter")
            {
                if (selectStationValue == "-1")
                {
                    return "请选择上级分拣中心！";
                }
            }
            if (selectType == "rdoDistribution")
            {
                if (selectStationValue == "-1")
                {
                    return "请选择接货配送商！";
                }
            }
            if (model.ReturnStatus == 0 || model.ReturnStatus == null)
            {
                return "此单处于正向状态，不能退库装箱!";
            }
            return "";
        }
        /// <summary>
        /// 获取箱号
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="expressID"></param>
        /// <returns></returns>
        private string GetBoxNo(string BoxNo, string returnMerchant, string returnto)
        {
            BillReturnBoxInfoModel box = new BillReturnBoxInfoModel();
            var boxNo = BoxNo;
            if (string.IsNullOrEmpty(BoxNo))
            {
                var maxBoxNo = _returnBLL.GetMaxBoxNO(UserContext.CurrentUser.DeptID + DateTime.Now.ToString("yyMMdd"));
                if (string.IsNullOrEmpty(maxBoxNo))
                {
                    boxNo = UserContext.CurrentUser.DeptID + DateTime.Now.ToString("yyMMdd") + "000001";
                }
                else
                {
                    boxNo = maxBoxNo.Substring(0, maxBoxNo.Length - 6) + (long.Parse(maxBoxNo.Substring(maxBoxNo.Length - 6, 6)) + 1).ToString("000000");
                }
                if (!string.IsNullOrEmpty(returnMerchant)) box.ReturnMerchant = returnMerchant.Trim();
                else { box.ReturnMerchant = returnto; }
                box.ReturnTo = returnto;
                box.CreateBy = UserContext.CurrentUser.ID;
                box.BoxNo = boxNo;
                box.CreateDept = UserContext.CurrentUser.DeptName;
                box.UpdateBy = UserContext.CurrentUser.ID;
                box.CurrentDistributionCode = UserContext.CurrentUser.DistributionCode;
                box.CreateDeptID = UserContext.CurrentUser.DeptID;
                //box.BoxStatus = (Int16)Enums.ReturnStatus.ReturnInbounded;
                _returnBillBoxBLL.AddBox(box);
            }
            return boxNo;

        }
        //加密值为了和页面上的值进行匹配
        private void EncryptValue(SortingPackingBoxModel boxModel)
        {
            if (boxModel == null)
            {
                return;
            }
            switch (boxModel.InboundType)
            {
                case Enums.SortCenterOperateType.SimpleSorting:
                    //站点
                    break;
                case Enums.SortCenterOperateType.SecondSorting:
                    //分拣中心
                    boxModel.EncryptValue = DES.Encrypt3DES(String.Format("{0};{1};{2}", boxModel.ArrivalID, (int)boxModel.ArrivalCompanyFlag, boxModel.ArrivalDistributionCode));
                    break;
                case Enums.SortCenterOperateType.DistributionSorting:
                    //配送商
                    boxModel.EncryptValue = DES.Encrypt3DES(String.Format("{0};{1};{2}", boxModel.ArrivalID, (int)boxModel.ArrivalCompanyFlag, boxModel.ArrivalDistributionCode));
                    break;
            }
        }

        /// <summary>
        /// 获取日志模板
        /// </summary>
        /// <returns></returns>
        private OperateLogEntityModel GetOpModel()
        {
            var model = new OperateLogEntityModel();
            model.LogType = 0;
            model.OperateTime = DateTime.Now;
            model.Operation = "外单退库";
            model.LogOperator = UserContext.CurrentUser.UserName;
            model.OperatorStation = UserContext.CurrentUser.DeptID;
            model.Result = "您的订单已经成功退回至商家库房";
            return model;
        }
        /// <summary>
        /// 是否为退库单
        /// </summary>
        /// <param name="wbObj"></param>
        /// <returns></returns>
        private static bool IsReturnbill(BillModel billObj)
        {
            if (billObj.Status == Enums.BillStatus.Rejected ||
                billObj.Status == Enums.BillStatus.DeliverySuccess)
            {
                //to do
                if (billObj.Status == Enums.BillStatus.Rejected)
                {
                    return billObj.Source == Enums.BillSource.VANCL ||
                           billObj.Source == Enums.BillSource.Others;
                }
                else
                {
                    return billObj.BillType == Enums.BillType.Exchange ||
                           billObj.BillType == Enums.BillType.Return;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 返货入库
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Message"></param>
        private void BillReturnInbound(BillReturnEntityModel model, out string Message)
        {
            Message = "";
            _billReturn.BillReturnInbound(model, out Message);
        }
        /// <summary>
        /// 验证重量
        /// </summary>
        /// <param name="nullable"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        private static bool WeightCompare(decimal? waybillWeight, string weight, decimal errorValue)
        {
            try
            {
                return waybillWeight * 1000 - errorValue < (decimal.Parse(weight) * 1000) || (decimal.Parse(weight) * 1000) < waybillWeight * 1000 + errorValue;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
