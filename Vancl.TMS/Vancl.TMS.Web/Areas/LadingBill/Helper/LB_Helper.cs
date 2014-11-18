/*
 * 提货模板帮助类
 * 
 */
using System;
using Vancl.TMS.Core.ServiceFactory;
using Vancl.TMS.IBLL.LadingBill;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.Web.Areas.LadingBill.Helper
{
    public class LB_Helper
    {
        private ILB_TASKBLL _lbTaskbll = ServiceFactory.GetService<ILB_TASKBLL>();

        /// <summary>
        /// 生成任务编号
        /// </summary>
        /// <returns></returns>
        public string CreateTaskCode(UserModel curUser)
        {
            string taskcode = string.Empty;

            DateTime dbtime = _lbTaskbll.GetDBTime();
            //需改
            taskcode = curUser.DistributionCode.ToUpper()
                             + dbtime.Year.ToString().Substring(2, 2)
                             + dbtime.Month.ToString().PadLeft(2, '0')
                             + dbtime.Day.ToString().PadLeft(2, '0');

            string todayCount = _lbTaskbll.ToDayTaskCount().ToString();

            if (Convert.ToInt32(todayCount) < 10)
            {
                todayCount = "00" + todayCount;
            }
            else if (Convert.ToInt32(todayCount) > 9 && Convert.ToInt32(todayCount) < 99)
            {
                todayCount = "0" + todayCount;
            }

            return taskcode +"-"+ todayCount;
        }
    }
}