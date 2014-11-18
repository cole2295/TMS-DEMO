using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model;

namespace Vancl.TMS.IDAL
{
    /// <summary>
    /// 数据层基础接口
    /// </summary>
    public interface ISequenceDAL
    {
        /// <summary>
        /// 取得序列值
        /// </summary>
        /// <param name="sequenceName">序列名称</param>
        /// <returns></returns>
        long GetNextSequence(string sequenceName);
    }

    public interface IDbModelDAL<TModel, TKey> where TModel : BaseDbModel<TKey>
    {
        TModel Get(TKey id);
        int Add(TModel model);
        int Update(TModel model);
        int Delete(TKey id);
    }
}
