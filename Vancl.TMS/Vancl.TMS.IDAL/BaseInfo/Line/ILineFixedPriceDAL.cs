using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.IDAL.BaseInfo.Line
{
    public interface ILineFixedPriceDAL
    {
        /// <summary>
        /// 新增线路固定价格
        /// </summary>
        /// <param name="model">线路固定价格</param>
        /// <returns></returns>
        int Add(LineFixedPriceModel model);
        /// <summary>
        /// 根据线路计划id取得线路固定价格
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        LineFixedPriceModel GetByLinePlanID(int lpid);
        /// <summary>
        /// 更新线路固定价格
        /// </summary>
        /// <param name="model">线路固定价格</param>
        /// <returns></returns>
        int Update(LineFixedPriceModel model);
        /// <summary>
        /// 是否已存在线路固定价格
        /// </summary>
        /// <param name="linePlanID">线路计划id</param>
        /// <returns></returns>
        bool IsExist(string linePlanID);
        /// <summary>
        /// 删除线路固定价格
        /// </summary>
        /// <param name="lpidList">线路计划ID列表</param>
        /// <returns></returns>
        int Delete(List<int> lpidList);
    }
}
