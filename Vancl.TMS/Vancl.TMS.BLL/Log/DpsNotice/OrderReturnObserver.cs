using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.BLL.Log.DpsNotice
{
    public class OrderReturnObserver : IDpsNoticeObserver
    {
        IBillDAL billDAL = ServiceFactory.GetService<IBillDAL>("BillDAL");
        public bool DoAction(BillChangeLogModel notice)
        {
            if (notice.CurrentStatus == Enums.BillStatus.ReturnOnStation
                && notice.ReturnStatus != null
                && notice.ReturnStatus.Value == Enums.ReturnStatus.ReturnInTransit)
            {
                return billDAL.UpdateReturnStatus(notice.FormCode, (int)notice.ReturnStatus.Value);
            }
            return true;
        }
    }
}
