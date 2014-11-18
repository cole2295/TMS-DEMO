using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.CityScan;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IDAL.Sorting.CityScan
{
    public interface ICityScanDAL : ISequenceDAL
    {
        bool Exists(String formCode);

        Int32 AddCityScan(CityScanModel cityScanModel);

        PagedList<CityScanModel> SearchCityScanStatistics(CityScanSearchModel searchModel);

        IList<CityScanExprotModel> SearchExportScan(List<string> batchnoList);

        IList<CityScanBatchDetail> SearchCityScanPrint(String batchno);
    }        
}
