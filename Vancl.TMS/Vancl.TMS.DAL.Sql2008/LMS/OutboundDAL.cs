using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using System.Data.SqlClient;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class OutboundDAL : BaseDAL, IOutboundDAL
    {
        #region IOutboundDAL 成员

        public long Add(Model.LMS.OutboundEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            String sql = @"
INSERT INTO OutBound(WaybillNO, BatchNO, ToStation ,Operator ,OutStationType ,OutBoundStation, DeliveryMan ,DeliveryDriver ,OutBoundTime ,CreatBy ,CreatStation ,CreatTime
           ,UpdateBy ,UpdateStation  ,UpdateTime ,IsDelete  ,OutBoundKid, IsPrint )
VALUES
           (@WaybillNO, @BatchNO  ,@ToStation ,@Operator ,@OutStationType  ,@OutBoundStation ,@DeliveryMan  ,@DeliveryDriver ,@OutBoundTime ,@CreatBy ,@CreatStation  ,@CreatTime
           ,@UpdateBy ,@UpdateStation ,@UpdateTime, 0 , @OutBoundKid, 0 );
SELECT scope_identity()
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter(){ ParameterName="@WaybillNO" , SqlDbType = System.Data.SqlDbType.BigInt , Value = model.WaybillNo },
                new SqlParameter(){ ParameterName="@BatchNO" , SqlDbType = System.Data.SqlDbType.NVarChar , Size = 20 , Value = model.BatchNo },
                new SqlParameter(){ ParameterName="@ToStation" , SqlDbType = System.Data.SqlDbType.Int , Value = model.ToStation },
                new SqlParameter(){ ParameterName="@Operator" , SqlDbType = System.Data.SqlDbType.Int , Value = model.OutboundOperator },
                new SqlParameter(){ ParameterName="@OutStationType" , SqlDbType = System.Data.SqlDbType.NVarChar , Size = 20, Value = ((int)model.OutStationType).ToString() },
                new SqlParameter(){ ParameterName="@OutBoundStation" , SqlDbType = System.Data.SqlDbType.Int , Value =model.OutboundStation },
                new SqlParameter(){ ParameterName="@DeliveryMan" , SqlDbType = System.Data.SqlDbType.Int , Value = model.DeliveryMan },
                new SqlParameter(){ ParameterName="@DeliveryDriver" , SqlDbType = System.Data.SqlDbType.Int , Value = model.DeliveryDriver },
                new SqlParameter(){ ParameterName="@OutBoundTime" , SqlDbType = System.Data.SqlDbType.DateTime , Value = model.OutboundTime },
                new SqlParameter(){ ParameterName="@CreatBy" , SqlDbType = System.Data.SqlDbType.Int , Value = model.CreateBy },
                new SqlParameter(){ ParameterName="@CreatStation" , SqlDbType = System.Data.SqlDbType.Int , Value = model.CreateDept },
                new SqlParameter(){ ParameterName="@CreatTime" , SqlDbType = System.Data.SqlDbType.DateTime , Value = model.CreateTime },
                new SqlParameter(){ ParameterName="@UpdateBy" , SqlDbType = System.Data.SqlDbType.Int , Value = model.UpdateBy },
                new SqlParameter(){ ParameterName="@UpdateStation" , SqlDbType = System.Data.SqlDbType.Int , Value = model.UpdateDept },
                new SqlParameter(){ ParameterName="@UpdateTime" , SqlDbType = System.Data.SqlDbType.DateTime , Value = model.UpdateTime },
                new SqlParameter(){ ParameterName="@OutBoundKid" , SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = model.OutboundKid }
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
