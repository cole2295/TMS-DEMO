using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Vancl.TMS.Model.BaseInfo.Line;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.BaseInfo.Line
{
    public class LineLadderPriceDAL : BaseDAL, ILineLadderPriceDAL
    {
        #region ILineLadderPriceDAL 成员

        public int Add(LineLadderPriceModel model)
        {
            string sql = string.Format(@"INSERT INTO TMS_LineLadderPrice 
                        (
                            LLPID,
                            LPID,
                            StartWeight,
                            EndWeight,
                            Price,
                            Note,
                            CreateBy,
                            UpdateBy,
                            IsDeleted
                        ) 
                        VALUES 
                        (
                            {0},
                            :LPID,
                            :StartWeight,
                            :EndWeight,
                            :Price,
                            :Note,
                            :CreateBy,
                            :UpdateBy,
                            0
                        )", model.SequenceNextValue());
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = model.LPID},
                new OracleParameter(){ParameterName = "StartWeight" ,DbType = DbType.Decimal , Value = model.StartWeight},
                new OracleParameter(){ParameterName = "EndWeight" ,DbType = DbType.Decimal , Value = model.EndWeight},
                new OracleParameter(){ParameterName = "Price" ,DbType = DbType.Decimal , Value = model.Price},
                new OracleParameter(){ParameterName = "Note" ,DbType = DbType.String , Value = model.Note},
                new OracleParameter(){ParameterName = "CreateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID},
                new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        public IList<LineLadderPriceModel> GetByLinePlanID(int lpid)
        {
            string sql = "SELECT * FROM TMS_LineLadderPrice WHERE LPID=:LPID AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = lpid}
            };
            return ExecuteSql_ByReaderReflect<LineLadderPriceModel>(TMSReadOnlyConnection, sql, parameters);
        }

        public int Delete(List<int> lpidList)
        {
            if (lpidList == null)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE TMS_LineLadderPrice
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE LPID =:LPIDs
                    AND IsDeleted=0", UserContext.CurrentUser.ID);
            //int[] arr = lpids.Split(',').ConvertArray<int>();
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="LPIDs",DbType= DbType.Int32,Value=lpidList.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lpidList.Count , arguments);
        }

        #endregion
    }
}
