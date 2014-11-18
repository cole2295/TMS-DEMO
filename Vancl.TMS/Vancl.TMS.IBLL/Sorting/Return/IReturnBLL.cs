using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Util.Pager;

namespace Vancl.TMS.IBLL.Sorting.Return
{
    /// <summary>
    /// 退货业务接口
    /// </summary>
    public interface IReturnBLL : ISortCenterBLL
    {
        /// <summary>
        /// 退货前置条件对象
        /// </summary>
        /// <param name="DistributionCode"></param>
        /// <returns></returns>
        ReturnBillPreConditionModel GetPreCondition(String FormCode);

        /// <summary>
        /// 获取最大的箱号
        /// </summary>
        /// <param name="boxNoHead"></param>
        /// <returns></returns>
        string GetMaxBoxNO(string boxNoHead);
        /// <summary>
        /// 装箱打印操作
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        bool InBoxPrint(string BoxNo,decimal Weight,out string Message);
        
        /// <summary>
        /// 退货出库操作
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        ResultModel ReturnOutBound(string BoxNoOrFormCodes, Enums.ReturnStatus status, bool isBox);
        /// <summary>
        /// 更新运单返货状态
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        bool UpdateBillReturnStatus(string FormCode, Enums.ReturnStatus status);
        /// <summary>
        /// 改变周转箱使用状态
        /// </summary>
        /// <param name="isUsing"></param>
        /// <returns></returns>
        bool UpdateUsingStatus(bool isUsing, string boxNo);
        /// <summary>
        /// 验证周转箱
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        string ValidBox(string BoxNo);
    }
}
