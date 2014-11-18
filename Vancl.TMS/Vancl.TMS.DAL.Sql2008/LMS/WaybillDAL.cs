using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using System.Data.SqlClient;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class WaybillDAL : BaseDAL, IWaybillDAL
    {
        #region IWaybillDAL 成员

        /// <summary>
        /// 入库时修改主单对象【不限制站点】
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>影响行数</returns>
        public int UpdateWaybillModelByInbound_NoLimitedStation(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            String sql = @"
UPDATE Waybill
SET Status = @Status
    ,DeliverStationID = @DeliverStationID
    ,UpdateBy = @UpdateBy
    ,UpdateStation = @UpdateDept
    ,UpdateTime = @UpdateTime
    ,OutboundID = @OutboundID
    ,InBoundID = @InBoundID
    ,CreatStation = CASE WHEN InBoundID IS NULL THEN  @UpdateDept ELSE CreatStation END    
WHERE WaybillNo = @WaybillNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@WaybillNo", DbType = System.Data.DbType.Int64, Value = waybillModel.WaybillNo },
                new SqlParameter() { ParameterName="@Status", DbType = System.Data.DbType.String, Value = ((int)waybillModel.Status).ToString() },
                new SqlParameter() { ParameterName="@DeliverStationID", DbType = System.Data.DbType.Int32, Value = waybillModel.DeliverStationID },
                new SqlParameter() { ParameterName="@UpdateBy", DbType = System.Data.DbType.Int32, Value = waybillModel.UpdateBy },
                new SqlParameter() { ParameterName="@UpdateDept", DbType = System.Data.DbType.Int32, Value = waybillModel.UpdateDept },                
                new SqlParameter() { ParameterName="@UpdateTime", DbType = System.Data.DbType.DateTime, Value = waybillModel.UpdateTime },                  
                new SqlParameter() { ParameterName="@InBoundID", DbType = System.Data.DbType.Int64, Value = waybillModel.InboundID },
                new SqlParameter() { ParameterName="@OutboundID", DbType = System.Data.DbType.Int64, Value = waybillModel.OutboundID }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }



        /// <summary>
        /// 入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>是否成功</returns>
        public int UpdateWaybillModelByInbound(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            String sql = @"
UPDATE Waybill
SET Status = @Status
    ,UpdateBy = @UpdateBy
    ,UpdateStation = @UpdateDept
    ,UpdateTime = @UpdateTime
    ,OutboundID = @OutboundID
    ,InBoundID = @InBoundID
    ,CreatStation = CASE WHEN InBoundID IS NULL THEN  @UpdateDept ELSE CreatStation END    
WHERE WaybillNo = @WaybillNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@WaybillNo", DbType = System.Data.DbType.Int64, Value = waybillModel.WaybillNo },
                new SqlParameter() { ParameterName="@Status", DbType = System.Data.DbType.String, Value = ((int)waybillModel.Status).ToString() },
                new SqlParameter() { ParameterName="@UpdateBy", DbType = System.Data.DbType.Int32, Value = waybillModel.UpdateBy },
                new SqlParameter() { ParameterName="@UpdateDept", DbType = System.Data.DbType.Int32, Value = waybillModel.UpdateDept },                
                new SqlParameter() { ParameterName="@UpdateTime", DbType = System.Data.DbType.DateTime, Value = waybillModel.UpdateTime },                  
                new SqlParameter() { ParameterName="@InBoundID", DbType = System.Data.DbType.Int64, Value = waybillModel.InboundID },
                new SqlParameter() { ParameterName="@OutboundID", DbType = System.Data.DbType.Int64, Value = waybillModel.OutboundID }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IWaybillDAL 成员

        public WaybillEntityModel GetModel(long WaybillNo)
        {
            String sql = @"
SELECT
    WaybillNo,
    Status,
    Sources AS Source,
    CustomerBatchNO,
    DeliverManID AS DeliveryMan,
    DeliverManID AS DeliveryDriver,
    DeliverTime AS DeliveryTime,
    CustomerOrder,
    DistributionCode,
    MerchantID ,
    DeliverStationID ,
    CurrentDistributionCode
FROM Waybill WITH (NOLOCK) 
WHERE WaybillNo = @WaybillNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@WaybillNo",  SqlDbType = System.Data.SqlDbType.BigInt , Value = WaybillNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<WaybillEntityModel>(LMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 订单装车 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateWaybillModelByLoading(WaybillEntityModel model)
        {
            var strSql = @"
Update Waybill 
SET   Status = @Status,
    UpdateTime = @UpdateTime,
    CurrentDistributionCode = @CurrentDistributionCode 
WHERE   waybillNo = @waybillNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@Status",  SqlDbType = System.Data.SqlDbType.VarChar , Size = 20, Value = ((int)model.Status).ToString() },    
                new SqlParameter() { ParameterName="@UpdateTime",  SqlDbType = System.Data.SqlDbType.DateTime , Value = model.UpdateTime },  
                new SqlParameter() { ParameterName="@CurrentDistributionCode",  SqlDbType = System.Data.SqlDbType.NVarChar , Size = 50, Value = model.CurrentDistributionCode },
                new SqlParameter() { ParameterName="@WaybillNo",  SqlDbType = System.Data.SqlDbType.BigInt, Value = model.WaybillNo },
            };
            return ExecuteNonQuery(LMSWriteConnection, strSql, arguments);
        }

        public int UpdateWaybillModelByOutbound(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            String sql = @"
UPDATE  Waybill
SET Status = @Status ,
        UpdateBy = @UpdateBy ,
        UpdateStation = @UpdateStation ,
        UpdateTime = GETDATE() ,
        OutBoundID = @OutBoundID,
        DeliverTime = @DeliverTime,
        CurrentDistributionCode = @CurrentDistributionCode
WHERE   WaybillNO = @WaybillNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            { 
                new SqlParameter() { ParameterName="@Status", SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = ((int)waybillModel.Status).ToString() },
                new SqlParameter() { ParameterName="@UpdateBy", SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.UpdateBy },
                new SqlParameter() { ParameterName="@UpdateStation",  SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.UpdateDept },
                new SqlParameter() { ParameterName="@OutBoundID",  SqlDbType = System.Data.SqlDbType.BigInt , Value = waybillModel.OutboundID },
                new SqlParameter() { ParameterName="@DeliverTime",  SqlDbType = System.Data.SqlDbType.DateTime , Value = waybillModel.DeliveryTime },
                new SqlParameter() { ParameterName="@CurrentDistributionCode",  SqlDbType = System.Data.SqlDbType.NVarChar , Size = 50 , Value = waybillModel.CurrentDistributionCode },
                new SqlParameter() { ParameterName="@WaybillNo",  SqlDbType = System.Data.SqlDbType.BigInt, Value = waybillModel.WaybillNo },
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }

        #endregion


        public int UpdateWaybillStatus(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            String sql = @"
UPDATE Waybill
SET Status = @Status
    ,UpdateBy = @UpdateBy
    ,UpdateStation = @UpdateDept
    ,UpdateTime = @UpdateTime   
WHERE WaybillNo = @WaybillNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@Status", SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = ((int)waybillModel.Status).ToString() },
                new SqlParameter() { ParameterName="@UpdateBy", SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.UpdateBy },
                new SqlParameter() { ParameterName="@UpdateDept", SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.UpdateDept },                
                new SqlParameter() { ParameterName="@UpdateTime",  SqlDbType = System.Data.SqlDbType.DateTime , Value = waybillModel.UpdateTime },     
                new SqlParameter() { ParameterName="@WaybillNo",  SqlDbType = System.Data.SqlDbType.BigInt , Value = waybillModel.WaybillNo },
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }

        public int UpdateWaybillReturnStatus(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            String sql = @"
UPDATE Waybill
SET BackStatus = @ReturnStatus
    ,Status = @Status
    ,UpdateBy = @UpdateBy
    ,UpdateStation = @UpdateDept
    ,UpdateTime = @UpdateTime   
WHERE WaybillNo = @WaybillNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@ReturnStatus",  SqlDbType = System.Data.SqlDbType.Int , Value = (int)waybillModel.ReturnStatus },
                new SqlParameter() { ParameterName="@Status", SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = ((int)waybillModel.Status).ToString() },
                new SqlParameter() { ParameterName="@UpdateBy",  SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.UpdateBy },
                new SqlParameter() { ParameterName="@UpdateDept",SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.UpdateDept },                
                new SqlParameter() { ParameterName="@UpdateTime",  SqlDbType = System.Data.SqlDbType.DateTime , Value = waybillModel.UpdateTime },     
                new SqlParameter() { ParameterName="@WaybillNo",  SqlDbType = System.Data.SqlDbType.BigInt , Value = waybillModel.WaybillNo },
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }


        /// <summary>
        /// 返货的入库
        /// </summary>
        /// <param name="waybillModel"></param>
        /// <returns></returns>
        public int WaybillReturnInBound(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            string time = waybillModel.ReturnStatus == Enums.ReturnStatus.ReturnInbounded ? "ReturnExpressCompanyTime=getdate()" : "ReturnTime=getdate(),ReturnExpressCompanyTime=getdate()";

            String sql = String.Format(@"
UPDATE Waybill
SET BackStatus = @ReturnStatus
    ,Status=@Status
    ,UpdateBy = @UpdateBy
    ,UpdateStation = @UpdateDept
    ,UpdateTime = getdate()
    ,{0}
    ,ReturnExpressCompanyId=@ReturnExpressCompanyId   
WHERE WaybillNo = @WaybillNo
", time);
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@ReturnStatus", SqlDbType = System.Data.SqlDbType.Int , Value = (int)waybillModel.ReturnStatus },
                new SqlParameter() { ParameterName="@Status", SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = ((int)waybillModel.Status).ToString() },
                new SqlParameter() { ParameterName="@UpdateBy", SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.UpdateBy },
                new SqlParameter() { ParameterName="@UpdateDept", SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.UpdateDept },                
                new SqlParameter() { ParameterName="@ReturnExpressCompanyId", SqlDbType = System.Data.SqlDbType.Int , Value = waybillModel.ReturnExpressCompanyId },     
                new SqlParameter() { ParameterName="@WaybillNo",  SqlDbType = System.Data.SqlDbType.BigInt , Value = waybillModel.WaybillNo },
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }
    }
}
