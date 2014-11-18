using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Vancl.TMS.BLL.Log.DpsNotice;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util;
using Vancl.TMS.Core.ServiceFactory;

namespace Cloud.TMS.Schedule.UpdateTmsByLog.Impl
{
    public class DpsNoticBillChangelogDeal : IStatefulJob
    {
        private List<IDpsNoticeObserver> _observers;
        public List<IDpsNoticeObserver> Observers
        {
            get
            {
                if (_observers == null)
                {
                    _observers = new List<IDpsNoticeObserver>
                        {
                            new AssignDistributionObserver(),
                            new AssignStationObserver(),
                            new OrderReturnObserver()
                        };
                }
                return _observers;
            }
        }

        IDpsNotice _dpsNotice = ServiceFactory.GetService<IDpsNotice>("DpsNotice");

        public void Execute(JobExecutionContext context)
        {
            try
            {
                Deal();
            }
            catch (Exception ex)
            {
                MessageCollector.Instance.Collect(GetType(), ex, true);
            }
        }

        public void Deal()
        {
            int count = 200;
            var notices = _dpsNotice.GetNotices(count);
            foreach (var n in notices)
            {
                foreach (var obServer in Observers)
                {
                    if (!obServer.DoAction(n))
                    {
                        MessageCollector.Instance.Collect(GetType(), string.Format("{0}({1})更新失败", n.FormCode, n.BCID), true);
                    }
                }
                _dpsNotice.UpdateSynStatus(n.BCID);
            }
        }
    }
}
