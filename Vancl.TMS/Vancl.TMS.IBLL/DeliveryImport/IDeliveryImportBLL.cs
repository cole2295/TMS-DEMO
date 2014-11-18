using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.ImportRecord;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model.Transport.PreDispatch;

namespace Vancl.TMS.IBLL.DeliveryImport
{
    public interface IDeliveryImportBLL
    {
        /// <summary>
        /// 添加提货单录入记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddRecord(DeliveryInRecordModel model);

        /// <summary>
        /// 获取记录列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<DeliveryInRecordModel> GetRecordList(DeliveryInRecordSearchModel conditions);

        /// <summary>
        /// 新增提货单
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        ResultModel AddDelivery(string[,] data, string templatePath);

        /// <summary>
        /// 取得预调度信息【扫描批次合并】
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        List<ViewPreDispatchModel> GetBatchPreDispatchInfo(PreDispatchSearchModel searchmodel);

        /// <summary>
        /// 取得预调度信息【查询批次合并】
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        List<ViewPreDispatchModel> SearchPreDispatchInfo(PreDispatchSearchModel searchmodel);

        /// <summary>
        /// 新增提货单信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ResultModel AddToDispatch(ViewDispatchWithDetailsModel model, Enums.DeliverySource source);

        /// <summary>
        /// 待调度批次查询
        /// </summary>
        /// <param name="searchmodel"></param>
        /// <returns></returns>
        List<ViewPreDispatchModel> SearchPreDispatchInfoV1(PreDispatchSearchModel searchmodel);
        /// <summary>
        /// 修改单据状态
        /// </summary>
        /// <param name="listPDID"></param>
        /// <param name="UpdateStatus"></param>
        /// <returns></returns>
        int UpdateToDisabledDispatchV1(List<long> listPDID, Enums.DispatchStatus UpdateStatus);

        List<ViewPreDispatchModel> GetBatchPreDispatchInfoV1(PreDispatchSearchModel searchModel);
    }
}
