using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.BillPrint;

namespace Vancl.TMS.IDAL.Sorting.BillPrint
{
    public interface IBillWeighDAL
    {
        int Add(BillPackageModel model);
        int UpdateWeight(string formCode, int packageIndex, decimal weight);
        BillPackageModel Get(string formCode, int packageIndex);
        IList<BillPackageModel> GetListByFormCode(string formCode);
        int UpdateSyncStatus(string formCode, int packageIndex, Vancl.TMS.Model.Common.Enums.SyncStatus syncStatus);
        int UpdateSyncStatus(string formCode, Model.Common.Enums.SyncStatus prevFlag, Model.Common.Enums.SyncStatus nextFlag);

    }
}
