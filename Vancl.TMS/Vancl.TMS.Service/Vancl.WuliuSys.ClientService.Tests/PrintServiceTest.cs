using Vancl.WuliuSys.ClientLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Vancl.WuliuSys.ClientService.Tests
{
    
    
    /// <summary>
    ///这是 PrintServiceTest 的测试类，旨在
    ///包含所有 PrintServiceTest 单元测试
    ///</summary>
    [TestClass()]
    public class PrintServiceTest
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
        ///PrintWebPage 的测试
        ///</summary>
        [TestMethod()]
        public void PrintWebPageTest()
        {
            //PrintService target = new PrintService(); 
            //string url = "http://www.baidu.com";
            //bool expected = true;
            //bool actual;
            //actual = target.PrintWebPage(url);
            //Assert.AreEqual(expected, actual);
            //Thread.Sleep(100000);
        }

        /// <summary>
        ///Print 的测试
        ///</summary>
        [TestMethod()]
        public void PrintTest()
        {
            PrintService target = new PrintService(); 
        //    string url = @"http://localhost:14914/Sorting/print/billprint";
            string url = @"http://www.baidu.com";
       //     ReturnModel expected = null; // TODO: 初始化为适当的值
            PrintModel actual;
            actual = target.Print(url);
         //   Console.Write(actual.ToString());
          //  Assert.AreEqual(expected, actual);
          //  Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
