using Vancl.TMS.IDAL.Sorting.BillPrint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Sorting.BillPrint;
using System.Collections.Generic;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IDAL.BaseInfo;

namespace TMSUnitTestPrj
{


    /// <summary>
    ///这是 IBillPrintFieldDALTest 的测试类，旨在
    ///包含所有 IBillPrintFieldDALTest 单元测试
    ///</summary>
    [TestClass()]
    public class IBillPrintFieldDALTest
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


        internal virtual IBillPrintFieldDAL CreateIBillPrintFieldDAL()
        {
            // TODO: 实例化相应的具体类。
            IBillPrintFieldDAL target = ServiceFactory.GetService<IBillPrintFieldDAL>();
            return target;
        }

        [TestMethod()]
        public void squ()
        {
            IBoxDAL target = ServiceFactory.GetService<IBoxDAL>();
            for (int i = 0; i < 1000; i++)
            {
                var id = target.GetNextSequence("seq_tms_box_bid");
            }
        }
        /// <summary>
        ///GetBillPrintField 的测试
        ///</summary>
        [TestMethod()]
        public void GetBillPrintFieldTest()
        {
            IBillPrintFieldDAL target = CreateIBillPrintFieldDAL();
            var model = new BillPrintFieldModel
            {
                CreateBy = 0,
                CreateTime = DateTime.Now,
                UpdateBy = 0,
                UpdateTime = DateTime.Now,
                IsDeleted = false,
                Sort = 0,
                Remark = "remark",
                DefaultStyle = "defaultstyle",
                DefaultValue = "defaultvalue",
                FieldName = "fieldname",
                ShowName = "showname",
                FieldFormat = "adf",
                //    MerchantID = 0,
            };
            //     var id = target.Add(model);
            model = target.Get(3);
            model.FieldName = "new 123";
            target.Update(model);
            target.Delete(3);
            Assert.IsNotNull(model);
        }
    }
}
