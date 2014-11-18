using Vancl.TMS.IBLL.BaseInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.BaseInfo.Line;
using System.Collections.Generic;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 ILinePlanBLLTest 的测试类，旨在
    ///包含所有 ILinePlanBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class ILinePlanBLLTest
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


        internal virtual ILinePlanBLL CreateILinePlanBLL()
        {

            ILinePlanBLL target = ServiceFactory.GetService<ILinePlanBLL>();
            return target;
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            ILinePlanBLL target = CreateILinePlanBLL();  
            LinePlanModel model = null;  
            IList<ILinePrice> linePriceModel = null;  
            int expected = 0;  
            ResultModel actual;
            actual = target.Add(model, linePriceModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///AuditLinePlan 的测试
        ///</summary>
        [TestMethod()]
        public void AuditLinePlanTest()
        {
            ILinePlanBLL target = CreateILinePlanBLL();  
            int lpid = 0;  
            int expected = 0;  
            ResultModel actual;
            actual = target.AuditLinePlan(lpid,Enums.LineStatus.Approved,DateTime.Now);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Delete 的测试
        ///</summary>
        [TestMethod()]
        public void DeleteTest()
        {
            ILinePlanBLL target = CreateILinePlanBLL();  
            //string ids = string.Empty;  
            List<int> lpidList = new List<int>();
            int expected = 0;  
            ResultModel actual;
            actual = target.Delete(lpidList);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetLinePlan 的测试
        ///</summary>
        [TestMethod()]
        public void GetLinePlanTest()
        {
            ILinePlanBLL target = CreateILinePlanBLL();  
            LinePlanSearchModel searchModel = null;  
            PagedList<ViewLinePlanModel> expected = null;  
            PagedList<ViewLinePlanModel> actual;
            actual = target.GetLinePlan(searchModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetLinePlan 的测试
        ///</summary>
        [TestMethod()]
        public void GetLinePlanTest1()
        {
            ILinePlanBLL target = CreateILinePlanBLL();
            int lpid = 66;  
            LinePlanModel expected = null;  
            LinePlanModel actual;
            actual = target.GetLinePlan(lpid);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///GetLinePrice 的测试
        ///</summary>
        [TestMethod()]
        public void GetLinePriceTest()
        {
            ILinePlanBLL target = CreateILinePlanBLL();  
            int lpid = 0;  
            Enums.ExpressionType expressionType = new Enums.ExpressionType();  
            IList<ILinePrice> expected = null;  
            IList<ILinePrice> actual;
            actual = target.GetLinePrice(lpid, expressionType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///IsExsitLinePlan 的测试
        ///</summary>
        [TestMethod()]
        public void IsExsitLinePlanTest()
        {
            ILinePlanBLL target = CreateILinePlanBLL();  
            LinePlanModel linePlanModel = null;  
            bool expected = false;  
            bool actual;
            actual = target.IsExsitLinePlan(linePlanModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///Update 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateTest()
        {
            ILinePlanBLL target = CreateILinePlanBLL();  
            LinePlanModel model = null;  
            IList<ILinePrice> linePriceModel = null;  
            int expected = 0;  
            ResultModel actual;
            actual = target.Update(model, linePriceModel);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
