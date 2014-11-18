using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using System.Data;
using System.Data.SqlClient;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class InboundDAL : BaseDAL, IInboundDAL
    {
        #region IInboundDAL 成员
        /// <summary>
        /// 新增入库数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Add(InboundEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundEntityModel is null");
            String sql = @"
INSERT INTO InBound (WaybillNO ,CustomerBatchNO ,FromStation, Operator ,IntoTime , IntoStation, IntoStationType, DeliveryMan, DeliveryTime, IsPrint 
        ,CreatBy, CreatStation ,CreatTime ,UpdateBy  ,UpdateStation ,UpdateTime ,IsDelete ,ToStation  ,InBoundKid)
VALUES  (@WaybillNO, @CustomerBatchNO, @FromStation, @Operator, @IntoTime, @IntoStation , @IntoStationType , @DeliveryMan , @DeliveryTime, 0
        ,@CreateBy,@CreateDept ,@CreateTime ,@UpdateBy ,@UpdateDept ,@UpdateTime ,0 ,@ToStation ,@InBoundKid );
SELECT scope_identity()
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName= "@WaybillNO" , SqlDbType = SqlDbType.BigInt , Value = model.WaybillNO },
                new SqlParameter() { ParameterName = "@CustomerBatchNO",  SqlDbType = SqlDbType.NVarChar , Size = 20, Value = model.CustomerBatchNO },                
                new SqlParameter() { ParameterName = "@FromStation",  SqlDbType = SqlDbType.Int , Value = model.FromStation },
                new SqlParameter() { ParameterName = "@Operator",  SqlDbType = SqlDbType.Int , Value = model.CurOperator },
                new SqlParameter() { ParameterName = "@IntoTime" , SqlDbType = SqlDbType.DateTime , Value = model.IntoTime },
                new SqlParameter() { ParameterName = "@IntoStation" ,  SqlDbType = SqlDbType.Int , Value = model.IntoStation },
                new SqlParameter() { ParameterName = "@IntoStationType" , SqlDbType = SqlDbType.NVarChar , Size = 20 , Value = ((int)model.IntoStationType).ToString() },
                new SqlParameter() { ParameterName = "@DeliveryMan" ,  SqlDbType = SqlDbType.Int , Value = model.DeliveryMan },
                new SqlParameter() { ParameterName = "@DeliveryTime" ,  SqlDbType = SqlDbType.DateTime , Value = model.DeliveryTime },                
                new SqlParameter() { ParameterName = "@CreateBy" ,  SqlDbType = SqlDbType.Int , Value = model.CreateBy },
                new SqlParameter() { ParameterName = "@CreateDept" ,  SqlDbType = SqlDbType.Int , Value = model.CreateDept },
                new SqlParameter() { ParameterName = "@CreateTime" , SqlDbType = SqlDbType.DateTime , Value = model.CreateTime },
                new SqlParameter() { ParameterName = "@UpdateBy" , SqlDbType = SqlDbType.Int , Value = model.UpdateBy },
                new SqlParameter() { ParameterName = "@UpdateDept" , SqlDbType = SqlDbType.Int  , Value = model.UpdateDept },
                new SqlParameter() { ParameterName = "@UpdateTime" , SqlDbType = SqlDbType.DateTime  , Value = model.UpdateTime },                
                new SqlParameter() { ParameterName = "@ToStation" , SqlDbType = SqlDbType.Int , Value = model.ToStation },
                new SqlParameter() { ParameterName = "@InBoundKid" , SqlDbType = SqlDbType.VarChar , Size = 20 , Value = model.InBoundKid }
            };
            object objValue = ExecuteScalar(LMSWriteConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return Convert.ToInt64(objValue);
            }
            return 0;
        }

        #endregion
    }
}
