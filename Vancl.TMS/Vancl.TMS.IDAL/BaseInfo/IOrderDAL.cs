using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Order;

namespace Vancl.TMS.IDAL.BaseInfo
{
    public interface IOrderDAL
    {
        /// <summary>
        /// 取得已经存在于系统的单子
        /// </summary>
        /// <param name="formCodeList"></param>
        /// <returns></returns>
        List<String> GetExistsFormCode(List<String> formCodeList);


        /// <summary>
        /// 新建订单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(List<OrderModel> model);

        /// <summary>
        /// 根据lms运单号取得单号
        /// </summary>
        /// <param name="waybillNo">lms运单号</param>
        /// <returns></returns>
        string GetFormCodeByWaybillNo(long waybillNo);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="formCode">单号</param>
        /// <returns></returns>
        bool isExists(string formCode);
    }
}
