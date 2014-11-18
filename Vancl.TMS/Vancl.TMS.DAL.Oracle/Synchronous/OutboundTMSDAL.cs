using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Synchronous;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;
using System.Data;
using Vancl.TMS.Model.BaseInfo.Order;

namespace Vancl.TMS.DAL.Oracle.Synchronous
{
    /// <summary>
    /// 出库同步从文件到TMS系统
    /// </summary>
    public class OutboundTMSDAL : BaseDAL, IOutboundTMSDAL
    {

        #region IOutboundTMSDAL 成员

        private string GenerateTempRecordstr(List<OrderModel> listOrder)
        {
            StringBuilder strBuffer = new StringBuilder();
            string dual = " FROM DUAL ";
            string unionall = " UNION ALL ";
            string select = " SELECT ";
            for (int i = 0; i < listOrder.Count; i++)
            {
                strBuffer.Append(select);
                strBuffer.Append
                    (
                        string.Format(@" '{0}'AS FormCode,{1} AS LMSWaybillNo,{2} AS LMSWaybillType,{3} AS Price
                            ,{4} AS DepartureID,{5} AS ArrivalID ,{6} AS GoodsType,'{7}' AS BoxNo,{8} AS ProtectedPrice, '{9}' AS CustomerOrder "
                        , listOrder[i].FormCode.Replace("'", "")
                        , listOrder[i].LMSwaybillNo.HasValue ? listOrder[i].LMSwaybillNo.Value.ToString() : "0"
                        , listOrder[i].LMSwaybillType.HasValue ? listOrder[i].LMSwaybillType.Value.ToString() : "NULL"
                        , listOrder[i].Price
                        , listOrder[i].DepartureID
                        , listOrder[i].ArrivalID
                        , (int)listOrder[i].GoodsType
                        , listOrder[i].BoxNo.Replace("'", "")
                        , listOrder[i].ProtectedPrice
                        , listOrder[i].CustomerOrder
                        )
                    );
                strBuffer.Append(dual);
                strBuffer.Append(unionall);
            }
            return strBuffer.ToString().Substring(0, strBuffer.Length - unionall.Length);
        }

        private string GenerateTempRecordstr(List<OrderDetailModel> listOrderDetail)
        {
            StringBuilder strBuffer = new StringBuilder();
            string dual = " FROM DUAL ";
            string unionall = " UNION ALL ";
            string select = " SELECT ";
            for (int i = 0; i < listOrderDetail.Count; i++)
            {
                strBuffer.Append(select);
                strBuffer.Append
                    (
                        string.Format(@" '{0}' as FormCode,'{1}' as ProductName,'{2}' as ProductCode,
                            {3} as ProductCount,'{4}' as ProductUnit,{5} as ProductPrice,'{6}' as ProductSize"
                        , listOrderDetail[i].FormCode.Replace("'", "")
                        , String.IsNullOrWhiteSpace(listOrderDetail[i].ProductName) ? "  " : listOrderDetail[i].ProductName.Replace("'", "")
                        , String.IsNullOrWhiteSpace(listOrderDetail[i].ProductCode) ? "  " : listOrderDetail[i].ProductCode.Replace("'", "")
                        , listOrderDetail[i].ProductCount
                        , String.IsNullOrWhiteSpace(listOrderDetail[i].ProductUnit) ? "  " : listOrderDetail[i].ProductUnit.Replace("'", "")
                        , listOrderDetail[i].ProductPrice
                        , String.IsNullOrWhiteSpace(listOrderDetail[i].ProductSize) ? "  " : listOrderDetail[i].ProductSize.Replace("'", "")
                        )
                    );
                strBuffer.Append(dual);
                strBuffer.Append(unionall);
            }
            return strBuffer.ToString().Substring(0, strBuffer.Length - unionall.Length);
        }

        public void ImportOrderDetailList(List<OrderDetailModel> listOrderDetail)
        {
            string sql = String.Format(@"
INSERT INTO  TMS_OrderDetail (ODID, FormCode, ProductName, ProductCode,  ProductCount ,ProductUnit ,ProductPrice , ProductSize)
SELECT SEQ_TMS_OrderDetail_ODID.NEXTVAL, FormCode, ProductName, ProductCode, ProductCount ,ProductUnit ,ProductPrice , ProductSize
FROM
(
    WITH CTE_ALL AS
    (
        {0}
    ),
    CTE_EQUAL AS
    (
        SELECT tmpAll.*
        FROM CTE_ALL tmpAll JOIN TMS_OrderDetail  tod 
        ON tmpAll.FormCode = tod.FormCode and tmpAll.ProductCode = tod.ProductCode and  tmpAll.ProductSize = tod.ProductSize
    ),
    CTE_Result AS
    (
        SELECT tmpAll.*
        FROM CTE_ALL  tmpAll LEFT JOIN CTE_EQUAL tmpEqual 
        ON tmpAll.FormCode = tmpEqual.FormCode and tmpAll.ProductCode = tmpEqual.ProductCode and  tmpAll.ProductSize = tmpEqual.ProductSize
        WHERE tmpEqual.FormCode IS NULL
    )
    SELECT * 
    FROM CTE_Result
)
"
                , GenerateTempRecordstr(listOrderDetail));
            ExecuteSqlNonQuery(TMSWriteConnection, sql);
        }

        public void ImportOrder(List<OrderModel> listOrder)
        {
            string sql = String.Format(@"
INSERT INTO  TMS_Order(OID, FormCode , CustomerOrder, LMSWaybillNo, LMSWaybillType , Price , ProtectedPrice, DepartureID , ArrivalID, GoodsType, BoxNo,CreateBy,UpdateBy)
SELECT SEQ_TMS_Order_OID.NEXTVAL, FormCode ,CustomerOrder , LMSWaybillNo, LMSWaybillType , Price , ProtectedPrice, DepartureID , ArrivalID, GoodsType, BoxNo, 0, 0
FROM
(
    WITH CTE_ALL AS
    (
        {0}
    ),
    CTE_EQUAL AS
    (
        SELECT tmpAll.*
        FROM CTE_ALL tmpAll JOIN TMS_ORDER  too
        ON tmpAll.FormCode = too.FormCode 
    ),
    CTE_Result AS
    (
        SELECT tmpAll.*
        FROM CTE_ALL  tmpAll LEFT JOIN CTE_EQUAL tmpEqual 
        ON tmpAll.FormCode = tmpEqual.FormCode 
        WHERE tmpEqual.FormCode IS NULL
    )
    SELECT * 
    FROM CTE_Result
)
"
               , GenerateTempRecordstr(listOrder));
            ExecuteSqlNonQuery(TMSWriteConnection, sql);
        }

        public void ImportBox(BoxModel boxModel)
        {
            string sql = @"
INSERT INTO TMS_BOX(BID,BoxNo, DepartureID ,ArrivalID ,TotalCount, TotalAmount, ProtectedPrice,Weight, BoxType, ContentType , CreateBy, UpdateBy)
VALUES(SEQ_TMS_BOX_BID.NEXTVAL ,:BoxNo ,:DepartureID ,:ArrivalID, :TotalCount ,:TotalAmount ,:ProtectedPrice ,:Weight,1 ,:ContentType ,0 ,0)
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="BoxNo", DbType = System.Data.DbType.String, Value=boxModel.BoxNo},
                new OracleParameter(){ ParameterName="DepartureID", DbType= System.Data.DbType.Int32, Value=boxModel.DepartureID},
                new OracleParameter(){ ParameterName="ArrivalID", DbType = System.Data.DbType.Int32, Value= boxModel.ArrivalID},
                new OracleParameter(){ ParameterName="TotalCount", DbType= System.Data.DbType.Int32,Value=boxModel.TotalCount},
                new OracleParameter(){ ParameterName="TotalAmount", DbType= System.Data.DbType.Decimal, Value=boxModel.TotalAmount},
                new OracleParameter(){ ParameterName="ProtectedPrice", DbType= System.Data.DbType.Decimal, Value = boxModel.ProtectedPrice},
                new OracleParameter(){ParameterName="Weight", DbType= System.Data.DbType.Decimal, Value=boxModel.Weight},
                new OracleParameter(){ParameterName="ContentType",DbType = System.Data.DbType.Int32,Value=(int)boxModel.ContentType}
            };
            ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }


        public bool IsExistsBoxNo(string boxNo)
        {
            string sql = @"
SELECT COUNT(*) AS CT
FROM TMS_BOX
WHERE BoxNo=:BoxNo
";
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="BoxNo", DbType = System.Data.DbType.String, Value=boxNo}
            };
            object Intval = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            return Convert.ToInt32(Intval) > 0;
        }

        public void UpdatePreDispatchByBoxNo(string boxNo, int DepartureID)
        {
            string strSql = string.Format(@"
                UPDATE TMS_PreDispatch
                SET DispatchStatus=:DispatchStatus, UpdateBy = 0, UpdateTime = sysdate
                WHERE IsDeleted=0
                    AND BoxNo=:BoxNo
                    AND DepartureID=:DepartureID
                    AND DispatchStatus={0}", (int)Enums.DispatchStatus.CanNotDispatch);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DispatchStatus",DbType = DbType.Int32,Value = (int)Enums.DispatchStatus.CanDispatch},
                new OracleParameter() { ParameterName="BoxNo",DbType = DbType.String,Value = boxNo},
                new OracleParameter() { ParameterName="DepartureID",DbType = DbType.Int32,Value = DepartureID},
            };
            ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public void UpdateBoxOutBoundTime(string boxNo)
        {
            string sql = @"
UPDATE TMS_Box
SET UpdateBy = 0,UpdateTime = sysdate
WHERE IsDeleted=0 AND BoxNo=:BoxNo
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="BoxNo",DbType = DbType.String,Value = boxNo}
            };
            ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public void ImportBoxDetail(List<BoxDetailModel> lstBoxDetail)
        {
            string strSql = String.Format(@"
                INSERT INTO  TMS_BoxDetail (BDID, BoxNo, FormCode, CreateBy, UpdateBy)
                SELECT SEQ_TMS_BoxDetail_BDID.NEXTVAL, BoxNo, FormCode,0,0
                FROM
                (
                    WITH CTE_ALL AS
                    (
                        {0}
                    ),
                    CTE_EQUAL AS
                    (
                        SELECT tmpAll.*
                        FROM CTE_ALL tmpAll 
                        JOIN TMS_BoxDetail  tbd 
                            ON tmpAll.BoxNo = tbd.BoxNo 
                            AND tmpAll.FormCode = tbd.FormCode
                    ),
                    CTE_Result AS
                    (
                        SELECT tmpAll.*
                        FROM CTE_ALL  tmpAll 
                        LEFT JOIN CTE_EQUAL tmpEqual 
                            ON tmpAll.BoxNo = tmpEqual.BoxNo
                                AND tmpAll.FormCode = tmpEqual.FormCode
                        WHERE tmpEqual.BoxNo IS NULL
                    )
                    SELECT * 
                    FROM CTE_Result
                )", GenerateTempRecordstr(lstBoxDetail));
            ExecuteSqlNonQuery(TMSWriteConnection, strSql);
        }

        private string GenerateTempRecordstr(List<BoxDetailModel> lstBoxDetail)
        {
            StringBuilder strBuffer = new StringBuilder();
            string dual = " FROM DUAL ";
            string unionall = " UNION ALL ";
            string select = " SELECT ";
            for (int i = 0; i < lstBoxDetail.Count; i++)
            {
                strBuffer.Append(select);
                strBuffer.Append
                    (
                        string.Format(@" '{0}' as BoxNo,'{1}' as FormCode"
                        , lstBoxDetail[i].BoxNo.Replace("'", "")
                        , lstBoxDetail[i].FormCode.Replace("'", "")
                        )
                    );
                strBuffer.Append(dual);
                strBuffer.Append(unionall);
            }
            return strBuffer.ToString().Substring(0, strBuffer.Length - unionall.Length);
        }
        #endregion
    }

}
