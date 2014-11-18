using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Model.Transport.Dispatch;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Exceptions;

namespace Vancl.TMS.DAL.Oracle.Transport.Dispatch
{
    public class DispOrderDetailDAL : BaseDAL, IDispOrderDetailDAL
    {
        #region IDispOrderDetailDAL 成员

        public void Add(DispOrderDetailModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加提货单订单明细
        /// </summary>
        /// <param name="model"></param>
        public void Add(List<DispOrderDetailModel> listmodel)
        {
            string sql = String.Format(@"
INSERT INTO TMS_DispOrderDetail(DODID, DDID, BoxNo, FormCode, ArrivalID , IsArrived, CreateBy, UpdateBy, DeliveryNO, Price, ProtectedPrice)
VALUES({0}, :DDID, :BoxNo, :FormCode, :ArrivalID , :IsArrived, :CreateBy, :UpdateBy, :DeliveryNO, :Price, :ProtectedPrice)
",
  listmodel[0].SequenceNextValue()
 );
            long[] arrDDID = new long[listmodel.Count];
            string[] arrBoxNo = new string[listmodel.Count];
            string[] arrFormCode = new string[listmodel.Count];
            int[] arrArrivalID = new int[listmodel.Count];
            bool[] arrIsArrived = new bool[listmodel.Count];
            int[] arrCreateBy = new int[listmodel.Count];
            int[] arrUpdateBy = new int[listmodel.Count];
            string[] arrDeliveryNO = new string[listmodel.Count];
            decimal[] arrPrice = new decimal[listmodel.Count];
            decimal[] arrProtectedPrice = new decimal[listmodel.Count];

            int pos = 0;                        //index
            listmodel.ForEach(p =>
            {
                arrDDID[pos] = p.DDID;
                arrBoxNo[pos] = p.BoxNo;
                arrFormCode[pos] = p.FormCode;
                arrArrivalID[pos] = p.ArrivalID;
                arrIsArrived[pos] = p.IsArrived;
                arrCreateBy[pos] = p.CreateBy;
                arrUpdateBy[pos] = p.UpdateBy;
                arrDeliveryNO[pos] = p.DeliveryNo;
                arrPrice[pos] = p.Price;
                arrProtectedPrice[pos] = p.ProtectedPrice;
                pos++;
            });
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter(){  ParameterName="DDID", DbType = DbType.Int64, Value = arrDDID},
                new OracleParameter(){ ParameterName="BoxNo", DbType=DbType.String, Value= arrBoxNo},
                new OracleParameter() { ParameterName="FormCode", DbType= DbType.String, Value= arrFormCode},
                new OracleParameter() { ParameterName="ArrivalID", DbType= DbType.Int32, Value= arrArrivalID},
                new OracleParameter() { ParameterName="IsArrived", DbType= DbType.Byte, Value=  arrIsArrived},
                new OracleParameter() { ParameterName="CreateBy", DbType= DbType.Int32, Value=  arrCreateBy},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value=  arrUpdateBy},
                new OracleParameter() { ParameterName="DeliveryNO", DbType= DbType.String, Value=  arrDeliveryNO},
                new OracleParameter() { ParameterName="Price", DbType= DbType.Decimal, Value=  arrPrice},
                new OracleParameter() { ParameterName="ProtectedPrice", DbType= DbType.Decimal, Value= arrProtectedPrice}
            };
            ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listmodel.Count, arguments);
        }

        public IList<ViewDispOrderDetailModel> GetOrderDetail(string deliveryNo, string boxNo)
        {
            string strSql = @"
                SELECT d.DeliveryNo,d.BoxNo,d.FormCode,o.Price,o.ProtectedPrice,o.GoodsType
                FROM TMS_DispOrderDetail d
                JOIN TMS_Order o
                    ON o.FormCode=d.FormCode
                        AND o.IsDeleted=0
                WHERE d.IsDeleted=0
                    AND d.DeliveryNo=:DeliveryNo
                    AND d.BoxNo=:BoxNo";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo },
                new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = boxNo }
            };
            return ExecuteSql_ByReaderReflect<ViewDispOrderDetailModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public decimal GetOrderTotalAmountByDeliveryNo(string deliveryNo)
        {
            string strSql = @"
                SELECT SUM(Price) TotalAmount
                FROM TMS_DispOrderDetail
                WHERE IsDeleted=0
                    AND DeliveryNo=:DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo }
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToDecimal(o);
            }
            return 0;
        }

        public decimal GetTotalProtectedPriceByDeliveryNo(string deliveryNo)
        {
            string strSql = @"
                SELECT SUM(ProtectedPrice) TotalProtectedPrice
                FROM TMS_DispOrderDetail
                WHERE IsDeleted=0
                    AND DeliveryNo=:DeliveryNo";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = deliveryNo }
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToDecimal(o);
            }
            return 0;
        }

        public decimal GetOrderTotalAmountByFormCodes(List<string> lstFormCode)
        {
            string strSql = BulidFormCodeCteSql(lstFormCode);
            strSql += @"
                SELECT SUM(dod.Price) TotalAmount
                FROM TMS_DispOrderDetail dod
                JOIN cte_formcode c
                    ON dod.FormCode=c.FormCode
                WHERE dod.IsDeleted=0";
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql);
            if (o != null)
            {
                return Convert.ToDecimal(o);
            }
            return 0;
        }

        public decimal GetTotalProtectedPriceByFormCodes(List<string> lstFormCode)
        {
            string strSql = BulidFormCodeCteSql(lstFormCode);
            strSql += @"
                SELECT SUM(dod.ProtectedPrice) TotalProtectedPrice
                FROM TMS_DispOrderDetail dod
                JOIN cte_formcode c
                    ON dod.FormCode=c.FormCode
                WHERE dod.IsDeleted=0";
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql);
            if (o != null)
            {
                return Convert.ToDecimal(o);
            }
            return 0;
        }

        public IList<ViewDispOrderDetailModel> GetOrderDetail(List<string> lstFormCode)
        {
            string strSql = BulidFormCodeCteSql(lstFormCode);
            strSql += @"
                SELECT d.DeliveryNo,d.BoxNo,d.FormCode,o.Price,o.ProtectedPrice,o.GoodsType
                FROM TMS_DispOrderDetail d
                JOIN TMS_Order o
                    ON o.FormCode=d.FormCode
                        AND o.IsDeleted=0
                JOIN cte_formcode c
                    ON d.FormCode=c.FormCode
                WHERE d.IsDeleted=0";
            return ExecuteSql_ByReaderReflect<ViewDispOrderDetailModel>(TMSReadOnlyConnection, strSql);
        }

        #endregion

        /// <summary>
        /// 构造单号CTE的SQL语句
        /// </summary>
        /// <param name="lstFormCode">单号列表</param>
        /// <returns></returns>
        private string BulidFormCodeCteSql(List<string> lstFormCode)
        {
            if (lstFormCode == null || lstFormCode.Count == 0)
            {
                throw new CodeNotValidException();
            }
            StringBuilder sb = new StringBuilder();
            foreach (string formCode in lstFormCode)
            {
                if (sb.Length > 0)
                {
                    sb.Append(@"
                        UNION ALL");
                }
                sb.AppendFormat(@"
                    SELECT '{0}' FormCode FROM DUAL", formCode);
            }
            sb.Insert(0, @"
                WITH cte_formcode as
                (")
            .Append(@"
                )");
            return sb.ToString();
        }

        public int Delete(string DeliveryNo)
        {
            string sql = @"
UPDATE TMS_DispOrderDetail
SET IsDeleted = 1 ,UpdateTime = sysdate  , UpdateBy = :UpdateBy     
WHERE DeliveryNo = :DeliveryNo AND  IsDeleted = 0
            ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value = DeliveryNo},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public bool IsExists(string formCode, int arrivalID)
        {
            string strSql = @"
                SELECT COUNT(1)
                FROM TMS_DispOrderDetail
                WHERE IsDeleted=0
                    AND FormCode=:FormCode
                    AND ArrivalID=:ArrivalID";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="FormCode",DbType= DbType.String,Value = formCode},
                new OracleParameter() { ParameterName="ArrivalID", DbType= DbType.Int32, Value = arrivalID}
            };
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (o != null)
            {
                return Convert.ToInt32(o) > 0;
            }
            return false;
        }

        #region IDispOrderDetailDAL 成员


        public int Delete(List<long> listDDID)
        {
            if (listDDID == null) throw new ArgumentNullException("listDDID is null");
            string sql = @"
UPDATE TMS_DispOrderDetail
SET IsDeleted = 1 ,UpdateTime = sysdate  , UpdateBy = :UpdateBy     
WHERE DDID = :DDID AND  IsDeleted = 0
";
            long[] arrDDID = new long[listDDID.Count];
            int[] arrUserID = new int[listDDID.Count];
            var userID = UserContext.CurrentUser.ID;
            for (int i = 0; i < listDDID.Count; i++)
            {
                arrDDID[i] = listDDID[i];
                arrUserID[i] = userID;
            }
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="DDID",DbType= DbType.Int64,Value = arrDDID },
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = arrUserID }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql,listDDID.Count, arguments);
        }

        #endregion

        #region IDispOrderDetailDAL 成员


        public int UpdateBy_ConfirmDispatch(List<long> DDID, string DeliveryNo)
        {
            if (DDID == null) throw new ArgumentNullException("DDID is null.");
            if (String.IsNullOrWhiteSpace(DeliveryNo)) throw new ArgumentNullException("DeliveryNo is null or empty.");
            if (DDID.Count <= 0) return 0;
            String sql = @"
UPDATE TMS_DispOrderDetail
SET DeliveryNo = :DeliveryNo
WHERE DDID = :DDID
AND IsDeleted = 0
";
            long[] arrDDID = new long[DDID.Count];
            String[] arrDeliveryNo = new String[DDID.Count];
            for (int i = 0; i < DDID.Count; i++)
            {
                arrDDID[i] = DDID[i];
                arrDeliveryNo[i] = DeliveryNo;
            }
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "DDID", DbType = DbType.Int64, Value = arrDDID },
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = arrDeliveryNo }
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, DDID.Count, parameters);            
        }

        #endregion
    }
}
