using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.BaseInfo.Carrier
{
    /// <summary>
    /// 适用范围数据访问层接口
    /// </summary>
    public interface ICoverageDAL
    {
        /// <summary>
        /// 新增适用范围
        /// </summary>
        /// <param name="lstModel">适用范围</param>
        /// <returns>数据所影响行数</returns>
        int Add(IList<CoverageModel> lstModel);

        /// <summary>
        /// 根据承运商id取得适用范围
        /// </summary>
        /// <param name="carrierID">承运商ID</param>
        /// <returns>适用范围模型列表</returns>
        IList<CoverageModel> GetByCarrierID(int carrierID);

        /// <summary>
        /// 根据适用范围主键id取得适用范围
        /// </summary>
        /// <param name="coverageID">适用范围ID</param>
        /// <returns>适用范围模型</returns>
        CoverageModel GetByCoverageID(int coverageID);

        /// <summary>
        /// 删除适用范围
        /// </summary>
        /// <param name="carrierIDs">承运商id列表</param>
        /// <returns>数据所影响行数</returns>
        int Delete(IList<int> carrierIDs);
    }
}
