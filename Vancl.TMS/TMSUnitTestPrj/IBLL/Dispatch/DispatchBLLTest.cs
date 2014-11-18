using Vancl.TMS.BLL.Transport.Dispatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Transport.Dispatch;
using System.Data;
using System.Collections.Generic;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;

namespace TMSUnitTestPrj
{


    /// <summary>
    ///这是 DispatchBLLTest 的测试类，旨在
    ///包含所有 DispatchBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class DispatchBLLTest
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
        ///DispatchBLL 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void DispatchBLLConstructorTest()
        {
            DispatchBLL target = new DispatchBLL();
            Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///ComfirmUnplannedDispatch 的测试
        ///</summary>
        [TestMethod()]
        public void ComfirmUnplannedDispatchTest()
        {
            DispatchBLL target = new DispatchBLL();
            string DeliveryNo = string.Empty;
            string waybillno = string.Empty;
            int LPID = 0;
            string[] disPatchedBox = null;
            string strNoticeInfo = string.Empty;
            string strNoticeInfoExpected = string.Empty;
            ResultModel expected = null;
            ResultModel actual;
            actual = target.ComfirmUnplannedDispatch(DeliveryNo, waybillno, LPID, disPatchedBox);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");

        }

        /// <summary>
        ///ConfirmDispatch 的测试
        ///</summary>
        [TestMethod()]
        public void ConfirmDispatchTest()
        {
            DispatchBLL target = new DispatchBLL();
            string DeliveryNo = DateTime.Now.ToString("yyyyMMddHHmmssms");
            string waybillno = String.Format("WW{0}", DateTime.Now.ToString("yyyyMMddHHmmssms"));
            int LPID = 2;
            string[] disPatchedBox = new string[] { "ABC_00005", "ABC_00006" };
            string strNoticeInfo = string.Empty;
            string strNoticeInfoExpected = string.Empty;
            bool expected = false;
            bool actual;
            // actual = target.ConfirmDispatch(DeliveryNo, waybillno, LPID, disPatchedBox, out strNoticeInfo);

            //Assert.AreEqual(strNoticeInfoExpected, strNoticeInfo);
            //   Assert.AreEqual(expected, actual);
            // Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ExportReport 的测试
        ///</summary>
        [TestMethod()]
        public void ExportReportTest()
        {
            DispatchBLL target = new DispatchBLL();
            DispatchSearchModel searchModel = null;
            DataTable expected = null;
            DataTable actual;
            actual = target.ExportReport(searchModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Get 的测试
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            DispatchBLL target = new DispatchBLL();
            string deliveryNo = string.Empty;
            DispatchModel expected = null;
            DispatchModel actual;
            actual = target.Get(deliveryNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Get 的测试
        ///</summary>
        [TestMethod()]
        public void GetTest1()
        {
            DispatchBLL target = new DispatchBLL();
            long did = 0;
            DispatchModel expected = null;
            DispatchModel actual;
            actual = target.Get(did);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDispOrderDetailModelList 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Vancl.TMS.BLL.dll")]
        public void GetDispOrderDetailModelListTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///GetDispatchDetailModelList 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Vancl.TMS.BLL.dll")]
        public void GetDispatchDetailModelListTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///GetDispatchModel 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Vancl.TMS.BLL.dll")]
        public void GetDispatchModelTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///GetStatisticInfo 的测试
        ///</summary>
        [TestMethod()]
        public void GetStatisticInfoTest()
        {
            DispatchBLL target = new DispatchBLL();
            DispatchSearchModel searchModel = null;
            ViewDispatchStatisticModel expected = null;
            ViewDispatchStatisticModel actual;
            searchModel = new DispatchSearchModel();
            searchModel.DepartureID = 6;
            actual = target.GetStatisticInfo(searchModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetValidBoxList 的测试
        ///</summary>
        [TestMethod()]
        public void GetValidBoxListTest()
        {
            DispatchBLL target = new DispatchBLL();
            int DepartureID = 2;
            string[] disPatchedBox = new string[] { "ABC_00003" }
                ;
            List<ViewDispatchBoxModel> expected = null;
            List<ViewDispatchBoxModel> actual=null;
       //     actual = target.GetValidBoxList(DepartureID, disPatchedBox, "", "");
            //Assert.AreEqual(expected, actual);
            Assert.AreEqual<int>(1, actual.Count);
            // Assert.("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetWaybillModel 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Vancl.TMS.BLL.dll")]
        public void GetWaybillModelTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///ReBuildDispatch 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Vancl.TMS.BLL.dll")]
        public void ReBuildDispatchTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///ReBuildDispatchDetail 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Vancl.TMS.BLL.dll")]
        public void ReBuildDispatchDetailTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///RejectDispatch 的测试
        ///</summary>
        [TestMethod()]
        public void RejectDispatchTest()
        {
            DispatchBLL target = new DispatchBLL();
            string DeliveryNo = "20120302113305335";
            string strNoticeInfo = string.Empty;
            string strNoticeInfoExpected = string.Empty;
            bool expected = true;
            bool actual;
            //     actual = target.RejectDispatch(DeliveryNo, out strNoticeInfo);
            //Assert.AreEqual(strNoticeInfoExpected, strNoticeInfo);
            //    Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Search 的测试
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            DispatchBLL target = new DispatchBLL();
            DispatchSearchModel searchModel = null;
            PagedList<ViewDispatchModel> expected = null;
            PagedList<ViewDispatchModel> actual;
            actual = target.Search(searchModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///SplitDispatchDetailByPreDispatch 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Vancl.TMS.BLL.dll")]
        public void SplitDispatchDetailByPreDispatchTest()
        {
            // 为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败
            Assert.Inconclusive("为“Microsoft.VisualStudio.TestTools.TypesAndSymbols.Assembly”创建专用访问器失败");
        }

        /// <summary>
        ///UpdateDeliveryStatus 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateDeliveryStatusTest()
        {
            DispatchBLL target = new DispatchBLL();
            string deliveryNO = string.Empty;
            Enums.DeliveryStatus deliveryStatus = new Enums.DeliveryStatus();
            int expected = 0;
            var actual = target.UpdateDeliveryStatus(deliveryNO, deliveryStatus);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
