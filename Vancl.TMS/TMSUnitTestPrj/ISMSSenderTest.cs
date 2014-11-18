using Vancl.TMS.IBLL.SMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.SMS;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 ISMSSenderTest 的测试类，旨在
    ///包含所有 ISMSSenderTest 单元测试
    ///</summary>
    [TestClass()]
    public class ISMSSenderTest
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


        internal virtual ISMSSender CreateISMSSender()
        {
            // TODO: 实例化相应的具体类。
            ISMSSender target = ServiceFactory.GetService<ISMSSender>("SMSSender"); ;
            return target;
        }

        /// <summary>
        ///Send 的测试
        ///</summary>
        [TestMethod()]
        public void SendTest()
        {
            //ISMSSender target = CreateISMSSender(); // TODO: 初始化为适当的值
            //SMSMessage msg = new SMSMessage() 
            //{
            //    Content = "你妹妹",
            //    FormCode = "32323232",
            //    PhoneNumber="13161025443",
            //    Title="短信单元测试"
            //}; // TODO: 初始化为适当的值
            //ResultModel expected = null; // TODO: 初始化为适当的值
            //ResultModel actual;
            //actual = target.Send(msg);
            //Assert.AreEqual(null, actual.Message);
        }
    }
}
