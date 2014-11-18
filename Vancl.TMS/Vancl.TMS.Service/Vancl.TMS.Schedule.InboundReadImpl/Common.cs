using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Schedule.InboundReadImpl
{
    public static class Common
    {
        public static string GetFileName()
        {
            return "w" + DateTime.Now.ToString("yyyyMMddHHmmssms") + ".xml";
        }

        public static string GetCompleteFileName()
        {
            return "r" + DateTime.Now.ToString("yyyyMMddHHmmssms") + ".xml";
        }
    }
}
