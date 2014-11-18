using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Report.ComplexReport;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Report.ComplexReport;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Report.ComplexReport
{
    /// <summary>
    /// 综合报表
    /// </summary>
    public class ComplexReportDAL : BaseDAL, IComplexReportDAL
    {
        /// <summary>
        /// 无法识别的单子
        /// </summary>
        String CannotRecognizesql = @"
 SELECT 
        NULL AS DepartureTime,NULL AS DeliveryNo ,NULL AS CustWaybillNo
        ,MAX(ecd.CompanyName) AS DepartureName,MAX(ct.CityName) AS ArrivalCity,MAX(eca.CompanyName) AS ArrivalName            
        ,NULL AS CarrierName ,NULL AS TransportType,NULL AS ArrivalTiming
        ,MAX(b.ContentType) AS Goodstype,COUNT(1) AS BoxCount,SUM(b.TotalCount) AS OrderCount
        ,SUM(b.TotalAmount) AS TotalAmount,SUM(b.Weight) AS TotalWeight
        ,{7}  AS  DeliveryStatus  ,NULL AS Confirmexparrivaldate,NULL AS Desreceivedate ,NULL AS Signeduser
        ,NULL AS IsDelay   ,NULL AS Delaytimespan ,NULL AS Delaytype ,NULL AS Delayreason,NULL  AS LostType
    FROM TMS_Box b
    JOIN EXPRESSCOMPANY ecd   ON b.DEPARTUREID = ecd.EXPRESSCOMPANYID
    JOIN EXPRESSCOMPANY eca   ON b.ARRIVALID = eca.EXPRESSCOMPANYID 
    JOIN City  ct ON ct.CityID = eca.CityID
    WHERE b.IsDeleted=0
        {5}       
        AND EXISTS(
                SELECT 1
                FROM TMS_BoxDetail bd
                JOIN TMS_Order o ON o.IsDeleted = 0   AND o.OrderTMSStatus = {0}  AND o.FormCode = bd.FormCode
                WHERE bd.BoxNo=b.BoxNo  {6}
        )
        AND NOT EXISTS (
                SELECT 1
                FROM TMS_PreDispatch pd
                JOIN TMS_LinePlan lp  ON lp.IsDeleted=0  AND lp.Status={1}  AND lp.LPID=pd.LPID  AND lp.IsEnabled=1
                JOIN TMS_CARRIER c   ON c.CarrierID=lp.CarrierID  AND c.IsDeleted=0  AND c.Status={2}
                WHERE pd.IsDeleted=0   AND pd.BoxNo=b.BoxNo    AND pd.DispatchStatus={3}
                UNION ALL
                SELECT 1
                FROM TMS_DispatchDetail dd
                JOIN TMS_Dispatch d    ON d.IsDeleted=0    AND d.DID=dd.DID    AND d.DeliveryStatus IN ({4})
                WHERE dd.IsDeleted=0   AND dd.BoxNo=b.BoxNo
                        )
    GROUP BY b.DepartureID,b.ArrivalID,b.ContentType
";



        /// <summary>
        /// 待调度的单子
        /// </summary>
        String PreDispatchsql = @"
SELECT
                    NULL AS DepartureTime,NULL AS DeliveryNo ,NULL AS CustWaybillNo
                   ,max(DepartureName) AS DepartureName,MAX(ArrivalCity) AS ArrivalCity,MAX(ArrivalName) AS ArrivalName            
                   ,MAX(CarrierName) AS CarrierName ,MAX(TransportType) AS TransportType,MAX(ArrivalTiming) AS ArrivalTiming
                   ,MAX(Goodstype) AS Goodstype,SUM(BoxCount) AS BoxCount,SUM(OrderCount) AS OrderCount
                   ,SUM(TotalAmount) AS TotalAmount,SUM(TotalWeight) AS TotalWeight
                   ,{6} AS DeliveryStatus, MAX(Confirmexparrivaldate) AS Confirmexparrivaldate,NULL AS Desreceivedate ,NULL AS Signeduser
                   ,NULL AS IsDelay ,NULL AS Delaytimespan ,NULL AS Delaytype ,NULL AS Delayreason,NULL  AS LostType
           FROM
           (          
                    SELECT 
                             DepartureName ,ArrivalCity ,ArrivalName ,CarrierName ,TransportType ,ArrivalTiming ,Confirmexparrivaldate ,LPID ,LineGoodsType  AS Goodstype
                           ,(
                                SELECT COUNT(1) CC
                                FROM TMS_Box b
                                WHERE b.IsDeleted=0 AND b.BoxNo=predisp.BoxNo
                            ) AS BoxCount
                            ,(
                                SELECT COUNT(1) CC
                                FROM TMS_BoxDetail bd
                                JOIN TMS_Order o  ON o.IsDeleted=0  AND o.FormCode=bd.FormCode   AND o.OrderTMSStatus= {0}
                                WHERE bd.BoxNo=predisp.BoxNo
                            ) AS OrderCount
                            ,(
                                SELECT SUM(o.Price) Amount
                                FROM TMS_BoxDetail bd
                                JOIN TMS_Order o  ON o.IsDeleted=0  AND o.FormCode=bd.FormCode  AND o.OrderTMSStatus= {0}
                                WHERE bd.BoxNo=predisp.BoxNo
                            )  AS TotalAmount
                            ,(
                                SELECT SUM(b.WEIGHT) CC
                                FROM TMS_Box b
                                WHERE b.IsDeleted=0 AND b.BoxNo=predisp.BoxNo
                            ) AS  TotalWeight
                    FROM
                    (     
                        --从预调度表中查询承运商线路启用并且都生效的
                        SELECT ecd.CompanyName AS DepartureName ,eca.CompanyName AS ArrivalName ,ct.CityName AS ArrivalCity
                             ,pd.LPID,pd.BoxNo ,c.CarrierName,lp.TransportType ,ArrivalTiming ,pd.LineGoodsType
                            ,sysdate+ArrivalTiming/24 AS ConfirmExpArrivalDate
                        FROM TMS_PreDispatch pd
                        JOIN TMS_LinePlan lp   ON lp.IsDeleted=0  AND lp.Status={1} AND lp.LPID = pd.LPID     AND lp.IsEnabled=1
                        JOIN EXPRESSCOMPANY ecd  ON pd.DepartureID = ecd.EXPRESSCOMPANYID
                        JOIN EXPRESSCOMPANY eca  ON pd.ArrivalID = eca.EXPRESSCOMPANYID
                        JOIN City ct  ON ct.CityID = eca.CityID
                        JOIN TMS_CARRIER c   ON c.CarrierID=lp.CarrierID   AND c.IsDeleted=0   AND c.Status={2}
                        WHERE pd.IsDeleted=0  AND pd.DispatchStatus={3}
                        --其他筛选信息
                        {4}
                    )  predisp
                        --预计到货时间筛选
                        {5}
             ) t
             GROUP BY t.LPID
";

        /// <summary>
        /// 报表通用PL-SQL
        /// </summary>
        String commonSql = @"
SELECT 
siteass.createtime AS DepartureTime
,disp.deliveryno AS DeliveryNo
,custWaybill.Waybillno AS CustWaybillNo
,ecd.companyname AS DepartureName
,ct.cityname AS ArrivalCity
,eca.companyname AS ArrivalName
,carr.carriername AS CarrierName
,disp.TransportType AS TransportType
,disp.ArrivalTiming AS ArrivalTiming
,disp.Linegoodstype AS Goodstype
,disp.boxcount AS BoxCount
,disp.TotalAmount AS TotalAmount
,(
  SELECT SUM(ordercount) 
  FROM tms_dispatchdetail dispdetail
  WHERE disp.did = dispdetail.did AND dispdetail.Isdeleted = 0
) AS OrderCount
,(
    SELECT SUM(weight) 
    FROM  TMS_BOX box
          JOIN    tms_dispatchdetail dispdetail ON box.boxno = dispdetail.boxno    
    WHERE disp.did = dispdetail.did AND box.isdeleted = 0
) AS TotalWeight
,disp.deliverystatus AS DeliveryStatus
,disp.confirmexparrivaldate AS Confirmexparrivaldate
,disp.desreceivedate AS Desreceivedate
,disp.signeduser AS Signeduser
,disp.IsDelay 
,CASE WHEN disp.IsDelay = 1 THEN del.delaytimespan ELSE NULL END AS Delaytimespan
,CASE WHEN disp.IsDelay = 1 THEN del.delaytype ELSE NULL END AS Delaytype
,CASE WHEN disp.IsDelay = 1 THEN del.delayreason ELSE NULL END AS Delayreason
,NULL  AS LostType
FROM TMS_DISPATCH disp
    JOIN TMS_CARRIERWAYBILL custWaybill ON disp.carrierwaybillid = custWaybill.Cwid
    JOIN EXPRESSCOMPANY ecd   ON disp.DepartureID = ecd.EXPRESSCOMPANYID
    JOIN EXPRESSCOMPANY eca    ON disp.ArrivalID = eca.EXPRESSCOMPANYID
    JOIN City ct    ON ct.CityID = eca.CityID
    JOIN TMS_CARRIER carr  ON carr.CarrierID = disp.CarrierID  
     LEFT JOIN TMS_SITEASSESSMENT siteass ON disp.deliveryno = siteass.deliveryno
     LEFT JOIN tms_delay del ON disp.deliveryno = del.deliveryno AND del.Isdeleted = 0     
WHERE  disp.Isdeleted = 0
";

        /// <summary>
        /// 验证是否使用单号进行检索(提货单号，物流单号)
        /// 使用单号检索，忽略其他非单号的检索条件
        /// </summary>
        /// <param name="searchModel">检索条件对象</param>
        /// <returns></returns>
        private bool IsQueryByDispatchedCode(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            if (!String.IsNullOrWhiteSpace(searchModel.DeliveryNo))
            {
                return true;
            }
            if (!String.IsNullOrWhiteSpace(searchModel.CustWaybillNo))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证是否使用单号进行检索(箱号和运单号)
        /// 使用单号检索，忽略其他非单号的检索条件
        /// </summary>
        /// <param name="searchModel">检索条件对象</param>
        /// <returns></returns>
        private bool IsQueryByCommonCode(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            if (!String.IsNullOrWhiteSpace(searchModel.BoxNo))
            {
                return true;
            }
            if (!String.IsNullOrWhiteSpace(searchModel.FormCode))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证是否使用单号进行检索
        /// 使用单号检索，忽略其他非单号的检索条件
        /// </summary>
        /// <param name="searchModel">检索条件对象</param>
        /// <returns></returns>
        private bool IsQueryByCode(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            if (IsQueryByCommonCode(searchModel))
            {
                return true;
            }
            if (IsQueryByDispatchedCode(searchModel))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 构建无法识别状态的PL-SQL
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        private String CreateCannotRecognizePLSQL(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            //待调度仅根据出发地,目的地,箱号,运单号做检索,忽略其他条件
            StringBuilder cannotRecognizeSql = new StringBuilder();
            //运单号条件
            String waybillSql = "";
            //其他条件
            String otherConditionSql = "";
            if (IsQueryByCommonCode(searchModel))
            {
                if (!String.IsNullOrWhiteSpace(searchModel.BoxNo))
                {
                    otherConditionSql += @"
AND b.boxno = :BoxNo ";
                }
                if (!String.IsNullOrWhiteSpace(searchModel.FormCode))
                {
                    waybillSql += @"
AND o.FormCode = :FormCode";
                }

                cannotRecognizeSql.AppendFormat(this.CannotRecognizesql
    , (int)Enums.OrderTMSStatus.Normal
    , (int)Enums.LineStatus.Effective
    , (int)Enums.CarrierStatus.Valid
    , (int)Enums.DispatchStatus.CanDispatch
    , string.Join(",", base.GetDeliveryNotFinishStatus())
    , otherConditionSql
    , waybillSql
    , (int)Enums.ComplexReportDeliveryStatus.CannotRecognize
    );
                return cannotRecognizeSql.ToString();
            }
            //出发地,目的地检索

            if (searchModel.DepartureID > 0)
            {
                otherConditionSql += @"
AND b.departureid = :DepartureID
";
            }
            if (searchModel.ArrivalID.HasValue)
            {
                otherConditionSql += @"
AND b.arrivalid = :ArrivalID
";
            }
            cannotRecognizeSql.AppendFormat(this.CannotRecognizesql
    , (int)Enums.OrderTMSStatus.Normal
    , (int)Enums.LineStatus.Effective
    , (int)Enums.CarrierStatus.Valid
    , (int)Enums.DispatchStatus.CanDispatch
    , string.Join(",", base.GetDeliveryNotFinishStatus())
    , otherConditionSql
    , waybillSql
    , (int)Enums.ComplexReportDeliveryStatus.CannotRecognize
    );
            return cannotRecognizeSql.ToString();
        }

        /// <summary>
        /// 构建待调度状态的PL-SQL
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        private String CreatePreDispatchPLSQL(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            //待调度仅根据延误时间,出发地,目的地,箱号,运单号,承运商做检索,忽略其他条件
            StringBuilder preDispatchSql = new StringBuilder();
            //预计到货时间条件
            String exparrivalTimeSql = "";
            //其他条件
            String otherConditionSql = "";
            if (IsQueryByCommonCode(searchModel))
            {
                if (!String.IsNullOrWhiteSpace(searchModel.BoxNo))
                {
                    otherConditionSql += @"
AND pd.boxno = :BoxNo ";
                }
                if (!String.IsNullOrWhiteSpace(searchModel.FormCode))
                {
                    otherConditionSql += @"
AND   EXISTS
        (
            SELECT *
                FROM tms_box box 
                    JOIN tms_boxdetail  boxdetail ON box.boxno = boxdetail.boxno
                    JOIN TMS_ORDER ord ON boxdetail.formcode = ord.formcode
            WHERE box.boxno = pd.boxno AND box.departureid = pd.departureid AND box.arrivalid = pd.arrivalid
                        AND  boxdetail.isdeleted = 0 AND ord.isdeleted = 0 AND box.Isdeleted = 0
                        AND ord.formcode = :FormCode
        )
";
                }

                preDispatchSql.AppendFormat(this.PreDispatchsql
    , (int)Enums.OrderTMSStatus.Normal
    , (int)Enums.LineStatus.Effective
    , (int)Enums.CarrierStatus.Valid
    , (int)Enums.DispatchStatus.CanDispatch
    , otherConditionSql
    , exparrivalTimeSql
    ,(int)Enums.ComplexReportDeliveryStatus.PreDispatched
    );
                return preDispatchSql.ToString();
            }
            //不用单号用其他条件检索
            if (searchModel.ExparrivalStartTime.HasValue
                || searchModel.ExparrivalEndTime.HasValue
            )
            {
                exparrivalTimeSql += " WHERE 1=1 ";
                if (searchModel.ExparrivalStartTime.HasValue)
                {
                    exparrivalTimeSql += @" AND predisp.ConfirmExpArrivalDate >= :ExparrivalStartTime ";
                }
                if (searchModel.ExparrivalEndTime.HasValue)
                {
                    exparrivalTimeSql += " AND predisp.confirmexparrivaldate <= :ExparrivalEndTime ";
                }
            }
            if (searchModel.DepartureID > 0)
            {
                otherConditionSql += @"
AND pd.departureid = :DepartureID
";
            }
            if (searchModel.ArrivalID.HasValue)
            {
                otherConditionSql += @"
AND pd.arrivalid = :ArrivalID
";
            }
            if (!String.IsNullOrWhiteSpace(searchModel.CarrierID))
            {
                otherConditionSql += @"
AND c.CarrierID = :CarrierID
";
            }
            preDispatchSql.AppendFormat(this.PreDispatchsql
                , (int)Enums.OrderTMSStatus.Normal
                , (int)Enums.LineStatus.Effective
                , (int)Enums.CarrierStatus.Valid
                , (int)Enums.DispatchStatus.CanDispatch
                , otherConditionSql
                , exparrivalTimeSql
                , (int)Enums.ComplexReportDeliveryStatus.PreDispatched
                );
            return preDispatchSql.ToString();
        }

        /// <summary>
        /// 构建调度状态以及之后状态的PL-SQL
        /// </summary>
        /// <param name="searchModel">检索对象</param>
        /// <returns></returns>
        private String CreateAfterDispatchedPLSQL(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            StringBuilder AfterDispatchedSql = new StringBuilder();
            AfterDispatchedSql.Append(commonSql);
            if (IsQueryByCode(searchModel))
            {
                if (!String.IsNullOrWhiteSpace(searchModel.DeliveryNo))
                {
                    AfterDispatchedSql.Append(@"
--提货单号(左边模糊查询,忽略非单号的检索条件)
AND disp.deliveryno LIKE :DeliveryNo || '%' 
");
                }
                if (!String.IsNullOrWhiteSpace(searchModel.CustWaybillNo))
                {
                    AfterDispatchedSql.Append(@"
--物流运单号(忽略非单号的检索条件)
AND custWaybill.Waybillno =  :CustWaybillNo
");
                }
                if (!String.IsNullOrWhiteSpace(searchModel.BoxNo))
                {
                    AfterDispatchedSql.Append(@"
--箱号(忽略非单号的检索条件)
AND EXISTS
  (
          SELECT *
          FROM TMS_DISPATCHDETAIL dispdetail
          WHERE disp.did = dispdetail.did AND dispdetail.Isdeleted = 0
          AND dispdetail.boxno = :BoxNo
  )
");
                }
                if (!String.IsNullOrWhiteSpace(searchModel.FormCode))
                {
                    AfterDispatchedSql.Append(@"
--LMS运单号(忽略非单号的检索条件)
AND EXISTS
  (
          SELECT *
          FROM TMS_DISPATCHDETAIL dispdetail
               JOIN TMS_DISPORDERDETAIL disporder ON dispdetail.ddid = disporder.ddid
               JOIN TMS_ORDER ord ON disporder.Formcode = ord.formcode
          WHERE disp.did = dispdetail.did AND dispdetail.Isdeleted = 0
          AND ord.FormCode = :FormCode
  )
");
                }
                return AfterDispatchedSql.ToString();
            }
            if (searchModel.DepartureStartTime.HasValue)
            {
                AfterDispatchedSql.Append(@"
AND siteass.createtime >= :DepartureStartTime
                ");
            }
            if (searchModel.DesreceiveEndTime.HasValue)
            {
                AfterDispatchedSql.Append(@"
AND siteass.createtime <= :DesreceiveEndTime
                ");
            }
            if (searchModel.ExparrivalStartTime.HasValue)
            {
                AfterDispatchedSql.Append(@"
AND disp.confirmexparrivaldate >= :ExparrivalStartTime
");
            }
            if (searchModel.ExparrivalEndTime.HasValue)
            {
                AfterDispatchedSql.Append(@"
AND disp.confirmexparrivaldate <= :ExparrivalEndTime
");
            }
            if (searchModel.DesreceiveStartTime.HasValue)
            {
                AfterDispatchedSql.Append(@"
AND disp.desreceivedate >= :DesreceiveStartTime
");
            }
            if (searchModel.DesreceiveEndTime.HasValue)
            {
                AfterDispatchedSql.Append(@"
AND disp.desreceivedate <= :DesreceiveEndTime
");
            }
            if (searchModel.DepartureID > 0)
            {
                AfterDispatchedSql.Append(@"
AND disp.departureid = :DepartureID
");
            }
            if (searchModel.ArrivalID.HasValue)
            {
                AfterDispatchedSql.Append(@"
AND disp.arrivalid = :ArrivalID
");
            }
            if (!String.IsNullOrWhiteSpace(searchModel.CarrierID))
            {
                AfterDispatchedSql.Append(@"
AND disp.carrierid = :CarrierID
");
            }
            if (searchModel.Status.HasValue)
            {
                AfterDispatchedSql.Append(@"
AND disp.deliverystatus = :Status
");
            }
            if (searchModel.IsDelay.HasValue)
            {
                if (searchModel.IsDelay.Value)
                {
                    AfterDispatchedSql.Append(@"
AND  disp.IsDelay = 1
 ");
                    if (searchModel.DelaytimeStart.HasValue)
                    {
                        AfterDispatchedSql.Append(@"
AND del.delaytimespan  >= :DelaytimeStart
");
                    }
                    if (searchModel.DelaytimeEnd.HasValue)
                    {
                        AfterDispatchedSql.Append(@"
AND del.delaytimespan  <= :DelaytimeEnd
");
                    }
                }
                else
                {
                    AfterDispatchedSql.Append(@"
AND disp.IsDelay = 0
");

                }
            }
            return AfterDispatchedSql.ToString();
        }


        /// <summary>
        /// 创建报表相关SQL【扩展，按照提货单进行流转】
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private String CreateReportPLSQLEx(ComplexReportSearchModel searchModel)
        {
            String sql = @"
SELECT 
siteass.Leavetime AS DepartureTime
,disp.deliveryno AS DeliveryNo
,custWaybill.Waybillno AS CustWaybillNo
,ecd.companyname AS DepartureName
,ct.cityname AS ArrivalCity
,eca.companyname AS ArrivalName
,carr.carriername AS CarrierName
,disp.TransportType AS TransportType
,disp.ArrivalTiming AS ArrivalTiming
,disp.Linegoodstype AS Goodstype
,disp.boxcount AS BoxCount
,disp.TotalAmount AS TotalAmount
,disp.ProtectedPrice
,custWaybill.TotalCount AS OrderCount
,custWaybill.Weight AS TotalWeight
,disp.deliverystatus AS DeliveryStatus
,disp.confirmexparrivaldate AS Confirmexparrivaldate
,disp.ExpectArrivalDate AS ExpectArrivalDate
,disp.desreceivedate AS Desreceivedate
,disp.signeduser AS Signeduser
,disp.IsDelay 
,CASE WHEN disp.IsDelay = 1 THEN del.delaytimespan ELSE NULL END AS Delaytimespan
,CASE WHEN disp.IsDelay = 1 THEN del.delaytype ELSE NULL END AS Delaytype
,CASE WHEN disp.IsDelay = 1 THEN del.delayreason ELSE NULL END AS Delayreason
,NULL  AS LostType
FROM TMS_DISPATCH disp
    JOIN TMS_CARRIERWAYBILL custWaybill ON disp.carrierwaybillid = custWaybill.Cwid
    JOIN EXPRESSCOMPANY ecd   ON disp.DepartureID = ecd.EXPRESSCOMPANYID
    JOIN EXPRESSCOMPANY eca    ON disp.ArrivalID = eca.EXPRESSCOMPANYID
    JOIN City ct    ON ct.CityID = eca.CityID
    JOIN TMS_CARRIER carr  ON carr.CarrierID = disp.CarrierID  
    LEFT JOIN TMS_SITEASSESSMENT siteass ON disp.deliveryno = siteass.deliveryno
    LEFT JOIN tms_delay del ON disp.deliveryno = del.deliveryno AND del.Isdeleted = 0     
WHERE  disp.Isdeleted = 0
";
            StringBuilder resultSql = new StringBuilder(sql);
            if (!String.IsNullOrWhiteSpace(searchModel.DeliveryNo))
            {
                resultSql.Append(@"
--提货单号(左边模糊查询,忽略非单号的检索条件)
AND disp.deliveryno LIKE :DeliveryNo || '%' 
");
            }
            if (!String.IsNullOrWhiteSpace(searchModel.CustWaybillNo))
            {
                resultSql.Append(@"
--物流运单号(忽略非单号的检索条件)
AND custWaybill.Waybillno =  :CustWaybillNo
");
            }
            if (searchModel.DepartureStartTime.HasValue)
            {
                resultSql.Append(@"
AND siteass.Leavetime >= :DepartureStartTime
                ");
            }
            if (searchModel.DepartureEndTime.HasValue)
            {
                resultSql.Append(@"
AND siteass.Leavetime <= :DepartureEndTime
                ");
            }
            if (searchModel.ExparrivalStartTime.HasValue)
            {
                resultSql.Append(@"
AND disp.confirmexparrivaldate >= :ExparrivalStartTime
");
            }
            if (searchModel.ExparrivalEndTime.HasValue)
            {
                resultSql.Append(@"
AND disp.confirmexparrivaldate <= :ExparrivalEndTime
");
            }
            if (searchModel.DesreceiveStartTime.HasValue)
            {
                resultSql.Append(@"
AND disp.desreceivedate >= :DesreceiveStartTime
");
            }
            if (searchModel.DesreceiveEndTime.HasValue)
            {
                resultSql.Append(@"
AND disp.desreceivedate <= :DesreceiveEndTime
");
            }
            if (searchModel.DepartureID > 0)
            {
                resultSql.Append(@"
AND disp.departureid = :DepartureID
");
            }
            if (searchModel.ArrivalID.HasValue)
            {
                resultSql.Append(@"
AND disp.arrivalid = :ArrivalID
");
            }
            if (!String.IsNullOrWhiteSpace(searchModel.CarrierID))
            {
                resultSql.Append(@"
AND disp.carrierid = :CarrierID
");
            }
            if (searchModel.Status.HasValue)
            {
                resultSql.Append(@"
AND disp.deliverystatus = :Status
");
            }
            if (searchModel.IsDelay.HasValue)
            {
                if (searchModel.IsDelay.Value)
                {
                    resultSql.Append(@"
AND  disp.IsDelay = 1
 ");
                    if (searchModel.DelaytimeStart.HasValue)
                    {
                        resultSql.Append(@"
AND del.delaytimespan  >= :DelaytimeStart
");
                    }
                    if (searchModel.DelaytimeEnd.HasValue)
                    {
                        resultSql.Append(@"
AND del.delaytimespan  <= :DelaytimeEnd
");
                    }
                }
                else
                {
                    resultSql.Append(@"
AND disp.IsDelay = 0
");
                }
            }
            return resultSql.ToString();
        }

        /// <summary>
        /// 创建报表相关SQL
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        private String CreateReportPLSQL(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            StringBuilder resultSql = new StringBuilder();
            if (searchModel.Status.HasValue)
            {
                if (searchModel.Status.Value == Enums.ComplexReportDeliveryStatus.PreDispatched)
                {
                    return CreatePreDispatchPLSQL(searchModel);
                }
                if (searchModel.Status.Value == Enums.ComplexReportDeliveryStatus.CannotRecognize)
                {
                    return CreateCannotRecognizePLSQL(searchModel);
                }
                return CreateAfterDispatchedPLSQL(searchModel);
            }
            if (IsQueryByDispatchedCode(searchModel))
            {
                return CreateAfterDispatchedPLSQL(searchModel);
            }
            resultSql.Append(CreatePreDispatchPLSQL(searchModel));
            resultSql.Append(@"
UNION ALL
");
            resultSql.Append(CreateCannotRecognizePLSQL(searchModel));
            resultSql.Append(@"
UNION ALL
");
            resultSql.Append(CreateAfterDispatchedPLSQL(searchModel));
            return resultSql.ToString();
        }

        /// <summary>
        /// 创建PL-SQL参数【扩展，按照提货单进行流转】
        /// </summary>
        /// <param name="searchModel">检索条件对象</param>
        /// <returns></returns>
        private OracleParameter[] CreateParameterEx(ComplexReportSearchModel searchModel)
        {
            List<OracleParameter> listParameter = new List<OracleParameter>();
            if (!String.IsNullOrWhiteSpace(searchModel.DeliveryNo))
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = searchModel.DeliveryNo });
            }
            if (!String.IsNullOrWhiteSpace(searchModel.CustWaybillNo))
            {
                listParameter.Add(new OracleParameter() { ParameterName = "CustWaybillNo", DbType = DbType.String, Value = searchModel.CustWaybillNo });
            }
            if (searchModel.DepartureStartTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DepartureStartTime", DbType = DbType.DateTime, Value = searchModel.DepartureStartTime.Value });
            }
            if (searchModel.DepartureEndTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DepartureEndTime", DbType = DbType.DateTime, Value = searchModel.DepartureEndTime.Value });
            }
            if (searchModel.ExparrivalStartTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "ExparrivalStartTime", DbType = DbType.DateTime, Value = searchModel.ExparrivalStartTime.Value });
            }
            if (searchModel.ExparrivalEndTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "ExparrivalEndTime", DbType = DbType.DateTime, Value = searchModel.ExparrivalEndTime.Value });
            }
            if (searchModel.DesreceiveStartTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DesreceiveStartTime", DbType = DbType.DateTime, Value = searchModel.DesreceiveStartTime.Value });
            }
            if (searchModel.DesreceiveEndTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DesreceiveEndTime", DbType = DbType.DateTime, Value = searchModel.DesreceiveEndTime.Value });
            }
            if (searchModel.DepartureID > 0)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID });
            }
            if (searchModel.ArrivalID.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (!String.IsNullOrWhiteSpace(searchModel.CarrierID))
            {
                listParameter.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.String, Value = searchModel.CarrierID });
            }
            if (searchModel.Status.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "Status", DbType = DbType.Int32, Value = (int)searchModel.Status.Value });
            }
            if (searchModel.IsDelay.HasValue)
            {
                if (searchModel.IsDelay.Value)
                {
                    if (searchModel.DelaytimeStart.HasValue)
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "DelaytimeStart", DbType = DbType.Decimal, Value = searchModel.DelaytimeStart.Value });
                    }
                    if (searchModel.DelaytimeEnd.HasValue)
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "DelaytimeEnd", DbType = DbType.Decimal, Value = searchModel.DelaytimeEnd.Value });
                    }
                }
            }
            return listParameter.ToArray();
        }

        /// <summary>
        /// 创建PL-SQL参数
        /// </summary>
        /// <param name="searchModel">检索条件对象</param>
        /// <returns></returns>
        private OracleParameter[] CreateParameter(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            List<OracleParameter> listParameter = new List<OracleParameter>();
            if (searchModel.Status.HasValue)
            {
                if (searchModel.Status.Value == Enums.ComplexReportDeliveryStatus.CannotRecognize
                    || searchModel.Status.Value == Enums.ComplexReportDeliveryStatus.PreDispatched)
                {
                    if (IsQueryByCommonCode(searchModel))
                    {
                        if (!String.IsNullOrWhiteSpace(searchModel.BoxNo))
                        {
                            listParameter.Add(new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = searchModel.BoxNo });
                        }
                        if (!String.IsNullOrWhiteSpace(searchModel.FormCode))
                        {
                            listParameter.Add(new OracleParameter() { ParameterName = "FormCode", DbType = DbType.Int64, Value = searchModel.FormCode });
                        }
                        return listParameter.ToArray();
                    }
                    if (searchModel.DepartureID > 0)
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID });
                    }
                    if (searchModel.ArrivalID.HasValue)
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
                    }
                    ///无法识别仅能通过出发地目的地或者单号检索
                    if (searchModel.Status.Value == Enums.ComplexReportDeliveryStatus.CannotRecognize)
                    {
                        return listParameter.ToArray();
                    }
                    ///待调度支持延误时间,承运商
                    if (searchModel.ExparrivalStartTime.HasValue)
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "ExparrivalStartTime", DbType = DbType.DateTime, Value = searchModel.ExparrivalStartTime.Value });
                    }
                    if (searchModel.ExparrivalEndTime.HasValue)
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "ExparrivalEndTime", DbType = DbType.DateTime, Value = searchModel.ExparrivalEndTime.Value });
                    }
                    if (!String.IsNullOrWhiteSpace(searchModel.CarrierID))
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.String, Value = searchModel.CarrierID });
                    }
                    return listParameter.ToArray();
                }
            }
            if (IsQueryByCode(searchModel))
            {
                if (!String.IsNullOrWhiteSpace(searchModel.DeliveryNo))
                {
                    listParameter.Add(new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = searchModel.DeliveryNo });
                }
                if (!String.IsNullOrWhiteSpace(searchModel.CustWaybillNo))
                {
                    listParameter.Add(new OracleParameter() { ParameterName = "CustWaybillNo", DbType = DbType.String, Value = searchModel.CustWaybillNo });
                }
                if (!String.IsNullOrWhiteSpace(searchModel.BoxNo))
                {
                    listParameter.Add(new OracleParameter() { ParameterName = "BoxNo", DbType = DbType.String, Value = searchModel.BoxNo });
                }
                if (!String.IsNullOrWhiteSpace(searchModel.FormCode))
                {
                    listParameter.Add(new OracleParameter() { ParameterName = "FormCode", DbType = DbType.Int64, Value = searchModel.FormCode });
                }
                return listParameter.ToArray();
            }
            if (searchModel.DepartureStartTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DepartureStartTime", DbType = DbType.DateTime, Value = searchModel.DepartureStartTime.Value });
            }
            if (searchModel.DesreceiveEndTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DesreceiveEndTime", DbType = DbType.DateTime, Value = searchModel.DesreceiveEndTime.Value });
            }
            if (searchModel.ExparrivalStartTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "ExparrivalStartTime", DbType = DbType.DateTime, Value = searchModel.ExparrivalStartTime.Value });
            }
            if (searchModel.ExparrivalEndTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "ExparrivalEndTime", DbType = DbType.DateTime, Value = searchModel.ExparrivalEndTime.Value });
            }
            if (searchModel.DesreceiveStartTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DesreceiveStartTime", DbType = DbType.DateTime, Value = searchModel.DesreceiveStartTime.Value });
            }
            if (searchModel.DesreceiveEndTime.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DesreceiveEndTime", DbType = DbType.DateTime, Value = searchModel.DesreceiveEndTime.Value });
            }
            if (searchModel.DepartureID > 0)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "DepartureID", DbType = DbType.Int32, Value = searchModel.DepartureID });
            }
            if (searchModel.ArrivalID.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "ArrivalID", DbType = DbType.Int32, Value = searchModel.ArrivalID.Value });
            }
            if (!String.IsNullOrWhiteSpace(searchModel.CarrierID))
            {
                listParameter.Add(new OracleParameter() { ParameterName = "CarrierID", DbType = DbType.String, Value = searchModel.CarrierID });
            }
            if (searchModel.Status.HasValue)
            {
                listParameter.Add(new OracleParameter() { ParameterName = "Status", DbType = DbType.Int32, Value = (int)searchModel.Status.Value });
            }
            if (searchModel.IsDelay.HasValue)
            {
                if (searchModel.IsDelay.Value)
                {
                    if (searchModel.DelaytimeStart.HasValue)
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "DelaytimeStart", DbType = DbType.Decimal, Value = searchModel.DelaytimeStart.Value });
                    }
                    if (searchModel.DelaytimeEnd.HasValue)
                    {
                        listParameter.Add(new OracleParameter() { ParameterName = "DelaytimeEnd", DbType = DbType.Decimal, Value = searchModel.DelaytimeEnd.Value });
                    }
                }
            }
            return listParameter.ToArray();
        }

        #region IComplexReportDAL 成员

        /// <summary>
        /// 综合报表查询
        /// </summary>
        /// <param name="searchModel">检索</param>
        /// <returns></returns>
        public Util.Pager.PagedList<Model.Report.ComplexReport.ViewComplexReport> Search(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("ComplexReportSearchModel is null");
            searchModel.OrderByString = "DeliveryNo";
            //return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewComplexReport>(TMSReadOnlyConnection, CreateReportPLSQL(searchModel), searchModel, CreateParameter(searchModel));
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewComplexReport>(TMSReadOnlyConnection, CreateReportPLSQLEx(searchModel), searchModel, CreateParameterEx(searchModel));
        }

        /// <summary>
        /// 导出报表
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public List<Model.Report.ComplexReport.ViewComplexReport> Export(Model.Report.ComplexReport.ComplexReportSearchModel searchModel)
        {
            if (searchModel == null) throw new ArgumentNullException("ComplexReportSearchModel is null");
            //var listResult = ExecuteSql_ByReaderReflect<ViewComplexReport>(TMSReadOnlyConnection, CreateReportPLSQL(searchModel), CreateParameter(searchModel));
            var listResult = ExecuteSql_ByReaderReflect<ViewComplexReport>(TMSReadOnlyConnection, CreateReportPLSQLEx(searchModel), CreateParameterEx(searchModel));
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        #endregion
    }
}
