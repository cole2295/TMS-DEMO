using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Vancl.TMS.Tools.VCMRecords.DAL;
using Vancl.TMS.Tools.VCMRecords.Tool.DB;
using Vancl.TMS.Tools.VCMRecords.Tool.Return;
using Vancl.TMS.Tools.VCMRecords.Entity;

namespace Vancl.TMS.Tools.VCMRecords.BLL
{
    /// <summary>
    /// VCM操作记录业务逻辑处理
    /// </summary>
    public class VCMRecordBLL
    {
        VCMRecordDAL dal = new VCMRecordDAL();

        /// <summary>
        /// 创建一个VCM日志
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ReturnResult Create(VCMRecord record)
        {
            return dal.Create(record);
        }

        /// <summary>
        /// 更新VCM日志
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ReturnResult Update(VCMRecord record)
        {
            return dal.Update(record);
        }

        /// <summary>
        /// 删除一条记录
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ReturnResult Remove(VCMRecord record)
        {
            return dal.Remove(record);
        }

        /// <summary>
        /// 删除多条记录
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public ReturnResult Remove(VCMRecord[] records)
        {
            ReturnResult result = new ReturnResult();
            int failCount = 0;
            foreach (VCMRecord record in records)
            {
                if (Remove(record).Result == false)
                {
                    failCount++;
                }
            }
            if (failCount>0)
            {
                Fault fault = new Fault(String.Format("{0}记录未能删除", failCount));
                result.Fault = fault;
            }
            return result;
        }

       

    }
}
