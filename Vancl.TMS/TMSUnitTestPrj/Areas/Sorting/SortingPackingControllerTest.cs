using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vancl.TMS.Web.Areas.Sorting.Controllers;

namespace TMSUnitTestPrj.Areas.Sorting
{
    [TestClass]
    public class SortingPackingControllerTest
    {
        [TestMethod]
        public void TestAddInboundBoxV2_装箱_第一单_未装箱()
        {
            SortingPackingController spc = new SortingPackingController();
            int codetype = 1;
            string code = "123";
            string boxno = "";
            decimal weight = 1;
            bool isfirst = true;

            spc.ScanFormCodeV2("", codetype, code, boxno, weight, isfirst, 0, "");
        }
    }
}
