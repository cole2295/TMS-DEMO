using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Util.Pager;
using Vancl.TMS.Model.Sorting.Loading;
using Vancl.TMS.Model.Common;

namespace Vancl.TMS.IBLL.Sorting.Loading
{
    public interface IBillTruckBLL
    {
        PagedList<BillTruckBatchModel> GetBillTruckList(BillTruckSearchModel searchModel);

        IList<ViewBillTruckModel> GetOutbondByBatch(BillTruckSearchModel searchModel);


        /// <summary>
        /// 批量批次号列表装车
        /// </summary>
        /// <param name="batchNoList">批次号列表</param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel LoadingByBatchNoList(List<string> batchNoList, BillTruckModel model);

        /// <summary>
        /// 根据订单号列表装车
        /// </summary>
        /// <param name="formCodeList"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel LoadingByFormCodeList(List<string> formCodeList, BillTruckModel model);

        /// <summary>
        /// 订单下车
        /// </summary>
        /// <param name="formCodeList"></param>
        /// <param name="mdoel"></param>
        /// <returns></returns>
        ResultModel RemovBillTruck(List<string> formCodeList, BillTruckModel mdoel);

    }
}
