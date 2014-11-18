using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Quartz;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Util.ConfigUtil;
using Vancl.TMS.Util.IO;
using System.IO;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Schedule.InboundReadImpl
{
    public class InboundReadOrderDBjob : QuartzExecute
    {
        IInboundBLL _bll = ServiceFactory.GetService<IInboundBLL>("InboundBLL");
        public override void DoJob(JobExecutionContext context)
        {
            int remainder = Convert.ToInt32(context.JobDetail.JobDataMap["Remainder"]);
            List<InboundModel> list = _bll.GetInboundOrder(Consts.BATCH_MAX_COUNT, Consts.ORDER_THREAD_COUNT, remainder);
            if (list == null || list.Count == 0)
            {
                return;
            }
            string threadDir = Consts.THREAD_CATEGORY_DIR + (Consts.BOX_THREAD_COUNT + remainder).ToString();
            string path = Path.Combine(Consts.FILE_DIR, threadDir);
            string fileFullPath = Path.Combine(path, Common.GetFileName());
            try
            {
                //写文件
                IOHelper.EntitiesToFile<InboundModel>(list, fileFullPath);
                //更新状态
                _bll.UpdateOrderSyncFlag(string.Join(",", list.Select(m => m.ID)), Enums.SC2TMSSyncFlag.Synchronizing);
                //重命名文件
                string fileCompleteFullPath = Path.Combine(path, Common.GetCompleteFileName());
                File.Move(fileFullPath, fileCompleteFullPath);
            }
            catch (Exception ex)
            {
                //出错删除文件
                File.Delete(fileFullPath);
                throw ex;
            }
        }
    }
}
