using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound.SMS;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Sorting.Inbound
{
    public class InboundSMSQueueBLL : BaseBLL, IInboundSMSQueueBLL
    {
        IInboundSMSQueueDAL dal = ServiceFactory.GetService<IInboundSMSQueueDAL>("InboundSMSQueueDAL");

        #region IInboundSMSQueueBLL 成员

        /// <summary>
        /// 添加入库短信队列项
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(InboundSMSQueueEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundSMSQueueEntityModel is null");
            return dal.Add(model);
        }

        /// <summary>
        /// 取得需要待发送短信的入库短信队列
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public List<InboundSMSQueueEntityModel> GetInboundSMSQueue(InboundSMSQueueJobArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundSMSQueueJobArgModel is null.");
            return dal.GetInboundSMSQueue(argument);
        }

        /// <summary>
        /// 处理计数自动增1
        /// </summary>
        /// <param name="ID"></param>
        public int UpdateOpCount(long ID, string ErrorInfo)
        {
            return dal.UpdateOpCount(ID, ErrorInfo);
        }

        /// <summary>
        /// 更新为已处理状态
        /// </summary>
        /// <param name="ID"></param>
        public int UpdateToHandled(long ID)
        {
            return dal.UpdateToHandled(ID);
        }

        /// <summary>
        /// 更新为处理失败
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="ErrorInfo"></param>
        /// <returns></returns>
        public int UpdateToError(long ID, string ErrorInfo)
        {
            return dal.UpdateToError(ID, ErrorInfo);
        }

        #endregion
    }
}
