using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.Outbound;

namespace Vancl.TMS.IBLL.BaseInfo
{
    /// <summary>
    /// 配送商业务接口
    /// </summary>
    public interface IDistributionBLL
    {
        /// <summary>
        /// 取得配送商对象
        /// </summary>
        /// <param name="DistributionCode">配送商Code</param>
        /// <returns></returns>
        Distribution GetModel(string DistributionCode);

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
        /// 取得出库的后置条件
        /// </summary>
        /// <param name="currentDistributionCode">当前操作的配送商</param>
        /// <param name="targetDistributionCode">出库转到的目的地配送商</param>
        /// <param name="outboundType">出库类型</param>
        /// <returns></returns>
        OutboundAfterConditionModel GetOutboundAfterConditionModel(String currentDistributionCode, String targetDistributionCode, Enums.SortCenterOperateType outboundType);

        /// <summary>
        /// 获取所有配送商信息
        /// </summary>
        /// <returns></returns>
        IList<Distribution> GetDistributionList();

    }
}
