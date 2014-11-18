using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.DAL.Sql2008.Log
{
    public class DeliveryFlowLogDAL :BaseDAL, ILogDAL<DeliveryFlowLogModel>
    {
        #region ILogDAL<DeliveryLogModel> 成员

        public int Write(DeliveryFlowLogModel model)
        {
            throw new NotImplementedException();
        }

        public List<DeliveryFlowLogModel> Read(LogSearchModel model)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
