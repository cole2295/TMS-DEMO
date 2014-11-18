using System;
using System.Collections.Generic;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Carrier;
using System.Data;
using System.Linq;
using Oracle.DataAccess.Client;
using Vancl.TMS.IDAL.BaseInfo.Carrier;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.BaseInfo.Carrier
{
    public class CarrierDAL : BaseDAL, ICarrierDAL
    {
        #region ICarrierDAL 成员

        public int Add(CarrierModel model)
        {
            string sql = @"
                INSERT INTO TMS_CARRIER(
                    CarrierID,
                    CarrierNo,
                    CarrierName,
                    CarrierAllName,
                    Contacter,
                    IsAllCoverage,
                    Phone,
                    Address,
                    Email,
                    Status,
                    CreateBy,
                    UpdateBy,
                    ExpiredDate,
                    BeginDate,
                    DistributionCode,
                    IsDeleted,
                    ContractNumber
                 )
                VALUES(
                    :CarrierID,
                    :CarrierNo,
                    :CarrierName,
                    :CarrierAllName,
                    :Contacter,
                    :IsAllCoverage,
                    :Phone,
                    :Address,
                    :Email,
                    :Status,
                    :CreateBy,
                    :UpdateBy,
                    :ExpiredDate,
                    :BeginDate,
                    :DistributionCode,
                    0,
                    :ContractNumber
                )";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CarrierID",DbType= DbType.Int32,Value=model.CarrierID},
                new OracleParameter() { ParameterName="CarrierNo",DbType= DbType.String,Value=model.CarrierNo},
                new OracleParameter() { ParameterName="CarrierName",DbType= DbType.String,Value=model.CarrierName},
                new OracleParameter() { ParameterName="CarrierAllName",DbType= DbType.String,Value=model.CarrierAllName},
                new OracleParameter() { ParameterName="Contacter",DbType= DbType.String,Value=model.Contacter},
                new OracleParameter() { ParameterName="IsAllCoverage",DbType= DbType.Byte,Value=model.IsAllCoverage},
                new OracleParameter() { ParameterName="Phone",DbType= DbType.String,Value=model.Phone},
                new OracleParameter() { ParameterName="Address",DbType= DbType.String,Value=model.Address},
                new OracleParameter() { ParameterName="Email",DbType= DbType.String,Value=model.Email},
                new OracleParameter() { ParameterName="Status",DbType= DbType.Int32,Value=(int)model.Status},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="ExpiredDate",DbType= DbType.DateTime,Value=model.ExpiredDate},
                new OracleParameter() { ParameterName="BeginDate",DbType= DbType.DateTime,Value=model.BeginDate},
                new OracleParameter() { ParameterName="DistributionCode",DbType= DbType.String,Value=model.DistributionCode},
                new OracleParameter() { ParameterName="ContractNumber",DbType= DbType.String,Value=model.ContractNumber}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public CarrierModel Get(int id)
        {
            string sql = @"
                SELECT carrierid,CarrierNo, carriername, carrierallname, contacter, isallcoverage
                    , phone, address, email, status, createby, createtime, updateby, updatetime
                    , expireddate, isdeleted,distributioncode,begindate,ContractNumber 
                FROM tms_carrier 
                WHERE carrierid=:CarrierID
                    AND isdeleted=0";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = id } 
            };
            return ExecuteSqlSingle_ByReaderReflect<CarrierModel>(TMSReadOnlyConnection, sql, arguments);
        }

        public int Update(CarrierModel model)
        {
            string strSql = @"
                UPDATE TMS_CARRIER
                SET CarrierNo=:CarrierNo
                    ,CarrierName=:CarrierName
                    ,CarrierAllName=:CarrierAllName
                    ,Contacter=:Contacter
                    ,IsAllCoverage=:IsAllCoverage
                    ,Phone=:Phone
                    ,Address=:Address
                    ,Email=:Email
                    ,Status=:Status
                    ,UpdateBy=:UpdateBy
                    ,UpdateTime=sysdate
                    ,ExpiredDate=:ExpiredDate
                    ,BeginDate=:BeginDate
                    ,ContractNumber=:ContractNumber
                WHERE CarrierID=:CarrierID
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CarrierNo",DbType= DbType.String,Value=model.CarrierNo},
                new OracleParameter() { ParameterName="CarrierName",DbType= DbType.String,Value=model.CarrierName},
                new OracleParameter() { ParameterName="CarrierAllName",DbType= DbType.String,Value=model.CarrierAllName},
                new OracleParameter() { ParameterName="Contacter",DbType= DbType.String,Value=model.Contacter},
                new OracleParameter() { ParameterName="IsAllCoverage",DbType= DbType.Byte,Value=model.IsAllCoverage},
                new OracleParameter() { ParameterName="Phone",DbType= DbType.String,Value=model.Phone},
                new OracleParameter() { ParameterName="Address",DbType= DbType.String,Value=model.Address},
                new OracleParameter() { ParameterName="Email",DbType= DbType.String,Value=model.Email},
                new OracleParameter() { ParameterName="Status",DbType= DbType.Int32,Value=(int)model.Status},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="ExpiredDate",DbType= DbType.DateTime,Value=model.ExpiredDate},
                new OracleParameter() { ParameterName="BeginDate",DbType= DbType.DateTime,Value=model.BeginDate},
                new OracleParameter() { ParameterName="CarrierID",DbType= DbType.String,Value=model.CarrierID},
                new OracleParameter() { ParameterName="ContractNumber",DbType= DbType.String,Value=model.ContractNumber},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Delete(IList<int> ids)
        {
            if (ids.Count == 0)
            {
                return 0;
            }
            string strSql = string.Format(@"
                UPDATE TMS_CARRIER
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE CarrierID =:CarrierIDs
                    AND IsDeleted=0", UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CarrierIDs",DbType= DbType.Int32,Value=ids.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, ids.Count, arguments);
        }

        public bool IsExistCarrierName(string name, int carrierID)
        {
            var paramList = new List<OracleParameter>();
            string sql = @"
                SELECT COUNT(*) AS CT FROM TMS_CARRIER
                WHERE CarrierName = :CarrierName 
                AND IsDeleted=0";
            paramList.Add(new OracleParameter() { ParameterName = "CarrierName", DbType = DbType.String, Value = name });

            if (carrierID > 0)
            {
                sql += " AND CarrierID != :CarrierID";
                paramList.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = carrierID });
            }
            object value = ExecuteSqlScalar(TMSReadOnlyConnection, sql, paramList.ToArray());
            int nCount = Convert.ToInt32(value);
            return nCount > 0;
        }

        public bool IsExistContractNumber(string contractNumber, int carrierID)
        {
            var paramList = new List<OracleParameter>();
            string sql = @"
                SELECT COUNT(*) AS CT FROM TMS_CARRIER
                WHERE ContractNumber = :ContractNumber 
                AND IsDeleted=0";
            paramList.Add(new OracleParameter() { ParameterName = "ContractNumber", DbType = DbType.String, Value = contractNumber });

            if (carrierID > 0)
            {
                sql += " AND CarrierID != :CarrierID";
                paramList.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = carrierID });
            }
            object value = ExecuteSqlScalar(TMSReadOnlyConnection, sql, paramList.ToArray());
            int nCount = Convert.ToInt32(value);
            return nCount > 0;
        }

        public bool IsExistCarrierAllName(string name, int carrierID)
        {
            var paramList = new List<OracleParameter>();
            string sql = @"
                SELECT COUNT(*) AS CT FROM TMS_CARRIER
                WHERE CarrierAllName = :CarrierAllName 
                AND IsDeleted=0";
            paramList.Add(new OracleParameter() { ParameterName = "CarrierAllName", DbType = DbType.String, Value = name });

            if (carrierID > 0)
            {
                sql += " AND CarrierID != :CarrierID";
                paramList.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.String, Value = carrierID });
            }
            object value = ExecuteSqlScalar(TMSReadOnlyConnection, sql, paramList.ToArray());
            int nCount = Convert.ToInt32(value);
            return nCount > 0;
        }

        public bool IsExistEmail(string email, int carrierID)
        {
            var paramList = new List<OracleParameter>();
            string sql = @"
                SELECT COUNT(*) AS CT FROM TMS_CARRIER
                WHERE Email = :Email
                AND IsDeleted=0";
            paramList.Add(new OracleParameter() { ParameterName = "Email", DbType = DbType.String, Value = email });

            if (carrierID > 0)
            {
                sql += " AND CarrierID != :CarrierID";
                paramList.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.Int32, Value = carrierID });
            }
            object value = ExecuteSqlScalar(TMSReadOnlyConnection, sql, paramList.ToArray());
            int nCount = Convert.ToInt32(value);
            return nCount > 0;
        }

        public PagedList<CarrierModel> Search(CarrierSearchModel searchModel)
        {
            string sql = @"
                SELECT  carrierid,CarrierNo, carriername, carrierallname, contacter, isallcoverage, phone, address, email, 
                    status, createby, createtime, updateby, updatetime, expireddate, isdeleted ,distributioncode,begindate,ContractNumber 
                FROM TMS_Carrier
                WHERE IsDeleted=0 ";

            var paramList = new List<OracleParameter>();

            if (!string.IsNullOrWhiteSpace(searchModel.CarrierName))
            {
                sql += " AND CarrierName like :CarrierName || '%' ";
                paramList.Add(new OracleParameter() { ParameterName = "CarrierName", DbType = DbType.String, Value = searchModel.CarrierName });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.CarrierAllName))
            {
                sql += " AND CarrierAllName like :CarrierAllName || '%' ";
                paramList.Add(new OracleParameter() { ParameterName = "CarrierAllName", DbType = DbType.String, Value = searchModel.CarrierAllName });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.Contacter))
            {
                sql += " AND Contacter like :Contacter || '%' ";
                paramList.Add(new OracleParameter() { ParameterName = "Contacter", DbType = DbType.String, Value = searchModel.Contacter });
            }
            if (searchModel.Status != null)
            {
                sql += " AND Status = :Status ";
                paramList.Add(new OracleParameter() { ParameterName = "Status", DbType = DbType.Int32, Value = (int)searchModel.Status });
            }
            if (!string.IsNullOrEmpty(searchModel.ContractNumber))
            {
                sql += " AND ContractNumber like :ContractNumber|| '%'";
                paramList.Add(new OracleParameter() { ParameterName = "ContractNumber", DbType = DbType.String, Value = searchModel.ContractNumber });
            }

            sql += " ORDER BY createtime DESC ";

            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<CarrierModel>(TMSReadOnlyConnection, sql, searchModel, paramList.ToArray());
        }


        public IList<CarrierModel> GetAll()
        {
            string sql = string.Format(@"
                SELECT carrierid,CarrierNo, carriername, carrierallname, contacter, isallcoverage, phone, address, email, 
                    status, createby, createtime, updateby, updatetime, expireddate, isdeleted ,distributioncode,begindate
                FROM TMS_Carrier
                WHERE IsDeleted=0
                    AND Status={0}
                ORDER BY carriername", (int)Enums.CarrierStatus.Valid);
            return ExecuteSql_ByReaderReflect<CarrierModel>(TMSReadOnlyConnection, sql);
        }

        #endregion

        #region ICarrierDAL 成员


        public int GetCarrierIdByName(string name, string distributionCode)
        {
            string sql = @"SELECT CarrierID FROM tms_carrier WHERE distributioncode=:distributioncode AND carriername=:carriername AND isdeleted=0";

            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "distributioncode", DbType = DbType.String, Value = distributionCode }, 
                new OracleParameter() { ParameterName = "carriername", DbType = DbType.String, Value = name }
            };

            object o = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            int id = -1;
            if (o != null)
            {
                int.TryParse(o.ToString(), out id);
            }
            return id;
        }

        #endregion
    }
}
