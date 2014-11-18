using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Sorting.BillPrint;

namespace Vancl.TMS.DAL.Oracle.Sorting.BillPrint
{
    public class BillPrintFieldDAL : BaseDbDAL<BillPrintFieldModel, long>, IBillPrintFieldDAL
    {


        public IList<Model.Sorting.BillPrint.BillPrintFieldModel> GetBillPrintField(string distributionCode)
        {
            StringBuilder SbSql = new StringBuilder();
            List<OracleParameter> Parameters = new List<OracleParameter>();
            SbSql.Append(@"
SELECT id, 
  DistributionCode, 
  showname, 
  fieldname, 
  fieldformat, 
  defaultvalue, 
  defaultstyle, 
  remark, 
  sort, 
  isdeleted, 
  createtime, 
  createby, 
  updatetime, 
  updateby
FROM sc_billprintfield
WHERE isdeleted=0           ");
            if (!string.IsNullOrWhiteSpace(distributionCode))
            {
                SbSql.AppendLine("AND DistributionCode=:DistributionCode");
                Parameters.Add(
                        new OracleParameter() { ParameterName = "DistributionCode", DbType = DbType.String, Value = distributionCode }
                    );
            }
            SbSql.AppendLine("ORDER BY sort");

            return ExecuteSql_ByDataTableReflect<BillPrintFieldModel>(TMSReadOnlyConnection, SbSql.ToString(), Parameters.ToArray());
        }

    }
}
