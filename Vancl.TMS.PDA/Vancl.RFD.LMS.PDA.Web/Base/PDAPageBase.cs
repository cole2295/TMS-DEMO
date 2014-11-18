using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Vancl.TMS.PDA.Core.Model;
using Vancl.TMS.BLL.Proxy;
using System.Web;

namespace Vancl.TMS.PDA.Web.Base
{
    public class PDAPageBase : Page
    {
        private PDAUserModel _currentUser = null;

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public PDAUserModel CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    _currentUser = UserContext.GetCurrentUserFromCookies();
                }
                return _currentUser;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //CurrentUser = UserContext.GetCurrentUserFromCookies();
            if (CurrentUser == null)
                Response.Redirect("~/User/UserLogin.aspx");
        }

        protected void AlertMessage(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) msg = "未知提示";
            string js = "<script language=\"javascript\">\n alert(\"" + msg.Trim() + "\");\n</script>";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), Guid.NewGuid().ToString(), js);
        }
    }
}
