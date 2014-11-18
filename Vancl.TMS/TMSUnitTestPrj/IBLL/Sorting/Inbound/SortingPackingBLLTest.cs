using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vancl.TMS.BLL.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound.Packing;

namespace TMSUnitTestPrj.IBLL.Sorting.Inbound
{
    [TestClass]
    public class SortingPackingBLLTest
    {
        [TestMethod]
        public void TestAddInboundBoxV2_装箱_第一单_未装箱()
        {
            string boxno = "";
            decimal weight = 1;
            string formcode = "2914513961";
            bool isFirst = true;
            SortingPackingBLL bll = new SortingPackingBLL();
            SortCenterUserModel opuser = new SortCenterUserModel() { ExpressId = 2, UserId = 1, DistributionCode = "rfd" };
            InboundPreConditionModel precondition = new InboundPreConditionModel();
            ViewInboundValidateModel inboundData = new ViewInboundValidateModel(opuser, precondition);





            bll.AddInboundBoxV2(inboundData, boxno, weight, formcode, isFirst, new ViewPackingBoxToModel(1,""));
        }

        [TestMethod]
        public void TestAddInboundBoxV2_装箱_第二单_未装箱()
        {
            string boxno = "220130730000001";
            decimal weight = 2;
            string formcode = "2914513962";
            bool isFirst = false;
            SortingPackingBLL bll = new SortingPackingBLL();
            SortCenterUserModel opuser = new SortCenterUserModel() { ExpressId = 2, UserId = 1, DistributionCode = "rfd" };
            InboundPreConditionModel precondition = new InboundPreConditionModel();
            ViewInboundValidateModel inboundData = new ViewInboundValidateModel(opuser, precondition);

            bll.AddInboundBoxV2(inboundData, boxno, weight, formcode, isFirst, new ViewPackingBoxToModel(1, ""));
        }

        [TestMethod]
        public void TestAddInboundBoxV2_装箱_第一单_已装箱()
        {
        }

        [TestMethod]
        public void TestAddInboundBoxV2_装箱_第二单_已装箱()
        {
        }

        [TestMethod]
        public void TestAddInboundBoxV2_删除()
        {
        }
    }
}
