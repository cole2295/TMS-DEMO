using Vancl.TMS.IDAL.Sorting.Outbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.Outbound;
using System.Collections.Generic;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IOutboundDALTest 的测试类，旨在
    ///包含所有 IOutboundDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class IOutboundDALTest
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


        internal virtual IOutboundDAL CreateIOutboundDAL()
        {
            // TODO: 实例化相应的具体类。
            IOutboundDAL target = null;
            return target;
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IOutboundDAL target = CreateIOutboundDAL(); // TODO: 初始化为适当的值
            OutboundEntityModel outboundModel = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Add(outboundModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BatchAdd 的测试
        ///</summary>
        [TestMethod()]
        public void BatchAddTest()
        {
            IOutboundDAL target = CreateIOutboundDAL(); // TODO: 初始化为适当的值
            List<OutboundEntityModel> listOutboundModel = null; // TODO: 初始化为适当的值
            target.BatchAdd(listOutboundModel);
            Assert.Inconclusive("无法验证不返回值的方法。");
        }

        /// <summary>
        ///GetInboundingCount 的测试
        ///</summary>
        [TestMethod()]
        public void GetInboundingCountTest()
        {
            IOutboundDAL target = CreateIOutboundDAL(); // TODO: 初始化为适当的值
            OutboundSearchArgModel argument = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.GetInboundingCount(argument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetNeededOutboundFormCodeList 的测试
        ///</summary>
        [TestMethod()]
        public void GetNeededOutboundFormCodeListTest()
        {
            IOutboundDAL target = CreateIOutboundDAL(); // TODO: 初始化为适当的值
            OutboundSearchArgModel argument = null; // TODO: 初始化为适当的值
            List<ViewOutboundSearchListModel.InnerFormCodeList> expected = null; // TODO: 初始化为适当的值
            List<ViewOutboundSearchListModel.InnerFormCodeList> actual;
            actual = target.GetNeededOutboundFormCodeList(argument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
