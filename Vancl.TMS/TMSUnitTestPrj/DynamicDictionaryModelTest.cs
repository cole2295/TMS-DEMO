using Vancl.TMS.Model.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Dynamic;

namespace TMSUnitTestPrj
{


    /// <summary>
    ///这是 DynamicDictionaryModelTest 的测试类，旨在
    ///包含所有 DynamicDictionaryModelTest 单元测试
    ///</summary>
    [TestClass()]
    public class DynamicDictionaryModelTest
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
        ///DynamicDictionaryModel 构造函数 的测试
        ///</summary>
        [TestMethod()]
        public void DynamicDictionaryModelConstructorTest()
        {
            dynamic d = new DynamicDictionaryModel();
            d.abc = "aaaaaa";
            Console.WriteLine(d.abc);
        }

        /// <summary>
        ///TryGetMember 的测试
        ///</summary>
        [TestMethod()]
        public void TryGetMemberTest()
        {
            DynamicDictionaryModel target = new DynamicDictionaryModel(); // TODO: 初始化为适当的值
            GetMemberBinder binder = null; // TODO: 初始化为适当的值
            object result = null; // TODO: 初始化为适当的值
            object resultExpected = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.TryGetMember(binder, out result);
            Assert.AreEqual(resultExpected, result);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///TrySetMember 的测试
        ///</summary>
        [TestMethod()]
        public void TrySetMemberTest()
        {
            DynamicDictionaryModel target = new DynamicDictionaryModel(); // TODO: 初始化为适当的值
            SetMemberBinder binder = null; // TODO: 初始化为适当的值
            object value = null; // TODO: 初始化为适当的值
            bool expected = false; // TODO: 初始化为适当的值
            bool actual;
            actual = target.TrySetMember(binder, value);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
