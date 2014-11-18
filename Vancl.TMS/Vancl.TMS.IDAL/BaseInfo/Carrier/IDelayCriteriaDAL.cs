using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.BaseInfo.Carrier
{
    /// <summary>
    /// 延误考核标准数据访问层接口
    /// </summary>
    public interface IDelayCriteriaDAL
    {
        /// <summary>
        /// 新增延误考核标准
        /// </summary>
        /// <param name="model">延误考核标准</param>
        /// <returns>数据操作所影响行数</returns>
        int Add(DelayCriteriaModel model);

        /// <summary>
        /// 根据承运商id取得延误考核标准
        /// </summary>
        /// <param name="carrierID">承运商id</param>
        /// <returns></returns>
        IList<DelayCriteriaModel> GetByCarrierID(int carrierID);

        /// <summary>
        /// 删除延误考核标准
        /// </summary>
        /// <param name="carrierIDs">承运商id列表</param>
        /// <returns>数据操作所影响行数</returns>
        int Delete(IList<int> carrierIDs);

        /// <summary>
        /// 取得折扣
        /// </summary>
        /// <param name="carrierID">承运商ID</param>
        /// <param name="delayTimeSpan">延误时长</param>
        /// <returns></returns>
        decimal? GetDisCount(int carrierID, decimal delayTimeSpan);

    }
}
