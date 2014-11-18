using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.CityScan;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IBLL.Sorting.CityScan
{
    public interface ICityScanBLL
    {
        ResultModel ScanCode(CityScanModel cityScanModel);

        PagedList<CityScanModel> SearchCityScanStatistics(CityScanSearchModel searchModel);

        IList<CityScanExprotModel> SearchExportScan(List<String> batchnoList);

        IList<CityScanBatchDetail> SearchCityScanPrint(String batchno);
    }
}
