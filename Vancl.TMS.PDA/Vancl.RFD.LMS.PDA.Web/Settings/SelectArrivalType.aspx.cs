using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vancl.TMS.PDA.Core;
using Vancl.TMS.BLL.Proxy;
using Vancl.TMS.PDA.Web.Base;

namespace Vancl.TMS.PDA.Web
{
    public partial class SelectArrivalType : PDAPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChooseFunction.aspx");
        }

        protected void btnStation_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChooseCity.aspx");
        }

        protected void btnSortCenter_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChooseArrival.aspx?type=1&param=null");
        }

        protected void btnDistribution_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChooseArrival.aspx?type=3&param=null");
        }
    }
}
