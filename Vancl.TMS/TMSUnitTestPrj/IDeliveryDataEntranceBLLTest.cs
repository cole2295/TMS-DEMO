using Vancl.TMS.IBLL.Delivery.DataInteraction.Entrance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Delivery.DataInteraction.Entrance;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IDeliveryDataEntranceBLLTest 的测试类，旨在
    ///包含所有 IDeliveryDataEntranceBLLTest 单元测试
    ///</summary>
    [TestClass()]
    public class IDeliveryDataEntranceBLLTest
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


        internal virtual IDeliveryDataEntranceBLL CreateIDeliveryDataEntranceBLL()
        {
            // TODO: 实例化相应的具体类。
            IDeliveryDataEntranceBLL target = ServiceFactory.GetService<IDeliveryDataEntranceBLL>("DeliveryDataEntranceBLL"); ;
            return target;
        }

        /// <summary>
        ///DataEntrance 的测试
        ///</summary>
        [TestMethod()]
        public void DataEntranceTest()
        {
            IDeliveryDataEntranceBLL target = CreateIDeliveryDataEntranceBLL(); // TODO: 初始化为适当的值
            TMSEntranceModel entranceModel = new TMSEntranceModel()
            {
                Arrival =  "807",//"145",
                BatchNo = DateTime.Now.ToString("yyyyMMddHHmmssms"),
                ContentType = Enums.GoodsType.Normal,
                Departure = "2", //"0101",
                Source = Enums.TMSEntranceSource.SortingOutbound ,
                Detail = new System.Collections.Generic.List<BillDetailModel>(),
                Weight = 2347.4M,
                TotalCount = 50,
                TotalAmount = 0
            }; // TODO: 初始化为适当的值

            Random rm = new Random(405500080);
            for (int i = 0; i < 50; i++)
            {
                entranceModel.Detail.Add(new BillDetailModel()
                {
                    BillType = Enums.BillType.Exchange,
                    FormCode = rm.Next(100000000).ToString(),
                    Price = i*2,
                    GoodsType = Enums.GoodsType.Contraband,
                    CustomerOrder = rm.Next(100000000).ToString()
                });
            }

            ResultModel expected = null; // TODO: 初始化为适当的值
            ResultModel actual;
            actual = target.DataEntrance(entranceModel);
            Assert.AreEqual(expected, actual.Message);
        }
    }
}
