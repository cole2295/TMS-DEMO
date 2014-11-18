using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.IDAL.LMS
{
    public interface ITransitBoxDAL
    {
        /// <summary>
        /// 改变周转箱使用状态
        /// </summary>
        /// <param name="isUsing"></param>
        /// <returns></returns>
        bool UpdateUsingStatus(bool isUsing, string boxNo);
    }
}
