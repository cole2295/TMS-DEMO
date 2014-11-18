using Vancl.TMS.Core.FormulaManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Outbound.SMS;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///这是 IFormulaTest 的测试类，旨在
    ///包含所有 IFormulaTest 单元测试
    ///</summary>
    [TestClass()]
    public class IFormulaTest
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
        ///Execute 的测试
        ///</summary>
        public void ExecuteTestHelper<T, M>()
        {
            IFormula<T, M> target = CreateIFormula<T, M>(); // TODO: 初始化为适当的值
            M context = default(M); // TODO: 初始化为适当的值
            T expected = default(T); // TODO: 初始化为适当的值
            T actual;
            actual = target.Execute(context);
            Assert.AreEqual(expected, actual);
        }

        internal virtual IFormula<T, M> CreateIFormula<T, M>()
        {
            // TODO: 实例化相应的具体类。
            IFormula<T, M> target = null;
            return target;
        }


        [TestMethod()]
        public void ExecuteOutboundSMSTest()
        {
            IFormula<OutboundSMSGetContentResult, OutboundSMSContext> smsformula = FormulasFactory.GetFormula<IFormula<OutboundSMSGetContentResult, OutboundSMSContext>>("OutboundSMSFormula");
            //OutboundSMSFormula
            var context = new OutboundSMSContext()
                {
                    BillModel = new OutboundSMSBillModel()
                    {
                        FormCode = "9120512848741",
                        MerchantID = 2,
                        OpType = Enums.SortCenterOperateType.DistributionSorting,
                        Source = Enums.BillSource.Others,
                        ArrivalID = 826,
                        DepartureID = 2
                    },
                    SMSConfig = new OutboundSMSConfigModel()
                };
            context.SMSConfig.Detail = new System.Collections.Generic.List<OutboundSMSConfigModel.SortCenterOuboundDetailConfig>();
            context.SMSConfig.Detail.Add(new OutboundSMSConfigModel.SortCenterOuboundDetailConfig() 
            { 
                MerchantID = 2, 
                OpType = Enums.SortCenterOperateType.SecondSorting, 
                Source = Enums.BillSource.Others,
                Template = "您好，您的小米订单已从[当前城市]分拣中心发往[目标城市]分拣中心，详情请登录www.rufengda.com进行查询。谢谢。[如风达快递]" 
            });
            context.SMSConfig.Detail.Add(new OutboundSMSConfigModel.SortCenterOuboundDetailConfig()
            {
                MerchantID = 2,
                OpType = Enums.SortCenterOperateType.SimpleSorting,
                Source = Enums.BillSource.Others,
                Template = "您好，您的小米订单已从[当前城市]分拣中心发往[目标部门]正在中转，详情请登录www.rufengda.com进行查询。谢谢。[如风达快递]"
            });
            context.SMSConfig.Detail.Add(new OutboundSMSConfigModel.SortCenterOuboundDetailConfig()
            {
                MerchantID = 2,
                OpType = Enums.SortCenterOperateType.DistributionSorting,
                Source = Enums.BillSource.Others,
                Template = "您的小米订单已从[当前城市]分拣中心发往[目标部门]正在中转，订单详情请登录www.rufengda.com进行查询。谢谢。[如风达快递]"
            });
            OutboundSMSGetContentResult expected = null;
            OutboundSMSGetContentResult actual = smsformula.Execute(context);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void ExecuteTest()
        {
            IFormula<string, SerialNumberModel> outbountbatchgenerator = FormulasFactory.GetFormula<IFormula<string, SerialNumberModel>>("OutboundBatchNoGenerateFormula");
            String expected = "20121023000004";
            string actual = outbountbatchgenerator.Execute(new SerialNumberModel() { FillerCharacter = "0", NumberLength = 6 });
            Assert.AreEqual(expected, actual);
            //ExecuteTestHelper<GenericParameterHelper, GenericParameterHelper>();
        }
    }
}
