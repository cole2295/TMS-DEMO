using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vancl.TMS.BLL.Proxy;

namespace Vancl.TMS.PDA.Web
{
    public partial class UserLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnLogin.ServerClick += new EventHandler(btnLogin_ServerClick);
        }

        void btnLogin_ServerClick(object sender, EventArgs e)
        {
            string uName = this.txtUserName.Value.Trim();
            string uPwd = this.txtUserPwd.Value;
            try
            {
                UserContext.Login(uName, uPwd);
                Response.Redirect("~/Settings/ChooseFunction.aspx");
            }
            catch (Exception ee)
            {
                this.divError.Attributes.Add("style", "display:block");
                this.divError.InnerText = ee.Message;
            }
        }
    }
}
