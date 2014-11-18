using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Quartz;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Util.IO;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Util.ConfigUtil;
using System.IO;

namespace Vancl.TMS.Schedule.InboundWriteImpl
{
    public class InboundWriteDBJob : QuartzExecute
    {
        IInboundBLL _bll = ServiceFactory.GetService<IInboundBLL>("InboundBLL");
        public override void DoJob(JobExecutionContext context)
        {
            int remainder = Convert.ToInt32(context.JobDetail.JobDataMap["Remainder"]);
            string threadDir = Consts.THREAD_CATEGORY_DIR + remainder.ToString();
            string path = Path.Combine(Consts.FILE_DIR, threadDir);
            string filePath = IOHelper.SearchFile(path, Consts.SEARCH_PATTERN);
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }
            string fileName = new FileInfo(filePath).Name;
            try
            {
                //反序列化文件
                List<InboundModel> list = IOHelper.FileToEntities<InboundModel>(filePath);
                //同步状态
                _bll.UpdateTMS(list);
                if (list[0].IsFCL)
                {
                    //整箱入库的情况
                    _bll.UpdateBoxSyncFlag(list[0].ID, Model.Common.Enums.SC2TMSSyncFlag.Already);
                }
                else
                {
                    //订单入库的情况
                    _bll.UpdateOrderSyncFlag(string.Join(",", list.Select(m => m.ID)), Model.Common.Enums.SC2TMSSyncFlag.Already);
                }

                //备份缓冲文件
                string backupPath = Path.Combine(path, Consts.BACKUP_DIR, fileName);
                IOHelper.Remove(filePath, backupPath);
            }
            catch (Exception ex)
            {
                //出错移到出错文件夹
                string errorPath = Path.Combine(path, Consts.ABNORMAL_DIR, fileName);
                IOHelper.Remove(filePath, errorPath);
                throw ex;
            }
        }
    }
}
