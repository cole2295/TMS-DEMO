using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Oracle.DataAccess.Client;
using Vancl.TMS.IDAL.BaseInfo.Carrier;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Exceptions;

namespace Vancl.TMS.DAL.Oracle.BaseInfo.Carrier
{
    public class DelayCriteriaDAL : BaseDAL, IDelayCriteriaDAL
    {
        /// <summary>
        /// 新增承运商延误标准
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(DelayCriteriaModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO TMS_DELAYCRITERIA(
                    DCID,
                    CARRIERID,
                    STARTREGION,
                    ENDREGION,
                    DISCOUNT,
                    CreateBy,
                    UpdateBy,
                    IsDeleted
                 )
                VALUES(
                    {0},
                    :CARRIERID,
                    :STARTREGION,
                    :ENDREGION,
                    :DISCOUNT,
                    :CreateBy,
                    :UpdateBy,
                    0
                )", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CARRIERID",DbType= DbType.Int32,Value=model.CarrierID},
                new OracleParameter() { ParameterName="STARTREGION",DbType= DbType.Int32,Value=model.StartRegion},
                new OracleParameter() { ParameterName="ENDREGION",DbType= DbType.Int32,Value=model.EndRegion},
                new OracleParameter() { ParameterName="DISCOUNT",DbType= DbType.Decimal,Value=model.Discount},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        /// <summary>
        /// 查询指定承运商延误考核标准
        /// </summary>
        /// <param name="strCarrierID">承运商ID</param>
        /// <returns></returns>
        public IList<DelayCriteriaModel> GetByCarrierID(int carrierID)
        {
            string strSql = @"select DCID,CARRIERID,STARTREGION,ENDREGION,DISCOUNT ,CreateBy,CreateTime,UpdateBy,UpdateTime,IsDeleted
                            from TMS_DELAYCRITERIA where CARRIERID = :CARRIERID AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CARRIERID" , DbType= DbType.Int32 , Value=carrierID }};
            return ExecuteSql_ByReaderReflect<DelayCriteriaModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        /// <summary>
        /// 删除指定承运商延误考核标准
        /// </summary>
        /// <param name="strCarrierID"></param>
        /// <returns></returns>
        public int Delete(IList<int> carrierIDs)
        {
            if (carrierIDs == null || carrierIDs.Count == 0)
            {
                throw new ArgumentNullException("carrierIDs");
            }
            string strSql = string.Format(@"
                UPDATE TMS_DelayCriteria
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE CarrierID =:CarrierIDs
                    AND IsDeleted=0", UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CarrierIDs",DbType= DbType.Int32,Value=carrierIDs.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, carrierIDs.Count, arguments);
        }

        public decimal? GetDisCount(int carrierID, decimal delayTimeSpan)
        {
            if (carrierID <= 0)
            {
                throw new CodeNotValidException();
            }
            if (delayTimeSpan <= 0)
            {
                return null;
            }
            string strSql = @"
                SELECT Discount,EndRegion
                FROM TMS_DelayCriteria
                WHERE IsDeleted=0
                    AND CarrierID =:CarrierID
                    AND StartRegion<:Region
                ORDER BY EndRegion";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CarrierID",DbType= DbType.Int32,Value=carrierID},
                new OracleParameter() { ParameterName="Region",DbType= DbType.Decimal,Value=delayTimeSpan}
            };
            DataTable dt = ExecuteSqlDataTable(TMSReadOnlyConnection, strSql, arguments);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["EndRegion"] == DBNull.Value)
                {
                    return Convert.ToDecimal(dt.Rows[i]["Discount"]);
                }
                if (Convert.ToDecimal(dt.Rows[i]["EndRegion"]) >= delayTimeSpan)
                {
                    return Convert.ToDecimal(dt.Rows[i]["Discount"]);
                }
            }
            return null;
        }
    }
}
