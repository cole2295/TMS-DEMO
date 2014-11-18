using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vancl.TMS.IBLL.Common
{
    /// <summary>
    /// 验证码操作
    /// </summary>
    public interface IVerificationCode
    {
        /// <summary>
        /// 设置验证码
        /// </summary>
        /// <param name="code">验证码</param>
        /// <returns>唯一标识</returns>
        string SetVerificationCode(string code);

        /// <summary>
        /// 验证验证码
        /// </summary>
        /// <param name="id">唯一标识</param>
        /// <param name="code">验证码</param>
        /// <param name="clearCode">是否清除code</param>
        /// <returns>是/否 过能验证</returns>
        /// <remarks>
        /// 
        /// </remarks>
        bool VerifyCode(string id, string code,bool clearCode);
    }
}
