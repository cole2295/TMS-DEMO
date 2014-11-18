using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using Vancl.TMS.IDAL.BaseInfo;
using Vancl.TMS.Model.BaseInfo;

namespace Vancl.TMS.DAL.Sql2008.BaseInfo
{
    public class ExpressCompanyDAL : BaseDAL, IExpressCompanyDAL
    {
        #region IExpressCompanyDAL 成员

        /// <summary>
        /// 根据父节点获取子站点的所有数据
        /// </summary>
        /// <param name="parentId">父节点编号</param>
        /// <returns></returns>
        public IList<ExpressCompanyModel> GetChildExpressCompany(int parentId)
        {
            var sql = @"
                   SELECT   a.ExpressCompanyID 'ID', 
                            a.CompanyName 'Name',
                            a.SimpleSpell,
                            a.ParentID,
                            CASE WHEN isnull(b.c,0) > 0 THEN 1 ELSE 0 END AS HasChild   
                     FROM   RFD_PMS.dbo.ExpressCompany(NOLOCK) a
                LEFT JOIN  
                (
                    SELECT  COUNT(1) as c,
                            ParentID 
                     FROM   RFD_PMS.dbo.ExpressCompany(NOLOCK) ec
                 GROUP BY   ParentID	
                ) b
                ON  a.ExpressCompanyID = b.ParentID 
                WHERE a.ParentID = @ParentID AND a.IsDeleted = 0
                ORDER BY HasChild DESC";
            var parameters = new SqlParameter[] 
            { 
                new SqlParameter("@ParentID", SqlDbType.Int){ Value = parentId }
            };
            return ExecuteReader<ExpressCompanyModel>(TMSReadOnlyConnection, sql, parameters);
        }

        /// <summary>
        /// 获取所有的部门名称(供自动完成使用)
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllCompanyNames()
        {
            var sql = @"
                   SELECT  DISTINCT (CompanyName) 'Name'
                     FROM  RFD_PMS.dbo.ExpressCompany(NOLOCK) 
                    WHERE  IsDeleted = 0";
            var models = ExecuteReader<ExpressCompanyModel>(TMSReadOnlyConnection, sql);
            return (from m in models
                    select m.Name).ToList();
        }
        #endregion
    }
}
