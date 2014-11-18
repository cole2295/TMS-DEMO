using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.ImportRecord;

namespace Vancl.TMS.IDAL.DeliveryImport
{
    /// <summary>
    /// 提货单入口数据接口
    /// </summary>
    public interface IDeliveryImportDAL
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
    }
}
