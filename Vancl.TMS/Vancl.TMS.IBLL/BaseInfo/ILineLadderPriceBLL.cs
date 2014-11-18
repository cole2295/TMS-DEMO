using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.IBLL.BaseInfo
{
    public interface ILineLadderPriceBLL
    {
        /// <summary>
        /// 新增线路阶梯价格
        /// </summary>
        /// <param name="models">线路阶梯价格</param>
        /// <returns></returns>
        int Add(List<LineLadderPriceModel> models);
        /// <summary>
        /// 根据线路计划id取得线路阶梯价格
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        IList<LineLadderPriceModel> GetByLinePlanID(int lpid);
        /// <summary>
        /// 批量删除线路阶梯价格
        /// </summary>
        /// <param name="lpidList">线路计划ID列表</param>
        /// <returns></returns>
        int Delete(List<int> lpidList);
        /// <summary>
        /// 更新线路阶梯价格
        /// </summary>
        /// <param name="models">线路阶梯价格</param>
        /// <returns></returns>
        int Update(List<LineLadderPriceModel> models);
    }
}
