using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.IDAL.Synchronous
{
    public interface IInboundTMSDAL
    {
        /// <summary>
        /// 根据箱号更新是否到达状态
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="arrivalID"></param>
        /// <returns></returns>
        int UpdateIsArrivedStatusByBoxNo(string boxNo, int arrivalID);

        /// <summary>
        /// 根据单号更新分拣中心接受状态
        /// </summary>
        /// <param name="formCodes"></param>
        /// <param name="arrivalIDs"></param>
        /// <returns></returns>
        int UpdateIsArrivedStatusByFormCodes(string[] formCodes, int[] arrivalIDs);

        /// <summary>
        /// 根据箱号更新调度表
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="arrivalID"></param>
        /// <param name="receiveDate"></param>
        /// <returns></returns>
        int UpdateDispatchByBoxNo(string boxNo, int arrivalID, DateTime receiveDate);

        /// <summary>
        /// 根据单号更新调度表
        /// </summary>
        /// <param name="formCodes"></param>
        /// <param name="arrivalIDs"></param>
        /// <param name="receiveDates"></param>
        /// <returns></returns>
        int UpdateDispatchByFormCodes(string[] formCodes, int[] arrivalIDs, DateTime[] receiveDates);

        /// <summary>
        /// 根据箱号更新到库状态
        /// </summary>
        /// <param name="boxNo"></param>
        /// <param name="arrivalID"></param>
        /// <returns></returns>
        int UpdateOrderTMSStatusByBoxNo(string boxNo, int arrivalID);

        /// <summary>
        /// 根据单号更新到库状态
        /// </summary>
        /// <param name="formCodes"></param>
        /// <param name="arrivalIDs"></param>
        /// <returns></returns>
        int UpdateOrderTMSStatusByFormCodes(string[] formCodes, int[] arrivalIDs);
    }
}
