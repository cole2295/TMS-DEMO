using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Synchronous.InSync;

namespace Vancl.TMS.IBLL.Synchronous
{
    public interface ILms2TmsSyncBLL
    {
        /// <summary>
        /// 读取lms中间表数据
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        List<LmsWaybillStatusChangeLogModel> ReadLmsChangeLogs(Lms2TmsJobArgs args);

        /// <summary>
        /// 执行同步操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel DoSync(LmsWaybillStatusChangeLogModel model);
    }
}
