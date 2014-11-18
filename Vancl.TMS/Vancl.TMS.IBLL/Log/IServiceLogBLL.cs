using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IBLL.Log
{
    public interface IServiceLogBLL
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">内容</param>
        /// <returns></returns>
        int WriteLog(ServiceLogModel log);

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="conditions">查询条件</param>
        /// <returns></returns>
        PagedList<ServiceLogModel> ReadLogs(ServiceLogSearchModel conditions);

        /// <summary>
        /// 重置同步状态
        /// </summary>
        /// <param name="logID">日志ID</param>
        /// <param name="syncKey">唯一标识</param>
        /// <param name="logType">日志类型</param>
        /// <returns></returns>
        int ResetSyncFlag(long logID, string syncKey, Vancl.TMS.Model.Common.Enums.ServiceLogType logType);
    }
}
