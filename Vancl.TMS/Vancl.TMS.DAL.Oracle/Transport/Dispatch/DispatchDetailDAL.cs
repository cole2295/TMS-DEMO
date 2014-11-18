using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Transport.Dispatch;
using Vancl.TMS.Model.Transport.Dispatch;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.Transport.Dispatch
{
    public class DispatchDetailDAL : BaseDAL, IDispatchDetailDAL
    {
        #region IDispatchDetailDAL 成员

        public void Add(Model.Transport.Dispatch.DispatchDetailModel model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 添加提货单箱明细
        /// </summary>
        /// <param name="model"></param>
        public void Add(List<DispatchDetailModel> listmodel)
        {
            string sql = @"
INSERT INTO TMS_DispatchDetail(DDID,  DID, BoxNo, PDID, IsPlan, CreateBy, UpdateBy, OrderCount, DeliveryNo, TotalAmount, ProtectedPrice )
VALUES(:DDID,  :DID, :BoxNo, :PDID, :IsPlan, :CreateBy, :UpdateBy, :OrderCount, :DeliveryNo, :TotalAmount, :ProtectedPrice )
";
            long[] arrDDID = new long[listmodel.Count];
            long[] arrDID = new long[listmodel.Count];
            long?[] arrPDID = new long?[listmodel.Count];
            string[] arrBoxNo = new string[listmodel.Count];
            bool[] arrIsPlan = new bool[listmodel.Count];
            int[] arrCreateBy = new int[listmodel.Count];
            int[] arrUpdateBy = new int[listmodel.Count];
            string[] arrDeliveryNO = new string[listmodel.Count];
            int[] arrOrderCount = new int[listmodel.Count];
            decimal[] arrTotalAmount = new decimal[listmodel.Count];
            decimal[] arrProtectedPrice = new decimal[listmodel.Count];
            int pos = 0;                        //index
            listmodel.ForEach(p =>
            {
                arrDDID[pos] = p.DDID;
                arrBoxNo[pos] = p.BoxNo;
                arrPDID[pos] = p.PDID;      //.HasValue ? p.PDID.Value: -1;
                arrDID[pos] = p.DID;
                arrIsPlan[pos] = p.IsPlan;
                arrCreateBy[pos] = p.CreateBy;
                arrUpdateBy[pos] = p.UpdateBy;
                arrDeliveryNO[pos] = p.DeliveryNo;
                arrOrderCount[pos] = p.OrderCount;
                arrTotalAmount[pos] = p.TotalAmount;
                arrProtectedPrice[pos] = p.ProtectedPrice;
                pos++;
            });
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter(){  ParameterName="DDID", DbType = DbType.Int64, Value = arrDDID},
                new OracleParameter(){ ParameterName="BoxNo", DbType=DbType.String, Value= arrBoxNo},
                new OracleParameter() { ParameterName="PDID", DbType= DbType.Int64, Value= arrPDID},
                new OracleParameter() { ParameterName="DID", DbType= DbType.Int64, Value= arrDID},
                new OracleParameter() { ParameterName="IsPlan", DbType= DbType.Byte, Value=  arrIsPlan},
                new OracleParameter() { ParameterName="CreateBy", DbType= DbType.Int32, Value=  arrCreateBy},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value=  arrUpdateBy},
                new OracleParameter() { ParameterName="DeliveryNO", DbType= DbType.String, Value=  arrDeliveryNO},
                new OracleParameter() { ParameterName="OrderCount", DbType = System.Data.DbType.Int32, Value = arrOrderCount},
                new OracleParameter() { ParameterName="TotalAmount", DbType= DbType.Decimal, Value=  arrTotalAmount},
                new OracleParameter() { ParameterName="ProtectedPrice", DbType= DbType.Decimal, Value= arrProtectedPrice}
            };
            ExecuteSqlArrayNonQuery(TMSWriteConnection, sql, listmodel.Count, arguments);
        }


        #endregion

        #region IDispatchDetailDAL 成员


        public int Delete(string DeliveryNo)
        {
            string sql = @"
UPDATE TMS_DispatchDetail
SET IsDeleted = 1  ,UpdateTime = sysdate, UpdateBy = :UpdateBy
WHERE DeliveryNo = :DeliveryNo AND  IsDeleted = 0
            ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="DeliveryNo",DbType= DbType.String,Value = DeliveryNo},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        /// <summary>
        /// 根据运输调度主键逻辑删除明细信息
        /// </summary>
        /// <param name="DID">运输调度主键</param>
        /// <returns></returns>
        public int Delete(long DID)
        {
            string sql = @"
UPDATE TMS_DispatchDetail
SET IsDeleted = 1  ,UpdateTime = sysdate, UpdateBy = :UpdateBy
WHERE DID = :DID AND  IsDeleted = 0
            ";
            OracleParameter[] arguments = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName="DID",DbType = DbType.Int64,Value = DID},
                new OracleParameter() { ParameterName="UpdateBy", DbType= DbType.Int32, Value = UserContext.CurrentUser.ID}
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, arguments);
        }

        #endregion

        #region IDispatchDetailDAL 成员


        public int UpdateBy_ConfirmDispatch(long DID, string DeliveryNo)
        {
            if (String.IsNullOrWhiteSpace(DeliveryNo)) throw new ArgumentNullException("DeliveryNo is null or empty");
            String sql = @"
UPDATE TMS_DispatchDetail
SET DeliveryNo = :DeliveryNo
WHERE DID = :DID
AND IsDeleted = 0
";
            OracleParameter[] parameters = new OracleParameter[] 
            {
                new OracleParameter() { ParameterName = "DID", DbType = DbType.Int64, Value = DID },
                new OracleParameter() { ParameterName = "DeliveryNo", DbType = DbType.String, Value = DeliveryNo }
            };
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }

        /// <summary>
        /// 根据DID查询出所有的调度明细
        /// </summary>
        /// <param name="DID"></param>
        /// <returns></returns>
        public List<DispatchDetailModel> GetDispatchDetailByDID(long DID)
        {
            string strSql = @"SELECT * FROM TMS_DISPATCHDETAIL WHERE DID= :DID";
            OracleParameter[] arguments = new OracleParameter[]
                                              {
                                                  new OracleParameter()
                                                      {ParameterName = "DID", DbType = DbType.Int64, Value = DID}
                                              };
            return (List<DispatchDetailModel>)ExecuteSql_ByReaderReflect<DispatchDetailModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        #endregion
    }
}
