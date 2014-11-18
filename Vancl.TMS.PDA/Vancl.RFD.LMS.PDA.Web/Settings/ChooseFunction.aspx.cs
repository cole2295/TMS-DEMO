using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vancl.TMS.PDA.Web.Base;

namespace Vancl.TMS.PDA.Web
{
    public partial class ChooseFunction : PDAPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_InBound_Click(object sender, EventArgs e)
        {
            Response.Redirect("SelectArrivalType.aspx");
        }
    }
}
