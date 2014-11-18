using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Vancl.TMS.Model.Sorting.BillPrint;
using Oracle.DataAccess.Client;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Sorting.BillPrint
{
    public class BillPrintTemplateDAL : BaseDbDAL<BillPrintTemplateModel, long>, IBillPrintTemplateDAL
    {
        public IList<BillPrintTemplateModel> GetBillPrintTemplates(string distributionCode)
        {
            StringBuilder SbSql = new StringBuilder();
            List<OracleParameter> Parameters = new List<OracleParameter>();
            SbSql.Append(@"
SELECT 
id, 
distributioncode, 
name, 
storage, 
width, 
height, 
background, 
remark, 
isdeleted, 
createtime, 
createby, 
updatetime, 
updateby,
isdefault
FROM sc_billprinttemplate
WHERE isdeleted=0           ");
            if (!string.IsNullOrWhiteSpace(distributionCode))
            {
                SbSql.AppendLine("AND DistributionCode=:DistributionCode");
                Parameters.Add(
                        new OracleParameter() { ParameterName = "DistributionCode", DbType = DbType.String, Value = distributionCode }
                    );
            }
            SbSql.AppendLine("ORDER BY id");

            return ExecuteSql_ByDataTableReflect<BillPrintTemplateModel>(TMSWriteConnection, SbSql.ToString(), Parameters.ToArray());

        }

        public bool ClearDefault(string distributionCode)
        {
            string sql = @" update sc_billprinttemplate
                               set isdefault = 0
                             where isdefault = 1
                               and distributioncode = :distributioncode";
            OracleParameter[] arguments ={ 
               new OracleParameter
                   {
                       ParameterName = "distributioncode", DbType = DbType.String, Value = distributionCode
                   } };

            ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
            return true;
        }
    }
}
