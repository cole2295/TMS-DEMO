using Vancl.TMS.Schedule.Lms2TmsImpl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Quartz;
using Vancl.TMS.Model.Synchronous.InSync;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using System.Collections.Generic;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Synchronous.OutSync;
using Vancl.TMS.BLL.Synchronous.InSync;
using Vancl.TMS.Core.Schedule;
using System.Linq;

namespace TMS.Service.UnitTest
{


    /// <summary>
    ///这是 Lms2TmsJobTest 的测试类，旨在
    ///包含所有 Lms2TmsJobTest 单元测试
    ///</summary>
    [TestClass()]
    public class Lms2TmsJobTest
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


        [TestMethod()]
        public void TMS2LMSJobTest()
        {
            ITms2LmsSyncBLL _bll = ServiceFactory.GetService<ITms2LmsSyncBLL>("Tms2LmsSyncBLL");
            List<BillChangeLogModel> list = _bll.ReadTmsChangeLogs
                (
                    new Tms2LmsJobArgs()
                    {
                        IntervalDay = Vancl.TMS.Schedule.Tms2LmsImpl.Consts.IntervalDay,
                        TopCount = Vancl.TMS.Schedule.Tms2LmsImpl.Consts.BATCH_MAX_COUNT,
                        Mod = Vancl.TMS.Schedule.Tms2LmsImpl.Consts.THREAD_COUNT,
                        Remainder = 2
                    }
                );
            if (list == null || list.Count == 0)
            {
                return;
            }
            foreach (BillChangeLogModel model in list)
            {
                _bll.DoSync(model);
            }
        }

        /// <summary>
        ///DoJob 的测试
        ///</summary>
        [TestMethod()]
        public void DoJobTest()
        {
            Lms2TmsSyncBLL _bll = new Lms2TmsSyncBLL();
			//ILms2TmsSyncBLL _bll = ServiceFactory.GetService<ILms2TmsSyncBLL>("Lms2TmsSyncBLL");
            List<LmsWaybillStatusChangeLogModel> list = _bll.ReadLmsChangeLogs(
                new Lms2TmsJobArgs()
                {
                    IntervalDay = 1,
                    Mod = 1,
                    OnlineTime = DateTime.Parse("2012-08-01"),
                    Remainder = 0,
                    TopCount =200
                });
            if (list == null || list.Count == 0)
            {
                return;
            }
            var OrderedLogModelList = list.OrderBy(p => p.WaybillNo);
            foreach (LmsWaybillStatusChangeLogModel model in OrderedLogModelList)
            {
                _bll.DoSync(model);
            }
        }
    }
}
