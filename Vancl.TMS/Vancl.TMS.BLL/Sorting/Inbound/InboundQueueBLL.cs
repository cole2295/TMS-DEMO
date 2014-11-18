using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Sorting.Inbound;
using Vancl.TMS.Model.Sorting.Inbound;
using Vancl.TMS.IDAL.Sorting.Inbound;
using Vancl.TMS.Core.ServiceFactory;

namespace Vancl.TMS.BLL.Sorting.Inbound
{
    public class InboundQueueBLL : BaseBLL, IInboundQueueBLL
    {
        IInboundQueueDAL InboundQueueDAL = ServiceFactory.GetService<IInboundQueueDAL>("InboundQueueDAL"); 

        #region IInboundQueueBLL 成员

        /// <summary>
        /// 取得需要待处理的分拣服务队列
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public List<InboundQueueEntityModel> GetInboundQueueList(InboundQueueJobArgModel argument)
        {
            if (argument == null) throw new ArgumentNullException("InboundQueueJobArgModel is null.");
            return InboundQueueDAL.GetInboundQueueList(argument);
        }

        /// <summary>
        /// 处理计数自动增1
        /// </summary>
        /// <param name="ID"></param>
        public int UpdateOpCount(long ID)
        {
            return InboundQueueDAL.UpdateOpCount(ID);
        }

        /// <summary>
        /// 更新为已处理状态
        /// </summary>
        /// <param name="ID"></param>
        public int UpdateToHandled(long ID)
        {
            return InboundQueueDAL.UpdateToHandled(ID);
        }

        /// <summary>
        /// 更新为处理失败
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int UpdateToError(long ID)
        {
            return InboundQueueDAL.UpdateToError(ID);
        }

        /// <summary>
        /// 新增分家服务队列数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Add(InboundQueueEntityModel model)
        {
            if (model == null) throw new ArgumentNullException("InboundQueueEntityModel is null");
            return InboundQueueDAL.Add(model);
        }

        #endregion
    }
}
