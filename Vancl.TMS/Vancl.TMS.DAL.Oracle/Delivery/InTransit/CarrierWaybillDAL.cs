using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Delivery.InTransit;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Delivery.InTransit;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.Delivery.InTransit
{
    public class CarrierWaybillDAL : BaseDAL, ICarrierWaybillDAL
    {
        #region ICarrierWaybillDAL 成员

        public int Add(CarrierWaybillModel model)
        {
            string strSql = @"
                INSERT INTO TMS_CarrierWaybill(
                    CWID,
                    WaybillNo,
                    TotalCount,
                    CreateBy,
                    UpdateBy
                 )
                VALUES(
                    :CWID,
                    :WaybillNo,
                    :TotalCount,
                    :CreateBy,
                    :UpdateBy
                )";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CWID",DbType= DbType.Int64,Value=model.CWID},
                new OracleParameter() { ParameterName="WaybillNo",DbType= DbType.String,Value=model.WaybillNo},
                new OracleParameter() { ParameterName="TotalCount",DbType= DbType.Int32,Value=model.TotalCount},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Delete(List<long> lstCwid)
        {
            if (lstCwid == null || lstCwid.Count == 0)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE TMS_CarrierWaybill
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE CWID =:CWIDs
                    AND IsDeleted=0", UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CWIDs",DbType= DbType.Int64,Value=lstCwid.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lstCwid.Count, arguments);
        }

        public int Update(CarrierWaybillModel model)
        {
            string strSql = @"
                UPDATE TMS_CarrierWaybill
                SET WaybillNo=:WaybillNo
                    ,TotalCount=:TotalCount
                    ,Weight=:Weight
                    ,ArrivalTime=:ArrivalTime
                    ,UpdateBy=:UpdateBy
                    ,UpdateTime=sysdate
                WHERE CWID=:CWID
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="WaybillNo",DbType= DbType.String,Value=model.WaybillNo},
                new OracleParameter() { ParameterName="TotalCount",DbType= DbType.Int32,Value=model.TotalCount},
                new OracleParameter() { ParameterName="Weight",DbType= DbType.Decimal,Value=model.Weight},
                new OracleParameter() { ParameterName="ArrivalTime",DbType= DbType.DateTime,Value=model.ArrivalTime},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="CWID",DbType= DbType.Int64,Value=model.CWID},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public CarrierWaybillModel Get(long cwid)
        {
            string strSql = @"
                SELECT CWID,WaybillNo,TotalCount,Weight
                    ,ArrivalTime,CreateBy,CreateTime
                    ,UpdateBy,UpdateTime,IsDeleted
                FROM TMS_CarrierWaybill
                WHERE CWID=:CWID
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CWID",DbType= DbType.Int64,Value=cwid}
            };
            return ExecuteSqlSingle_ByReaderReflect<CarrierWaybillModel>(TMSReadOnlyConnection, strSql, arguments);
        }


        public CarrierWaybillModel GetByDispatchID(long dispatchID)
        {
            string strSql = @"
                SELECT TMS_Dispatch.DID,
                    TMS_CarrierWaybill.CWID,
                    TMS_CarrierWaybill.WaybillNo,
                    TMS_CarrierWaybill.TotalCount,
                    TMS_Dispatch.Boxcount,
                    TMS_CarrierWaybill.Weight,
                    TMS_CarrierWaybill.ArrivalTime,
                    TMS_CarrierWaybill.CreateBy,
                    TMS_CarrierWaybill.CreateTime,
                    TMS_CarrierWaybill.UpdateBy,
                    TMS_CarrierWaybill.UpdateTime,
                    TMS_CarrierWaybill.IsDeleted
                FROM TMS_CarrierWaybill
                JOIN TMS_Dispatch ON TMS_Dispatch.CarrierWaybillID = TMS_CarrierWaybill.CWID AND TMS_Dispatch.IsDeleted=0
                WHERE TMS_Dispatch.DID=:DispatchID
                    AND TMS_Dispatch.IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DispatchID",DbType= DbType.Int64,Value=dispatchID}
            };
            return ExecuteSqlSingle_ByReaderReflect<CarrierWaybillModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public string GetWaybillNoByID(long cwid)
        {
            string strSql = @"
                SELECT WaybillNo 
                FROM TMS_CarrierWaybill
                WHERE CWID=:CWID
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CWID",DbType= DbType.Int64,Value=cwid}
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToString(o);
            }
            return "";
        }

        public int UpdateWaybillNo(string waybillNo, long cwid)
        {
            string strSql = @"
                UPDATE TMS_CARRIERWAYBILL
                SET UPDATEBY=:UPDATEBY
                    ,UPDATETIME=sysdate
                    ,WAYBILLNO=:WAYBILLNO
                WHERE ISDELETED=0
                    AND CWID=:CWID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="UPDATEBY",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="WAYBILLNO",DbType= DbType.String,Value=waybillNo},
                new OracleParameter() { ParameterName="CWID",DbType= DbType.Int64,Value= cwid}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int AddEx(CarrierWaybillModel model)
        {
            string strSql = @"
                INSERT INTO TMS_CarrierWaybill(
                    CWID,
                    WaybillNo,
                    TotalCount,
                    CreateBy,
                    UpdateBy,
                    Weight
                 )
                VALUES(
                    :CWID,
                    :WaybillNo,
                    :TotalCount,
                    :CreateBy,
                    :UpdateBy,
                    :Weight
                )";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CWID",DbType= DbType.Int64,Value=model.CWID},
                new OracleParameter() { ParameterName="WaybillNo",DbType= DbType.String,Value=model.WaybillNo},
                new OracleParameter() { ParameterName="TotalCount",DbType= DbType.Int32,Value=model.TotalCount},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="Weight",DbType= DbType.Decimal,Value=model.Weight}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }
        #endregion
    }
}
