﻿using Vancl.TMS.IBLL.Sorting.Inbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using System.Collections.Generic;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IInboundSMSQueueBLLTest 的测试类，旨在
    ///包含所有 IInboundSMSQueueBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IInboundSMSQueueBLLTest
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


        internal virtual IInboundSMSQueueBLL CreateIInboundSMSQueueBLL()
        {
            // TODO: 实例化相应的具体类。
            IInboundSMSQueueBLL target = ServiceFactory.GetService<IInboundSMSQueueBLL>("InboundSMSQueueBLL"); ;
            return target;
        }

        /// <summary>
        ///Add 的测试
        ///</summary>
        [TestMethod()]
        public void AddTest()
        {
            IInboundSMSQueueBLL target = CreateIInboundSMSQueueBLL(); // TODO: 初始化为适当的值
            InboundSMSQueueEntityModel model = null; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetInboundSMSQueue 的测试
        ///</summary>
        [TestMethod()]
        public void GetInboundSMSQueueTest()
        {
            IInboundSMSQueueBLL target = CreateIInboundSMSQueueBLL(); // TODO: 初始化为适当的值
            InboundSMSQueueJobArgModel argument = new InboundSMSQueueJobArgModel() {
                ModValue = 10,
                OpCount = 7,
                PerBatchCount = 300,
                Remaider = 0            
            }; // TODO: 初始化为适当的值
            List<InboundSMSQueueEntityModel> expected = null; // TODO: 初始化为适当的值
            List<InboundSMSQueueEntityModel> actual;
            actual = target.GetInboundSMSQueue(argument);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///UpdateOpCount 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateOpCountTest()
        {
            IInboundSMSQueueBLL target = CreateIInboundSMSQueueBLL(); // TODO: 初始化为适当的值
            long ID = 14; // TODO: 初始化为适当的值
            string ErrorInfo = "单元测试项"; // TODO: 初始化为适当的值
            int expected = 1; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateOpCount(ID, ErrorInfo);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///UpdateToError 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToErrorTest()
        {
            IInboundSMSQueueBLL target = CreateIInboundSMSQueueBLL(); // TODO: 初始化为适当的值
            long ID = 14; // TODO: 初始化为适当的值
            string ErrorInfo = "单元测试项,shiai"; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToError(ID, ErrorInfo);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///UpdateToHandled 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateToHandledTest()
        {
            IInboundSMSQueueBLL target = CreateIInboundSMSQueueBLL(); // TODO: 初始化为适当的值
            long ID = 15; // TODO: 初始化为适当的值
            int expected = 0; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateToHandled(ID);
            Assert.AreEqual(expected, actual);
        }
    }
}
