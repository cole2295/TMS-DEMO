using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    public interface IWaybillTakeSendInfoDAL
    {
        TakeSend_DeliverStationEntityModel GetTakeSendAndDeliverStationInfo(long WaybillNo);
    }
}
