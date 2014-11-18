using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class WaybillDAL : LMSBaseDAL, IWaybillDAL
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
SET Status = :Status
    ,DeliverStationID = :DeliverStationID
    ,UpdateBy = :UpdateBy
    ,UpdateStation = :UpdateDept
    ,UpdateTime = :UpdateTime
    ,OutboundID = :OutboundID
    ,InBoundID = :InBoundID
    ,CreatStation = CASE WHEN InBoundID IS NULL THEN  :UpdateDept ELSE CreatStation END    
WHERE WaybillNo = :WaybillNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="WaybillNo", OracleDbType = OracleDbType.Int64, Value = waybillModel.WaybillNo },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Varchar2, Size = 20, Value = ((int)waybillModel.Status).ToString() },
                new OracleParameter() { ParameterName="DeliverStationID", OracleDbType = OracleDbType.Int32, Value = waybillModel.DeliverStationID },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32, Value = waybillModel.UpdateBy },
                new OracleParameter() { ParameterName="UpdateDept", OracleDbType = OracleDbType.Int32, Value = waybillModel.UpdateDept },                
                new OracleParameter() { ParameterName="UpdateTime", OracleDbType = OracleDbType.Date, Value = waybillModel.UpdateTime },                  
                new OracleParameter() { ParameterName="InBoundID", OracleDbType = OracleDbType.Int64, Value = waybillModel.InboundID },
                new OracleParameter() { ParameterName="OutboundID", OracleDbType = OracleDbType.Int64, Value = waybillModel.OutboundID }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
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
SET Status = :Status
    ,UpdateBy = :UpdateBy
    ,UpdateStation = :UpdateDept
    ,UpdateTime = :UpdateTime
    ,OutboundID = :OutboundID
    ,InBoundID = :InBoundID
    ,CreatStation = CASE WHEN InBoundID IS NULL THEN  :UpdateDept ELSE CreatStation END    
WHERE WaybillNo = :WaybillNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="WaybillNo", OracleDbType = OracleDbType.Int64, Value = waybillModel.WaybillNo },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Varchar2, Size = 20, Value = ((int)waybillModel.Status).ToString() },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32, Value = waybillModel.UpdateBy },
                new OracleParameter() { ParameterName="UpdateDept", OracleDbType = OracleDbType.Int32, Value = waybillModel.UpdateDept },                
                new OracleParameter() { ParameterName="UpdateTime", OracleDbType = OracleDbType.Date, Value = waybillModel.UpdateTime },                  
                new OracleParameter() { ParameterName="InBoundID", OracleDbType = OracleDbType.Int64, Value = waybillModel.InboundID },
                new OracleParameter() { ParameterName="OutboundID", OracleDbType = OracleDbType.Int64, Value = waybillModel.OutboundID }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }

        public int UpdateWaybillModelByLoading(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            String sql = @"
                            UPDATE Waybill
                            SET Status = :Status
                                ,UpdateBy = :UpdateBy
                                ,UpdateTime = :UpdateTime 
                                ,CurrentDistributionCode = :CurrentDistributionCode 
                            WHERE WaybillNo = :WaybillNo
                            ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="WaybillNo", OracleDbType = OracleDbType.Int64, Value = waybillModel.WaybillNo },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Varchar2, Size = 20, Value = ((int)waybillModel.Status).ToString() },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32, Value = waybillModel.UpdateBy },               
                new OracleParameter() { ParameterName="UpdateTime", OracleDbType = OracleDbType.Date, Value = waybillModel.UpdateTime },
                new OracleParameter() { ParameterName="CurrentDistributionCode", OracleDbType = OracleDbType.Varchar2, Size = 100, Value = waybillModel.CurrentDistributionCode }  
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
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
FROM Waybill
WHERE WaybillNo = :WaybillNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="WaybillNo", OracleDbType = OracleDbType.Int64 , Value = WaybillNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<WaybillEntityModel>(LMSOracleWriteConnection, sql, arguments);
        }


        public int UpdateWaybillModelByOutbound(WaybillEntityModel waybillModel)
        {
            String sql = @"
UPDATE  Waybill
SET Status = :Status ,
        UpdateBy = :UpdateBy ,
        UpdateStation = :UpdateStation ,
        UpdateTime = SYSDATE ,
        OutBoundID = :OutBoundID ,
        DeliverTime = :DeliverTime ,
        CurrentDistributionCode = :CurrentDistributionCode
WHERE   WaybillNO = :WaybillNo
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = ((int)waybillModel.Status).ToString() },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32 , Value = waybillModel.UpdateBy },
                new OracleParameter() { ParameterName="UpdateStation",  OracleDbType = OracleDbType.Int32 , Value = waybillModel.UpdateDept },
                new OracleParameter() { ParameterName="OutBoundID", OracleDbType = OracleDbType.Int64 , Value = waybillModel.OutboundID },
                new OracleParameter() { ParameterName="DeliverTime",  OracleDbType = OracleDbType.Date , Value = waybillModel.DeliveryTime },
                new OracleParameter() { ParameterName="CurrentDistributionCode", OracleDbType = OracleDbType.Varchar2 , Size = 100 , Value = waybillModel.CurrentDistributionCode },
                new OracleParameter() { ParameterName="WaybillNo",  OracleDbType = OracleDbType.Int64 , Value = waybillModel.WaybillNo },
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }

        #endregion


        public int UpdateWaybillStatus(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            String sql = @"
UPDATE Waybill
SET Status = :Status
    ,UpdateBy = :UpdateBy
    ,UpdateStation = :UpdateDept
    ,UpdateTime = :UpdateTime
WHERE WaybillNo = :WaybillNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = ((int)waybillModel.Status).ToString() },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32 , Value = waybillModel.UpdateBy },
                new OracleParameter() { ParameterName="UpdateDept", OracleDbType = OracleDbType.Int32 , Value = waybillModel.UpdateDept },                
                new OracleParameter() { ParameterName="UpdateTime",  OracleDbType = OracleDbType.Date , Value = waybillModel.UpdateTime },
                new OracleParameter() { ParameterName="WaybillNo",  OracleDbType = OracleDbType.Int64 , Value = waybillModel.WaybillNo },
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }


        public int UpdateWaybillReturnStatus(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            String sql = @"
UPDATE Waybill
SET BACKSTATUS = :ReturnStatus
    ,Status = :Status
    ,UpdateBy = :UpdateBy
    ,UpdateStation = :UpdateDept
    ,UpdateTime = :UpdateTime
WHERE WaybillNo = :WaybillNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="ReturnStatus",  OracleDbType = OracleDbType.Int32 , Value = (int)waybillModel.ReturnStatus },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = (int)waybillModel.Status },
                new OracleParameter() { ParameterName="UpdateBy",  OracleDbType = OracleDbType.Int32 , Value = waybillModel.UpdateBy },
                new OracleParameter() { ParameterName="UpdateDept", OracleDbType = OracleDbType.Int32 , Value = waybillModel.UpdateDept },                
                new OracleParameter() { ParameterName="UpdateTime",  OracleDbType = OracleDbType.Date , Value = waybillModel.UpdateTime },
                new OracleParameter() { ParameterName="WaybillNo",  OracleDbType = OracleDbType.Int64 , Value = waybillModel.WaybillNo },
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }


        /// <summary>
        /// 返货单的入库
        /// </summary>
        /// <param name="waybillModel"></param>
        /// <returns></returns>
        public int WaybillReturnInBound(WaybillEntityModel waybillModel)
        {
            if (waybillModel == null) throw new ArgumentNullException("WaybillEntityModel is null.");
            string time = waybillModel.ReturnStatus == Enums.ReturnStatus.ReturnInbounded ? "ReturnExpressCompanyTime=sysdate" : "ReturnTime=sysdate,ReturnExpressCompanyTime=sysdate";
            String sql = String.Format(@"
UPDATE Waybill
SET BACKSTATUS = :ReturnStatus
    ,Status = :Status
    ,UpdateBy = :UpdateBy
    ,UpdateStation = :UpdateDept
    ,UpdateTime = sysdate
    ,{0}
    ,ReturnExpressCompanyId=:ReturnExpressCompanyId   
WHERE WaybillNo = :WaybillNo
", time);
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="ReturnStatus",  OracleDbType = OracleDbType.Int32 , Value =(int)waybillModel.ReturnStatus },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = (int)waybillModel.Status },
                new OracleParameter() { ParameterName="UpdateBy",  OracleDbType = OracleDbType.Int32 , Value = waybillModel.UpdateBy },
                new OracleParameter() { ParameterName="UpdateDept",  OracleDbType = OracleDbType.Int32 , Value = waybillModel.UpdateDept },                
                new OracleParameter() { ParameterName="ReturnExpressCompanyId",  OracleDbType = OracleDbType.Int32 , Value = waybillModel.ReturnExpressCompanyId },
                new OracleParameter() { ParameterName="WaybillNo",  OracleDbType = OracleDbType.Int64 , Value = waybillModel.WaybillNo },
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }
    }
}