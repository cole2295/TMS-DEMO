using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.Log
{
    public interface IServiceLogDAL
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">内容</param>
        /// <returns></returns>
        int WriteLog(ServiceLogModel log);

        /// <summary>
        /// 批量写日志
        /// </summary>
        /// <param name="listLog"></param>
        /// <returns></returns>
        int BatchWriteLog(List<ServiceLogModel> listLog);

        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="conditions">查询条件</param>
        /// <returns></returns>
        PagedList<ServiceLogModel> ReadLogs(ServiceLogSearchModel conditions);

        /// <summary>
        /// 设置日志为处理状态
        /// </summary>
        /// <returns></returns>
        int SetLogStatus(long logID, Enums.ServiceLogProcessingStatus status);
    }
}
