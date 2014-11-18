using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.BLL.Log.DpsNotice
{
    public class AssignDistributionObserver : IDpsNoticeObserver
    {
        private IBillDAL billDAL = ServiceFactory.GetService<IBillDAL>("BillDAL");
        private IExpressCompanyDAL expDal = ServiceFactory.GetService<IExpressCompanyDAL>();
        public bool DoAction(Model.Log.BillChangeLogModel notice)
        {
            if (notice.CurrentStatus == Enums.BillStatus.AssignDistribution)
            {
                if (notice.ToDistributionCode == null)
                {
                    return true;
                }

                var exps = expDal.GetAllDistributors();
                var exp = exps.Find(e => e.DistributionCode == notice.ToDistributionCode);

                return billDAL.UpdateDeliverStation(notice.FormCode, int.Parse(exp.ID));
            }
            return true;
        }
    }
}
