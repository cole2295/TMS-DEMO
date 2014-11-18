using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Log;
using Vancl.TMS.IBLL.Log;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Log
{
    public class DeliveryFlowLogBLL : ILogBLL<DeliveryFlowLogModel>
    {
        ILogDAL<DeliveryFlowLogModel> dal = ServiceFactory.GetService<ILogDAL<DeliveryFlowLogModel>>();

        #region ILogBLL<DeliveryFlowLogModel> 成员

        public int Write(DeliveryFlowLogModel model)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryFlowLogModel> Read(BaseLogSearchModel searchmodel)
        {
            if (searchmodel == null) throw new ArgumentNullException("searchmodel is null");
            return dal.Read(searchmodel);
        }

        #endregion
    }
}
