using Vancl.TMS.IBLL.Sorting.BillPrint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IBillPrintBLLTest 的测试类，旨在
    ///包含所有 IBillPrintBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IBillPrintBLLTest
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

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion



        /// <summary>
        ///GetPrintData 的测试
        ///</summary>
        [TestMethod()]
        public void GetPrintDataTest()
        {
            IBillPrintBLL BillPrintBLL = ServiceFactory.GetService<IBillPrintBLL>();
            var pm = BillPrintBLL.GetPrintData("27451347", 0);
            Assert.IsNotNull(pm);
        }

        internal virtual IBillPrintBLL CreateIBillPrintBLL()
        {
            // TODO: 实例化相应的具体类。
            IBillPrintBLL target = null;
            return target;
        }

    }
}
