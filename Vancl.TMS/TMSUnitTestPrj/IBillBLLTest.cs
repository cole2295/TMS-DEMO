using Vancl.TMS.IBLL.BaseInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Synchronous;
using System.Collections.Generic;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.IBLL.Sorting.Outbound;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IBillBLLTest 的测试类，旨在
    ///包含所有 IBillBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IBillBLLTest
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


        internal virtual IBillBLL CreateIBillBLL()
        {
            // TODO: 实例化相应的具体类。
            IBillBLL target = ServiceFactory.GetService<IBillBLL>() as IBillBLL; ;
            return target;
        }

        /// <summary>
        ///GetBillStatus 的测试
        ///</summary>
        [TestMethod()]
        public void GetReturnStatusTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = "3232"; // TODO: 初始化为适当的值
            Nullable<Enums.ReturnStatus> expected = new Nullable<Enums.ReturnStatus>(); // TODO: 初始化为适当的值
            Nullable<Enums.ReturnStatus> actual;
            actual = target.GetBillReturnStatus(FormCode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetBillStatus 的测试
        ///</summary>
        [TestMethod()]
        public void GetBillStatusTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = "3232"; // TODO: 初始化为适当的值
            Nullable<Enums.BillStatus> expected = new Nullable<Enums.BillStatus>(); // TODO: 初始化为适当的值
            Nullable<Enums.BillStatus> actual;
            actual = target.GetBillStatus(FormCode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Lms2Tms_OutboundUpdateBill 的测试
        ///</summary>
        [TestMethod()]
        public void Lms2Tms_OutboundUpdateBillTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            BillModel billmodel = new BillModel() {
                CurrentDistributionCode="rfd",
                FormCode="rrere",
                Status = Enums.BillStatus.Outbounded
            }; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Lms2Tms_OutboundUpdateBill(billmodel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///UpdateBillStatus 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateBillStatusTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode ="3232@re"; // TODO: 初始化为适当的值
            Enums.BillStatus status = new Enums.BillStatus(); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateBillStatus(FormCode, status);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///UpdateBillReturnStatus 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateBillReturnStatusTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode ="4343"; // TODO: 初始化为适当的值
            Enums.ReturnStatus status = new Enums.ReturnStatus(); // TODO: 初始化为适当的值
            int expected = 10; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateBillReturnStatus(FormCode, status);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetBillForComparing 的测试
        ///</summary>
        public void GetBillForComparingTestHelper<T>()
            where T : IBillLms2TmsForComparing, new()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string formCode = string.Empty; // TODO: 初始化为适当的值
            T expected = new T(); // TODO: 初始化为适当的值
            T actual;
            actual = target.GetBillForComparing<T>(formCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        [TestMethod()]
        public void GetBillForComparingTest()
        {
            Assert.Inconclusive("没有找到能够满足 T 的类型约束的相应类型参数。请以适当的类型参数来调用 GetBillForComparingTestHelper<T>()。");
        }

        /// <summary>
        ///GetBillReturnStatus 的测试
        ///</summary>
        [TestMethod()]
        public void GetBillReturnStatusTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = string.Empty; // TODO: 初始化为适当的值
            Nullable<Enums.ReturnStatus> expected = new Nullable<Enums.ReturnStatus>(); // TODO: 初始化为适当的值
            Nullable<Enums.ReturnStatus> actual;
            actual = target.GetBillReturnStatus(FormCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetBillStatus 的测试
        ///</summary>
        [TestMethod()]
        public void GetBillStatusTest1()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = string.Empty; // TODO: 初始化为适当的值
            Nullable<Enums.BillStatus> expected = new Nullable<Enums.BillStatus>(); // TODO: 初始化为适当的值
            Nullable<Enums.BillStatus> actual;
            actual = target.GetBillStatus(FormCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetFormCodeByCustomerOrder 的测试
        ///</summary>
        [TestMethod()]
        public void GetFormCodeByCustomerOrderTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string CustomerOrder = string.Empty; // TODO: 初始化为适当的值
            List<string> expected = null; // TODO: 初始化为适当的值
            List<string> actual;
            actual = target.GetFormCodeByCustomerOrder(CustomerOrder);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetInboundBillModel 的测试
        ///</summary>
        [TestMethod()]
        public void GetInboundBillModelTest()
        {
            Enums.ReturnStatus? Status;
            Status = (Enums.ReturnStatus)Enum.Parse(typeof(Enums.ReturnStatus), "0");
            Assert.AreEqual(Enums.ReturnStatus.RejectedInbounded, Status);



            //IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            //string FormCode = "11212040000018"; // TODO: 初始化为适当的值
            //InboundBillModel expected = null; // TODO: 初始化为适当的值
            //InboundBillModel actual;
            //actual = target.GetInboundBillModel(FormCode);
            //Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetInboundBillModel_ByQueueHandled 的测试
        ///</summary>
        [TestMethod()]
        public void GetInboundBillModel_ByQueueHandledTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = "11208150000598";// TODO: 初始化为适当的值
            InboundBillModel expected = null; // TODO: 初始化为适当的值
            InboundBillModel actual;
            actual = target.GetInboundBillModel_ByQueueHandled(FormCode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetInboundBillModel_BySMSQueueHandled 的测试
        ///</summary>
        [TestMethod()]
        public void GetInboundBillModel_BySMSQueueHandledTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = "11208150000598"; // TODO: 初始化为适当的值
            InboundBillModel expected = null; // TODO: 初始化为适当的值
            InboundBillModel actual;
            actual = target.GetInboundBillModel_BySMSQueueHandled(FormCode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetMerchantFormCodeRelation 的测试
        ///</summary>
        [TestMethod()]
        public void GetMerchantFormCodeRelationTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            Enums.SortCenterFormType formType =  Enums.SortCenterFormType.Order; // TODO: 初始化为适当的值
            string code = "121111080"; // TODO: 初始化为适当的值
            List<MerchantFormCodeRelationModel> expected = null; // TODO: 初始化为适当的值
            List<MerchantFormCodeRelationModel> actual;
            actual = target.GetMerchantFormCodeRelation(formType, code);
            Assert.AreEqual(1, actual.Count);
        }

        /// <summary>
        ///GetTurnInboundBillModel 的测试
        ///</summary>
        [TestMethod()]
        public void GetTurnInboundBillModelTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = "11208150000598"; // TODO: 初始化为适当的值
            InboundBillModel expected = null; // TODO: 初始化为适当的值
            InboundBillModel actual;
            actual = target.GetInboundBillModel_TurnStation(FormCode);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Lms2Tms_AssignStationUpdateBill 的测试
        ///</summary>
        [TestMethod()]
        public void Lms2Tms_AssignStationUpdateBillTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            BillModel billmodel = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Lms2Tms_AssignStationUpdateBill(billmodel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Lms2Tms_OutboundUpdateBill 的测试
        ///</summary>
        [TestMethod()]
        public void Lms2Tms_OutboundUpdateBillTest1()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            BillModel billmodel = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Lms2Tms_OutboundUpdateBill(billmodel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Lms2Tms_TurnStationApplyUpdateBill 的测试
        ///</summary>
        [TestMethod()]
        public void Lms2Tms_TurnStationApplyUpdateBillTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            BillModel billmodel = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Lms2Tms_TurnStationApplyUpdateBill(billmodel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateBillReturnStatus 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateBillReturnStatusTest1()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = string.Empty; // TODO: 初始化为适当的值
            Enums.ReturnStatus status = new Enums.ReturnStatus(); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateBillReturnStatus(FormCode, status);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateBillStatus 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateBillStatusTest1()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = string.Empty; // TODO: 初始化为适当的值
            Enums.BillStatus status = new Enums.BillStatus(); // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateBillStatus(FormCode, status);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///UpdateBillModelByOutbound 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateBillModelByOutboundTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            OutboundBillModel billModel = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.UpdateBillModelByOutbound(billModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetOutboundBillModel 的测试
        ///</summary>
        [TestMethod()]
        public void GetOutboundBillModelTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            string FormCode = string.Empty; // TODO: 初始化为适当的值
            OutboundBillModel expected = null; // TODO: 初始化为适当的值
            OutboundBillModel actual;
            actual = target.GetOutboundBillModel(FormCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetOutboundBillModel_BatchOutbound 的测试
        ///</summary>
        [TestMethod()]
        public void GetOutboundBillModel_BatchOutboundTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            List<string> FormCode = null; // TODO: 初始化为适当的值
            List<OutboundBillModel> expected = null; // TODO: 初始化为适当的值
            List<OutboundBillModel> actual;
            actual = target.GetOutboundBillModel_BatchOutbound(FormCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetOutboundBillModel_PackingOutbound 的测试
        ///</summary>
        [TestMethod()]
        public void GetOutboundBillModel_PackingOutboundTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            IOutboundArgModel outboundArg = null; // TODO: 初始化为适当的值
            List<string> FormCode = null; // TODO: 初始化为适当的值
            List<OutboundBillModel> expected = null; // TODO: 初始化为适当的值
            List<OutboundBillModel> actual;
            actual = target.GetOutboundBillModel_PackingOutbound(outboundArg, FormCode);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetOutboundBillModel_SearchOutbound 的测试
        ///</summary>
        [TestMethod()]
        public void GetOutboundBillModel_SearchOutboundTest()
        {
            IBillBLL target = CreateIBillBLL(); // TODO: 初始化为适当的值
            IOutboundBLL Obtarget = ServiceFactory.GetService<IOutboundBLL>("SC_OutboundBLL"); ;
            IOutboundArgModel outboundArg = new OutboundSearchArgModel()
            {
                OpUser = Obtarget.GetUserModel(1),
                PreCondition = Obtarget.GetPreCondition("rfd"),
                ToStation = Obtarget.GetToStationModel(857)
            }; // TODO: 初始化为适当的值
            List<string> FormCode = new List<string>(); // TODO: 初始化为适当的值
            FormCode.Add("13211111570");



            List<OutboundBillModel> expected = null; // TODO: 初始化为适当的值
            List<OutboundBillModel> actual;
            actual = target.GetOutboundBillModel_SearchOutbound(outboundArg, FormCode);
            Assert.AreEqual(expected, actual);
        }
    }
}
