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
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.BaseInfo.Line
{
    public class LineFormulaPriceDAL : BaseDAL, ILineFormulaPriceDAL
    {
        #region ILineFormulaPriceDAL 成员

        public int Add(LineFormulaPriceModel model)
        {
            string sql = @"INSERT INTO TMS_LineFormulaPrice 
                        (
                            LPID,
                            BasePrice,
                            BaseWeight,
                            OverPrice,
                            Note,
                            CreateBy,
                            UpdateBy,
                            IsDeleted
                        ) 
                        VALUES 
                        (
                            :LPID,
                            :BasePrice,
                            :BaseWeight,
                            :OverPrice,
                            :Note,
                            :CreateBy,
                            :UpdateBy,
                            0
                        )";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = model.LPID},
                new OracleParameter(){ParameterName = "BasePrice" ,DbType = DbType.Decimal , Value = model.BasePrice},
                new OracleParameter(){ParameterName = "BaseWeight" ,DbType = DbType.Decimal , Value = model.BaseWeight},
                new OracleParameter(){ParameterName = "OverPrice" ,DbType = DbType.Decimal , Value = model.OverPrice},
                new OracleParameter(){ParameterName = "Note" ,DbType = DbType.String , Value = model.Note},
                new OracleParameter(){ParameterName = "CreateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID},
                new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        public LineFormulaPriceModel GetByLinePlanID(int lpid)
        {
            string sql = "SELECT * FROM TMS_LineFormulaPrice WHERE LPID=:LPID AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = lpid}
            };
            return ExecuteSqlSingle_ByReaderReflect<LineFormulaPriceModel>(TMSReadOnlyConnection, sql, parameters);
        }

        public int Update(LineFormulaPriceModel model)
        {
            string sql = @"UPDATE TMS_LineFormulaPrice 
                         SET 
                            BasePrice=:BasePrice,
                            BaseWeight=:BaseWeight,
                            OverPrice=:OverPrice,
                            Note=:Note,
                            UpdateBy=:UpdateBy,
                            UpdateTime=sysdate
                         WHERE
                            LPID=:LPID
                            AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = model.LPID},
                new OracleParameter(){ParameterName = "BasePrice" ,DbType = DbType.Decimal , Value = model.BasePrice},
                new OracleParameter(){ParameterName = "BaseWeight" ,DbType = DbType.Decimal , Value = model.BaseWeight},
                new OracleParameter(){ParameterName = "OverPrice" ,DbType = DbType.Decimal , Value = model.OverPrice},
                new OracleParameter(){ParameterName = "Note" ,DbType = DbType.String , Value = model.Note},
                new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        public bool IsExist(int lpid)
        {
            string sql = "SELECT COUNT(*) FROM TMS_LineFormulaPrice WHERE LPID=:LPID AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = lpid}
            };
            object o = base.ExecuteSqlScalar(TMSReadOnlyConnection, sql, parameters);

            return int.Parse(o.ToString()) > 0 ? true : false;
        }

        public int Delete(List<int> lpidList)
        {
            if (lpidList == null)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE TMS_LineFormulaPrice
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE LPID =:LPIDs
                    AND IsDeleted=0", UserContext.CurrentUser.ID);
            //int[] arr = lpids.Split(',').ConvertArray<int>();
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="LPIDs",DbType= DbType.Int32,Value=lpidList.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lpidList.Count, arguments);
        }
        #endregion

        #region ILineFormulaPriceDAL 成员


        public int AddDetail(List<LineFormulaPriceDetailModel> details)
        {
            if (details == null || details.Count <= 0) throw new Exception("公式续价明细信息不能为空!");

            string sql = string.Format(@"INSERT INTO TMS_LineFormulaEx 
                        (
                            LFEID,
                            LPID,
                            StartWeight,
                            EndWeight,
                            Price,
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
                            :CreateBy,
                            :UpdateBy,
                            0
                        )", details[0].SequenceNextValue());
            int[] LPID = new int[details.Count];
            int[] StartWeight = new int[details.Count];
            int?[] EndWeight = new int?[details.Count];
            decimal[] Price = new decimal[details.Count];
            int[] CreateBy = new int[details.Count];
            int[] UpdateBy = new int[details.Count];
            int index = 0;
            details.ForEach(d =>
                {
                    LPID[index] = d.LPID;
                    StartWeight[index] = d.StartWeight;
                    EndWeight[index] = d.EndWeight;
                    Price[index] = d.Price;
                    CreateBy[index] = UserContext.CurrentUser.ID;
                    UpdateBy[index] = UserContext.CurrentUser.ID;
                    index++;
                }
            );
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "LPID" ,DbType = DbType.Int32 , Value = LPID},
                new OracleParameter(){ParameterName = "StartWeight" ,DbType = DbType.Int32 , Value = StartWeight},
                new OracleParameter(){ParameterName = "EndWeight" ,DbType = DbType.Int32 , Value = EndWeight},
                new OracleParameter(){ParameterName = "Price" ,DbType = DbType.Decimal , Value = Price},
                new OracleParameter(){ParameterName = "CreateBy" ,DbType = DbType.Int32 , Value = CreateBy},
                new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UpdateBy}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, details.Count, parameters);
        }

        public List<LineFormulaPriceDetailModel> GetLineFormulaDetails(int lpid)
        {
            string sql = @"SELECT LFEID,LPID,STARTWEIGHT,ENDWEIGHT,PRICE,CREATEBY,CREATETIME,UPDATEBY,UPDATETIME
                           FROM TMS_LineFormulaEx 
                           WHERE LPID=:LPID AND ISDELETED=0";
            OracleParameter[] parameters = new OracleParameter[] { new OracleParameter() { ParameterName = "LPID", DbType = DbType.Int32, Value = lpid } };
            return ExecuteSql_ByReaderReflect<LineFormulaPriceDetailModel>(TMSReadOnlyConnection, sql, parameters).ToList();
        }

        public int DeleteDetails(int lpid)
        {
            string sql = @"UPDATE TMS_LineFormulaEx
                            SET ISDELETED=1
                            WHERE LPID=:LPID AND ISDELETED=0";
            OracleParameter[] parameters = new OracleParameter[] { new OracleParameter() { ParameterName = "LPID", DbType = DbType.Int32, Value = lpid } };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        #endregion
    }
}
