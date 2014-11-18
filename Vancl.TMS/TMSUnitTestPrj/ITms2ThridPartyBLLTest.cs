using Vancl.TMS.IBLL.Synchronous;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Synchronous.OutSync;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 ITms2ThridPartyBLLTest 的测试类，旨在
    ///包含所有 ITms2ThridPartyBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class ITms2ThridPartyBLLTest
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


        internal virtual ITms2ThridPartyBLL CreateITms2ThridPartyBLL()
        {
            // TODO: 实例化相应的具体类。
            ITms2ThridPartyBLL target = ServiceFactory.GetService<ITms2ThridPartyBLL>("Tms2ThridPartyBLL"); ;
            return target;
        }

        /// <summary>
        ///TMS2LMS4AgingMonitoring 的测试
        ///</summary>
        [TestMethod()]
        public void TMS2LMS4AgingMonitoringTest()
        {
            ITms2ThridPartyBLL target = CreateITms2ThridPartyBLL(); // TODO: 初始化为适当的值
            Tms2ThridPartyJobArgs args = new Tms2ThridPartyJobArgs() 
            { 
                IntervalDay = 2,
                OnlineTime = DateTime.Parse("2012-12-28"),
                TopCount = 3
            }; // TODO: 初始化为适当的值
            target.TMS2LMS4AgingMonitoring(args);
        }
    }
}
