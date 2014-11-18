using Vancl.TMS.IBLL.BaseInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.BaseInfo.Carrier;
using System.Collections.Generic;
using Vancl.TMS.Util.Pager;

namespace TMSUnitTestPrj
{


    /// <summary>
    ///这是 ICarrierBLLTest 的测试类，旨在
    ///包含所有 ICarrierBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class ICarrierBLLTest
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


        internal virtual ICarrierBLL CreateICarrierBLL()
        {
            ICarrierBLL target = null;
            return target;
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            ICarrierBLL target = CreateICarrierBLL(); 
            CarrierModel carrier = null; 
            IList<DelayCriteriaModel> delayCriteriaList = null;  
            IList<CoverageModel> coverageList = null;  
            int expected = 0;  
            var actual = target.Add(carrier, delayCriteriaList, coverageList);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            ICarrierBLL target = CreateICarrierBLL();  
            string ids = string.Empty;  
            int expected = 0;  
            int actual;
            // actual = target.Delete(ids);
            //  Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Get 的测试
        ///</summary>
        [TestMethod()]
        public void GetTest()
        {
            ICarrierBLL target = CreateICarrierBLL();  
            int carrierID = 0;  
            CarrierModel expected = null;  
            CarrierModel actual;
            actual = target.Get(carrierID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetAll 的测试
        ///</summary>
        [TestMethod()]
        public void GetAllTest()
        {
            ICarrierBLL target = CreateICarrierBLL();  
            IList<CarrierModel> expected = null;  
            IList<CarrierModel> actual;
            actual = target.GetAll();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetCoverageByCarrierID 的测试
        ///</summary>
        [TestMethod()]
        public void GetCoverageByCarrierIDTest()
        {
            ICarrierBLL target = CreateICarrierBLL();  
            int carrierID = 0;  
            IList<CoverageModel> expected = null;  
            IList<CoverageModel> actual;
            actual = target.GetCoverageByCarrierID(carrierID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDelayCriteriaByCarrierID 的测试
        ///</summary>
        [TestMethod()]
        public void GetDelayCriteriaByCarrierIDTest()
        {
            ICarrierBLL target = CreateICarrierBLL();  
            int carrierID = 0;  
            IList<DelayCriteriaModel> expected = null;  
            IList<DelayCriteriaModel> actual;
            actual = target.GetDelayCriteriaByCarrierID(carrierID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///IsExistCarrier 的测试
        ///</summary>
        [TestMethod()]
        public void IsExistCarrierTest()
        {
            ICarrierBLL target = CreateICarrierBLL();  
            string name = string.Empty;  
            bool expected = false;  
            bool actual;
            actual = target.IsExistCarrier(name, 0);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Search 的测试
        ///</summary>
        [TestMethod()]
        public void SearchTest()
        {
            ICarrierBLL target = CreateICarrierBLL();  
            CarrierSearchModel searchModel = null;  
            PagedList<CarrierModel> expected = null;  
            PagedList<CarrierModel> actual;
            actual = target.Search(searchModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            ICarrierBLL target = CreateICarrierBLL();  
            CarrierModel carrier = null;  
            IList<DelayCriteriaModel> delayCriteriaList = null;  
            IList<CoverageModel> coverageList = null;  
            int expected = 0;  
            int actual;
            //  actual = target.Update(carrier, delayCriteriaList, coverageList);
            // Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
