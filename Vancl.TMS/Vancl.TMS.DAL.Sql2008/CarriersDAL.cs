using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.BaseInfo;
using Microsoft.ApplicationBlocks.Data;
using Vancl.TMS.Model.Common;
using System.Data.SqlClient;
using System.Data;

namespace Vancl.TMS.DAL.SQL2008.BaseInfo
{
    public class CarriersDAL : BaseDAL, ICarriersDAL
    {
        #region ICarriersDAL 成员

        public int Add(CarriersModel model)
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
                new SqlParameter("@CarriersID",SqlDbType.NVarChar,50){Value = model.CarriersID}
                ,new SqlParameter("@CarriersName",SqlDbType.NVarChar,100){Value = model.CarriersName}
                ,new SqlParameter("@CarriersAllName",SqlDbType.NVarChar,100){Value = model.CarriersAllName}
                ,new SqlParameter("@LinkMan",SqlDbType.NVarChar,20){Value = model.LinkMan}
                ,new SqlParameter("@Phone",SqlDbType.NVarChar,20){Value = model.Phone}
                ,new SqlParameter("@Address",SqlDbType.NVarChar,100){Value = model.Address}
                ,new SqlParameter("@Email",SqlDbType.NVarChar,200){Value = model.Email}
                ,new SqlParameter("@CreateBy",SqlDbType.NVarChar,40){Value = model.CreateBy}
                ,new SqlParameter("@UpdateBy",SqlDbType.NVarChar,40){Value = model.UpdateBy}
                ,new SqlParameter("@PassWord",SqlDbType.NVarChar,100){Value = model.Password}
                ,new SqlParameter("@UsefulLife",SqlDbType.Date){Value = model.UsefulLife}
                ,new SqlParameter("@CarriersCode",SqlDbType.NVarChar,100){Value = model.CarriersCode}};

            return SqlHelper.ExecuteNonQuery(TMSWriteConnection, System.Data.CommandType.Text, strSql, parameters);
        }

        public int Update(CarriersModel model)
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
                new SqlParameter("@CarriersID",SqlDbType.NVarChar,50){Value = model.CarriersID}
                ,new SqlParameter("@CarriersName",SqlDbType.NVarChar,100){Value = model.CarriersName}
                ,new SqlParameter("@CarriersAllName",SqlDbType.NVarChar,100){Value = model.CarriersAllName}
                ,new SqlParameter("@LinkMan",SqlDbType.NVarChar,20){Value = model.LinkMan}
                ,new SqlParameter("@Phone",SqlDbType.NVarChar,20){Value = model.Phone}
                ,new SqlParameter("@Address",SqlDbType.NVarChar,100){Value = model.Address}
                ,new SqlParameter("@Email",SqlDbType.NVarChar,200){Value = model.Email}
                ,new SqlParameter("@UpdateBy",SqlDbType.NVarChar,40){Value = model.UpdateBy}
                ,new SqlParameter("@PassWord",SqlDbType.NVarChar,100){Value = model.Password}
                ,new SqlParameter("@UsefulLife",SqlDbType.Date){Value = model.UsefulLife}
                ,new SqlParameter("@CarriersCode",SqlDbType.NVarChar,100){Value = model.CarriersCode}};

            return SqlHelper.ExecuteNonQuery(TMSWriteConnection, System.Data.CommandType.Text, strSql, parameters);
        }

        public int Delete(int id)
        {
            string strSql = @"
                UPDATE TMS_Carriers
                SET [DeleteFlag]=1";

            return SqlHelper.ExecuteNonQuery(TMSWriteConnection, System.Data.CommandType.Text, strSql);
        }

        public IList<CarriersModel> Search(CarriersSearchModel searchModel, PageInfo pageInfo)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
                SELECT * FROM TMS_Carriers(NOLOCK)
            ");
            return new List<CarriersModel>().SetValueFromDB<CarriersModel>(
                SqlHelper.ExecuteReaderPagedByTopGrammar(TMSReadOnlyConnection, sb.ToString(), pageInfo));
        }

        #endregion
    }
}
