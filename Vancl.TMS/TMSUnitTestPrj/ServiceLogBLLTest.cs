using Vancl.TMS.BLL.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Log;
using System.Collections.Generic;

namespace TMSUnitTestPrj
{


    /// <summary>
    ///这是 ServiceLogBLLTest 的测试类，旨在
    ///包含所有 ServiceLogBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class ServiceLogBLLTest
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
        ///WriteLog 的测试
        ///</summary>
        [TestMethod()]
        public void WriteLogTest()
        {
            ServiceLogBLL target = new ServiceLogBLL(); // TODO: 初始化为适当的值

            for (int i = 0; i < 100; i++)
            {
                ServiceLogModel log = new ServiceLogModel(); // TODO: 初始化为适当的值
                log.LogType = i > 50 ? Vancl.TMS.Model.Common.Enums.ServiceLogType.Lms2TmsSync : Vancl.TMS.Model.Common.Enums.ServiceLogType.Tms2LmsSync;
                log.OpFunction = 1;
                log.Note = "你妹" + i;
                log.FormCode = "FormCode" + i;
                log.IsSuccessed = (i % 2) == 0;
                log.SyncKey = i.ToString();
                int actual;
                actual = target.WriteLog(log);
            }

            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReadLogs 的测试
        ///</summary>
        [TestMethod()]
        public void ReadLogsTest()
        {
            ServiceLogBLL target = new ServiceLogBLL(); // TODO: 初始化为适当的值
            ServiceLogSearchModel conditions = new ServiceLogSearchModel(); // TODO: 初始化为适当的值
            List<ServiceLogModel> expected = null; // TODO: 初始化为适当的值
            List<ServiceLogModel> actual;
            actual = target.ReadLogs(conditions);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
