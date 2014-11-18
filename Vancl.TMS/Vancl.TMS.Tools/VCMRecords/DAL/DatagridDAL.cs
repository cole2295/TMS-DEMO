using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using Vancl.TMS.Tools.VCMRecords.Tool.DataGrid;
using Vancl.TMS.Tools.VCMRecords.Tool.DB;

namespace Vancl.TMS.Tools.VCMRecords.DAL
{
    public class DatagridDAL
    {
        private string connectionStr
        {
            get
            {
                return DBConn.VCMRecordsConnctionString;
            }
        }

        /// <summary>
        /// 根据PageModel查询 当前页面数据
        /// </summary>
        /// <param name="pageModel"></param>
        public void FindPageModel(PageModel pageModel)
        {
            if (pageModel.count < 0)
            {
                pageModel.count = Convert.ToInt32(SqlHelper.ExecuteScalar(connectionStr,
                    CommandType.Text, DBFunc.GetQueryCountSqlByPageModel(pageModel), 
                    pageModel.sqlParameters));
            }
            pageModel.table = SqlHelper.ExecuteDataset(connectionStr, CommandType.Text, DBFunc.GetQuerySQLByPageModel(pageModel), pageModel.sqlParameters).Tables[0];           
        }
    }
}
