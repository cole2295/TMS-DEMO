using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Synchronous.InSync
{
    public class Lms2TmsSyncTMSDAL : BaseDAL, ILms2TmsSyncTMSDAL
    {
        #region ILms2TmsSyncTMSDAL 成员

        public bool IsBillExists(string formCode)
        {
            string strSql = @"
                SELECT COUNT(BID)
                FROM SC_Bill
                WHERE FormCode=:FormCode
                    AND IsDeleted=0";
            OracleParameter[] parameters = { new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } };
            object o = ExecuteSqlScalar(TMSWriteConnection, strSql, parameters);
            if (o == null || o == DBNull.Value)
            {
                return false;
            }
            return Convert.ToInt32(o) > 0;
        }

        public bool IsBillInfoExists(string formCode)
        {
            string strSql = @"
                SELECT COUNT(BIID)
                FROM SC_BillInfo
                WHERE FormCode=:FormCode
                    AND IsDeleted=0";
            OracleParameter[] parameters = { new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } };
            object o = ExecuteSqlScalar(TMSWriteConnection, strSql, parameters);
            if (o == null || o == DBNull.Value)
            {
                return false;
            }
            return Convert.ToInt32(o) > 0;
        }

        public int AddBill(BillModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO SC_Bill(
                    BID
                    ,FormCode
                    ,Status
                    ,BillType
                    ,CustomerOrder
                    ,DeliverStationID
                    ,Source
                    ,MerchantID
                    ,DistributionCode
                    ,CurrentDistributionCode
                    ,WarehouseID
                    ,DeliverCode
                    ,ReturnStatus
                    ,CreateBy
                    ,CreateDept
                    ,CreateTime
                    ,UpdateBy
                    ,UpdateDept
                    ,UpdateTime)
                VALUES(
                    {0}
                    ,:FormCode
                    ,:Status
                    ,:BillType
                    ,:CustomerOrder
                    ,:DeliverStationID
                    ,:Source
                    ,:MerchantID
                    ,:DistributionCode
                    ,:CurrentDistributionCode
                    ,:WarehouseID
                    ,:DeliverCode
                    ,:ReturnStatus
                    ,:CreateBy
                    ,:CreateDept
                    , sysdate
                    ,:UpdateBy
                    ,:UpdateDept
                    ,:UpdateTime)", model.SequenceNextValue());
            OracleParameter[] parameters = { 
                new OracleParameter() { ParameterName="FormCode",DbType= DbType.String,Value=model.FormCode},
                new OracleParameter() { ParameterName="Status",DbType= DbType.Int32,Value=(int)model.Status}, 
                new OracleParameter() { ParameterName="BillType",DbType= DbType.Int32,Value=(int)model.BillType},
                new OracleParameter() { ParameterName="CustomerOrder",DbType= DbType.String,Value=model.CustomerOrder},
                new OracleParameter() { ParameterName="DeliverStationID",DbType= DbType.Int32,Value=model.DeliverStationID},
                new OracleParameter() { ParameterName="Source",DbType= DbType.Int32,Value=(int)model.Source},
                new OracleParameter() { ParameterName="MerchantID",DbType= DbType.Int32,Value=model.MerchantID},
                new OracleParameter() { ParameterName="DistributionCode",DbType= DbType.String,Value=model.DistributionCode},
                new OracleParameter() { ParameterName="CurrentDistributionCode",DbType= DbType.String,Value=model.CurrentDistributionCode},
                new OracleParameter() { ParameterName="WarehouseID",DbType= DbType.String,Value=model.WarehouseID},
                new OracleParameter() { ParameterName="DeliverCode",DbType= DbType.String,Value=model.DeliverCode},
                new OracleParameter() { ParameterName="ReturnStatus",DbType= DbType.Int32,Value = model.ReturnStatus.HasValue ? (int)model.ReturnStatus.Value : (int?)null},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=model.CreateBy},
                new OracleParameter() { ParameterName="CreateDept",DbType= DbType.Int32,Value=model.CreateDept},
                //new OracleParameter() { ParameterName="CreateTime",DbType= DbType.DateTime,Value=model.CreateTime},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=model.UpdateBy},
                new OracleParameter() { ParameterName="UpdateDept",DbType= DbType.Int32,Value=model.UpdateDept},
                new OracleParameter() { ParameterName="UpdateTime",DbType= DbType.DateTime,Value=model.UpdateTime}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        public int AddBillInfo(BillInfoModel model)
        {
            string strSql = string.Format(@"
                INSERT INTO SC_BillInfo(
                    BIID
                    ,FormCode
                    ,CustomerWeight
                    ,CustomerBoxNo
                    ,BillGoodsType
                    ,Weight
                    ,PayType
                    ,ReceivableAmount
                    ,InsuredAmount
                    ,Tips
                    ,PackageMode
                    ,PackageCount
                    ,TotalAmount)
                VALUES(
                    {0}
                    ,:FormCode
                    ,:CustomerWeight
                    ,:CustomerBoxNo
                    ,:BillGoodsType
                    ,:Weight
                    ,:PayType
                    ,:ReceivableAmount
                    ,:InsuredAmount
                    ,:Tips
                    ,:PackageMode
                    ,:PackageCount
                    ,:TotalAmount)", model.SequenceNextValue());
            OracleParameter[] parameters = { 
                new OracleParameter() { ParameterName="FormCode",DbType= DbType.String,Value=model.FormCode},
                new OracleParameter() { ParameterName="CustomerWeight",DbType= DbType.Decimal,Value=model.CustomerWeight},
                new OracleParameter() { ParameterName="CustomerBoxNo",DbType= DbType.String,Value=model.CustomerBoxNo},
                new OracleParameter() { ParameterName="BillGoodsType",DbType= DbType.Int32,Value=(int)model.BillGoodsType},
                new OracleParameter() { ParameterName="Weight",DbType= DbType.Decimal,Value=model.Weight},
                new OracleParameter() { ParameterName="PayType",DbType= DbType.Int32,Value=(int)model.PayType},
                new OracleParameter() { ParameterName="ReceivableAmount",DbType= DbType.Decimal,Value=model.ReceivableAmount},
                new OracleParameter() { ParameterName="InsuredAmount",DbType= DbType.Decimal,Value=model.InsuredAmount},
                new OracleParameter() { ParameterName="Tips",DbType= DbType.String,Value=model.Tips},
                new OracleParameter() { ParameterName="PackageMode",DbType= DbType.String,Value=model.PackageMode},
                new OracleParameter() { ParameterName="PackageCount",DbType= DbType.Int32,Value=model.PackageCount},
                new OracleParameter() { ParameterName="TotalAmount",DbType= DbType.Decimal,Value=model.TotalAmount}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        #endregion
    }
}
