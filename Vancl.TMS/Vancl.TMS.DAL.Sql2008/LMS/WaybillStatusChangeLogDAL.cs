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
    public class WaybillStatusChangeLogDAL : BaseDAL, IWaybillStatusChangeLogDAL
    {
        #region IWaybillStatusChangeLogDAL 成员
        /// <summary>
        /// 添加状态变更日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(WaybillStatusChangeLogEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("WaybillStatusChangeLogEntityModel is null");
            String sql = @"
INSERT INTO LMS_WaybillStatusChangeLog( WaybillNO,CurNode,Status,SubStatus,MerchantID,DistributionCode,DeliverStationID,CreateTime,CreateBy ,CreateStation,IsSyn,Description
           ,CustomerOrder ,LMS_WaybillStatusChangeLogKid ,IsM2sSyn, OperateType ,TmsSyncStatus )
VALUES
           (@WaybillNO ,@CurNode ,@Status ,@SubStatus ,@MerchantID ,@DistributionCode ,@DeliverStationID ,GETDATE() ,@CreateBy ,@CreateDept ,@IsSyn ,@Note
           ,@CustomerOrder ,@LMS_WaybillStatusChangeLogKid ,@IsM2sSyn, @OperateType ,@TmsSyncStatus )
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@WaybillNO", SqlDbType = System.Data.SqlDbType.BigInt, Value = model.WaybillNO },
                new SqlParameter() { ParameterName="@CurNode",  SqlDbType = SqlDbType.Int , Value = (int)model.CurNode },
                new SqlParameter() { ParameterName="@Status",  SqlDbType = SqlDbType.NVarChar , Size = 20 , Value = ((int)model.Status).ToString() },
                new SqlParameter() { ParameterName="@SubStatus",  SqlDbType = SqlDbType.Int , Value = model.SubStatus },
                new SqlParameter() { ParameterName="@MerchantID",  SqlDbType = SqlDbType.Int , Value = model.MerchantID },
                new SqlParameter() { ParameterName="@DistributionCode", SqlDbType = SqlDbType.NVarChar , Size = 50 , Value = model.DistributionCode },
                new SqlParameter() { ParameterName="@DeliverStationID",  SqlDbType = SqlDbType.Int , Value = model.DeliverStationID },
                new SqlParameter() { ParameterName="@CreateTime",  SqlDbType = SqlDbType.DateTime , Value = model.CreateTime },
                new SqlParameter() { ParameterName="@CreateBy", SqlDbType = SqlDbType.Int , Value = model.CreateBy },
                new SqlParameter() { ParameterName="@CreateDept", SqlDbType = SqlDbType.Int , Value = model.CreateDept },
                new SqlParameter() { ParameterName="@IsSyn",  SqlDbType = SqlDbType.Int , Value = Convert.ToInt32(model.IsSyn) },
                new SqlParameter() { ParameterName="@Note", SqlDbType = SqlDbType.NVarChar , Size = 400 , Value = model.Note },
                new SqlParameter() { ParameterName="@CustomerOrder", SqlDbType = SqlDbType.NVarChar , Size = 50 , Value = model.CustomerOrder },
                new SqlParameter() { ParameterName="@LMS_WaybillStatusChangeLogKid", SqlDbType = SqlDbType.VarChar , Size = 20 , Value = model.LMS_WaybillStatusChangeLogKid },
                new SqlParameter() { ParameterName="@IsM2sSyn", SqlDbType = SqlDbType.VarChar , Size = 1 , Value = model.IsM2sSyn },
                new SqlParameter() { ParameterName="@OperateType",  SqlDbType = SqlDbType.Int , Value = (int)model.OperateType },
                new SqlParameter() { ParameterName="@TmsSyncStatus", SqlDbType = SqlDbType.Int , Value = (int)model.TmsSyncStatus }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }

        #endregion
    }
}
