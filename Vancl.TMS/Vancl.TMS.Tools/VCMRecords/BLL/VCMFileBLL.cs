using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Tools.VCMRecords.Tool.Return;
using Vancl.TMS.Tools.VCMRecords.Entity;
using Vancl.TMS.Tools.VCMRecords.DAL;

namespace Vancl.TMS.Tools.VCMRecords.BLL
{
    /// <summary>
    /// VCM附件业务逻辑处理
    /// </summary>
    public class VCMFileBLL
    {
        VCMFileDAL dal = new VCMFileDAL();

        /// <summary>
        /// 获取日志的附件
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public List<VCMFile> Find(VCMRecord record)
        {
            return dal.Find(record);
        }

        /// <summary>
        /// 获取文件的文件内容
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public String FindFileContent(VCMFile file)
        {
            return dal.FindFileContent(file);
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public ReturnResult Create(VCMFile[] files)
        {
            ReturnResult result = new ReturnResult(true);
            int failCount = 0;
            foreach (VCMFile file in files)
            {
                if (dal.Create(file).Result == false)
                {
                    failCount++;
                }
            }
            if (failCount>0)
            {
                Fault fault = new Fault(String.Format("{0}个文件未能创建", failCount));
                result.Result = false;
            }
            return result;            
        }

        /// <summary>
        /// 移除文件
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public ReturnResult Remove(VCMFile[] files)
        {
            ReturnResult result = new ReturnResult();
            int failCount = 0;
            foreach (VCMFile file in files)
            {
                if (dal.Remove(file).Result == false)
                {
                    result.Result = false;
                    failCount++;
                }
            }
            if (result.Result == false)
            {
                Fault fault = new Fault(String.Format("{0}个文件未能删除", failCount));
                result.Fault = fault;
                return result;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 通过record删除
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ReturnResult Remove(VCMRecord record)
        {
            return dal.Remove(record);
        }

    }
}
