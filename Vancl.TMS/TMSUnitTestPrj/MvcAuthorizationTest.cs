using Vancl.TMS.Web.WebControls.Mvc.Authorization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TMSUnitTestPrj
{


    /// <summary>
    ///这是 MvcAuthorizationTest 的测试类，旨在
    ///包含所有 MvcAuthorizationTest 单元测试
    ///</summary>
    [TestClass()]
    public class MvcAuthorizationTest
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
        ///Author 的测试
        ///</summary>
        [TestMethod()]
        public void AuthorTest()
        {
            bool actual;
            List<string> roles = new List<string> { "a",""};

            actual = MvcAuthorization.Author("BaseInfo", "", "", "zbd", null);
            Assert.IsTrue(actual);

            actual = MvcAuthorization.Author("BaseInfo", "Carrier", "Add1", "zbd", null);
            Assert.IsTrue(actual);

            actual = MvcAuthorization.Author("BaseInfo", "Carrier", "", "", roles);
            Assert.IsFalse(actual);

            actual = MvcAuthorization.Author("LinePlan", "Line", "Edit1", "", null);
            Assert.IsFalse(actual);

        }
    }
}
