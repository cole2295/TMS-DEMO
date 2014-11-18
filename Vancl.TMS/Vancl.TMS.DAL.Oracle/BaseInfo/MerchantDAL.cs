using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.DAL.Oracle.BaseInfo
{
    class MerchantDAL : BaseDAL, IMerchantDAL
    {
        public IList<MerchantModel> GetMerchantListByDistributionCode(string distributionCode)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                           SELECT mb.Id ,mb.MerchantName AS Name,mb.MerchantCode
                                                        ,NVL(mb.IsSkipPrintBill, 0) AS IsSkipPrintBill 
                                                        , NVL(mb.IsNeedWeight, 0) AS IsNeedWeight    
                                                        ,NVL(mb.IsCheckWeight, 0) AS IsCheckWeight
                                                        ,NVL(mb.CheckWeight, 0) AS CheckWeight 

                                                        FROM MerchantBaseInfo mb
                                                        LEFT JOIN DistributionMerchantRelation dmr ON dmr.MerchantId=mb.ID 

                                                        WHERE mb.IsSubMerchant=0   AND mb.IsDeleted = 0 AND dmr.IsDeleted=0
                                                        and dmr.DistributionCode= :DistributionCode
                                        ");

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DistributionCode", DbType = DbType.String, Value = distributionCode } 
            };

            return ExecuteSql_ByReaderReflect<MerchantModel>(TMSReadOnlyConnection, SbSql.ToString(), arguments);
        }

        public MerchantModel GetByID(long merchantID)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                           SELECT mb.Id ,mb.MerchantName Name ,mb.MerchantCode Code
                                                        ,NVL(mb.IsSkipPrintBill, 0) AS IsSkipPrintBill 
                                                        , NVL(mb.IsNeedWeight, 0) AS IsNeedWeight    
                                                        ,NVL(mb.IsCheckWeight, 0) AS IsCheckWeight
                                                        ,NVL(mb.CheckWeight, 0) AS CheckWeight 
                                                        ,NVL(mb.isvalidatebill, 0) AS IsValidateBill 
                                                        ,IsExpressDelivery
                                                        FROM MerchantBaseInfo mb
                                                        WHERE  mb.IsDeleted = 0 AND mb.Id= :ID                                                       
                                        ");

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "ID", DbType = DbType.Int32, Value = merchantID } 
            };

            return ExecuteSqlSingle_ByReaderReflect<MerchantModel>(TMSReadOnlyConnection, SbSql.ToString(), arguments);
        }
    }
}
