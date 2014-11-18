using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.BillPrint;

namespace Vancl.TMS.IDAL.Sorting.BillPrint
{
    public interface IBillPrintTemplateDAL : IDbModelDAL<BillPrintTemplateModel, long>, ISequenceDAL
    {
        IList<BillPrintTemplateModel> GetBillPrintTemplates(string distributionCode);

        bool ClearDefault(string distributionCode);
    }
}
