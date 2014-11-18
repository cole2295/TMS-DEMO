using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Model.Transport.Dispatch;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Exceptions;

namespace Vancl.TMS.DAL.Oracle.Transport.Dispatch
{
    public class DispTransitionDAL : BaseDAL, IDispTransitionDAL
    {
        #region IDispTransitionDAL 成员

        public int Add(DispTransitionModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO TMS_DispTransition (
                    DTID
                    ,DeliveryNo
                    ,PlateNo
                    ,Consignor
                    ,Consignee
                    ,ConsigneePhone
                    ,ReceiveAddress
                    ,CreateBy
                    ,UpdateBy)
                VALUES (
                    {0}
                    ,:DeliveryNo
                    ,:PlateNo
                    ,:Consignor
                    ,:Consignee
                    ,:ConsigneePhone
                    ,:ReceiveAddress
                    ,:CreateBy
                    ,:UpdateBy)", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter(){  ParameterName="DeliveryNo", DbType = DbType.String, Value = model.DeliveryNo},
                new OracleParameter(){ ParameterName="PlateNo", DbType=DbType.String, Value= model.PlateNo},
                new OracleParameter() { ParameterName="Consignor", DbType= DbType.String, Value= model.Consignor},
                new OracleParameter() { ParameterName="Consignee", DbType= DbType.String, Value= model.Consignee},
                new OracleParameter() { ParameterName="ConsigneePhone", DbType= DbType.String, Value= model.ConsigneePhone},
                new OracleParameter() { ParameterName="ReceiveAddress", DbType= DbType.String, Value= model.ReceiveAddress},
                new OracleParameter() { ParameterName="CreateBy", DbType= DbType.Int32, Value= UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value=  UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Delete(string deliveryNo)
        {
            string strSql = @"
                UPDATE TMS_DispTransition
                SET IsDeleted=1
                    ,UpdateTime=sysdate
                    ,UpdateBy=:UpdateBy
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter(){  ParameterName="UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID},
                new OracleParameter(){  ParameterName="DeliveryNo", DbType = DbType.String, Value = deliveryNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Update(DispTransitionModel model)
        {
            string strSql = @"
                UPDATE TMS_DispTransition
                SET PlateNo=:PlateNo
                    ,Consignor=:Consignor
                    ,Consignee=:Consignee
                    ,ConsigneePhone=:ConsigneePhone
                    ,ReceiveAddress=:ReceiveAddress
                    ,UpdateTime=sysdate
                    ,UpdateBy=:UpdateBy
                WHERE DTID=:DTID
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter(){  ParameterName="PlateNo", DbType = DbType.String, Value = model.PlateNo},
                new OracleParameter(){  ParameterName="Consignor", DbType = DbType.String, Value = model.Consignor},
                new OracleParameter(){  ParameterName="Consignee", DbType = DbType.String, Value = model.Consignee},
                new OracleParameter(){  ParameterName="ConsigneePhone", DbType = DbType.String, Value = model.ConsigneePhone},
                new OracleParameter(){  ParameterName="ReceiveAddress", DbType = DbType.String, Value = model.ReceiveAddress},
                new OracleParameter(){  ParameterName="UpdateBy", DbType = DbType.Int32, Value = UserContext.CurrentUser.ID},
                new OracleParameter(){  ParameterName="DTID", DbType = DbType.Int64, Value = model.DTID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public DispTransitionModel Get(string deliveryNo)
        {
            string strSql = @"
                SELECT DTID
                    ,DeliveryNo
                    ,PlateNo
                    ,Consignor
                    ,Consignee
                    ,ConsigneePhone
                    ,ReceiveAddress
                    ,CreateBy
                    ,CreateTime
                    ,UpdateBy
                    ,UpdateTime
                    ,IsDeleted
                FROM TMS_DispTransition
                WHERE IsDeleted=0
                    AND DeliveryNo=:DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter(){  ParameterName="DeliveryNo", DbType = DbType.String, Value =deliveryNo}
            };
            return ExecuteSqlSingle_ByReaderReflect<DispTransitionModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        #endregion
    }
}
