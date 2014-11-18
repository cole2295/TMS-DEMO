using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.IDAL.Sorting.BillPrint;

namespace Vancl.TMS.DAL.Oracle.Sorting.BillPrint
{
    public class BillInfoDAL : BaseDAL, IBillInfoDAL
    {

        public BillInfoModel GetBillInfoByFormCode(string FormCode)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                           SELECT  BIID
                                                         ,FormCode
                                                         ,CustomerWeight
                                                         ,CustomerBoxNo
                                                         ,BillGoodsType
                                                         ,Weight
                                                         ,PayType
                                                         ,ReceivableAmount
                                                         ,InsuredAmount
                                                         ,Tips
                                                         ,PackageMode
                                                         ,PackageCount
                                                         ,TotalAmount
                                                         ,CASE WHEN IsValidateBill IS NULL THEN 0 ELSE IsValidateBill END IsValidateBill
                                                        FROM SC_BillInfo 
                                                        WHERE  IsDeleted = 0 AND FormCode = :FormCode             

                                         ");
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = FormCode } 
            };
            return ExecuteSqlSingle_ByReaderReflect<BillInfoModel>(TMSWriteConnection, SbSql.ToString(), arguments);
        }


        public int UpdateBillInfoWeight(string fromCode, decimal weight)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                Update SC_BillInfo
                Set Weight = :Weight
                WHERE FormCode = :FormCode
                                         ");
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "Weight", DbType = DbType.Decimal, Value = weight },
                new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = fromCode } ,
            };
            return base.ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments);
        }


        public int UpdateValidateStatus(string formCode, bool p)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                Update SC_BillInfo
                Set IsValidateBill = :IsValidateBill
                WHERE FormCode = :FormCode
                                         ");
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "IsValidateBill", OracleDbType = OracleDbType.Byte, Value = p?1:0 },
                new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2, Value = formCode } ,
            };
            return base.ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments);
        }
    }
}
