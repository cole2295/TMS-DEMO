using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class TransitBoxDAL: LMSBaseDAL, ITransitBoxDAL
    {
        /// <summary>
        /// 改变周转箱使用状态
        /// </summary>
        /// <param name="isUsing"></param>
        /// <returns></returns>
        public bool UpdateUsingStatus(bool isUsing, string boxNo)
        {
            String sql = @"
UPDATE TransitBox
SET  IsUsing = :IsUsing
WHERE TransitBoxCode = :BoxNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="IsUsing", OracleDbType = OracleDbType.Int32 , Value = Convert.ToInt32(isUsing) },
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = boxNo }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments) > 0;
        }
    }
}
