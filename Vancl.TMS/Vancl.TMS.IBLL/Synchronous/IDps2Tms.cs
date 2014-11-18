using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.IBLL.Synchronous
{
    public interface IDps2Tms
    {
        bool ImportBaseDataToTms(BillModel billm, BillInfoModel billInfom);
    }
}