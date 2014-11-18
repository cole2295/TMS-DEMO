using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.Sorting.Inbound.SMS;

namespace Vancl.TMS.IDAL.Sorting.Inbound
{
    /// <summary>
    /// 入库短信队列数据层
    /// </summary>
    public interface IInboundSMSQueueDAL
    {
        /// <summary>
        /// 添加入库短信队列项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(InboundSMSQueueEntityModel model);

        /// <summary>
        /// 取得需要待发送短信的入库短信队列
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        List<InboundSMSQueueEntityModel> GetInboundSMSQueue(InboundSMSQueueJobArgModel argument);

        /// <summary>
        /// 处理计数自动增1
        /// </summary>
        /// <param name="ID"></param>
        int UpdateOpCount(long ID, string ErrorInfo);

        /// <summary>
        /// 更新为已处理状态
        /// </summary>
        /// <param name="ID"></param>
        int UpdateToHandled(long ID);

        /// <summary>
        /// 更新为处理失败
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ErrorInfo"></param>
        /// <returns></returns>
        int UpdateToError(long ID, string ErrorInfo);
    }
}
