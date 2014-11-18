using Vancl.TMS.IDAL.BaseInfo.Line;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Vancl.TMS.Core.ServiceFactory;

namespace TMS.Service.UnitTest
{
    
    
    /// <summary>
    ///这是 ILinePlanDALTest 的测试类，旨在
    ///包含所有 ILinePlanDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class ILinePlanDALTest
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


        internal virtual ILinePlanDAL CreateILinePlanDAL()
        {
            // TODO: 实例化相应的具体类。
            ILinePlanDAL target = ServiceFactory.GetService<ILinePlanDAL>("LinePlanDAL");
            return target;
        }

        /// <summary>
        ///GetEffectivedLinePlan 的测试
        ///</summary>
        [TestMethod()]
        public void GetEffectivedLinePlanTest()
        {
            ILinePlanDAL target = CreateILinePlanDAL(); // TODO: 初始化为适当的值
            string LineID = "A-20-115-00-03-26"; // TODO: 初始化为适当的值
            List<int> expected = null; // TODO: 初始化为适当的值
            List<int> actual;
            actual = target.GetEffectivedLinePlan(LineID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetNeedEffectivedLinePlan 的测试
        ///</summary>
        [TestMethod()]
        public void GetNeedEffectivedLinePlanTest()
        {
            ILinePlanDAL target = CreateILinePlanDAL(); // TODO: 初始化为适当的值
            Nullable<int> expected = new Nullable<int>(); // TODO: 初始化为适当的值
            Nullable<int> actual;
            actual = target.GetNeedEffectivedLinePlan();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///UpdateToExpired 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToExpiredTest()
        {
            ILinePlanDAL target = CreateILinePlanDAL(); // TODO: 初始化为适当的值
            int LPID = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToExpired(LPID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///UpdateToEffective 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToEffectiveTest()
        {
            ILinePlanDAL target = CreateILinePlanDAL(); // TODO: 初始化为适当的值
            int LPID = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToEffective(LPID);
            Assert.AreEqual(expected, actual);
        }
    }
}
