using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.BaseInfo.Order;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Transport.DeliveryAbnormal;

namespace Vancl.TMS.DAL.Oracle.BaseInfo
{
    public class BoxDAL : BaseDAL, IBoxDAL
    {
        /// <summary>
        /// 构建箱号CTE SQL字符串
        /// </summary>
        /// <param name="arrBox"></param>
        /// <returns></returns>
        private string BuildingBoxCTEsql(string[] arrBox)
        {
            StringBuilder strBuffer = new StringBuilder();
            string dual = " FROM DUAL ";
            string unionall = " UNION ALL ";
            string select = " SELECT ";
            for (int i = 0; i < arrBox.Length; i++)
            {
                strBuffer.Append(select);
                strBuffer.Append
                    (
                        string.Format(@" '{0}' AS BOXNO "
                        , arrBox[i].Replace("'", "")
                        )
                    );
                strBuffer.Append(dual);
                strBuffer.Append(unionall);
            }
            return strBuffer.ToString().Substring(0, strBuffer.Length - unionall.Length);
        }


        #region IBoxDAL 成员
        public List<OrderModel> GetUnLostOrderList(string[] box)
        {
            string sql = @"
SELECT ord.Oid, ord.FormCode, ord.lmswaybillno,ord.lmswaybilltype,ord.price,ord.departureid,ord.arrivalid,ord.goodstype,box.boxno,ord.ordertmsstatus,ord.ProtectedPrice
FROM TMS_BOX box
JOIN TMS_BoxDetail bd ON bd.BoxNo = box.BoxNo
JOIN TMS_ORDER ord ON bd.FormCode = ord.FormCode AND ord.ordertmsstatus = 0
JOIN
(
    SELECT REGEXP_SUBSTR(:listBatchNoStr, '[^,]+', 1, LEVEL) AS BatchNo
    FROM DUAL
    CONNECT BY LEVEL <=
    LENGTH(TRIM(TRANSLATE(:listBatchNoStr,TRANSLATE(:listBatchNoStr, ',', ' '), ' '))) + 1
) tmpBatchNo ON box.BoxNo = tmpBatchNo.BatchNo
WHERE box.IsDeleted = 0 AND ord.isdeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "listBatchNoStr", DbType = DbType.String, Value = String.Join(",",box) }
            };
            IList<OrderModel> ilist = ExecuteSql_ByDataTableReflect<OrderModel>(TMSWriteConnection, sql, parameters);
            if (ilist != null)
            {
                return ilist.ToList();
            }
            return null;
        }

        public int UpdatePreDispatched(string boxNos)
        {
            string strSql = string.Format(@"
                UPDATE TMS_Box
                SET IsPreDispatch=1
                    ,UpdateTime=sysdate
                WHERE IsDeleted=0
                    AND BoxNo IN ({0})"
                , boxNos);
            return ExecuteSqlNonQuery(TMSWriteConnection, strSql);
        }
        #endregion

        #region IBoxDAL 成员

        /// <summary>
        /// 是否存在系统批次数据对接信息不全
        /// </summary>
        /// <param name="listBatchNo">系统批次号列表</param>
        /// <returns></returns>
        public bool IsExistsBatchDockingFailed(List<String> listBatchNo)
        {
            if (listBatchNo == null) throw new ArgumentNullException("BatchNo is null or empty");
            if (listBatchNo.Count <= 0) return false;
            String sql = @"
SELECT 1
FROM TMS_Box box
JOIN
(
    SELECT  boxdetail.BoxNo, COUNT(*) AS TotalCount
    FROM TMS_BoxDetail boxdetail
    JOIN
    (
        SELECT REGEXP_SUBSTR(:listBatchNoStr, '[^,]+', 1, LEVEL) AS BatchNo
        FROM DUAL
        CONNECT BY LEVEL <=
        LENGTH(TRIM(TRANSLATE(:listBatchNoStr,TRANSLATE(:listBatchNoStr, ',', ' '), ' '))) + 1
    ) tmpBatchNo ON boxdetail.BoxNo = tmpBatchNo.BatchNo
    GROUP BY  boxdetail.BoxNo
) tmpStatistic ON box.BoxNo = tmpStatistic.BoxNo AND box.TotalCount <> tmpStatistic.TotalCount
WHERE ROWNUM = 1
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "listBatchNoStr", DbType = DbType.String, Value = String.Join(",",listBatchNo) }
            };
            object objValue = ExecuteSqlScalar(TMSWriteConnection, sql, parameters);
            if (objValue != null && objValue != DBNull.Value)
            {
                return objValue.ToString().Equals("1");
            }
            return false;
        }

        /// <summary>
        /// 是否系统批次号存在
        /// </summary>
        /// <param name="BatchNo">系统批次号</param>
        /// <returns></returns>
        public bool IsBoxNoExists(String BatchNo)
        {
            if (String.IsNullOrWhiteSpace(BatchNo)) throw new ArgumentNullException("BatchNo is null or empty");
            String sql = @"
SELECT 1
FROM TMS_Box
WHERE BoxNo = :BatchNo
AND ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="BatchNo", DbType = DbType.String, Value = BatchNo}
            };
            object objValue = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return objValue.ToString() == "1";
            }
            return false;
        }

        /// <summary>
        /// 是否来源，客户批次号存在
        /// </summary>
        /// <param name="BatchNo"></param>
        /// <param name="Source"></param>
        /// <returns></returns>
        public bool IsBoxNoExists(String BatchNo, Enums.TMSEntranceSource Source)
        {
            if (String.IsNullOrWhiteSpace(BatchNo)) throw new ArgumentNullException("BatchNo is null or empty");
            String sql = @"
SELECT 1
FROM TMS_Box
WHERE CustomerBatchNo = :CustomerBatchNo
AND Source = :Source
AND ROWNUM = 1
";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="CustomerBatchNo", DbType = DbType.String, Value = BatchNo},
                new OracleParameter() { ParameterName="Source", DbType = DbType.Int16, Value = (int)Source}
            };
            object objValue = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (objValue != null && objValue != DBNull.Value)
            {
                return objValue.ToString() == "1";
            }
            return false;
        }

        public int Add(BoxModel model)
        {
            if (model == null) throw new ArgumentNullException("BoxModel is null");
            String sql = String.Format(@"
MERGE INTO TMS_Box  des
USING 
(
    SELECT :BoxNo AS BoxNo , :Source AS Source, :CustomerBatchNo AS CustomerBatchNo , :DepartureID AS  DepartureID , :ArrivalID AS ArrivalID ,
    :TotalCount AS TotalCount  , :TotalAmount AS TotalAmount , :ProtectedPrice AS ProtectedPrice , :Weight AS Weight , :BoxType AS BoxType ,
    :ContentType AS ContentType , :CreateBy AS CreateBy  , :UpdateBy AS UpdateBy
   FROM dual
)  src
ON ( des.Source = src.Source AND des.CustomerBatchNo = src.CustomerBatchNo )
WHEN MATCHED THEN 
    UPDATE SET des.UpdateBy = src.UpdateBy  ,  des.UpdateTime = SYSDATE
WHEN NOT MATCHED THEN
INSERT (BID, BoxNo, Source, CustomerBatchNo, DepartureID ,ArrivalID ,TotalCount, TotalAmount, ProtectedPrice,Weight, BoxType, ContentType , CreateBy, UpdateBy )
VALUES({0}, src.BoxNo, src.Source , src.CustomerBatchNo, src.DepartureID ,src.ArrivalID, src.TotalCount, src.TotalAmount ,src.ProtectedPrice ,src.Weight ,src.BoxType ,src.ContentType ,src.CreateBy , src.UpdateBy )
", model.SequenceNextValue());
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                new OracleParameter(){ ParameterName="BoxNo", DbType = System.Data.DbType.String, Value = model.BoxNo},
                new OracleParameter(){ ParameterName="Source", DbType = System.Data.DbType.Int32, Value = (int)model.Source},
                new OracleParameter(){ ParameterName="CustomerBatchNo", DbType = System.Data.DbType.String, Value = model.CustomerBatchNo},
                new OracleParameter(){ ParameterName="DepartureID", DbType= System.Data.DbType.Int32, Value = model.DepartureID},
                new OracleParameter(){ ParameterName="ArrivalID", DbType = System.Data.DbType.Int32, Value = model.ArrivalID},
                new OracleParameter(){ ParameterName="TotalCount", DbType= System.Data.DbType.Int32,Value = model.TotalCount},
                new OracleParameter(){ ParameterName="TotalAmount", DbType= System.Data.DbType.Decimal, Value = model.TotalAmount},
                new OracleParameter(){ ParameterName="ProtectedPrice", DbType= System.Data.DbType.Decimal, Value = model.ProtectedPrice},
                new OracleParameter(){ParameterName="Weight", DbType= System.Data.DbType.Decimal, Value = model.Weight},
                new OracleParameter(){ParameterName="BoxType", DbType= System.Data.DbType.Int16, Value = (int)model.BoxType},
                new OracleParameter(){ParameterName="ContentType",DbType = System.Data.DbType.Int32,Value = (int)model.ContentType},
                new OracleParameter(){ParameterName="CreateBy", DbType= System.Data.DbType.Int32, Value =  model.CreateBy},
                new OracleParameter(){ParameterName="UpdateBy", DbType= System.Data.DbType.Int32, Value =  model.UpdateBy}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        public int AddBoxDetail(List<BoxDetailModel> model)
        {
            if (model == null) throw new ArgumentNullException("model is null");
            if(model.Count <= 0 ) throw new ArgumentNullException("model.count <= 0");
            String sql = String.Format( @"
INSERT INTO  TMS_BoxDetail (BDID, BoxNo, FormCode, CreateBy, UpdateBy)
VALUES({0}, :BoxNo, :FormCode,:CreateBy ,:UpdateBy )
",
 model[0].SequenceNextValue()
 );
            String[] arrBoxNo = new String[model.Count];
            String[] arrFormCode = new String[model.Count];
            int[] arrCreateBy = new int[model.Count];
            int[] arrUpdateBy = new int[model.Count];
            int nPos = 0;
            model.ForEach(p =>
            {
                arrBoxNo[nPos] = p.BoxNo;
                arrFormCode[nPos] = p.FormCode;
                arrCreateBy[nPos] = p.CreateBy;
                arrUpdateBy[nPos] = p.UpdateBy;
                nPos++;
            });
            OracleParameter[] arguments = new OracleParameter[] { 
                new OracleParameter(){ ParameterName="BoxNo", DbType = System.Data.DbType.String, Value = arrBoxNo},
                new OracleParameter(){ ParameterName="FormCode", DbType = System.Data.DbType.String, Value = arrFormCode},
                new OracleParameter(){ParameterName="CreateBy", DbType= System.Data.DbType.Int32, Value = arrCreateBy },
                new OracleParameter(){ParameterName="UpdateBy", DbType= System.Data.DbType.Int32, Value =  arrUpdateBy}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, model.Count, arguments);
        }


        public int UpdatePreDispatched(List<long> listBID)
        {
            if (listBID == null || listBID.Count <= 0) return 0;
            String strSql = @"
UPDATE TMS_Box
SET IsPreDispatch = :afterStatus ,UpdateTime = sysdate
WHERE BID = :BID
    AND (IsPreDispatch = :preStatus OR IsPreDispatch=:preStatus1)
    AND IsDeleted = 0
";
            int[] arrpreStatus = new int[listBID.Count];
            int[] arrpreStatus1 = new int[listBID.Count];
            int[] arrafterStatus = new int[listBID.Count];
            for (int i = 0; i < listBID.Count; i++)
            {
                arrpreStatus[i] = (int)Enums.BatchPreDispatchedStatus.NoDispatched;
                arrpreStatus1[i] = (int)Enums.BatchPreDispatchedStatus.DispatchedError;
                arrafterStatus[i] = (int)Enums.BatchPreDispatchedStatus.IsDispatched;
            }
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BID", DbType = DbType.Int64, Value = listBID.ToArray() },
                new OracleParameter() { ParameterName = "preStatus", DbType = DbType.Int32, Value = arrpreStatus },
                new OracleParameter() { ParameterName = "preStatus1", DbType = DbType.Int32, Value = arrpreStatus1 },
                new OracleParameter() { ParameterName = "afterStatus", DbType = DbType.Int32, Value = arrafterStatus },
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, listBID.Count, parameters);
        }

        public int UpdatePreDispatchedError(List<long> listBID)
        {
            if (listBID == null || listBID.Count <= 0) return 0;
            String strSql = @"
UPDATE TMS_Box
SET IsPreDispatch = :afterStatus ,UpdateTime = sysdate
WHERE BID = :BID
    AND IsPreDispatch = :preStatus
    AND IsDeleted = 0
";
            int[] arrpreStatus = new int[listBID.Count];
            int[] arrafterStatus = new int[listBID.Count];
            for (int i = 0; i < listBID.Count; i++)
            {
                arrpreStatus[i] = (int)Enums.BatchPreDispatchedStatus.NoDispatched;
                arrafterStatus[i] = (int)Enums.BatchPreDispatchedStatus.DispatchedError;
            }
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BID", DbType = DbType.Int64, Value = listBID.ToArray() },
                new OracleParameter() { ParameterName = "preStatus", DbType = DbType.Int32, Value = arrpreStatus },
                new OracleParameter() { ParameterName = "afterStatus", DbType = DbType.Int32, Value = arrafterStatus },
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, listBID.Count, parameters);
        }

        /// <summary>
        /// 把预调度错误的更新为未调度状态
        /// </summary>
        /// <param name="listBID"></param>
        /// <returns></returns>
        public int UpdateNoPreDispatched_ByRePreDispatch(List<long> listBID)
        {
            if (listBID == null || listBID.Count <= 0) return 0;
            String strSql = @"
UPDATE TMS_Box
SET IsPreDispatch = :afterStatus ,UpdateTime = sysdate
WHERE BID = :BID
    AND IsPreDispatch = :preStatus
    AND IsDeleted = 0
";
            int[] arrpreStatus = new int[listBID.Count];
            int[] arrafterStatus = new int[listBID.Count];
            for (int i = 0; i < listBID.Count; i++)
            {
                arrpreStatus[i] = (int)Enums.BatchPreDispatchedStatus.DispatchedError;
                arrafterStatus[i] = (int)Enums.BatchPreDispatchedStatus.NoDispatched;
            }
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "BID", DbType = DbType.Int64, Value = listBID.ToArray() },
                new OracleParameter() { ParameterName = "preStatus", DbType = DbType.Int32, Value = arrpreStatus },
                new OracleParameter() { ParameterName = "afterStatus", DbType = DbType.Int32, Value = arrafterStatus },
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, listBID.Count, parameters);
        }


        public List<BoxModel> GetBatchInfo(string[] arrBatchNo)
        {
            if (arrBatchNo == null) throw new ArgumentNullException("arrBatchNo is null.");
            if (arrBatchNo.Length > 0)
            {
                String sql = @"
SELECT box.BoxNo, box.TotalCount, box.TotalAmount, box.Weight
FROM TMS_Box box
JOIN
(
    SELECT REGEXP_SUBSTR(:listBatchNoStr, '[^,]+', 1, LEVEL) AS BatchNo
    FROM DUAL
    CONNECT BY LEVEL <=
    LENGTH(TRIM(TRANSLATE(:listBatchNoStr,TRANSLATE(:listBatchNoStr, ',', ' '), ' '))) + 1
) tmpBatch ON box.BoxNo = tmpBatch.BatchNo
WHERE box.IsDeleted = 0
";
                OracleParameter[] parameters = new OracleParameter[] 
                {
                    new OracleParameter() { ParameterName = "listBatchNoStr", DbType = DbType.String, Value = String.Join(",",arrBatchNo) }
                };
                var listResult = ExecuteSql_ByDataTableReflect<BoxModel>(TMSWriteConnection, sql, parameters);
                if (listResult != null)
                {
                    return listResult.ToList();
                }
            }
            return null;
        }

        #endregion

        #region IBoxDAL 成员

        public void UpdateBatchTotalAmount(Dictionary<string, decimal> listAmount)
        {
            if (listAmount == null) throw new ArgumentNullException("listAmount is null");
            if (listAmount.Count <= 0) return;
            String sql = @"
UPDATE TMS_BOX
SET TotalAmount = :TotalAmount
WHERE boxno = :boxno
    AND IsDeleted = 0
";
            String[] arrBatchNo = new String[listAmount.Count];
            Decimal[] arrTotalAmount = new Decimal[listAmount.Count];
            for (int i = 0; i < listAmount.Count; i++)
            {
                arrBatchNo[i] = listAmount.Keys.ElementAt(i);
                arrTotalAmount[i] = listAmount.Values.ElementAt(i);
            }
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "TotalAmount", DbType = DbType.Decimal, Value = arrTotalAmount },
                new OracleParameter() { ParameterName = "boxno", DbType = DbType.String, Value = arrBatchNo }
            };
            ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listAmount.Count, parameters);
        }

        #endregion


        public PagedList<PreDispatchAbnormalModel> GetPreDispatchAbnormalList(PreDispatchAbnormalSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException("PreDispatchAbnormalSearchModel is null ");

            if(!searchModel.BoxTimeStart.HasValue)
                throw new ArgumentException("PreDispatchAbnormalSearchModel BoxTimeStart is null ");

            if (!searchModel.BoxTimeEnd.HasValue)
                throw new ArgumentException("PreDispatchAbnormalSearchModel BoxTimeEnd is null ");

            String sql = @"select ROWNUM AS SerialNumber,
                                TB.BID,
                                TB.BOXNO,
                                TB.CUSTOMERBATCHNO,TB.DEPARTUREID,TB.ARRIVALID,
                                ec.companyname as DEPARTURENAME,
                                ec1.companyname as ARRIVALNAME,
                                TB.CONTENTTYPE,
                                TB.BOXTYPE,
                                TB.CREATETIME 
                            from TMS_BOX TB
                            join expresscompany ec on ec.expresscompanyid=TB.DEPARTUREID
                            join expresscompany ec1 on ec1.expresscompanyid=TB.ARRIVALID
                            WHERE TB.ISDELETED=0 {0}";
            StringBuilder sbWhere = new StringBuilder();
            List<OracleParameter> parameterList = new List<OracleParameter>();

            sbWhere.Append(" AND TB.ISPREDISPATCH=:ISPREDISPATCH ");
            parameterList.Add(new OracleParameter(":ISPREDISPATCH", OracleDbType.Decimal) { Value=(int)(Enums.BatchPreDispatchedStatus)searchModel.IsPreDispatch});

            sbWhere.Append(" AND TB.Createtime>=:CreatetimeStart ");
            parameterList.Add(new OracleParameter(":CreatetimeStart", OracleDbType.Date) { Value = searchModel.BoxTimeStart.Value });

            sbWhere.Append(" AND TB.Createtime<=:CreatetimeEnd ");
            parameterList.Add(new OracleParameter(":CreatetimeEnd", OracleDbType.Date) { Value = searchModel.BoxTimeEnd.Value });

            sql = string.Format(sql,sbWhere.ToString());
            
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<PreDispatchAbnormalModel>(TMSReadOnlyConnection, sql, searchModel, parameterList.ToArray());
        }
    }
}
