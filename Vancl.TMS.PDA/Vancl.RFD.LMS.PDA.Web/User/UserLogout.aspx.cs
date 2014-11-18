using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vancl.TMS.BLL.Proxy;

namespace Vancl.TMS.PDA.Web
{
    public partial class UserLogout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserContext.Logout();
            Response.Redirect("UserLogin.aspx");
        }
    }
}
