using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Oracle.DataAccess.Client;
using Vancl.TMS.IDAL.LadingBill;
using Vancl.TMS.Model.LadingBill;
using Vancl.TMS.Util.DbHelper;

namespace Vancl.TMS.DAL.Oracle.LadingBill
{
    public class MERCHANTWAREHOUSEDAL : BaseDAL, IMERCHANTWAREHOUSEDAL
    {

        public IList<Model.LadingBill.MERCHANTWAREHOUSE> GetModelList(Dictionary<string, object> searchParams)
        {
            var strSql = new StringBuilder("select * from PS_PMS.MERCHANTWAREHOUSE ");
            strSql.Append(" where 1 = 1 ");

            var paramers = new List<OracleParameter>();

            if (searchParams != null)
            {
                searchParams.ToList().ForEach(item =>
                {
                    strSql.Append(string.Format(" and {0} = :{0}", item.Key));
                    paramers.Add(new OracleParameter(string.Format(":{0}", item.Key), item.Value));
                });
            }

            var resultData = ExecuteSql_ByDataTableReflect<MERCHANTWAREHOUSE>(TMSReadOnlyConnection, strSql.ToString(), paramers.ToArray());

            return resultData;
        }

        /// <summary>
        /// 根据仓库id获得仓库实体
        /// </summary>
        /// <param name="warehouseid"></param>
        /// <returns></returns>
        public MERCHANTWAREHOUSE GetModelByid(string warehouseid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select w.*, m.merchantname
                           from ps_pms.MERCHANTWAREHOUSE w
                            join ps_pms.MERCHANTBASEINFO m
                            on w.merchantid = m.id
                           where w.warehousecode = :WAREHOUSEID");

            OracleParameter[] parameters = {
					new OracleParameter(":WAREHOUSEID", OracleDbType.Varchar2,20)};
            parameters[0].Value = warehouseid;

            MERCHANTWAREHOUSE model = new MERCHANTWAREHOUSE();
            model = ExecuteSqlSingle_ByDataTableReflect<MERCHANTWAREHOUSE>(TMSReadOnlyConnection, strSql.ToString(), parameters);

            return model;
        }
    }
}
