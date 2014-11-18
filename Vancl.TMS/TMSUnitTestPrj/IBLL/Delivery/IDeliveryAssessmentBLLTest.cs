using Vancl.TMS.IBLL.Delivery.KPIAppraisal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.BLL.Delivery.KPIAppraisal;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IDeliveryAssessmentBLLTest 的测试类，旨在
    ///包含所有 IDeliveryAssessmentBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IDeliveryAssessmentBLLTest
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


        internal virtual IDeliveryAssessmentBLL CreateIDeliveryAssessmentBLL()
        {
            // TODO: 实例化相应的具体类。
            IDeliveryAssessmentBLL target = new DeliveryAssessmentBLL();
            return target;
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IDeliveryAssessmentBLL target = CreateIDeliveryAssessmentBLL(); // TODO: 初始化为适当的值
            string DeliveryNo = "LN201204160007"; // TODO: 初始化为适当的值
            ViewAssPriceModel model;
            model = target.SearchAssPrice(DeliveryNo);
            KPICalcInputModel inputModel = new KPICalcInputModel()
            {
                DeliveryNo = model.DeliveryNo,
                AssPriceList = model.AssPriceList,
                Discount = model.Discount,
                ExpressionType = model.ExpressionType,
                InsuranceRate = model.InsuranceRate,
                IsDelayAssess = model.IsDelayAssess,
                LongDeliveryAmount = model.LongDeliveryAmount,
                LongPickPrice = model.LongPickPrice,
                LongTransferRate = model.LongTransferRate,
                LostDeduction = model.LostDeduction
            }; // TODO: 初始化为适当的值
            // TODO: 初始化为适当的值
            ResultModel actual;
            actual = target.Add(inputModel);

            Assert.AreEqual(false, actual.IsSuccess);
        }

        /// <summary>
        ///Get 的测试
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            IDeliveryAssessmentBLL target = CreateIDeliveryAssessmentBLL(); // TODO: 初始化为适当的值
            string deliveryNo = "LN201204110006"; // TODO: 初始化为适当的值
            DeliveryAssessmentModel expected = null; // TODO: 初始化为适当的值
            DeliveryAssessmentModel actual;
            actual = target.Get(deliveryNo);
            Assert.AreEqual(deliveryNo, actual.DeliveryNo);
        }

        /// <summary>
        ///KPICalculate 的测试
        ///</summary>
        [TestMethod()]
        public void KPICalculateTest()
        {
            IDeliveryAssessmentBLL target = CreateIDeliveryAssessmentBLL(); // TODO: 初始化为适当的值
            KPICalcInputModel inputModel = new KPICalcInputModel()
                {
                    DeliveryNo = "LN201204110006"
                }; // TODO: 初始化为适当的值
            KPICalcOutputModel expected = null; // TODO: 初始化为适当的值
            KPICalcOutputModel actual;
            actual = null; // target.KPICalculate(inputModel);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Search 的测试
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            IDeliveryAssessmentBLL target = CreateIDeliveryAssessmentBLL(); // TODO: 初始化为适当的值
            DeliveryAssessmentSearchModel searchModel = null; // TODO: 初始化为适当的值
            PagedList<ViewDeliveryAssessmentModel> expected = null; // TODO: 初始化为适当的值
            PagedList<ViewDeliveryAssessmentModel> actual;
            actual = null;//target.Search(searchModel);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///SearchAssPrice 的测试
        ///</summary>
        [TestMethod()]
        public void SearchAssPriceTest()
        {
            IDeliveryAssessmentBLL target = CreateIDeliveryAssessmentBLL(); // TODO: 初始化为适当的值
            string DeliveryNo = "LN201204160007"; // TODO: 初始化为适当的值
            ViewAssPriceModel actual;
            actual = target.SearchAssPrice(DeliveryNo);
            Assert.AreEqual(Enums.ExpressionType.Formula, actual.ExpressionType);
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            IDeliveryAssessmentBLL target = CreateIDeliveryAssessmentBLL(); // TODO: 初始化为适当的值
            ViewAssPriceModel model;
            model = target.SearchAssPrice("LN201204160007");
            ResultModel actual;
            KPICalcInputModel inputModel = new KPICalcInputModel()
            {
                DeliveryNo = model.DeliveryNo,
                AssPriceList = model.AssPriceList,
                Discount = model.Discount,
                ExpressionType = model.ExpressionType,
                InsuranceRate = model.InsuranceRate,
                //IsDelayAssess = model.IsDelayAssess,
                LongDeliveryAmount = model.LongDeliveryAmount,
                LongPickPrice = model.LongPickPrice,
                LongTransferRate = model.LongTransferRate,
                LostDeduction = model.LostDeduction
            }; // TODO: 初始化为适当的值
            actual = target.Update(inputModel);
            Assert.AreEqual(true, actual.IsSuccess);
        }
    }
}
