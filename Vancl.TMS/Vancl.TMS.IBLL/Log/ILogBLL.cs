using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Vancl.TMS.Model;
using Vancl.TMS.Model.Log;

namespace Vancl.TMS.IBLL.Log
{
    /// <summary>
    /// 日志读写接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILogBLL<T> where T : BaseLogModel
    {
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="model">日志模型</param>
        /// <returns></returns>
        int Write(T model);
        /// <summary>
        /// 读取日志
        /// </summary>
        /// <param name="model">日志搜索条件</param>
        /// <returns></returns>
        List<T> Read(BaseLogSearchModel searchmodel);


    }
}
