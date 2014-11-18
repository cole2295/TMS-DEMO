using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.DeliveryImport;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;
using System.Data;
using Vancl.TMS.Model.ImportRecord;
using Vancl.TMS.Model;

namespace Vancl.TMS.DAL.Oracle.DeliveryImport
{
    public class DeliveryImportDAL : BaseDAL, IDeliveryImportDAL
    {
        #region IDeliveryImportDAL 成员

        public int AddRecord(Model.ImportRecord.DeliveryInRecordModel model)
        {
            if (model != null)
            {
                string sql = string.Format(@"INSERT INTO tms_deliveryinrecord
                            (
                                RECORDID,
                                RECORDCOUNT,
                                FAULTCOUNT,
                                CREATETIME,
                                CREATEBY,
                                ISDELETED,
                                DELIVERYSOURCE,
                                NOTE,
                                BATCHNO,
                                FILEPATH
                            ) 
                            VALUES
                            (
                                {0},
                                :RECORDCOUNT,
                                :FAULTCOUNT,
                                sysdate,
                                :CREATEBY,
                                0,
                                :DELIVERYSOURCE,
                                :NOTE,
                                :BATCHNO,
                                :FILEPATH
                            )", model.SequenceNextValue());
                OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="RECORDCOUNT",OracleDbType= OracleDbType.Int32,Value=model.RecordCount},
                new OracleParameter() { ParameterName="FAULTCOUNT",OracleDbType= OracleDbType.Int32,Value=model.FaultCount},
                new OracleParameter() { ParameterName="CREATEBY",OracleDbType= OracleDbType.Int32,Value=model.CreateBy},
                new OracleParameter() { ParameterName="DELIVERYSOURCE",OracleDbType= OracleDbType.Int32,Value=(int)model.DeliverySource},
                new OracleParameter() { ParameterName="NOTE",OracleDbType= OracleDbType.Varchar2, Size = 1000, Value=model.Note},
                new OracleParameter() { ParameterName="BATCHNO",OracleDbType= OracleDbType.Varchar2, Size = 50, Value=model.BatchNo},
                new OracleParameter() { ParameterName="FILEPATH",OracleDbType= OracleDbType.Varchar2, Size = 100, Value=model.FilePath}
            };
                return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
            }
            else
                return 0;
        }

        public List<DeliveryInRecordModel> GetRecordList(Model.ImportRecord.DeliveryInRecordSearchModel conditions)
        {
            StringBuilder sb = new StringBuilder(@"SELECT 
                                dir.RECORDID,
                                dir.RECORDCOUNT RecordCount,
                                dir.FAULTCOUNT FaultCount,
                                dir.CREATETIME,
                                dir.CREATEBY,
                                dir.DELIVERYSOURCE,
                                dir.NOTE,
                                dir.BATCHNO,
                                dir.FILEPATH,
                                e.employeeName CREATEBYNAME 
                                FROM tms_deliveryinrecord dir
                                LEFT JOIN employee e ON e.employeeid = dir.CREATEBY
                                WHERE dir.ISDELETED=0");
            List<OracleParameter> arguments = new List<OracleParameter>();
            if (conditions != null)
            {
                if (conditions.CreateTime != null)
                {
                    sb.Append(" AND dir.CreateTime>=:CreateTime ");
                    arguments.Add(new OracleParameter() { ParameterName = "CreateTime", OracleDbType = OracleDbType.Date, Value = conditions.CreateTime });
                }

                if (conditions.DeliverySource != null)
                {
                    sb.Append(" AND dir.DELIVERYSOURCE=:DELIVERYSOURCE ");
                    arguments.Add(new OracleParameter() { ParameterName = "DELIVERYSOURCE", OracleDbType = OracleDbType.Int32, Value = (int)conditions.DeliverySource.Value });
                }
            }

            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<DeliveryInRecordModel>(TMSReadOnlyConnection, sb.ToString(), conditions, arguments.ToArray());
        }

        #endregion
    }
}
