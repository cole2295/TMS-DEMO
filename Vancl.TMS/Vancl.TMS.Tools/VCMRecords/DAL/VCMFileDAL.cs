using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using Vancl.TMS.Tools.VCMRecords.Tool;
using Vancl.TMS.Tools.VCMRecords.Entity;
using Vancl.TMS.Tools.VCMRecords.Tool.DB;
using Vancl.TMS.Tools.VCMRecords.Tool.Return;

namespace Vancl.TMS.Tools.VCMRecords.DAL
{
    public class VCMFileDAL:BaseDAL
    {
        public VCMFileDAL()
        {
            tablename = "VCM_Files";
        }

        /// <summary>
        /// 获得日志关联的附件
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public List<VCMFile> Find(VCMRecord record)
        {
            String columns = "ID,FileName,UploadTime,SizeLabel";
            DBCondition condition = new DBCondition("RecordID=@RecordID", "@RecordID", record.ID);
            try
            {
                DataTable resulttab = base.FindByCondition(new DBCondition[] { condition }, columns);
                List<VCMFile> resultlist = Transfer.TableToList<VCMFile>(resulttab);
                return resultlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取文件的文件内容
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public String FindFileContent(VCMFile file)
        {
            String columns = "FileContent";
            DBCondition condition = new DBCondition("ID=@ID", "@ID", file.ID);
            try
            {
                DataTable resulttab = base.FindByCondition(new DBCondition[] { condition }, columns);
                return resulttab.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public ReturnResult Create(VCMFile file)
        {
            return base.Create<VCMFile>(file);
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ReturnResult Remove(VCMFile file)
        {
            return base.Remove<VCMFile>(file);
        }

        /// <summary>
        /// 通过record删除
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ReturnResult Remove(VCMRecord record)
        {
            String sql = String.Format("DELETE FROM {0} WHERE RECORDID = @RECORDID", tablename);
            SqlParameter[] parameters = new SqlParameter[] {new SqlParameter("@RECORDID",record.ID) };
            try
            {
                SqlHelper.ExecuteNonQuery(connectionString, CommandType.Text,sql,parameters);
                return new ReturnResult(true);
            }
            catch(Exception ex)
            {
                return new ReturnResult(false,new Fault("删除日志失败",ex));
            }
        }

    }
}
