using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    public interface IBatchDAL
    {
        /// <summary>
        /// 新增批次记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(BatchEntityModel model);
    }
}
