using Vancl.TMS.IDAL.Sorting.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 ISortCenterDALTest 的测试类，旨在
    ///包含所有 ISortCenterDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class ISortCenterDALTest
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


        internal virtual ISortCenterDAL CreateISortCenterDAL()
        {
            // TODO: 实例化相应的具体类。
            ISortCenterDAL target = ServiceFactory.GetService<ISortCenterDAL>("SortCenterDAL"); 
            return target;
        }

        /// <summary>
        ///GetToStationModel 的测试
        ///</summary>
        [TestMethod()]
        public void GetToStationModelTest()
        {
            ISortCenterDAL target = CreateISortCenterDAL(); // TODO: 初始化为适当的值
            int ArrivalID = 394; // TODO: 初始化为适当的值
            SortCenterToStationModel expected = null; // TODO: 初始化为适当的值
            SortCenterToStationModel actual;
            actual = target.GetToStationModel(ArrivalID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetUserModel 的测试
        ///</summary>
        [TestMethod()]
        public void GetUserModelTest()
        {
            ISortCenterDAL target = CreateISortCenterDAL(); // TODO: 初始化为适当的值
            int UserID = 434; // TODO: 初始化为适当的值
            SortCenterUserModel expected = null; // TODO: 初始化为适当的值
            SortCenterUserModel actual;
            actual = target.GetUserModel(UserID);
            Assert.AreEqual(expected,actual);
        }
    }
}
