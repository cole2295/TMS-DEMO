using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Vancl.TMS.Model.BaseInfo.Carrier;
using Vancl.TMS.IDAL.BaseInfo.Carrier;
using Oracle.DataAccess.Client;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Core.Security;

namespace Vancl.TMS.DAL.Oracle.BaseInfo.Carrier
{
    /// <summary>
    /// 适用范围数据访问层实现
    /// </summary>
    public class CoverageDAL : BaseDAL, ICoverageDAL
    {
        #region ICoverageDAL 成员

        /// <summary>
        /// 新增适用范围
        /// </summary>
        /// <param name="lstModel">适用范围</param>
        /// <returns>数据所影响行数</returns>
        public int Add(IList<CoverageModel> lstModel)
        {
            string strSql = string.Format(@"
                INSERT INTO TMS_Coverage(
                    CoverageID,
                    CarrierID,
                    CityID,
                    CreateBy,
                    UpdateBy,
                    IsDeleted
                 )
                VALUES(
                    {0},
                    :CarrierID,
                    :CityID,
                    {1},
                    {2},
                    0
                )", lstModel[0].SequenceNextValue()
                  , UserContext.CurrentUser.ID
                  , UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CarrierID",DbType= DbType.Int32,Value=lstModel.Select(m=>m.CarrierID).ToArray()},
                new OracleParameter() { ParameterName="CityID",DbType= DbType.String,Value=lstModel.Select(m=>m.CityID).ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, lstModel.Count, arguments);
        }

        /// <summary>
        /// 根据承运商id取得适用范围
        /// </summary>
        /// <param name="carrierID">承运商ID</param>
        /// <returns>适用范围模型列表</returns>
        public IList<CoverageModel> GetByCarrierID(int carrierID)
        {
            string strSql = @"
                SELECT CoverageID
                    ,CarrierID
                    ,CityID
                    ,CreateBy
                    ,CreateTime
                    ,UpdateBy
                    ,UpdateTime
                    ,IsDeleted
                FROM TMS_Coverage
                WHERE CarrierID=:CarrierID
                    AND IsDeleted=0
            ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CarrierID",DbType= DbType.Int32,Value=carrierID}
            };
            return ExecuteSql_ByReaderReflect<CoverageModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        /// <summary>
        /// 根据适用范围主键id取得适用范围
        /// </summary>
        /// <param name="coverageID">适用范围ID</param>
        /// <returns>适用范围模型</returns>
        public CoverageModel GetByCoverageID(int coverageID)
        {
            string strSql = @"
                SELECT CoverageID
                    ,CarrierID
                    ,CityID
                    ,CreateBy
                    ,CreateTime
                    ,UpdateBy
                    ,UpdateTime
                    ,IsDeleted
                FROM TMS_Coverage
                WHERE CoverageID=:CoverageID
                    AND IsDeleted=0
            ";
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CoverageID",DbType= DbType.Int32,Value=coverageID}
            };
            return ExecuteSqlSingle_ByReaderReflect<CoverageModel>(TMSReadOnlyConnection, strSql, arguments);
        }

        /// <summary>
        /// 删除适用范围
        /// </summary>
        /// <param name="carrierIDs">承运商id列表</param>
        /// <returns>数据所影响行数</returns>
        public int Delete(IList<int> carrierIDs)
        {
            if (carrierIDs == null)
            {
                throw new ArgumentNullException("carrierIDs");
            }
            if (carrierIDs.Count == 0)
            {
                throw new ArgumentException("承运商id列表数目不能为0");
            }
            string strSql = string.Format(@"
                UPDATE TMS_Coverage
                SET IsDeleted=1
                    ,UpdateBy={0}
                    ,UpdateTime=sysdate
                WHERE CarrierID =:CarrierIDs
                    AND IsDeleted=0", UserContext.CurrentUser.ID);
            OracleParameter[] arguments = new OracleParameter[] {
                new OracleParameter() { ParameterName="CarrierIDs",DbType= DbType.Int32,Value=carrierIDs.ToArray()}
            };
            return ExecuteSqlArrayNonQuery(TMSWriteConnection, strSql, carrierIDs.Count, arguments);
        }

        #endregion
    }
}
