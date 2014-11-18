using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.BillPrint;

namespace Vancl.TMS.IBLL.Sorting.BillPrint
{
    /// <summary>
    /// 外单打印
    /// </summary>
    public interface IBillPrintBLL : ISortCenterBLL
    {        
        /// <summary>
        /// 扫描重量
        /// </summary>
        /// <param name="billInfo"></param>
        /// <param name="argModel"></param>
        /// <returns></returns>
        BillPageViewModel ScanWeight(BillScanWeightArgModel argModel);

        /// <summary>
        /// 获取打印数据
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        BillPrintViewModel GetPrintData(string formCode, int packageIndex);
        /// <summary>
        /// 新打印数据
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        PrintBillNewModel GetPrintData(string formCode);

        /// <summary>
        /// 重新称重
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="packageIndex"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        ResultModel ReWeigh(string formCode, int packageIndex, decimal weight, int MerchantId, out decimal totalWeight);

        /// <summary>
        /// 面单校验
        /// </summary>
        /// <param name="customerOrder"></param>
        /// <param name="formCode"></param>
        /// <returns></returns>
        BillValidateResultModel ValidateBill(string customerOrder, string formCode);
        /// <summary>
        /// 新扫描称重
        /// </summary>
        /// <param name="argModel"></param>
        /// <returns></returns>
        BillPageViewModel ScanWeightNew(BillScanWeightArgModel argModel);
    }
}
