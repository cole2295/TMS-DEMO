using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Common;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Sorting.Inbound
{
    /// <summary>
    /// 入库数据层
    /// </summary>
    public class InboundDAL : BaseDAL, IInboundDAL
    {

        #region IInboundDAL 成员

        /// <summary>
        /// 新增入库记录
        /// </summary>
        /// <param name="model">入库实体对象</param>
        /// <returns></returns>
        public int Add(InboundEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundEntityModel is null");
            String sql = @"
INSERT INTO SC_Inbound(IBID,FormCode,InboundType,ApplyStation,DepartureID,ArrivalID,CreateBy,UpdateBy,IsDeleted,SyncFlag)
VALUES(:IBID ,:FormCode,:InboundType,:ApplyStation,:DepartureID,:ArrivalID,:CreateBy,:UpdateBy,0,:SyncFlag)
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="IBID", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = model.IBID},
                new OracleParameter() { ParameterName="FormCode",  OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.FormCode},
                new OracleParameter() { ParameterName="InboundType", OracleDbType = OracleDbType.Int16 , Value = (int)model.InboundType},
                new OracleParameter() { ParameterName="ApplyStation", OracleDbType = OracleDbType.Int32 , Value = model.ApplyStation},
                new OracleParameter() { ParameterName="DepartureID", OracleDbType = OracleDbType.Int32 , Value = model.DepartureID},                
                new OracleParameter() { ParameterName="ArrivalID",  OracleDbType = OracleDbType.Int32 , Value = model.ArrivalID},
                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32 , Value = model.CreateBy},
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32 , Value = model.UpdateBy},
                new OracleParameter() { ParameterName="SyncFlag", OracleDbType = OracleDbType.Int16 , Value = (int)model.SyncFlag}            
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 取得处于已入库状态数量
        /// </summary>
        /// <param name="DepartureID">当前分拣中心</param>
        /// <param name="ArrivalID">目的地分检中心</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <returns></returns>
        public int GetInboundCount(int DepartureID, int ArrivalID, DateTime StartTime, DateTime EndTime)
        {
            String sql = @"
SELECT COUNT(1)
FROM (
    SELECT SIBQ.FormCode
    FROM SC_InboundQueue  SIBQ
    WHERE SIBQ.CreateTime BETWEEN  :StartTime AND :EndTime
        AND SIBQ.DepartureID=:DepartureID
        AND SIBQ.ArrivalID=:ArrivalID
        AND SIBQ.SeqStatus=:SeqStatus
        AND SIBQ.IsDeleted=0
    UNION
    SELECT SB.FormCode
    FROM SC_Inbound SIB 
            JOIN SC_Bill SB ON SB.InBoundKey = SIB.IBID  AND SB.FormCode=SIB.FormCode
    WHERE SIB.CreateTime BETWEEN  :StartTime AND :EndTime
        AND SIB.DepartureID = :DepartureID
        AND SIB.ArrivalID =:ArrivalID
        AND SB.Status = :BillStatus
        AND SB.IsDeleted = 0
        AND SIB.IsDeleted = 0
)
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                     new OracleParameter() { ParameterName="DepartureID",  OracleDbType = OracleDbType.Int32 , Value = DepartureID  },
                     new OracleParameter() { ParameterName="ArrivalID",   OracleDbType = OracleDbType.Int32 , Value = ArrivalID },
                     new OracleParameter() { ParameterName="StartTime",  OracleDbType = OracleDbType.Date , Value = StartTime },
                     new OracleParameter() { ParameterName="EndTime",  OracleDbType = OracleDbType.Date , Value = EndTime },
                     new OracleParameter() { ParameterName="SeqStatus", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
                     new OracleParameter() { ParameterName="BillStatus", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.BillStatus.HaveBeenSorting }
            };
            var data = ExecuteSqlScalar(TMSWriteConnection, sql, parameters);
            if (data != null && data != DBNull.Value)
            {
                return Convert.ToInt32(data);
            }
            return 0;
        }


        /// <summary>
        /// [Check]运单称重
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public bool ValidateBillWeight(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
SELECT 
     NVL(BI.Weight,0) AS Weight
    ,NVL(MBI.IsNeedWeight,0) AS  IsNeedWeight
FROM SC_Bill  BILL
    JOIN MerchantBaseInfo MBI ON BILL.MerchantID =  MBI.ID
    JOIN SC_BillInfo BI ON BILL.FormCode = BI.FormCode
    WHERE BILL.FormCode = :FormCode AND BILL.IsDeleted = 0    
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                    new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode  }
            };
            DataTable dtResult = ExecuteSqlDataTable(TMSWriteConnection, sql, parameters);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                DataRow dr = dtResult.Rows[0];
                //某些商家必须要称重
                if (Convert.ToInt32(dr["IsNeedWeight"]) == 1 && Convert.ToDecimal(dr["Weight"]) <= 0M)
                {
                    //check失败,必须由面单打印称重
                    return false;
                }
                return true;
            }
            return false;
        }


        /// <summary>
        /// 验证站点是否属于配送商
        /// </summary>
        /// <param name="DistributionCode">配送商</param>
        /// <param name="DeliverStationID">站点信息</param>
        /// <returns></returns>
        public bool ValidateDistributionDeliverStation(string DistributionCode, int DeliverStationID)
        {
            if (String.IsNullOrWhiteSpace(DistributionCode)) throw new ArgumentNullException("DistributionCode is null or enmty.");
            String sql = @"
SELECT COUNT(1) AS CT
FROM ExpressCompany
WHERE ExpressCompanyID = :ExpressCompanyID AND DistributionCode = :DistributionCode
    AND IsDeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                     new OracleParameter() { ParameterName="ExpressCompanyID",  OracleDbType = OracleDbType.Int32 , Value = DeliverStationID  },
                     new OracleParameter() { ParameterName="DistributionCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = DistributionCode }                       
            };
            var obj = ExecuteSqlScalar(TMSReadOnlyConnection, sql, parameters);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj) > 0;
            }
            return false;
        }

        /// <summary>
        /// 存在同最后一次入库相同出发地目的地入库记录
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <param name="DepartureID">出发地</param>
        /// <param name="ArrivalID">目的地</param>
        /// <returns></returns>
        public bool ExistsLine_EqualLastInboud(string FormCode, int DepartureID, int ArrivalID)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
SELECT COUNT(1)
FROM SC_Bill BILL
    JOIN SC_Inbound SIB  ON SIB.IBID = BILL.InBoundKey  AND BILL.FormCode = SIB.FormCode
WHERE BILL.FormCode = :FormCode
        AND SIB.DepartureID = :DepartureID
        AND SIB.ArrivalID = :ArrivalID
        AND BILL.IsDeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="FormCode" , OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode },
                new OracleParameter() { ParameterName="DepartureID" ,  OracleDbType = OracleDbType.Int32 , Value = DepartureID },
                new OracleParameter() { ParameterName="ArrivalID" ,  OracleDbType = OracleDbType.Int32 , Value = ArrivalID }
            };
            var obj = ExecuteSqlScalar(TMSWriteConnection, sql, parameters);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj) > 0;
            }
            return false;
        }

        #endregion

        #region IInboundDAL 成员


        public InboundEntityModel GetInboundEntityModel4TmsSync2Lms(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
SELECT *
FROM
(
    SELECT IBID, FormCode , InboundType, ApplyStation, DepartureID, ArrivalID, SyncFlag, CreateBy, CreateTime
    FROM SC_Inbound
    WHERE FormCode = :FormCode
    AND SyncFlag = :SyncFlag
    ORDER BY CreateTime ASC
)
WHERE ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SyncStatus.NotYet },
                new OracleParameter() { ParameterName = "FormCode" , OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode }
            };
            return ExecuteSqlSingle_ByDataTableReflect<InboundEntityModel>(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IInboundDAL 成员

        /// <summary>
        /// 更新同步状态【TMS同步会LMS主库时调用】
        /// </summary>
        /// <param name="InboundKey">入库主键key</param>
        /// <returns></returns>
        public int UpdateSyncedStatus4Tms2Lms(string InboundKey)
        {
            if (String.IsNullOrWhiteSpace(InboundKey)) throw new ArgumentNullException("InboundKey is null or empty.");
            String sql = @"
UPDATE SC_Inbound
SET SyncFlag = :SyncFlag , UpdateTime = SYSDATE 
WHERE IBID = :IBID
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" ,  OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SyncStatus.Already },
                new OracleParameter() { ParameterName = "IBID" , OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = InboundKey }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IInboundDAL 成员
        public Int32 GetPrintDept(long formCode)
        {
            String strSql = @"
SELECT e.stationid 
          FROM sc_billchangelog BCL
               JOIN Employee e on e.employeeid=BCL.CREATEBY
          WHERE ROWNUM=1 AND BCL.operatetype=0 and BCL.formcode=:FormCode
          ORDER BY BCL.Createtime desc";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "FormCode" ,  OracleDbType = OracleDbType.Decimal , Value = formCode },
            };

            var data = ExecuteSqlScalar(TMSReadOnlyConnection, strSql, arguments);
            if (data != null && data != DBNull.Value)
            {
                return Convert.ToInt32(data);
            }

            return 0;
        }
        #endregion


        public int GetInboundCountNew(int DepartureID, DateTime StartTime, DateTime EndTime)
        {
            String sql = @"
SELECT COUNT(1)
FROM (
    SELECT SIBQ.FormCode
    FROM SC_InboundQueue  SIBQ
    WHERE SIBQ.CreateTime BETWEEN  :StartTime AND :EndTime
        AND SIBQ.DepartureID=:DepartureID
        AND SIBQ.SeqStatus=:SeqStatus
        AND SIBQ.IsDeleted=0
    UNION
    SELECT SIB.FormCode
    FROM SC_Inbound SIB 
    WHERE SIB.CreateTime BETWEEN  :StartTime AND :EndTime
        AND SIB.DepartureID = :DepartureID
        AND SIB.IsDeleted = 0
)
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                     new OracleParameter() { ParameterName="DepartureID",  OracleDbType = OracleDbType.Int32 , Value = DepartureID  },
                     new OracleParameter() { ParameterName="StartTime",  OracleDbType = OracleDbType.Date , Value = StartTime },
                     new OracleParameter() { ParameterName="EndTime",  OracleDbType = OracleDbType.Date , Value = EndTime },
                     new OracleParameter() { ParameterName="SeqStatus", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
            };
            var data = ExecuteSqlScalar(TMSWriteConnection, sql, parameters);
            if (data != null && data != DBNull.Value)
            {
                return Convert.ToInt32(data);
            }
            return 0;
        }


        public int GetFormCodeInbound(string FormCode)
        {
            String sql = @"
SELECT COUNT(1)
FROM (
    SELECT SIBQ.FormCode
    FROM SC_InboundQueue  SIBQ
    WHERE SIBQ.FormCode =:FormCode
        AND SIBQ.SeqStatus=:SeqStatus
        AND SIBQ.IsDeleted=0
    UNION
    SELECT SB.FormCode
    FROM SC_Inbound SIB 
            JOIN SC_Bill SB ON SB.InBoundKey = SIB.IBID  AND SB.FormCode=SIB.FormCode
    WHERE SIB.FormCode =:FormCode
        AND SB.Status = :BillStatus
        AND SB.IsDeleted = 0
        AND SIB.IsDeleted = 0
)
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                     new OracleParameter(){  ParameterName = "FormCode",OracleDbType = OracleDbType.Varchar2,Value = FormCode},
                     new OracleParameter() { ParameterName="SeqStatus", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SeqStatus.NoHand },
                     new OracleParameter() { ParameterName="BillStatus", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.BillStatus.HaveBeenSorting }
            };
            var data = ExecuteSqlScalar(TMSWriteConnection, sql, parameters);
            if (data != null && data != DBNull.Value)
            {
                return Convert.ToInt32(data);
            }
            return 0;
        }


        public int AddV2(InboundEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundEntityModel is null");
            String sql = @"
INSERT INTO SC_Inbound(IBID,FormCode,InboundType,ApplyStation,DepartureID,ArrivalID,CreateBy,UpdateBy,IsDeleted,SyncFlag,DistributionCode)
VALUES(:IBID ,:FormCode,:InboundType,:ApplyStation,:DepartureID,:ArrivalID,:CreateBy,:UpdateBy,0,:SyncFlag,:DistributionCode)
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="IBID", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = model.IBID},
                new OracleParameter() { ParameterName="FormCode",  OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.FormCode},
                new OracleParameter() { ParameterName="InboundType", OracleDbType = OracleDbType.Int16 , Value = 0},
                new OracleParameter() { ParameterName="ApplyStation", OracleDbType = OracleDbType.Int32 , Value = model.ApplyStation},
                new OracleParameter() { ParameterName="DepartureID", OracleDbType = OracleDbType.Int32 , Value = model.DepartureID}, 
                new OracleParameter() { ParameterName="ArrivalID",  OracleDbType = OracleDbType.Int32 , Value = model.ArrivalID},
                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32 , Value = model.CreateBy},
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32 , Value = model.UpdateBy},
                new OracleParameter() { ParameterName="SyncFlag", OracleDbType = OracleDbType.Int16 , Value = (int)model.SyncFlag},   
                new OracleParameter(){ ParameterName = "DistributionCode",OracleDbType = OracleDbType.Varchar2,Value = model.DistributionCode}, 
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }


        public int GetDistributionInboundCount(string FormCode, string DistributionCode)
        {
            String sql = @"
SELECT COUNT(1)
FROM   SC_Inbound where FormCode = :FormCode and DistributionCode =:DistributionCode
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                     new OracleParameter(){  ParameterName = "FormCode",OracleDbType = OracleDbType.Varchar2,Value = FormCode},
                     new OracleParameter() { ParameterName="DistributionCode", OracleDbType = OracleDbType.Varchar2 , Value = DistributionCode},
            };
            var data = ExecuteSqlScalar(TMSWriteConnection, sql, parameters);
            if (data != null && data != DBNull.Value)
            {
                return Convert.ToInt32(data);
            }
            return 0;
        }
    }
}
