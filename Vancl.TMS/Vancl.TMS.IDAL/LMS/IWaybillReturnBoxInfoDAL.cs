using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.LMS;

namespace Vancl.TMS.IDAL.LMS
{
    /// <summary>
    /// 退货分拣称重箱号数据接口层
    /// </summary>
    public interface IWaybillReturnBoxInfoDAL
    {
        /// <summary>
        /// 添加退货分拣称重箱号信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        long Add(WaybillReturnBoxInfoEntityModel model);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="BoxNo"></param>
        /// <returns></returns>
        WaybillReturnBoxInfoEntityModel GetModel(long BoxNo);
        /// <summary>
        /// 更新退货分拣称重箱重量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateBoxWeight(WaybillReturnBoxInfoEntityModel model);

        /// <summary>
        /// 打印退货交接表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int PrintBackForm(WaybillReturnBoxInfoEntityModel model);

        /// <summary>
        /// 装箱打印
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int PrintBackPacking(WaybillReturnBoxInfoEntityModel model);
    }
}
