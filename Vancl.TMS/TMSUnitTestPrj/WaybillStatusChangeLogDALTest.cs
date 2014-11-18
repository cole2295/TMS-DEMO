using Vancl.TMS.DAL.Oracle.LMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.LMS;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///This is a test class for WaybillStatusChangeLogDALTest and is intended
    ///to contain all WaybillStatusChangeLogDALTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WaybillStatusChangeLogDALTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            WaybillStatusChangeLogDAL target = new WaybillStatusChangeLogDAL(); // TODO: Initialize to an appropriate value
            
            WaybillStatusChangeLogEntityModel model =new WaybillStatusChangeLogEntityModel(); // TODO: Initialize to an appropriate value
            model.WaybillNO = 101;
            model.CurNode =0;
            model.Status = Enums.BillStatus.InvalidBill;
            model.SubStatus = 0;
            model.MerchantID = 0;
            model.DistributionCode = "rfd";
            model.DeliverStationID = 0;
            model.CustomerOrder = "100";
            model.CreateBy =0;
            model.CreateTime =DateTime.Now;
            model.CreateDept = 0;
            model.IsSyn =false;
            model.OperateType = Enums.Lms2TmsOperateType.ReturnOutbound;
            model.TmsSyncStatus = Enums.SyncStatus.Already;
            model.Note = "";
            model.LMS_WaybillStatusChangeLogKid = "";

            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
