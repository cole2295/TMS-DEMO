using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Synchronous.OutSync;

namespace Vancl.TMS.IBLL.Synchronous
{
    /// <summary>
    /// TMS推送数据服务[到其他]
    /// </summary>
    public interface ITms2ThridPartyBLL
    {

        /// <summary>
        /// TMS城际运输数据同步回LMS4时效监控
        /// </summary>
        /// <param name="args"></param>
        void TMS2LMS4AgingMonitoring(Tms2ThridPartyJobArgs args);

    }
}
