using System.Collections.Generic;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Model.Sorting.Inbound.Packing;
using Vancl.TMS.IDAL.Sorting.Inbound;
using System;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Sorting;

namespace Vancl.TMS.DAL.Oracle.Sorting.Inbound
{
    /// <summary>
    /// 分拣装箱数据层
    /// </summary>
    public class SortingPackingDAL : BaseDAL, ISortingPackingDAL
    {
        #region ISortingPackingDAL 成员

        /// <summary>
        /// 添加装箱数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddInboundPacking(InboundPackingEntityModel model)
        {
            string strSql = string.Format(@"
INSERT INTO SC_InboundPacking( IPID ,BoxNo ,Weight  ,BillCount ,InboundType  ,DepartureID ,ArrivalID ,IsOutbounded  ,CreateBy  ,UpdateBy ,SyncFlag )
VALUES( {0}  , :BoxNo  , :Weight , :BillCount , :InboundType  , :DepartureID , :ArrivalID  , :IsOutbounded  , :CreateBy  , :CreateBy   , :SyncFlag)
", model.KeyCodeNextValue());
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.BoxNo },
                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal , Value = model.Weight },
                new OracleParameter() { ParameterName="BillCount", OracleDbType = OracleDbType.Int32, Value = model.BillCount },
                new OracleParameter() { ParameterName="InboundType", OracleDbType = OracleDbType.Int32,Value = (int)model.InboundType},
                new OracleParameter() { ParameterName="DepartureID", OracleDbType = OracleDbType.Int32,Value = model.DepartureID},
                new OracleParameter() { ParameterName="ArrivalID", OracleDbType = OracleDbType.Int32,Value = model.ArrivalID},
                new OracleParameter() { ParameterName="IsOutbounded", OracleDbType = OracleDbType.Int32 ,Value =Convert.ToInt32(model.IsOutbounded) },
                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32,Value = model.CreateBy},
                new OracleParameter() { ParameterName="Syncflag", OracleDbType = OracleDbType.Int32,Value = (int)Enums.SyncStatus.NotYet}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        /// <summary>
        /// 批量添加装箱数据明细
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <param name="formCodeList">单号列表</param>
        /// <param name="userID">操作人</param>
        /// <returns></returns>
        public int BatchAddInboundPackingDetail(string boxNo, List<string> formCodeList, int userID)
        {
            string strSql = string.Format(@"
INSERT INTO SC_InboundPackingDetail(IPDID ,BoxNo  ,FormCode   ,CreateBy   ,UpdateBy    ,SyncFlag )
VALUES (  {0}  ,:BoxNo   ,:FormCode   ,:CreateBy   ,:CreateBy    ,{1})
", new InboundPackingDetailModel().KeyCodeNextValue(), 0);
            string[] arrBoxNo = new string[formCodeList.Count];
            int[] arrCreateBy = new int[formCodeList.Count];
            for (int i = 0; i < formCodeList.Count; i++)
            {
                arrBoxNo[i] = boxNo;
                arrCreateBy[i] = userID;
            }
            OracleParameter[] parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "BoxNo" , OracleDbType  = OracleDbType.Varchar2 , Size = 50 , Value = arrBoxNo },
                new OracleParameter(){ParameterName = "FormCode" , OracleDbType  = OracleDbType.Varchar2 , Size = 50 ,Value = formCodeList.ToArray()},
                new OracleParameter(){ParameterName = "CreateBy" , OracleDbType = OracleDbType.Int32 , Value = arrCreateBy}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, formCodeList.Count, parameters);
        }

        public List<SortingPackingCheckModel> GetPackingCheckModel(List<string> lstFormCode, int expressID)
        {
            string strSql = string.Format(@"
SELECT b.FormCode
,b.Status
,b.DeliverStationID AS ArrivalID
,CASE WHEN EXISTS
(
    SELECT ipd.ipdid
    FROM SC_InboundPackingDetail ipd
    JOIN SC_InboundPacking ip
        ON ipd.BoxNo = ip.BoxNo
            AND ip.DepartureID = :DepartureID
            AND ip.IsDeleted = 0
    WHERE ipd.FormCode = b.FormCode
        AND ipd.IsDeleted = 0
) THEN 1
  ELSE 0 END IsPacked
,ib.DepartureID
,ib.InboundType
FROM SC_Bill b
    JOIN SC_Inbound ib  ON ib.ibid = b.InboundKey  AND ib.IsDeleted = 0
WHERE b.FormCode IN ({0})
    AND b.IsDeleted = 0
"
                , "'" + string.Join("','", lstFormCode) + "'");
            OracleParameter[] parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "DepartureID" , OracleDbType = OracleDbType.Int32 , Value = expressID     }
            };
            return (List<SortingPackingCheckModel>)ExecuteSql_ByDataTableReflect<SortingPackingCheckModel>(TMSWriteConnection, strSql, parameters);
        }

        public List<string> GetFormCodesByBoxNo(string boxNo)
        {
            string strSql = @"
                SELECT FormCode
                FROM   SC_InboundPackingDetail ipd
                WHERE  BoxNo =:BoxNo
                    AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "BoxNo" ,DbType = DbType.String , Value = boxNo}
            };
            DataTable dt = ExecuteSqlDataTable(TMSWriteConnection, strSql, parameters);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            List<string> list = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                list.Add(Convert.ToString(dr["FormCode"]));
            }
            return list;
        }

        public int BatchDeleteInboundPackingDetail(string boxNo, List<string> lstFormCode)
        {
            string strSql = string.Format(@"
UPDATE SC_InboundPackingDetail
SET IsDeleted = 1
    ,UpdateBy = :UpdateBy
    ,UpdateTime = sysdate
WHERE BoxNo = :BoxNo
    AND IsDeleted = 0
    AND FormCode IN ({0})
", "'" + string.Join("','", lstFormCode) + "'");
            OracleParameter[] parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "UpdateBy" , OracleDbType = OracleDbType.Int32 , Value = UserContext.CurrentUser.ID },
                new OracleParameter(){ParameterName = "BoxNo" , OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = boxNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        public int DeleteInboundPacking(string boxNo)
        {
            string strSql = @"
                UPDATE SC_InboundPacking
                SET IsDeleted=1
                    ,UpdateBy=:UpdateBy
                    ,UpdateTime=sysdate
                WHERE BoxNo=:BoxNo
                    AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "UpdateBy" ,DbType = DbType.Int32 , Value = UserContext.CurrentUser.ID},
                new OracleParameter(){ParameterName = "BoxNo" ,DbType = DbType.String , Value = boxNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, parameters);
        }

        public List<SortingPackingBillModel> GetPackingBillsByBoxNo(string boxNo)
        {
            string strSql = @" SELECT ipd.FormCode,
                                       ipd.BoxNo,
                                       b.DeliverStationID AS ArrivalID,
                                       ec.CompanyName AS ArrivalName,
                                       ec.DistributionCode AS ArrivalDistributionCode,
                                       d.distributionname AS ArrivalDistributionName,
                                       ib.DepartureID
                                  FROM SC_InboundPackingDetail ipd
                                  JOIN SC_BILL b ON b.FormCode = ipd.FormCode AND b.IsDeleted = 0
                                  JOIN ExpressCompany ec ON ec.ExpressCompanyID = b.DeliverStationID
                                  JOIN SC_Inbound ib ON ib.IBID = b.InboundKey
                                  JOIN Distribution d on d.distributioncode = ec.DistributionCode
                                 WHERE ipd.BoxNo = :BoxNo
                                   AND ipd.IsDeleted = 0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "BoxNo" ,DbType = DbType.String , Value = boxNo}
            };
            return (List<SortingPackingBillModel>)ExecuteSql_ByDataTableReflect<SortingPackingBillModel>(TMSWriteConnection, strSql, parameters);
        }

        public List<SortingPackingBillModel> GetPackingBillsByFormCode(string formCode)
        {
            string strSql = @"
SELECT ipd.FormCode
    ,ipd.BoxNo
    ,b.DeliverStationID AS ArrivalID
    ,ec.CompanyName AS ArrivalName
    ,ec.DistributionCode AS ArrivalDistributionCode
    ,ib.DepartureID
    ,d.distributionname as ArrivalDistributionName
FROM SC_InboundPackingDetail ipd
JOIN SC_BILL b ON b.FormCode = ipd.FormCode  AND b.IsDeleted = 0
JOIN ExpressCompany ec  ON ec.ExpressCompanyID = b.DeliverStationID
JOIN SC_Inbound ib  ON ib.IBID = b.InboundKey
JOIN Distribution d on d.distributioncode = ec.DistributionCode
WHERE ipd.BoxNo =
    (
        SELECT scipd.BoxNo
        FROM SC_InboundPackingDetail scipd
        WHERE scipd.FormCode=:FormCode
            AND scipd.IsDeleted=0
            AND ROWNUM=1
    )
    AND ipd.IsDeleted=0
";
            OracleParameter[] parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "FormCode" , OracleDbType = OracleDbType.Varchar2 , Size = 50,  Value = formCode}
            };
            return (List<SortingPackingBillModel>)ExecuteSql_ByDataTableReflect<SortingPackingBillModel>(TMSWriteConnection, strSql, parameters);
        }

        public SortingPackingBoxModel GetPackingBox(string boxNo)
        {
            if (String.IsNullOrWhiteSpace(boxNo)) throw new ArgumentNullException("boxNo is null or empty.");
            string strSql = @"
SELECT ip.BoxNo
    ,ip.Weight
    ,ip.InboundType
    ,ip.ArrivalID
    ,ec.CompanyName AS ArrivalCompanyName
    ,ec.CompanyFlag AS ArrivalCompanyFlag
    ,ec.DistributionCode AS ArrivalDistributionCode
    ,d.DistributionName AS ArrivalDistributionName
FROM SC_InboundPacking ip
JOIN ExpressCompany ec ON ec.ExpressCompanyID = ip.ArrivalID
JOIN Distribution d on d.distributioncode = ec.distributioncode
WHERE ip.BoxNo = :BoxNo
    AND ip.IsDeleted = 0
    AND ROWNUM=1
";
            OracleParameter[] parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "BoxNo" , OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = boxNo}
            };
            return ExecuteSqlSingle_ByDataTableReflect<SortingPackingBoxModel>(TMSWriteConnection, strSql, parameters);
        }

        public bool IsBillAlreadyPacked(string formCode, int expressID)
        {
            string strSql = @"
SELECT COUNT(1)
FROM SC_InboundPackingDetail ipd
    JOIN SC_InboundPacking ip ON ip.BoxNo = ipd.BoxNo
        AND ip.DepartureID = :DepartureID
        AND ip.IsDeleted = 0
WHERE ipd.FormCode = :FormCode
    AND ipd.IsDeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "DepartureID" , OracleDbType = OracleDbType.Int32 , Value = expressID },
                new OracleParameter(){ParameterName = "FormCode" , OracleDbType = OracleDbType.Varchar2  ,  Size = 50 , Value = formCode}
            };
            object o = ExecuteSqlScalar(TMSWriteConnection, strSql, parameters);
            if (o == null || o == DBNull.Value)
            {
                return false;
            }
            return Convert.ToInt32(o) > 0;
        }

        public SortingPackingBillModel GetSortingPackingBill(string formCode)
        {
            string strSql = string.Format(@"
SELECT b.FormCode
    ,b.Status
    ,b.DeliverStationID AS ArrivalID
    ,ec.CompanyName AS ArrivalName
    ,ec.DistributionCode AS ArrivalDistributionCode
    ,d.distributionname AS ArrivalDistributionName
    ,ib.DepartureID AS DepartureID
    ,b.CustomerOrder
    ,b.DeliverCode
    ,CASE WHEN EXISTS(
        SELECT  iq.ibsid
        FROM SC_InboundQueue iq
        WHERE iq.FormCode = :FormCode
            AND iq.SeqStatus = {0}
            AND iq.IsDeleted = 0
        ) THEN 1
    ELSE 0 END AS IsInbounding
FROM SC_Bill b
    JOIN ExpressCompany ec  ON ec.ExpressCompanyID = b.DeliverStationID
    JOIN Distribution d on d.distributioncode = ec.DistributionCode
LEFT JOIN SC_Inbound ib   ON ib.ibid = b.InboundKey  AND ib.IsDeleted = 0
WHERE b.FormCode = :FormCode
    AND b.IsDeleted = 0
    AND ROWNUM = 1
", (int)Enums.SeqStatus.NoHand);
            OracleParameter[] parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "FormCode" , OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = formCode}
            };
            return ExecuteSqlSingle_ByDataTableReflect<SortingPackingBillModel>(TMSWriteConnection, strSql, parameters);
        }

        public Enums.BoxOutBoundedStatus GetBoxOutBoundedStatus(string boxNo)
        {
            string strSql = @"
                SELECT IsOutBounded
                FROM SC_InboundPacking
                WHERE BoxNo=:BoxNo";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "BoxNo" ,DbType = DbType.String , Value = boxNo}
            };
            object o = ExecuteSqlScalar(TMSWriteConnection, strSql, parameters);
            if (o == null || o == DBNull.Value)
            {
                return Enums.BoxOutBoundedStatus.NotExists;
            }
            return Convert.ToInt32(o) == 0 ? Enums.BoxOutBoundedStatus.NotYet : Enums.BoxOutBoundedStatus.Outbounded;
        }
        #endregion

        #region ISortingPackingDAL 成员

        /// <summary>
        /// 取得装箱打印信息
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        public SortingPackingPrintModel GetPackingPrintModel(string boxNo)
        {
            if (String.IsNullOrWhiteSpace(boxNo)) throw new ArgumentNullException("boxNo is null or empty.");
            String sql = @"
SELECT Packing.BoxNo
, Packing.CreateTime AS DepartureTime
, ECA.CompanyName AS  ArrivalName
, ECD.CompanyName AS  DepartureName
, Packing.BillCount
, Packing.Weight AS TotalWeight
,  E.EmployeeName AS PackingOpUser
, d.DISTRIBUTIONNAME
, Packing.DepartureID
, Packing.ArrivalID
FROM SC_InboundPacking Packing
    JOIN ExpressCompany ECD ON Packing.DepartureID = ECD.ExpressCompanyID
    JOIN ExpressCompany ECA ON Packing.ArrivalID = ECA.ExpressCompanyID
    JOIN Employee E ON Packing.CreateBy = E.EmployeeID
    join DISTRIBUTION d on d.DISTRIBUTIONCODE = e.distributioncode
WHERE Packing.BoxNo = :BoxNo
    AND Packing.IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = boxNo }
            };
            return ExecuteSqlSingle_ByReaderReflect<SortingPackingPrintModel>(TMSReadOnlyConnection, sql, arguments);
        }

        /// <summary>
        /// 根据发货地，目的地，日期，获得装了多少箱
        /// </summary>
        /// <param name="departureId"></param>
        /// <param name="arrivalId"></param>
        /// <param name="packingDate"></param>
        /// <returns></returns>
        public List<SortingPackingPrintModel> GetPackingModelList(int departureId, int arrivalId, DateTime packingDate)
        {
            string strSql = @"select ibp.BOXNO as BoxNo, ibp.CREATETIME as DepartureTime
                              from sc_inboundpacking ibp
                             where ibp.DEPARTUREID = :departureId
                               and ibp.ARRIVALID = :arrivalId
                               and ibp.CREATETIME >= :bTime
                               and ibp.CREATETIME < :eTime ";


            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "departureId" ,DbType = DbType.Int32 , Value = departureId},
                new OracleParameter(){ParameterName = "arrivalId" ,DbType = DbType.Int32 , Value = arrivalId},
                new OracleParameter(){ParameterName = "bTime" ,DbType = DbType.DateTime , Value = packingDate.Date},
                new OracleParameter(){ParameterName = "eTime" ,DbType = DbType.DateTime , Value = packingDate.Date.AddDays(1)}
            };
            return (List<SortingPackingPrintModel>)ExecuteSql_ByDataTableReflect<SortingPackingPrintModel>(TMSWriteConnection, strSql, parameters);

        }


        #endregion

        #region ISortingPackingDAL 成员

        /// <summary>
        /// 更新装箱数据
        /// </summary>
        /// <param name="model">装箱对象</param>
        /// <returns></returns>
        public int UpdateInboundPacking(InboundPackingEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundPackingEntityModel is null");
            String strSql = @"
UPDATE SC_InboundPacking
SET BillCount = :BillCount
    ,Weight = :Weight
    ,UpdateBy = :UpdateBy
    ,UpdateTime = sysdate
WHERE BoxNo = :BoxNo
    AND IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.BoxNo },
                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal , Value = model.Weight },
                new OracleParameter() { ParameterName="BillCount", OracleDbType = OracleDbType.Int32, Value = model.BillCount },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32,Value = model.UpdateBy }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int UpdateBoxWeightWhenPrint(string boxNo, Decimal weight, int updateBy)
        {

            String strSql = @"UPDATE SC_InboundPacking
                            SET  Weight = :Weight
                                ,UpdateBy = :UpdateBy
                                ,UpdateTime = sysdate
                            WHERE BoxNo = :BoxNo
                                AND IsDeleted = 0
                            ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = boxNo },
                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal , Value = weight },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32,Value = updateBy }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        #endregion

        #region ISortingPackingDAL 成员

        /// <summary>
        /// 根据箱号取得运单主对象
        /// </summary>
        /// <param name="boxNo">箱号</param>
        /// <returns></returns>
        public List<BillModel> GetBillModelByBoxNo(string boxNo)
        {
            if (String.IsNullOrWhiteSpace(boxNo)) throw new ArgumentNullException("boxNo is null or empty");
            String strSql = @"
SELECT bill.FormCode, bill.Status, bill.DeliverStationID
FROM   SC_Bill bill 
WHERE EXISTS
(
    SELECT 1
    FROM SC_InboundPackingDetail ipd
    WHERE  ipd.BoxNo = :BoxNo AND ipd.FormCode = bill.FormCode AND  ipd.IsDeleted = 0
)
AND bill.IsDeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            { 
                new OracleParameter(){ParameterName = "BoxNo" , OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = boxNo}
            };
            return (List<BillModel>)ExecuteSql_ByDataTableReflect<BillModel>(TMSWriteConnection, strSql, parameters);
        }

        #endregion

        public string GetBoxNoByFormcode(string formcode)
        {
            string strSql = @"
                SELECT BoxNo
                FROM   SC_InboundPackingDetail ipd
                WHERE  FormCode =:formcode
                    AND IsDeleted=0";
            OracleParameter[] parameters = new OracleParameter[] { 
                new OracleParameter(){ParameterName = "formcode" ,DbType = DbType.String , Value = formcode}
            };
            DataTable dt = ExecuteSqlDataTable(TMSWriteConnection, strSql, parameters);
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            if (dt.Rows.Count >= 2)
            {
                throw new Exception("一个运单号对应多个箱号");
            }

            return Convert.ToString(dt.Rows[0]["BoxNo"]);
        }
    }
}
