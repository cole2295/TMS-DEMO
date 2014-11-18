using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.Core.FormulaManager
{
    /// <summary>
    /// 计算公式接口
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <typeparam name="M">参数类型</typeparam>
    public interface IFormula<T, M>
    {
        /// <summary>
        /// 执行计算
        /// </summary>
        /// <param name="context">参数</param>
        /// <returns></returns>
        T Execute(M context);
    }
}
