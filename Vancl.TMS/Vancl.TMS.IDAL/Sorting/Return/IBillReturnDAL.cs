using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Return;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IDAL.Sorting.Return
{
    /// <summary>
    /// 返货入库数据接口层
    /// </summary>
    public interface IBillReturnDAL
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
        BillReturnModel GetModel(string FormCode, int CodeType);
        
        /// <summary>
        /// 查询运单信息用于退货分拣称重装箱
        /// </summary>
        /// <param name="formCode"></param>
        /// <returns></returns>
        BillReturnModel GetBillByFormCode(string formCode);
        /// <summary>
        /// 根据箱号批量更新运单返货状态
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        //bool BatchUpdateBillReturnStatus(string BoxNo, Enums.ReturnStatus status);
    }
}
