using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.BillPrint;

namespace Vancl.TMS.IDAL.Sorting.BillPrint
{
    public interface IBillPrintFieldDAL : IDbModelDAL<BillPrintFieldModel, long>
    {
        /// <summary>
        /// 获取面单打印字段
        /// </summary>
        /// <param name="distributionCode"></param>
        /// <returns></returns>
        IList<BillPrintFieldModel> GetBillPrintField(string distributionCode=null);
    }
}
