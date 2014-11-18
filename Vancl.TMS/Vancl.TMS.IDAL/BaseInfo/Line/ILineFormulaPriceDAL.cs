using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Line;

namespace Vancl.TMS.IDAL.BaseInfo.Line
{
    public interface ILineFormulaPriceDAL
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
        /// 是否已经存在
        /// </summary>
        /// <param name="lpid">线路计划id</param>
        /// <returns></returns>
        bool IsExist(int lpid);

        /// <summary>
        /// 删除线路公式价格
        /// </summary>
        /// <param name="lpidList">线路计划ID列表</param>
        /// <returns></returns>
        int Delete(List<int> lpidList);

        /// <summary>
        /// 新增明细
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        int AddDetail(List<LineFormulaPriceDetailModel> details);

        /// <summary>
        /// 获取线路公式价格明细
        /// </summary>
        /// <param name="lpid"></param>
        /// <returns></returns>
        List<LineFormulaPriceDetailModel> GetLineFormulaDetails(int lpid);

        /// <summary>
        /// 删除明细
        /// </summary>
        /// <param name="lpid"></param>
        /// <returns></returns>
        int DeleteDetails(int lpid);
    }
}
