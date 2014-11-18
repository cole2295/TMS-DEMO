using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Core.Schedule;
using Quartz;
using Vancl.TMS.IBLL.Synchronous;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.Model.Synchronous;
using Vancl.TMS.Util.IO;
using Vancl.TMS.Util.ConfigUtil;
using System.Threading;
using System.IO;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Schedule.InboundReadImpl
{
    public class InboundReadBoxDBJob : QuartzExecute
    {
        IInboundBLL _bll = ServiceFactory.GetService<IInboundBLL>("InboundBLL");
        public override void DoJob(JobExecutionContext context)
        {
            int remainder = Convert.ToInt32(context.JobDetail.JobDataMap["Remainder"]);
            //取得入库数据
            InboundModel model = _bll.GetInboundBox(Consts.BOX_THREAD_COUNT, remainder);
            if (model == null)
            {
                return;
            }
            List<InboundModel> list = new List<InboundModel>();
            list.Add(model);
            string threadDir = Consts.THREAD_CATEGORY_DIR + remainder.ToString();
            string path = Path.Combine(Consts.FILE_DIR, threadDir);
            string fileFullPath = Path.Combine(path, Common.GetFileName());
            try
            {
                //写文件
                IOHelper.EntitiesToFile<InboundModel>(list, fileFullPath);
                //更新状态
                _bll.UpdateBoxSyncFlag(model.ID, Enums.SC2TMSSyncFlag.Synchronizing);
                //重命名文件
                string fileCompleteFullPath = Path.Combine(path, Common.GetCompleteFileName());
                IOHelper.Remove(fileFullPath, fileCompleteFullPath);
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
