using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Vancl.TMS.Schedule.Tms2LmsImpl;

namespace Vancl.TMS.Schedule.Tms2LmsService.Test
{
    [TestFixture]
    public class Tms2LmsJobTest
    {
        [Test]
        public void TestDo()
        {
            Tms2LmsJob job = new Tms2LmsJob();
            job.Do(0);
        }
    }
}
