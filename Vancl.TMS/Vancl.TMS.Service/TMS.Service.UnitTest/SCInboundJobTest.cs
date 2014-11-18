using Vancl.TMS.Schedule.SCInboundImpl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Quartz;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Sorting.Inbound;
using System.Collections.Generic;

namespace TMS.Service.UnitTest
{
    
    
    /// <summary>
    ///这是 SCInboundJobTest 的测试类，旨在
    ///包含所有 SCInboundJobTest 单元测试
    ///</summary>
    [TestClass()]
    public class SCInboundJobTest
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
        ///DoJob 的测试
        ///</summary>
        [TestMethod()]
        public void DoJobTest()
        {
            IInboundBLL InboundBLL = ServiceFactory.GetService<IInboundBLL>("SC_InboundBLL");
            IInboundQueueBLL queue = ServiceFactory.GetService<IInboundQueueBLL>();
            SCInboundJob target = new SCInboundJob(); // TODO: 初始化为适当的值
            JobExecutionContext context = null; // TODO: 初始化为适当的值
            //target.DoJob(context);
            //Assert.Inconclusive("无法验证不返回值的方法。");
            var list = queue.GetInboundQueueList(new InboundQueueJobArgModel()
            {
                OpCount = 600,
                IntervalDay = 5,
                PerBatchCount = 600,
                ModValue = 3,
                Remaider = 2
            });
            if (list == null || list.Count <= 0)
            {
                return;
            }
            var listErrorInfo = new List<string>();
            var LineAreaSMSConfig = InboundBLL.GetLineAreaSMSConfig();
            var MerchantSMSConfig = InboundBLL.GetMerchantSMSConfig();
            foreach (var item in list)
            {
                try
                {
                    InboundQueueArgModel argument = new InboundQueueArgModel()
                    {
                        LineAreaSMSConfig = LineAreaSMSConfig,
                        MerchantSMSConfig = MerchantSMSConfig,
                        QueueItem = item
                    };
                    InboundBLL.HandleInboundQueue(argument);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
    }
}
