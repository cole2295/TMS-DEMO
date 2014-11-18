using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Core.ACIDManager;
using System.Data.SqlClient;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class WayBillReturnDAL: BaseDAL, IWayBillReturnDAL
    {
        public WaybillReturnEntityModel GetModel(long WaybillNo)
        {
            String sql = @"
SELECT TOP 1 WaybillReturnInfoID,WaybillNo,Weight,LabelNo,BoxNo,BoxLabelNo,
      CreateBy,CreateTime,UpdateBy,UpdateTime,BoxStatus 
FROM WaybillReturnInfo(NOLOCK)
WHERE WaybillNo=@WaybillNo
";
            SqlParameter[] arguments = new SqlParameter[]
            {
				new SqlParameter() { ParameterName="@WaybillNo",  SqlDbType = System.Data.SqlDbType.BigInt , Value = WaybillNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<WaybillReturnEntityModel>(LMSWriteConnection, sql, arguments);
        }

        public bool Update(WaybillReturnEntityModel model)
        {
            String sql = @"
UPDATE WaybillReturnInfo
SET WaybillNo=@WaybillNo,
    Weight=@Weight,
    LabelNo=@LabelNo,
    BoxNo=@BoxNo,
    CreateBy=@CreateBy,
    CreateTime=@CreateTime,
UpdateBy=@UpdateBy,
UpdateTime=@UpdateTime,
BoxStatus=@BoxStatus,
BoxLabelNo=@BoxLabelNo
WHERE WaybillReturnInfoID=@WaybillReturnInfoID
";
            SqlParameter[] arguments = new SqlParameter[]
            {
				    new SqlParameter() { ParameterName="@WaybillNo",  SqlDbType = System.Data.SqlDbType.BigInt , Value = model.WaybillNo },
				    new SqlParameter() { ParameterName="@Weight", SqlDbType = System.Data.SqlDbType.Decimal , Value = model.Weight },
				    new SqlParameter() { ParameterName="@LabelNo",  SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = model.LabelNo },
				    new SqlParameter() { ParameterName="@BoxNo",  SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = model.BoxNo },
				    new SqlParameter() { ParameterName="@CreateBy",  SqlDbType = System.Data.SqlDbType.Int , Value = model.CreateBy },
				    new SqlParameter() { ParameterName="@CreateTime",  SqlDbType = System.Data.SqlDbType.DateTime , Value = model.CreateTime },
				    new SqlParameter() { ParameterName="@UpdateBy",  SqlDbType = System.Data.SqlDbType.Int , Value = model.UpdateBy },
				    new SqlParameter() { ParameterName="@UpdateTime", SqlDbType = System.Data.SqlDbType.DateTime , Value = model.UpdateTime},
				    new SqlParameter() { ParameterName="@BoxStatus",  SqlDbType = System.Data.SqlDbType.SmallInt , Value = model.BoxStatus },
				    new SqlParameter() { ParameterName="@BoxLabelNo", SqlDbType = System.Data.SqlDbType.VarChar , Size = 20 , Value = model.BoxLabelNo },
				    new SqlParameter() { ParameterName="@WaybillReturnInfoID",  SqlDbType = System.Data.SqlDbType.BigInt , Value = model.WaybillReturnInfoID }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments) > 0;
        }
    }
}
