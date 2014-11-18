using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.IDAL.BaseInfo.Line
{
    public interface ILineLadderPriceDAL
    {
        /// <summary>
        /// 新增线路阶梯价格
        /// </summary>
        /// <param name="model">线路阶梯价格</param>
        /// <returns></returns>
        int Add(LineLadderPriceModel model);
        /// <summary>
        /// 根据线路计划id取得线路阶梯价格
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        IList<LineLadderPriceModel> GetByLinePlanID(int lpid);
        /// <summary>
        /// 删除线路阶梯价格
        /// </summary>
        /// <param name="lpidList">线路计划ID列表</param>
        /// <returns></returns>
        int Delete(List<int> lpidList);
    }
}
