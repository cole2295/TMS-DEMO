using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IDAL.Sorting.Search;
using Vancl.TMS.Model.Sorting.WeighReview;
using Vancl.TMS.Util.Pager;
using Oracle.DataAccess.Client;
using System.Data;

namespace Vancl.TMS.DAL.Oracle.Sorting.Search
{
    public class WeighReviewSearchDAL : BaseDAL, IWeighReviewSearchDAL
    {
        public PagedList<WeighReviewViewModel> WeighReviewSearch(WeighReviewSearchModel searchModel)
        {
            StringBuilder sbSql = new StringBuilder();
            sbSql.Append(@"
SELECT scb.FormCode
       ,scb.CustomerOrder
       ,ee.EmployeeCode
       ,ee.EmployeeName OperatorName
       ,scbw.UpdateTime OperateTime
FROM SC_BillWeigh scbw 
JOIN SC_BILL  scb  ON scb.FormCode = scbw.FormCode 
JOIN SC_BillInfo scbi ON scb.FormCode =scbi.FormCode
JOIN PS_PMS.MerchantBaseInfo mbi  ON scb.MerchantID = mbi.ID
JOIN PS_PMS.Employee ee ON scbw.Updateby = ee.EmployeeID
where scbw.BWID =(select max(scbw2.BWID) from SC_BillWeigh scbw2 where scbw2.FormCode = scb.FormCode )  
                                    ");
            var paramList = new List<OracleParameter>();
            #region 条件筛选
            //运单号
            if (string.IsNullOrWhiteSpace(searchModel.Code)==false)
            {
                sbSql.Append(" AND scb.FormCode = :FormCode");
                paramList.Add(new OracleParameter() { ParameterName = "FormCode", DbType = DbType.String, Value = searchModel.Code });
            }
            //复核状态
            if (searchModel.WeighReviewStatus == WeighReviewStatus.Abnormal)
            {
                sbSql.Append(" AND ( mbi.IsCheckWeight=1 AND ABS(nvl(scbi.Weight,0)-nvl(scbi.CustomerWeight,0))>nvl(mbi.CheckWeight, 0)  ) ");
            }
            else
            {
                sbSql.Append(" AND ( mbi.IsCheckWeight=1 AND ABS(nvl(scbi.Weight,0)-nvl(scbi.CustomerWeight,0))<=nvl(mbi.CheckWeight, 0)  ) ");
            }
            //员工号
            if (string.IsNullOrWhiteSpace(searchModel.EmployeeCode) == false)
            {
                sbSql.Append(" AND ee.EmployeeCode = :EmployeeCode ");
                paramList.Add(new OracleParameter() { ParameterName = "EmployeeCode", DbType = DbType.String, Value = searchModel.EmployeeCode });
            }
            //时间范围
            if (searchModel.BeginTime != null && searchModel.EndTime != null)
            {
                sbSql.Append(" AND scbw.UpdateTime > :BeginTime  AND scbw.UpdateTime < :EndTime");
                paramList.Add(new OracleParameter() { ParameterName = "BeginTime", DbType = DbType.DateTime, Value = searchModel.BeginTime });
                paramList.Add(new OracleParameter() { ParameterName = "EndTime", DbType = DbType.DateTime, Value = searchModel.EndTime });
            }
            #endregion 条件筛选
            sbSql.Append("  ORDER BY  scbw.UpdateTime DESC");
            var list =  ExecuteSqlPaged_ByRowNumberGrammar_ByReaderReflect<WeighReviewViewModel>(TMSReadOnlyConnection, sbSql.ToString(), searchModel, paramList.ToArray());
            list.ForEach(item => item.WeighReviewStatus = searchModel.WeighReviewStatus);
            return list;
        }
    }

}

