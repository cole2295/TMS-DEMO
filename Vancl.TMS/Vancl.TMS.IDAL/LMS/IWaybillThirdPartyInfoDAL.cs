using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    public interface IWaybillThirdPartyInfoDAL
    {
        /// <summary>
        /// 保存运单重量
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        int SaveWeight(string formCode, decimal weight);
        /// <summary>
        /// 获取箱子信息
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="packageIndex"></param>
        /// <returns></returns>
        WaybillThirdPartyInfoEntityModel GetPackageModel(string formCode, int packageIndex);
        /// <summary>
        /// 添加箱子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddBillPackage(WaybillThirdPartyInfoEntityModel model);
        /// <summary>
        /// 更新箱子
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateBillPackage(WaybillThirdPartyInfoEntityModel model);
    }
}
