using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Util.DateTimeUtil
{
    public static class DateTimeUtil
    {
        public static TimeSpan DateDiff(this DateTime DateTimeBigger, DateTime DateTimeSmaller)
        {
            TimeSpan ts = new System.TimeSpan(DateTimeBigger.Ticks - DateTimeSmaller.Ticks);
            return ts;
        }   
    }
}
