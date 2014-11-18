using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.IDAL.Synchronous
{
    public interface ILms2TmsSyncTMSDAL
    {
        /// <summary>
        /// 运单是否在运单主表存在
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <returns></returns>
        bool IsBillExists(string formCode);

        /// <summary>
        /// 运单是否在运单信息表存在
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <returns></returns>
        bool IsBillInfoExists(string formCode);

        /// <summary>
        /// 增加运单主表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddBill(BillModel model);

        /// <summary>
        /// 增加运单信息表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddBillInfo(BillInfoModel model);
    }
}
