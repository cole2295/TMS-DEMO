using Vancl.TMS.IBLL.Sorting.Inbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Common;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.Model.Sorting.Inbound.TurnStation;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.BLL.Sorting.Inbound;
using Vancl.TMS.IDAL.Sorting.Inbound;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IInboundBLLTest 的测试类，旨在
    ///包含所有 IInboundBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IInboundBLLTest
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


        internal virtual IInboundBLL CreateIInboundBLL()
        {
            // TODO: 实例化相应的具体类。
            IInboundBLL target = ServiceFactory.GetService<IInboundBLL>("SC_InboundBLL");
            return target;
        }

        /// <summary>
        ///BatchInbound 的测试
        ///</summary>
        [TestMethod()]
        public void BatchInboundTest()
        {
            IInboundBLL target = CreateIInboundBLL(); // TODO: 初始化为适当的值
            InboundBatchArgModel argument = null; // TODO: 初始化为适当的值
            ViewInboundBatchModel expected = null; // TODO: 初始化为适当的值
            ViewInboundBatchModel actual;
            actual = target.BatchInbound(argument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetInboundCount 的测试
        ///</summary>
        [TestMethod()]
        public void GetInboundCountTest()
        {
            IInboundBLL target = CreateIInboundBLL(); // TODO: 初始化为适当的值
            ISortCenterArgModel argument = new InboundSimpleArgModel()
            {
                OpUser = new SortCenterUserModel() { ExpressId = 2 },
                ToStation = new SortCenterToStationModel() { ExpressCompanyID = 8 }
            }; // TODO: 初始化为适当的值
            int expected = 2; // TODO: 初始化为适当的值
            int actual;
            actual = target.GetInboundCount(argument);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetPreCondition 的测试
        ///</summary>
        [TestMethod()]
        public void GetPreConditionTest()
        {
            IInboundBLL target = CreateIInboundBLL(); // TODO: 初始化为适当的值
            string DistributionCode = "rfd"; // TODO: 初始化为适当的值
            InboundPreConditionModel expected = null; // TODO: 初始化为适当的值
            InboundPreConditionModel actual;
            actual = target.GetPreCondition(DistributionCode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///HandleInboundQueue 的测试
        ///</summary>
        [TestMethod()]
        public void HandleInboundQueueTest()
        {
            IInboundBLL target = CreateIInboundBLL(); // TODO: 初始化为适当的值
            IInboundQueueBLL queue = ServiceFactory.GetService<IInboundQueueBLL>();

            var list = queue.GetInboundQueueList(new InboundQueueJobArgModel()
            {
                OpCount = 7,
                PerBatchCount = 300
            });
            if(list == null || list.Count <= 0)
            {
                return;
            }

            InboundQueueArgModel argument = new InboundQueueArgModel()
            {
                LineAreaSMSConfig = target.GetLineAreaSMSConfig(),
                MerchantSMSConfig = target.GetMerchantSMSConfig(),
                QueueItem = list[0]
            }; // TODO: 初始化为适当的值
            target.HandleInboundQueue(argument);
        }

        /// <summary>
        ///HandleInboundSMSQueue 的测试
        ///</summary>
        [TestMethod()]
        public void HandleInboundSMSQueueTest()
        {
            IInboundBLL target = CreateIInboundBLL(); // TODO: 初始化为适当的值
            IInboundSMSQueueBLL smsqueue = ServiceFactory.GetService<IInboundSMSQueueBLL>();
            var list = smsqueue.GetInboundSMSQueue(new InboundSMSQueueJobArgModel() 
            {
                OpCount = 7,
                PerBatchCount = 300
            });
            if (list == null || list.Count <= 0)
            {
                return;
            }

            InboundSMSQueueArgModel argument = new InboundSMSQueueArgModel()
            {
                QueueItem = list[0]
            }; // TODO: 初始化为适当的值
            target.HandleInboundSMSQueue(argument);
        }

        /// <summary>
        ///SimpleInbound 的测试
        ///</summary>
        [TestMethod()]
        public void SimpleInboundTest()
        {
            IInboundBLL target = CreateIInboundBLL(); // TODO: 初始化为适当的值
            InboundSimpleArgModel argument = new InboundSimpleArgModel()
            {
                FormCode = "9876543210989",
                FormType = Vancl.TMS.Model.Common.Enums.SortCenterFormType.Waybill,
                OpUser = target.GetUserModel(1),
                PreCondition = target.GetPreCondition("rfd"),
                ToStation = target.GetToStationModel(20)
            }; // TODO: 初始化为适当的值
            ViewInboundSimpleModel expected = null; // TODO: 初始化为适当的值
            ViewInboundSimpleModel actual;
            actual = target.SimpleInbound(argument);
            Assert.AreEqual(expected, actual.Message);
        }

        /// <summary>
        ///TurnInbound 的测试
        ///</summary>
        [TestMethod()]
        public void TurnInboundTest()
        {
            IInboundBLL target = CreateIInboundBLL(); // TODO: 初始化为适当的值
            InboundSimpleArgModel argument = new InboundSimpleArgModel()
            {
                FormCode = "212102764436",//212102764436
                FormType = Vancl.TMS.Model.Common.Enums.SortCenterFormType.Order,
                OpUser = target.GetUserModel(1),
                PreCondition = target.GetPreCondition("rfd"),
                ToStation = target.GetToStationModel(90)
            }; // TODO: 初始化为适当的值
            ViewTurnInboundSimpleModel expected = null; // TODO: 初始化为适当的值
            ViewTurnInboundSimpleModel actual;
            actual = target.TurnInbound(argument);
            Assert.AreEqual(expected, actual.Message);
        }

        /// <summary>
        ///SimpleInbound_NoLimitedStation 的测试
        ///</summary>
        [TestMethod()]
        public void SimpleInbound_NoLimitedStationTest()
        {
            IInboundBLL target = CreateIInboundBLL(); // TODO: 初始化为适当的值
            InboundSimpleArgModel argument = new InboundSimpleArgModel()
            {
                FormCode = "11208230000373",//212102764436
                FormType = Vancl.TMS.Model.Common.Enums.SortCenterFormType.Waybill,
                OpUser = target.GetUserModel(1),
                PreCondition = target.GetPreCondition("rfd"),
                ToStation = target.GetToStationModel(20)
            }; // TODO: 初始化为适当的值
            ViewInboundSimpleModel expected = null; // TODO: 初始化为适当的值
            ViewInboundSimpleModel actual;
            actual = target.SimpleInbound_NoLimitedStation(argument);
            Assert.AreEqual(expected, actual.Message);
        }
    }
}
