using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vancl.TMS.Model.BaseInfo;
using Vancl.TMS.Model.Common;
using Vancl.TMS.Model;
using System.Web;
using System.Web.Security;
using Vancl.TMS.Util.ConfigUtil;
using Vancl.TMS.Util.SerializationUtil;
using Vancl.TMS.Util.Security;
using Vancl.TMS.Util.JsonUtil;

namespace Vancl.TMS.Core.Security
{
    public class UserContext
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public static UserModel CurrentUser
        {
            get
            {
                return GetAuthUser();
            }
        }

        /// <summary>
        /// 代理用户
        /// </summary>
        public static UserModel AgentUser
        {
            get
            {
                return GetAgentUser();
            }
        }

        /// <summary>
        /// 获取用户对象
        /// </summary>
        /// <returns></returns>
        private static UserModel GetAuthUser()
        {
            if (HttpContext.Current == null)
            {
                return new UserModel();
            }
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket authTicket = null;
            UserModel model = null;
            try
            {
                //解密
                authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                model = authTicket.UserData.DesrializeToObject<UserModel>();
            }
            catch (Exception ex)
            {
                Logging.Log.logger.Error("AuthTicket解密异常，登陆失败", ex);

                FormsAuthentication.SignOut();
                FormsAuthentication.RedirectToLoginPage();
            }
            return model ?? new UserModel();
        }

        /// <summary>
        /// 获取代理用户对象
        /// </summary>
        /// <returns></returns>
        private static UserModel GetAgentUser()
        {
            if (HttpContext.Current == null)
            {
                return null;
            }
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(Consts.AGENT_USER_COOKIE_NAME);
            if (cookie == null)
            {
                return null;
            }
            UserModel model = null;
            try
            {
                //解密
                string value = DES.Decrypt3DES(cookie.Value);
                model = JsonHelper.ConvertToObject<UserModel>(value);
            }
            catch (Exception ex)
            {
                Logging.Log.logger.Error("AgentCookie解密异常", ex);
            }
            return model;
        }

        /// <summary>
        /// 设置代理用户Cookie
        /// </summary>
        /// <param name="agentUser"></param>
        public static void SetAgentCookie(UserModel agentUser)
        {
            if (HttpContext.Current == null)
            {
                return;
            }
            if (agentUser == null)
            {
                if (HttpContext.Current.Request.Cookies.AllKeys.Contains(Consts.AGENT_USER_COOKIE_NAME))
                {
                    HttpContext.Current.Response.Cookies[Consts.AGENT_USER_COOKIE_NAME].Expires = DateTime.Now.AddDays(-1);
                }
                return;
            }
            HttpCookie cookie = new HttpCookie(Consts.AGENT_USER_COOKIE_NAME);
            string value = DES.Encrypt3DES(JsonHelper.ConvertToJosnString(agentUser));
            cookie.Value = value;
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(Consts.AGENT_USER_COOKIE_NAME))
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 设置客户端口验证票据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userModel"></param>
        /// <param name="createPersistentCookie"></param>
        /// <param name="strCookiePath"></param>
        public static void SetAuthCookie(UserModel userModel, bool createPersistentCookie, string strCookiePath = "/")
        {
            string userName = userModel.UserCode;

            // 获得Cookie
            HttpCookie authCookie = FormsAuthentication.GetAuthCookie(userName, createPersistentCookie, strCookiePath);

            string userData = userModel.SerializeToString();

            //// 得到ticket凭据
            //FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            // 根据之前的ticket凭据创建新ticket凭据，然后加入自定义信息
            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                2, userModel.UserCode, DateTime.Now, DateTime.Now.AddDays(Consts.COOKIE_EXPIRES),
                createPersistentCookie, userData, strCookiePath);

            // 将新的Ticke转变为Cookie值，然后添加到Cookies集合中
            authCookie.Value = FormsAuthentication.Encrypt(newTicket);
            if (HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName] == null)
            {
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Set(authCookie);
            }
        }

        public static List<MerchantModel> GetPrivateMerchantList(List<MerchantModel> baseList, string userCode)
        {
            string value = ConfigurationHelper.GetAppSetting("PrivateBillPrint");
            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    foreach (var merchant in value.Split('|'))
                    {
                        if (userCode == merchant.Split('=')[0])
                        {
                            string[] split = merchant.Split('=')[1].Split(new Char[] { ',' }, StringSplitOptions.None);//返回:{"1","2","3","","4"} 保留空元素
                            var ids = Array.ConvertAll<string, long>(split, delegate(string s) { return long.Parse(s); });
                            baseList = baseList.Where(x => ids.Contains(x.ID)).ToList();
                            break;
                        }
                    }
                    return baseList;
                }
                catch (Exception)
                {
                    return baseList;
                }
            }
            return baseList;
        }
    }
}
