using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.IO;
using Vancl.TMS.Tools.VCMRecords.Entity;
using Vancl.TMS.Tools.VCMRecords.Tool.Return;
using Vancl.TMS.Tools.VCMRecords.Tool.DB;
using System.Data.SqlClient;

namespace Vancl.TMS.Tools.VCMRecords.DAL
{
    public enum ExecuteStatusType { 未执行=0,已执行=1,执行失败=2}

    public class VCMRecordDAL:BaseDAL
    {   

        public VCMRecordDAL()
        {
            this.tablename = "VCM_Records";
            this.connectionString = DBConn.VCMRecordsConnctionString;
        }

        /// <summary>
        /// 创建一个VCM日志
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ReturnResult Create(VCMRecord record)
        {
            return base.Create<VCMRecord>(record);
        }

        /// <summary>
        /// 更新一个VCM日志
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ReturnResult Update(VCMRecord record)
        {
            return base.Update<VCMRecord>(record);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ReturnResult Remove(VCMRecord record)
        {
            //删除记录时，需要把记录的附件一起删除
            String sql = String.Format(
                "DELETE FROM VCM_Files WHERE RECORDID = @RECORDID ; "
                + "DELETE FROM {0} WHERE ID = @ID",
                this.tablename
                );         
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@RECORDID", record.ID),new SqlParameter("@ID",record.ID) };
            try
            {
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, sql, parameters);
                return new ReturnResult(true);
            }
            catch (Exception ex)
            {
                return new ReturnResult(false, new Fault("删除VCM日志失败", ex));
            }
        }

    }
}
