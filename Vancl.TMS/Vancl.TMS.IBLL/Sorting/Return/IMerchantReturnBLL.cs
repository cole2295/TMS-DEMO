using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.IBLL.Sorting.Common;

namespace Vancl.TMS.IBLL.Sorting.Return
{
    /// <summary>
    /// 商家入库确认接口层
    /// </summary>
    public interface IMerchantReturnBLL : ISortCenterBLL
    {
        /// <summary>
        /// 查找列表信息
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <returns></returns>
        List<MerchantReturnBillViewModel> GetMerchantReturnBillList(ReturnBillSeachModel SearchModel, string isHasPrint,string CreateDept, out string FormCodelists);
        /// <summary>
        /// 查找列表信息
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <returns></returns>
        List<MerchantReturnBillStatisticModel> GetReturnbillStatistic(string FormCodelists);

        /// <summary>
        /// 商家入库确认
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <param name="Message"></param>
        void ReturnbillInBound(string FormCodes,out string Message);
        /// <summary>
        /// 查找列表信息
        /// </summary>
        /// <param name="SearchModel"></param>
        /// <returns></returns>
        List<MerchantReturnBillViewModel> GetMerchantReturnBillListForPrint(string FormCodelists, out List<string> MerchantNames, out List<string> billStatus);

    }
}
