using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Log.DpsNotice
{
    public class AssignStationObserver : IDpsNoticeObserver
    {
        IBillDAL billDAL = ServiceFactory.GetService<IBillDAL>("BillDAL");
        public bool DoAction(Model.Log.BillChangeLogModel notice)
        {
            if (notice.CurrentStatus == Enums.BillStatus.AssignStation)
            {
                return billDAL.UpdateDeliverStation(notice.FormCode, notice.ToExpressCompanyID);
            }
            return true;
        }
    }
}
