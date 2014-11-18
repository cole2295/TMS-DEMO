using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.OutSync;

namespace Vancl.TMS.IDAL.Synchronous
{
    public interface ITms2LmsSyncTMSDAL
    {
        /// <summary>
        /// 读取tms中间表数据
        /// </summary>
        /// <param name="count">每次读取数量</param>
        /// <param name="mod">取模</param>
        /// <param name="remainder">余数</param>
        /// <returns></returns>
        List<BillChangeLogModel> ReadTmsChangeLogs(Tms2LmsJobArgs argument);

        /// <summary>
        /// 更新tms中间表同步状态
        /// </summary>
        /// <param name="bcid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        int UpdateSyncStatus(String bcid, Enums.SyncStatus status);
    }
}
