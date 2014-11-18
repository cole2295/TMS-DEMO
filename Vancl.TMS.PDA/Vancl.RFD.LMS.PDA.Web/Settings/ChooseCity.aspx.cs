using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Vancl.TMS.BLL.Proxy;
using Vancl.TMS.PDA.Web.Base;
using Vancl.TMS.PDA.Core.Model;

namespace Vancl.TMS.PDA.Web
{
    public partial class ChooseCity : PDAPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCity();
            }
        }

        protected void BindCity()
        {
            this.ddl_City.Items.Clear();

            List<IDAndNameModel> citys = UserContext.GetCityBySortingCenter(CurrentUser.UserID);

            this.ddl_City.DataSource = citys;
            this.ddl_City.DataTextField = "Name";
            this.ddl_City.DataValueField = "ID";
            this.ddl_City.DataBind();

            this.ddl_City.SelectedIndex = 0;

            if (this.ddl_City.Items.Count == 0)
                this.btn_Next.Enabled = false;
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.ddl_City.SelectedValue))
                Response.Redirect("ChooseArrival.aspx?type=2&param=" + this.ddl_City.SelectedValue);
            else
                AlertMessage("请选择城市.");
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            Response.Redirect("SelectArrivalType.aspx");
        }
    }
}
