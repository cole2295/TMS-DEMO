using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.Common;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.Synchronous;
using System.Reflection;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Outbound;
using Vancl.TMS.Model.Sorting.Return;

namespace Vancl.TMS.DAL.Oracle.BaseInfo
{
    /// <summary>
    /// 表单数据层
    /// </summary>
    public class BillDAL : BaseDAL, IBillDAL
    {
        #region IBillDAL 成员

        /// <summary>
        /// 取得单号的逆向状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public Enums.ReturnStatus? GetBillReturnStatus(String FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
SELECT RETURNSTATUS
FROM SC_BILL
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType= OracleDbType.Varchar2,Size=50, Value= FormCode}
            };
            var objStatus = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (objStatus != null && objStatus != DBNull.Value)
            {
                return (Enums.ReturnStatus)Convert.ToInt32(objStatus);
            }
            return null;
        }

        /// <summary>
        /// 取得单号的状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns>运单不存在，返回null，否者返回本身的状态</returns>
        public Enums.BillStatus? GetBillStatus(String FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
SELECT STATUS
FROM SC_BILL
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType= OracleDbType.Varchar2 , Size = 50 , Value= FormCode}
            };
            var objStatus = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (objStatus != null && objStatus != DBNull.Value)
            {
                return (Enums.BillStatus)Convert.ToInt32(objStatus);
            }
            return null;
        }

        /// <summary>
        /// 更新单号状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <param name="status">更新的状态</param>
        /// <returns></returns>
        public int UpdateBillStatus(string FormCode, Enums.BillStatus status)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
UPDATE  SC_BILL
SET STATUS = :STATUS
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value= FormCode},
                    new OracleParameter() { ParameterName="STATUS", OracleDbType = OracleDbType.Int16, Value= (int)status}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 更新单号逆向状态
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <param name="status">更新的状态</param>
        /// <returns></returns>
        public int UpdateBillReturnStatus(String FormCode, Enums.ReturnStatus status)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
UPDATE  SC_BILL
SET RETURNSTATUS = :RETURNSTATUS
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType= OracleDbType.Varchar2,Size =50, Value= FormCode},
                    new OracleParameter() { ParameterName="RETURNSTATUS", OracleDbType= OracleDbType.Int16, Value= (Int16)status}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，入库更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_InboundUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            String sql = @"
UPDATE  SC_BILL
SET STATUS = :STATUS
,InboundKey = :InboundKey
,OutboundKey = :OutboundKey
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value= billmodel.FormCode},
                    new OracleParameter() { ParameterName="STATUS", OracleDbType = OracleDbType.Int16, Value= (int)billmodel.Status},
                    new OracleParameter() { ParameterName="InboundKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = billmodel.InboundKey},                     
                    new OracleParameter() { ParameterName="OutboundKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = billmodel.OutboundKey}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，运单装车更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_BillLoadingUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            String sql = @"
UPDATE  SC_BILL
SET STATUS = :STATUS
,CurrentDistributionCode = :CurrentDistributionCode
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billmodel.FormCode},
                    new OracleParameter() { ParameterName="STATUS", OracleDbType = OracleDbType.Int16, Value = (int)billmodel.Status},
                    new OracleParameter() { ParameterName="CurrentDistributionCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billmodel.CurrentDistributionCode}                 
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，出库更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_OutboundUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            String sql = @"
UPDATE  SC_BILL
SET STATUS = :STATUS
,CurrentDistributionCode = :CurrentDistributionCode
,OutboundKey = :OutboundKey
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billmodel.FormCode},
                    new OracleParameter() { ParameterName="STATUS", OracleDbType = OracleDbType.Int16, Value = (int)billmodel.Status},
                    new OracleParameter() { ParameterName="CurrentDistributionCode", OracleDbType = OracleDbType.Varchar2 ,  Size = 50 , Value = billmodel.CurrentDistributionCode},                     
                    new OracleParameter() { ParameterName="OutboundKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = billmodel.OutboundKey}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，分配站点更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_AssignStationUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            String sql = @"
UPDATE  SC_BILL
SET STATUS = :STATUS
,DeliverStationID = :DeliverStationID
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billmodel.FormCode },
                    new OracleParameter() { ParameterName="STATUS", OracleDbType = OracleDbType.Int16, Value= (int)billmodel.Status },
                     new OracleParameter() { ParameterName="DeliverStationID", OracleDbType = OracleDbType.Int32, Value = billmodel.DeliverStationID }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 取得LMS同步到TMS时用于比较的运单信息
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public T GetBillForComparing<T>(string formCode) where T : IBillLms2TmsForComparing, new()
        {
            if (String.IsNullOrWhiteSpace(formCode)) throw new ArgumentNullException("formCode is null or empty.");
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            PropertyInfo[] pis = typeof(T).GetProperties();
            int count = 0;
            foreach (var pi in pis)
            {
                if (pi.CanWrite && pi.CanRead)
                {
                    if (count > 0)
                    {
                        sb.Append(",");
                    }
                    sb.Append(pi.Name);
                    count++;
                }
            }
            sb.Append(@"
                FROM SC_BILL
                WHERE FORMCODE = :FORMCODE");
            OracleParameter[] arguments = new OracleParameter[]
            {
                new OracleParameter() { ParameterName = "FORMCODE", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = formCode }
            };
            return ExecuteSqlSingle_ByReaderReflect<T>(TMSWriteConnection, sb.ToString(), arguments);
        }

        /// <summary>
        /// LMS系统同步到TMS系统，转站申请更新Bill相关信息
        /// </summary>
        /// <param name="billmodel"></param>
        /// <returns></returns>
        public int Lms2Tms_TurnStationApplyUpdateBill(BillModel billmodel)
        {
            if (billmodel == null) throw new ArgumentNullException("billmodel is null.");
            String sql = @"
UPDATE  SC_BILL
SET TURNSTATIONKEY = :TURNSTATIONKEY , DeliverStationID = :DeliverStationID
,STATUS = :STATUS
WHERE FORMCODE = :FORMCODE
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="FORMCODE", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billmodel.FormCode},
                new OracleParameter() { ParameterName="STATUS", OracleDbType = OracleDbType.Int16, Value = (int)billmodel.Status},
                new OracleParameter() { ParameterName="DeliverStationID", OracleDbType = OracleDbType.Int32, Value = billmodel.DeliverStationID},
                new OracleParameter() { ParameterName="TURNSTATIONKEY", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = billmodel.TurnstationKey}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }
        public BillModel GetBillByFormCode(string formCode)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                         SELECT BID
                                                       ,Status ,OutboundKey ,UpdateBy
                                                       ,UpdateDept,UpdateTime
                                                       ,FormCode,BillType,WarehouseId
                                                       ,CustomerOrder,DeliverStationID,TurnstationKey
                                                       ,InboundKey,CurrentDistributionCode,MerchantID
                                                       ,DistributionCode,Source,CreateDept,DeliverCode,ReturnStatus
                                                       ,CreateTime,CreateBy
                                          FROM    SC_BILL
                                          WHERE FormCode = :FormCode
                                         ");
            OracleParameter[] arguments = { new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } };
            return ExecuteSqlSingle_ByReaderReflect<BillModel>(TMSReadOnlyConnection, SbSql.ToString(), arguments);
        }

        public string GetCustomerByFormCode(string FormCode)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
SELECT CustomerOrder
FROM    SC_BILL
WHERE FormCode = :FormCode
    AND rownum=1
                                         ");
            OracleParameter[] arguments = { new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = FormCode } };

            var dt = ExecuteSqlDataTable(TMSReadOnlyConnection, SbSql.ToString(), arguments);
            if (dt == null || dt.Rows.Count == 0) return null;
            return Convert.ToString(dt.Rows[0]["CustomerOrder"]);
        }
        #endregion

        #region IBillDAL 成员

        /// <summary>
        /// 取得商家单号对应关系
        /// </summary>
        /// <param name="formType">单号类型</param>
        /// <param name="code">单号</param>
        /// <returns></returns>
        public List<MerchantFormCodeRelationModel> GetMerchantFormCodeRelation(Enums.SortCenterFormType formType, string code, int? merchantId)
        {
            String strbillcode = "FormCode";
            if (formType == Enums.SortCenterFormType.Order)
            {
                strbillcode = "CustomerOrder";
            }
            else if (formType == Enums.SortCenterFormType.DeliverCode)
            {
                strbillcode = "DeliverCode";
            }
            String sql = String.Format(@"
SELECT  bill.FormCode ,bill.Status, bill.MerchantID, mb.MerchantName,bill.DeliverCode
    ,mb.IsSkipPrintBill,mb.IsNeedWeight,mb.IsCheckWeight,mb.CheckWeight
FROM SC_Bill bill
JOIN PS_PMS.MerchantBaseInfo mb ON bill.merchantid=mb.ID
WHERE bill.IsDeleted = 0 AND {0} = :BillCode
"
                , strbillcode);
            List<OracleParameter> arguments = new List<OracleParameter>();
            arguments.Add(new OracleParameter() { ParameterName = "BillCode", OracleDbType = OracleDbType.Varchar2, Size = 50, Value = code });
            if (merchantId.HasValue && merchantId.Value > 0)
            {
                sql += " AND bill.merchantid=:merchantid ";
                arguments.Add(new OracleParameter() { ParameterName = "merchantid", OracleDbType = OracleDbType.Int32, Value = merchantId.Value });
            }
            var listResult = ExecuteSql_ByDataTableReflect<MerchantFormCodeRelationModel>(TMSWriteConnection, sql, arguments.ToArray());
            if (listResult != null)
            {
                return listResult.ToList();
            }
            return null;
        }

        /// <summary>
        /// 根据客户订单号取得系统运单号
        /// </summary>
        /// <param name="CustomerOrder">客户订单号</param>
        /// <returns></returns>
        public List<string> GetFormCodeByCustomerOrder(string CustomerOrder)
        {
            String sql = @"
SELECT  FormCode 
FROM SC_Bill
WHERE IsDeleted = 0 AND CustomerOrder = :CustomerOrder
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="CustomerOrder",  OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = CustomerOrder}
            };
            var dtResult = ExecuteSqlDataTable(TMSWriteConnection, sql, arguments);
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                List<String> listFormCode = new List<string>(dtResult.Rows.Count);
                foreach (DataRow item in dtResult.Rows)
                {
                    listFormCode.Add(item["FormCode"].ToString());
                }
                return listFormCode;
            }
            return null;
        }

        /// <summary>
        /// 根据客户订单号和商家取得系统运单号
        /// </summary>
        /// <param name="CustomerOrder">客户订单号</param>
        /// <param name="MerchantID">商家ID</param>
        /// <returns></returns>
        public List<string> GetFormCodeByCustomerOrder(string CustomerOrder, int MerchantID)
        {
            List<OracleParameter> parameters = new List<OracleParameter>();
            String sql = @"
                SELECT  FormCode 
                FROM SC_Bill
                WHERE IsDeleted = 0 AND CustomerOrder = :CustomerOrder
                ";
            parameters.Add(new OracleParameter() { ParameterName = "CustomerOrder", OracleDbType = OracleDbType.Varchar2, Size = 50, Value = CustomerOrder });
            if (MerchantID > 0)
            {
                sql += (" AND MerchantID=:MerchantID");
                parameters.Add(new OracleParameter() { ParameterName = "MerchantID", OracleDbType = OracleDbType.Int32, Value = MerchantID });
            }
            var dtResult = ExecuteSqlDataTable(TMSWriteConnection, sql, parameters.ToArray());
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                List<String> listFormCode = new List<string>(dtResult.Rows.Count);
                foreach (DataRow item in dtResult.Rows)
                {
                    listFormCode.Add(item["FormCode"].ToString());
                }
                return listFormCode;
            }
            return null;
        }

        /// <summary>
        /// 取得入库单号对象
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel(string FormCode)
        {
            String sql = @"
SELECT 
    BILL.FormCode
    ,BILL.Status
    ,BILL.BillType
    ,BILL.WarehouseID
    ,BILL.CustomerOrder
    ,BILL.DeliverStationID
    ,BILL.TurnstationKey
    ,BILL.OutBoundKey
    ,BILL.InBoundKey
    ,BILL.CurrentDistributionCode
    ,CASE WHEN EXISTS(
        SELECT SIQ.IBSID
        FROM SC_InboundQueue SIQ
        WHERE SIQ.FormCode = :FormCode
            AND SIQ.SeqStatus = :SeqStatus
            AND SIQ.IsDeleted=0
        ) THEN 1
    ELSE 0 END AS IsInbounding
    ,CASE WHEN BILL.Source=2 AND (mbi.IsSkipPrintBill=0 OR mbi.IsNeedWeight=1) then
          1
    ELSE 0 END IsJudgePrint
    ,CASE WHEN mbi.isvalidatebill is null or  mbi.isvalidatebill=0 then
          0
    ELSE 1 END IsValidateBill
FROM SC_Bill BILL
JOIN  MerchantBaseInfo mbi ON BILL.Merchantid=mbi.ID
WHERE BILL.FormCode = :FormCode
    AND BILL.IsDeleted = 0
    AND ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[] 
            {   
                 new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode },
                 new OracleParameter() { ParameterName = "SeqStatus", OracleDbType = OracleDbType.Int16, Value = (int)Enums.SeqStatus.NoHand }
            };
            return ExecuteSqlSingle_ByDataTableReflect<InboundBillModel>(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 取得入库单号对象【不限制站点】
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel_NoLimitedStation(string FormCode)
        {
            String sql = @"
SELECT
    BILL.FormCode
    ,BILL.Status
    ,BILL.BillType
    ,BILL.CustomerOrder
    ,BILL.DeliverStationID
    ,BILL.TurnstationKey
    ,BILL.InBoundKey
    ,BILL.MerchantID
    ,BILL.DistributionCode
    ,BILL.CurrentDistributionCode
    ,BILL.Source
FROM SC_Bill BILL
WHERE BILL.FormCode= :FormCode
    AND BILL.IsDeleted = 0
    AND ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[] 
            {   
                 new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode }
            };
            return ExecuteSqlSingle_ByDataTableReflect<InboundBillModel>(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 队列处理服务取得入库单号对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel_ByQueueHandled(string FormCode)
        {
            String sql = @"
SELECT
    BILL.FormCode
    ,BILL.Status
    ,BILL.BillType
    ,BILL.CustomerOrder
    ,BILL.DeliverStationID
    ,BILL.TurnstationKey
    ,BILL.InBoundKey
    ,BILL.MerchantID
    ,BILL.DistributionCode
    ,BILL.CurrentDistributionCode
    ,BILL.Source
FROM SC_Bill BILL
WHERE BILL.FormCode= :FormCode
    AND BILL.IsDeleted = 0
    AND ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[] 
            {   
                 new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode }
            };
            var model = ExecuteSqlSingle_ByDataTableReflect<InboundBillModel>(TMSWriteConnection, sql, parameters);
            if (model != null)
            {
                model.IsFirstInbound = String.IsNullOrWhiteSpace(model.InboundKey);
                return model;
            }
            return null;
        }

        /// <summary>
        /// 短信队列处理服务取得入库单号对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel_BySMSQueueHandled(string FormCode)
        {
            String sql = @"
SELECT
     BILL.FormCode
    ,BILL.DeliverStationID
    ,BILL.Source
    ,BILL.InBoundKey
    ,BILL.MerchantID
FROM SC_Bill BILL
WHERE BILL.FormCode= :FormCode
    AND BILL.IsDeleted = 0
    AND ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[] 
            {   
                 new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode }
            };
            return ExecuteSqlSingle_ByDataTableReflect<InboundBillModel>(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 取得转站入库单号对象
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public InboundBillModel GetInboundBillModel_TurnStation(string FormCode)
        {
            String sql = @"
SELECT
    BILL.FormCode
    ,BILL.Status
    ,BILL.BillType
    ,BILL.CustomerOrder
    ,BILL.DeliverStationID
    ,BILL.TurnstationKey
    ,BILL.MerchantID
    ,BILL.DistributionCode
    ,BILL.CurrentDistributionCode
FROM SC_Bill BILL
WHERE BILL.FormCode= :FormCode
    AND BILL.IsDeleted = 0
    AND ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[] 
            {   
                 new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode }
            };
            return ExecuteSqlSingle_ByDataTableReflect<InboundBillModel>(TMSWriteConnection, sql, parameters);
        }


        /// <summary>
        /// 入库时修改主单对象【不限制站点】
        /// </summary>
        /// <param name="billModel">入库运单对象</param>
        /// <returns></returns>
        public int UpdateBillModelByInbound_NoLimitedStation(InboundBillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("billModel is null.");
            String sql = @"
UPDATE SC_Bill
SET Status = :Status ,
    InboundKey = :InboundKey ,
    OutboundKey = :OutboundKey ,
    DeliverStationID = :DeliverStationID ,
    UpdateBy = :OpUser ,
    UpdateTime = SYSDATE ,
    UpdateDept = :OpDept ,
    CreateDept = CASE WHEN  InboundKey IS NULL THEN :OpDept ELSE CreateDept END
WHERE
    FormCode = :FormCode
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2  , Size = 50 , Value = billModel.FormCode },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16 , Value = (int)billModel.Status },
                new OracleParameter() { ParameterName="DeliverStationID", OracleDbType = OracleDbType.Int32 , Value = billModel.DeliverStationID },
                new OracleParameter() { ParameterName="InboundKey", OracleDbType = OracleDbType.Varchar2  , Size = 20 , Value = billModel.InboundKey },
                new OracleParameter() { ParameterName="OutboundKey", OracleDbType = OracleDbType.Varchar2  , Size = 20 , Value = billModel.OutboundKey },
                new OracleParameter() { ParameterName="OpUser", OracleDbType = OracleDbType.Int32 , Value = billModel.UpdateBy },
                new OracleParameter() { ParameterName="OpDept", OracleDbType = OracleDbType.Int32 , Value = billModel.UpdateDept }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns></returns>
        public int UpdateBillModelByInbound(InboundBillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("billModel is null.");
            String sql = String.Format(@"
UPDATE SC_Bill
SET Status = :Status ,
    InboundKey = :InboundKey ,
    OutboundKey = :OutboundKey ,
    UpdateBy = :OpUser ,
    UpdateTime = SYSDATE ,
    UpdateDept = :OpDept 
    {0}
WHERE
    FormCode = :FormCode
",
 billModel.IsFirstInbound ? " , CreateDept = :OpDept " : ""
    );
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billModel.FormCode },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16 , Value = (int)billModel.Status },
                new OracleParameter() { ParameterName="InboundKey", OracleDbType = OracleDbType.Varchar2  , Size = 20 , Value = billModel.InboundKey },
                new OracleParameter() { ParameterName="OutboundKey", OracleDbType = OracleDbType.Varchar2  , Size = 20 , Value = billModel.OutboundKey },
                new OracleParameter() { ParameterName="OpUser", OracleDbType = OracleDbType.Int32 , Value = billModel.UpdateBy },
                new OracleParameter() { ParameterName="OpDept", OracleDbType = OracleDbType.Int32 , Value = billModel.UpdateDept }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IBillDAL 成员

        /// <summary>
        /// 取得TMS分拣同步到LMS物流主库所需要的运单信息
        /// </summary>
        /// <param name="FormCode">系统运单号</param>
        /// <returns></returns>
        public BillModel GetBillModel4TmsSync2Lms(string FormCode)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IBillDAL 成员


        public Model.Sorting.Outbound.OutboundBillModel GetOutboundBillModel(string FormCode)
        {
            if (FormCode == null) throw new ArgumentNullException("FormCode is null.");
            String sql = @"
SELECT bill.FormCode, bill.Status , bill.DeliverStationID, bill.DistributionCode, ib.DepartureID, ib.ArrivalID, ib.InboundType, bill.MerchantID, bill.Source, bill.InboundKey,bill.OutboundKey, bill.CustomerOrder
FROM SC_Bill bill
    JOIN SC_Inbound ib ON bill.InboundKey = ib.IBID AND bill.FormCode = ib.FormCode
WHERE bill.FormCode = :FormCode
    AND bill.IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="FormCode", OracleDbType= OracleDbType.Varchar2 , Size = 50 , Value = FormCode }
            };
            return ExecuteSqlSingle_ByDataTableReflect<OutboundBillModel>(TMSWriteConnection, sql, arguments);
        }

        public Model.Sorting.Outbound.OutboundBillModel GetOutboundBillModelV2(string FormCode)
        {
            if (FormCode == null) throw new ArgumentNullException("FormCode is null.");
            String sql = @"
SELECT bill.FormCode, bill.Status , bill.DeliverStationID, bill.DistributionCode, ob.DepartureID, ob.ArrivalID,  bill.MerchantID, bill.Source, bill.InboundKey, bill.CustomerOrder
FROM SC_Bill bill
    JOIN sc_outbound ob ON bill.outboundkey = ob.OBID AND bill.FormCode = ob.FormCode
WHERE bill.FormCode = :FormCode
    AND bill.IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[]
			    {
				    new OracleParameter()
					    {
						    ParameterName = "FormCode",
						    OracleDbType = OracleDbType.Varchar2,
						    Size = 50,
						    Value = FormCode
					    }
			    };
            return ExecuteSqlSingle_ByDataTableReflect<OutboundBillModel>(TMSWriteConnection, sql, arguments);
        }

        public List<Model.Sorting.Outbound.OutboundBillModel> GetOutboundBillModel_BatchOutbound(List<string> FormCode)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得出库单号对象列表【查询出库】
        /// </summary>
        /// <param name="outboundArg">出库参数对象</param>
        /// <param name="FormCode">系统运单号列表</param>
        /// <returns></returns>
        public List<OutboundBillModel> GetOutboundBillModel_SearchOutbound(IOutboundArgModel outboundArg, List<string> FormCode)
        {
            if (outboundArg == null) throw new ArgumentNullException("outboundArg is null.");
            if (FormCode == null) throw new ArgumentNullException("FormCode is null.");
            String sql = @"
SELECT bill.FormCode, bill.Status , bill.DeliverStationID, bill.DistributionCode, ib.DepartureID, ib.ArrivalID, ib.InboundType, bill.MerchantID, bill.Source
FROM SC_Bill bill
    JOIN SC_Inbound ib ON bill.InboundKey = ib.IBID AND bill.FormCode = ib.FormCode
    JOIN
    (
        SELECT REGEXP_SUBSTR(:listFormCodeStr, '[^,]+', 1, LEVEL) AS FormCode
        FROM DUAL
        CONNECT BY LEVEL <=
        LENGTH(TRIM(TRANSLATE(:listFormCodeStr,TRANSLATE(:listFormCodeStr, ',', ' '), ' '))) + 1
    ) tmpFormCode ON bill.FormCode = tmpFormCode.FormCode
WHERE bill.Status = :Status
    AND ib.DepartureID = :DepartureID
    AND ib.ArrivalID = :ArrivalID
    AND bill.IsDeleted = 0
";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16 , Value = (int)outboundArg.PreCondition.PreStatus },
                new OracleParameter() { ParameterName="DepartureID", OracleDbType = OracleDbType.Int32, Value = outboundArg.OpUser.ExpressId.Value },
                new OracleParameter() { ParameterName="ArrivalID", OracleDbType = OracleDbType.Int32, Value = outboundArg.ToStation.ExpressCompanyID },
                new OracleParameter() { ParameterName="listFormCodeStr", OracleDbType = OracleDbType.Varchar2 , Size = 4000 , Value = String.Join(",",FormCode) }
            };
            var list = ExecuteSql_ByReaderReflect<OutboundBillModel>(TMSWriteConnection, sql, arguments);
            if (list != null)
            {
                return list.ToList();
            }
            return null;
        }

        public List<OutboundBillModel> GetOutboundBillModel_PackingOutbound(IOutboundArgModel outboundArg, List<string> FormCode)
        {
            if (outboundArg == null) throw new ArgumentNullException("outboundArg is null.");
            if (FormCode == null) throw new ArgumentNullException("FormCode is null.");
            String sql = @"
			SELECT bill.FormCode, bill.Status , bill.DeliverStationID, bill.DistributionCode, ib.DepartureID, ib.ArrivalID, ib.InboundType, bill.MerchantID, bill.Source
			FROM SC_Bill bill
				JOIN SC_Inbound ib ON bill.InboundKey = ib.IBID AND bill.FormCode = ib.FormCode
				JOIN
				(
					SELECT REGEXP_SUBSTR(:listFormCodeStr, '[^,]+', 1, LEVEL) AS FormCode
					FROM DUAL
					CONNECT BY LEVEL <=
					LENGTH(TRIM(TRANSLATE(:listFormCodeStr,TRANSLATE(:listFormCodeStr, ',', ' '), ' '))) + 1
				) tmpFormCode ON bill.FormCode = tmpFormCode.FormCode
			WHERE bill.Status = :Status
				AND ib.DepartureID = :DepartureID
				--AND ib.ArrivalID = :ArrivalID
				AND bill.IsDeleted = 0
			";
            OracleParameter[] arguments = new OracleParameter[] 
			{ 
				new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16 , Value = (int)outboundArg.PreCondition.PreStatus },
				new OracleParameter() { ParameterName="DepartureID", OracleDbType = OracleDbType.Int32, Value = outboundArg.OpUser.ExpressId.Value },
				new OracleParameter() { ParameterName="listFormCodeStr", OracleDbType = OracleDbType.Varchar2 , Size = 4000 , Value = String.Join(",",FormCode) }
			};
            var list = ExecuteSql_ByReaderReflect<OutboundBillModel>(TMSWriteConnection, sql, arguments);
            if (list != null)
            {
                return list.ToList();
            }
            return null;
        }

		public List<OutboundBillModel> GetOutboundBillModel_PackingOutboundV2(IOutboundArgModel outboundArg, List<string> FormCode)
		{
			if (outboundArg == null) throw new ArgumentNullException("outboundArg is null.");
			if (FormCode == null) throw new ArgumentNullException("FormCode is null.");
			String sql = @"
			SELECT bill.FormCode, bill.Status , bill.DeliverStationID, bill.DistributionCode, ib.DepartureID, ib.ArrivalID, ib.InboundType, bill.MerchantID, bill.Source
			FROM SC_Bill bill
				JOIN SC_Inbound ib ON bill.InboundKey = ib.IBID AND bill.FormCode = ib.FormCode
				JOIN
				(
					SELECT REGEXP_SUBSTR(:listFormCodeStr, '[^,]+', 1, LEVEL) AS FormCode
					FROM DUAL
					CONNECT BY LEVEL <=
					LENGTH(TRIM(TRANSLATE(:listFormCodeStr,TRANSLATE(:listFormCodeStr, ',', ' '), ' '))) + 1
				) tmpFormCode ON bill.FormCode = tmpFormCode.FormCode
			WHERE bill.Status = :Status
				AND ib.DepartureID = :DepartureID
				--AND ib.ArrivalID = :ArrivalID
				AND bill.IsDeleted = 0
			";
			OracleParameter[] arguments = new OracleParameter[] 
			{ 
				new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.BillStatus.Inbounded },
				new OracleParameter() { ParameterName="DepartureID", OracleDbType = OracleDbType.Int32, Value = outboundArg.OpUser.ExpressId.Value },
				new OracleParameter() { ParameterName="listFormCodeStr", OracleDbType = OracleDbType.Varchar2 , Size = 4000 , Value = String.Join(",",FormCode) }
			};
			var list = ExecuteSql_ByReaderReflect<OutboundBillModel>(TMSWriteConnection, sql, arguments);
			if (list != null)
			{
				return list.ToList();
			}
			return null;
		}
        /// <summary>
        /// 出库时修改主单对象
        /// </summary>
        /// <param name="billModel"></param>
        /// <returns></returns>
        public int UpdateBillModelByOutbound(OutboundBillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("billModel is null");
            String sql = @"
			UPDATE SC_Bill
			SET Status = :Status ,
				UpdateBy = :UpdateBy ,
				UpdateDept = :UpdateDept ,
				UpdateTime = SYSDATE ,
				OutboundKey = :OutboundKey ,
				CurrentDistributionCode = :CurrentDistributionCode
			WHERE  FormCode = :FormCode AND IsDeleted = 0
			";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billModel.FormCode},
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16, Value = (int)billModel.Status },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32, Value = billModel.UpdateBy },
                new OracleParameter() { ParameterName="UpdateDept", OracleDbType = OracleDbType.Int32, Value = billModel.UpdateDept },
                new OracleParameter() { ParameterName="OutboundKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = billModel.OutboundKey },
                new OracleParameter() { ParameterName="CurrentDistributionCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billModel.CurrentDistributionCode }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

		public int UpdateBillModelByOutboundV2(OutboundBillModel billModel)
		{
			if (billModel == null) throw new ArgumentNullException("billModel is null");
			String sql = @"
			UPDATE SC_Bill
			SET Status = :Status ,
				UpdateBy = :UpdateBy ,
				UpdateDept = :UpdateDept ,
				UpdateTime = SYSDATE ,
				OutboundKey = :OutboundKey ,
				CurrentDistributionCode = :CurrentDistributionCode,
				Deliverstationid = :Deliverstationid
			WHERE  FormCode = :FormCode AND IsDeleted = 0
			";
			OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billModel.FormCode},
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16, Value = (int)billModel.Status },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32, Value = billModel.UpdateBy },
                new OracleParameter() { ParameterName="UpdateDept", OracleDbType = OracleDbType.Int32, Value = billModel.UpdateDept },
                new OracleParameter() { ParameterName="OutboundKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = billModel.OutboundKey },
                new OracleParameter() { ParameterName="CurrentDistributionCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = billModel.CurrentDistributionCode },
				new OracleParameter() { ParameterName="Deliverstationid", OracleDbType = OracleDbType.Int32 , Size = 50 , Value = billModel.DeliverStationID }
            };
			return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
		}

        /// <summary>
        /// 批量出库时修改主单对象
        /// </summary>
        /// <param name="listBillModel">需要修改为的对象列表</param>
        public void BatchUpdateBillModelByOutbound(List<OutboundBillModel> listBillModel)
        {
            if (listBillModel == null) throw new ArgumentNullException("listBillModel is null");
            String sql = @"
			UPDATE SC_Bill
			SET Status = :Status ,
				UpdateBy = :UpdateBy ,
				UpdateDept = :UpdateDept ,
				UpdateTime = SYSDATE ,
				OutboundKey = :OutboundKey ,
				CurrentDistributionCode = :CurrentDistributionCode
			WHERE  FormCode = :FormCode AND IsDeleted = 0
			";
            int npos = 0;
            String[] arrFormCode = new String[listBillModel.Count];
            int[] arrStatus = new int[listBillModel.Count];
            int[] arrUpdateBy = new int[listBillModel.Count];
            int[] arrUpdateDept = new int[listBillModel.Count];
            String[] arrOutboundKey = new String[listBillModel.Count];
            String[] arrCurrentDistributionCode = new String[listBillModel.Count];
            listBillModel.ForEach(p =>
            {
                arrFormCode[npos] = p.FormCode;
                arrStatus[npos] = (int)p.Status;
                arrUpdateBy[npos] = p.UpdateBy;
                arrUpdateDept[npos] = p.UpdateDept;
                arrOutboundKey[npos] = p.OutboundKey;
                arrCurrentDistributionCode[npos] = p.CurrentDistributionCode;
                npos++;
            });
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = arrFormCode},
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16 , Value = arrStatus },
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32, Value = arrUpdateBy },
                new OracleParameter() { ParameterName="UpdateDept", OracleDbType = OracleDbType.Int32, Value = arrUpdateDept },
                new OracleParameter() { ParameterName="OutboundKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = arrOutboundKey },
                new OracleParameter() { ParameterName="CurrentDistributionCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = arrCurrentDistributionCode }
            };
            ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listBillModel.Count, arguments);
        }

	    public void BatchUpdateBillModelByOutboundV2(List<OutboundBillModel> listBillModel)
	    {
		    if (listBillModel == null) throw new ArgumentNullException("listBillModel is null");
		    String sql = @"
			UPDATE SC_Bill
			SET Status = :Status ,
				UpdateBy = :UpdateBy ,
				UpdateDept = :UpdateDept ,
				UpdateTime = SYSDATE ,
				OutboundKey = :OutboundKey ,
				CurrentDistributionCode = :CurrentDistributionCode,
				Deliverstationid = :Deliverstationid
			WHERE  FormCode = :FormCode AND IsDeleted = 0
			";
		    int npos = 0;
		    String[] arrFormCode = new String[listBillModel.Count];
		    int[] arrStatus = new int[listBillModel.Count];
		    int[] arrUpdateBy = new int[listBillModel.Count];
		    int[] arrUpdateDept = new int[listBillModel.Count];
		    String[] arrOutboundKey = new String[listBillModel.Count];
		    String[] arrCurrentDistributionCode = new String[listBillModel.Count];
		    int[] arrDeliverstationid = new int[listBillModel.Count];
		    listBillModel.ForEach(p =>
			    {
				    arrFormCode[npos] = p.FormCode;
				    arrStatus[npos] = (int) p.Status;
				    arrUpdateBy[npos] = p.UpdateBy;
				    arrUpdateDept[npos] = p.UpdateDept;
				    arrOutboundKey[npos] = p.OutboundKey;
				    arrCurrentDistributionCode[npos] = p.CurrentDistributionCode;
				    arrDeliverstationid[npos] = p.DeliverStationID;
				    npos++;
			    });
		    OracleParameter[] arguments = new OracleParameter[]
			    {
				    new OracleParameter()
					    {
						    ParameterName = "FormCode",
						    OracleDbType = OracleDbType.Varchar2,
						    Size = 50,
						    Value = arrFormCode
					    },
				    new OracleParameter() {ParameterName = "Status", OracleDbType = OracleDbType.Int16, Value = arrStatus},
				    new OracleParameter() {ParameterName = "UpdateBy", OracleDbType = OracleDbType.Int32, Value = arrUpdateBy},
				    new OracleParameter() {ParameterName = "UpdateDept", OracleDbType = OracleDbType.Int32, Value = arrUpdateDept},
				    new OracleParameter()
					    {
						    ParameterName = "OutboundKey",
						    OracleDbType = OracleDbType.Varchar2,
						    Size = 20,
						    Value = arrOutboundKey
					    },
				    new OracleParameter()
					    {
						    ParameterName = "CurrentDistributionCode",
						    OracleDbType = OracleDbType.Varchar2,
						    Size = 50,
						    Value = arrCurrentDistributionCode
					    },
					new OracleParameter()
					{
						ParameterName = "Deliverstationid",
						OracleDbType = OracleDbType.Int32,
						Size = 50,
						Value = arrDeliverstationid
					}
			    };
		    ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listBillModel.Count, arguments);
	    }

	    #endregion

        #region IWaybillDao 成员

        /// <summary>
        /// 订单装车 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateWaybillByTruck(BillModel model)
        {
            var strSql = @"
Update SC_Bill 
SET  Status = :Status,
    UpdateTime = :UpdateTime,
    CurrentDistributionCode = :CurrentDistributionCode 
WHERE  FormCode = :FormCode
";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="Status", OracleDbType  = OracleDbType.Int16 , Value = (int)model.Status },
                new OracleParameter() { ParameterName="UpdateTime", OracleDbType = OracleDbType.Date , Value = model.UpdateTime },
                new OracleParameter() { ParameterName="CurrentDistributionCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.CurrentDistributionCode },
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = model.FormCode }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql, arguments);
        }

        #endregion

        #region IBillDAL 成员

        /// <summary>
        /// 转站入库时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns>是否成功</returns>
        public int UpdateBillModelByTurnInbound(InboundBillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("billModel is null.");
            String sql = String.Format(@"
UPDATE SC_Bill
SET Status = :Status ,
    InboundKey = :InboundKey ,
    OutboundKey = :OutboundKey ,
    UpdateBy = :OpUser ,
    UpdateTime = SYSDATE ,
    UpdateDept = :OpDept
WHERE
    FormCode = :FormCode
");
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2  , Size = 50 , Value = billModel.FormCode },
                new OracleParameter() { ParameterName="Status", OracleDbType = OracleDbType.Int16 , Value = (int)billModel.Status },
                new OracleParameter() { ParameterName="InboundKey", OracleDbType = OracleDbType.Varchar2 , Size = 20 , Value = billModel.InboundKey },
                new OracleParameter() { ParameterName="OutboundKey", OracleDbType = OracleDbType.Varchar2  , Size = 20 , Value = billModel.OutboundKey },
                new OracleParameter() { ParameterName="OpUser", OracleDbType = OracleDbType.Int32 , Value = billModel.UpdateBy },
                new OracleParameter() { ParameterName="OpDept", OracleDbType = OracleDbType.Int32 , Value = billModel.UpdateDept }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion



        public BillDeliveryModel QueryDeliveryInfo(string formCode, string receiveArea)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
select  
    ec.SiteNo StationNum,
    ec.CompanyName StationName,
    area.ZoneCode CityCode,
    ec.COMPANYFLAG 
from sc_bill w    
LEFT JOIN PS_PMS.ExpressCompany ec  ON  ec.ExpressCompanyID = W.DeliverStationID 
LEFT JOIN PS_PMS.Area area   ON area.AreaName=:ReceiveArea and area.CityID = ec.CITYID and area.ISDELETED=0 
where w.FormCode =:FormCode
                                         ");
            OracleParameter[] arguments = { new OracleParameter() { ParameterName = "ReceiveArea", DbType = DbType.String, Value = receiveArea } ,
                                          new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } ,};
            return ExecuteSqlSingle_ByReaderReflect<BillDeliveryModel>(TMSReadOnlyConnection, SbSql.ToString(), arguments);
        }
        /// <summary>
        /// 退库单入库
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool BillReturnInBound(BillModel model)
        {
            //如果为返货入库，则更新返货入库时间，反之更新拒收入库或退换货入库时间
            string time = model.ReturnStatus == Enums.ReturnStatus.ReturnInbounded ? "ReturnExpressCompanyTime" : "ReturnTime";
            String sql = String.Format(@"
UPDATE  SC_BILL
SET RETURNSTATUS = :RETURNSTATUS,
    UPDATEBY=:UPDATEBY,
    updatedept=:updatedept,
    UPDATETIME=:UPDATETIME
WHERE FORMCODE = :FORMCODE
   AND STATUS IN({0},{1})
", (Int16)Enums.BillStatus.DeliverySuccess, (Int16)Enums.BillStatus.Rejected);
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="RETURNSTATUS", OracleDbType= OracleDbType.Int16, Value= (Int16)model.ReturnStatus},
                    new OracleParameter() { ParameterName="UPDATEBY", OracleDbType= OracleDbType.Int32,Size=4, Value= model.UpdateBy},
                    new OracleParameter() { ParameterName="updatedept", OracleDbType= OracleDbType.Int32,Size=4, Value= model.UpdateDept},
                    new OracleParameter() { ParameterName="FORMCODE", OracleDbType= OracleDbType.Varchar2,Size=50, Value= model.FormCode},
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments) > 0;
        }
        public string GetFormCodeLists(MerchantReturnSearchModel searchModel)
        {
            string FormCodelists = "";
            StringBuilder SbSql = new StringBuilder();
            List<OracleParameter> arguments = new List<OracleParameter>();
            SbSql.Append(@"
SELECT distinct sb.formcode  
FROM SC_BILL sb
JOIN ExpressCompany ec ON ec.expresscompanyid = sb.deliverstationid
JOIN City ct ON ct.cityid = ec.cityid
JOIN StatusInfo b ON b.statusNo = sb.status AND b.statustypeno=1
JOIN StatusInfo c ON c.statusNo = sb.Billtype AND c.statustypeno=2
JOIN StatusInfo d ON d.statusNo = sb.returnstatus AND d.statustypeno=5
JOIN distribution db ON ec.distributioncode = db.distributioncode
JOIN SC_BILLINFO sbi ON sbi.formcode=sb.formcode
JOIN merchantbaseinfo mbi ON mbi.id=sb.merchantid
JOIN SC_BILLRETURNDETAILINFO srd ON srd.formcode=sb.formcode
JOIN distributionmerchantrelation dmr ON dmr.merchantid=sb.merchantid
WHERE sb.isdeleted = 0 and dmr.isdeleted=0
      AND (srd.isdeleted=0 or srd.isdeleted is null)
");
            if (!string.IsNullOrEmpty(searchModel.CurrentDistributionCode))
            {
                SbSql.AppendFormat(" AND dmr.DistributionCode = '{0}'", searchModel.CurrentDistributionCode);
            }
            if (!string.IsNullOrEmpty(searchModel.FormCode))
            {
                SbSql.Append(" AND sb.FormCode = :FormCode ");
                arguments.Add(new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2, Size = 50, Value = searchModel.FormCode });
            }
            if (!string.IsNullOrEmpty(searchModel.DeliverStationID))
            {
                SbSql.Append(" AND sb.deliverstationid = :deliverstationid ");
                arguments.Add(new OracleParameter() { ParameterName = "deliverstationid", OracleDbType = OracleDbType.Int32, Size = 4, Value = searchModel.DeliverStationID });
            }
            if (!string.IsNullOrEmpty(searchModel.DeliverCode))
            {
                SbSql.Append(" AND sb.DeliverCode = :DeliverCode ");
                arguments.Add(new OracleParameter() { ParameterName = "DeliverCode", OracleDbType = OracleDbType.Varchar2, Size = 50, Value = searchModel.DeliverCode });
            }
            if (!string.IsNullOrEmpty(searchModel.BoxNo))
            {
                SbSql.Append(" AND srd.BoxNo = :BoxNo ");
                arguments.Add(new OracleParameter() { ParameterName = "BoxNo", OracleDbType = OracleDbType.Varchar2, Size = 50, Value = searchModel.BoxNo });
            }
            if (!searchModel.isHasPrint)
            {
                if (!string.IsNullOrEmpty(searchModel.ReturnStatus))
                {
                    SbSql.AppendFormat(" AND sb.ReturnStatus in ({0}) ", searchModel.ReturnStatus);
                }
                else
                {
                    SbSql.AppendFormat(" AND sb.ReturnStatus not in ({0},{1},{2},{3}) ", (Int32)Enums.ReturnStatus.RejectedInbounded, (Int32)Enums.ReturnStatus.ReturnExchangeInbounded, (Int32)Enums.ReturnStatus.ReturnInTransit, (Int32)Enums.ReturnStatus.ReturnSignedBounded);
                }
            }
            else
            {
                SbSql.AppendFormat(" AND sb.ReturnStatus in ({0},{1},{2}) ", (Int32)Enums.ReturnStatus.RejectedInbounded, (Int32)Enums.ReturnStatus.ReturnExchangeInbounded, (Int32)Enums.ReturnStatus.ReturnSignedBounded);
            }
            if (!string.IsNullOrEmpty(searchModel.Source))
            {
                SbSql.AppendFormat(" AND sb.Source in ({0}) ", searchModel.Source);
            }
            if (!string.IsNullOrEmpty(searchModel.CurrentDeptName))
            {
                SbSql.Append(" AND srd.CreateDept like :CreateDept ");
                arguments.Add(new OracleParameter() { ParameterName = "CreateDept", OracleDbType = OracleDbType.Varchar2, Size = 50, Value = "%" + searchModel.CurrentDeptName + "%" });
            }
            var dtResult = ExecuteSqlDataTable(TMSWriteConnection, SbSql.ToString(), arguments.ToArray());
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                foreach (DataRow item in dtResult.Rows)
                {
                    if (string.IsNullOrEmpty(FormCodelists))
                    {
                        FormCodelists += item["FormCode"].ToString();
                    }
                    else
                    {
                        FormCodelists += ',' + item["FormCode"].ToString();
                    }
                }
            }
            return FormCodelists;
        }

        /// <summary>
        /// 商家入库确认时修改主单对象
        /// </summary>
        /// <param name="billModel">需要修改为的对象</param>
        /// <returns></returns>
        public int UpdateBillModelMerchantInBound(BillModel billModel)
        {
            if (billModel == null) throw new ArgumentNullException("billModel is null.");
            String sql = @"
UPDATE SC_Bill
SET returnstatus = :returnstatus ,
    UpdateBy = :OpUser ,
    UpdateTime = SYSDATE ,
    UpdateDept = :OpDept 
WHERE
    FormCode = :FormCode ";
            var arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2,Size=50 , Value = billModel.FormCode },
                new OracleParameter() { ParameterName="returnstatus", OracleDbType = OracleDbType.Int16 , Value = (int)billModel.ReturnStatus },
                new OracleParameter() { ParameterName="OpUser", OracleDbType = OracleDbType.Int32,Size=4 , Value = billModel.UpdateBy },
                new OracleParameter() { ParameterName="OpDept", OracleDbType = OracleDbType.Int32,Size=4 , Value = billModel.UpdateDept }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public InboundBillModel GetInboundBillModelV2(string FormCode)
        {
            String sql = @"
SELECT 
    BILL.FormCode
    ,BILL.Status
    ,BILL.BillType
    ,BILL.WarehouseID
    ,BILL.CustomerOrder
    ,BILL.DeliverStationID
    ,BILL.TurnstationKey
    ,BILL.OutBoundKey
    ,BILL.InBoundKey
    ,BILL.CurrentDistributionCode
    ，SIB.DepartureID
    ,CASE WHEN EXISTS(
        SELECT SIQ.IBSID
        FROM SC_InboundQueue SIQ
        WHERE SIQ.FormCode = :FormCode
            AND SIQ.SeqStatus = :SeqStatus
            AND SIQ.IsDeleted=0
        ) THEN 1
    ELSE 0 END AS IsInbounding
    ,CASE WHEN BILL.Source=2 AND (mbi.IsSkipPrintBill=0 OR mbi.IsNeedWeight=1) then
          1
    ELSE 0 END IsJudgePrint
    ,CASE WHEN mbi.isvalidatebill is null or  mbi.isvalidatebill=0 then
          0
    ELSE 1 END IsValidateBill
FROM SC_Bill BILL
JOIN  MerchantBaseInfo mbi ON BILL.Merchantid=mbi.ID
left Join SC_Inbound SIB ON BILL.InBoundKey=SIB.IBID
WHERE BILL.FormCode = :FormCode
    AND BILL.IsDeleted = 0
    AND ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[] 
            {   
                 new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2 , Size = 50 , Value = FormCode },
                 new OracleParameter() { ParameterName = "SeqStatus", OracleDbType = OracleDbType.Int16, Value = (int)Enums.SeqStatus.NoHand }
            };
            var model = ExecuteSqlSingle_ByDataTableReflect<InboundBillModel>(TMSWriteConnection, sql, parameters);
            if (model != null)
            {
               // model.IsFirstInbound = String.IsNullOrWhiteSpace(model.InboundKey);
                return model;
            }
            return null;
        }

        /// <summary>
        /// 修改当前配送商
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="assDistributionCode"></param>
        /// <returns></returns>
        public bool UpdateAssignDistribution(string formCode, string assDistributionCode)
        {
            string sql = "Update sc_bill Set DISTRIBUTIONCODE=:assDistributionCode where FORMCODE=:formCode";

            OracleParameter[] parameters = 
            {
                new OracleParameter(":assDistributionCode", OracleDbType.Varchar2, 100) { Value = assDistributionCode },
                new OracleParameter(":formCode", OracleDbType.Varchar2,50) { Value = formCode }
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters) > 0;
        }

        /// <summary>
        /// 修改配送站点
        /// </summary>
        /// <param name="waybillNo"></param>
        /// <param name="deliverStationId"></param>
        /// <returns></returns>
        public bool UpdateDeliverStation(string formCode, int deliverStationId)
        {
            string sql = "Update sc_bill Set DELIVERSTATIONID=:DeliverStationId where FORMCODE=:formCode";

            OracleParameter[] parameters = 
            {
                new OracleParameter(":DeliverStationId", OracleDbType.Int64) { Value = deliverStationId },
               new OracleParameter(":formCode", OracleDbType.Varchar2,50) { Value = formCode }
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters) > 0;
        }


        public bool UpdateReturnStatus(string formCode, int returnStatus)
        {
            string sql = @"Update sc_bill Set RETURNSTATUS=:returnStatus 
                            where FORMCODE=:formCode";

            OracleParameter[] parameters = 
            {
                new OracleParameter(":returnStatus", OracleDbType.Int64) { Value = returnStatus },
               new OracleParameter(":formCode", OracleDbType.Varchar2,50) { Value = formCode }
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters) > 0;
        }
    }
}
