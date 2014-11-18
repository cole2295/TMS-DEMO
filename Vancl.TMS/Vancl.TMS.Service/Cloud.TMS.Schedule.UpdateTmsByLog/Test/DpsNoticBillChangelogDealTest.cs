using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cloud.TMS.Schedule.UpdateTmsByLog.Impl;
using NUnit.Framework;

namespace Cloud.TMS.Schedule.UpdateTmsByLog.Test
{
    [TestFixture]
    public class DpsNoticBillChangelogDealTest
    {
        [Test]
        public void TestDeal()
        {
            DpsNoticBillChangelogDeal changelog = new DpsNoticBillChangelogDeal();
            changelog.Deal();

        }
    }
}
