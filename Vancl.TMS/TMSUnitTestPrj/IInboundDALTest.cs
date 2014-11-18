using Vancl.TMS.IDAL.Sorting.Inbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;


namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IInboundDALTest 的测试类，旨在
    ///包含所有 IInboundDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class IInboundDALTest
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


        internal virtual IInboundDAL CreateIInboundDAL()
        {
            // TODO: 实例化相应的具体类。
            IInboundDAL target = ServiceFactory.GetService<IInboundDAL>("InboundDAL"); 
            return target;
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IInboundDAL target = CreateIInboundDAL(); // TODO: 初始化为适当的值
            InboundEntityModel model = new InboundEntityModel() 
            {
                ApplyStation = null,
                ArrivalID = 9,
                CreateBy = 1,
                DepartureID = 2,
                FormCode = "11208150004463",
                InboundType = Vancl.TMS.Model.Common.Enums.SortCenterOperateType.SimpleSorting,
                SyncFlag = Vancl.TMS.Model.Common.Enums.SyncStatus.NotYet,
                UpdateBy = 1
            }; // TODO: 初始化为适当的值
            int expected = 1; // TODO: 初始化为适当的值
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetInboundCount 的测试
        ///</summary>
        [TestMethod()]
        public void GetInboundCountTest()
        {
            IInboundDAL target = CreateIInboundDAL(); // TODO: 初始化为适当的值
            int DepartureID = 2; // TODO: 初始化为适当的值
            int ArrivalID = 9; // TODO: 初始化为适当的值
            DateTime StartTime =DateTime.Now.AddDays(-4); // TODO: 初始化为适当的值
            DateTime EndTime = DateTime.Now; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.GetInboundCount(DepartureID, ArrivalID, StartTime, EndTime);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///CheckBillWeight 的测试
        ///</summary>
        [TestMethod()]
        public void CheckBillWeightTest()
        {
            IInboundDAL target = CreateIInboundDAL(); // TODO: 初始化为适当的值
            string FormCode = "2107"; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.ValidateBillWeight(FormCode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///CheckBillWeight 的测试
        ///</summary>
        [TestMethod()]
        public void ValidateDistributionDeliverStationTest()
        {
            IInboundDAL target = CreateIInboundDAL(); // TODO: 初始化为适当的值
            string DistributionCode = "RFD"; // TODO: 初始化为适当的值
            int DeliverStationID = 404;
            bool expected = true; // TODO: 初始化为适当的值
            bool actual;
            actual = target.ValidateDistributionDeliverStation(DistributionCode, DeliverStationID);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void ExistsEqualLine()
        {
            IInboundDAL target = CreateIInboundDAL(); // TODO: 初始化为适当的值
            String FormCode = "11208150004460";
            int DepartureID = 2;
            int ArrivalID = 9;
            bool expected = true; // TODO: 初始化为适当的值
            bool actual;
            actual = target.ExistsLine_EqualLastInboud(FormCode, DepartureID, ArrivalID);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest1()
        {
            IInboundDAL target = CreateIInboundDAL(); // TODO: 初始化为适当的值
            InboundEntityModel model = null; // TODO: 初始化为适当的值
            long expected = 0; // TODO: 初始化为适当的值
            long actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
