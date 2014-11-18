using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.IDAL.BaseInfo.Carrier;

namespace Vancl.TMS.DAL.Sql2008.BaseInfo
{
    public class CarrierDAL : BaseDAL, ICarrierDAL
    {
        #region ICarriersDAL 成员

        public int Add(CarrierModel model)
        {
            string strSql = @"
                INSERT INTO TMS_Carriers
                    ([CarriersID]
                    ,[CarriersName]
                    ,[CarriersAllName]
                    ,[LinkMan]
                    ,[Phone]
                    ,[Address]
                    ,[Email]
                    ,[Status]
                    ,[CreateBy]
                    ,[CreateTime]
                    ,[UpdateBy]
                    ,[UpdateTime]
                    ,[DeleteFlag]
                    ,[PassWord]
                    ,[UsefulLife]
                    ,[CarriersCode])
                VALUES
                    (@CarriersID
                    ,@CarriersName
                    ,@CarriersAllName
                    ,@LinkMan
                    ,@Phone
                    ,@Address
                    ,@Email
                    ,@Status
                    ,@CreateBy
                    ,GETDATE()
                    ,@UpdateBy
                    ,GETDATE()
                    ,0
                    ,@PassWord
                    ,@UsefulLife
                    ,@CarriersCode)";
            SqlParameter[] parameters = { 
                new SqlParameter("@CarriersID",SqlDbType.NVarChar,50){Value = model.CarrierID}
                ,new SqlParameter("@CarriersName",SqlDbType.NVarChar,100){Value = model.CarrierName}
                ,new SqlParameter("@CarriersAllName",SqlDbType.NVarChar,100){Value = model.CarrierAllName}
                ,new SqlParameter("@LinkMan",SqlDbType.NVarChar,20){Value = model.Contacter}
                ,new SqlParameter("@Phone",SqlDbType.NVarChar,20){Value = model.Phone}
                ,new SqlParameter("@Address",SqlDbType.NVarChar,100){Value = model.Address}
                ,new SqlParameter("@Email",SqlDbType.NVarChar,200){Value = model.Email}
                ,new SqlParameter("@CreateBy",SqlDbType.NVarChar,40){Value = model.CreateBy}
                ,new SqlParameter("@UpdateBy",SqlDbType.NVarChar,40){Value = model.UpdateBy}
                ,new SqlParameter("@UsefulLife",SqlDbType.Date){Value = model.ExpiredDate}
                ,new SqlParameter("@Status",SqlDbType.Int,100){Value = model.Status}};

            return ExecuteNonQuery(TMSWriteConnection, strSql, parameters);
        }

        public int Update(CarrierModel model)
        {
            string strSql = @"
                UPDATE TMS_Carriers
                SET [CarriersID]=@CarriersID
                    ,[CarriersName]=@CarriersName
                    ,[CarriersAllName]=@CarriersAllName
                    ,[LinkMan]=@LinkMan
                    ,[Phone]=@Phone
                    ,[Address]=@Address
                    ,[Email]=@Email
                    ,[Status]=@Status
                    ,[UpdateBy]=@UpdateBy
                    ,[UpdateTime]=GETDATE()
                    ,[PassWord]=@PassWord
                    ,[UsefulLife]=@UsefulLife
                    ,[CarriersCode]=@CarriersCode";
            SqlParameter[] parameters = { 
                new SqlParameter("@CarriersID",SqlDbType.NVarChar,50){Value = model.CarrierID}
                ,new SqlParameter("@CarriersName",SqlDbType.NVarChar,100){Value = model.CarrierName}
                ,new SqlParameter("@CarriersAllName",SqlDbType.NVarChar,100){Value = model.CarrierAllName}
                ,new SqlParameter("@LinkMan",SqlDbType.NVarChar,20){Value = model.Contacter}
                ,new SqlParameter("@Phone",SqlDbType.NVarChar,20){Value = model.Phone}
                ,new SqlParameter("@Address",SqlDbType.NVarChar,100){Value = model.Address}
                ,new SqlParameter("@Email",SqlDbType.NVarChar,200){Value = model.Email}
                ,new SqlParameter("@UpdateBy",SqlDbType.NVarChar,40){Value = model.UpdateBy}
                ,new SqlParameter("@UsefulLife",SqlDbType.Date){Value = model.ExpiredDate}};

            return ExecuteNonQuery(TMSWriteConnection, strSql, parameters);
        }

        public int Delete(CarrierModel model)
        {
            string strSql = @"
                UPDATE TMS_Carriers
                SET [DeleteFlag]=1";

            return ExecuteNonQuery(TMSWriteConnection, strSql);
        }

        public IList<CarrierModel> Search(CarrierSearchModel searchModel, PageInfo pageInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                SELECT * FROM TMS_Carriers(NOLOCK)
            ");
            return ExecuteReaderPagedByTopGrammar<CarrierModel>(TMSReadOnlyConnection, sb.ToString(), pageInfo);
        }

        public bool IsExistCarrier(string name)
        {
            string strSql = @"SELECT COUNT(1) FROM TMS_Carriers(NOLOCK) WHERE CarriersName=@CarriersName";
            SqlParameter[] parameters = { 
                                            new SqlParameter("@CarriersName",SqlDbType.NVarChar,100){Value = name}
                                        };

            int count = (int)ExecuteScalar(TMSReadOnlyConnection, strSql, parameters);
            return count > 0;
        }

        public CarrierModel Get(string id)
        {
            string strSql = @"
                SELECT
                    ([CarriersID]
                    ,[CarriersName]
                    ,[CarriersAllName]
                    ,[LinkMan]
                    ,[Phone]
                    ,[Address]
                    ,[Email]
                    ,[Status]
                    ,[CreateBy]
                    ,[CreateTime]
                    ,[UpdateBy]
                    ,[UpdateTime]
                    ,[DeleteFlag]
                    ,[PassWord]
                    ,[UsefulLife]
                    ,[CarriersCode])
                FROM TMS_Carriers
                WHERE TMSCarriersID = @TMSCarriersID";

            SqlParameter[] parameters = { 
                new SqlParameter("@TMSCarriersID",SqlDbType.NVarChar,50){Value = id}};

            //    return SqlHelper.ExecuteNonQuery(TMSWriteConnection, System.Data.CommandType.Text, strSql, parameters);
            throw new NotImplementedException("");
        }

        #endregion

        #region ICarrierDAL 成员


        public PagedList<CarrierModel> Search(CarrierSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICarrierDAL 成员


        public IList<CarrierModel> GetAll()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
