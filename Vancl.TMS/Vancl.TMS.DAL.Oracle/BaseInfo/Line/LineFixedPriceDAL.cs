using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo.Line;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.BaseInfo.Line;
using Vancl.TMS.Util.Converter;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.BaseInfo.Line
{
    public class LineFixedPriceDAL : BaseDAL, ILineFixedPriceDAL
    {
        #region ILineFixedPriceDAL 成员

        public int Add(LineFixedPriceModel model)
        {
            string sql = @"INSERT INTO TMS_LineFixedPrice 
                        (
                            LPID,
                            Price,
                            Note,
                            CreateBy,
                            UpdateBy,
                            IsDeleted
                        ) 
                        VALUES 
                        (
                            :LPID,
                            :Price,
                            :Note,
                            :CreateBy,
                            :UpdateBy,
                            0
                        )";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = model.LPID},
                new OracleParameter(){ParameterName = "Price" ,DbType = DbType.Decimal , Value = model.Price},
                new OracleParameter(){ParameterName = "Note" ,DbType = DbType.String , Value = model.Note},
                new OracleParameter(){ParameterName = "CreateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID},
                new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        public LineFixedPriceModel GetByLinePlanID(int lpid)
        {
            string sql = "SELECT * FROM TMS_LineFixedPrice WHERE LPID=:LPID AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = lpid}
            };
            return ExecuteSqlSingle_ByReaderReflect<LineFixedPriceModel>(TMSReadOnlyConnection, sql, parameters);
        }

        public int Update(LineFixedPriceModel model)
        {
            string sql = @"UPDATE TMS_LineFixedPrice 
                         SET 
                            Price=:Price,
                            Note=:Note,
                            UpdateBy=:UpdateBy,
                            UpdateTime=sysdate
                         WHERE
                            LPID=:LPID
                            AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = model.LPID},
                new OracleParameter(){ParameterName = "Price" ,DbType = DbType.Decimal , Value = model.Price},
                new OracleParameter(){ParameterName = "Note" ,DbType = DbType.String , Value = model.Note},
                new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        public bool IsExist(string linePlanID)
        {
            string sql = "SELECT COUNT(*) FROM TMS_LineFixedPrice WHERE LPID=:LPID AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = linePlanID}
            };

            object o = base.ExecuteSqlScalar(TMSReadOnlyConnection, sql, parameters);
            if (o.ToString() == "1")
                return true;
            else if (o.ToString() == "0")
                return false;
            else
                throw new Exception("数据错误:大于1条记录.");

        }

        public int Delete(List<int> lpidList)
        {
            if (lpidList==null)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE TMS_LineFixedPrice
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
