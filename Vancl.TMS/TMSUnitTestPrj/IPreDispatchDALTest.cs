using Vancl.TMS.IDAL.Transport.Dispatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Vancl.TMS.Model.Transport.PreDispatch;
using System.Data;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.BaseInfo.Order;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.Pager;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IPreDispatchDALTest 的测试类，旨在
    ///包含所有 IPreDispatchDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class IPreDispatchDALTest
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


        internal virtual IPreDispatchDAL CreateIPreDispatchDAL()
        {
            // TODO: 实例化相应的具体类。
            IPreDispatchDAL target = ServiceFactory.GetService<IPreDispatchDAL>("PreDispatchDAL");
            return target;
        }

        /// <summary>
        ///UpdateToInvalid 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToInvalidTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            List<long> listPDID = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToInvalid(listPDID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateToDispatched 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToDispatchedTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            List<long> listPDID = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToDispatched(listPDID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateToDisabledDispatch 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToDisabledDispatchTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            List<long> listPDID = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToDisabledDispatch(listPDID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateToCanDispatch 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToCanDispatchTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            List<long> listPDID = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToCanDispatch(listPDID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SearchPreDispatchInfo 的测试
        ///</summary>
        [TestMethod()]
        public void SearchPreDispatchInfoTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            PreDispatchSearchModel searchmodel = new PreDispatchSearchModel()
            {
                ArrivalID = 145,
                DepartureID = 2
            }; // TODO: 初始化为适当的值
            List<ViewPreDispatchModel> expected = null; // TODO: 初始化为适当的值
            List<ViewPreDispatchModel> actual;
            actual = target.SearchPreDispatchInfo(searchmodel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetValidBoxNoAndTPID 的测试
        ///</summary>
        [TestMethod()]
        public void GetValidBoxNoAndTPIDTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            int count = 0; // TODO: 初始化为适当的值
            DataTable expected = null; // TODO: 初始化为适当的值
            DataTable actual;
            actual = target.GetValidBoxNoAndTPID(count);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetPreDispatchInfo 的测试
        ///</summary>
        [TestMethod()]
        public void GetPreDispatchInfoTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            PreDispatchSearchModel searchmodel = new PreDispatchSearchModel() {
                ArrivalID = 145,
                DepartureID = 2,
                CustomerBatchNo = "201212131147474747"
            }; // TODO: 初始化为适当的值
            List<ViewPreDispatchModel> expected = null; // TODO: 初始化为适当的值
            List<ViewPreDispatchModel> actual;
            actual = target.GetPreDispatchInfo(searchmodel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetPreDispatchBoxList 的测试
        ///</summary>
        [TestMethod()]
        public void GetPreDispatchBoxListTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            int LPID = 0; // TODO: 初始化为适当的值
            List<ViewDispatchBoxModel> expected = null; // TODO: 初始化为适当的值
            List<ViewDispatchBoxModel> actual;
            actual = target.GetPreDispatchBoxList(LPID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetNeededPreDispatchBatchList 的测试
        ///</summary>
        [TestMethod()]
        public void GetNeededPreDispatchBatchListTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            PreDispatchJobArgModel arguments = null; // TODO: 初始化为适当的值
            List<BoxModel> expected = null; // TODO: 初始化为适当的值
            List<BoxModel> actual;
            actual = target.GetNeededPreDispatchBatchList(arguments);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetCurPreDispatchList 的测试
        ///</summary>
        [TestMethod()]
        public void GetCurPreDispatchListTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            string[] box = null; // TODO: 初始化为适当的值
            bool isContainDispatched = false; // TODO: 初始化为适当的值
            List<PreDispatchModel> expected = null; // TODO: 初始化为适当的值
            List<PreDispatchModel> actual;
            actual = target.GetCurPreDispatchList(box, isContainDispatched);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BatchAddPreDispatchLog 的测试
        ///</summary>
        [TestMethod()]
        public void BatchAddPreDispatchLogTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            List<PreDispatchLogEntityModel> listLogModel = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.BatchAddPreDispatchLog(listLogModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///BatchAdd 的测试
        ///</summary>
        [TestMethod()]
        public void BatchAddTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            List<PreDispatchModel> model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.BatchAdd(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///AddPreDispatchLog 的测试
        ///</summary>
        [TestMethod()]
        public void AddPreDispatchLogTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            PreDispatchLogEntityModel LogModel = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.AddPreDispatchLog(LogModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            PreDispatchModel model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest1()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            string BoxNos = string.Empty; // TODO: 初始化为适当的值
            int TPID = 0; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Add(BoxNos, TPID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SearchPreDispatchStatisticLog 的测试
        ///</summary>
        [TestMethod()]
        public void SearchPreDispatchStatisticLogTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            PreDispatchLogSearchModel searchModel = new PreDispatchLogSearchModel() {
            DepartureID = 2,
            ArrivalID = 9
            };  // TODO: 初始化为适当的值
            List<ViewPreDispatchLogStatisticModel> expected = null; // TODO: 初始化为适当的值
            List<ViewPreDispatchLogStatisticModel> actual;
            actual = target.SearchPreDispatchStatisticLog(searchModel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///SearchPreDispatchLog 的测试
        ///</summary>
        [TestMethod()]
        public void SearchPreDispatchLogTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            PreDispatchLogSearchModel searchModel = new PreDispatchLogSearchModel() {
            DepartureID = 2,
            ArrivalID = 9
            }; // TODO: 初始化为适当的值
            PagedList<ViewPreDispatchLogModel> expected = null; // TODO: 初始化为适当的值
            PagedList<ViewPreDispatchLogModel> actual;
            actual = target.SearchPreDispatchLog(searchModel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///UpdateBoxToWaitforDispatch 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateBoxToWaitforDispatchTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            BoxModel box = new BoxModel() { BoxNo = "2ST20130110100001" }; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateBoxToWaitforDispatch(box);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///BatchUpdateUpdateBoxToWaitforDispatch 的测试
        ///</summary>
        [TestMethod()]
        public void BatchUpdateUpdateBoxToWaitforDispatchTest()
        {
            IPreDispatchDAL target = CreateIPreDispatchDAL(); // TODO: 初始化为适当的值
            List<BoxModel> listbox = new List<BoxModel>(); // TODO: 初始化为适当的值
            BoxModel box = new BoxModel() { BoxNo = "2ST20130110100001" };
            listbox.Add(box);
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.BatchUpdateUpdateBoxToWaitforDispatch(listbox);
            Assert.AreEqual(expected, actual);
        }
    }
}
