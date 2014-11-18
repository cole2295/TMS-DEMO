using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vancl.TMS.BLL.Proxy;
using System.Data;
using Vancl.TMS.PDA.Web.Base;
using Vancl.TMS.PDA.Core.Security;
using Vancl.TMS.BLL.Proxy.TMSAPIService;

namespace Vancl.TMS.PDA.Web
{
    public partial class ChooseArrival : PDAPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindArrival();
            }

            if (this.ddl_Arrival.Items.Count == 0)
                this.btn_Next.Enabled = false;
        }

        private void BindArrival()
        {
            if (Request.QueryString["type"] != null)
            {
                int type = int.Parse(Request.QueryString["type"]);

                switch (type)
                {
                    case 1:
                        BindSortCenters();
                        break;
                    case 2:
                        BindStatioins(Request.QueryString["param"]);
                        break;
                    case 3:
                        BindDistributions();
                        break;
                }
            }
        }

        private void BindStatioins(string cityID)
        {
            this.ddl_Arrival.Items.Clear();
            this.ddl_Arrival.DataSource = UserContext.GetStationsByCityId(cityID, CurrentUser.UserID);
            this.ddl_Arrival.DataTextField = "Name";
            this.ddl_Arrival.DataValueField = "ID";
            this.ddl_Arrival.DataBind();
            this.ddl_Arrival.SelectedIndex = 0;
        }

        private void BindSortCenters()
        {
            this.ddl_Arrival.Items.Clear();
            this.ddl_Arrival.DataSource = UserContext.GetSortCenters(CurrentUser.UserDistributeCode, CurrentUser.UserDeptID.HasValue ? CurrentUser.UserDeptID.Value : -1);
            this.ddl_Arrival.DataTextField = "CompanyName";
            this.ddl_Arrival.DataValueField = "ExpressCompanyID";
            this.ddl_Arrival.DataBind();
            this.ddl_Arrival.SelectedIndex = 0;
        }

        private void BindDistributions()
        {
            this.ddl_Arrival.Items.Clear();
            this.ddl_Arrival.DataSource = UserContext.GetDistributions(CurrentUser.UserDistributeCode);
            this.ddl_Arrival.DataTextField = "CompanyName";
            this.ddl_Arrival.DataValueField = "ExpressCompanyID";
            this.ddl_Arrival.DataBind();
            this.ddl_Arrival.SelectedIndex = 0;
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {

            if (Request.QueryString["type"] != null)
            {
                int type = int.Parse(Request.QueryString["type"]);

                switch (type)
                {
                    case 1:
                        Response.Redirect("SelectArrivalType.aspx");
                        break;
                    case 2:
                        Response.Redirect("ChooseCity.aspx");
                        break;
                    case 3:
                        Response.Redirect("SelectArrivalType.aspx");
                        break;
                }
            }
        }

        protected void btn_Next_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] != null)
            {
                int type = int.Parse(Request.QueryString["type"]);
                int companyFlag = 0;

                switch (type)
                {
                    case 1:
                        companyFlag = 1;
                        break;
                    case 2:
                        companyFlag = 2;
                        break;
                    case 3:
                        companyFlag = 3;
                        break;
                }

                string station = this.ddl_Arrival.SelectedItem.Text;
                string ditributionCode = string.Empty;

                if (type == 3)
                {
                    SortCenterToStationModel toStation = InBoundProxy.GetToStationModel(int.Parse(this.ddl_Arrival.SelectedItem.Value));
                    ditributionCode = toStation.DistributionCode;
                }
                else
                {
                    ditributionCode = CurrentUser.UserDistributeCode;
                }

                string enValue = DES.Encrypt3DES(ddl_Arrival.SelectedValue + ";" + companyFlag + ";" + ditributionCode).Replace("+", "%2B");
                int preStatus;
                InboundPreConditionModel preCondition = InBoundProxy.GetPreCondition(CurrentUser.UserDistributeCode);
                if (preCondition != null)
                    preStatus = (int)preCondition.PreStatus;
                else
                {
                    this.lit_Msg.Text = "配送商流程未设置.";
                    return;
                }
                Response.Redirect("~/Inbound/SimpleInBound.aspx?type=" + type.ToString() + "&param=" + Request.QueryString["param"] + "&Station=" + station + "&EnValue=" + enValue + "&prestatus=" + preStatus);
            }
        }
    }
}
