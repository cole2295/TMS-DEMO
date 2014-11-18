using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Log;
using Vancl.TMS.Model.Synchronous.OutSync;

namespace Vancl.TMS.IBLL.Synchronous
{
    public interface ITms2LmsSyncBLL
    {
        /// <summary>
        ///  读取tms中间表数据
        /// </summary>
        /// <param name="argument"></param>
        /// <returns></returns>
        List<BillChangeLogModel> ReadTmsChangeLogs(Tms2LmsJobArgs argument);
        /// <summary>
        /// 执行同步操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel DoSync(BillChangeLogModel model);
    }
}
