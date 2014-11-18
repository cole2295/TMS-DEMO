using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Delivery.KPIAppraisal;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Delivery.KPIAppraisal
{
    public class AssLadderPriceDAL : BaseDAL, IAssLadderPriceDAL
    {
        #region IAssLadderPriceDAL 成员

        public int Add(List<AssLadderPriceModel> lstModel)
        {
            if (lstModel == null || lstModel.Count == 0)
            {
                throw new CodeNotValidException();
            }
            string strSql = string.Format(@"
                INSERT INTO TMS_AssLadderPrice(
                    ALPID
                    ,DeliveryNo
                    ,StartWeight
                    ,EndWeight
                    ,Price
                    ,Note
                    ,CreateBy
                    ,UpdateBy
                    ,IsDeleted
                 )
                VALUES(
                    {0} 
                    ,:DeliveryNo
                    ,:StartWeight
                    ,:EndWeight
                    ,:Price
                    ,:Note
                    ,{1}
                    ,{2}
                    ,0
                )", lstModel[0].SequenceNextValue()
                  , UserContext.CurrentUser.ID
                  , UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=lstModel.Select(m=>m.DeliveryNo).ToArray()},
                new OracleParameter() { ParameterName="StartWeight",DbType= DbType.Int32,Value=lstModel.Select(m=>m.StartWeight).ToArray()},
                new OracleParameter() { ParameterName="EndWeight",DbType= DbType.Int32,Value=lstModel.Select(m=>m.EndWeight).ToArray()},
                new OracleParameter() { ParameterName="Price",DbType= DbType.Decimal,Value=lstModel.Select(m=>m.Price).ToArray()},
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value=lstModel.Select(m=>m.Note).ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lstModel.Count, arguments);
        }

        public int Delete(string deliveryNo)
        {
            string strSql = @"
                UPDATE TMS_AssLadderPrice
                SET IsDeleted=1
                    ,UpdateTime=sysdate
                    ,UpdateBy=:UpdateBy
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public bool IsExist(string deliveryNo)
        {
            string strSql = @"
                SELECT COUNT(1) CC
                FROM TMS_AssLadderPrice
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

        public List<AssLadderPriceModel> Get(string deliveryNo)
        {
            string strSql = @"
                SELECT
                    ALPID
                    ,DeliveryNo
                    ,StartWeight
                    ,EndWeight
                    ,Price
                    ,Note
                    ,CreateBy
                    ,CreateTime
                    ,UpdateBy
                    ,UpdateTime
                    ,IsDeleted
                FROM TMS_AssLadderPrice
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            return (List<AssLadderPriceModel>)ExecuteSql_ByReaderReflect<AssLadderPriceModel>(TMSReadOnlyConnection, strSql, arguments);
        }
        #endregion
    }
}
