using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.BaseInfo.Order;

namespace Vancl.TMS.DAL.Oracle.BaseInfo
{
    public class OrderDAL : BaseDAL, IOrderDAL
    {
        #region IOrderDAL 成员

        public string GetFormCodeByWaybillNo(long waybillNo)
        {
            string strSql = @"
                SELECT FormCode
                FROM TMS_Order
                WHERE IsDeleted=0
                    AND LMSwaybillNo=:WaybillNo";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="WaybillNo",DbType= DbType.Int64,Value = waybillNo}
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToString(o);
            }
            return string.Empty;
        }

        public bool isExists(string formCode)
        {
            string strSql = @"
                SELECT COUNT(1)
                FROM TMS_Order
                WHERE IsDeleted=0
                    AND FormCode=:FormCode";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="FormCode",DbType= DbType.String,Value = formCode}
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToInt32(o) > 0;
            }
            return false;
        }

        #endregion

        #region IOrderDAL 成员


        public int Add(List<OrderModel> model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            if (model.Count <= 0) throw new ArgumentNullException("model.count <= 0");
            String sql = String.Format(@"
INSERT INTO  TMS_Order(OID, FormCode , CustomerOrder, LMSWaybillNo, LMSWaybillType , Price , ProtectedPrice, DepartureID , ArrivalID, GoodsType, BoxNo,CreateBy,UpdateBy)
VALUES( {0}, :FormCode ,:CustomerOrder , :LMSWaybillNo, :LMSWaybillType , :Price , :ProtectedPrice, :DepartureID , :ArrivalID, :GoodsType, :BoxNo, :CreateBy, :UpdateBy)
", model[0].SequenceNextValue()
);
            String[] arrFormCode = new String[model.Count];
            String[] arrCustomerOrder = new String[model.Count];
            long?[] arrLMSWaybillNo = new long?[model.Count];
            int?[] arrLMSWaybillType = new int?[model.Count];
            decimal[] arrPrice = new decimal[model.Count];
            decimal[] arrProtectedPrice = new decimal[model.Count];
            int[] arrDepartureID = new int[model.Count];
            int[] arrArrivalID = new int[model.Count];
            int[] arrGoodsType = new int[model.Count];
            String[] arrBoxNo = new String[model.Count];
            int[] arrCreateBy = new int[model.Count];
            int[] arrUpdateBy = new int[model.Count];
            int nPos = 0;
            model.ForEach(p =>
            {
                arrFormCode[nPos] = p.FormCode;
                arrCustomerOrder[nPos] = p.CustomerOrder;
                arrLMSWaybillNo[nPos] = p.LMSwaybillNo;
                arrLMSWaybillType[nPos] = p.LMSwaybillType;
                arrPrice[nPos] = p.Price;
                arrProtectedPrice[nPos] = p.ProtectedPrice;
                arrDepartureID[nPos] = p.DepartureID;
                arrArrivalID[nPos] = p.ArrivalID;
                arrGoodsType[nPos] =(int) p.GoodsType;
                arrBoxNo[nPos] = p.BoxNo;
                arrCreateBy[nPos] = p.CreateBy;
                arrUpdateBy[nPos] = p.UpdateBy;
                nPos++;
            });
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="FormCode", DbType = System.Data.DbType.String, Value =  arrFormCode },
                new OracleParameter(){ ParameterName="CustomerOrder", DbType = System.Data.DbType.String, Value = arrCustomerOrder},
                new OracleParameter(){ ParameterName="LMSWaybillNo", DbType = System.Data.DbType.Int64, Value = arrLMSWaybillNo},
                new OracleParameter(){ ParameterName="LMSWaybillType", DbType = System.Data.DbType.Int16, Value = arrLMSWaybillType},
                new OracleParameter(){ParameterName="Price", DbType= System.Data.DbType.Decimal, Value = arrPrice},
                new OracleParameter(){ ParameterName="ProtectedPrice", DbType= System.Data.DbType.Decimal, Value = arrProtectedPrice},
                new OracleParameter(){ ParameterName="DepartureID", DbType= System.Data.DbType.Int32, Value = arrDepartureID},
                new OracleParameter(){ ParameterName="ArrivalID", DbType = System.Data.DbType.Int32, Value = arrArrivalID},
                new OracleParameter(){ParameterName="GoodsType",DbType = System.Data.DbType.Int32,Value = arrGoodsType},
                new OracleParameter(){ ParameterName="BoxNo", DbType = System.Data.DbType.String, Value = arrBoxNo},    
                new OracleParameter(){ParameterName="CreateBy", DbType= System.Data.DbType.Int32, Value =  arrCreateBy},
                new OracleParameter(){ParameterName="UpdateBy", DbType= System.Data.DbType.Int32, Value =  arrUpdateBy}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, model.Count, arguments);
        }

        #endregion

        #region IOrderDAL 成员
        /// <summary>
        /// 取得已经存在于系统的单子
        /// </summary>
        /// <param name="formCodeList"></param>
        /// <returns></returns>
        public List<string> GetExistsFormCode(List<string> formCodeList)
        {
            if (formCodeList == null) throw new ArgumentNullException("formCodeList is null");
            if (formCodeList.Count <= 0) throw new ArgumentNullException("formCodeList.Count <= 0 ");
            String sql = @"
SELECT od.FormCode
FROM TMS_Order od
JOIN
(
    SELECT REGEXP_SUBSTR(:listFormCodeStr, '[^,]+', 1, LEVEL) AS FormCode
    FROM DUAL
    CONNECT BY LEVEL <=
    LENGTH(TRIM(TRANSLATE(:listFormCodeStr,TRANSLATE(:listFormCodeStr, ',', ' '), ' '))) + 1
) tmpFormCode ON od.FormCode = tmpFormCode.FormCode
WHERE od.IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="listFormCodeStr",  OracleDbType = OracleDbType.Varchar2, Size = 4000, Value = String.Join(",",formCodeList) }
            };
            var dtResult = ExecuteSqlDataTable(TMSWriteConnection, sql, arguments);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                List<string> listFormCode = new List<string>(dtResult.Rows.Count);
                foreach (DataRow item in dtResult.Rows)
                {
                    listFormCode.Add(item["FormCode"].ToString());
                }
                return listFormCode;
            }
            return null;
        }

        #endregion
    }
}
