using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.IDAL.Log;

namespace Vancl.TMS.BLL.Log.DpsNotice
{
    public class DpsNotice : IDpsNotice
    {
        IBillChangeLogDAL _billChangeLogDAL = ServiceFactory.GetService<IBillChangeLogDAL>("BillChangeLogDAL");

        public List<Model.Log.BillChangeLogModel> GetNotices(int count)
        {
            return _billChangeLogDAL.GetNotices(count);
        }

        public void UpdateSynStatus(string bcid)
        {
            _billChangeLogDAL.UpdateSynStatus(bcid);
        }
    }
}
