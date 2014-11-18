using Vancl.TMS.IBLL.Synchronous;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.OutSync;
using System.Collections.Generic;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 ITms2LmsSyncBLLTest 的测试类，旨在
    ///包含所有 ITms2LmsSyncBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class ITms2LmsSyncBLLTest
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


        internal virtual ITms2LmsSyncBLL CreateITms2LmsSyncBLL()
        {
            // TODO: 实例化相应的具体类。
            ITms2LmsSyncBLL target = ServiceFactory.GetService<ITms2LmsSyncBLL>("Tms2LmsSyncBLL"); //Tms2LmsSyncBLL
            return target;
        }

        /// <summary>
        ///DoSync 的测试
        ///</summary>
        [TestMethod()]
        public void DoSyncTest()
        {
            ITms2LmsSyncBLL target = CreateITms2LmsSyncBLL(); // TODO: 初始化为适当的值
            Tms2LmsJobArgs argument = new Tms2LmsJobArgs()
            {
                IntervalDay = 30,
                Mod = 1,
                Remainder = 0,
                TopCount = 1000
            }; 
            List<BillChangeLogModel> listLog = target.ReadTmsChangeLogs(argument);
            if (listLog == null || listLog.Count <= 0)
            {
                return;
            }
            ResultModel expected = new ResultModel(); // TODO: 初始化为适当的值
            expected.IsSuccess = true;
            ResultModel actual;
            actual = target.DoSync(listLog[0]);
            Assert.AreEqual(expected.IsSuccess, actual.IsSuccess);
        }

        ///// <summary>
        /////ReadTmsChangeLogs 的测试
        /////</summary>
        //[TestMethod()]
        //public void ReadTmsChangeLogsTest()
        //{
        //    ITms2LmsSyncBLL target = CreateITms2LmsSyncBLL(); // TODO: 初始化为适当的值
        //    Tms2LmsJobArgs argument = new Tms2LmsJobArgs() 
        //    {
        //        IntervalDay = 30,
        //        Mod = 10,
        //        Remainder = 9,
        //        TopCount = 1000
        //    }; // TODO: 初始化为适当的值
        //    List<BillChangeLogModel> expected = null; // TODO: 初始化为适当的值
        //    List<BillChangeLogModel> actual;
        //    actual = target.ReadTmsChangeLogs(argument);
        //    Assert.AreEqual(expected, actual);
        //}
    }
}
