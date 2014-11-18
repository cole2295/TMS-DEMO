using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.IDAL.BaseInfo
{
    public interface IDistributionDAL
    {
        /// <summary>
        /// 取得配送商对象
        /// </summary>
        /// <param name="DistributionCode">配送商Code</param>
        /// <returns></returns>
        Model.BaseInfo.Distribution GetModel(string DistributionCode);


        /// <summary>
        /// 根据配送商Code取得配送商名称
        /// </summary>
        /// <param name="distributionCode">配送商Code</param>
        /// <returns></returns>
        string GetDistributionNameByCode(string distributionCode);

        /// <summary>
        /// 取得配送商前一个状态
        /// </summary>
        /// <param name="DistributionCode">配送商Code</param>
        /// <param name="Status">状态</param>
        /// <returns></returns>
        Enums.BillStatus? GetDistributionPreBillStatus(String DistributionCode, Enums.BillStatus Status);

        /// <summary>
        /// 是否存在运输中心
        /// </summary>
        /// <param name="distributionCode">配送商</param>
        /// <returns></returns>
        bool ExistsTrafficCenter(String distributionCode);

        /// <summary>
        /// 是否存在分拣中心
        /// </summary>
        /// <param name="distributionCode">配送商</param>
        /// <returns></returns>
        bool ExistsSortCenter(String distributionCode);




        /// <summary>
        /// 判断配送商是否支持分拣
        /// </summary>
        /// <param name="distributionCode">配送商代码</param>
        /// <returns></returns>
        bool IsSupportSorting(string distributionCode);

        /// <summary>
        /// 获取所有配送商信息
        /// </summary>
        /// <returns></returns>
        IList<Distribution> GetDistributionList();
    }
}
