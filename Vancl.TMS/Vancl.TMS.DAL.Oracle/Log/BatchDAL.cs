using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Log;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Log;
using Oracle.DataAccess.Client;
using Vancl.TMS.Util.DbHelper;

namespace Vancl.TMS.DAL.Oracle.Log
{
    public class BatchDAL : BaseDAL,IBatchDAL
    {
        public List<BatchModel> GetBatch(BatchSearchModel searchModel)
        {
            if (searchModel == null)
                return null;

            StringBuilder sbWhere = new StringBuilder();
            List<OracleParameter> parameterList = new List<OracleParameter>();
            string sql = @" select 
                                 rownum as SerialNumber,
                                 td.Formcode,
                                 t.CustomerBatchNo,
                                 t.BoxNo,
                                 t.DepartureId,
                                 ec.companyname as DepartureName,
                                 t.ArrivalId,
                                 ec1.companyname as ArrivalName,
                                 1 as IsConsistency
                             from tms_box t
                                 join tms_boxdetail td on t.BoxNo=td.Boxno
                                 join Expresscompany ec on ec.expresscompanyid=t.departureid
                                 join Expresscompany ec1 on ec1.expresscompanyid=t.arrivalid
                             where 1=1 {0}";
            if (!string.IsNullOrWhiteSpace(searchModel.BatchNo))
            {
                sbWhere.Append(" AND t.customerbatchno=:customerbatchno");
                parameterList.Add(new OracleParameter(":customerbatchno", OracleDbType.Varchar2) { Value = searchModel.BatchNo });
            }
            sql=string.Format(sql,sbWhere.ToString());
            var list= ExecuteSql_ByReaderReflect<BatchModel>(TMSReadOnlyConnection, sql, parameterList.ToArray());
            if (list != null)
                return list.ToList();
            else
                return null;
        }

        public Int32 RepairTest(String txt)
        {
            Int32 n= ExecuteSqlNonQuery(TMSWriteConnection, txt);
            return n;
        }
    }
}
