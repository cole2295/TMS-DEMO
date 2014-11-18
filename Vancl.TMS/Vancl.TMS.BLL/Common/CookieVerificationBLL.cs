using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.IBLL.Common;
using System.Web;
using System.Web.Security;
using Vancl.TMS.Util.Security;

namespace Vancl.TMS.BLL.Common
{
    public class CookieVerificationBLL : IVerificationCode
    {
        private const string cookieName = "CookieVerificationCode";

        #region IVerificationCode 成员

        public string SetVerificationCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("code");

            string id = Guid.NewGuid().ToString();
            //加密
            code = DES.Encrypt3DES(code);
            HttpCookie cookie = new HttpCookie(cookieName, code);
            //不设置过期时间，浏览器session cookie
            //cookie.Expires = DateTime.Now.AddMinutes(5);
            HttpContext.Current.Response.Cookies.Add(cookie);
            return id;
        }

        public bool VerifyCode(string id, string code, bool clearCode)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id");
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("code");

            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                return false;
            }
            string cookieCode = cookie.Value;
            //解密
            cookieCode = DES.Decrypt3DES(cookieCode);
            if (clearCode)
            {
                //清除cookie
                cookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Set(cookie);
            }

            return string.Compare(cookieCode, code, true) == 0;
        }

        #endregion
    }
}
