using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vancl.TMS.BLL.CustomizeFlow;
using Vancl.TMS.BLL.Sorting.Outbound;
using Vancl.TMS.Model.CustomizeFlow;
using Vancl.TMS.Model.CustomizeFlow.Parameter;
using Vancl.TMS.Util.EnumUtil;
using Vancl.TMS.Web.Common;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Util.Security;
using System.Linq;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Util.JsonUtil;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Web.Areas.Sorting.Models;
using Vancl.TMS.Model.Sorting.Inbound.Packing;
using Vancl.TMS.Core.FormulaManager;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class SortingPackingController : Controller
    {
        ISortingPackingBLL _sortingPackingBLL = ServiceFactory.GetService<ISortingPackingBLL>("SortingPackingBLL");
        IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
        IFormula<String, InboundPackingNoContextModel> _packingNoGenerator = FormulasFactory.GetFormula<IFormula<String, InboundPackingNoContextModel>>("PackingNoGenerateFormula");
        IInboundBLL _inboundBLL = ServiceFactory.GetService<IInboundBLL>("SC_InboundBLL");
        IBillBLL _billBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
        IDistributionBLL _distributionBLL = ServiceFactory.GetService<IDistributionBLL>("DistributionBLL");
	    private FlowFunFacade TMSFlowFunFacade = new FlowFunFacade();

        #region V1


        public ActionResult Index()
        {
            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            expressList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
			//List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetAllDistributors();
			//distributorList.ForEach(l =>
			//{
			//    l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
			//});
			List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetDistributionCooprelation(UserContext.CurrentUser.DistributionCode);
            this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);

            SortCenterUserModel userModel = _inboundBLL.GetUserModel(UserContext.CurrentUser.ID);
            InboundPreConditionModel preCondition = null;
            if (userModel != null)
            {
                preCondition = _inboundBLL.GetPreCondition(UserContext.CurrentUser.DistributionCode);
            }
            ViewInboundValidateModel viewModel = new ViewInboundValidateModel(userModel, preCondition);
            ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));

            return View();
        }

        /// <summary>
        /// 获取入库数量
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="selectedType"></param>
        /// <param name="selectStationValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCount(string hidData, int selectedType, string selectStationValue)
        {
            var CorrectType = new int[] { 0, 2, 3 };
            if (!CorrectType.Contains(selectedType))
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, ExtendedObj = new { Count = 0 }, Message = "类型选择错误" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(selectStationValue))
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, ExtendedObj = new { Count = 0 }, Message = "站点选择错误" }, JsonRequestBehavior.AllowGet);
            }
            var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
            var argument = new InboundSimpleArgModel() { OpUser = InboundData.OpUser };
            var ToStationModel = new SortCenterToStationModel();
            if (0 == selectedType)    //站点
            {
                ToStationModel.ExpressCompanyID = int.Parse(selectStationValue);
                ToStationModel.DistributionCode = InboundData.OpUser.DistributionCode;
                ToStationModel.CompanyFlag = Enums.CompanyFlag.DistributionStation;
                argument.ToStation = ToStationModel;
                return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { Count = _inboundBLL.GetInboundCount(argument) } }, JsonRequestBehavior.AllowGet);
            }
            string[] arrDecryptValue = DES.Decrypt3DES(selectStationValue).Split(';');
            if (arrDecryptValue == null || arrDecryptValue.Length != 3)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, ExtendedObj = new { Count = 0 }, Message = "数据解密错误" }, JsonRequestBehavior.AllowGet);
            }
            ToStationModel.ExpressCompanyID = int.Parse(arrDecryptValue[0]);
            ToStationModel.CompanyFlag = (Enums.CompanyFlag)int.Parse(arrDecryptValue[1]);
            ToStationModel.DistributionCode = arrDecryptValue[2];
            argument.ToStation = ToStationModel;
            return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { Count = _inboundBLL.GetInboundCount(argument) } }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 扫描箱号
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScanBoxNo(string hidData, string boxNo)
        {
            List<SortingPackingBillModel> list = _sortingPackingBLL.GetPackingBillsByBoxNo(boxNo);
            string message = string.Empty;
            if (!CheckBoxValid(hidData, list, out message))
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = message });
            }
            SortingPackingBoxModel boxModel = _sortingPackingBLL.GetPackingBox(boxNo);
            if (boxModel == null)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该箱子不存在或已经被删除!" });
            }
            EncryptValue(boxModel);
            return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { List = list, Box = boxModel } });
        }

        /// <summary>
        /// 扫描单号
        /// </summary>
        /// <param name="hidData">隐藏加密数据</param>
        /// <param name="selectedType">操作类型[单号，箱号]</param>
        /// <param name="selectStationValue">目的地站点</param>
        /// <param name="code">编号</param>
        /// <param name="boxNo">箱号</param>
        /// <param name="isFirst">是否扫描第一单</param>
        /// <param name="formType">单号类型</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScanFormCode(String hidData, int selectedType
                                        , String selectStationValue, String code
                                        , String boxNo, bool isFirst, int formType)
        {
            var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
            if (!InboundData.IsValidate)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = InboundData.ValidateMsg });
            }
            Enums.SortCenterFormType sortCenterFormType = (Enums.SortCenterFormType)formType;
            String formCode = code;
            if (sortCenterFormType != Enums.SortCenterFormType.Waybill)
            {
                var lstMerchantFormCode = _billBLL.GetMerchantFormCodeRelation(sortCenterFormType, formCode);
                if (lstMerchantFormCode == null)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该单不存在!" });
                }
                if (lstMerchantFormCode.Count > 1)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该单号对应多个运单!" });
                }
                formCode = lstMerchantFormCode[0].FormCode;
            }
            bool isAlreadyPacked = _sortingPackingBLL.IsBillAlreadyPacked(formCode, InboundData.OpUser.ExpressId.Value);
            if (isFirst)
            {
                //判断是否已经装箱则显示补打印
                if (isAlreadyPacked)
                {
                    List<SortingPackingBillModel> list = _sortingPackingBLL.GetPackingBillsByFormCode(formCode);
                    string message = string.Empty;
                    if (!CheckBoxValid(hidData, list, out message))
                    {
                        return Json(new SortingPackingResultModel() { IsSuccess = false, Message = message });
                    }
                    SortingPackingBoxModel boxModel = _sortingPackingBLL.GetPackingBox(list[0].BoxNo);
                    if (boxModel == null)
                    {
                        return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该箱子不存在或已经被删除!" });
                    }
                    EncryptValue(boxModel);
                    return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { List = list, Box = boxModel, IsReprint = true } });
                }
            }
            else
            {
                if (isAlreadyPacked)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该运单已经装箱!" });
                }
            }
            var CorrectType = new int[] { 0, 2, 3 };
            if (!CorrectType.Contains(selectedType))
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "请选择站点类型!" });
            }
            if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "请选择站点!" });
            }
            SortingPackingBillModel billModel = _sortingPackingBLL.GetSortingPackingBill(formCode);
            if (billModel == null)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该运单不存在!" });
            }
            if (billModel.Status != Enums.BillStatus.WaitingInbound
                && billModel.Status != Enums.BillStatus.HaveBeenSorting)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该运单状态不符合!" });
            }
            string[] arrDecryptValue = null;
            if (selectedType != (int)Enums.SortCenterOperateType.SimpleSorting)
            {
                arrDecryptValue = DES.Decrypt3DES(selectStationValue).Split(';');
                if (arrDecryptValue == null || arrDecryptValue.Length != 3)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "数据解密错误!" });
                }
            }
            //已分拣状态或者正在入库状态，不处理等待提交装箱操作
            if (billModel.IsInbounding || billModel.Status == Enums.BillStatus.HaveBeenSorting)
            {
                if (billModel.DepartureID != InboundData.OpUser.ExpressId)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该运单不属于当前分拣中心!" });
                }
                if (selectedType == (int)Enums.SortCenterOperateType.SimpleSorting)
                {
                    if (billModel.ArrivalID != Convert.ToInt32(selectStationValue))
                    {
                        return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "分拣站点选择错误!" });
                    }
                }
                else if (selectedType == (int)Enums.SortCenterOperateType.DistributionSorting)
                {
                    if (billModel.ArrivalDistributionCode != arrDecryptValue[2])
                    {
                        return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "配送商选择错误!" });
                    }
                }
                return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { Bill = billModel, Count = -1, BoxNo = GetBoxNo(boxNo, InboundData.OpUser.ExpressId.Value), IsReprint = false } });
            }
            //进行入库操作
            var argument = new InboundSimpleArgModel() { OpUser = InboundData.OpUser, LimitedInboundCount = 150, PreCondition = InboundData.PreCondition };
            var ToStationModel = new SortCenterToStationModel();
            if (selectedType == (int)Enums.SortCenterOperateType.SimpleSorting)    //站点
            {
                ToStationModel.ExpressCompanyID = int.Parse(selectStationValue);
                ToStationModel.DistributionCode = InboundData.OpUser.DistributionCode;
                ToStationModel.CompanyFlag = Enums.CompanyFlag.DistributionStation;
            }
            else
            {
                ToStationModel.ExpressCompanyID = int.Parse(arrDecryptValue[0]);
                ToStationModel.CompanyFlag = (Enums.CompanyFlag)int.Parse(arrDecryptValue[1]);
                ToStationModel.DistributionCode = arrDecryptValue[2];
            }
            argument.ToStation = ToStationModel;
            argument.FormType = Enums.SortCenterFormType.Waybill;
            argument.FormCode = formCode;
            var Result = _inboundBLL.SimpleInbound(argument);
            if (Result.IsSuccess)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { Bill = billModel, Count = Result.InboundCount, BoxNo = GetBoxNo(boxNo, InboundData.OpUser.ExpressId.Value), IsReprint = false } });
            }
            return Json(new SortingPackingResultModel() { IsSuccess = false, Message = Result.Message });
        }

        /// <summary>
        /// 显示包装箱打印相关信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public ActionResult ShowPackingPrintInfo(String boxNo)
        {
            if (String.IsNullOrWhiteSpace(boxNo)) throw new ArgumentNullException("ShowPackingPrintInfo.boxNo is null or empty.");
            return View("PackingPrint", _sortingPackingBLL.GetPackingPrintModel(boxNo));
        }

        /// <summary>
        /// 装箱
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="boxNo"></param>
        /// <param name="weight"></param>
        /// <param name="formCodes"></param>
        /// <param name="selectStationValue"></param>
        /// <param name="selectedType"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Packing(string hidData, string boxNo, decimal weight
                                    , string formCodes, string selectStationValue, int selectedType
                                    , bool isUpdate)
        {
            try
            {
                if (String.IsNullOrEmpty(selectStationValue))
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "站点选择错误" });
                }
                var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
                if (!InboundData.IsValidate)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = InboundData.ValidateMsg });
                }
                int arrivalID = 0;
                if (selectedType == (int)Enums.SortCenterOperateType.SimpleSorting)
                {
                    arrivalID = Convert.ToInt32(selectStationValue);
                }
                else
                {
                    string[] arrDecryptValue = DES.Decrypt3DES(selectStationValue).Split(';');
                    if (arrDecryptValue == null || arrDecryptValue.Length != 3)
                    {
                        return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "数据解密错误!" });
                    }
                    arrivalID = Convert.ToInt32(arrDecryptValue[0]);
                }
                List<string> lstFormCode = new List<string>();
                if (!string.IsNullOrEmpty(formCodes))
                {
                    formCodes.Split(',').ToList().ForEach(s => lstFormCode.Add(s));
                }
                //去重
                lstFormCode = lstFormCode.Distinct().ToList();
                if (!isUpdate && lstFormCode.Count == 0)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "没有要称重的运单!" });
                }
                //装箱操作
                var result = _sortingPackingBLL.AddInboundBox(InboundData, boxNo, weight, lstFormCode, arrivalID, isUpdate);
                if (!result.IsSuccess)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = result.Message });
                }
                else
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = true, Message = result.Message, ExtendedObj = new { PrintData = GetPrintModel(boxNo) } });
                }
            }
            catch (Exception ex)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 面单补打印
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BoxRePrint(string boxNo)
        {
            return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { PrintData = GetPrintModel(boxNo) } });
        }

        private string GetPrintModel(string boxNo)
        {
            //TODO: 取得打印数据
            return "OK";
        }

        private string GetBoxNo(string boxNo, int expressID)
        {
            if (string.IsNullOrEmpty(boxNo))
            {
                return _packingNoGenerator.Execute(new InboundPackingNoContextModel() { SortingCenterID = expressID, FillerCharacter = "0", NumberLength = 6 });
            }
            return boxNo;
        }

        private bool CheckBoxValid(string hidData, List<SortingPackingBillModel> list, out string message)
        {
            message = string.Empty;
            if (list == null || list.Count == 0)
            {
                message = "该箱号不存在!";
                return false;
            }
            var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
            if (!InboundData.IsValidate)       //初始化不成功
            {
                message = InboundData.ValidateMsg;
                return false;
            }
            if (list[0].DepartureID != InboundData.OpUser.ExpressId)
            {
                message = "该箱不属于当前分拣中心!";
                return false;
            }
            return true;
        }


        private bool CheckBoxValid(ViewInboundValidateModel inboundData, List<SortingPackingBillModel> list, out string message)
        {
            message = string.Empty;
            if (list == null || list.Count == 0)
            {
                message = "该箱号不存在!";
                return false;
            }

            if (list[0].DepartureID != inboundData.OpUser.ExpressId)
            {
                message = "该箱不属于当前分拣中心!";
                return false;
            }
            return true;
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

        #endregion


        #region V2

	    public ActionResult IndexV2()
	    {
		    string distributionCode = UserContext.CurrentUser.DistributionCode;
			ViewBag.FunPackingStation = TMSFlowFunFacade.IsExitsCurrFun(distributionCode, FunCode.FunPackingStation);
			ViewBag.FunPackingDeliverCenter = TMSFlowFunFacade.IsExitsCurrFun(distributionCode,
		                                                                       FunCode.FunPackingDeliverCenter);
			ViewBag.FunPackingCompany = TMSFlowFunFacade.IsExitsCurrFun(distributionCode, FunCode.FunPackingCompany);
		    
			    //分拣中心列表
			    List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
			    expressList.ForEach(l =>
				    {
					    l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int) l.CompanyFlag, l.DistributionCode));
				    });
			    this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
		    
			    //配送商列表
				//List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetAllDistributors();
				//distributorList.ForEach(l =>
				//    {
				//        l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int) l.CompanyFlag, l.DistributionCode));
				//    });
				List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetDistributionCooprelation(UserContext.CurrentUser.DistributionCode);
			    this.SendSelectListToView<ExpressCompanyModel>(distributorList, "DistributorList", true);
		    
		    SortCenterUserModel userModel = _inboundBLL.GetUserModel(UserContext.CurrentUser.ID);
            InboundPreConditionModel preCondition = null;
            if (userModel != null)
            {
                preCondition = _inboundBLL.GetPreCondition(UserContext.CurrentUser.DistributionCode);
            }
            ViewInboundValidateModel viewModel = new ViewInboundValidateModel(userModel, preCondition);
            ViewBag.HiddenValue = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));

            return View();
        }


        /// <summary>
        /// 扫描单号
        /// </summary>
        /// <param name="hidData">隐藏加密数据</param>
        /// <param name="codeType">单号类型</param>
        /// <param name="code">单号</param>
        /// <param name="boxNo">箱号</param>
        /// <param name="weight">重量</param>
        /// <param name="isFirst">是否是第一单</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScanFormCodeV2(String hidData, int codeType, String code, String boxNo, decimal weight, bool isFirst, int selectedType, String selectStationValue)
        {
            var inboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
            if (!inboundData.IsValidate)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = inboundData.ValidateMsg });
            }

            string formCode = code;
            SortingPackingBillModel billModel = _sortingPackingBLL.GetSortingPackingBill(formCode);
            if (!_sortingPackingBLL.Validate(formCode, inboundData.OpUser.ExpressId.Value, isFirst))
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = _sortingPackingBLL.ValidateMsg });
            }

            if (_sortingPackingBLL.ValidateMsg == "第一单已装箱")
            {
                List<SortingPackingBillModel> list = _sortingPackingBLL.GetPackingBillsByFormCode(formCode);
                string message = string.Empty;
                if (!CheckBoxValid(inboundData, list, out message))
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = message });
                }
                SortingPackingBoxModel boxModel = _sortingPackingBLL.GetPackingBox(list[0].BoxNo);
                if (boxModel == null)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该箱子不存在或已经被删除!" });
                }
                EncryptValueV2(boxModel);
                return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { List = list, Box = boxModel, IsReprint = true } });
            }

            var vBoxTo = new ViewPackingBoxToModel(selectedType, selectStationValue);
            if (!vBoxTo.Validate())
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = vBoxTo.ValidateMsg });
            }

            string validateMsg;
            //if (!Validate(billModel, vBoxTo, out validateMsg))
            if (!ValidateV2(billModel, vBoxTo, inboundData.OpUser.ExpressId.Value, out validateMsg))
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = validateMsg });
            }

            if (isFirst)
            {
                boxNo = GetBoxNo("", inboundData.OpUser.ExpressId.Value);
            }

            var result = _sortingPackingBLL.AddInboundBoxV2(inboundData, boxNo, weight, code, isFirst, vBoxTo);

            if (!result.IsSuccess)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = result.Message });
            }
            return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { Bill = billModel, Count = -1, BoxNo = boxNo, IsReprint = false } });
        }

        private bool Validate(SortingPackingBillModel billModel, ViewPackingBoxToModel vBoxTo, out string validateMsg)
        {
            if (vBoxTo.CompanyFlag == Enums.CompanyFlag.DistributionStation)
            {
                if (billModel.ArrivalID != vBoxTo.ArrivalId)
                {
                    validateMsg = "此运单不属于当前配送站";
                    return false;
                }
            }

            if (vBoxTo.CompanyFlag == Enums.CompanyFlag.Distributor)
            {
                if (billModel.ArrivalDistributionCode != vBoxTo.DistributionCode)
                {
                    validateMsg = "此运单不属于当前配送商";
                    return false;
                }
			}
			if (vBoxTo.CompanyFlag == Enums.CompanyFlag.SortingCenter)
            {
                var scs = _expressCompanyBLL.GetSortingCenterByStation(billModel.ArrivalID);

                if (!scs.Contains(vBoxTo.ArrivalId))
                {
                    validateMsg = "该运单不属于当前分拣中心!";
                    return false;
                }
            }

            validateMsg = "";
            return true;
        }

		/// <summary>
		/// 流程自定义
		/// </summary>
		/// <param name="billModel"></param>
		/// <param name="vBoxTo"></param>
		/// <param name="expressCompanyId"></param>
		/// <param name="validateMsg"></param>
		/// <returns></returns>
        private bool ValidateV2(SortingPackingBillModel billModel, ViewPackingBoxToModel vBoxTo, int expressCompanyId, out string validateMsg)
	    {
			var checkParameters = new CheckerParameter();
			checkParameters.WaybillNo = Convert.ToInt64(billModel.FormCode);
			checkParameters.FromExpressCompanyId = Convert.ToInt32(expressCompanyId);
			checkParameters.ToExpressCompanyId = Convert.ToInt32(vBoxTo.ArrivalId);
			var flowCheckResult = new CheckerResult();
			if (vBoxTo.CompanyFlag == Enums.CompanyFlag.DistributionStation)
			{
				flowCheckResult = TMSFlowFunFacade.Check<CheckerParameter>(checkParameters, FunCode.FunPackingStation);
			}
			if (vBoxTo.CompanyFlag == Enums.CompanyFlag.SortingCenter)
			{
				flowCheckResult = TMSFlowFunFacade.Check<CheckerParameter>(checkParameters, FunCode.FunPackingDeliverCenter);
			}
			if (vBoxTo.CompanyFlag == Enums.CompanyFlag.Distributor)
			{
				flowCheckResult = TMSFlowFunFacade.Check<CheckerParameter>(checkParameters, FunCode.FunPackingCompany);
			}
			if (!flowCheckResult.Result)
			{
				validateMsg = flowCheckResult.Message;
				return false;
			}
		    validateMsg = "";
		    return true;
	    }

	    [HttpPost]
        public ActionResult PackingV2(string hidData, string boxNo, decimal weight)
        {
            try
            {
                if (weight <= 0)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "重量不能小于0" });
                }
                var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
                if (!InboundData.IsValidate)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = InboundData.ValidateMsg });
                }

                //装箱操作
                var ok = _sortingPackingBLL.UpdateBoxWeightWhenPrint(boxNo, weight, InboundData.OpUser.UserId);
                if (!ok)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "打印时更新重量失败" });
                }
                else
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = true, Message = "OK", ExtendedObj = new { PrintData = GetPrintModel(boxNo) } });
                }
            }
            catch (Exception ex)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = ex.Message });
            }
        }


        /// <summary>
        /// 扫描箱号
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ScanBoxNoV2(string hidData, string boxNo)
        {
            List<SortingPackingBillModel> list = _sortingPackingBLL.GetPackingBillsByBoxNo(boxNo);
            string message = string.Empty;
            if (!CheckBoxValid(hidData, list, out message))
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = message });
            }
            SortingPackingBoxModel boxModel = _sortingPackingBLL.GetPackingBox(boxNo);
            if (boxModel == null)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "该箱子不存在或已经被删除!" });
            }
            EncryptValueV2(boxModel);
            return Json(new SortingPackingResultModel() { IsSuccess = true, ExtendedObj = new { List = list, Box = boxModel } });
        }

        [HttpPost]
        public ActionResult UnLoadBox(string hidData, string boxNo, string formCodes)
        {
            try
            {
                var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
                if (!InboundData.IsValidate)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = InboundData.ValidateMsg });
                }

                if (String.IsNullOrEmpty(formCodes))
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "运单号为空" });
                }

                if (String.IsNullOrEmpty(boxNo))
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = "箱号为空" });
                }

                List<string> lcode = formCodes.Split(',').ToList();
                var result = _sortingPackingBLL.UnLoadBox(InboundData, boxNo, lcode);

                if (!result.IsSuccess)
                {
                    return Json(new SortingPackingResultModel() { IsSuccess = false, Message = result.Message });
                }

                return Json(new SortingPackingResultModel() { IsSuccess = true, Message = result.Message, ExtendedObj = new { PrintData = GetPrintModel(boxNo) } });
            }
            catch (Exception ex)
            {
                return Json(new SortingPackingResultModel() { IsSuccess = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 显示包装箱打印相关信息
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        public ActionResult ShowPackingPrintInfoV2(String boxNo)
        {
            if (String.IsNullOrWhiteSpace(boxNo)) throw new ArgumentNullException("ShowPackingPrintInfo.boxNo is null or empty.");
            return View("PackingPrintV2", _sortingPackingBLL.GetPackingPrintModelV2(boxNo));
        }


        //加密值为了和页面上的值进行匹配
        private void EncryptValueV2(SortingPackingBoxModel boxModel)
        {
            if (boxModel == null)
            {
                return;
            }
            switch (boxModel.ArrivalCompanyFlag)
            {
                case Enums.CompanyFlag.DistributionStation:
                    //站点
                    break;
                case Enums.CompanyFlag.SortingCenter:
                    //分拣中心
                    boxModel.EncryptValue = DES.Encrypt3DES(String.Format("{0};{1};{2}", boxModel.ArrivalID, (int)boxModel.ArrivalCompanyFlag, boxModel.ArrivalDistributionCode));
                    break;
                case Enums.CompanyFlag.Distributor:
                    //配送商
                    boxModel.EncryptValue = DES.Encrypt3DES(String.Format("{0};{1};{2}", boxModel.ArrivalID, (int)boxModel.ArrivalCompanyFlag, boxModel.ArrivalDistributionCode));
                    break;
            }
		}

		#endregion
	}
}
