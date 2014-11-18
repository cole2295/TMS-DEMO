using Vancl.TMS.IBLL.Report.ComplexReport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Report.ComplexReport;
using System.Collections.Generic;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IComplexReportBLLTest 的测试类，旨在
    ///包含所有 IComplexReportBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IComplexReportBLLTest
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


        internal virtual IComplexReportBLL CreateIComplexReportBLL()
        {
            // TODO: 实例化相应的具体类。
            IComplexReportBLL target = ServiceFactory.GetService<IComplexReportBLL>("ComplexReportBLL");
            return target;
        }

        /// <summary>
        ///Export 的测试
        ///</summary>
        [TestMethod()]
        public void ExportTest()
        {
            IComplexReportBLL target = CreateIComplexReportBLL(); // TODO: 初始化为适当的值
            ComplexReportSearchModel searchModel = null; // TODO: 初始化为适当的值
            ViewComplexReportExportModel expected = null; // TODO: 初始化为适当的值
            ViewComplexReportExportModel actual;
            actual = target.Export(searchModel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Search 的测试
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            IComplexReportBLL target = CreateIComplexReportBLL(); // TODO: 初始化为适当的值
            ComplexReportSearchModel searchModel = new ComplexReportSearchModel() { 
                //DepartureStartTime = DateTime.Parse("2012-05-07"),
                //DepartureEndTime = DateTime.Parse("2012-09-07"),
                DeliveryNo = "LN20120516",      //LN201205160002
                PageIndex = 1,
                PageSize = 10
            }; // TODO: 初始化为适当的值
            ViewComplexReportPageModel expected = null; // TODO: 初始化为适当的值
            ViewComplexReportPageModel actual;
            actual = target.Search(searchModel);
            Assert.AreEqual(expected, actual);
        }
    }
}
