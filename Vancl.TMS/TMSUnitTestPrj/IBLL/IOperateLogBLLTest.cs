using Vancl.TMS.IBLL.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Log;
using System.Collections.Generic;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.BaseInfo;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.IBLL.Transport.Plan;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IOperateLogBLLTest 的测试类，旨在
    ///包含所有 IOperateLogBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IOperateLogBLLTest
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


        internal virtual IOperateLogBLL CreateIOperateLogBLL()
        {
            // TODO: 实例化相应的具体类。
            //IOperateLogBLL target = ServiceFactory.GetService<ICarrierBLL>("CarrierBLL") as IOperateLogBLL;
            //IOperateLogBLL target = ServiceFactory.GetService<ILinePlanBLL>() as IOperateLogBLL;
            IOperateLogBLL target = ServiceFactory.GetService<ITransportPlanBLL>() as IOperateLogBLL;
            return target;
        }

        /// <summary>
        ///Read 的测试
        ///</summary>
        [TestMethod()]
        public void ReadTest()
        {
            IOperateLogBLL target = CreateIOperateLogBLL(); // TODO: 初始化为适当的值
            BaseOperateLogSearchModel searchmodel = new BaseOperateLogSearchModel() { KeyValue = "189"}; // TODO: 初始化为适当的值
            List<OperateLogModel> expected = null; // TODO: 初始化为适当的值
            List<OperateLogModel> actual;
            actual = target.Read(searchmodel);
            Assert.AreEqual(expected, actual);
        }
    }
}
