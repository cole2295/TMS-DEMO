using Vancl.TMS.IBLL.Sorting.Outbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IOutboundBLLTest 的测试类，旨在
    ///包含所有 IOutboundBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IOutboundBLLTest
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


        internal virtual IOutboundBLL CreateIOutboundBLL()
        {
            // TODO: 实例化相应的具体类。
            IOutboundBLL target = ServiceFactory.GetService<IOutboundBLL>("SC_OutboundBLL"); ;
            return target;
        }

        /// <summary>
        ///BatchOutbound 的测试
        ///</summary>
        [TestMethod()]
        public void BatchOutboundTest()
        {
            IOutboundBLL target = CreateIOutboundBLL(); // TODO: 初始化为适当的值
            OutboundBatchArgModel argument = null; // TODO: 初始化为适当的值
            ViewOutboundBatchModel expected = null; // TODO: 初始化为适当的值
            ViewOutboundBatchModel actual;
            actual = target.BatchOutbound(argument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetNeededOutboundInfo 的测试
        ///</summary>
        [TestMethod()]
        public void GetNeededOutboundInfoTest()
        {
            IOutboundBLL target = CreateIOutboundBLL(); // TODO: 初始化为适当的值
            OutboundSearchArgModel argument = new OutboundSearchArgModel()
            {
                OpUser = target.GetUserModel(1),
                PreCondition = target.GetPreCondition("rfd"),
                ToStation = target.GetToStationModel(826),
                InboundStartTime = DateTime.Now.AddDays(-1),
                InboundEndTime = DateTime.Now
            }; // TODO: 初始化为适当的值
            ViewOutboundSearchListModel expected = null; // TODO: 初始化为适当的值
            ViewOutboundSearchListModel actual;
            actual = target.GetNeededOutboundInfo(argument);
            Assert.AreEqual(1, actual.InboundingCount);
        }

        /// <summary>
        ///GetPreCondition 的测试
        ///</summary>
        [TestMethod()]
        public void GetPreConditionTest()
        {
            IOutboundBLL target = CreateIOutboundBLL(); // TODO: 初始化为适当的值
            string DistributionCode = string.Empty; // TODO: 初始化为适当的值
            OutboundPreConditionModel expected = null; // TODO: 初始化为适当的值
            OutboundPreConditionModel actual;
            actual = target.GetPreCondition(DistributionCode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///PackingOutbound 的测试
        ///</summary>
        [TestMethod()]
        public void PackingOutboundTest()
        {
            IOutboundBLL target = CreateIOutboundBLL(); // TODO: 初始化为适当的值
            OutboundPackingArgModel argument = null; // TODO: 初始化为适当的值
            ViewOutboundPackingModel expected = null; // TODO: 初始化为适当的值
            ViewOutboundPackingModel actual;
            actual = target.PackingOutbound(argument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SearchOutbound 的测试
        ///</summary>
        [TestMethod()]
        public void SearchOutboundTest()
        {
            IOutboundBLL target = CreateIOutboundBLL(); // TODO: 初始化为适当的值
            OutboundSearchArgModel argument = new OutboundSearchArgModel()
            {
                OpUser = target.GetUserModel(1),
                PreCondition = target.GetPreCondition("rfd"),
                ToStation = target.GetToStationModel(826),
                PerBatchCount = 5,
                FormType = Vancl.TMS.Model.Common.Enums.SortCenterFormType.Waybill,
                ArrFormCode = new String[] { "9120512848741" }
            }; // TODO: 初始化为适当的值
            ViewOutboundBatchModel expected = null; // TODO: 初始化为适当的值
            ViewOutboundBatchModel actual;
            actual = target.SearchOutbound(argument);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///SimpleOutbound 的测试
        ///</summary>
        [TestMethod()]
        public void SimpleOutboundTest()
        {
            IOutboundBLL target = CreateIOutboundBLL(); // TODO: 初始化为适当的值
            OutboundSimpleArgModel argument = new OutboundSimpleArgModel()
            {
                OpUser = target.GetUserModel(1),
                PreCondition = target.GetPreCondition("rfd"),
                ToStation = target.GetToStationModel(90),
                FormType = Vancl.TMS.Model.Common.Enums.SortCenterFormType.Waybill,
                BatchNo = "",
                FormCode = "111110101098"
            }
                ; // TODO: 初始化为适当的值
            ViewOutboundSimpleModel expected = null; // TODO: 初始化为适当的值
            ViewOutboundSimpleModel actual;
            actual = target.SimpleOutbound(argument);
            Assert.AreEqual(expected, actual.Message);
        }

    }
}
