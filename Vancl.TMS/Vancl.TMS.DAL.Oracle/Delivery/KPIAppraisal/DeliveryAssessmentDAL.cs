using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Delivery.KPIAppraisal;
using Vancl.TMS.Model.Delivery.KPIAppraisal;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Util.Exceptions;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Delivery.KPIAppraisal
{
    public class DeliveryAssessmentDAL : BaseDAL, IDeliveryAssessmentDAL
    {
        #region IDeliveryAssessmentDAL 成员

        public int Add(DeliveryAssessmentModel model)
        {
            string strSql = @"
INSERT INTO TMS_DeliveryAssessment(
    DeliveryNo
    ,InsuranceRate
    ,InsuranceAmount
    ,BaseAmount
    ,NeedAmount
    ,LongTransferRate
    ,LongDeliveryAmount
    ,LongTransferAmount
    ,LongPickPrice
    ,ComplementAmount
    ,IsDelayAssess
    ,Discount
    ,ApprovedAmount
    ,LostDeduction
    ,ExpressionType
    ,ConfirmedAmount
    ,CreateBy
    ,UpdateBy
    ,IsDeleted
    ,KPIDelayType
    ,DelayAmount
    ,OtherAmount   
)
VALUES(
    :DeliveryNo
    ,:InsuranceRate
    ,:InsuranceAmount
    ,:BaseAmount
    ,:NeedAmount
    ,:LongTransferRate
    ,:LongDeliveryAmount
    ,:LongTransferAmount
    ,:LongPickPrice
    ,:ComplementAmount
    ,:IsDelayAssess
    ,:Discount
    ,:ApprovedAmount
    ,:LostDeduction
    ,:ExpressionType
    ,:ConfirmedAmount
    ,:CreateBy
    ,:UpdateBy
    ,0
    ,:KPIDelayType
    ,:DelayAmount
    ,:OtherAmount   
)
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo},
                new OracleParameter() { ParameterName="InsuranceRate",DbType= DbType.Decimal,Value=model.InsuranceRate},
                new OracleParameter() { ParameterName="InsuranceAmount",DbType= DbType.Decimal,Value=model.InsuranceAmount},
                new OracleParameter() { ParameterName="BaseAmount",DbType= DbType.Decimal,Value=model.BaseAmount},
                new OracleParameter() { ParameterName="NeedAmount",DbType= DbType.Decimal,Value=model.NeedAmount},
                new OracleParameter() { ParameterName="LongTransferRate",DbType= DbType.Decimal,Value=model.LongTransferRate},
                new OracleParameter() { ParameterName="LongDeliveryAmount",DbType= DbType.Decimal,Value=model.LongDeliveryAmount},
                new OracleParameter() { ParameterName="LongTransferAmount",DbType= DbType.Decimal,Value=model.LongTransferAmount},
                new OracleParameter() { ParameterName="LongPickPrice",DbType= DbType.Decimal,Value=model.LongPickPrice},
                new OracleParameter() { ParameterName="ComplementAmount",DbType= DbType.Decimal,Value=model.ComplementAmount},
                new OracleParameter() { ParameterName="IsDelayAssess",DbType= DbType.Byte,Value=model.IsDelayAssess},
                new OracleParameter() { ParameterName="Discount",DbType= DbType.Decimal,Value=model.Discount},
                new OracleParameter() { ParameterName="ApprovedAmount",DbType= DbType.Decimal,Value=model.ApprovedAmount},
                new OracleParameter() { ParameterName="LostDeduction",DbType= DbType.Decimal,Value=model.LostDeduction},
                new OracleParameter() { ParameterName="KPIDelayType",DbType= DbType.Int16,Value = (int)model.KPIDelayType },
                new OracleParameter() { ParameterName="DelayAmount",DbType= DbType.Decimal,Value = model.DelayAmount },
                new OracleParameter() { ParameterName="OtherAmount",DbType= DbType.Decimal,Value = model.OtherAmount },
                new OracleParameter() { ParameterName="ExpressionType",DbType= DbType.Int32,Value=(int)model.ExpressionType},
                new OracleParameter() { ParameterName="ConfirmedAmount", DbType= System.Data.DbType.Decimal, Value = model.ConfirmedAmount},
                new OracleParameter() { ParameterName="CreateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Update(DeliveryAssessmentModel model)
        {
            string strSql = @"
UPDATE TMS_DeliveryAssessment
SET InsuranceRate=:InsuranceRate
    ,InsuranceAmount=:InsuranceAmount
    ,BaseAmount=:BaseAmount
    ,NeedAmount=:NeedAmount
    ,LongTransferRate=:LongTransferRate
    ,LongDeliveryAmount=:LongDeliveryAmount
    ,LongTransferAmount=:LongTransferAmount
    ,LongPickPrice=:LongPickPrice
    ,ComplementAmount=:ComplementAmount
    ,IsDelayAssess=:IsDelayAssess
    ,Discount=:Discount
    ,ApprovedAmount=:ApprovedAmount
    ,LostDeduction=:LostDeduction
    ,ExpressionType=:ExpressionType
    ,ConfirmedAmount = :ConfirmedAmount
    ,UpdateTime=sysdate
    ,UpdateBy=:UpdateBy
    ,KPIDelayType = :KPIDelayType
    ,DelayAmount = :DelayAmount
    ,OtherAmount = :OtherAmount  
WHERE DeliveryNo=:DeliveryNo
    AND IsDeleted=0
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="InsuranceRate",DbType= DbType.Decimal,Value=model.InsuranceRate},
                new OracleParameter() { ParameterName="InsuranceAmount",DbType= DbType.Decimal,Value=model.InsuranceAmount},
                new OracleParameter() { ParameterName="BaseAmount",DbType= DbType.Decimal,Value=model.BaseAmount},
                new OracleParameter() { ParameterName="NeedAmount",DbType= DbType.Decimal,Value=model.NeedAmount},
                new OracleParameter() { ParameterName="LongTransferRate",DbType= DbType.Decimal,Value=model.LongTransferRate},
                new OracleParameter() { ParameterName="LongDeliveryAmount",DbType= DbType.Decimal,Value=model.LongDeliveryAmount},
                new OracleParameter() { ParameterName="LongTransferAmount",DbType= DbType.Decimal,Value=model.LongTransferAmount},
                new OracleParameter() { ParameterName="LongPickPrice",DbType= DbType.Decimal,Value=model.LongPickPrice},
                new OracleParameter() { ParameterName="ComplementAmount",DbType= DbType.Decimal,Value=model.ComplementAmount},
                new OracleParameter() { ParameterName="IsDelayAssess",DbType= DbType.Byte,Value=model.IsDelayAssess},
                new OracleParameter() { ParameterName="Discount",DbType= DbType.Decimal,Value=model.Discount},
                new OracleParameter() { ParameterName="ApprovedAmount",DbType= DbType.Decimal,Value=model.ApprovedAmount},
                new OracleParameter() { ParameterName="LostDeduction",DbType= DbType.Decimal,Value=model.LostDeduction},
                new OracleParameter() { ParameterName="ExpressionType",DbType= DbType.Int32,Value=(int)model.ExpressionType},
                new OracleParameter() { ParameterName="KPIDelayType",DbType= DbType.Int16,Value = (int)model.KPIDelayType },
                new OracleParameter() { ParameterName="DelayAmount",DbType= DbType.Decimal,Value = model.DelayAmount },
                new OracleParameter() { ParameterName="OtherAmount",DbType= DbType.Decimal,Value = model.OtherAmount },
                new OracleParameter() { ParameterName="ConfirmedAmount", DbType= System.Data.DbType.Decimal, Value = model.ConfirmedAmount},
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=model.DeliveryNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public int Delete(string deliveryNo)
        {
            string strSql = @"
                UPDATE TMS_DeliveryAssessment
                SET IsDeleted=1
                    ,UpdateTime=sysdate
                    ,UpdateBy=:UpdateBy
                WHERE DeliveryNo=:DeliveryNo
                    AND IsDeleted=0";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="UpdateBy",DbType= DbType.Int32,Value=UserContext.CurrentUser.ID},
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        public bool IsExist(string deliveryNo)
        {
            string strSql = @"
                SELECT COUNT(1) CC
                FROM TMS_DeliveryAssessment
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

        public DeliveryAssessmentModel Get(string deliveryNo)
        {
            string strSql = @"
SELECT
    DeliveryNo
    ,InsuranceRate
    ,InsuranceAmount
    ,BaseAmount
    ,NeedAmount
    ,LongTransferRate
    ,LongDeliveryAmount
    ,LongTransferAmount
    ,LongPickPrice
    ,ComplementAmount
    ,IsDelayAssess
    ,Discount
    ,ApprovedAmount
    ,LostDeduction
    ,ExpressionType
    ,CreateBy
    ,CreateTime
    ,UpdateBy
    ,UpdateTime
    ,IsDeleted
    ,KPIDelayType
    ,DelayAmount
    ,OtherAmount               
FROM TMS_DeliveryAssessment
WHERE DeliveryNo=:DeliveryNo
    AND IsDeleted=0
";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value=deliveryNo}
            };
            return ExecuteSqlSingle_ByReaderReflect<DeliveryAssessmentModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        public PagedList<ViewDeliveryAssessmentModel> Search(DeliveryAssessmentSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new CodeNotValidException();
            }
            List<OracleParameter> lstParam = new List<OracleParameter>();
            string strSql = @"
SELECT siteAss.Leavetime AS DepartureTime ,d.DepartureID,ecd.CompanyName as DepartureName ,d.ArrivalID,eca.CompanyName as ArrivalName,City.CityName as ArrivalCity
    ,cw.TotalCount as OrderCount ,c.CarrierName,d.TransportType,d.ArrivalTiming,d.DeliveryNo ,cw.WaybillNo,d.LineGoodsType,d.TotalAmount,d.DeliveryStatus, d.ProtectedPrice
    ,d.ExpectArrivalDate,d.ConfirmExpArrivalDate,d.DesReceiveDate,delay.DelayTimeSpan ,delay.DelayType,delay.DelayReason
    ,(
        SELECT ApproveStatus
        FROM TMS_DelayHandle dh
        WHERE dh.DHID = 
            (
            SELECT MAX(dh1.DHID)
            FROM TMS_DelayHandle dh1
            WHERE dh1.DelayID=delay.DID
                AND dh1.IsDeleted=0
            )
    ) DelayHandleStatus
FROM TMS_Dispatch d
JOIN TMS_SITEASSESSMENT siteAss ON d.deliveryno = siteAss.Deliveryno
JOIN TMS_Carrier c  ON c.CarrierID=d.CarrierID  AND c.IsDeleted=0
JOIN ExpressCompany ecd ON ecd.ExpressCompanyID = d.DepartureID
JOIN ExpressCompany eca  ON eca.ExpressCompanyID = d.ArrivalID
JOIN City  ON City.CityID=eca.CityID
JOIN TMS_CarrierWaybill cw  ON cw.CWID=d.CarrierWaybillID  AND cw.IsDeleted=0
LEFT JOIN TMS_Delay delay ON delay.DeliveryNo=d.DeliveryNo  AND delay.IsDeleted=0
WHERE d.IsDeleted=0
";
            if (searchModel.BeginTime.HasValue)
            {
                strSql += " AND siteAss.Leavetime >= :BeginTime";
                lstParam.Add(new OracleParameter() { ParameterName = "BeginTime", OracleDbType = OracleDbType.Date, Value = searchModel.BeginTime.Value });
            }
            if (searchModel.EndTime.HasValue)
            {
                strSql += " AND siteAss.Leavetime <= :EndTime";
                lstParam.Add(new OracleParameter() { ParameterName = "EndTime", OracleDbType = OracleDbType.Date, Value = searchModel.EndTime.Value });
            }
            if (searchModel.CarrierID.HasValue)
            {
                strSql += " AND d.CarrierID=:CarrierID";
                lstParam.Add(new OracleParameter() { ParameterName = "CarrierID", OracleDbType = OracleDbType.Int32, Value = searchModel.CarrierID.Value });
            }
            if (searchModel.DepartureID.HasValue)
            {
                strSql += " AND d.DepartureID=:DepartureID";
                lstParam.Add(new OracleParameter() { ParameterName = "DepartureID", OracleDbType = OracleDbType.Int32, Value = searchModel.DepartureID.Value });
            }
            if (searchModel.DeliveryStatus.HasValue)
            {
                strSql += " AND d.DeliveryStatus=:DeliveryStatus";
                lstParam.Add(new OracleParameter() { ParameterName = "DeliveryStatus", OracleDbType = OracleDbType.Int32, Value = (int)searchModel.DeliveryStatus.Value });
            }
            else
            {
                strSql += string.Format(" AND d.DeliveryStatus IN ({0},{1},{2},{3})"
                    , (int)Enums.DeliveryStatus.ArrivedOnTime
                    , (int)Enums.DeliveryStatus.ArrivedDelay
                    , (int)Enums.DeliveryStatus.AllLost
                    , (int)Enums.DeliveryStatus.KPIApproved);
            }
            if (searchModel.DelaySpanBegin.HasValue)
            {
                strSql += " AND delay.DelayTimeSpan>=:DelaySpanBegin";
                lstParam.Add(new OracleParameter() { ParameterName = "DelaySpanBegin", OracleDbType = OracleDbType.Decimal, Value = searchModel.DelaySpanBegin.Value });
            }
            if (searchModel.DelaySpanEnd.HasValue)
            {
                strSql += " AND delay.DelayTimeSpan<=:DelaySpanEnd";
                lstParam.Add(new OracleParameter() { ParameterName = "DelaySpanEnd", OracleDbType = OracleDbType.Decimal, Value = searchModel.DelaySpanEnd.Value });
            }
            searchModel.OrderByString = "DepartureTime";
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<ViewDeliveryAssessmentModel>(TMSReadOnlyConnection, strSql, searchModel, lstParam.ToArray());
        }
        #endregion
    }
}
