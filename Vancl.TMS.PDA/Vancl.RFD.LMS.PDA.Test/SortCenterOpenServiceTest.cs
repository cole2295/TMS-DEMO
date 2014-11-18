using RFD.LMS.SortCenter.OpenService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using RFD.LMS.Model.SortingCenter;

namespace Vancl.RFD.LMS.PDA.Test
{
    
    
    /// <summary>
    ///这是 SortCenterOpenServiceTest 的测试类，旨在
    ///包含所有 SortCenterOpenServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class SortCenterOpenServiceTest
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
        ///GetSortCenterInboundPreCondition 的测试
        ///</summary>
        // TODO: 确保 UrlToTest 特性指定一个指向 ASP.NET 页的 URL(例如，
        // http://.../Default.aspx)。这对于在 Web 服务器上执行单元测试是必需的，
        //无论要测试页、Web 服务还是 WCF 服务都是如此。
        [TestMethod()]
        [HostType("ASP.NET")]
        [AspNetDevelopmentServerHost("D:\\vancl\\work\\svn\\LMS\\Vancl.TMS.PDA\\Vancl.RFD.LMS.SortCenter.API", "/")]
        [UrlToTest("http://localhost:1922/")]
        public void GetSortCenterInboundPreConditionTest()
        {
            SortCenterOpenService target = new SortCenterOpenService(); // TODO: 初始化为适当的值
            string DistributionCode = "rfd"; // TODO: 初始化为适当的值
            SortCenterInboundPreCondition expected = null; // TODO: 初始化为适当的值
            SortCenterInboundPreCondition actual;
            actual = target.GetSortCenterInboundPreCondition(DistributionCode);
        }
    }
}
