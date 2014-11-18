using Vancl.TMS.IDAL.BaseInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Order;
using System.Collections.Generic;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IBoxDALTest 的测试类，旨在
    ///包含所有 IBoxDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class IBoxDALTest
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


        internal virtual IBoxDAL CreateIBoxDAL()
        {
            // TODO: 实例化相应的具体类。
            IBoxDAL target = ServiceFactory.GetService<IBoxDAL>("BoxDAL"); ;
            return target;
        }

        /// <summary>
        ///IsBoxNoExists 的测试
        ///</summary>
        [TestMethod()]
        public void IsBoxNoExistsTest()
        {
            IBoxDAL target = CreateIBoxDAL(); // TODO: 初始化为适当的值
            string BatchNo ="dasdsadsa"; // TODO: 初始化为适当的值
            Enums.TMSEntranceSource Source = Enums.TMSEntranceSource.SortingOutbound;
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.IsBoxNoExists(BatchNo, Source);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///AddBoxDetail 的测试
        ///</summary>
        [TestMethod()]
        public void AddBoxDetailTest()
        {
            IBoxDAL target = CreateIBoxDAL(); // TODO: 初始化为适当的值
            List<BoxDetailModel> model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.AddBoxDetail(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IBoxDAL target = CreateIBoxDAL(); // TODO: 初始化为适当的值
            BoxModel model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
