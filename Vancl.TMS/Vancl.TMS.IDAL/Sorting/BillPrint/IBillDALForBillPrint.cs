using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.BillPrint;

namespace Vancl.TMS.IDAL.Sorting.BillPrint
{
    public interface IBillDALForBillPrint
    {
        Enums.BillType? GetBillTypeByFormCode(string FormCode);
        /// <summary>
        /// 获取面单打印所需要的运单信息
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        PrintBillModel GetBillByFormCode(string formCode);

        /// <summary>
        /// 更新运单状态
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="prevStatus"></param>
        /// <param name="currStatus"></param>
        /// <returns></returns>
        int UpdateBillStatus(string formCode, Enums.BillStatus prevStatus, Enums.BillStatus currStatus);


        /// <summary>
        /// 获取面单打印所需要的数据
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <param name="merchantID">运单商家ID</param>
        /// <param name="packageIndex">包装序号</param>
        /// <param name="receiveArea">接受区域</param>
        /// <returns></returns>
        BillPrintViewModel GetBillPrintMenuData(string formCode, int? merchantID, int packageIndex, string receiveArea);
    }
}
