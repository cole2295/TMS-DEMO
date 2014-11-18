using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.IBLL.BaseInfo
{
    public interface ILineFormulaPriceBLL
    {
        /// <summary>
        /// 新增线路公式价格
        /// </summary>
        /// <param name="model">线路公式价格</param>
        /// <returns></returns>
        int Add(LineFormulaPriceModel model);
        /// <summary>
        /// 根据线路计划id取得线路公式价格
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        LineFormulaPriceModel GetByLinePlanID(int lpid);
        /// <summary>
        /// 更新线路公式价格
        /// </summary>
        /// <param name="model">线路公式价格</param>
        /// <returns></returns>
        int Update(LineFormulaPriceModel model);
        /// <summary>
        /// 批量删除线路公式价格
        /// </summary>
        /// <param name="lpidList">线路计划ID列表</param>
        /// <returns></returns>
        int Delete(List<int> lpidList);
    }
}
