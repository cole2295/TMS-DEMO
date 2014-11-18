using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Return;
using Oracle.DataAccess.Client;
using Vancl.TMS.IDAL.Sorting.Return;
using System.Data;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.DAL.Oracle.Sorting.Return
{
    /// <summary>
    /// 退货分拣数据实现层
    /// </summary>
    public class BillReturnDetailInfoDAL : BaseDAL, IBillReturnDetailInfoDAL
    {
        /// <summary>
        /// 添加扫描单
        /// </summary>
        /// <param name="model">单实体对象</param>
        /// <returns></returns>
        public int Add(BillReturnDetailInfoModel model)
        {
            if (model == null) throw new ArgumentNullException("BillReturnDetailInfoModel is null");
            string sql =  String.Format(@"
INSERT INTO SC_BillReturnDetailInfo(RBID,FormCode,BoxNo,Weight,CREATEBY,ReturnTo,UpdateBy,CustomerOrder,CreateDept,SyncFlag)
VALUES({0},:FormCode,:BoxNo,:Weight,:CreateBy,:ReturnTo,:UpdateBy,:CustomerOrder,:CreateDept,:SyncFlag)", model.KeyCodeNextValue());
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.FormCode},
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.BoxNo},
                new OracleParameter() { ParameterName="Weight", OracleDbType = OracleDbType.Decimal, Value = model.Weight},
                new OracleParameter() { ParameterName="CreateBy", OracleDbType = OracleDbType.Int32,Size=4, Value = model.CreateBy},
                new OracleParameter() { ParameterName="ReturnTo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.ReturnTo},                
                new OracleParameter() { ParameterName="UpdateBy", OracleDbType = OracleDbType.Int32,Size=4, Value = model.UpdateBy},
                new OracleParameter() { ParameterName="CustomerOrder", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.CustomerOrder}            ,
                new OracleParameter() { ParameterName="CreateDept", OracleDbType = OracleDbType.Varchar2,Size=50, Value = model.CreateDept},            
                new OracleParameter() { ParameterName="SyncFlag", OracleDbType = OracleDbType.Int16 , Value = (int)Enums.SyncStatus.NotYet}            
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 删除运单
        /// </summary>
        /// <param name="FormCodeLists"></param>
        /// <returns></returns>
        public int Delete(string FormCodeLists)
        {
            if (string.IsNullOrEmpty(FormCodeLists)) throw new ArgumentNullException("FormCodeLists is null");
            string sql = string.Format(@"UPDATE SC_BillReturnDetailInfo SET IsDeleted = 1 where FormCode in({0})",FormCodeLists);
            return ExecuteSqlNonQuery(TMSWriteConnection, sql);
        }

        /// <summary>
        /// 判断运单是否已经扫描退货出库
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public bool isExist(string FormCode)
        {
            if (string.IsNullOrEmpty(FormCode)) throw new ArgumentNullException("FormCode is null");
            string sql = @"SELECT COUNT(1) FROM SC_BillReturnDetailInfo Detail WHERE Detail.FormCode=:FormCode AND Detail.IsDeleted = 0";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2,Size=50, Value = FormCode}
            };
            var obj = ExecuteSqlScalar(TMSWriteConnection, sql, parameters);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj) > 0;
            }

            return false;
        }

        /// <summary>
        /// 判断运单是否已经退货分拣称重至分拣中心
        /// </summary>
        /// <param name="FormCode"></param>
        /// <param name="returnTo"></param>
        /// <returns></returns>
        public bool IsReturn(string FormCode, string returnTo,out string returnBoxNo)
        {
            returnBoxNo = "";
            if (string.IsNullOrEmpty(FormCode)) throw new ArgumentNullException("FormCode is null");
            string sql = @"SELECT COUNT(1) AS count,MAX(BOXNO) BoxNO FROM SC_BillReturnDetailInfo Detail WHERE Detail.FormCode=:FormCode AND Detail.IsDeleted = 0
                         AND Detail.ReturnTo=:returnto   ";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2,Size=50, Value = FormCode},
                new OracleParameter() { ParameterName="returnto", OracleDbType = OracleDbType.Varchar2,Size=50, Value = returnTo}
            };
            DataTable dt = ExecuteSqlDataTable(TMSWriteConnection, sql, parameters);
            if (dt != null && dt.Rows.Count>0 && int.Parse(dt.Rows[0]["count"].ToString())>0)
            {
                if (dt.Rows[0]["BoxNo"]!=null && !string.IsNullOrEmpty(dt.Rows[0]["BoxNo"].ToString()))
                {
                    returnBoxNo = dt.Rows[0]["BoxNo"].ToString();
                }
                return true;
            }

            return false;
        }

        #region IBillReturnDetailDAL 成员


        public int UpdateSyncedStatus4Tms2Lms(string BillReturnDetailKey)
        {
            if (String.IsNullOrWhiteSpace(BillReturnDetailKey)) throw new ArgumentNullException("BillReturnDetailKey is null or empty.");
            String sql = @"
UPDATE SC_BillReturnDetailInfo
SET SyncFlag = :SyncFlag , UpdateTime = SYSDATE 
WHERE rbid = :rbid
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SyncStatus.Already },
                new OracleParameter() { ParameterName = "rbid" , OracleDbType = OracleDbType.Varchar2,Size=20, Value = BillReturnDetailKey }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion
        /// <summary>
        /// 获取已经退货装箱的箱号
        /// </summary>
        /// <param name="formCode"></param>
        /// <param name="returnTo"></param>
        /// <returns></returns>
        public string GetBoxNO(string formCode, string returnTo)
        {
            string sql = @"
SELECT BoxNo FROM sc_billreturndetailinfo 
WHERE formCode=:formCode AND ReturnTo=:returnto and IsDeleted = 0
            ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName= "formCode", OracleDbType = OracleDbType.Varchar2,Size=50, Value = formCode },
                new OracleParameter() { ParameterName= "returnto", OracleDbType = OracleDbType.Varchar2,Size=50, Value = returnTo }
            };
            object objStatus = ExecuteSqlScalar(TMSReadOnlyConnection, sql, arguments);
            if (objStatus != null && objStatus != DBNull.Value)
            {
                return Convert.ToString(objStatus);
            }
            return string.Empty;
        }
        /// <summary>
        /// 根据运单号获取运单信息列表
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnDetailInfoModel> GetListByFormCodes(string FormCodes)
        {
            if (string.IsNullOrEmpty(FormCodes)) throw new ArgumentNullException("FormCodes is null");
            string sql = string.Format(@"SELECT RBID,FormCode,BoxNo,Weight,CREATEBY,CREATETIME,ReturnTo,CustomerOrder,CreateDept,rownum as rNow FROM SC_BillReturnDetailInfo Detail
                         WHERE Detail.FormCode in ({0}) AND Detail.IsDeleted = 0  order by Detail.RBID", FormCodes);
            //OracleParameter[] parameters = new OracleParameter[]
            //{
            //    new OracleParameter() { ParameterName="FormCode", DbType = System.Data.DbType.String, Value = FormCodes}
            //};
            IList<BillReturnDetailInfoModel> Resultlists = ExecuteSql_ByDataTableReflect<BillReturnDetailInfoModel>(TMSWriteConnection, sql);
            if (Resultlists != null)
            {
                return Resultlists.ToList();
            }
            return null;
        }

        /// <summary>
        /// 根据箱号获取已经扫描的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnDetailInfoModel> GetListByBoxNo(string boxNo, string createdept)
        {
            if (string.IsNullOrEmpty(boxNo)) throw new ArgumentNullException("boxNo is null");
            string sql = @"SELECT RBID,FormCode,BoxNo,Weight,CREATEBY,CREATETIME,ReturnTo,CustomerOrder,CreateDept,rownum as rNow FROM SC_BillReturnDetailInfo Detail
                          WHERE Detail.BoxNo = :BoxNo and Detail.IsDeleted=0 and Detail.CreateDept=:CreateDept order by rNow";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = boxNo},
                new OracleParameter() { ParameterName="CreateDept", OracleDbType = OracleDbType.Varchar2,Size=50, Value = createdept}
            };
            IList<BillReturnDetailInfoModel> Resultlists = ExecuteSql_ByDataTableReflect<BillReturnDetailInfoModel>(TMSWriteConnection, sql, parameters);
            if (Resultlists != null)
            {
                return Resultlists.ToList();
            }
            return null;
        }
        /// <summary>
        /// 根据箱号获取已经扫描的单号信息
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnDetailInfoModel> GetListByBoxNo(string boxNo)
        {
            if (string.IsNullOrEmpty(boxNo)) throw new ArgumentNullException("boxNo is null");
            string sql = @"SELECT RBID,FormCode,BoxNo,Weight,CREATEBY,CREATETIME,ReturnTo,CustomerOrder,CreateDept,rownum as rNow FROM SC_BillReturnDetailInfo Detail
                          WHERE Detail.BoxNo = :BoxNo and Detail.IsDeleted=0 order by rNow";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="BoxNo", OracleDbType = OracleDbType.Varchar2,Size=50, Value = boxNo}            };
            IList<BillReturnDetailInfoModel> Resultlists = ExecuteSql_ByDataTableReflect<BillReturnDetailInfoModel>(TMSWriteConnection, sql, parameters);
            if (Resultlists != null)
            {
                return Resultlists.ToList();
            }
            return null;
        }

        /// <summary>
        /// 获取箱号信息用于退货交接表打印
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<ReturnBillSortingDetailModel> GetDetailListByBoxNo(string boxNo)
        {
            if (string.IsNullOrEmpty(boxNo) || string.IsNullOrEmpty(boxNo.Trim())) throw new ArgumentNullException("boxNo is null");
            string sql = @"
SELECT WRD.FormCode,WRD.CustomerOrder,BoxNo,WRD.CreateDept,ReturnTo,
WRD.CreateTime,SI.STATUSNAME formType,nvl(WRD.Weight,0) Weight,rownum as rNow
from  SC_BillReturnDetailInfo WRD
LEFT JOIN  SC_bill WB ON WB.FormCode = WRD.FormCode 
LEFT JOIN  StatusInfo SI ON SI.STATUSNO = WB.BillType AND SI.STATUSTYPENO = 2 
WHERE WRD.BoxNo=:BoxNo and WRD.IsDeleted=0
            ";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="BoxNo", OracleDbType= OracleDbType.Varchar2,Size=50, Value= boxNo}
            };
            IList<ReturnBillSortingDetailModel> Resultlists = ExecuteSql_ByDataTableReflect<ReturnBillSortingDetailModel>(TMSWriteConnection, sql, arguments);
            if (Resultlists != null)
            {
                return Resultlists.ToList();
            }
            return null;
        }
         /// <summary>
        /// 装箱打印操作
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        public bool InBoxPrint(string BoxNo)
        {
            return true;
        }

        #region IBillReturnDetailInfo 成员

        /// <summary>
        /// 取得TMS退货分拣称重单号记录需要同步到LMS物流主库的出库实体对象
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public BillReturnDetailInfoModel GetBillReturnDetailIntoModel4TmsSync2Lms(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
SELECT *
FROM
(
    SELECT RBID, FormCode, BoxNo,Weight, CreateBy,CreateTime, ReturnTo, CreateDept,CustomerOrder
    FROM sc_billreturndetailinfo
    WHERE FormCode = :FormCode
    AND SyncFlag = :SyncFlag
    ORDER BY CreateTime ASC
)
WHERE ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SyncStatus.NotYet },
                new OracleParameter() { ParameterName = "FormCode" , OracleDbType = OracleDbType.Varchar2,Size=50, Value = FormCode }
            };
            return ExecuteSqlSingle_ByDataTableReflect<BillReturnDetailInfoModel>(TMSWriteConnection, sql, arguments);
        }
        /// <summary>
        /// 重新设置未同步标志
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public bool UpdateSyncedStatus(string FormCode)
        {
            if (string.IsNullOrEmpty(FormCode)) throw new ArgumentNullException("FormCode is null");
            string sql = @"
UPDATE SC_BillReturnDetailInfo 
SET SyncFlag=:SyncFlag 
where isdeleted = 0 and FormCode =:FormCode";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SyncStatus.NotYet },
                new OracleParameter() { ParameterName = "FormCode" , OracleDbType = OracleDbType.Varchar2,Size=50, Value = FormCode }
            };
            var obj = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj) > 0;
            }

            return false;

        }

                /// <summary>
        /// 查看箱中运单是否返货到同一商家或配送商
        /// </summary>
        /// <param name="returnBoxNo"></param>
        /// <returns></returns>
        public BillReturnCountModel GetReturnToCount(string returnBoxNo)
        {
            string sql = @"
SELECT count(DISTINCT(MerchantId)) MerchantCount ,count(Distinct(DistributionCode)) DistributionCount
FROM  sc_bill sb
JOIN sc_billreturndetailinfo d ON sb.formcode = d.formcode AND sb.isdeleted = 0
JOIN sc_billreturnboxinfo b ON b.boxno = d.boxno AND b.isdeleted = 0
WHERE d.isdeleted =0 AND d.boxno = :BoxNO
            ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName= "BoxNO", OracleDbType = OracleDbType.Varchar2,Size=50, Value = returnBoxNo }
            };
            return ExecuteSqlSingle_ByDataTableReflect<BillReturnCountModel>(TMSWriteConnection, sql, arguments);
        }

        #endregion
        /// <summary>
        /// 查询退货出库后的剩余订单
        /// </summary>
        /// <param name="FormCodes"></param>
        /// <returns></returns>
        public List<BillReturnDetailInfoModel> GetBillAfterOutBound(string FormCodes,string CreateDept)
        {
            if (string.IsNullOrEmpty(FormCodes)) throw new ArgumentNullException("FormCodes is null");
            string sql = string.Format(@"
SELECT RBID,FormCode,BoxNo,Weight,CREATEBY,CREATETIME,ReturnTo,CustomerOrder,CreateDept,rownum as rNow 
FROM SC_BillReturnDetailInfo Detail
WHERE Detail.IsDeleted = 0  AND Createdept = :Createdept AND Detail.FormCode in ({0}) 
order by Detail.RBID", FormCodes);
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="Createdept", DbType = System.Data.DbType.String, Value = CreateDept}
            };
            IList<BillReturnDetailInfoModel> Resultlists = ExecuteSql_ByDataTableReflect<BillReturnDetailInfoModel>(TMSWriteConnection, sql,parameters);
            if (Resultlists != null)
            {
                return Resultlists.ToList();
            }
            return null;
        }

        /// <summary>
        /// 判断是否已经同步完成
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public bool IsSynced(string FormCode)
        {
            if (string.IsNullOrEmpty(FormCode)) throw new ArgumentNullException("FormCode is null");
            string sql = @"
SELECT COUNT(1) FROM sc_billreturndetailinfo
WHERE syncflag = :SyncFlag AND formcode = :FormCode
ORDER BY createtime DESC";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "SyncFlag" , OracleDbType = OracleDbType.Int16, Value = (int)Enums.SyncStatus.NotYet },
                new OracleParameter() { ParameterName = "FormCode" , OracleDbType = OracleDbType.Varchar2,Size=50, Value = FormCode }
            };
            var obj = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj) > 0;
            }

            return false;
        }

    }
}
