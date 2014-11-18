using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using System.Data.SqlClient;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class TransitBoxDAL : BaseDAL, ITransitBoxDAL
    {
        /// <summary>
        /// 改变周转箱使用状态
        /// </summary>
        /// <param name="isUsing"></param>
        /// <returns></returns>
        public bool UpdateUsingStatus(bool isUsing, string boxNo)
        {
            string sql = @"
UPDATE TransitBox
    SET    IsUsing = @IsUsing
WHERE  TransitBoxCode = @BoxNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {                
                new SqlParameter() { ParameterName="@IsUsing",  SqlDbType = System.Data.SqlDbType.Bit , Value = isUsing },
                new SqlParameter() { ParameterName="@BoxNo",  SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = boxNo }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments)>0;

        }
    }
}
