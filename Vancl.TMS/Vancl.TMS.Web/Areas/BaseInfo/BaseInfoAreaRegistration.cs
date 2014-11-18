using System.Web.Mvc;

namespace Vancl.TMS.Web.Areas.BaseInfo
{
    public class BaseInfoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "BaseInfo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "BaseInfo_default",
                "BaseInfo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
