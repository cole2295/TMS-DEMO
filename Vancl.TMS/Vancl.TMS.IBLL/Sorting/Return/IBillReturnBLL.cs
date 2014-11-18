using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Common;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.Model.BaseInfo.Sorting;
using Vancl.TMS.Model.LMS;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Sorting.Return
{
    /// <summary>
    /// 返货入库接口
    /// </summary>
    public interface IBillReturnBLL : ISortCenterBLL
    {
        /// <summary>
        /// 添加一条返货入库数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(BillReturnModel model);
        /// <summary>
        /// 根据运单号查询一条返货入库的记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        BillReturnModel GetModel(string FormCode);
        /// <summary>
        /// 根据运单号或者箱号或者标签号查询一条返货入库的记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <param name="CodeType"></param>
        /// <returns></returns>
        BillReturnEntityModel GetModel(string FormCode, string weight,int codetype,out string err);
        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        ViewBillReturnInfoModel GetReturnBillInfo(string FormCode);

        /// <summary>
        /// 退换货入库
        /// </summary>
        /// <param name="billModel"></param>
        /// <param name="logModel"></param>
        void BillReturnInbound(BillReturnEntityModel billModel, out string Message);
        /// <summary>
        /// 退换货or拒收入库
        /// </summary>
        /// <param name="wbObj"></param>
        /// <param name="opLog"></param>
        /// <returns></returns>
        bool ChangeOrRefuseInBound(BillModel bill, OperateLogEntityModel opLog);
        /// <summary>
        /// 运单是否已经逆向入库
        /// </summary>
        /// <param name="FormCode"></param>
        /// <returns></returns>
        bool IsBillReturning(string FormCode);
    }
}
