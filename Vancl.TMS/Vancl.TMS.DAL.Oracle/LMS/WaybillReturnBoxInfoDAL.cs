using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.LMS;
using Vancl.TMS.Model.LMS;
using Oracle.DataAccess.Client;

namespace Vancl.TMS.DAL.Oracle.LMS
{
    public class WaybillReturnBoxInfoDAL : LMSBaseDAL, IWaybillReturnBoxInfoDAL
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public long Add(WaybillReturnBoxInfoEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            //model.WaybillReturnBoxInfoID = GetNextSequence(model.SequenceName);
            String sql = String.Format(@"
MERGE INTO WaybillReturnBoxInfo des
USING 
(  
   SELECT :boxno as boxno,:createby as createby,:createtime as createtime,:returnmerchant as returnmerchant,
   :returnto as returnto,:createdep as createdep,:weight as weight,:isprintbackpacking as isprintbackpacking,
   :isprintbackform as isprintbackform
   FROM dual
) src
ON ( des.BoxNo = src.BoxNo )
WHEN MATCHED THEN 
UPDATE SET des.returnmerchant=src.returnmerchant,des.returnto=src.returnto,des.createdep=src.createdep,
       des.weight=src.weight,des.isprintbackpacking=src.isprintbackpacking,des.isprintbackform=src.isprintbackform
WHEN NOT MATCHED THEN
INSERT (WAYBILLRETURNBOXINFOID,boxno,createby,createtime,returnmerchant,returnto,
createdep,weight,isprintbackpacking,isprintbackform,isdelete )
VALUES ({0},src.boxno,src.createby,src.createtime,src.returnmerchant,src.returnto,
src.createdep,src.weight,src.isprintbackpacking,src.isprintbackform,0 )", GetNextSequence(model.SequenceName));

            OracleParameter[] arguments = new OracleParameter[] 
            {
                //new OracleParameter(){ ParameterName="WAYBILLRETURNBOXINFOID" ,DbType = System.Data.DbType.Int64, Value = model.WaybillReturnBoxInfoID },
                new OracleParameter(){ ParameterName="boxno" ,OracleDbType = OracleDbType.Varchar2, Size = 60, Value = model.BoxNo },
                new OracleParameter(){ ParameterName="createby" ,OracleDbType = OracleDbType.Int32, Value = model.CreateBy },
                new OracleParameter(){ ParameterName="createtime" ,OracleDbType = OracleDbType.Date, Value = model.CreateTime },
                new OracleParameter(){ ParameterName="returnmerchant" ,OracleDbType = OracleDbType.Varchar2, Size = 100, Value = model.ReturnMerchant },
                new OracleParameter(){ ParameterName="returnto" ,OracleDbType = OracleDbType.Varchar2, Size = 100, Value = model.ReturnTo },
                new OracleParameter(){ ParameterName="createdep" ,OracleDbType = OracleDbType.Varchar2, Size = 100, Value = model.CreateDep },
                new OracleParameter(){ ParameterName="weight" ,OracleDbType = OracleDbType.Decimal, Value = model.Weight },
                new OracleParameter(){ ParameterName="isprintbackpacking" ,OracleDbType = OracleDbType.Int16, Value = Convert.ToInt16(model.IsPrintBackPacking) },
                new OracleParameter(){ ParameterName="isprintbackform" ,OracleDbType = OracleDbType.Int16, Value = Convert.ToInt16(model.IsPrintBackForm) }
            };
            if (-1 == ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments))
            {
                return 1;
            }
            return 0;
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
FROM WaybillReturnBoxInfo 
WHERE BoxNo = :BoxNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2, Size = 60, Value = BoxNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<WaybillReturnBoxInfoEntityModel>(LMSOracleWriteConnection, sql, arguments);

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
SET IsPrintBackForm=1
WHERE BoxNo = :BoxNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2, Size = 60, Value = model.BoxNo }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
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
SET IsPrintBackPacking=1,
    Weight=:Weight
WHERE BoxNo = :BoxNo
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal, Value = model.Weight },
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2, Size = 60, Value = model.BoxNo }
            };
            return ExecuteSqlNonQuery(LMSOracleWriteConnection, sql, arguments);
        }
    }
}
