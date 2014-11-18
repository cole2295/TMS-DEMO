using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    /// <summary>
    /// LMS物流主库数据层基类
    /// </summary>
    public class LMSBaseDAL:BaseDAL
    {
        public override long GetNextSequence(string sequenceName)
        {
            string strSql = string.Format(@"
                SELECT {0}.NEXTVAL FROM dual
            ", sequenceName);
            object o = ExecuteSqlScalar(LMSOracleWriteConnection, strSql);
            return o == null ? 0 : Convert.ToInt64(o);
        }
    }



}

