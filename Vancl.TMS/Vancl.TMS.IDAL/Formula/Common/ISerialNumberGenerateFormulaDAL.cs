using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.IDAL.Formula.Common
{
    /// <summary>
    /// 序列号产生数据层
    /// </summary>
    public interface ISerialNumberGenerateFormulaDAL
    {
        /// <summary>
        /// 使用系统的Sequence自动参数
        /// </summary>
        /// <param name="numberType">Sequence名称</param>
        /// <returns></returns>
        string GetNextNumber(string numberType);

        /// <summary>
        /// 使用用户自定义产生流水码
        /// </summary>
        /// <param name="SeqNoType">序列号类型</param>
        /// <returns></returns>
        int GetUserDefinedSeqNo(Vancl.TMS.Model.Common.Enums.SerialNumberType SeqNoType);

    }
}
