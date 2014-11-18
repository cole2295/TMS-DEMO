using Vancl.TMS.Model.Log;
using System.Collections.Generic;

namespace Vancl.TMS.IDAL.Log
{
    /// <summary>
    /// 拣运状态改变日志表数据层接口
    /// </summary>
    public interface IBillChangeLogDAL
    {
        /// <summary>
        /// 新增状态改变日志
        /// </summary>
        /// <param name="logModel">改变日志对象</param>
        /// <returns></returns>
        int Add(BillChangeLogModel logModel);

        /// <summary>
        /// 批量新增状态改变日志
        /// </summary>
        /// <param name="listLogModel">改变日志列表</param>
        /// <returns></returns>
        int BatchAdd(List<BillChangeLogModel> listLogModel);

        List<BillChangeLogModel> GetNotices(int count);

        bool UpdateSynStatus(string bcid);
    }
}
