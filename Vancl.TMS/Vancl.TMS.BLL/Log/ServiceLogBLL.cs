using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Log;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Core.ACIDManager;

namespace Vancl.TMS.BLL.Log
{
    public class ServiceLogBLL : IServiceLogBLL
    {
        IServiceLogDAL service = ServiceFactory.GetService<IServiceLogDAL>();
        ILms2TmsSyncLMSDAL lmsSyncDAL = ServiceFactory.GetService<ILms2TmsSyncLMSDAL>();
        ITms2LmsSyncTMSDAL tmsSyncDAL = ServiceFactory.GetService<ITms2LmsSyncTMSDAL>();

        #region IServiceLogBLL 成员

        public int WriteLog(ServiceLogModel log)
        {
            if (log != null)
            {
                return service.WriteLog(log);
            }
            else
                return 0;
        }

        public PagedList<ServiceLogModel> ReadLogs(ServiceLogSearchModel conditions)
        {
            return service.ReadLogs(conditions);
        }

        public int ResetSyncFlag(long logID, string syncKey, Model.Common.Enums.ServiceLogType logType)
        {
            using (IACID scope = ACIDScopeFactory.GetTmsACID())
            {
                if (logType == Model.Common.Enums.ServiceLogType.Lms2TmsSync)
                {
                    lmsSyncDAL.UpdateSyncStatus(long.Parse(syncKey), Model.Common.Enums.SyncStatus.NotYet);
                }
                if (logType == Model.Common.Enums.ServiceLogType.Tms2LmsSync)
                {
                    tmsSyncDAL.UpdateSyncStatus(syncKey, Model.Common.Enums.SyncStatus.NotYet);
                }

                int i = service.SetLogStatus(logID, Model.Common.Enums.ServiceLogProcessingStatus.Handled);
                scope.Complete();

                return i;
            }
        }

        #endregion
    }
}
