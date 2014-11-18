using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.BillPrint;
using Oracle.DataAccess.Client;
using System.Data;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Sorting.BillPrint;

namespace Vancl.TMS.DAL.Oracle.Sorting.BillPrint
{
    public class BillDALForBillPrint : BaseDAL, IBillDALForBillPrint
    {
        public Enums.BillType? GetBillTypeByFormCode(string FormCode)
        {
            if (String.IsNullOrWhiteSpace(FormCode)) throw new ArgumentNullException("FormCode is null or empty.");
            String sql = @"
                                    SELECT BillType
                                    FROM SC_BILL
                                    WHERE FORMCODE = :FORMCODE
                                    ";
            OracleParameter[] arguments = new OracleParameter[] 
            { 
                    new OracleParameter() { ParameterName="FORMCODE", DbType= DbType.String, Value= FormCode}
            };
            var objType = ExecuteSqlScalar(TMSWriteConnection, sql, arguments);
            if (objType != null && objType != DBNull.Value)
            {
                return (Enums.BillType)Convert.ToInt32(objType);
            }
            return null;
        }
        public PrintBillModel GetBillByFormCode(string formCode)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
   SELECT BID
                 ,Status  ,FormCode,BillType ,CustomerOrder
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
            OracleParameter[] arguments = { new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } };
            return ExecuteSqlSingle_ByDataTableReflect<PrintBillModel>(TMSWriteConnection, SbSql.ToString(), arguments);
        }


        public int UpdateBillStatus(string formCode, Enums.BillStatus prevStatus, Enums.BillStatus currStatus)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                         UPDATE SC_Bill
                        SET    Status = :CurrStatus
                        WHERE  FormCode = :FormCode
                               AND Status = :PrevStatus
                                         ");
            OracleParameter[] arguments = { 
                                           new OracleParameter() { ParameterName = "CurrStatus", DbType = DbType.Int32, Value = (int)currStatus } ,
                                           new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode } ,
                                           new OracleParameter() { ParameterName = "PrevStatus", DbType = DbType.Int32, Value = (int)prevStatus } ,
                                       };
            return base.ExecuteSqlNonQuery(TMSWriteConnection, SbSql.ToString(), arguments);
        }


        public BillPrintViewModel GetBillPrintMenuData(string formCode, int? merchantID, int packageIndex, string receiveArea)
        {
            StringBuilder SbSql = new StringBuilder();
            SbSql.Append(@"
                                         SELECT  scb.FormCode FormCode
                                                        ,scb.billtype    BillType
                                                        ,scb.customerorder CustomerOrder
                                                        ,scbi.Tips  ReceiveComment
                                                        ,scbi.TotalAmount
                                                        ,scbi.ReceivableAmount
                                                        ,scbi.PayType
                                                        ,scbi.PackageMode
                                                        ,scbi.CustomerBoxNo
                                                        ,scbi.Weight
                                                        ,scbi.BillGoodsType
                                                        ,scbi.PackageCount
                                                        ,scbw.PackageIndex
                                                        ,mbi.MerchantName   
                                                        ,ecs.CompanyName  SendStation

                                                        ,ec.CompanyFlag
                                                        ,ec.DistributionCode
                                                        ,ec.CompanyName
                                                        ,ec.MnemonicName DeliveryStation

                                                        , area.ZoneCode
/*
		                                                ,case when ec.COMPANYFLAG<>3 then 
			                                                (		
				                                                select  CompanyName 
				                                                from ExpressCompany
				                                                where EXPRESSCOMPANYID = ec.TOPCODCOMPANYID 
					                                                and COMPANYFLAG = 3 
					                                                and ISDELETED = 0
			                                                ) else ec.CompanyName 		
		                                                end as DistributionName
        */
                                                        ,case when ec.COMPANYFLAG<>3 then 
			                                                (		
				                                                select  MnemonicCode  
				                                                from ExpressCompany 
				                                                where (EXPRESSCOMPANYID = ec.TOPCODCOMPANYID or DistributionCode = ec.DistributionCode)
					                                                and COMPANYFLAG = 3 
					                                                and ISDELETED = 0
                                                                    and rownum=1
			                                                ) else ec.MnemonicCode 		
		                                                end as DistributionName

                                                        ,case when ec.DistributionCode <>'rfd' and  ec.COMPANYFLAG<>3 then
			                                                (
				                                                select  SiteNo 
				                                                from ExpressCompany
				                                                where EXPRESSCOMPANYID = ec.TOPCODCOMPANYID 
					                                                and COMPANYFLAG = 3 
					                                                and ISDELETED = 0
			                                                )else ec.SiteNo
		                                                end as SiteNo
  
        FROM    SC_Bill scb 
        LEFT JOIN SC_BillInfo  scbi ON  scb.formcode = scbi.formcode
        LEFT JOIN ExpressCompany ecs  ON ecs.ExpressCompanyID = scb.CreateDept
        JOIN ExpressCompany ec  ON ec.ExpressCompanyID = scb.DeliverStationID 
           
        LEFT JOIN MerchantBaseInfo mbi ON mbi.ID = scb.MerchantID
        LEFT JOIN SC_BillWeigh scbw ON scb.formcode = scbw.formcode AND scbw.IsDeleted = 0 
        LEFT JOIN Area area  ON    area.AreaName=:ReceiveArea and area.CityID = ec.CityID
        WHERE scb.FormCode = :FormCode 
                                       ");
            IList<OracleParameter> argumentsList = new List<OracleParameter>();
            argumentsList.Add(new OracleParameter() { ParameterName = "ReceiveArea", DbType = DbType.String, Value = receiveArea });
            argumentsList.Add(new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = formCode });

            if (merchantID != null && merchantID > 0)
            {
                SbSql.Append(@" AND scb.MerchantID = :MerchantID ");
                argumentsList.Add(new OracleParameter()
                {
                    ParameterName = "MerchantID",
                    DbType = DbType.Int32,
                    Value = merchantID
                });
            }
            if (packageIndex > 0)
            {
                SbSql.Append(@" AND scbw.PackageIndex = :PackageIndex ");
                argumentsList.Add(new OracleParameter()
                {
                    ParameterName = "PackageIndex",
                    DbType = DbType.Int32,
                    Value = packageIndex
                });
            }
            return ExecuteSqlSingle_ByReaderReflect<BillPrintViewModel>(TMSWriteConnection, SbSql.ToString(), argumentsList.ToArray());
        }
    }
}
