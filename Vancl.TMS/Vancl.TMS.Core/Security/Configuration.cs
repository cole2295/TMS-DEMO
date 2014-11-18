using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Vancl.TMS.Core.Security
{
    public class Configuration
    {
        //是否是调试状态
        public static bool IsDebug
        {
            get
            {

                System.Web.Configuration.CompilationSection ds
                    = (System.Web.Configuration.CompilationSection)ConfigurationManager.GetSection("system.web/compilation");
                return ds.Debug;
            }
        }
    }
}
