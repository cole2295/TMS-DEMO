using Vancl.TMS.Model.Log;

namespace Vancl.TMS.IBLL.Log
{
    public interface IDpsNoticeObserver
    {
        bool DoAction(BillChangeLogModel notice);
    }
}