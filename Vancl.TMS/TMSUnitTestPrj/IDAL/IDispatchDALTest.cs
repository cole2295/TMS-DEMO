using Vancl.TMS.IDAL.Transport.Dispatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Util.Pager;
using System.Collections.Generic;
using System.Data;

namespace TMSUnitTestPrj
{


    /// <summary>
    ///这是 IDispatchDALTest 的测试类，旨在
    ///包含所有 IDispatchDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class IDispatchDALTest
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


        internal virtual IDispatchDAL CreateIDispatchDAL()
        {

            IDispatchDAL target = null;
            return target;
        }

        /// <summary>
        ///UpdateDispatchCofirmExpArrivalDate 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateDispatchCofirmExpArrivalDateTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            long did = 0;
            DateTime dt = new DateTime();
            int expected = 0;
            int actual;
            actual = target.UpdateDispatchCofirmExpArrivalDate(did, dt);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateDeliveryStatus 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateDeliveryStatusTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            string deliveryNO = string.Empty;
            Enums.DeliveryStatus deliveryStatus = new Enums.DeliveryStatus();
            int expected = 0;
            int actual;
            actual = target.UpdateDeliveryStatus(deliveryNO, deliveryStatus);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            DispatchModel model = null;
            int expected = 0;
            int actual;
            actual = target.Update(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Search 的测试
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            DispatchSearchModel searchModel = null;
            PagedList<ViewDispatchModel> expected = null;
            PagedList<ViewDispatchModel> actual;
            actual = target.Search(searchModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetValidBoxList 的测试
        ///</summary>
        [TestMethod()]
        public void GetValidBoxListTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            int DepartureID = 0;
            string[] disPatchedBox = null;
            List<ViewDispatchBoxModel> expected = null;
            List<ViewDispatchBoxModel> actual=null;
      //      actual = target.GetValidBoxList(DepartureID, disPatchedBox, "", "");
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetStatisticInfo 的测试
        ///</summary>
        [TestMethod()]
        public void GetStatisticInfoTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            DispatchSearchModel searchModel = null;
            ViewDispatchStatisticModel expected = null;
            ViewDispatchStatisticModel actual;
            actual = target.GetStatisticInfo(searchModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDispatchIsPlanedPDID 的测试
        ///</summary>
        [TestMethod()]
        public void GetDispatchIsPlanedPDIDTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            string DeliveryNo = string.Empty;
            List<long> expected = null;
            List<long> actual;
            actual = target.GetDispatchIsPlanedPDID(DeliveryNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Get 的测试
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            long did = 0;
            DispatchModel expected = null;
            DispatchModel actual;
            actual = target.Get(did);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Get 的测试
        ///</summary>
        [TestMethod()]
        public void GetTest1()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            string deliveryNo = string.Empty;
            DispatchModel expected = null;
            DispatchModel actual;
            actual = target.Get(deliveryNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ExportReport 的测试
        ///</summary>
        [TestMethod()]
        public void ExportReportTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            DispatchSearchModel searchModel = null;
            DataTable expected = null;
            DataTable actual;
            actual = target.ExportReport(searchModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///DispatchBoxList 的测试
        ///</summary>
        [TestMethod()]
        public void DispatchBoxListTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            int DepartureID = 0;
            string[] disPatchedBox = null;
            List<DispatchDetailModel> expected = null;
            List<DispatchDetailModel> actual;
            actual = target.DispatchBoxList(DepartureID, disPatchedBox);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            long did = 0;
            int expected = 0;
            int actual;
            actual = target.Delete(did);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteTest1()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            string DeliveryNo = string.Empty;
            int expected = 0;
            int actual;
            actual = target.Delete(DeliveryNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IDispatchDAL target = CreateIDispatchDAL();
            DispatchModel model = null;
            int expected = 0;
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
