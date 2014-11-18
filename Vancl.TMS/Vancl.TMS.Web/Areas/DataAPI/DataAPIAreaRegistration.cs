using System.Web.Mvc;

namespace Vancl.TMS.Web.Areas.DataAPI
{
    public class DataAPIAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DataAPI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "DataAPI_default",
                "DataAPI/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
