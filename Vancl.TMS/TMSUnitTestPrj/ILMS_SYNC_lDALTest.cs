using Vancl.TMS.IDAL.LMS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Core.ServiceFactory;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IWaybillDALTest 的测试类，旨在
    ///包含所有 IWaybillDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class ILMS_SYNC_lDALTest
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


        internal virtual IWaybillDAL CreateIWaybillDAL()
        {
            // TODO: 实例化相应的具体类。
            //IWaybillDAL target = ServiceFactory.GetService<IWaybillDAL>("LMSWaybillDAL_Oracle");
            IWaybillDAL target = ServiceFactory.GetService<IWaybillDAL>("LMSWaybillDAL_SQL");
            return target;
        }

        internal virtual IInboundDAL CreateIInboundDAL()
        { 
            IInboundDAL target = ServiceFactory.GetService<IInboundDAL>("LMSInboundDAL_SQL");
            //IInboundDAL target = ServiceFactory.GetService<IInboundDAL>("LMSInboundDAL_Oracle");
            return target;
        }

        internal virtual IOperateLogDAL CreateIOperateLogDAL()
        {
            //IOperateLogDAL target = ServiceFactory.GetService<IOperateLogDAL>("LMSOperateLogDAL_SQL");
            IOperateLogDAL target = ServiceFactory.GetService<IOperateLogDAL>("LMSOperateLogDAL_Oracle");
            return target;
        }

        internal virtual IWaybill_SortCenterDAL CreateIWaybill_SortCenterDAL()
        {
            //IWaybill_SortCenterDAL target = ServiceFactory.GetService<IWaybill_SortCenterDAL>("LMSWaybill_SortCenterDAL_SQL");
            IWaybill_SortCenterDAL target = ServiceFactory.GetService<IWaybill_SortCenterDAL>("LMSWaybill_SortCenterDAL_Oracle");
            return target;
        }

        internal virtual IWaybillStatusChangeLogDAL CreateIWaybillStatusChangeLogDAL()
        {
            //IWaybillStatusChangeLogDAL target = ServiceFactory.GetService<IWaybillStatusChangeLogDAL>("LMSWaybillStatusChangeLogDAL_SQL");
            IWaybillStatusChangeLogDAL target = ServiceFactory.GetService<IWaybillStatusChangeLogDAL>("LMSWaybillStatusChangeLogDAL_Oracle");
            return target;
        }

        [TestMethod()]
        public void ChangeStatusLogTest()
        {
            IWaybillStatusChangeLogDAL target = CreateIWaybillStatusChangeLogDAL();
            var model = new WaybillStatusChangeLogEntityModel() 
            {
                CreateBy = 1,
                CreateDept = 2,
                CreateTime = DateTime.Now,
                CurNode = Vancl.TMS.Model.Common.Enums.StatusChangeNodeType.DeliverCenter,
                CustomerOrder = "11208150004976",
                DeliverStationID = 2,
                DistributionCode = "rfd",
                IsSyn = false,
                IsM2sSyn = "0",
                LMS_WaybillStatusChangeLogKid = "ST000300000000000007",
                MerchantID = 26,
                Note = "dddddddddddddddtestfsdafasdf",
                OperateType = Vancl.TMS.Model.Common.Enums.Lms2TmsOperateType.Inbound,
                Status = Vancl.TMS.Model.Common.Enums.BillStatus.HaveBeenSorting,
                SubStatus = (int)Vancl.TMS.Model.Common.Enums.BillStatus.HaveBeenSorting,
                TmsSyncStatus = Vancl.TMS.Model.Common.Enums.SyncStatus.Already,
                UpdateBy = 1,
                UpdateTime = DateTime.Now,
                WaybillNO = 11208150004976
            };
            int expected = 1;
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void OperateLogTest()
        {
            IOperateLogDAL target = CreateIOperateLogDAL();
            var model = new OperateLogEntityModel()
            {
                CreateBy = 1,
                CreateTime = DateTime.Now,
                IsSyn = 0,
                LogOperator = "FDFD",
                LogType = 0,
                OperateLogKid = "ST000700000000000007",
                OperateTime = DateTime.Now,
                Operation = "入库",
                OperatorStation = 2,
                Result = "入库logtest",
                Status = Vancl.TMS.Model.Common.Enums.BillStatus.HaveBeenSorting,
                UpdateBy = 1,
                UpdateTime = DateTime.Now,
                WaybillNO = 11208150004458
            };
            int expected = 1;
            int actual;
            actual = target.Add(model);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void Waybill_SortCenterTest()
        {
            IWaybill_SortCenterDAL target = CreateIWaybill_SortCenterDAL();
            var model = new Waybill_SortCenterEntityModel() {
                WaybillNO = 11208150004458,
                InBoundKid = "ST000100000000000007",
                UpdateBy = 1
            };
            int expected = 1;
            int actual = -13;
            actual = target.Merge(model);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void InboudAddTest()
        {
            IInboundDAL target = CreateIInboundDAL();
            var model = new InboundEntityModel() 
            {
                WaybillNO = 1234567890 ,
                CreateBy = 1,
                CreateDept = 2,
                CreateTime = DateTime.Now,
                CurOperator = 1,
                CustomerBatchNO = "xxxxx",
                DeliveryDriver = 1,
                DeliveryMan = 1,
                FromStation = 66,
                InBoundKid = "ST000100000000000008",
                IntoStation = 2,
                IntoStationType = Vancl.TMS.Model.Common.Enums.SortCenterOperateType.SecondSorting,
                IntoTime = DateTime.Now,
                ToStation = 9,
                UpdateBy = 1,
                UpdateDept = 2,
                UpdateTime = DateTime.Now
            };
            long expected = 0;
            long actual = -1;
            actual = target.Add(model);
            Assert.Equals(expected, actual);
        }

        //

        /// <summary>
        ///UpdateWaybillModelByInbound 的测试
        ///</summary>
        [TestMethod()]
        public void UpdateWaybillModelByInboundTest()
        {
            IWaybillDAL target = CreateIWaybillDAL(); // TODO: 初始化为适当的值
            WaybillEntityModel waybillModel = new WaybillEntityModel() 
            {
                WaybillNo = 9876543210989,
                InboundID = 22444422,
                OutboundID = null,
                Status = Vancl.TMS.Model.Common.Enums.BillStatus.HaveBeenSorting,
                UpdateBy = 1,
                UpdateDept = 20,
                UpdateTime = DateTime.Now
            }; // TODO: 初始化为适当的值
            int expected = 1; // TODO: 初始化为适当的值
            int actual;
            actual = target.UpdateWaybillModelByInbound(waybillModel);
            Assert.AreEqual(expected, actual);
        }
    }
}
