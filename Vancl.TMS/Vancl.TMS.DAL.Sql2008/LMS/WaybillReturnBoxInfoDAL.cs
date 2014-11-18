using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using System.Data.SqlClient;

namespace Vancl.TMS.DAL.Sql2008.LMS
{
    public class WaybillReturnBoxInfoDAL : BaseDAL, IWaybillReturnBoxInfoDAL
    {
        public long Add(WaybillReturnBoxInfoEntityModel model)
        { 
            if (model == null) throw new ArgumentNullException("model is null");
            String sql = @"
MERGE INTO WaybillReturnBoxInfo AS des
USING 
(  
   SELECT @BoxNo as BoxNo,@CreateBy as CreateBy,@CreateTime as CreateTime,@ReturnMerchant as ReturnMerchant,
   @ReturnTo as ReturnTo,@CreateDep as CreateDep,@Weight as Weight,@IsPrintBackPacking as IsPrintBackPacking,
   @IsPrintBackForm as IsPrintBackForm
) AS src
ON des.BoxNo = src.BoxNo
WHEN MATCHED THEN 
UPDATE SET des.ReturnMerchant=src.ReturnMerchant,des.ReturnTo=src.ReturnTo,des.CreateDep=src.CreateDep,
       des.Weight=src.Weight,des.IsPrintBackPacking=src.IsPrintBackPacking
WHEN NOT MATCHED THEN
INSERT (BoxNo,CreateBy,CreateTime,ReturnMerchant,ReturnTo,CreateDep,Weight,IsPrintBackPacking,IsPrintBackForm,IsDelete)
VALUES (src.BoxNo,src.CreateBy,src.CreateTime,src.ReturnMerchant,src.ReturnTo,src.CreateDep,src.Weight, src.IsPrintBackPacking,src.IsPrintBackForm,0);
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName = "@BoxNo",  SqlDbType= System.Data.SqlDbType.NVarChar , Size = 30 , Value = model.BoxNo },
                new SqlParameter() { ParameterName = "@CreateBy",  SqlDbType = System.Data.SqlDbType.Int , Value = model.CreateBy },
                new SqlParameter() { ParameterName = "@CreateTime", SqlDbType = System.Data.SqlDbType.DateTime , Value = model.CreateTime },
                new SqlParameter() { ParameterName = "@ReturnMerchant",  SqlDbType= System.Data.SqlDbType.NVarChar , Size = 50 , Value = model.ReturnMerchant },
                new SqlParameter() { ParameterName = "@ReturnTo",  SqlDbType= System.Data.SqlDbType.NVarChar , Size = 50 , Value = model.ReturnTo },
                new SqlParameter() { ParameterName = "@CreateDep", SqlDbType= System.Data.SqlDbType.NVarChar , Size = 50 , Value = model.CreateDep },
                new SqlParameter() { ParameterName = "@Weight",  SqlDbType = System.Data.SqlDbType.Decimal , Value = model.Weight },
                new SqlParameter() { ParameterName = "@IsPrintBackPacking",  SqlDbType = System.Data.SqlDbType.Bit , Value = model.IsPrintBackPacking},
                new SqlParameter() { ParameterName = "@IsPrintBackForm",   SqlDbType = System.Data.SqlDbType.Bit , Value = model.IsPrintBackForm }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public WaybillReturnBoxInfoEntityModel GetModel(long BoxNo)
        {
            String sql = @"
SELECT
    WaybillReturnBoxInfoID,
    BoxNo,
    CreateBy,
    ReturnMerchant,
    ReturnTo,
    CreateDep,
    Weight,
    IsPrintBackPacking,
    IsPrintBackForm
FROM WaybillReturnBoxInfo WITH (NOLOCK) 
WHERE BoxNo = @BoxNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@BoxNo", SqlDbType= System.Data.SqlDbType.NVarChar , Size = 30 , Value = BoxNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<WaybillReturnBoxInfoEntityModel>(LMSWriteConnection, sql, arguments);

        }

        /// <summary>
        /// 更新退货分拣称重箱重量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateBoxWeight(WaybillReturnBoxInfoEntityModel model)
        {
            return 0;
        }

        /// <summary>
        /// 打印退货交接表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int PrintBackForm(WaybillReturnBoxInfoEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            String sql = @"
UPDATE WaybillReturnBoxInfo
SET IsPrintBackForm = 1
WHERE BoxNo = @BoxNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@BoxNo", SqlDbType= System.Data.SqlDbType.NVarChar , Size = 30 , Value = model.BoxNo }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 装箱打印
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int PrintBackPacking(WaybillReturnBoxInfoEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            String sql = @"
UPDATE WaybillReturnBoxInfo
SET IsPrintBackPacking = 1,
    Weight = @Weight
WHERE BoxNo = @BoxNo
";
            SqlParameter[] arguments = new SqlParameter[] 
            {
                new SqlParameter() { ParameterName="@Weight",  SqlDbType = System.Data.SqlDbType.Decimal , Value = model.Weight },
                new SqlParameter() { ParameterName="@BoxNo", SqlDbType= System.Data.SqlDbType.NVarChar , Size = 30 , Value = model.BoxNo }
            };
            return ExecuteNonQuery(LMSWriteConnection, sql, arguments);

        }
    }
}
