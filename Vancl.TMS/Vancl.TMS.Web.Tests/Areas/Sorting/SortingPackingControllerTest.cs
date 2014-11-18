using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Util.JsonUtil;
using Vancl.TMS.Web.Areas.Sorting.Controllers;
using Vancl.TMS.Util.Security;
using Vancl.TMS.Web.Areas.Sorting.Models;

namespace Vancl.TMS.Web.Tests.Areas.Sorting
{
    [TestClass]
    public class SortingPackingControllerTest
    {
        [TestMethod]
        public void TestScanFormCodeV2_运单不存在()
        {
            var h = GetHidData(2, Enums.CompanyFlag.SortingCenter);
            SortingPackingController spc = new SortingPackingController();
            int codetype = 1;
            string code = "123";
            string boxno = "";
            decimal weight = 1;
            bool isfirst = true;

            var r = spc.ScanFormCodeV2(h, codetype, code, boxno, weight, isfirst,0,"");

            var rmodel = ((SortingPackingResultModel)(((JsonResult)(r)).Data));

            Assert.IsFalse(rmodel.IsSuccess);
            Assert.AreEqual("该运单不存在!", rmodel.Message);
        }

        [TestMethod]
        public void TestScanFormCodeV2_运单状态不符合装箱条件()
        {
            var h = GetHidData(2, Enums.CompanyFlag.SortingCenter);
            SortingPackingController spc = new SortingPackingController();
            int codetype = 1;
            string code = "13211111561";
            string boxno = "";
            decimal weight = 1;
            bool isfirst = true;

            var r = spc.ScanFormCodeV2(h, codetype, code, boxno, weight, isfirst, 0, "");

            var rmodel = ((SortingPackingResultModel)(((JsonResult)(r)).Data));

            Assert.IsFalse(rmodel.IsSuccess);
            Assert.AreEqual(true, rmodel.Message.Contains("运单状态不符合装箱条件"));
        }

        [TestMethod]
        public void TestScanFormCodeV2_第二单_该运单已装箱()
        {
            var h = GetHidData(2, Enums.CompanyFlag.SortingCenter);
            SortingPackingController spc = new SortingPackingController();
            int codetype = 1;
            string code = "2914513962";
            string boxno = "";
            decimal weight = 1;
            bool isfirst = false;

            var r = spc.ScanFormCodeV2(h, codetype, code, boxno, weight, isfirst, 0, "");

            var rmodel = ((SortingPackingResultModel)(((JsonResult)(r)).Data));

            Assert.IsFalse(rmodel.IsSuccess);
            Assert.AreEqual(true, rmodel.Message.Contains("该运单已装箱"));
        }

        [TestMethod]
        public void TestScanFormCodeV2_第一单_该运单已装箱()
        {
            var h = GetHidData(2, Enums.CompanyFlag.SortingCenter);
            SortingPackingController spc = new SortingPackingController();
            int codetype = 1;
            string code = "2914513962";
            string boxno = "";
            decimal weight = 1;
            bool isfirst = true;

            var r = spc.ScanFormCodeV2(h, codetype, code, boxno, weight, isfirst, 0, "");

            var rmodel = ((SortingPackingResultModel)(((JsonResult)(r)).Data));

            Assert.IsTrue(rmodel.IsSuccess);
        }





        [TestMethod]
        public void TestScanFormCodeV2_装箱_第一单_未装箱()
        {
            var h = GetHidData(2, Enums.CompanyFlag.SortingCenter);
            SortingPackingController spc = new SortingPackingController();
            int codetype = 1;
            string code = "13211111570";
            string boxno = "";
            decimal weight = 1;
            bool isfirst = true;

            spc.ScanFormCodeV2(h, codetype, code, boxno, weight, isfirst, 0, "");
        }

        [TestMethod]
        public void TestScanFormCodeV2_装箱_第二单_未装箱()
        {
            var h = GetHidData(2, Enums.CompanyFlag.SortingCenter);
            SortingPackingController spc = new SortingPackingController();
            int codetype = 1;
            string code = "314564401";
            string boxno = "220130731000002";
            decimal weight = 1;
            bool isfirst = false;

            spc.ScanFormCodeV2(h, codetype, code, boxno, weight, isfirst, 0, "");
        }

        private string GetHidData(int? expressId, Enums.CompanyFlag comFlag)
        {
            SortCenterUserModel userModel = new SortCenterUserModel();
            userModel.ExpressId = expressId;
            userModel.CompanyFlag = comFlag;
            InboundPreConditionModel preCondition = new InboundPreConditionModel();

            ViewInboundValidateModel viewModel = new ViewInboundValidateModel(userModel, preCondition);
            var h = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(viewModel));
            return h;
        }
    }
}
