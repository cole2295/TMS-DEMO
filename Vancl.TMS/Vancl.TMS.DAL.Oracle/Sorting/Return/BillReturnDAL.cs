using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Return;
using Oracle.DataAccess.Client;
using Vancl.TMS.IDAL.Sorting.Return;
using Vancl.TMS.Model.Common;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Sorting.Return
{
    /// <summary>
    /// 返货入库数据实现层
    /// </summary>
    public class BillReturnDAL : BaseDAL, IBillReturnDAL
    {
        /// <summary>
        /// 添加一条返货入库数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(BillReturnModel model)
        {
            return 0;
        }
        /// <summary>
        /// 根据运单号查询一条返货入库的记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        public BillReturnModel GetModel(string FormCode)
        {
            if (string.IsNullOrEmpty(FormCode)) throw new ArgumentNullException("FormCode is null");
            string sql = @"
SELECT RBID,FormCode,BoxNo,LabelNo,BoxLabelNo,BoxStatus,Createby,CreateTime,IsDeleted
FROM sc_billreturninfo
WHERE FormCode =:FormCode
            ";
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType =OracleDbType.Varchar2,Size=50, Value = FormCode}           
            };
            return ExecuteSqlSingle_ByReaderReflect<BillReturnModel>(TMSReadOnlyConnection, sql, parameters);      
        }

        /// <summary>
        /// 根据运单号或者箱号或者标签号查询一条返货入库的记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <param name="CodeType"></param>
        /// <returns></returns>
        public BillReturnModel GetModel(string FormCode, int CodeType)
        {
            if (string.IsNullOrEmpty(FormCode)) throw new ArgumentNullException("FormCode is null");
            string sql = @"
SELECT RBID,FormCode,BoxNo,LabelNo,BoxLabelNo,BoxStatus,Createby,CreateTime,IsDeleted
FROM sc_billreturninfo WHERE IsDeleted=0 ";
            if(CodeType==0)
            {
                sql+=" AND FormCode = :Code ";
            }
            if(CodeType==1)
            {
                sql += " AND LabelNo = :Code ";
            }
            if (CodeType == 2)
            {
                sql += " AND BoxLabelNo = :Code ";
            }
            OracleParameter[] parameters = new OracleParameter[]
            {
                new OracleParameter() { ParameterName="FormCode", OracleDbType = OracleDbType.Varchar2,Size=50, Value = FormCode}           
            };
            return ExecuteSqlSingle_ByReaderReflect<BillReturnModel>(TMSReadOnlyConnection, sql, parameters);
        }

        /// <summary>
        /// 查询运单信息用于退货分拣称重装箱
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        public BillReturnModel GetBillByFormCode(string formCode)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
   SELECT BID
                 ,Status  ,ReturnStatus,FormCode,BillType ,CustomerOrder
                 ,DeliverStationID
                 ,(
                      SELECT MAX(CompanyName)
                      FROM ExpressCompany
                      WHERE  IsDeleted = 0 AND ExpressCompanyID = bill.DeliverStationID
                   ) DeliverStationName
                  ,ec.DistributionCode
                 ,CurrentDistributionCode
                 ,MerchantID
                 ,(
                       SELECT MAX(MerchantName)
                       FROM MerchantBaseInfo
                       WHERE ID = bill.MerchantID AND IsDeleted = 0
                   ) MerchantName
              --   ,DistributionCode
                 ,Source
                 ,DeliverCode
                 ,case when ec.DistributionCode<>'rfd' and  ec.COMPANYFLAG<>3 then
            			(
            				select SiteNo 
            				from PS_PMS.ExpressCompany
            				where EXPRESSCOMPANYID = ec.TOPCODCOMPANYID
            					and COMPANYFLAG = 3 
            					and ISDELETED = 0
                      and ROWNUM=1
            			)else ec.SiteNo
            		end as SiteNo
                ,ec.companyflag
                ,ec.companyname
		        ,case when ec.COMPANYFLAG<>3 then 
			        (		
				        select CompanyName 
				        from PS_PMS.ExpressCompany
				        where EXPRESSCOMPANYID = ec.TOPCODCOMPANYID
					        and COMPANYFLAG = 3 
					        and ISDELETED = 0
                            and ROWNUM=1
			        ) else ec.CompanyName 		
		        end as DistributionName

    FROM    SC_BILL bill
    LEFT JOIN PS_PMS.ExpressCompany ec ON ec.ExpressCompanyID = bill.DeliverStationID
    WHERE FormCode = :FormCode
                                         ");
            OracleParameter[] arguments = { new OracleParameter() { ParameterName = "FormCode", OracleDbType = OracleDbType.Varchar2,Size=50, Value = formCode } };
            return ExecuteSqlSingle_ByReaderReflect<BillReturnModel>(TMSReadOnlyConnection, SbSql.ToString(), arguments);
        }
    }
}
