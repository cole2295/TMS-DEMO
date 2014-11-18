using Vancl.TMS.IDAL.Sorting.Inbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.Inbound;
using System.Collections.Generic;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IInboundQueueDALTest 的测试类，旨在
    ///包含所有 IInboundQueueDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class IInboundQueueDALTest
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


        internal virtual IInboundQueueDAL CreateIInboundQueueDAL()
        {
            // TODO: 实例化相应的具体类。
            IInboundQueueDAL target = ServiceFactory.GetService<IInboundQueueDAL>("InboundQueueDAL"); 
            return target;
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IInboundQueueDAL target = CreateIInboundQueueDAL(); // TODO: 初始化为适当的值
            InboundQueueEntityModel model = new InboundQueueEntityModel() 
            { 
                ArrivalID = 9,
                CreateBy = 1,
                CreateDept = 2,
                DepartureID = 2,
                FormCode = "11208150000600",
                SeqStatus = Vancl.TMS.Model.Common.Enums.SeqStatus.Handed,
                Status = Vancl.TMS.Model.Common.Enums.BillStatus.WaitingInbound,
                UpdateBy = 1
            }; // TODO: 初始化为适当的值
            int expected = 1; // TODO: 初始化为适当的值
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetInboundQueueList 的测试
        ///</summary>
        [TestMethod()]
        public void GetInboundQueueListTest()
        {
            IInboundQueueDAL target = CreateIInboundQueueDAL(); // TODO: 初始化为适当的值
            InboundQueueJobArgModel argument = null; // TODO: 初始化为适当的值
            List<InboundQueueEntityModel> expected = null; // TODO: 初始化为适当的值
            List<InboundQueueEntityModel> actual;
            actual = target.GetInboundQueueList(argument);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateOpCount 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateOpCountTest()
        {
            IInboundQueueDAL target = CreateIInboundQueueDAL(); // TODO: 初始化为适当的值
            long ID = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateOpCount(ID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateToError 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToErrorTest()
        {
            IInboundQueueDAL target = CreateIInboundQueueDAL(); // TODO: 初始化为适当的值
            long ID = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToError(ID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateToHandled 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToHandledTest()
        {
            IInboundQueueDAL target = CreateIInboundQueueDAL(); // TODO: 初始化为适当的值
            long ID = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToHandled(ID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
