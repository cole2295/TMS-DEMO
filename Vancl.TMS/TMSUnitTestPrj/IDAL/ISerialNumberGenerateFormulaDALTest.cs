using Vancl.TMS.IDAL.Formula.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.DAL.Oracle.Formula.Common;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 ISerialNumberGenerateFormulaDALTest 的测试类，旨在
    ///包含所有 ISerialNumberGenerateFormulaDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class ISerialNumberGenerateFormulaDALTest
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


        internal virtual ISerialNumberGenerateFormulaDAL CreateISerialNumberGenerateFormulaDAL()
        {
             
            ISerialNumberGenerateFormulaDAL target = new SerialNumberGenerateFormulaDAL();
            return target;
        }

        /// <summary>
        ///GetNextNumber 的测试
        ///</summary>
        [TestMethod()]
        public void GetNextNumberTest()
        {
            ISerialNumberGenerateFormulaDAL target = CreateISerialNumberGenerateFormulaDAL();  
            string numberType = string.Empty;  
            string expected = string.Empty;  
            string actual;
            actual = target.GetNextNumber(numberType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetUserDefinedSeqNo 的测试
        ///</summary>
        [TestMethod()]
        public void GetUserDefinedSeqNoTest()
        {
            ISerialNumberGenerateFormulaDAL target = CreateISerialNumberGenerateFormulaDAL();  
            Enums.SerialNumberType SeqNoType = Enums.SerialNumberType.DeliveryNo;  
            int expected = 3;  
            int actual;
            actual = target.GetUserDefinedSeqNo(SeqNoType);
            Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
