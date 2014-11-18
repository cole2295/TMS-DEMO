using System.Web.Mvc;

namespace Vancl.TMS.Web.Areas.Sorting
{
    public class SortingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Sorting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Sorting_default",
                "Sorting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
