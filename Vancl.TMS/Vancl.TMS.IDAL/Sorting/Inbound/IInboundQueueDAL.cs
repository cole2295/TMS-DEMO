using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound;

namespace Vancl.TMS.IDAL.Sorting.Inbound
{
    /// <summary>
    /// 入库队列数据层
    /// </summary>
    public interface IInboundQueueDAL
    {
        /// <summary>
        /// 取得需要待处理的分拣服务队列
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        List<InboundQueueEntityModel> GetInboundQueueList(InboundQueueJobArgModel argument);

        /// <summary>
        /// 处理计数自动增1
        /// </summary>
        /// <param name="ID"></param>
        int UpdateOpCount(long ID);

        /// <summary>
        /// 更新为已处理状态
        /// </summary>
        /// <param name="ID"></param>
        int UpdateToHandled(long ID);

        /// <summary>
        /// 更新为处理失败
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        int UpdateToError(long ID);

        /// <summary>
        /// 新增入库队列数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(InboundQueueEntityModel model);
        /// <summary>
        /// 新增入库队列数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddV2(InboundQueueEntityModel model);
    }

}
