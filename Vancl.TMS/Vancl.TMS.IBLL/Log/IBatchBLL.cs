using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Log
{
    public interface IBatchBLL
    {
        List<BatchModel> GetBatch(BatchSearchModel searchModel);

        ResultModel RepairBatchDetail(String formCode);

        Int32 RepairTest(String txt);
    }
}
