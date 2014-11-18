using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.CityScan;
using Oracle.DataAccess.Client;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Sorting.CityScan;

namespace Vancl.TMS.DAL.Oracle.Sorting.CityScan
{
    public class CityScanDAL : BaseDAL, ICityScanDAL
    {
        public bool Exists(String formCode)
        {
            if (formCode == null)
            {
                throw new ArgumentNullException("cityScan Exists");
            }
            string sql = @"SELECT COUNT(1) FROM sc_expresscityscan WHERE FORMCODE=:formcode AND IsDeleted=0";
            OracleParameter[] parameters ={
                                              new OracleParameter(":formcode",OracleDbType.Varchar2)
                                         };
            parameters[0].Value = formCode;
            object o = ExecuteSqlScalar(TMSReadOnlyConnection, sql, parameters.ToArray());
            if (o == null || Convert.ToInt32(o) > 0)
            {
                return true;
            }
            return false; 
        }

        public Int32 AddCityScan(Model.Sorting.CityScan.CityScanModel cityScanModel)
        {
            if (cityScanModel == null)
            {
                throw new ArgumentNullException("cityScanModel");
            }
            string sql = @"
insert into sc_expresscityscan
(
       bcsid,
       formcode,
       scansortcenter,
       scantime,
       batchno,
       createby,
       createtime,
       updateby,
       updatetime,
       isdeleted
)
values
(
       :bcsid,
       :formcode,
       :scansortcenter,
       sysdate,
       :batchno,
       :createby,
       sysdate,
       :updateby,
       sysdate,
       :isdeleted
)";
            OracleParameter[] parameters =
            {
                new OracleParameter(":bcsid",OracleDbType.Decimal),
                new OracleParameter(":formcode",OracleDbType.Varchar2),
                new OracleParameter(":scansortcenter",OracleDbType.Decimal),
                new OracleParameter(":batchno",OracleDbType.Varchar2),
                new OracleParameter(":createby",OracleDbType.Decimal),
                new OracleParameter(":updateby",OracleDbType.Decimal),
                new OracleParameter(":isdeleted",OracleDbType.Decimal),
            };
            parameters[0].Value = cityScanModel.BCSID;
            parameters[1].Value = cityScanModel.FormCode;
            parameters[2].Value = cityScanModel.ScanSortCenter;
            parameters[3].Value = cityScanModel.BatchNO;
            parameters[4].Value = cityScanModel.CreateBy;
            parameters[5].Value = cityScanModel.UpdateBy;
            parameters[6].Value = cityScanModel.IsDeleted;
            return ExecuteSqlNonQuery(TMSWriteConnection, sql, parameters);
        }


        public PagedList<CityScanModel> SearchCityScanStatistics(CityScanSearchModel searchModel)
        {
            if (searchModel == null)
            {
                throw new ArgumentNullException("cityScanSearchModel");
            }

            if (string.IsNullOrWhiteSpace(searchModel.ExpressCompanyID))
            {
                throw new ArgumentNullException("cityScanSearchModel ExpressCompanyID is null");
            }
            string sql = @"
select 
    ec.companyname as ScanSortCenterName,
    ecs.batchno,
    count(formcode) as CountNum 
from sc_expressCityscan ecs
join ExpressCompany ec on ecs.scansortcenter=ec.expresscompanyid
where 1=1 AND ecs.scansortcenter=:ExpressCompanyID {0}
group by ecs.batchno,ec.companyname";

            List<OracleParameter> parameterList = new List<OracleParameter>();
            StringBuilder sbWhere = new StringBuilder();
            parameterList.Add(new OracleParameter(":ExpressCompanyID", OracleDbType.Decimal) { Value = searchModel.ExpressCompanyID });
            if (!string.IsNullOrWhiteSpace(searchModel.BatchNo))
            {
                sbWhere.Append(" AND ecs.batchno=:batchno");
                parameterList.Add(new OracleParameter(":batchno", OracleDbType.Varchar2) { Value = searchModel.BatchNo });
            }
            if (!string.IsNullOrWhiteSpace(searchModel.FormCode))
            {
                sbWhere.Append(" AND ecs.formcode=:FormCode");
                parameterList.Add(new OracleParameter(":FormCode", OracleDbType.Varchar2) { Value = searchModel.FormCode });
            }
            if (searchModel.ScanStartTime.HasValue)
            {
                sbWhere.Append(" AND ecs.scantime>=:ScanStartTime");
                parameterList.Add(new OracleParameter(":ScanStartTime", OracleDbType.Date) { Value = searchModel.ScanStartTime.Value });
            }
            if (searchModel.ScanEndTime.HasValue)
            {
                sbWhere.Append(" AND ecs.scantime<=:ScanEndTime");
                parameterList.Add(new OracleParameter(":ScanEndTime", OracleDbType.Date) { Value = searchModel.ScanEndTime.Value });
            }
            sql = string.Format(sql,sbWhere.ToString());
            return ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<CityScanModel>(TMSReadOnlyConnection, sql, searchModel, parameterList.ToArray());
        }


        public IList<CityScanExprotModel> SearchExportScan(List<string> batchnoList)
        {
            if (batchnoList == null || batchnoList.Count <= 0)
            {
                throw new ArgumentNullException("cityScan Export batchnoList is null");
            }

            string sql = @"select 
rownum as SerialNumber,
ecs.batchno,
ecs.formcode ,
ec1.companyname as DeliverStationName,
mbi.merchantname,
ecs.scantime,
billInfo.Receivableamount
from sc_expressCityscan ecs
join ExpressCompany ec on ecs.scansortcenter=ec.expresscompanyid
join SC_BILL bill on bill.formcode=ecs.formcode
join SC_BILLINFO billInfo on billInfo.Formcode=ecs.formcode
join ExpressCompany ec1 on bill.deliverstationid=ec1.expresscompanyid
join Merchantbaseinfo mbi on mbi.id=bill.merchantid
where batchno in ({0}) order by batchno";
            List<OracleParameter> parameters = new List<OracleParameter>();
            var batchnoStr = string.Join(",", batchnoList.Select(row => string.Format("'{0}'", row)));
            sql = string.Format(sql,batchnoStr);
            return ExecuteSql_ByReaderReflect<CityScanExprotModel>(TMSReadOnlyConnection, sql, parameters.ToArray());
        }


        public IList<CityScanBatchDetail> SearchCityScanPrint(string batchno)
        {
            if (string.IsNullOrWhiteSpace(batchno))
            {
                throw new ArgumentNullException("Print batchno is null");
            }
            string sql = @"select formcode from sc_expressCityscan ecs where batchno=:batchno";
            List<OracleParameter> parameters = new List<OracleParameter>();
            parameters.Add(new OracleParameter(":batchno", OracleDbType.Varchar2) { Value = batchno });
            return ExecuteSql_ByReaderReflect<CityScanBatchDetail>(TMSReadOnlyConnection, sql, parameters.ToArray());
        }
    }
}
