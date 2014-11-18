using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Util.Security;
using Vancl.TMS.Web.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Util.JsonUtil;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class InboundController : Controller
    {
        IExpressCompanyBLL _expressCompanyBLL = ServiceFactory.GetService<IExpressCompanyBLL>("ExpressCompanyBLL");
        IInboundBLL _inboundBLL = ServiceFactory.GetService<IInboundBLL>("SC_InboundBLL");
        IBillBLL _billBLL = ServiceFactory.GetService<IBillBLL>("BillBLL");
        //
        // GET: /Sorting/Inbound/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 入库【不限制站点】
        /// </summary>
        /// <returns></returns>
        public ActionResult Inbound_NoLimitedStation()
        {
            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            expressList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
            distributorList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
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
        /// 扫描入库【不限制站点】
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="selectedType"></param>
        /// <param name="selectStationValue"></param>
        /// <param name="code"></param>
        /// <param name="formType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScanFormCode_NoLimitedStation(string hidData, int selectedType, string selectStationValue, string code, int formType)
        {
            var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
            if (!InboundData.IsValidate)
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = InboundData.ValidateMsg }, JsonRequestBehavior.AllowGet);
            }
            var CorrectType = new int[] { 0, 2, 3 };
            if (!CorrectType.Contains(selectedType))
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "请选择站点类型!" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "请选择站点!" }, JsonRequestBehavior.AllowGet);
            }
            string[] arrDecryptValue = null;
            if (selectedType != (int)Enums.SortCenterOperateType.SimpleSorting)
            {
                arrDecryptValue = DES.Decrypt3DES(selectStationValue).Split(';');
                if (arrDecryptValue == null || arrDecryptValue.Length != 3)
                {
                    return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "数据解密错误!" }, JsonRequestBehavior.AllowGet);
                }
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
            argument.FormType = (Enums.SortCenterFormType)formType;
            argument.FormCode = code.Trim();
            return Json(_inboundBLL.SimpleInbound_NoLimitedStation(argument), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 转站入库
        /// </summary>
        /// <returns></returns>
        public ActionResult TurnInbound()
        {
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
        /// 转站入库
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="selectStationValue"></param>
        /// <param name="code"></param>
        /// <param name="formType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScanFormCode_TurnInbound(string hidData, string selectStationValue, string code, int formType)
        {
            var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
            if (!InboundData.IsValidate)
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = InboundData.ValidateMsg }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "请选择站点!" }, JsonRequestBehavior.AllowGet);
            }
            //进行入库操作
            var argument = new InboundSimpleArgModel() { OpUser = InboundData.OpUser,  PreCondition = InboundData.PreCondition };
            var ToStationModel = new SortCenterToStationModel();

            ToStationModel.ExpressCompanyID = int.Parse(selectStationValue);
            ToStationModel.DistributionCode = InboundData.OpUser.DistributionCode;
            ToStationModel.CompanyFlag = Enums.CompanyFlag.DistributionStation;

            argument.ToStation = ToStationModel;
            argument.FormType = (Enums.SortCenterFormType)formType;
            argument.FormCode = code.Trim();
            return Json(_inboundBLL.TurnInbound(argument), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批量入库
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchInbound()
        {
            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            expressList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
            distributorList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
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
        /// 入库分拣
        /// </summary>
        /// <returns></returns>
        public ActionResult Inbound()
        {
            //分拣中心列表
            List<ExpressCompanyModel> expressList = _expressCompanyBLL.GetSortingCentersByDistributionCodeWithoutSelf();
            expressList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
            this.SendSelectListToView<ExpressCompanyModel>(expressList, "SortingCenterList", true);
            //配送商列表
            List<ExpressCompanyModel> distributorList = _expressCompanyBLL.GetRelatedDistributor(UserContext.CurrentUser.DistributionCode);
            distributorList.ForEach(l =>
            {
                l.ID = DES.Encrypt3DES(String.Format("{0};{1};{2}", l.ID, (int)l.CompanyFlag, l.DistributionCode));
            });
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
        /// 批量入库
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="selectedType"></param>
        /// <param name="selectStationValue"></param>
        /// <param name="code"></param>
        /// <param name="formType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult BatchScanFormCode(string hidData, int selectedType, string selectStationValue, string code, int formType)
        {
            if (String.IsNullOrWhiteSpace(code))
            {
                return Json(new ViewInboundBatchModel() { IsSuccess = false, Message = "请输入需要入库的单号!" }, JsonRequestBehavior.AllowGet);
            }
            var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
            if (!InboundData.IsValidate)
            {
                return Json(new ViewInboundBatchModel() { IsSuccess = false, Message = InboundData.ValidateMsg }, JsonRequestBehavior.AllowGet);
            }
            var CorrectType = new int[] { 0, 2, 3 };
            if (!CorrectType.Contains(selectedType))
            {
                return Json(new ViewInboundBatchModel() { IsSuccess = false, Message = "请选择站点类型!" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
            {
                return Json(new ViewInboundBatchModel() { IsSuccess = false, Message = "请选择站点!" }, JsonRequestBehavior.AllowGet);
            }
            string[] arrDecryptValue = null;
            if (selectedType != (int)Enums.SortCenterOperateType.SimpleSorting)
            {
                arrDecryptValue = DES.Decrypt3DES(selectStationValue).Split(';');
                if (arrDecryptValue == null || arrDecryptValue.Length != 3)
                {
                    return Json(new ViewInboundBatchModel() { IsSuccess = false, Message = "数据解密错误!" }, JsonRequestBehavior.AllowGet);
                }
            }
            //进行批量入库操作
            var argument = new InboundBatchArgModel() { OpUser = InboundData.OpUser, PreCondition = InboundData.PreCondition };
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
            argument.FormType = (Enums.SortCenterFormType)formType;
            argument.ArrFormCode = code.Split(';');
            return Json(_inboundBLL.BatchInbound(argument), JsonRequestBehavior.AllowGet);


        }



        /// <summary>
        /// 扫描入库
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="selectedType"></param>
        /// <param name="selectStationValue"></param>
        /// <param name="code"></param>
        /// <param name="formType"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ScanFormCode(string hidData, int selectedType, string selectStationValue, string code, int formType)
        {
            var InboundData = JsonHelper.ConvertToObject<ViewInboundValidateModel>(DES.Decrypt3DES(hidData));
            if (!InboundData.IsValidate)
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = InboundData.ValidateMsg }, JsonRequestBehavior.AllowGet);
            }
            var CorrectType = new int[] { 0, 2, 3 };
            if (!CorrectType.Contains(selectedType))
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "请选择站点类型!" }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(selectStationValue) || selectStationValue == "-1")
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "请选择站点!" }, JsonRequestBehavior.AllowGet);
            }
            string[] arrDecryptValue = null;
            if (selectedType != (int)Enums.SortCenterOperateType.SimpleSorting)
            {
                arrDecryptValue = DES.Decrypt3DES(selectStationValue).Split(';');
                if (arrDecryptValue == null || arrDecryptValue.Length != 3)
                {
                    return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "数据解密错误!" }, JsonRequestBehavior.AllowGet);
                }
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
            argument.FormType = (Enums.SortCenterFormType)formType;
            argument.FormCode = code.Trim();
            return Json(_inboundBLL.SimpleInbound(argument), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取入库数量
        /// </summary>
        /// <param name="hidData"></param>
        /// <param name="selectedType"></param>
        /// <param name="selectStationValue"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCount(string hidData, int selectedType, string selectStationValue)
        {
            var CorrectType = new int[] { 0, 2, 3 };
            if (!CorrectType.Contains(selectedType))
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "类型选择错误" }, JsonRequestBehavior.AllowGet);
            }
            if (String.IsNullOrEmpty(selectStationValue))
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "站点选择错误" }, JsonRequestBehavior.AllowGet);
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
                return Json(new ViewInboundSimpleModel() { IsSuccess = true, InboundCount = _inboundBLL.GetInboundCount(argument) }, JsonRequestBehavior.AllowGet);
            }
            string[] arrDecryptValue = DES.Decrypt3DES(selectStationValue).Split(';');
            if (arrDecryptValue == null || arrDecryptValue.Length != 3)
            {
                return Json(new ViewInboundSimpleModel() { IsSuccess = false, Message = "数据解密错误" }, JsonRequestBehavior.AllowGet);
            }
            ToStationModel.ExpressCompanyID = int.Parse(arrDecryptValue[0]);
            ToStationModel.CompanyFlag = (Enums.CompanyFlag)int.Parse(arrDecryptValue[1]);
            ToStationModel.DistributionCode = arrDecryptValue[2];
            argument.ToStation = ToStationModel;
            return Json(new ViewInboundSimpleModel() { IsSuccess = true, InboundCount = _inboundBLL.GetInboundCount(argument) } , JsonRequestBehavior.AllowGet);
        }

    }
}
