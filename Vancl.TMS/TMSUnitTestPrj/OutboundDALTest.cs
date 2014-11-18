﻿using System.Linq;
using Vancl.TMS.DAL.Oracle.Sorting.Outbound;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Common;

namespace TMSUnitTestPrj
{
    
    
    /// <summary>
    ///This is a test class for OutboundDALTest and is intended
    ///to contain all OutboundDALTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OutboundDALTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetOutboundEntityByBatchNoList
        ///</summary>
        [TestMethod()]
        public void GetOutboundEntityByBatchNoListTest()
        {
            OutboundDAL target = new OutboundDAL(); // TODO: Initialize to an appropriate value
            IList<string> batchNoList = new List<string>(); // TODO: Initialize to an appropriate value
            batchNoList.Add("20121030100002");
            batchNoList.Add("20121031100001");
            IList<OutboundEntityModel> expected = null; // TODO: Initialize to an appropriate value
            IList<OutboundEntityModel> actual;
            actual = target.GetOutboundEntityByBatchNoList(batchNoList);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetOrderCount
        ///</summary>
        [TestMethod()]
        public void GetOrderCountTest()
        {
            OutboundDAL target = new OutboundDAL(); // TODO: Initialize to an appropriate value
            IList<string> batchNoList = new List<string>(); ; // TODO: Initialize to an appropriate value
            batchNoList.Add("20121030100002");
            batchNoList.Add("20121031100001");
            IList<OutboundOrderCountModel> expected = null; // TODO: Initialize to an appropriate value
            IList<OutboundOrderCountModel> actual;
            actual = target.GetOrderCount(batchNoList);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetOutboundPrintExportModel
        ///</summary>
        [TestMethod()]
        public void GetOutboundPrintExportModelTest()
        {
            OutboundDAL target = new OutboundDAL(); // TODO: Initialize to an appropriate value
            string batchNos = @"
20130609100006,20130609100007,20130609100008,20130609100009,20130609100010,20130609100011,20130609100012,20130609100013,20130609100014,20130609100015,20130609100016,20130609100018,20130609100019,20130609100020,20130609100021,20130609100025,20130609100164,20130609100166,20130609100167,20130609100168,20130609100169,20130609100170,20130609100172,20130609100173,20130609100174,20130609100175,20130609100176,20130609100177,20130609100178,20130609100179,20130609100182,20130609100183,20130609100184,20130609100186,20130609100187,20130609100188,20130609100189,20130609100190,20130609100192,20130609100194,20130609100195,20130609100198,20130609100199,20130609100200,20130609100203,20130609100204,20130609100205,20130609100207,20130609100208,20130609100209,20130609100210,20130609100211,20130609100212,20130609100213,20130609100214,20130609100215,20130609100216,20130609100217,20130609100218,20130609100219,20130609100220,20130609100221,20130609100222,20130609100223,20130609100224,20130609100225,20130609100226,20130609100227,20130609100228,20130609100229,20130609100230,20130609100231,20130609100232,20130609100233,20130609100235,20130609100237,20130609100238,20130609100240,20130609100241,20130609100242,20130609100243,20130609100244,20130609100245,20130609100246,20130609100247,20130609100248,20130609100249,20130609100250,20130609100251,20130609100252,20130609100253,20130609100254,20130609100255,20130609100256,20130609100257,20130609100258,20130609100259,20130609100260,20130609100261,20130609100262,20130609100263,20130609100264,20130609100265,20130609100266,20130609100267,20130609100268,20130609100269,20130609100270,20130609100271,20130609100272,20130609100273,20130609100274,20130609100275,20130609100276,20130609100277,20130609100278,20130609100279,20130609100281,20130609100282,20130609100284,20130609100288,20130609100289,20130609100290,20130609100291,20130609100292,20130609100293,20130609100294,20130609100295,20130609100298,20130609100299,20130609100300,20130609100301,20130609100302,20130609100304,20130609100305,20130609100306,20130609100307,20130609100308,20130609100309,20130609100310,20130609100312,20130609100313,20130609100314,20130609100315,20130609100316,20130609100317,20130609100318,20130609100319,20130609100320,20130609100321,20130609100322,20130609100323,20130609100324,20130609100325,20130609100326,20130609100327,20130609100328,20130609100329,20130609100330,20130609100331,20130609100332,20130609100333,20130609100334,20130609100335,20130609100336,20130609100337,20130609100338,20130609100339,20130609100340,20130609100341,20130609100342,20130609100343,20130609100344,20130609100345,20130609100346,20130609100347,20130609100348,20130609100349,20130609100350,20130609100351,20130609100352,20130609100353,20130609100354,20130609100355,20130609100356,20130609100357,20130609100358,20130609100359,20130609100360,20130609100361,20130609100362,20130609100363,20130609100364,20130609100365,20130609100366,20130609100367,20130609100368,20130609100369,20130609100370,20130609100371,20130609100372,20130609100373,20130609100374,20130609100375,20130609100376,20130609100377,20130609100378,20130609100379,20130609100380,20130609100381,20130609100382,20130609100383,20130609100384,20130609100385,20130609100386,20130609100387,20130609100388,20130609100389,20130609100390,20130609100391,20130609100392,20130609100393,20130609100394,20130609100395,20130609100396,20130609100397,20130609100398,20130609100399,20130609100400,20130609100401,20130609100402,20130609100473,20130609100477,20130609100478,20130609100479,20130609100480,20130609100482,20130609100483,20130609100484,20130609100485,20130609100486,20130609100487,20130609100488,20130609100489,20130609100490,20130609100492,20130609100493,20130609100494,20130609100496,20130609100497,20130609100498,20130609100499,20130609100500,20130609100501,20130609100502,20130609100503,20130609100505,20130609100506,20130609100508,20130609100510,20130609100512,20130609100513,20130609100514,20130609100524,20130609100525,20130609100526,20130609100529,20130609100543,20130609100548,20130609100567,20130609100576,20130609100586,20130609100587,20130609100597,20130609100600,20130609100608,20130609100610,20130609100619,20130609100621,20130609100623,20130609100624,20130609100626,20130609100628,20130609100630,20130609100632,20130609100634,20130609100635,20130609100637,20130609100639,20130609100641,20130609100642,20130609100644,20130609100645,20130609100646,20130609100647,20130609100650,20130609100653,20130609100654,20130609100656,20130609100657,20130609100658,20130609100659,20130609100662,20130609100663,20130609100665,20130609100666,20130609100669,20130609100672,20130609100673,20130609100674,20130609100678,20130609100751,20130609100754,20130609100757,20130609100780,20130609100787,20130609100788,20130609100789,20130609100791,20130609100792,20130609100793,20130609100794,20130609100795,20130609100796,20130609100797,20130609100799,20130609100800,20130609100801,20130609100802,20130609100804,20130609100808,20130609100812,20130609100814,20130609100815,20130609100817,20130609100818,20130609100820,20130609100821,20130609100822,20130609100823,20130609100824,20130609100825,20130609100826,20130609100827,20130609100828,20130609100831,20130609100832,20130609100834,20130609100835,20130609100838,20130609100840,20130609100841,20130609100847,20130609100854,20130609100864,20130609100865,20130609100868,20130609100869,20130609100870,20130609100878,20130609100883,20130609100890,20130609100891,20130609100898,20130609100899,20130609100900,20130609100901,20130609100903,20130609100904,20130609100905,20130609100906,20130609100907,20130609100908,20130609100909,20130609100910,20130609100912,20130609100913,20130609100914,20130609100915,20130609100916,20130609100917,20130609100918,20130609100919,20130609100921,20130609100922,20130609100923,20130609100924,20130609100925,20130609100927,20130609100929,20130609100930,20130609100931,20130609100932,20130609100933,20130609100934,20130609100936,20130609100937,20130609100938,20130609100939,20130609100940,20130609100944,20130609100950,20130609100951,20130609100963,20130609101002,20130609101011,20130609101031,20130609101032,20130609101034,20130609101035,20130609101036,20130609101037,20130609101038,20130609101039,20130609101040,20130609101041,20130609101042,20130609101043,20130609101044,20130609101045,20130609101046,20130609101047,20130609101048,20130609101049,20130609101051,20130609101052,20130609101053,20130609101054,20130609101055,20130609101056,20130609101057,20130609101058,20130609101059,20130609101060,20130609101061,20130609101064,20130609101066,20130609101069,20130609101070,20130609101080,20130609101185,20130609101187,20130609101188,20130609101189,20130609101190,20130609101192,20130609101193,20130609101194,20130609101196,20130609101197,20130609101198,20130609101201,20130609101202,20130609101203,20130609101204,20130609101205,20130609101206,20130609101208,20130609101209,20130609101211,20130609101212,20130609101213,20130609101215,20130609101220,20130609101223,20130609101224,20130609101226,20130609101228,20130609101229,20130609101230,20130609101231,20130609101232,20130609101234,20130609101335,20130609101336,20130609101337,20130609101338,20130609101339,20130609101340,20130609101341,20130609101342,20130609101343,20130609101344,20130609101345,20130609101346,20130609101347,20130609101348,20130609101349,20130609101350,20130609101351,20130609101353,20130609101354,20130609101355,20130609101357,20130609101358,20130609101359,20130609101360,20130609101363,20130609101364,20130609101365,20130609101366,20130609101373,20130609101375,20130609101376,20130609101512,20130609101520,20130609101524,20130609101527,20130609101529,20130609101530,20130609101531,20130609101535,20130609101541,20130609101546,20130609101548,20130609101549,20130609101551,20130609101552,20130609101555,20130609101556,20130609101558
";
            var list = batchNos.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            IList<string> batchNoList = list; // TODO: Initialize to an appropriate value
            IList<OutboundPrintExportModel> expected = null; // TODO: Initialize to an appropriate value
            IList<OutboundPrintExportModel> actual;
            actual = target.GetOutboundPrintExportModel(batchNoList);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetBatchBillInfoForOutBoundSendMail
        ///</summary>
        [TestMethod()]
        public void GetBatchBillInfoForOutBoundSendMailTest()
        {
            OutboundDAL target = new OutboundDAL(); // TODO: Initialize to an appropriate value
            IList<string> formCodeList = new List<string>(); // TODO: Initialize to an appropriate value
            formCodeList.Add("1321111200");
            formCodeList.Add("132111132");
            Enums.SortCenterOperateType outboundType = new Enums.SortCenterOperateType(); // TODO: Initialize to an appropriate value
            IList<BatchBillInfoForOutBound> expected = null; // TODO: Initialize to an appropriate value
            IList<BatchBillInfoForOutBound> actual;
            actual = target.GetBatchBillInfoForOutBoundSendMail(formCodeList, outboundType);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

    }
}