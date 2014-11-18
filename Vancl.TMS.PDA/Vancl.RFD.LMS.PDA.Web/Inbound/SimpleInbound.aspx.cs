using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Vancl.TMS.PDA.Web.Base;
using Vancl.TMS.BLL.Proxy;
using Vancl.TMS.BLL.Proxy.TMSAPIService;
using Vancl.TMS.PDA.Core.Security;

namespace Vancl.TMS.PDA.Web
{
    public partial class SimpleInbound : PDAPageBase
    {
        /// <summary>
        /// 日志
        /// </summary>
        private ILog logger = LogManager.GetLogger("SortCenter.PDA.Inbound.V1.0.0.1");

        protected void Page_Load(object sender, EventArgs e)
        {
            InitPageView();
        }

        private void InitPageView()
        {
            try
            {
                if (Request.QueryString.Count < 2
                    || Request.QueryString["Station"] == null
                    || Request.QueryString["EnValue"] == null)
                {
                    Response.Redirect("../User/UserLogin.aspx");
                }
                ltArrivalStation.Text = Request.QueryString["Station"].ToString();
                string[] arrDecryptValue = DecryptQueryString();
                if (arrDecryptValue == null || arrDecryptValue.Length != 3)
                {
                    Response.Redirect("~/User/UserLogin.aspx");
                }

                InboundSimpleArgModel condition = CreateCondition(arrDecryptValue);

                if (!IsPostBack)
                {
                    ltInboundCount.Text = InBoundProxy.GetInboundCount(condition).ToString();
                }

                a_switchArrival.HRef = "~/Settings/ChooseArrival.aspx?type=" + Request.QueryString["type"] + "&param=" + Request.QueryString["param"];
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private string[] DecryptQueryString()
        {
            return DES.Decrypt3DES(Request.QueryString["EnValue"]).Split(';');
        }

        private InboundSimpleArgModel CreateCondition(string[] arrDecryptValue)
        {
            SortCenterToStationModel ToStation = new SortCenterToStationModel()
            {
                ExpressCompanyID = int.Parse(arrDecryptValue[0]),
                CompanyFlag = (Vancl.TMS.BLL.Proxy.TMSAPIService.EnumsCompanyFlag)int.Parse(arrDecryptValue[1]),
                DistributionCode = arrDecryptValue[2]
            };
            SortCenterUserModel opUser = InBoundProxy.GetUserModel(CurrentUser.UserID);
            InboundPreConditionModel preCondition = new InboundPreConditionModel();
            if (Request.QueryString["prestatus"] != null)
            {
                preCondition.PreStatus = (Vancl.TMS.BLL.Proxy.TMSAPIService.EnumsBillStatus)int.Parse(Request.QueryString["prestatus"]);
            }
            InboundSimpleArgModel condition = new InboundSimpleArgModel()
            {
                FormType = Vancl.TMS.BLL.Proxy.TMSAPIService.EnumsSortCenterFormType.Waybill,
                OpUser = opUser,
                ToStation = ToStation,
                PreCondition = preCondition
            };

            return condition;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                InboundSimpleArgModel condition = CreateCondition(DecryptQueryString());
                condition.FormType = (Vancl.TMS.BLL.Proxy.TMSAPIService.EnumsSortCenterFormType)int.Parse(ddlFormType.SelectedValue);
                condition.FormCode = txtCode.Text.Trim();

                ViewInboundSimpleModel result = InBoundProxy.SimpleInBound(condition);
                if (!result.IsSuccess)
                {
                    ltMsg.Text = result.Message;
                }
                else
                {
                    ltMsg.Text = string.IsNullOrEmpty(result.Message) ? "入库成功!" : result.Message;
                    ltInboundCount.Text = result.InboundCount.ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
