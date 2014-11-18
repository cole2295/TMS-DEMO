using System.Web.Mvc;

namespace Vancl.TMS.Web.Areas.SyncManager
{
    public class SyncManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SyncManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SyncManager_default",
                "SyncManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
