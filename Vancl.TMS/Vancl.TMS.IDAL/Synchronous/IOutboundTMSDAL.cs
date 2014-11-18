using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo.Order;

namespace Vancl.TMS.IDAL.Synchronous
{
    /// <summary>
    /// 出库同步从文件到TMS系统
    /// </summary>
    public interface IOutboundTMSDAL
    {
        /// <summary>
        /// 导入订单货物明细数据
        /// </summary>
        /// <param name="listOrderDetail"></param>
        void ImportOrderDetailList(List<OrderDetailModel> listOrderDetail);

        /// <summary>
        /// 导入订单数据
        /// </summary>
        /// <param name="listOrder"></param>
        void ImportOrder(List<OrderModel> listOrder);

        /// <summary>
        /// 导入箱数据
        /// </summary>
        /// <param name="boxModel"></param>
        void ImportBox(BoxModel boxModel);

        /// <summary>
        /// 导入箱明细数据
        /// </summary>
        /// <param name="lstBoxDetail"></param>
        void ImportBoxDetail(List<BoxDetailModel> lstBoxDetail);

        /// <summary>
        /// 更新预调度状态为可用状态
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="DepartureID"></param>
        void UpdatePreDispatchByBoxNo(string boxNo, int DepartureID);

        /// <summary>
        /// 判断箱号是否存在于TMS系统中，存在表明同步成功
        /// </summary>
        /// <param name="boxNo"></param>
        /// <returns></returns>
        bool IsExistsBoxNo(string boxNo);

        /// <summary>
        /// 更新箱号的出库时间
        /// </summary>
        /// <param name="boxNo"></param>
        void UpdateBoxOutBoundTime(string boxNo);

    }

}
