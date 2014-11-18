using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.DAL.Sql2008.Log
{
    public class OperateLogDAL :BaseDAL, ILogDAL<OperateLogModel>
    {
        #region ILogDAL<OperateLogModel> 成员

        public int Write(OperateLogModel model)
        {
            throw new NotImplementedException();
        }

        public List<OperateLogModel> Read(LogSearchModel model)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
