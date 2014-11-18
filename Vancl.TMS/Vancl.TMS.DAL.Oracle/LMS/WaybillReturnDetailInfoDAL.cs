using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.IDAL.LMS;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    /// <summary>
    /// 退货分拣称重数据实现层
    /// </summary>
    public class WaybillReturnDetailInfoDAL : LMSBaseDAL, IWaybillReturnDetailInfoDAL
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Add(WaybillReturnDetailInfoEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            String sql = string.Format(@"
MERGE INTO WaybillReturnDetailInfo  des
USING 
(  
   SELECT :WaybillNo as WaybillNo,:BoxNo as BoxNo,:CreateBy as CreateBy,:CreateTime as CreateTime,
   :CreateDep as CreateDep,:ReturnTo as ReturnTo,:OrderNo as OrderNo,:Weight as Weight
   FROM dual
)  src
ON ( des.WaybillNo = src.WaybillNo and des.ReturnTo = src.ReturnTo )
WHEN MATCHED THEN 
UPDATE SET des.BoxNo=src.BoxNo,des.CreateDep=src.CreateDep,des.Weight=src.Weight
WHEN NOT MATCHED THEN
INSERT(waybillreturndetailinfoid,WaybillNo,BoxNo,CreateBy,CreateTime,CreateDep,IsDelete,ReturnTo,OrderNo,Weight)
VALUES({0},src.WaybillNo,src.BoxNo,src.CreateBy,src.CreateTime,src.CreateDep,0,src.ReturnTo,src.OrderNo,src.Weight)",
GetNextSequence(model.SequenceName));
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter(){ ParameterName="WaybillNo" ,OracleDbType = OracleDbType.Int64, Value = model.WaybillNo },
                new OracleParameter(){ ParameterName="BoxNo" ,OracleDbType = OracleDbType.Varchar2, Size = 60, Value = model.BoxNo },
                new OracleParameter(){ ParameterName="CreateBy" ,OracleDbType = OracleDbType.Int32, Value = model.CreateBy },
                new OracleParameter(){ ParameterName="CreateTime" ,OracleDbType = OracleDbType.Date, Value = model.CreateTime },
                new OracleParameter(){ ParameterName="CreateDep" ,OracleDbType = OracleDbType.Varchar2, Size = 100, Value = model.CreateDep },
                new OracleParameter(){ ParameterName="ReturnTo" ,OracleDbType = OracleDbType.Varchar2, Size = 100, Value = model.ReturnTo },
                new OracleParameter(){ ParameterName="OrderNo" ,OracleDbType = OracleDbType.Varchar2, Size = 50, Value = model.OrderNo },
                new OracleParameter(){ ParameterName="Weight" ,OracleDbType = OracleDbType.Decimal, Value = model.Weight }
            };
            if (-1 == ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments))
            {
                return 1;
            }
            return -1;
        }
    }
}
