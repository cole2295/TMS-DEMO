using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class InboundDAL : LMSBaseDAL, IInboundDAL
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
            model.InboundID = GetNextSequence(model.SequenceName);
            String sql = @"
INSERT INTO InBound (InboundID, WaybillNO ,CustomerBatchNO ,FromStation, CurOperator ,IntoTime , IntoStation, IntoStationType, DeliveryMan, DeliveryTime, IsPrint 
        ,CreatBy, CreatStation ,CreatTime ,UpdateBy  ,UpdateStation ,UpdateTime ,IsDelete ,ToStation  ,InBoundKid)
VALUES  (:InboundID , :WaybillNO, :CustomerBatchNO, :FromStation, :CurOperator, :IntoTime, :IntoStation , :IntoStationType , :DeliveryMan , :DeliveryTime, 0
        ,:CreateBy, :CreateDept , :CreateTime , :UpdateBy ,:UpdateDept ,:UpdateTime ,0 ,:ToStation ,:InBoundKid )
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "InboundID", OracleDbType = OracleDbType.Int64 , Value = model.InboundID },
                new OracleParameter() { ParameterName= "WaybillNO" , OracleDbType = OracleDbType.Int64 , Value = model.WaybillNO },
                new OracleParameter() { ParameterName = "CustomerBatchNO", OracleDbType = OracleDbType.Varchar2 , Size = 20, Value = model.CustomerBatchNO },                
                new OracleParameter() { ParameterName = "FromStation",  OracleDbType = OracleDbType.Int32 , Value = model.FromStation },
                new OracleParameter() { ParameterName = "CurOperator",  OracleDbType = OracleDbType.Int32 , Value = model.CurOperator },
                new OracleParameter() { ParameterName = "IntoTime" ,  OracleDbType = OracleDbType.Date , Value = model.IntoTime },
                new OracleParameter() { ParameterName = "IntoStation" ,  OracleDbType = OracleDbType.Int32 , Value = model.IntoStation },
                new OracleParameter() { ParameterName = "IntoStationType" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = ((int)model.IntoStationType).ToString() },
                new OracleParameter() { ParameterName = "DeliveryMan" ,  OracleDbType = OracleDbType.Int32 , Value = model.DeliveryMan },
                new OracleParameter() { ParameterName = "DeliveryTime" ,  OracleDbType = OracleDbType.Date , Value = model.DeliveryTime },                
                new OracleParameter() { ParameterName = "CreateBy" , OracleDbType = OracleDbType.Int32  , Value = model.CreateBy },
                new OracleParameter() { ParameterName = "CreateDept" , OracleDbType = OracleDbType.Int32  , Value = model.CreateDept },
                new OracleParameter() { ParameterName = "CreateTime" ,  OracleDbType = OracleDbType.Date , Value = model.CreateTime },
                new OracleParameter() { ParameterName = "UpdateBy" , OracleDbType = OracleDbType.Int32  , Value = model.UpdateBy },
                new OracleParameter() { ParameterName = "UpdateDept" , OracleDbType = OracleDbType.Int32 , Value = model.UpdateDept },
                new OracleParameter() { ParameterName = "UpdateTime" ,  OracleDbType = OracleDbType.Date , Value = model.UpdateTime },                
                new OracleParameter() { ParameterName = "ToStation" ,  OracleDbType = OracleDbType.Int32 , Value = model.ToStation },
                new OracleParameter() { ParameterName = "InBoundKid" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = model.InBoundKid }
            };
            if (ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments) > 0)
            {
                return model.InboundID;
            }
            return 0;
        }

        #endregion
    }


}
