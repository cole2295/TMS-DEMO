using Vancl.TMS.IDAL.Delivery.KPIAppraisal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using System.Collections.Generic;
using Vancl.TMS.DAL.Oracle.Delivery.KPIAppraisal;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IAssFormulaPriceDALTest 的测试类，旨在
    ///包含所有 IAssFormulaPriceDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class IAssFormulaPriceDALTest
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


        internal virtual IAssFormulaPriceDAL CreateIAssFormulaPriceDAL()
        {
            // TODO: 实例化相应的具体类。
            IAssFormulaPriceDAL target = new AssFormulaPriceDAL();
            return target;
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IAssFormulaPriceDAL target = CreateIAssFormulaPriceDAL(); // TODO: 初始化为适当的值
            AssFormulaPriceModel model = new AssFormulaPriceModel() 
            {
                DeliveryNo=DateTime.Now.ToString("yyyyMMddHHmmssms"),
                BasePrice=18,
                BaseWeight=80,
                Note="unit test"                
            }; // TODO: 初始化为适当的值
            int expected = 1; // TODO: 初始化为适当的值
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
           
        }

        /// <summary>
        ///AddOverPriceDetail 的测试
        ///</summary>
        [TestMethod()]
        public void AddOverPriceDetailTest()
        {
            IAssFormulaPriceDAL target = CreateIAssFormulaPriceDAL(); // TODO: 初始化为适当的值
            List<AssFormulaPriceExModel> detailModel = new List<AssFormulaPriceExModel>(); // TODO: 初始化为适当的值
            detailModel.AddRange(new AssFormulaPriceExModel[]
            {
                new AssFormulaPriceExModel(){ DeliveryNo="201204111750285028",StartWeight=18,EndWeight=50,Price=100},
                new AssFormulaPriceExModel(){ DeliveryNo="201204111750285028",StartWeight=50,EndWeight=100,Price=120},
                new AssFormulaPriceExModel(){ DeliveryNo="201204111750285028",StartWeight=100,EndWeight=200,Price=150},
                new AssFormulaPriceExModel(){ DeliveryNo="201204111750285028",StartWeight=200,Price=180}
            });
            int expected = 4; // TODO: 初始化为适当的值
            int actual;
            actual = target.AddOverPriceDetail(detailModel);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///DeleteOverPriceDetail 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteOverPriceDetailTest()
        {
            IAssFormulaPriceDAL target = CreateIAssFormulaPriceDAL(); // TODO: 初始化为适当的值
            string DeliveryNo = "201204111750285028"; // TODO: 初始化为适当的值
            int expected = 4; // TODO: 初始化为适当的值
            int actual;
            actual = target.DeleteOverPriceDetail(DeliveryNo);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///Get 的测试
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            IAssFormulaPriceDAL target = CreateIAssFormulaPriceDAL(); // TODO: 初始化为适当的值
            string deliveryNo = "201204111750285028"; // TODO: 初始化为适当的值
            AssFormulaPriceModel expected = null; // TODO: 初始化为适当的值
            AssFormulaPriceModel actual;
            actual = target.Get(deliveryNo);
            Assert.AreEqual<int>(4, actual.Detail.Count);
           
        }

        /// <summary>
        ///GetOverPriceDetail 的测试
        ///</summary>
        [TestMethod()]
        public void GetOverPriceDetailTest()
        {
            IAssFormulaPriceDAL target = CreateIAssFormulaPriceDAL(); // TODO: 初始化为适当的值
            string DeliveryNo = string.Empty; // TODO: 初始化为适当的值
            List<AssFormulaPriceExModel> expected = null; // TODO: 初始化为适当的值
            List<AssFormulaPriceExModel> actual;
            actual = target.GetOverPriceDetail(DeliveryNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///IsExist 的测试
        ///</summary>
        [TestMethod()]
        public void IsExistTest()
        {
            IAssFormulaPriceDAL target = CreateIAssFormulaPriceDAL(); // TODO: 初始化为适当的值
            string deliveryNo = string.Empty; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.IsExist(deliveryNo);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            IAssFormulaPriceDAL target = CreateIAssFormulaPriceDAL(); // TODO: 初始化为适当的值
            AssFormulaPriceModel model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Update(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
