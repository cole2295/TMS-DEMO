using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.IBLL.Sorting.BillPrint;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Web.Areas.Sorting.Models;

namespace Vancl.TMS.Web.Areas.Sorting.Controllers
{
    public class InboundV2Controller : Controller
    {
        //
        // GET: /Sorting/InboundV2/
        IMerchantBLL MerchantBLL = ServiceFactory.GetService<IMerchantBLL>();
        IBillTemplatePrintBLL BillTemplatePrintBLL = ServiceFactory.GetService<IBillTemplatePrintBLL>();
        IInboundBLLV2 _inboundBLL = ServiceFactory.GetService<IInboundBLLV2>("SC_InboundBLLV2");
        private static IExpressCompanyBLL expressCompany = ServiceFactory.GetService<IExpressCompanyBLL>();
        private static SortCenterUserModel userModel= null;

        public ActionResult Index()
        {
            InitPage();
            return View("Inbound");
        }

        public ActionResult BatchInbound()
        {
            InitPage();
            return View("BatchInbound");
        }
        private void InitPage()
        {
            userModel = new SortCenterUserModel()
            {
                CompanyName = UserContext.CurrentUser.DeptName,
                DistributionCode = UserContext.CurrentUser.DistributionCode,
                ExpressId = UserContext.CurrentUser.DeptID,
                UserId = UserContext.CurrentUser.ID,
                UserName = UserContext.CurrentUser.UserName,
                CompanyFlag = expressCompany.GetModel(UserContext.CurrentUser.DeptID).CompanyFlag
            };
            var DistributionCode = UserContext.CurrentUser.DistributionCode;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");

            //商家
            var merchantList = MerchantBLL.GetMerchantListByDistributionCode(DistributionCode);



            UserModel curUser = UserContext.CurrentUser;
            //众诚e家账户只显示光大银行 后期可做配置
            if (curUser.UserCode == "zcyj")
            {
                merchantList = merchantList.Where(x => x.ID == 5799 || x.ID == 5809).ToList();
            }
            //面单打印招行信用卡专用,其他商家配置编码也可专享
            if (Request.Params["Merchant"] != null && !string.IsNullOrWhiteSpace(Request.Params["Merchant"].ToString())
               )
            {
                merchantList = merchantList.Where(x => x.MerchantCode == Request.Params["Merchant"].ToString()).ToList();
            }


            ViewBag.TotalCount = _inboundBLL.GetInboundCountV2(userModel);


            ViewBag.MerchantList = merchantList.OrderBy(x => x.Name).ToList();
            ViewBag.PrintTemplates = BillTemplatePrintBLL.GetBillPrintTemplates(UserContext.CurrentUser.DistributionCode);
        }


        public ActionResult Inbound(BillWeightScanArgModel argModel)
        {
            try
            {
                InboundResultModelV2 result = _inboundBLL.SimpleInboundV2(new InboundSimpleArgModelV2()
                {
                    BillWeight = argModel.BillWeight,
                    FormCode = argModel.FormCode,
                    ScanType = argModel.ScanType,
                    OpUser = userModel,
                    MerchantId = argModel.MerchantId
                });


                return Json(new
                {
                    result = result.IsSuccess,
                    Model = new InboundResultModel()
                    {
                        FormCode = result.FormCode,
                        DeliverCode = result.DeliverCode,
                        Message = result.Message,
                        Status = result.IsSuccess ? "已入库" : "入库失败"
                    },
                    TotalCount = result.TotalInboundCount,
                    CurrentCount = result.CurrentCount > 0 ? result.CurrentCount : 1,
                    IsNeedWeight = result.IsNeedWeight,
                    IsSkipPrint = result.IsSkipPrint,
                    CustomerWeight = result.CustomerWeight
                });
            }
            catch (Exception ex)
            {

                return Json(new
                {
                    result = false,
                    Model = new InboundResultModel()
                    {
                        Message = ex.Message,
                        Status =  "入库异常"
                    }
                  
                });
            }
            
        }

        public ActionResult ReWeight(string FormCode, string Weight)
        {
            InboundResultModelV2 result = _inboundBLL.ReWeight(new InboundSimpleArgModelV2()
                {
                    BillWeight = string.IsNullOrEmpty(Weight) ? 0.000m : Convert.ToDecimal(Weight),
                    FormCode = FormCode,
                    ScanType = Enums.SortCenterFormType.Waybill,
                    OpUser = userModel
                });

            return Json(new {result = result.IsSuccess, Message = result.Message});
        }
       

    }
}
