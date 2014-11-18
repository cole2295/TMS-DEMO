using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Vancl.TMS.Schedule.Dps2TmsService.Impl;

namespace Vancl.TMS.Schedule.Dps2TmsService.Test
{
    [TestFixture]
    public class Dps2TmsImplTest
    {
        [Test]
        public void TestDoJob()
        {
            Dps2TmsImpl impl = new Dps2TmsImpl();

            impl.DoJob();


        }
    }
}
