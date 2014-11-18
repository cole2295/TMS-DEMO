using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Claim;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Claim.Lost;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.Claim
{
    public class LostDetailDAL : BaseDAL, ILostDetailDAL
    {
        #region ILostDetailDAL 成员

        public int Add(LostDetailModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO TMS_LostDetail(
                    LDID
                    ,LID
                    ,BoxNo
                    ,FormCode
                    ,Price
                    ,ProtectedPrice
                    ,DeliveryNo
                    ,CreateBy
                    ,UpdateBy
                    ,IsDeleted)
                VALUES(
                    {0}
                    ,:LID
                    ,:BoxNo
                    ,:FormCode
                    ,:Price
                    ,:ProtectedPrice
                    ,:DeliveryNo
                    ,:CreateBy
                    ,:UpdateBy
                    ,0)", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "LID", DbType = DbType.Int64, Value = model.LID },
                new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = model.BoxNo },
                new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = model.FormCode },
                new OracleParameter() { ParameterName = "Price", DbType = DbType.Decimal, Value = model.Price },
                new OracleParameter() { ParameterName = "ProtectedPrice", DbType = DbType.Decimal, Value = model.ProtectedPrice },
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = model.DeliveryNo },
                new OracleParameter() { ParameterName = "CreateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Delete(string deliveryNo)
        {
            string strSql = @"
                UPDATE TMS_LostDetail
                SET IsDeleted=1
                    ,UpdateBy=:UpdateBy
                    ,UpdateTime=sysdate
                WHERE IsDeleted=0
                    AND DeliveryNo = :DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo },
                new OracleParameter() { ParameterName = "UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }
        #endregion
    }
}
