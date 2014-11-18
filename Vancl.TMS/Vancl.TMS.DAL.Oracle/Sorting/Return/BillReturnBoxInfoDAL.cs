using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Return;
using Vancl.TMS.Model.Sorting.Return;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Sorting.Return
{

    /// <summary>
    /// 退货分拣数据实现层
    /// </summary>
    public class BillReturnBoxInfoDAL : BaseDAL, IBillReturnBoxInfoDAL
    {
        /// <summary>
        /// 添加扫描箱号信息
        /// </summary>
        /// <param name="model">单实体对象</param>
        /// <returns></returns>
        public int Add(BillReturnBoxInfoModel model)
        {
            if (model == null) throw new ArgumentNullException("BillReturnBoxInfoModel is null");
            string sql = string.Format(@"
INSERT INTO SC_BillReturnBoxInfo(RBoxID,BoxNo,Weight,CREATEBY,ReturnTo,CreateDept,UpdateBy,ReturnMerchant,SyncFlag)
VALUES({0},:BoxNo,:Weight,:CreateBy,:ReturnTo,:CreateDept,:UpdateBy,:ReturnMerchant,:SyncFlag)", model.KeyCodeNextValue());
            OracleParameter[] parameters = new OracleParameter[]
            {
                //new OracleParameter() { ParameterName="RBoxID", DbType = System.Data.DbType.String, Value = model.RBoxID},
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.BoxNo},
                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal, Value = model.Weight},
                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32,Size=4, Value = model.CreateBy},
                new OracleParameter() { ParameterName="ReturnTo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.ReturnTo},                
                new OracleParameter() { ParameterName="CreateDept", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.CreateDept},
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32,Size=4, Value = model.UpdateBy},
                new OracleParameter() { ParameterName="ReturnMerchant", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.ReturnMerchant},
                new OracleParameter() { ParameterName="SyncFlag", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SyncStatus.NotYet}            
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 根据箱号获取箱号信息
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public BillReturnBoxInfoModel GetBoxInfoByBoxNo(string BoxNo)
        {
            if (string.IsNullOrEmpty(BoxNo)) throw new ArgumentNullException("BoxNo is null");
            string sql = @"
select RBOXID,BoxNo,CreateBy,CreateTime,IsDeleted,CreateDept,ReturnMerchant,ReturnTo,Weight,
IsPrintBackPacking,IsPrintBackForm,BillNum,rownum as rNow
from (
SELECT distinct RBOXID,box.BoxNo,box.CreateBy,box.CreateTime,box.IsDeleted,box.CreateDept,box.ReturnMerchant,box.ReturnTo,box.Weight
,box.IsPrintBackPacking,box.IsPrintBackForm,
(SELECT COUNT(1) FROM  sc_billreturndetailinfo Detail where Detail.boxno=box.boxno and Detail.isdeleted=0
) as BillNum
FROM sc_billreturnboxinfo box 
JOIN sc_billreturndetailinfo Detail1 ON Detail1.IsDeleted=0 
WHERE box.IsDeleted = 0 AND box.BoxNo = :BoxNo
)";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = BoxNo.Trim()}           
            };
            return ExecuteSqlSingle_ByReaderReflect<BillReturnBoxInfoModel>(TMSReadOnlyConnection, sql, parameters);      
        }

        /// <summary>
        /// 更新装箱称重重量
        /// </summary>
        /// <param name="boxNos"></param>
        /// <returns></returns>
        public int UpdateBoxWeight(decimal Weight,string BoxNo,int UpdateBy)
        {
            if (string.IsNullOrEmpty(BoxNo)) throw new ArgumentNullException("BoxNo is null");
            string sql = @"
UPDATE  SC_BillReturnBoxInfo 
SET Weight=:Weight,
    UpdateBy =:UpdateBy,
    SYNCFLAG=:SYNCFLAG,
    UpdateTime =sysdate 
WHERE IsDeleted = 0 AND BoxNo = :BoxNo";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal, Value = Weight},          
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32, Value = UpdateBy},           
                new OracleParameter() { ParameterName="SYNCFLAG", OracleDbType = OracleDbType.Int16, Value = (Int16)Enums.SyncStatus.NotYet},           
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = BoxNo}           
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 获取最大的箱号
        /// </summary>
        /// <param name="boxNoHead"></param>
        /// <returns></returns>
        public string GetMaxBoxNO(string boxNoHead)
        {
            string sql = string.Format(@"select max(BoxNo) from SC_BillReturnBoxInfo where BoxNo like '{0}%'", boxNoHead);
            object objStatus = ExecuteSqlScalar(TMSReadOnlyConnection, sql);
            if (objStatus != null && objStatus != DBNull.Value)
            {
                return Convert.ToString(objStatus);
            }
            return string.Empty;
        }

        /// <summary>
        /// 是否已经装箱打印
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool IsInBoxPrint(string BoxNo)
        {
            if (string.IsNullOrEmpty(BoxNo)) throw new ArgumentNullException("BoxNo is null");
            string sql = @"SELECT ISPRINTBACKPACKING FROM SC_BillReturnBoxInfo box WHERE box.BoxNo =:BoxNo AND box.IsDeleted =0";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = BoxNo}           
            };
            object objStatus = ExecuteSqlScalar(TMSReadOnlyConnection, sql,parameters);
            if (objStatus != null && objStatus != DBNull.Value)
            {
                return Convert.ToInt32(objStatus)==0?false:true;
            }
            return false;
        }
        /// <summary>
        /// 条件查询已经退货出库的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnBoxInfoModel> GetReturnDetailList(string returnto, string startDate, string endDate, string formType, string code, string currentDeptName)
        {
            StringBuilder sql = new StringBuilder();

            IList<OracleParameter> argumentsList = new List<OracleParameter>();
            sql.Append(@"
select RBOXID,BoxNo,CreateBy,CreateTime,IsDeleted,CreateDept,ReturnMerchant,ReturnTo,Weight,
IsPrintBackPacking,IsPrintBackForm,BillNum,CurrentDeptName,rownum as rNow
from (
SELECT distinct RBOXID,box.BoxNo,box.CreateBy,box.CreateTime,box.IsDeleted,box.CreateDept,box.ReturnMerchant,box.ReturnTo,box.Weight
,box.IsPrintBackPacking,box.IsPrintBackForm,
(SELECT COUNT(1) FROM  sc_billreturndetailinfo Detail where Detail.boxno=box.boxno and Detail.isdeleted = 0
) as BillNum,:currentDeptName as CurrentDeptName
FROM sc_billreturnboxinfo box 
JOIN sc_billreturndetailinfo Detail1 ON Detail1.IsDeleted=0 ");
            if (!string.IsNullOrEmpty(code))
            {
                if (formType == "0")
                {
                    sql.Append(" WHERE box.IsDeleted = 0 AND box.BoxNo=:BoxNo ");
                    argumentsList.Add(new OracleParameter() { ParameterName = "BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = code });
                }
                if (formType == "1")
                {
                    sql.Append(" AND Detail1.FormCode = :FormCode and Detail1.IsDeleted = 0 and box.BoxNo = Detail1.BoxNo ");
                    sql.Append(" WHERE box.IsDeleted = 0 ");
                    argumentsList.Add(new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2,Size=50, Value = code });
                }
                if (formType == "-1")
                {
                    sql.Append(" WHERE box.IsDeleted = 0 ");
                }
            }
            else
            {
                sql.Append(" WHERE box.IsDeleted = 0  ");
            }
            argumentsList.Add(new OracleParameter() { ParameterName = "currentDeptName", OracleDbType = OracleDbType.Varchar2,Size=100, Value = currentDeptName });
            if (!string.IsNullOrEmpty(returnto))
            {
                sql.Append(" AND box.ReturnTo=:returnto ");
                argumentsList.Add(new OracleParameter() { ParameterName = "returnto", OracleDbType = OracleDbType.Varchar2,Size=50, Value = returnto.Trim() });
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                //sql.Append(string.Format(" AND box.CreateTime>= to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", startDate.Trim()));
                sql.Append(" AND box.CreateTime>= :startDate");
                argumentsList.Add(new OracleParameter() { ParameterName = "startDate", OracleDbType = OracleDbType.Date, Value = DateTime.Parse(startDate.Trim()) });
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                //sql.Append(string.Format(" AND box.CreateTime<=to_date('{0}','yyyy-mm-dd hh24:mi:ss') ", endDate.Trim()));
                sql.Append(" AND box.CreateTime<= :endDate");
                argumentsList.Add(new OracleParameter() { ParameterName = "endDate", OracleDbType = OracleDbType.Date, Value = DateTime.Parse(endDate.Trim()) });
            }
            sql.Append(')');
            IList<BillReturnBoxInfoModel> Resultlists = ExecuteSql_ByDataTableReflect<BillReturnBoxInfoModel>(TMSReadOnlyConnection, sql.ToString(), argumentsList.ToArray());
            if (Resultlists != null)
            {
                return Resultlists.ToList();
            }
            return null;
        }

        /// <summary>
        /// 更新箱号状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int UpdateBoxStatus(string BoxNo, Enums.ReturnStatus status)
        {
            string sql = @"
UPDATE sc_billreturnboxinfo
SET BoxStatus=:status
WHERE IsDeleted = 0 AND BoxNo =:BoxNo
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="status", OracleDbType = OracleDbType.Int32,Size=4, Value = (Int16)status},           
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = BoxNo}           
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }
        /// <summary>
        /// 更新装箱打印状态
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool UpdateIsPrintBackPacking(string BoxNo, decimal Weight)
        {
            string sql = @"
UPDATE sc_billreturnboxinfo
SET isprintbackpacking=1,
    Weight = :Weight,
    SyncFlag = :SyncFlag
WHERE IsDeleted = 0 AND BoxNo =:BoxNo
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="SyncFlag", OracleDbType = OracleDbType.Int16, Value = (Int16)Enums.SyncStatus.NotYet},           
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = BoxNo},           
                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal, Value = Weight}           
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters)>0;
        }

        /// <summary>
        /// 更新箱号交接表打印状态
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool UpdateBoxIsPrintBackForm(string BoxNo)
        {
            string sql = @"
UPDATE sc_billreturnboxinfo
SET IsPrintBackForm=1,
     SyncFlag=:SyncFlag
WHERE IsDeleted = 0 AND BoxNo =:BoxNo
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="SyncFlag", OracleDbType = OracleDbType.Int16, Value = (Int16)Enums.SyncStatus.NotYet},           
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = BoxNo}           
            };

            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters) > 0;
        }

        /// <summary>
        /// 取得TMS退货分拣称重箱号记录需要同步到LMS物流主库的出库实体对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public BillReturnBoxInfoModel GetBillReturnBoxIntoModel4TmsSync2Lms(String BoxNo)
        {
             if (String.IsNullOrWhiteSpace(BoxNo)) throw new ArgumentNullException("BoxNo is null or empty.");
            String sql = @"
SELECT *
FROM
(
  SELECT rboxid, BoxNo,Weight,CreateBy,CreateTime,UPDATEBY,updatetime, returnto,isprintbackpacking,isprintbackform,
  createdept,returnmerchant,isdeleted
    FROM sc_billreturnboxinfo
    WHERE BoxNo = :BoxNo
    AND SyncFlag = :SyncFlag
    ORDER BY CreateTime ASC
)
WHERE ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BoxNo" , OracleDbType = OracleDbType.Varchar2,Size=50, Value = BoxNo },
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SyncStatus.NotYet }
            };
            return ExecuteSqlSingle_ByDataTableReflect<BillReturnBoxInfoModel>(TMSReadOnlyConnection, sql, arguments);
        }

        /// <summary>
        /// 更新同步标识[TMS到LMS]
        /// </summary>
        /// <param name="outboundKey">Key</param>
        /// <returns></returns>
        public int UpdateSyncedStatus4Tms2Lms(String BillReturnBoxKey)
        {
            if (String.IsNullOrWhiteSpace(BillReturnBoxKey)) throw new ArgumentNullException("BillReturnBoxKey is null or empty.");
            String sql = @"
UPDATE sc_billreturnboxinfo
SET SyncFlag = :SyncFlag , UpdateTime = SYSDATE 
WHERE RBOXID = :rbid
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SyncStatus.Already },
                new OracleParameter() { ParameterName = "rbid" , OracleDbType = OracleDbType.Varchar2,Size=20, Value = BillReturnBoxKey }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 是否已经在该部门装箱称重
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool IsInBoxWeight(string BoxNo, int CreateBy)
        {
            if (string.IsNullOrEmpty(BoxNo)) throw new ArgumentNullException("BoxNo is null");
            string sql = @"
SELECT COUNT(1) FROM SC_BillReturnBoxInfo box 
WHERE box.BoxNo =:BoxNO
AND CREATEBY = :CreateBy
AND box.IsDeleted =0
";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="BoxNO", OracleDbType = OracleDbType.Varchar2,Size=50, Value = BoxNo} ,          
                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32,Size=4, Value = CreateBy}           
            };
            var obj = ExecuteSqlScalar(TMSReadOnlyConnection, sql, parameters);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj) > 0;
            }

            return false;

        }
                /// <summary>
        /// 查看箱号的当前操作地
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public DataTable GetCreateDept(string BoxNo)
        {
            string sql = string.Format(@"
SELECT DISTINCT(br.createdept) as CreateDept from sc_billreturndetailinfo br
JOIN Sc_Billreturnboxinfo bx ON bx.boxno = br.boxno 
WHERE bx.Isdeleted = 0
AND bx.boxno ='{0}'", BoxNo);
            DataTable  dt = ExecuteSqlDataTable(TMSReadOnlyConnection, sql);
            return dt;
        }

    }
}
