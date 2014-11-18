using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Formula.Common;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Formula.Common
{
    public class SerialNumberGenerateFormulaDAL : BaseDAL, ISerialNumberGenerateFormulaDAL
    {
        #region ISerialNumberGenerateFormulaDAL 成员

        public string GetNextNumber(string numberType)
        {
            return base.GetNextSequence(numberType).ToString();
        }

        #endregion

        #region ISerialNumberGenerateFormulaDAL 成员

        /// <summary>
        /// 自定义流水码通用PL-SQL
        /// </summary>
        private static readonly string sql = @"
BEGIN
     SELECT LASTNO+1  INTO :V_MaxValue 
     FROM TMS_SequenceNo
     WHERE NoType = :V_NoType AND CURRENTDATE = :V_CurrentDate AND IsDeleted = 0
     FOR UPDATE OF LASTNO;
     --UPDATE
     UPDATE TMS_SequenceNo
     SET CURRENTDATE = :V_CurrentDate,LASTNO = :V_MaxValue, UpdateTime = SYSDATE
     WHERE NoType = :V_NoType AND IsDeleted = 0;
EXCEPTION  
    WHEN NO_DATA_FOUND THEN
        :V_MaxValue := 1;
        UPDATE TMS_SequenceNo
        SET CURRENTDATE = :V_CurrentDate,LASTNO = :V_MaxValue, UpdateTime = SYSDATE
        WHERE NoType = :V_NoType AND IsDeleted = 0;
        COMMIT;
    WHEN OTHERS  THEN
        ROLLBACK;  
END;
";

        /// <summary>
        /// 取得默认流水码
        /// </summary>
        /// <returns></returns>
        private int GetCommonSeqNo(Model.Common.Enums.SerialNumberType SeqNoType)
        {
            int DefaultValue = -1;
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="V_MaxValue", DbType = System.Data.DbType.Int32, Direction = System.Data.ParameterDirection.InputOutput, Value = DefaultValue },
                new OracleParameter(){ ParameterName="V_CurrentDate", DbType = System.Data.DbType.String, Value=DateTime.Now.ToString("yyyyMMdd") },
                new OracleParameter(){ ParameterName="V_NoType" , DbType = System.Data.DbType.Int32, Value = (int)SeqNoType }
            };
            ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
            return Convert.ToInt32(arguments[0].Value);
        }

        public int GetUserDefinedSeqNo(Model.Common.Enums.SerialNumberType SeqNoType)
        {
            int Result = -1;
            switch (SeqNoType)
            {
                case Enums.SerialNumberType.DeliveryNo:
                    Result = GetCommonSeqNo(Enums.SerialNumberType.DeliveryNo);
                    break;

                default:
                    Result = GetCommonSeqNo(SeqNoType);
                    break;
            }
            return Result;
        }

        #endregion
    }
}
