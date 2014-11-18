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
    public class OutboundDAL : LMSBaseDAL, IOutboundDAL
    {
        #region IOutboundDAL 成员

        public long Add(OutboundEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            model.OutboundID = GetNextSequence(model.SequenceName);
            String sql = @"
INSERT INTO OutBound(OutboundID, WaybillNO, BatchNO, ToStation ,OutboundOperator ,OutStationType ,OutBoundStation, DeliveryMan ,DeliveryDriver ,OutBoundTime ,CreatBy ,CreatStation ,CreatTime
           ,UpdateBy ,UpdateStation  ,UpdateTime ,IsDelete  ,OutBoundKid ,IsPrint )
VALUES
           (:OutboundID, :WaybillNO, :BatchNO  ,:ToStation ,:OutboundOperator ,:OutStationType  ,:OutBoundStation ,:DeliveryMan  ,:DeliveryDriver ,:OutBoundTime ,:CreatBy ,:CreatStation  ,:CreatTime
           ,:UpdateBy ,:UpdateStation ,:UpdateTime, 0 , :OutBoundKid, 0)
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter(){ ParameterName="OutboundID" , OracleDbType = OracleDbType.Int64 , Value = model.OutboundID },
                new OracleParameter(){ ParameterName="WaybillNO" , OracleDbType = OracleDbType.Int64 , Value = model.WaybillNo },
                new OracleParameter(){ ParameterName="BatchNO" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = model.BatchNo },
                new OracleParameter(){ ParameterName="ToStation" , OracleDbType = OracleDbType.Int32 , Value = model.ToStation },
                new OracleParameter(){ ParameterName="OutboundOperator" , OracleDbType = OracleDbType.Int32 , Value = model.OutboundOperator },
                new OracleParameter(){ ParameterName="OutStationType" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = ((int)model.OutStationType).ToString() },
                new OracleParameter(){ ParameterName="OutBoundStation" , OracleDbType = OracleDbType.Int32 , Value =model.OutboundStation },
                new OracleParameter(){ ParameterName="DeliveryMan" , OracleDbType = OracleDbType.Int32 , Value = model.DeliveryMan },
                new OracleParameter(){ ParameterName="DeliveryDriver" , OracleDbType = OracleDbType.Int32 , Value = model.DeliveryDriver },
                new OracleParameter(){ ParameterName="OutBoundTime" , OracleDbType = OracleDbType.Date , Value = model.OutboundTime },
                new OracleParameter(){ ParameterName="CreatBy" , OracleDbType = OracleDbType.Int32 , Value = model.CreateBy },
                new OracleParameter(){ ParameterName="CreatStation" , OracleDbType = OracleDbType.Int32 , Value = model.CreateDept },
                new OracleParameter(){ ParameterName="CreatTime" , OracleDbType = OracleDbType.Date , Value = model.CreateTime },
                new OracleParameter(){ ParameterName="UpdateBy" , OracleDbType = OracleDbType.Int32 , Value = model.UpdateBy },
                new OracleParameter(){ ParameterName="UpdateStation" , OracleDbType = OracleDbType.Int32 , Value = model.UpdateDept },
                new OracleParameter(){ ParameterName="UpdateTime" , OracleDbType = OracleDbType.Date , Value = model.UpdateTime },
                new OracleParameter(){ ParameterName="OutBoundKid" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = model.OutboundKid }
            };
            if (ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments) > 0)
            {
                return model.OutboundID;
            }
            return 0;
        }

        #endregion
    }
}
