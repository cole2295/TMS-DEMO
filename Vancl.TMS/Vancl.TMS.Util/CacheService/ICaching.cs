using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Vancl.TMS.Util.CacheService
{
    /// <summary>
    /// 缓存服务
    /// </summary>
    [ServiceContract]
    public interface ICaching
    {
        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="cache"></param>
        [OperationContract]
        void Store(Cache cache);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        Cache Get(string name);

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [OperationContract]
        bool Remove(string name);

        /// <summary>
        /// 获取缓存数量
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int Count();
    }
}
