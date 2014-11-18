using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Delivery.KPIAppraisal;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Delivery.KPIAppraisal
{
    public class AssFormulaPriceDAL : BaseDAL, IAssFormulaPriceDAL
    {
        #region IAssFormulaPriceDAL 成员

        public int Add(AssFormulaPriceModel model)
        {
            string strSql = @"
                INSERT INTO TMS_AssFormulaPrice(
                    DeliveryNo
                    ,BasePrice
                    ,BaseWeight
                    ,OverPrice
                    ,Note
                    ,CreateBy
                    ,UpdateBy
                    ,IsDeleted
                )
                VALUES(
                    :DeliveryNo
                    ,:BasePrice
                    ,:BaseWeight
                    ,:OverPrice
                    ,:Note
                    ,:CreateBy
                    ,:UpdateBy
                    ,0
                )";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo},
                new OracleParameter() { ParameterName="BasePrice",DbType= DbType.Decimal,Value=model.BasePrice},
                new OracleParameter() { ParameterName="BaseWeight",DbType= DbType.Int32,Value=model.BaseWeight},
                new OracleParameter() { ParameterName="OverPrice",DbType= DbType.Decimal,Value=model.OverPrice},
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value=model.Note},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Update(AssFormulaPriceModel model)
        {
            string strSql = @"
                UPDATE TMS_AssFormulaPrice
                SET BasePrice=:BasePrice
                    ,BaseWeight=:BaseWeight
                    ,OverPrice=:OverPrice
                    ,Note=:Note
                    ,UpdateTime=sysdate
                    ,UpdateBy=:UpdateBy
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="BasePrice",DbType= DbType.Decimal,Value=model.BasePrice},
                new OracleParameter() { ParameterName="BaseWeight",DbType= DbType.Int32,Value=model.BaseWeight},
                new OracleParameter() { ParameterName="OverPrice",DbType= DbType.Decimal,Value=model.OverPrice},
                new OracleParameter() { ParameterName="Note",DbType= DbType.String,Value=model.Note},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public bool IsExist(string deliveryNo)
        {
            string strSql = @"
                SELECT COUNT(1) CC
                FROM TMS_AssFormulaPrice
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                if (Convert.ToInt32(o) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public AssFormulaPriceModel Get(string deliveryNo)
        {
            string strSql = @"
                SELECT
                    DeliveryNo
                    ,BasePrice
                    ,BaseWeight
                    ,OverPrice
                    ,Note
                    ,CreateBy
                    ,CreateTime
                    ,UpdateBy
                    ,UpdateTime
                    ,IsDeleted
                FROM TMS_AssFormulaPrice
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            AssFormulaPriceModel model = ExecuteSqlSingle_ByReaderReflect<AssFormulaPriceModel>(TMSReadOnlyConnection, strSql, arguments);
            if (model != null)
            {
                model.Detail = GetOverPriceDetail(model.DeliveryNo);
            }
            return model;
        }

        public int AddOverPriceDetail(List<AssFormulaPriceExModel> detailModel)
        {
            if (null == detailModel || detailModel.Count < 1) throw new ArgumentNullException("AssFormulaPriceExModel is empty");
            string sql = String.Format(@"
INSERT INTO TMS_ASSFORMULAEX(AFEID,DeliveryNo,StartWeight,EndWeight,Price,CreateBy,UpdateBy)
VALUES({0},:DeliveryNo, :StartWeight, :EndWeight, :Price, :CreateBy, :UpdateBy)",
            detailModel[0].SequenceNextValue());
            string[] arrDeliveryNo = new string[detailModel.Count];
            int[] arrStartWeight = new int[detailModel.Count];
            int?[] arrEndWeight = new int?[detailModel.Count];
            decimal[] arrPrice = new decimal[detailModel.Count];
            int[] arrCreateBy = new int[detailModel.Count];
            int[] arrUpdateBy = new int[detailModel.Count];
            int pos = 0;
            detailModel.ForEach(p => 
            {
                arrDeliveryNo[pos] = p.DeliveryNo;
                arrStartWeight[pos] = p.StartWeight;
                arrEndWeight[pos] = p.EndWeight;
                arrPrice[pos] = p.Price;
                arrCreateBy[pos] = UserContext.CurrentUser.ID;
                arrUpdateBy[pos] = UserContext.CurrentUser.ID;
                pos++;
            });
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter(){ ParameterName="DeliveryNo", DbType= DbType.String, Value= arrDeliveryNo},
                new OracleParameter(){ ParameterName="StartWeight", DbType= DbType.Int32, Value= arrStartWeight},
                new OracleParameter(){ ParameterName="EndWeight", DbType= DbType.Int32, Value = arrEndWeight},
                new OracleParameter(){ ParameterName="Price", DbType= DbType.Decimal, Value=arrPrice},
                new OracleParameter(){ ParameterName="CreateBy", DbType= DbType.Int32, Value= arrCreateBy},
                new OracleParameter(){ ParameterName="UpdateBy", DbType= DbType.Int32, Value= arrUpdateBy}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, detailModel.Count, arguments);
        }

        public int DeleteOverPriceDetail(string DeliveryNo)
        {
            if (String.IsNullOrWhiteSpace(DeliveryNo)) throw new ArgumentNullException("DeliveryNo is empty");
            string sql = @"
UPDATE TMS_ASSFORMULAEX
SET UpdateBy = :UpdateBy, UpdateTime = sysdate, IsDeleted = 1
WHERE DeliveryNo = :DeliveryNo  AND  IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter(){ ParameterName="DeliveryNo", DbType= DbType.String, Value= DeliveryNo},
                new OracleParameter(){ ParameterName="UpdateBy", DbType= DbType.Int32, Value= UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public List<AssFormulaPriceExModel> GetOverPriceDetail(string DeliveryNo)
        {
            if (String.IsNullOrWhiteSpace(DeliveryNo)) throw new ArgumentNullException("DeliveryNo is empty");
            String sql = @"
SELECT AFEID,DELIVERYNO,STARTWEIGHT,ENDWEIGHT,PRICE,CREATEBY,CREATETIME,UPDATEBY,UPDATETIME,ISDELETED
FROM TMS_ASSFORMULAEX
WHERE DeliveryNo = :DeliveryNo  AND  IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter(){ ParameterName="DeliveryNo", DbType= DbType.String, Value= DeliveryNo}
            };
            return ExecuteSql_ByReaderReflect<AssFormulaPriceExModel>(TMSReadOnlyConnection, sql, arguments) as List<AssFormulaPriceExModel>;
        }

        #endregion
    }
}
