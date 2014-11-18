using Vancl.TMS.BLL.BaseInfo;
using Vancl.TMS.IBLL.BaseInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo;

namespace TMSUnitTestPrj
{


    /// <summary>
    ///这是 IExpressCompanyBLLTest 的测试类，旨在
    ///包含所有 IExpressCompanyBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IExpressCompanyBLLTest
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


        internal virtual IExpressCompanyBLL CreateIExpressCompanyBLL()
        {

            IExpressCompanyBLL target = null;
            return target;
        }

        /// <summary>
        ///GetAllCompanyNames 的测试
        ///</summary>
        [TestMethod()]
        public void GetAllCompanyNamesTest()
        {
            IExpressCompanyBLL target = CreateIExpressCompanyBLL();
            IList<string> expected = null;
            IList<string> actual;
            actual = target.GetAllCompanyNames();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetChildExpressCompany 的测试
        ///</summary>
        [TestMethod()]
        public void GetChildExpressCompanyTest()
        {
            IExpressCompanyBLL target = CreateIExpressCompanyBLL();
            int parentId = 0;
            IList<ExpressCompanyModel> expected = null;
            IList<ExpressCompanyModel> actual;
            actual = target.GetChildExpressCompany(parentId);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Search 的测试
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            IExpressCompanyBLL target = CreateIExpressCompanyBLL();
            List<int> listExpressCompanyID = null;
            IList<ExpressCompanyModel> expected = null;
            IList<ExpressCompanyModel> actual;
            actual = target.Search(listExpressCompanyID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }



        [TestMethod()]
        public void TestGetSortingCenterByStation()
        {
            IExpressCompanyBLL target = new ExpressCompanyBLL();
            var a = target.GetSortingCenterByStation(173);

            Assert.AreEqual(1, a.Count);
        }
    }
}
