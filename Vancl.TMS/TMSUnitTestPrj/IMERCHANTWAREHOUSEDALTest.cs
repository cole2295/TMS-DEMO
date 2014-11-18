using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vancl.TMS.DAL.Oracle.LadingBill;

namespace TMSUnitTestPrj
{
    [TestClass]
    public class IMERCHANTWAREHOUSEDALTest
    {
        private TestContext testContextInstance;


        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod()]
        public void GetModelByidtest()
        {
            MERCHANTWAREHOUSEDAL dd = new MERCHANTWAREHOUSEDAL();
            dd.GetModelByid("31");
        }
    }
}
