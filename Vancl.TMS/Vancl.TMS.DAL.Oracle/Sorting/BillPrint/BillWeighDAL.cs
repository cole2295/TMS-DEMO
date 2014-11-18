using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Vancl.TMS.Model.Sorting.BillPrint;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
namespace Vancl.TMS.DAL.Oracle.Sorting.BillPrint
{
    public class BillWeighDAL : BaseDAL, IBillWeighDAL
    {
        public int Add(BillPackageModel model)
        {
            string SbSql = String.Format(@"
                                          INSERT INTO SC_billweigh(BWID,FormCode,PackageIndex,Weight,CreateBy,CreateTime,UpdateBy,UpdateTime,IsDeleted,SyncFlag)
                                          VALUES({0},:FormCode,:PackageIndex,:Weight,:CreateBy,:CreateTime,:UpdateBy,:UpdateTime,0,:SyncFlag)
                                           ", model.KeyCodeNextValue());
            OracleParameter[] parameters = new OracleParameter[]
            {
           //     new OracleParameter() { ParameterName="BWID", DbType = System.Data.DbType.Int64, Value = model.BWID},
                new OracleParameter() { ParameterName="FormCode", DbType = System.Data.DbType.String, Value = model.FormCode},
                new OracleParameter() { ParameterName="PackageIndex", DbType = System.Data.DbType.Int32, Value = model.PackageIndex},
                new OracleParameter() { ParameterName="Weight", DbType = System.Data.DbType.Decimal, Value = model.Weight},
                new OracleParameter() { ParameterName="CreateBy", DbType = System.Data.DbType.Int32, Value = model.CreateBy},
                new OracleParameter() { ParameterName="CreateTime", DbType = System.Data.DbType.Date, Value = model.CreateTime},
                new OracleParameter() { ParameterName="UpdateBy", DbType = System.Data.DbType.Int32, Value = model.UpdateBy},
                 new OracleParameter() { ParameterName="UpdateTime", DbType = System.Data.DbType.Date, Value = model.UpdateTime},
                new OracleParameter() { ParameterName = "SyncFlag", DbType = DbType.Int32, Value = (int)Vancl.TMS.Model.Common.Enums.SyncStatus.NotYet } ,
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, SbSql, parameters);
        }
        public IList<BillPackageModel> GetListByFormCode(string formCode)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                           SELECT BWID
                                                         ,FormCode
                                                         ,PackageIndex
                                                         ,Weight
                                                        ,CreateTime
                                                        ,UpdateTime
                                                        ,SyncFlag
                                            FROM  SC_billweigh 
                                            WHERE IsDeleted = 0 AND FormCode = :FormCode                                                       
                                        ");

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } 
            };
            return ExecuteSql_ByDataTableReflect<BillPackageModel>(TMSWriteConnection, SbSql.ToString(), arguments);

        }


        public int UpdateSyncStatus(string formCode, int packageIndex, Enums.SyncStatus syncStatus)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                            Update  SC_billweigh 
                                            Set SyncFlag = :SyncFlag
                                            WHERE FormCode = :FormCode AND PackageIndex = :PackageIndex                                                       
                                        ");

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "SyncFlag", DbType = DbType.Int32, Value = (int)syncStatus } ,
                new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } ,
                new OracleParameter() { ParameterName = "PackageIndex", DbType = DbType.Int32, Value = packageIndex } ,
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments);
        }


        public int UpdateWeight(string formCode, int packageIndex, decimal weight)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                            Update  SC_billweigh 
                                            Set weight = :Weight,
                                                UpdateTime = sysdate,
                                                SyncFlag=:SyncFlag
                                            WHERE FormCode = :FormCode AND PackageIndex = :PackageIndex                                                       
                                        ");

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "Weight", DbType = DbType.Decimal, Value = weight } ,
                new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } ,
                new OracleParameter() { ParameterName = "PackageIndex", DbType = DbType.Int32, Value = packageIndex } ,
                new OracleParameter() { ParameterName = "SyncFlag", DbType = DbType.Int32, Value = (int)Vancl.TMS.Model.Common.Enums.SyncStatus.NotYet } ,
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments);
        }

        public BillPackageModel Get(string formCode, int packageIndex)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                           SELECT BWID
                                                         ,FormCode
                                                         ,PackageIndex
                                                         ,Weight
                                                        ,CreateTime
                                                        ,UpdateTime
                                                        ,SyncFlag
                                            FROM  SC_billweigh 
                                            WHERE IsDeleted = 0 AND FormCode = :FormCode  and PackageIndex=:PackageIndex                                                   
                                        ");

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } ,
                new OracleParameter() { ParameterName = "PackageIndex", DbType = DbType.Int32, Value = packageIndex } ,
            };
            return ExecuteSqlSingle_ByReaderReflect<BillPackageModel>(TMSWriteConnection, SbSql.ToString(), arguments);

        }


        public int UpdateSyncStatus(string formCode, Enums.SyncStatus prevFlag, Enums.SyncStatus nextFlag)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                            Update  SC_billweigh 
                                            Set SyncFlag = :nextFlag
                                            WHERE FormCode = :FormCode AND SyncFlag = :prevFlag                                                       
                                        ");

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "nextFlag", DbType = DbType.Int32, Value = (int)nextFlag } ,
                new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } ,
                new OracleParameter() { ParameterName = "prevFlag", DbType = DbType.Int32, Value = (int)prevFlag } ,
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments);
        }
    }
}
