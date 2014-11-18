using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class WayBillReturnDAL : LMSBaseDAL, IWayBillReturnDAL
    {
        public WaybillReturnEntityModel GetModel(long WaybillNo)
        {
            String sql = @"
SELECT WaybillReturnInfoID,WaybillNo,Weight,LabelNo,BoxNo,BoxLabelNo,
      CreateBy,CreateTime,UpdateBy,UpdateTime,BoxStatus
FROM WaybillReturnInfo
WHERE WaybillNo = :WaybillNo
     AND rownum = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="WaybillNo", OracleDbType = OracleDbType.Int64, Value = WaybillNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<WaybillReturnEntityModel>(LMSOracleWriteConnection, sql, arguments);
        }
        public bool Update(WaybillReturnEntityModel model)
        {
            String sql = @"
UPDATE WaybillReturnInfo
SET WaybillNo=:WaybillNo,
    Weight=:Weight,
    LabelNo=:LabelNo,
    BoxNo=:BoxNo,
    CreateBy=:CreateBy,
    CreateTime=:CreateTime,
UpdateBy=:UpdateBy,
UpdateTime=:UpdateTime,
BoxStatus=:BoxStatus,
BoxLabelNo=:BoxLabelNo
WHERE WaybillReturnInfoID=:WaybillReturnInfoID
";
            OracleParameter[] arguments = {
					                new OracleParameter() { ParameterName="WaybillNo", OracleDbType = OracleDbType.Int64, Value = model.WaybillNo },
					                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal, Value = model.Weight },
					                new OracleParameter() { ParameterName="LabelNo", OracleDbType = OracleDbType.Varchar2, Size = 20, Value = model.LabelNo },
					                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2, Size = 20, Value = model.BoxNo },
					                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32, Value = model.CreateBy },
					                new OracleParameter() { ParameterName="CreateTime", OracleDbType = OracleDbType.Date, Value = model.CreateTime },
					                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32, Value = model.UpdateBy },
					                new OracleParameter() { ParameterName="UpdateTime", OracleDbType = OracleDbType.Date, Value = model.UpdateTime},
					                new OracleParameter() { ParameterName="BoxStatus", OracleDbType = OracleDbType.Int32, Value = model.BoxStatus },
					                new OracleParameter() { ParameterName="BoxLabelNo", OracleDbType = OracleDbType.Varchar2, Size = 20, Value = model.BoxLabelNo },
					                new OracleParameter() { ParameterName="WaybillReturnInfoID", OracleDbType = OracleDbType.Int64, Value = model.WaybillReturnInfoID }
                                        };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments)>0;
        }

    }
}
