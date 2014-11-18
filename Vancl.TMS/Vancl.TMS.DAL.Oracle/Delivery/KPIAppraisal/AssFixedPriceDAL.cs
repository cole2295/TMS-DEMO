using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Delivery.KPIAppraisal;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.Delivery.KPIAppraisal
{
    public class AssFixedPriceDAL : BaseDAL, IAssFixedPriceDAL
    {
        #region IAssFixedPriceDAL 成员

        public int Add(AssFixedPriceModel model)
        {
            string strSql = @"
                INSERT INTO TMS_AssFixedPrice(
                    DeliveryNo
                    ,Price
                    ,CreateBy
                    ,UpdateBy
                    ,IsDeleted
                )
                VALUES(
                    :DeliveryNo
                    ,:Price
                    ,:CreateBy
                    ,:UpdateBy
                    ,0
                )";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo},
                new OracleParameter() { ParameterName="Price",DbType= DbType.Decimal,Value=model.Price},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Update(AssFixedPriceModel model)
        {
            string strSql = @"
                UPDATE TMS_AssFixedPrice
                SET Price=:Price
                    ,UpdateTime=sysdate
                    ,UpdateBy=:UpdateBy
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="Price",DbType= DbType.Decimal,Value=model.Price},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public bool IsExist(string deliveryNo)
        {
            string strSql = @"
                SELECT COUNT(1) CC
                FROM TMS_AssFixedPrice
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                if (Convert.ToInt32(o) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public AssFixedPriceModel Get(string deliveryNo)
        {
            string strSql = @"
                SELECT
                    DeliveryNo
                    ,Price
                    ,CreateBy
                    ,CreateTime
                    ,UpdateBy
                    ,UpdateTime
                    ,IsDeleted
                FROM TMS_AssFixedPrice
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            return ExecuteSqlSingle_ByReaderReflect<AssFixedPriceModel>(TMSReadOnlyConnection, strSql, arguments);
        }
        #endregion
    }
}
