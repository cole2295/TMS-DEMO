using System.Collections.Generic;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.IBLL.Log
{
    public interface IDpsNotice
    {
        List<BillChangeLogModel> GetNotices(int count);
        void UpdateSynStatus(string bcid);
    }
}